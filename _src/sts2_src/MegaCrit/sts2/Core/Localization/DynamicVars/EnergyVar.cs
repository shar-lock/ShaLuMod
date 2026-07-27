// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Localization.DynamicVars.EnergyVar
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Localization.DynamicVars;

public class EnergyVar(string name, int energy) : DynamicVar(name, (Decimal) energy)
{
  public const string defaultName = "Energy";

  public string ColorPrefix { get; set; } = string.Empty;

  public EnergyVar(int energy)
    : this("Energy", energy)
  {
  }
}
