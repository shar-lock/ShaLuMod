// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Helpers.GlyphConverter
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;

#nullable enable
namespace MegaCrit.Sts2.Core.Helpers;

public static class GlyphConverter
{
  private static TextServer GetTextServer() => TextServerManager.Singleton.GetPrimaryInterface();

  public static uint CharToGlyphIdx(Rid font, char c)
  {
    return (uint) GlyphConverter.GetTextServer().FontGetGlyphIndex(font, 1L, (long) c, 0L);
  }

  public static char GlyphIdxToChar(CharFXTransform charFx)
  {
    return (char) GlyphConverter.GetTextServer().FontGetCharFromGlyphIndex(charFx.Font, 1L, (long) charFx.GlyphIndex);
  }
}
