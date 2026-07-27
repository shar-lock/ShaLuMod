// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.TestSupport.TestMode
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

#nullable disable
namespace MegaCrit.Sts2.Core.TestSupport;

public static class TestMode
{
  public static bool IsOn { get; set; }

  public static bool IsOff => !TestMode.IsOn;

  public static void AssertOn()
  {
    if (!TestMode.IsOn)
      throw new TestModeOffException();
  }

  public static void AssertOff()
  {
    if (!TestMode.IsOff)
      throw new TestModeOnException();
  }

  public static void TurnOnInternal() => TestMode.IsOn = true;
}
