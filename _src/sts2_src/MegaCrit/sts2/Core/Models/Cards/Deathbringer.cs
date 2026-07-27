// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Deathbringer
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
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Deathbringer : CardModel
{
  public Deathbringer()
    : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies)
  {
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new PowerVar<DoomPower>(21M),
        (DynamicVar) new PowerVar<WeakPower>(1M)
      });
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlyArray<IHoverTip>(new IHoverTip[2]
      {
        HoverTipFactory.FromPower<DoomPower>(),
        HoverTipFactory.FromPower<WeakPower>()
      });
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
    IReadOnlyList<DoomPower> doomPowerList = await PowerCmd.Apply<DoomPower>(choiceContext, (IEnumerable<Creature>) this.CombatState?.HittableEnemies, this.DynamicVars.Doom.BaseValue, this.Owner.Creature, (CardModel) this);
    IReadOnlyList<WeakPower> weakPowerList = await PowerCmd.Apply<WeakPower>(choiceContext, (IEnumerable<Creature>) this.CombatState?.HittableEnemies, this.DynamicVars.Weak.BaseValue, this.Owner.Creature, (CardModel) this);
  }

  protected override void OnUpgrade() => this.DynamicVars.Doom.UpgradeValueBy(5M);
}
