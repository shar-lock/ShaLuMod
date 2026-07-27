// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Localization.SerializableDynamicVar
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Localization;

public struct SerializableDynamicVar : IPacketSerializable
{
  [JsonPropertyName("type")]
  public DynamicVarType type;
  [JsonPropertyName("decimal_value")]
  public Decimal decimalValue;
  [JsonPropertyName("bool_value")]
  public bool boolValue;
  [JsonPropertyName("string_value")]
  [JsonSerializeCondition(SerializationCondition.SaveIfNotTypeDefault)]
  public string? stringValue;

  public object ToDynamicVar(string name)
  {
    switch (this.type)
    {
      case DynamicVarType.BaseDynamic:
        return (object) new DynamicVar(name, this.decimalValue);
      case DynamicVarType.DynamicString:
        return (object) new StringVar(name, this.stringValue);
      case DynamicVarType.Decimal:
        return (object) this.decimalValue;
      case DynamicVarType.String:
        return (object) this.stringValue;
      case DynamicVarType.Bool:
        return (object) this.boolValue;
      default:
        throw new InvalidOperationException($"Tried to convert SerializableDynamicVar with invalid type {this.type}");
    }
  }

  public static SerializableDynamicVar? FromDynamicVar(object var)
  {
    SerializableDynamicVar? nullable;
    switch (var)
    {
      case StringVar stringVar:
        nullable = new SerializableDynamicVar?(new SerializableDynamicVar()
        {
          type = DynamicVarType.DynamicString,
          stringValue = stringVar.StringValue
        });
        break;
      case DynamicVar dynamicVar:
        nullable = new SerializableDynamicVar?(new SerializableDynamicVar()
        {
          type = DynamicVarType.BaseDynamic,
          decimalValue = dynamicVar.BaseValue
        });
        break;
      case string str:
        nullable = new SerializableDynamicVar?(new SerializableDynamicVar()
        {
          type = DynamicVarType.String,
          stringValue = str
        });
        break;
      case Decimal num:
        nullable = new SerializableDynamicVar?(new SerializableDynamicVar()
        {
          type = DynamicVarType.Decimal,
          decimalValue = num
        });
        break;
      case bool flag:
        nullable = new SerializableDynamicVar?(new SerializableDynamicVar()
        {
          type = DynamicVarType.Bool,
          boolValue = flag
        });
        break;
      default:
        nullable = new SerializableDynamicVar?();
        break;
    }
    return nullable;
  }

  public void Serialize(PacketWriter writer)
  {
    writer.WriteEnum<DynamicVarType>(this.type);
    switch (this.type)
    {
      case DynamicVarType.BaseDynamic:
      case DynamicVarType.Decimal:
        writer.WriteFloat((float) this.decimalValue);
        break;
      case DynamicVarType.DynamicString:
      case DynamicVarType.String:
        writer.WriteString(this.stringValue);
        break;
      case DynamicVarType.Bool:
        writer.WriteBool(this.boolValue);
        break;
      default:
        throw new InvalidOperationException($"Tried to convert SerializableDynamicVar with invalid type {this.type}");
    }
  }

  public void Deserialize(PacketReader reader)
  {
    this.type = reader.ReadEnum<DynamicVarType>();
    switch (this.type)
    {
      case DynamicVarType.BaseDynamic:
      case DynamicVarType.Decimal:
        this.decimalValue = (Decimal) reader.ReadFloat();
        break;
      case DynamicVarType.DynamicString:
      case DynamicVarType.String:
        this.stringValue = reader.ReadString();
        break;
      case DynamicVarType.Bool:
        this.boolValue = reader.ReadBool();
        break;
      default:
        throw new InvalidOperationException($"Tried to convert SerializableDynamicVar with invalid type {this.type}");
    }
  }
}
