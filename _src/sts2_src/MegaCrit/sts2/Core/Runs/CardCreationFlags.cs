// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Runs.CardCreationFlags
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;

#nullable disable
namespace MegaCrit.Sts2.Core.Runs;

[Flags]
public enum CardCreationFlags
{
  NoRarityModification = 1,
  NoUpgradeRoll = 2,
  NoHookUpgrades = 4,
  NoModifyHooks = 8,
  NoCardPoolModifications = 16, // 0x00000010
  NoCardModelModifications = 32, // 0x00000020
  ForceRarityOddsChange = 64, // 0x00000040
  IsCardReward = 128, // 0x00000080
  IsFromCombat = 256, // 0x00000100
  NoUpgrades = NoHookUpgrades | NoUpgradeRoll, // 0x00000006
  NoModifications = -1, // 0xFFFFFFFF
}
