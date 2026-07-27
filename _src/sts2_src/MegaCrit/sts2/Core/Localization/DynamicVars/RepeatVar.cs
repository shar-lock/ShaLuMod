// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Localization.DynamicVars.RepeatVar
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Localization.DynamicVars;

public class RepeatVar : DynamicVar
{
  public const string defaultName = "Repeat";

  public RepeatVar(int times)
    : base("Repeat", (Decimal) times)
  {
  }

  public RepeatVar(string name, int times)
    : base(name, (Decimal) times)
  {
  }
}
