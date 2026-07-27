// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Runs.SavedPropertyAttribute
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;

#nullable disable
namespace MegaCrit.Sts2.Core.Saves.Runs;

[AttributeUsage(AttributeTargets.Property)]
public class SavedPropertyAttribute : Attribute
{
  public readonly SerializationCondition defaultBehaviour;
  public readonly int order;

  public SavedPropertyAttribute() => this.defaultBehaviour = SerializationCondition.AlwaysSave;

  public SavedPropertyAttribute(SerializationCondition defaultBehaviour)
  {
    this.defaultBehaviour = defaultBehaviour;
  }

  public SavedPropertyAttribute(SerializationCondition defaultBehaviour, int order)
  {
    this.defaultBehaviour = defaultBehaviour;
    this.order = order;
  }
}
