// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Breakthrough
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Breakthrough : CardModel
{
  public Breakthrough()
    : base(1, CardType.Attack, CardRarity.Common, TargetType.AllEnemies)
  {
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new DamageVar(9M, ValueProp.Move),
        (DynamicVar) new HpLossVar(1M)
      });
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    VfxCmd.PlayOnCreatureCenter(this.Owner.Creature, "vfx/vfx_bloody_impact");
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, this.Owner.Creature, this.DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, (CardModel) this, cardPlay);
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this, cardPlay).TargetingAllOpponents(this.CombatState).WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "heavy_attack.mp3").Execute(choiceContext);
  }

  protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(4M);
}
