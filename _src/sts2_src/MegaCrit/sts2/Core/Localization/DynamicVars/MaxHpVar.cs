// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Localization.DynamicVars.MaxHpVar
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Localization.DynamicVars;

public class MaxHpVar : DynamicVar
{
  public const string defaultName = "MaxHp";

  public MaxHpVar(Decimal maxHp)
    : base("MaxHp", maxHp)
  {
  }

  public MaxHpVar(string name, Decimal maxHp)
    : base(name, maxHp)
  {
  }
}
