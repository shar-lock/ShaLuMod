// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Platform.SupportedWindowModeExtensions
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

#nullable disable
namespace MegaCrit.Sts2.Core.Platform;

public static class SupportedWindowModeExtensions
{
  public static bool ShouldForceFullscreen(this SupportedWindowMode mode)
  {
    return mode == SupportedWindowMode.FullscreenOnly || mode == SupportedWindowMode.FullscreenOnlyDisplayToggle;
  }
}
