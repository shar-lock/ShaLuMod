// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Cards.CardKeywordExtensions
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Cards;

internal static class CardKeywordExtensions
{
  private static readonly LocString _period = new LocString("card_keywords", "PERIOD");

  public static string GetLocKeyPrefix(this CardKeyword keyword)
  {
    return StringHelper.Slugify(keyword.ToString());
  }

  public static LocString GetTitle(this CardKeyword keyword)
  {
    return new LocString("card_keywords", keyword.GetLocKeyPrefix() + ".title");
  }

  public static LocString GetDescription(this CardKeyword keyword)
  {
    return new LocString("card_keywords", keyword.GetLocKeyPrefix() + ".description");
  }

  public static string GetCardText(this CardKeyword keyword)
  {
    return $"[gold]{keyword.GetTitle().GetFormattedText()}[/gold]{CardKeywordExtensions._period.GetRawText()}";
  }
}
