// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.RegalPillow
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class RegalPillow : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Common;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new HealVar(15M));
    }
  }

  public override Decimal ModifyRestSiteHealAmount(Creature creature, Decimal amount)
  {
    return creature.Player != this.Owner && creature.PetOwner != this.Owner ? amount : amount + this.DynamicVars.Heal.BaseValue;
  }

  public override Task AfterRestSiteHeal(Player player, bool isMimicked)
  {
    if (player != this.Owner)
      return Task.CompletedTask;
    this.Flash();
    this.Status = RelicStatus.Normal;
    return Task.CompletedTask;
  }

  public override IReadOnlyList<LocString> ModifyExtraRestSiteHealText(
    Player player,
    IReadOnlyList<LocString> currentExtraText)
  {
    if (!LocalContext.IsMe(this.Owner))
      return currentExtraText;
    IReadOnlyList<LocString> locStringList = currentExtraText;
    int index = 0;
    LocString[] items = new LocString[1 + locStringList.Count];
    foreach (LocString locString in (IEnumerable<LocString>) locStringList)
    {
      items[index] = locString;
      ++index;
    }
    items[index] = this.AdditionalRestSiteHealText;
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<LocString>) new \u003C\u003Ez__ReadOnlyArray<LocString>(items);
  }

  public override Task AfterRoomEntered(AbstractRoom room)
  {
    this.Status = room is RestSiteRoom ? RelicStatus.Active : RelicStatus.Normal;
    return Task.CompletedTask;
  }
}
