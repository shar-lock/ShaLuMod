// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Combat.CombatSideExtensions
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;

#nullable disable
namespace MegaCrit.Sts2.Core.Combat;

internal static class CombatSideExtensions
{
  public static CombatSide GetOppositeSide(this CombatSide side)
  {
    switch (side)
    {
      case CombatSide.None:
        return CombatSide.None;
      case CombatSide.Player:
        return CombatSide.Enemy;
      case CombatSide.Enemy:
        return CombatSide.Player;
      default:
        throw new ArgumentOutOfRangeException(nameof (side), (object) side, (string) null);
    }
  }
}
