// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.SevenStars
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class SevenStars : CardModel
{
  public SevenStars()
    : base(2, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
  {
  }

  public override int CanonicalStarCost => 7;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new DamageVar(7M, ValueProp.Move),
        (DynamicVar) new RepeatVar(7)
      });
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).WithHitCount(this.DynamicVars.Repeat.IntValue).FromCard((CardModel) this, cardPlay).TargetingAllOpponents(this.CombatState).WithHitFx("vfx/vfx_starry_impact", tmpSfx: "slash_attack.mp3").SpawningHitVfxOnEachCreature().Execute(choiceContext);
  }

  protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
}
