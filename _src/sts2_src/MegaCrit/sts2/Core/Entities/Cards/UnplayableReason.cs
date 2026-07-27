// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Cards.UnplayableReason
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;

#nullable disable
namespace MegaCrit.Sts2.Core.Entities.Cards;

[Flags]
public enum UnplayableReason
{
  None = 0,
  HasUnplayableKeyword = 2,
  BlockedByHook = 4,
  BlockedByCardLogic = 8,
  EnergyCostTooHigh = 16, // 0x00000010
  StarCostTooHigh = 32, // 0x00000020
  NoLivingAllies = 64, // 0x00000040
}
