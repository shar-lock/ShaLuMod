// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Migrations.MigratingData
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization.Metadata;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Migrations;

public class MigratingData
{
  private readonly JsonObject _data;

  public MigratingData(JsonDocument document)
  {
    this._data = JsonSerializer.Deserialize<JsonObject>(document, JsonSerializationUtility.GetTypeInfo<JsonObject>());
  }

  public MigratingData(string json)
  {
    this._data = (JsonObject) JsonNode.Parse(json, new JsonNodeOptions?(), new JsonDocumentOptions());
  }

  public MigratingData(JsonObject jsonObject) => this._data = jsonObject;

  public T ToObject<T>() where T : new()
  {
    return JsonSerializer.Deserialize<T>(((JsonNode) this._data).ToJsonString((JsonSerializerOptions) null), JsonSerializationUtility.GetTypeInfo<T>()) ?? new T();
  }

  public object? this[string key]
  {
    get
    {
      JsonNode node;
      return !this._data.TryGetPropertyValue(key, ref node) ? (object) null : MigratingData.ConvertJsonNodeToObject(node);
    }
  }

  public void Remove(string key)
  {
    if (!this._data.ContainsKey(key))
      return;
    this._data.Remove(key);
  }

  public void Rename(string oldKey, string newKey)
  {
    JsonNode jsonNode;
    if (!this._data.TryGetPropertyValue(oldKey, ref jsonNode))
      throw new MigrationException("Cannot rename a key that doesn't exist. Key=" + oldKey);
    ((JsonNode) this._data)[newKey] = jsonNode?.DeepClone();
    this._data.Remove(oldKey);
  }

  public bool Has(string key) => this._data.ContainsKey(key);

  private static object? ConvertJsonNodeToObject(JsonNode? node)
  {
    object obj;
    switch (node)
    {
      case null:
        obj = (object) null;
        break;
      case JsonObject _:
        obj = (object) JsonSerializer.Deserialize<Dictionary<string, object>>(node, JsonSerializationUtility.GetTypeInfo<Dictionary<string, object>>());
        break;
      case JsonArray _:
        obj = (object) JsonSerializer.Deserialize<List<JsonNode>>(node, JsonSerializationUtility.GetTypeInfo<List<JsonNode>>());
        break;
      case JsonValue jsonValue:
        string str;
        int num1;
        long num2;
        float num3;
        double num4;
        bool flag;
        obj = !jsonValue.TryGetValue<string>(ref str) ? (!jsonValue.TryGetValue<int>(ref num1) ? (!jsonValue.TryGetValue<long>(ref num2) ? (!jsonValue.TryGetValue<float>(ref num3) ? (!jsonValue.TryGetValue<double>(ref num4) ? (!jsonValue.TryGetValue<bool>(ref flag) ? (object) jsonValue.ToString() : (object) flag) : (object) num4) : (object) num3) : (object) num2) : (object) num1) : (object) str;
        break;
      default:
        obj = (object) null;
        break;
    }
    return obj;
  }

  public T? GetAsOrNull<T>(string key) where T : struct
  {
    return !this.Has(key) ? new T?() : new T?(this.GetAs<T>(key));
  }

  public T GetAs<T>(string key)
  {
    JsonNode jsonNode1;
    if (this._data.TryGetPropertyValue(key, ref jsonNode1))
    {
      if (jsonNode1 != null)
      {
        try
        {
          Type type = typeof (T);
          if (type == typeof (MigratingData) && jsonNode1 is JsonObject jsonObject1)
            return (T) new MigratingData(jsonObject1);
          if (type == typeof (List<MigratingData>) && jsonNode1 is JsonArray jsonArray)
          {
            List<MigratingData> migratingDataList = new List<MigratingData>();
            foreach (JsonNode jsonNode2 in jsonArray)
            {
              if (jsonNode2 is JsonObject jsonObject2)
                migratingDataList.Add(new MigratingData(jsonObject2));
              else
                throw new MigrationException($"Cannot convert array item to MigratingData: {jsonNode2}");
            }
            return (T) migratingDataList;
          }
          if (type == typeof (ModelId))
          {
            string json = jsonNode1.GetValue<string>();
            return string.IsNullOrEmpty(json) ? (T) ModelId.none : (T) ModelId.Deserialize(json);
          }
          if (type == typeof (string))
            return (T) jsonNode1.GetValue<string>();
          if (type == typeof (int))
            return (T) (ValueType) jsonNode1.GetValue<int>();
          if (type == typeof (long))
            return (T) (ValueType) jsonNode1.GetValue<long>();
          if (type == typeof (double))
            return (T) (ValueType) jsonNode1.GetValue<double>();
          if (type == typeof (float))
            return (T) (ValueType) jsonNode1.GetValue<float>();
          if (type == typeof (bool))
            return (T) (ValueType) jsonNode1.GetValue<bool>();
          if (type == typeof (DateTime))
            return (T) (ValueType) jsonNode1.GetValue<DateTime>();
          return JsonSerializer.Deserialize<T>(jsonNode1, JsonSerializationUtility.GetTypeInfo<T>()) ?? throw new MigrationException($"Unable to convert {key} to {typeof (T).Name}");
        }
        catch (Exception ex) when (!(ex is MigrationException))
        {
          throw new MigrationException($"Cannot convert value of key={key} to {typeof (T)}: {ex.Message}");
        }
      }
    }
    throw new MigrationException("Cannot get value of key=" + key);
  }

  public string GetString(string key) => this.GetAs<string>(key);

  public bool GetBool(string key) => this.GetAs<bool>(key);

  public int GetInt(string key) => this.GetAs<int>(key);

  public MigratingData GetObject(string key) => this.GetAs<MigratingData>(key);

  public void Set<T>(string key, T value)
  {
    if (value is List<MigratingData> items)
      this.SetList<MigratingData>(key, (IEnumerable<MigratingData>) items);
    if ((object) value is MigratingData)
      throw new NotImplementedException();
    ((JsonNode) this._data)[key] = (object) value != null ? (JsonNode) JsonValue.Create<T>(value, JsonSerializationUtility.GetTypeInfo<T>(), new JsonNodeOptions?()) : (JsonNode) null;
  }

  public void SetObject(string key, MigratingData value)
  {
    ((JsonNode) this._data)[key] = ((JsonNode) value._data).DeepClone();
  }

  public List<MigratingData> GetList(string key) => this.GetAs<List<MigratingData>>(key);

  public JsonNode? GetRawNode(string key)
  {
    JsonNode jsonNode;
    return !this._data.TryGetPropertyValue(key, ref jsonNode) ? (JsonNode) null : jsonNode;
  }

  public JsonObject GetRawNode() => this._data;

  public void SetList<T>(string key, IEnumerable<T> items)
  {
    if (typeof (T) == typeof (MigratingData))
    {
      JsonArray jsonArray = new JsonArray(new JsonNodeOptions?());
      foreach (T obj in items)
      {
        if (obj is MigratingData migratingData)
          jsonArray.Add(((JsonNode) migratingData._data).DeepClone());
      }
      ((JsonNode) this._data)[key] = (JsonNode) jsonArray;
    }
    else
    {
      string str = JsonSerializer.Serialize((object) items, (JsonTypeInfo) JsonSerializationUtility.GetTypeInfo<List<T>>());
      ((JsonNode) this._data)[key] = JsonNode.Parse(str, new JsonNodeOptions?(), new JsonDocumentOptions());
    }
  }
}
