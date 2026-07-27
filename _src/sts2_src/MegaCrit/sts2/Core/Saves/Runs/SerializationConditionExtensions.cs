// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Runs.SerializationConditionExtensions
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Runs;

public static class SerializationConditionExtensions
{
  private static readonly Dictionary<Type, object?> _defaultTypeCache = new Dictionary<Type, object>();

  private static object? GetTypeDefaultValue([DynamicallyAccessedMembers] Type t)
  {
    return t.IsValueType ? Activator.CreateInstance(t) : (object) null;
  }

  private static object? GetMemberDefaultValue(MemberInfo info)
  {
    object instance;
    if (!SerializationConditionExtensions._defaultTypeCache.TryGetValue(info.DeclaringType, out instance))
    {
      instance = Activator.CreateInstance(info.DeclaringType);
      SerializationConditionExtensions._defaultTypeCache[info.DeclaringType] = instance;
    }
    MemberTypes memberType = info.MemberType;
    if (memberType == 4)
      return ((FieldInfo) info).GetValue(instance);
    if (memberType == 16 /*0x10*/)
      return ((PropertyInfo) info).GetValue(instance);
    throw new ArgumentException($"Input MemberInfo must be of type FieldInfo or PropertyInfo. It was {info.MemberType} ({info.GetType()})");
  }

  [return: DynamicallyAccessedMembers]
  private static Type GetUnderlyingType(this MemberInfo member)
  {
    MemberTypes memberType = member.MemberType;
    if (memberType == 4)
      return ((FieldInfo) member).FieldType;
    if (memberType == 16 /*0x10*/)
      return ((PropertyInfo) member).PropertyType;
    throw new ArgumentException($"Input MemberInfo must be of type FieldInfo or PropertyInfo. It was {member.MemberType} ({member.GetType()})");
  }

  public static bool ShouldSerialize(
    this SerializationCondition condition,
    object? candidate,
    MemberInfo memberInfo)
  {
    switch (condition)
    {
      case SerializationCondition.AlwaysSave:
        return true;
      case SerializationCondition.SaveIfNotPropertyDefault:
        return !object.Equals(candidate, SerializationConditionExtensions.GetMemberDefaultValue(memberInfo));
      case SerializationCondition.SaveIfNotTypeDefault:
        return !object.Equals(candidate, SerializationConditionExtensions.GetTypeDefaultValue(memberInfo.GetUnderlyingType()));
      case SerializationCondition.SaveIfNotCollectionEmptyOrNull:
        return candidate is ICollection collection && collection.Count != 0;
      default:
        throw new InvalidEnumArgumentException("Invalid condition passed!");
    }
  }
}
