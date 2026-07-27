// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Runs.SavedProperties
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Runs;

[Serializable]
public class SavedProperties : IPacketSerializable
{
  [JsonIgnore]
  [JsonPropertyName("ints")]
  public List<SavedProperties.SavedProperty<int>>? ints;
  [JsonIgnore]
  [JsonPropertyName("bools")]
  public List<SavedProperties.SavedProperty<bool>>? bools;
  [JsonIgnore]
  [JsonPropertyName("strings")]
  public List<SavedProperties.SavedProperty<string>>? strings;
  [JsonIgnore]
  [JsonPropertyName("int_arrays")]
  public List<SavedProperties.SavedProperty<int[]>>? intArrays;
  [JsonIgnore]
  [JsonPropertyName("model_ids")]
  public List<SavedProperties.SavedProperty<ModelId>>? modelIds;
  [JsonIgnore]
  [JsonPropertyName("cards")]
  public List<SavedProperties.SavedProperty<SerializableCard>>? cards;
  [JsonIgnore]
  [JsonPropertyName("card_arrays")]
  public List<SavedProperties.SavedProperty<SerializableCard[]>>? cardArrays;

  public static SavedProperties? From(AbstractModel model)
  {
    return SavedProperties.FromInternal((object) model, model.Id);
  }

  public static SavedProperties? FromInternal(object model, ModelId? id)
  {
    SavedProperties savedProperties1 = new SavedProperties();
    foreach (PropertyInfo propertyInfo in ModelIdSerializationCache.GetJsonPropertiesForType(model.GetType()) ?? new List<PropertyInfo>())
    {
      string name1 = ((MemberInfo) propertyInfo).Name;
      object candidate = propertyInfo.GetValue(model);
      if (CustomAttributeExtensions.GetCustomAttribute<SavedPropertyAttribute>((MemberInfo) propertyInfo).defaultBehaviour.ShouldSerialize(candidate, (MemberInfo) propertyInfo))
      {
        switch (candidate)
        {
          case null:
            Log.Warn($"Property {name1} on {id} is null, which is not a valid SavedProperty");
            continue;
          case int num:
            SavedProperties savedProperties2 = savedProperties1;
            if (savedProperties2.ints == null)
              savedProperties2.ints = new List<SavedProperties.SavedProperty<int>>();
            savedProperties1.ints.Add(new SavedProperties.SavedProperty<int>(name1, num));
            continue;
          case int[] numArray:
            SavedProperties savedProperties3 = savedProperties1;
            if (savedProperties3.intArrays == null)
              savedProperties3.intArrays = new List<SavedProperties.SavedProperty<int[]>>();
            savedProperties1.intArrays.Add(new SavedProperties.SavedProperty<int[]>(name1, numArray));
            continue;
          case Enum @enum:
            SavedProperties savedProperties4 = savedProperties1;
            if (savedProperties4.ints == null)
              savedProperties4.ints = new List<SavedProperties.SavedProperty<int>>();
            savedProperties1.ints.Add(new SavedProperties.SavedProperty<int>(name1, Convert.ToInt32((object) @enum)));
            continue;
          case Enum[] source:
            SavedProperties savedProperties5 = savedProperties1;
            if (savedProperties5.intArrays == null)
              savedProperties5.intArrays = new List<SavedProperties.SavedProperty<int[]>>();
            List<SavedProperties.SavedProperty<int[]>> intArrays = savedProperties1.intArrays;
            string name2 = name1;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Func<Enum, int> selector = SavedProperties.\u003C\u003EO.\u003C0\u003E__ToInt32 ?? (SavedProperties.\u003C\u003EO.\u003C0\u003E__ToInt32 = new Func<Enum, int>(Convert.ToInt32));
            int[] array = ((IEnumerable<Enum>) source).Select<Enum, int>(selector).ToArray<int>();
            SavedProperties.SavedProperty<int[]> savedProperty = new SavedProperties.SavedProperty<int[]>(name2, array);
            intArrays.Add(savedProperty);
            continue;
          default:
            ModelId modelId = candidate as ModelId;
            if ((object) modelId == null)
            {
              switch (candidate)
              {
                case bool flag:
                  SavedProperties savedProperties6 = savedProperties1;
                  if (savedProperties6.bools == null)
                    savedProperties6.bools = new List<SavedProperties.SavedProperty<bool>>();
                  savedProperties1.bools.Add(new SavedProperties.SavedProperty<bool>(name1, flag));
                  continue;
                case string str:
                  SavedProperties savedProperties7 = savedProperties1;
                  if (savedProperties7.strings == null)
                    savedProperties7.strings = new List<SavedProperties.SavedProperty<string>>();
                  savedProperties1.strings.Add(new SavedProperties.SavedProperty<string>(name1, str));
                  continue;
                case SerializableCard serializableCard:
                  SavedProperties savedProperties8 = savedProperties1;
                  if (savedProperties8.cards == null)
                    savedProperties8.cards = new List<SavedProperties.SavedProperty<SerializableCard>>();
                  savedProperties1.cards.Add(new SavedProperties.SavedProperty<SerializableCard>(name1, serializableCard));
                  continue;
                case List<SerializableCard> serializableCardList:
                  SavedProperties savedProperties9 = savedProperties1;
                  if (savedProperties9.cardArrays == null)
                    savedProperties9.cardArrays = new List<SavedProperties.SavedProperty<SerializableCard[]>>();
                  savedProperties1.cardArrays.Add(new SavedProperties.SavedProperty<SerializableCard[]>(name1, serializableCardList.ToArray()));
                  continue;
                default:
                  throw new JsonException($"Property {name1} on {id} is not a valid type for [SavedProperty] (type {candidate.GetType()}).");
              }
            }
            else
            {
              SavedProperties savedProperties10 = savedProperties1;
              if (savedProperties10.modelIds == null)
                savedProperties10.modelIds = new List<SavedProperties.SavedProperty<ModelId>>();
              savedProperties1.modelIds.Add(new SavedProperties.SavedProperty<ModelId>(name1, modelId));
              continue;
            }
        }
      }
    }
    return !savedProperties1.Any() ? (SavedProperties) null : savedProperties1;
  }

