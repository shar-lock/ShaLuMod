// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Bloodletting
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Bloodletting : CardModel
{
  public Bloodletting()
    : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
  {
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new HpLossVar(3M),
        (DynamicVar) new EnergyVar(2)
      });
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(this.EnergyHoverTip);
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
    VfxCmd.PlayOnCreatureCenter(this.Owner.Creature, "vfx/vfx_bloody_impact");
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, this.Owner.Creature, this.DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, (CardModel) this, cardPlay);
    await PlayerCmd.GainEnergy(this.DynamicVars.Energy.BaseValue, this.Owner);
  }

  protected override void OnUpgrade() => this.DynamicVars.Energy.UpgradeValueBy(1M);
}
