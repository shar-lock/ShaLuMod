// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Commands.DamageCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Commands;

public static class DamageCmd
{
  public static AttackCommand Attack(Decimal damagePerHit) => new AttackCommand(damagePerHit);

  public static AttackCommand Attack(CalculatedDamageVar calculatedDamageVar)
  {
    return new AttackCommand(calculatedDamageVar);
  }
}
