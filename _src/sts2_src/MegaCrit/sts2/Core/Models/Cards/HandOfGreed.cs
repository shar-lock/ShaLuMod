// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.HandOfGreed
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class HandOfGreed : CardModel
{
  public const int goldAmount = 20;
  private const string _goldKey = "Gold";

  public HandOfGreed()
    : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
  {
  }

  public override bool CanBeGeneratedInCombat => false;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new DamageVar(20M, ValueProp.Move),
        new DynamicVar("Gold", 20M)
      });
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Fatal));
    }
  }

  protected override void OnUpgrade()
  {
    this.DynamicVars.Damage.UpgradeValueBy(5M);
    this.DynamicVars["Gold"].UpgradeValueBy(5M);
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    bool shouldTriggerFatal = cardPlay.Target.Powers.All<PowerModel>((Func<PowerModel, bool>) (p => p.ShouldOwnerDeathTriggerFatal()));
    Vector2? monsterPos = new Vector2?();
    if (TestMode.IsOff)
      monsterPos = NCombatRoom.Instance.GetCreatureNode(cardPlay.Target)?.VfxSpawnPosition;
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this, cardPlay).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3").Execute(choiceContext);
    if (!shouldTriggerFatal || !attackCommand.Results.SelectMany<List<DamageResult>, DamageResult>((Func<List<DamageResult>, IEnumerable<DamageResult>>) (r => (IEnumerable<DamageResult>) r)).Any<DamageResult>((Func<DamageResult, bool>) (r => r.WasTargetKilled)))
      return;
    if (monsterPos.HasValue)
      VfxCmd.PlayVfx(monsterPos.Value, "vfx/vfx_coin_explosion_regular", NCombatRoom.Instance?.CombatVfxContainer);
    await PlayerCmd.GainGold((Decimal) this.DynamicVars["Gold"].IntValue, this.Owner);
  }
}