  private bool Any()
  {
    if (this.ints != null)
    {
      List<SavedProperties.SavedProperty<int>> ints = this.ints;
      // ISSUE: explicit non-virtual call
      if ((ints != null ? (__nonvirtual (ints.Count) != 0 ? 1 : 0) : 1) != 0)
        goto label_17;
    }
    if (this.bools != null)
    {
      List<SavedProperties.SavedProperty<bool>> bools = this.bools;
      // ISSUE: explicit non-virtual call
      if ((bools != null ? (__nonvirtual (bools.Count) != 0 ? 1 : 0) : 1) != 0)
        goto label_17;
    }
    if (this.intArrays != null)
    {
      List<SavedProperties.SavedProperty<int[]>> intArrays = this.intArrays;
      // ISSUE: explicit non-virtual call
      if ((intArrays != null ? (__nonvirtual (intArrays.Count) != 0 ? 1 : 0) : 1) != 0)
        goto label_17;
    }
    if (this.strings != null)
    {
      List<SavedProperties.SavedProperty<string>> strings = this.strings;
      // ISSUE: explicit non-virtual call
      if ((strings != null ? (__nonvirtual (strings.Count) != 0 ? 1 : 0) : 1) != 0)
        goto label_17;
    }
    if (this.modelIds != null)
    {
      List<SavedProperties.SavedProperty<ModelId>> modelIds = this.modelIds;
      // ISSUE: explicit non-virtual call
      if ((modelIds != null ? (__nonvirtual (modelIds.Count) != 0 ? 1 : 0) : 1) != 0)
        goto label_17;
    }
    if (this.cards != null)
    {
      List<SavedProperties.SavedProperty<SerializableCard>> cards = this.cards;
      // ISSUE: explicit non-virtual call
      if ((cards != null ? (__nonvirtual (cards.Count) != 0 ? 1 : 0) : 1) != 0)
        goto label_17;
    }
    if (this.cardArrays == null)
      return false;
    List<SavedProperties.SavedProperty<SerializableCard[]>> cardArrays = this.cardArrays;
    // ISSUE: explicit non-virtual call
    return cardArrays == null || __nonvirtual (cardArrays.Count) != 0;
label_17:
    return true;
  }

  public void Fill(AbstractModel model) => this.FillInternal((object) model);

