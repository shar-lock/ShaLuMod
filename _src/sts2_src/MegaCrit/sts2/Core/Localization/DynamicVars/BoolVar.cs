// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Localization.DynamicVars.BoolVar
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Localization.DynamicVars;

public class BoolVar : DynamicVar
{
  public bool BoolVal
  {
    get => Convert.ToBoolean(this.BaseValue);
    set => this.BaseValue = Convert.ToDecimal(value);
  }

  public BoolVar(string name)
    : base(name, 0M)
  {
  }

  public BoolVar(string name, bool value)
    : base(name, Convert.ToDecimal(value))
  {
  }
}
