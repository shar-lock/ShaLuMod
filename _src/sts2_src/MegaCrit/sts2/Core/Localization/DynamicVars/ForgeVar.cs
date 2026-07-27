// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Localization.DynamicVars.ForgeVar
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Localization.DynamicVars;

public class ForgeVar(string name, int forge) : DynamicVar(name, (Decimal) forge)
{
  public const string defaultName = "Forge";

  public ForgeVar(int forge)
    : this("Forge", forge)
  {
  }
}
