// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Runs.JsonSerializeConditionAttribute
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization.Metadata;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Runs;

public class JsonSerializeConditionAttribute : Attribute
{
  public readonly SerializationCondition defaultBehaviour;

  public JsonSerializeConditionAttribute(SerializationCondition defaultBehaviour)
  {
    this.defaultBehaviour = defaultBehaviour;
  }

  public static void CheckJsonSerializeConditionsModifier(JsonTypeInfo typeInfo)
  {
    if (typeInfo.Kind != 1)
      return;
    foreach (JsonPropertyInfo property in (IEnumerable<JsonPropertyInfo>) typeInfo.Properties)
    {
      ICustomAttributeProvider attributeProvider = property.AttributeProvider;
      JsonSerializeConditionAttribute attr = attributeProvider != null ? attributeProvider.GetCustomAttributes(true).OfType<JsonSerializeConditionAttribute>().FirstOrDefault<JsonSerializeConditionAttribute>() : (JsonSerializeConditionAttribute) null;
      if (attr != null)
      {
        MemberInfo memberInfo = property.AttributeProvider as MemberInfo;
        if (memberInfo != null)
          property.ShouldSerialize = (Func<object, object, bool>) ((_, c) => attr.defaultBehaviour.ShouldSerialize(c, memberInfo));
      }
    }
  }
}
