// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.BeatIntoShape
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class BeatIntoShape : CardModel
{
  private const string _calculatedForgeKey = "CalculatedForge";

  public BeatIntoShape()
    : base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
  {
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[4]
      {
        (DynamicVar) new DamageVar(5M, ValueProp.Move),
        (DynamicVar) new CalculationBaseVar(5M),
        (DynamicVar) new CalculationExtraVar(5M),
        (DynamicVar) new CalculatedVar("CalculatedForge").WithMultiplier((Func<CardModel, Creature, Decimal>) ((card, target) => (Decimal) CombatManager.Instance.History.Entries.OfType<DamageReceivedEntry>().Count<DamageReceivedEntry>((Func<DamageReceivedEntry, bool>) (e => e.Receiver == target && e.Dealer == card.Owner.Creature && e.Result.Props.IsPoweredAttack() && e.HappenedThisTurn(card.CombatState)))))
      });
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromForge();

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this, cardPlay).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3").Execute(choiceContext);
    IEnumerable<SovereignBlade> sovereignBlades = await ForgeCmd.Forge(((CalculatedVar) this.DynamicVars["CalculatedForge"]).Calculate(cardPlay.Target) - (Decimal) attackCommand.Results.Count<List<DamageResult>>() * this.DynamicVars.CalculationExtra.BaseValue, this.Owner, (AbstractModel) this);
  }

  protected override void OnUpgrade()
  {
    this.DynamicVars.Damage.UpgradeValueBy(2M);
    this.DynamicVars.CalculationBase.UpgradeValueBy(2M);
    this.DynamicVars.CalculationExtra.UpgradeValueBy(2M);
  }
}
