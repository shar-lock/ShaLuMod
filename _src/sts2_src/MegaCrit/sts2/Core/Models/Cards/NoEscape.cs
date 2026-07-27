// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.NoEscape
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class NoEscape : CardModel
{
  private const string _calculatedDoomKey = "CalculatedDoom";
  private const string _doomThresholdKey = "DoomThreshold";

  public NoEscape()
    : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
  {
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[4]
      {
        new DynamicVar("DoomThreshold", 10M),
        (DynamicVar) new CalculationBaseVar(10M),
        (DynamicVar) new CalculationExtraVar(5M),
        (DynamicVar) new CalculatedVar("CalculatedDoom").WithMultiplier((Func<CardModel, Creature, Decimal>) ((card, target) => Math.Floor((Decimal) (target != null ? target.GetPowerAmount<DoomPower>() : 0) / card.DynamicVars["DoomThreshold"].BaseValue)))
      });
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DoomPower>());
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
    DoomPower doomPower = await PowerCmd.Apply<DoomPower>(choiceContext, cardPlay.Target, ((CalculatedVar) this.DynamicVars["CalculatedDoom"]).Calculate(cardPlay.Target), this.Owner.Creature, (CardModel) this);
  }

  protected override void OnUpgrade() => this.DynamicVars.CalculationBase.UpgradeValueBy(5M);
}