  [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "We only create array types that are referenced by SavedProperties that already exist in code.")]
  public void FillInternal(object model)
  {
    Type type = model.GetType();
    if (this.ints != null)
    {
      foreach (SavedProperties.SavedProperty<int> savedProperty in this.ints)
      {
        PropertyInfo property = type.GetProperty(savedProperty.name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        property?.SetValue(model, property.PropertyType.IsEnum ? Enum.ToObject(property.PropertyType, savedProperty.value) : (object) savedProperty.value);
      }
    }
    if (this.intArrays != null)
    {
      foreach (SavedProperties.SavedProperty<int[]> intArray in this.intArrays)
      {
        PropertyInfo property = type.GetProperty(intArray.name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (!PropertyInfo.op_Equality(property, (PropertyInfo) null))
        {
          Type elementType = property.PropertyType.GetElementType();
          if (elementType.IsEnum)
          {
            Array instance = Array.CreateInstance(elementType, intArray.value.Length);
            for (int index = 0; index < intArray.value.Length; ++index)
              instance.SetValue(Enum.ToObject(elementType, intArray.value[index]), index);
            property.SetValue(model, (object) instance);
          }
          else
            property.SetValue(model, (object) intArray.value);
        }
      }
    }
    if (this.bools != null)
    {
      foreach (SavedProperties.SavedProperty<bool> savedProperty in this.bools)
        type.GetProperty(savedProperty.name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.SetValue(model, (object) savedProperty.value);
    }
    if (this.modelIds != null)
    {
      foreach (SavedProperties.SavedProperty<ModelId> modelId in this.modelIds)
        type.GetProperty(modelId.name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.SetValue(model, (object) modelId.value);
    }
    if (this.cards != null)
    {
      foreach (SavedProperties.SavedProperty<SerializableCard> card in this.cards)
        type.GetProperty(card.name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.SetValue(model, (object) card.value);
    }
    if (this.cardArrays != null)
    {
      foreach (SavedProperties.SavedProperty<SerializableCard[]> cardArray in this.cardArrays)
        type.GetProperty(cardArray.name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.SetValue(model, (object) ((IEnumerable<SerializableCard>) cardArray.value).ToList<SerializableCard>());
    }
    if (this.strings == null)
      return;
    foreach (SavedProperties.SavedProperty<string> savedProperty in this.strings)
      type.GetProperty(savedProperty.name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.SetValue(model, (object) savedProperty.value);
  }

  private static void WritePropertyName(PacketWriter writer, string propertyName)
  {
    writer.WriteInt(ModelIdSerializationCache.GetNetIdForPropertyName(propertyName), ModelIdSerializationCache.PropertyIdBitSize);
  }

  private static string ReadPropertyName(PacketReader reader)
  {
    return ModelIdSerializationCache.GetPropertyNameForNetId(reader.ReadInt(ModelIdSerializationCache.PropertyIdBitSize));
  }

  public void Serialize(PacketWriter writer)
  {
    writer.WriteBool(this.ints != null);
    if (this.ints != null)
    {
      writer.WriteInt(this.ints.Count, 8);
      foreach (SavedProperties.SavedProperty<int> savedProperty in this.ints)
      {
        SavedProperties.WritePropertyName(writer, savedProperty.name);
        writer.WriteInt(savedProperty.value);
      }
    }
    writer.WriteBool(this.intArrays != null);
    if (this.intArrays != null)
    {
      writer.WriteInt(this.intArrays.Count, 8);
      foreach (SavedProperties.SavedProperty<int[]> intArray in this.intArrays)
      {
        SavedProperties.WritePropertyName(writer, intArray.name);
        writer.WriteInt(intArray.value.Length);
        foreach (int val in intArray.value)
          writer.WriteInt(val);
      }
    }
    writer.WriteBool(this.bools != null);
    if (this.bools != null)
    {
      writer.WriteInt(this.bools.Count, 8);
      foreach (SavedProperties.SavedProperty<bool> savedProperty in this.bools)
      {
        SavedProperties.WritePropertyName(writer, savedProperty.name);
        writer.WriteBool(savedProperty.value);
      }
    }
    writer.WriteBool(this.modelIds != null);
    if (this.modelIds != null)
    {
      writer.WriteInt(this.modelIds.Count, 8);
      foreach (SavedProperties.SavedProperty<ModelId> modelId in this.modelIds)
      {
        SavedProperties.WritePropertyName(writer, modelId.name);
        writer.WriteFullModelId(modelId.value);
      }
    }
    writer.WriteBool(this.cards != null);
    if (this.cards != null)
    {
      writer.WriteInt(this.cards.Count, 8);
      foreach (SavedProperties.SavedProperty<SerializableCard> card in this.cards)
      {
        SavedProperties.WritePropertyName(writer, card.name);
        writer.Write<SerializableCard>(card.value);
      }
    }
    writer.WriteBool(this.cardArrays != null);
    if (this.cardArrays != null)
    {
      writer.WriteInt(this.cardArrays.Count, 8);
      foreach (SavedProperties.SavedProperty<SerializableCard[]> cardArray in this.cardArrays)
      {
        SavedProperties.WritePropertyName(writer, cardArray.name);
        writer.WriteInt(cardArray.value.Length);
        foreach (SerializableCard val in cardArray.value)
          writer.Write<SerializableCard>(val);
      }
    }
    writer.WriteBool(this.strings != null);
    if (this.strings == null)
      return;
    writer.WriteInt(this.strings.Count, 8);
    foreach (SavedProperties.SavedProperty<string> savedProperty in this.strings)
    {
      SavedProperties.WritePropertyName(writer, savedProperty.name);
      writer.WriteString(savedProperty.value);
    }
  }

  public void Deserialize(PacketReader reader)
  {
    if (reader.ReadBool())
    {
      this.ints = new List<SavedProperties.SavedProperty<int>>();
      int num = reader.ReadInt(8);
      for (int index = 0; index < num; ++index)
        this.ints.Add(new SavedProperties.SavedProperty<int>(SavedProperties.ReadPropertyName(reader), reader.ReadInt()));
    }
    if (reader.ReadBool())
    {
      this.intArrays = new List<SavedProperties.SavedProperty<int[]>>();
      int num = reader.ReadInt(8);
      for (int index1 = 0; index1 < num; ++index1)
      {
        string name = SavedProperties.ReadPropertyName(reader);
        int length = reader.ReadInt();
        int[] numArray = new int[length];
        for (int index2 = 0; index2 < length; ++index2)
          numArray[index2] = reader.ReadInt();
        this.intArrays.Add(new SavedProperties.SavedProperty<int[]>(name, numArray));
      }
    }
    if (reader.ReadBool())
    {
      this.bools = new List<SavedProperties.SavedProperty<bool>>();
      int num = reader.ReadInt(8);
      for (int index = 0; index < num; ++index)
        this.bools.Add(new SavedProperties.SavedProperty<bool>(SavedProperties.ReadPropertyName(reader), reader.ReadBool()));
    }
    if (reader.ReadBool())
    {
      this.modelIds = new List<SavedProperties.SavedProperty<ModelId>>();
      int num = reader.ReadInt(8);
      for (int index = 0; index < num; ++index)
        this.modelIds.Add(new SavedProperties.SavedProperty<ModelId>(SavedProperties.ReadPropertyName(reader), reader.ReadFullModelId()));
    }
    if (reader.ReadBool())
    {
      this.cards = new List<SavedProperties.SavedProperty<SerializableCard>>();
      int num = reader.ReadInt(8);
      for (int index = 0; index < num; ++index)
        this.cards.Add(new SavedProperties.SavedProperty<SerializableCard>(SavedProperties.ReadPropertyName(reader), reader.Read<SerializableCard>()));
    }
    if (reader.ReadBool())
    {
      this.cardArrays = new List<SavedProperties.SavedProperty<SerializableCard[]>>();
      int num = reader.ReadInt(8);
      for (int index3 = 0; index3 < num; ++index3)
      {
        string name = SavedProperties.ReadPropertyName(reader);
        int length = reader.ReadInt();
        SerializableCard[] serializableCardArray = new SerializableCard[length];
        for (int index4 = 0; index4 < length; ++index4)
          serializableCardArray[index4] = reader.Read<SerializableCard>();
        this.cardArrays.Add(new SavedProperties.SavedProperty<SerializableCard[]>(name, serializableCardArray));
      }
    }
    if (!reader.ReadBool())
      return;
    this.strings = new List<SavedProperties.SavedProperty<string>>();
    int num1 = reader.ReadInt(8);
    for (int index = 0; index < num1; ++index)
      this.strings.Add(new SavedProperties.SavedProperty<string>(SavedProperties.ReadPropertyName(reader), reader.ReadString()));
  }

  public override string ToString()
  {
    if (!this.Any())
      return "<none>";
    List<string> values = new List<string>();
    if (this.ints != null)
    {
      foreach (SavedProperties.SavedProperty<int> savedProperty in this.ints)
        values.Add($"{savedProperty.name}={savedProperty.value}");
    }
    if (this.bools != null)
    {
      foreach (SavedProperties.SavedProperty<bool> savedProperty in this.bools)
        values.Add($"{savedProperty.name}={savedProperty.value}");
    }
    if (this.strings != null)
    {
      foreach (SavedProperties.SavedProperty<string> savedProperty in this.strings)
        values.Add($"{savedProperty.name}={savedProperty.value}");
    }
    if (this.intArrays != null)
    {
      foreach (SavedProperties.SavedProperty<int[]> intArray in this.intArrays)
        values.Add($"{intArray.name}={string.Join<int>(",", (IEnumerable<int>) intArray.value)}");
    }
    if (this.modelIds != null)
    {
      foreach (SavedProperties.SavedProperty<ModelId> modelId in this.modelIds)
        values.Add($"{modelId.name}={modelId.value}");
    }
    if (this.cards != null)
    {
      foreach (SavedProperties.SavedProperty<SerializableCard> card in this.cards)
        values.Add($"{card.name}={card.value}");
    }
    if (this.cardArrays != null)
    {
      foreach (SavedProperties.SavedProperty<SerializableCard[]> cardArray in this.cardArrays)
        values.Add($"{cardArray.name}={string.Join(",", ((IEnumerable<SerializableCard>) cardArray.value).Select<SerializableCard, string>((Func<SerializableCard, string>) (c => c.Id.Entry)))}");
    }
    return string.Join(",", (IEnumerable<string>) values);
  }

  public struct SavedProperty<T>(string name, T value)
  {
    public string name = name;
    public T value = value;
  }
}
