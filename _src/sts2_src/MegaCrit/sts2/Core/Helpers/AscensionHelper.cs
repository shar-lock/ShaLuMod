// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Helpers.AscensionHelper
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Helpers;

public static class AscensionHelper
{
  public static int GetValueIfAscension(
    AscensionLevel level,
    int ascensionValue,
    int fallbackValue)
  {
    return !RunManager.Instance.HasAscension(level) ? fallbackValue : ascensionValue;
  }

  public static float GetValueIfAscension(
    AscensionLevel level,
    float ascensionValue,
    float fallbackValue)
  {
    return !RunManager.Instance.HasAscension(level) ? fallbackValue : ascensionValue;
  }

  public static Decimal GetValueIfAscension(
    AscensionLevel level,
    Decimal ascensionValue,
    Decimal fallbackValue)
  {
    return !RunManager.Instance.HasAscension(level) ? fallbackValue : ascensionValue;
  }

  public static bool HasAscension(AscensionLevel level) => RunManager.Instance.HasAscension(level);

  public static LocString GetTitle(int level)
  {
    return new LocString("ascension", $"LEVEL_{AscensionHelper.GetKey(level)}.title");
  }

  public static LocString GetDescription(int level)
  {
    return new LocString("ascension", $"LEVEL_{AscensionHelper.GetKey(level)}.description");
  }

  private static string GetKey(int level) => level.ToString("D2");

  public static HoverTip GetHoverTip(CharacterModel character, int level, bool achievementsLocked)
  {
    LocString title;
    if (level > 0)
    {
      title = new LocString("ascension", "PORTRAIT_TITLE");
      title.Add(nameof (character), character.Title);
      title.Add("ascension", (Decimal) level);
    }
    else
    {
      title = new LocString("ascension", "PORTRAIT_TITLE_NO_ASCENSION");
      title.Add(nameof (character), character.Title);
    }
    LocString description = new LocString("ascension", "PORTRAIT_DESCRIPTION");
    List<string> variable = new List<string>();
    for (int level1 = 1; level1 <= level; ++level1)
      variable.Add(AscensionHelper.GetTitle(level1).GetFormattedText());
    description.Add("ascensions", (IList<string>) variable);
    if (!achievementsLocked)
      return new HoverTip(title, description);
    if (level == 0)
    {
      LocString locString = new LocString("gameplay_ui", "ACHIEVEMENTS_LOCKED");
      return new HoverTip(title, description.GetFormattedText() + locString.GetFormattedText());
    }
    LocString locString1 = new LocString("gameplay_ui", "ACHIEVEMENTS_LOCKED");
    return new HoverTip(title, $"{description.GetFormattedText()}\n{locString1.GetFormattedText()}");
  }

  public static double PovertyAscensionGoldMultiplier => 0.75;
}
