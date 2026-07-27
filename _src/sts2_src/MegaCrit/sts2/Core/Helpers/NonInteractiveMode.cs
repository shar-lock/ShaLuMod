// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Helpers.NonInteractiveMode
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.TestSupport;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Helpers;

public static class NonInteractiveMode
{
  public static Func<bool> AutoSlayerCheck { get; set; } = (Func<bool>) (() => false);

  public static bool IsActive => TestMode.IsOn || NonInteractiveMode.AutoSlayerCheck();
}
