// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Localization.Fonts.FontManager
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Localization.Fonts.FontPathSets;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Localization.Fonts;

public static class FontManager
{
  private static readonly FontPathSet _russian = (FontPathSet) new RusFontPathSet();
  private static readonly IReadOnlyDictionary<string, FontPathSet> _languageFontPathSets = (IReadOnlyDictionary<string, FontPathSet>) new Dictionary<string, FontPathSet>()
  {
    ["jpn"] = (FontPathSet) new JpnFontPathSet(),
    ["kor"] = (FontPathSet) new KorFontPathSet(),
    ["pol"] = FontManager._russian,
    ["rus"] = FontManager._russian,
    ["tha"] = (FontPathSet) new ThaFontPathSet(),
    ["zhs"] = (FontPathSet) new ZhsFontPathSet(),
    ["zht"] = (FontPathSet) new ZhtFontPathSet()
  };
  private static readonly Dictionary<string, Dictionary<FontType, Font>> _localeFonts = new Dictionary<string, Dictionary<FontType, Font>>();

  public static bool NeedsFontSubstitution(string language)
  {
    return FontManager._languageFontPathSets.ContainsKey(language);
  }

  public static Font? GetSubstituteFont(string language, FontType type)
  {
    return !FontManager.NeedsFontSubstitution(language) ? (Font) null : FontManager.GetFontForLanguage(language, type);
  }

  public static void ClearCache() => FontManager._localeFonts.Clear();

  private static Font? GetFontForLanguage(string language, FontType type)
  {
    Dictionary<FontType, Font> dictionary1;
    Font fontForLanguage1;
    if (FontManager._localeFonts.TryGetValue(language, out dictionary1) && dictionary1.TryGetValue(type, out fontForLanguage1))
      return fontForLanguage1;
    string path = FontManager._languageFontPathSets[language].GetPath(type);
    if (path == null)
      return (Font) null;
    Font fontForLanguage2 = ResourceLoader.Load<Font>(path, (string) null, (ResourceLoader.CacheMode) 1L);
    Dictionary<FontType, Font> dictionary2;
    if (!FontManager._localeFonts.TryGetValue(language, out dictionary2))
    {
      dictionary2 = new Dictionary<FontType, Font>();
      FontManager._localeFonts[language] = dictionary2;
    }
    dictionary2[type] = fontForLanguage2;
    return fontForLanguage2;
  }
}
