// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Potions.PotionOfBinding
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Potions;

public sealed class PotionOfBinding : PotionModel
{
  public override PotionRarity Rarity => PotionRarity.Uncommon;

  public override PotionUsage Usage => PotionUsage.CombatOnly;

  public override TargetType TargetType => TargetType.AllEnemies;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new PowerVar<VulnerablePower>(1M),
        (DynamicVar) new PowerVar<WeakPower>(1M)
      });
    }
  }

  public override IEnumerable<IHoverTip> ExtraHoverTips
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

  protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
  {
    IReadOnlyList<Creature> targets = this.Owner.Creature.CombatState.HittableEnemies;
    foreach (Creature target1 in (IEnumerable<Creature>) targets)
    {
      NCombatRoom instance = NCombatRoom.Instance;
      if (instance != null)
        ((Node) instance.CombatVfxContainer).AddChildSafely((Node) NSmokePuffVfx.Create(target1, NSmokePuffVfx.SmokePuffColor.Green));
    }
    IReadOnlyList<WeakPower> weakPowerList = await PowerCmd.Apply<WeakPower>(choiceContext, (IEnumerable<Creature>) targets, (Decimal) this.DynamicVars["VulnerablePower"].IntValue, this.Owner.Creature, (CardModel) null);
    IReadOnlyList<VulnerablePower> vulnerablePowerList = await PowerCmd.Apply<VulnerablePower>(choiceContext, (IEnumerable<Creature>) targets, (Decimal) this.DynamicVars["WeakPower"].IntValue, this.Owner.Creature, (CardModel) null);
    targets = (IReadOnlyList<Creature>) null;
  }
}
