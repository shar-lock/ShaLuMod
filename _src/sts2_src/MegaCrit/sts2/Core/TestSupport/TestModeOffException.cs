// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.TestSupport.TestModeOffException
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;

#nullable disable
namespace MegaCrit.Sts2.Core.TestSupport;

public class TestModeOffException : Exception
{
  public TestModeOffException()
    : base("Only call this in test mode.")
  {
  }
}
