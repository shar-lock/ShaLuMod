// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.ConsoleCommands.AbstractConsoleCmdSubtypes
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#nullable disable
namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;

public static class AbstractConsoleCmdSubtypes
{
  [DynamicallyAccessedMembers]
  private static readonly Type _t0 = typeof (AchievementConsoleCmd);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1 = typeof (ActConsoleCmd);
  [DynamicallyAccessedMembers]
  private static readonly Type _t2 = typeof (AfflictConsoleCmd);
  [DynamicallyAccessedMembers]
  private static readonly Type _t3 = typeof (AncientConsoleCmd);
  [DynamicallyAccessedMembers]
  private static readonly Type _t4 = typeof (ApplyPowerConsoleCmd);
  [DynamicallyAccessedMembers]
  private static readonly Type _t5 = typeof (ArtConsoleCmd);
  [DynamicallyAccessedMembers]
  private static readonly Type _t6 = typeof (BestiaryConsoleCmd);
  [DynamicallyAccessedMembers]
  private static readonly Type _t7 = typeof (BlockConsoleCmd);
  [DynamicallyAccessedMembers]
  private static readonly Type _t8 = typeof (CardConsoleCmd);
  [DynamicallyAccessedMembers]
  private static readonly Type _t9 = typeof (CloudConsoleCmd);
  [DynamicallyAccessedMembers]
  private static readonly Type _t10 = typeof (DamageConsoleCmd);
  [DynamicallyAccessedMembers]
  private static readonly Type _t11 = typeof (DieConsoleCmd);
  [DynamicallyAccessedMembers]
  private static readonly Type _t12 = typeof (DrawConsoleCmd);
  [DynamicallyAccessedMembers]
  private static readonly Type _t13 = typeof (DumpConsoleCmd);
  [DynamicallyAccessedMembers]
  private static readonly Type _t14 = typeof (EnchantConsoleCmd);
  [DynamicallyAccessedMembers]
  private static readonly Type _t15 = typeof (EnergyConsoleCmd);
  [DynamicallyAccessedMembers]
  private static readonly Type _t16 = typeof (EventConsoleCmd);
  [DynamicallyAccessedMembers]
  private static readonly Type _t17 = typeof (FightConsoleCmd);
  [DynamicallyAccessedMembers]
  private static readonly Type _t18 = typeof (GetLogsConsoleCmd);
  [DynamicallyAccessedMembers]
  private static readonly Type _t19 = typeof (GodModeConsoleCmd);
  [DynamicallyAccessedMembers]
  private static readonly Type _t20 = typeof (GoldConsoleCmd);
  [DynamicallyAccessedMembers]
  private static readonly Type _t21 = typeof (HealConsoleCmd);
  [DynamicallyAccessedMembers]
  private static readonly Type _t22 = typeof (InstantConsoleCmd);
  [DynamicallyAccessedMembers]
  private static readonly Type _t23 = typeof (KillConsoleCmd);
  [DynamicallyAccessedMembers]
  private static readonly Type _t24 = typeof (LeaderboardConsoleCmd);
  [DynamicallyAccessedMembers]
  private static readonly Type _t25 = typeof (LogConsoleCmd);
  [DynamicallyAccessedMembers]
  private static readonly Type _t26 = typeof (MultiplayerConsoleCmd);
  [DynamicallyAccessedMembers]
  private static readonly Type _t27 = typeof (OpenConsoleCmd);
  [DynamicallyAccessedMembers]
  private static readonly Type _t28 = typeof (PotionConsoleCmd);
  [DynamicallyAccessedMembers]
  private static readonly Type _t29 = typeof (RelicConsoleCmd);
  [DynamicallyAccessedMembers]
  private static readonly Type _t30 = typeof (RemoveCardConsoleCmd);
  [DynamicallyAccessedMembers]
  private static readonly Type _t31 = typeof (RoomConsoleCmd);
  [DynamicallyAccessedMembers]
  private static readonly Type _t32 = typeof (SentryConsoleCmd);
  [DynamicallyAccessedMembers]
  private static readonly Type _t33 = typeof (StarsConsoleCmd);
  [DynamicallyAccessedMembers]
  private static readonly Type _t34 = typeof (TrailerConsoleCmd);
  [DynamicallyAccessedMembers]
  private static readonly Type _t35 = typeof (TravelConsoleCmd);
  [DynamicallyAccessedMembers]
  private static readonly Type _t36 = typeof (UnlockConsoleCmd);
  [DynamicallyAccessedMembers]
  private static readonly Type _t37 = typeof (UpgradeCardConsoleCmd);
  [DynamicallyAccessedMembers]
  private static readonly Type _t38 = typeof (WinConsoleCmd);
  private static readonly Type[] _subtypes = new Type[39]
  {
    AbstractConsoleCmdSubtypes._t0,
    AbstractConsoleCmdSubtypes._t1,
    AbstractConsoleCmdSubtypes._t2,
    AbstractConsoleCmdSubtypes._t3,
    AbstractConsoleCmdSubtypes._t4,
    AbstractConsoleCmdSubtypes._t5,
    AbstractConsoleCmdSubtypes._t6,
    AbstractConsoleCmdSubtypes._t7,
    AbstractConsoleCmdSubtypes._t8,
    AbstractConsoleCmdSubtypes._t9,
    AbstractConsoleCmdSubtypes._t10,
    AbstractConsoleCmdSubtypes._t11,
    AbstractConsoleCmdSubtypes._t12,
    AbstractConsoleCmdSubtypes._t13,
    AbstractConsoleCmdSubtypes._t14,
    AbstractConsoleCmdSubtypes._t15,
    AbstractConsoleCmdSubtypes._t16,
    AbstractConsoleCmdSubtypes._t17,
    AbstractConsoleCmdSubtypes._t18,
    AbstractConsoleCmdSubtypes._t19,
    AbstractConsoleCmdSubtypes._t20,
    AbstractConsoleCmdSubtypes._t21,
    AbstractConsoleCmdSubtypes._t22,
    AbstractConsoleCmdSubtypes._t23,
    AbstractConsoleCmdSubtypes._t24,
    AbstractConsoleCmdSubtypes._t25,
    AbstractConsoleCmdSubtypes._t26,
    AbstractConsoleCmdSubtypes._t27,
    AbstractConsoleCmdSubtypes._t28,
    AbstractConsoleCmdSubtypes._t29,
    AbstractConsoleCmdSubtypes._t30,
    AbstractConsoleCmdSubtypes._t31,
    AbstractConsoleCmdSubtypes._t32,
    AbstractConsoleCmdSubtypes._t33,
    AbstractConsoleCmdSubtypes._t34,
    AbstractConsoleCmdSubtypes._t35,
    AbstractConsoleCmdSubtypes._t36,
    AbstractConsoleCmdSubtypes._t37,
    AbstractConsoleCmdSubtypes._t38
  };

  public static int Count => 39;

  public static IReadOnlyList<Type> All
  {
    get => (IReadOnlyList<Type>) AbstractConsoleCmdSubtypes._subtypes;
  }

  [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2063", Justification = "The list only contains types stored with the correct DynamicallyAccessedMembers attribute, enforced by source generation.")]
  [return: DynamicallyAccessedMembers]
  public static Type Get(int i) => AbstractConsoleCmdSubtypes._subtypes[i];
}
