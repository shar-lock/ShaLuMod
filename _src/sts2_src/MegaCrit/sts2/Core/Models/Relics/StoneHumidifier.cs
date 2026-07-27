// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.StoneHumidifier
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class StoneHumidifier : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Ancient;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new MaxHpVar(5M));
    }
  }

  public override async Task AfterRestSiteHeal(Player player, bool isMimicked)
  {
    if (player != this.Owner)
      return;
    this.Flash();
    await CreatureCmd.GainMaxHp(this.Owner.Creature, this.DynamicVars.MaxHp.BaseValue);
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
}
