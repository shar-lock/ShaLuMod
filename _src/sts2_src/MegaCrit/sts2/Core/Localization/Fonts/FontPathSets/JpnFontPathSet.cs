// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Localization.Fonts.FontPathSets.JpnFontPathSet
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Localization.Fonts.FontPathSets;

public class JpnFontPathSet : FontPathSet
{
  private const string _regular = "res://themes/fonts/jpn/noto_sans_cjkjp_regular_shared.tres";
  private const string _bold = "res://themes/fonts/jpn/noto_sans_cjkjp_bold_shared.tres";
  private const string _italic = "res://themes/fonts/jpn/noto_sans_cjkjp_medium_shared.tres";

  public override string GetPath(FontType type)
  {
    switch (type)
    {
      case FontType.Regular:
        return "res://themes/fonts/jpn/noto_sans_cjkjp_regular_shared.tres";
      case FontType.Bold:
        return "res://themes/fonts/jpn/noto_sans_cjkjp_bold_shared.tres";
      case FontType.Italic:
        return "res://themes/fonts/jpn/noto_sans_cjkjp_medium_shared.tres";
      default:
        throw new ArgumentOutOfRangeException(nameof (type), (object) type, (string) null);
    }
  }
}
