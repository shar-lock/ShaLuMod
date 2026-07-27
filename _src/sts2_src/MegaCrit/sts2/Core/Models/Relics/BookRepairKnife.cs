// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.BookRepairKnife
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class BookRepairKnife : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Uncommon;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DoomPower>());
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new HealVar(3M));
    }
  }

  public override Task AfterDiedToDoom(
    PlayerChoiceContext choiceContext,
    IReadOnlyList<Creature> creatures)
  {
    int num = creatures.Count<Creature>((Func<Creature, bool>) (c => c != this.Owner.Creature && c.Powers.All<PowerModel>((Func<PowerModel, bool>) (p => p.ShouldOwnerDeathTriggerFatal()))));
    if (num == 0)
      return Task.CompletedTask;
    this.Flash();
    return CreatureCmd.Heal(this.Owner.Creature, this.DynamicVars.Heal.BaseValue * (Decimal) num);
  }
}
