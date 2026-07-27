// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Debug.DebugSettings
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;

#nullable disable
namespace MegaCrit.Sts2.Core.Debug;

public static class DebugSettings
{
  public static bool DevSkip { get; } = Environment.GetEnvironmentVariable("STS2_DEV_SKIP") != null;

  public static bool IgnorePackedImages => false;
}
