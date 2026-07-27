// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Localization.Fonts.FontPathSets.RusFontPathSet
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Localization.Fonts.FontPathSets;

public class RusFontPathSet : FontPathSet
{
  private const string _regular = "res://themes/fonts/rus/fira_sans_extra_condensed_regular_shared.tres";
  private const string _bold = "res://themes/fonts/rus/fira_sans_extra_condensed_bold_shared.tres";
  private const string _italic = "res://themes/fonts/rus/fira_sans_extra_condensed_italic_shared.tres";

  public override string GetPath(FontType type)
  {
    switch (type)
    {
      case FontType.Regular:
        return "res://themes/fonts/rus/fira_sans_extra_condensed_regular_shared.tres";
      case FontType.Bold:
        return "res://themes/fonts/rus/fira_sans_extra_condensed_bold_shared.tres";
      case FontType.Italic:
        return "res://themes/fonts/rus/fira_sans_extra_condensed_italic_shared.tres";
      default:
        throw new ArgumentOutOfRangeException(nameof (type), (object) type, (string) null);
    }
  }
}
