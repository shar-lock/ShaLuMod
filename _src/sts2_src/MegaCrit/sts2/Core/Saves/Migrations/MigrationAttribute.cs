// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Migrations.MigrationAttribute
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Migrations;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class MigrationAttribute : Attribute
{
  public Type SaveType { get; }

  public int FromVersion { get; }

  public int ToVersion { get; }

  public MigrationAttribute(Type saveType, int fromVersion, int toVersion)
  {
    this.SaveType = saveType;
    this.FromVersion = fromVersion;
    this.ToVersion = toVersion;
  }
}
