// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Modifiers.NightTerrors
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Modifiers;

public class NightTerrors : ModifierModel
{
  private const int _maxHpLoss = 5;

  public override Decimal ModifyRestSiteHealAmount(Creature creature, Decimal amount)
  {
    return (Decimal) creature.MaxHp;
  }

  public override async Task AfterRestSiteHeal(Player player, bool isMimicked)
  {
    if (isMimicked)
      return;
    await CreatureCmd.LoseMaxHp((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), player.Creature, (Decimal) Mathf.Min(5, player.Creature.MaxHp - 1), false);
  }

  public override IReadOnlyList<LocString> ModifyExtraRestSiteHealText(
    Player player,
    IReadOnlyList<LocString> currentExtraText)
  {
    LocString restSiteHealText = this.AdditionalRestSiteHealText;
    restSiteHealText.Add("Heal", (Decimal) player.Creature.MaxHp);
    restSiteHealText.Add("MaxHpLoss", 5M);
    IReadOnlyList<LocString> locStringList = currentExtraText;
    int index = 0;
    LocString[] items = new LocString[1 + locStringList.Count];
    foreach (LocString locString in (IEnumerable<LocString>) locStringList)
    {
      items[index] = locString;
      ++index;
    }
    items[index] = restSiteHealText;
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<LocString>) new \u003C\u003Ez__ReadOnlyArray<LocString>(items);
  }
}
