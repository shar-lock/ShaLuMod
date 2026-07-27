// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.AutoSlay.AutoSlayConfig
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;

#nullable disable
namespace MegaCrit.Sts2.Core.AutoSlay;

public static class AutoSlayConfig
{
  public static readonly TimeSpan runTimeout = TimeSpan.FromMinutes(25L);
  public static readonly TimeSpan defaultRoomTimeout = TimeSpan.FromMinutes(2L);
  public static readonly TimeSpan defaultScreenTimeout = TimeSpan.FromSeconds(30L);
  public static readonly TimeSpan gameInitTimeout = TimeSpan.FromSeconds(10L);
  public static readonly TimeSpan runStateTimeout = TimeSpan.FromSeconds(30L);
  public static readonly TimeSpan nodeWaitTimeout = TimeSpan.FromSeconds(10L);
  public static readonly TimeSpan mapScreenTimeout = TimeSpan.FromSeconds(10L);
  public static readonly TimeSpan mapPointEnabledTimeout = TimeSpan.FromSeconds(30L);
  public static readonly TimeSpan pollingInterval = TimeSpan.FromMilliseconds(100L, 0L);
  public static readonly TimeSpan buttonClickDelay = TimeSpan.FromMilliseconds(100L, 0L);
  public const int maxFloor = 49;
  public const int combatStrengthRampStartTurn = 3;
  public const int combatStrengthRampPerTurn = 200;
  public static readonly TimeSpan watchdogTimeout = TimeSpan.FromSeconds(30L);
  public static readonly TimeSpan watchdogLogInterval = TimeSpan.FromSeconds(5L);
}
