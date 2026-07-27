// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Localization.Fonts.FontControlUtils
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.TestSupport;

#nullable enable
namespace MegaCrit.Sts2.Core.Localization.Fonts;

public static class FontControlUtils
{
  public static void ApplyLocaleFontSubstitution(
    this Control control,
    FontType fontType,
    StringName themeFontName)
  {
    if (Engine.IsEditorHint() || TestMode.IsOn || LocManager.Instance == null || !FontManager.NeedsFontSubstitution(LocManager.Instance.Language))
      return;
    Font substituteFont = FontManager.GetSubstituteFont(LocManager.Instance.Language, fontType);
    if (substituteFont == null)
      return;
    control.AddThemeFontOverride(themeFontName, substituteFont);
  }
}
