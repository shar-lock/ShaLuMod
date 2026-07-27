// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Localization.DynamicVars.StringVar
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

#nullable enable
namespace MegaCrit.Sts2.Core.Localization.DynamicVars;

public class StringVar : DynamicVar
{
  public string StringValue { get; set; }

  public StringVar(string name, string baseValue = "")
    : base(name, 0M)
  {
    this.StringValue = baseValue;
  }

  public override string ToString() => this.StringValue;
}
