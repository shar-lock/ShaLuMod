// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.HeavenlyDrill
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class HeavenlyDrill : CardModel
{
  public HeavenlyDrill()
    : base(0, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
  {
  }

  protected override bool HasEnergyCostX => true;

  protected override bool ShouldGlowGoldInternal
  {
    get => this.Owner.PlayerCombatState.Energy >= this.DynamicVars.Energy.IntValue;
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new DamageVar(8M, ValueProp.Move),
        (DynamicVar) new EnergyVar(4)
      });
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    int hitCount = this.ResolveEnergyXValue();
    if (hitCount >= this.DynamicVars.Energy.IntValue)
      hitCount *= 2;
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).WithHitCount(hitCount).FromCard((CardModel) this, cardPlay).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_giant_horizontal_slash", tmpSfx: "slash_attack.mp3").Execute(choiceContext);
  }

  protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(2M);
}
