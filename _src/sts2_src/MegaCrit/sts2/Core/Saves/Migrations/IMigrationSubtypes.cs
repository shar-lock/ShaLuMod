// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Migrations.IMigrationSubtypes
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Saves.Migrations.PrefsSaves;
using MegaCrit.Sts2.Core.Saves.Migrations.ProfileSaves;
using MegaCrit.Sts2.Core.Saves.Migrations.ProgressSaves;
using MegaCrit.Sts2.Core.Saves.Migrations.RunHistories;
using MegaCrit.Sts2.Core.Saves.Migrations.SerializableRuns;
using MegaCrit.Sts2.Core.Saves.Migrations.SettingsSaves;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#nullable disable
namespace MegaCrit.Sts2.Core.Saves.Migrations;

public static class IMigrationSubtypes
{
  [DynamicallyAccessedMembers]
  private static readonly Type _t0 = typeof (PrefsSaveV1ToV2);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1 = typeof (ProfileSaveV1ToV2);
  [DynamicallyAccessedMembers]
  private static readonly Type _t2 = typeof (ProgressSaveV20ToV21);
  [DynamicallyAccessedMembers]
  private static readonly Type _t3 = typeof (ProgressSaveV21ToV22);
  [DynamicallyAccessedMembers]
  private static readonly Type _t4 = typeof (RunHistoryV7ToV8);
  [DynamicallyAccessedMembers]
  private static readonly Type _t5 = typeof (RunHistoryV8ToV9);
  [DynamicallyAccessedMembers]
  private static readonly Type _t6 = typeof (SerializableRunV12ToV13);
  [DynamicallyAccessedMembers]
  private static readonly Type _t7 = typeof (SerializableRunV13ToV14);
  [DynamicallyAccessedMembers]
  private static readonly Type _t8 = typeof (SerializableRunV14ToV15);
  [DynamicallyAccessedMembers]
  private static readonly Type _t9 = typeof (SerializableRunV15ToV16);
  [DynamicallyAccessedMembers]
  private static readonly Type _t10 = typeof (SerializableRunV16ToV17);
  [DynamicallyAccessedMembers]
  private static readonly Type _t11 = typeof (SerializableRunV17ToV18);
  [DynamicallyAccessedMembers]
  private static readonly Type _t12 = typeof (SerializableRunV18ToV19);
  [DynamicallyAccessedMembers]
  private static readonly Type _t13 = typeof (SettingsSaveV3ToV4);
  [DynamicallyAccessedMembers]
  private static readonly Type _t14 = typeof (SettingsSaveV4ToV5);
  [DynamicallyAccessedMembers]
  private static readonly Type _t15 = typeof (SettingsSaveV5ToV6);
  private static readonly Type[] _subtypes = new Type[16 /*0x10*/]
  {
    IMigrationSubtypes._t0,
    IMigrationSubtypes._t1,
    IMigrationSubtypes._t2,
    IMigrationSubtypes._t3,
    IMigrationSubtypes._t4,
    IMigrationSubtypes._t5,
    IMigrationSubtypes._t6,
    IMigrationSubtypes._t7,
    IMigrationSubtypes._t8,
    IMigrationSubtypes._t9,
    IMigrationSubtypes._t10,
    IMigrationSubtypes._t11,
    IMigrationSubtypes._t12,
    IMigrationSubtypes._t13,
    IMigrationSubtypes._t14,
    IMigrationSubtypes._t15
  };

  public static int Count => 16 /*0x10*/;

  public static IReadOnlyList<Type> All => (IReadOnlyList<Type>) IMigrationSubtypes._subtypes;

  [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2063", Justification = "The list only contains types stored with the correct DynamicallyAccessedMembers attribute, enforced by source generation.")]
  [return: DynamicallyAccessedMembers]
  public static Type Get(int i) => IMigrationSubtypes._subtypes[i];
}
