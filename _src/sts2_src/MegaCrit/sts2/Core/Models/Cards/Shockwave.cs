// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Shockwave
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

public sealed class Shockwave : CardModel
{
  private const string _powerKey = "Power";

  public Shockwave()
    : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies)
  {
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Power", 3M));
    }
  }

  public override IEnumerable<CardKeyword> CanonicalKeywords
  {
    get
    {
      return (IEnumerable<CardKeyword>) new \u003C\u003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlyArray<IHoverTip>(new IHoverTip[2]
      {
        HoverTipFactory.FromPower<WeakPower>(),
        HoverTipFactory.FromPower<VulnerablePower>()
      });
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
    VfxCmd.PlayOnCreatureCenter(this.Owner.Creature, "vfx/vfx_flying_slash");
    int amount = this.DynamicVars["Power"].IntValue;
    foreach (Creature enemy in (IEnumerable<Creature>) this.CombatState.HittableEnemies)
    {
      WeakPower weakPower = await PowerCmd.Apply<WeakPower>(choiceContext, enemy, (Decimal) amount, this.Owner.Creature, (CardModel) this);
      VulnerablePower vulnerablePower = await PowerCmd.Apply<VulnerablePower>(choiceContext, enemy, (Decimal) amount, this.Owner.Creature, (CardModel) this);
    }
  }

  protected override void OnUpgrade() => this.DynamicVars["Power"].UpgradeValueBy(2M);
}
