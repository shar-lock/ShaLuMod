// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Serialization.ModelIdSerializationCache
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.Timeline;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO.Hashing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Serialization;

public static class ModelIdSerializationCache
{
  private static readonly Dictionary<Type, List<PropertyInfo>> _savedPropertyCache = new Dictionary<Type, List<PropertyInfo>>();
  private static readonly Dictionary<string, int> _categoryNameToNetIdMap = new Dictionary<string, int>()
  {
    [ModelId.none.Category] = 0
  };
  private static readonly List<string> _netIdToCategoryNameMap;
  private static readonly Dictionary<string, int> _entryNameToNetIdMap;
  private static readonly List<string> _netIdToEntryNameMap;
  private static readonly Dictionary<string, int> _epochNameToNetIdMap;
  private static readonly List<string> _netIdToEpochNameMap;
  private static readonly Dictionary<string, int> _propertyNameToNetIdMap;
  private static readonly List<string> _netIdToPropertyNameMap;
  private static bool _initialized;

  public static int CategoryIdBitSize { get; private set; }

  public static int EntryIdBitSize { get; private set; }

  public static int EpochIdBitSize { get; private set; }

  public static int PropertyIdBitSize { get; private set; }

  public static int MaxCategoryId => ModelIdSerializationCache._netIdToCategoryNameMap.Count - 1;

  public static int MaxEntryId => ModelIdSerializationCache._netIdToEntryNameMap.Count - 1;

  public static int MaxEpochId => ModelIdSerializationCache._netIdToEpochNameMap.Count - 1;

  public static int MaxPropertyId => ModelIdSerializationCache._netIdToPropertyNameMap.Count - 1;

  public static uint Hash { get; private set; }

  public static void Init()
  {
    byte[] byteBuffer = new byte[512 /*0x0200*/];
    XxHash32 hasher = new XxHash32();
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    List<ContentSorter<ModelId>.Item> objList = ContentSorter<ModelId>.Sort(ModelDb.All.Select<AbstractModel, Type>((Func<AbstractModel, Type>) (m => m.GetType())), ModelIdSerializationCache.\u003C\u003EO.\u003C0\u003E__GetId ?? (ModelIdSerializationCache.\u003C\u003EO.\u003C0\u003E__GetId = new Func<Type, ModelId>(ModelDb.GetId)));
    foreach (ContentSorter<ModelId>.Item obj in objList)
    {
      ModelId id = obj.id;
      if (!ModelIdSerializationCache._categoryNameToNetIdMap.ContainsKey(id.Category))
      {
        int count = ModelIdSerializationCache._netIdToCategoryNameMap.Count;
        ModelIdSerializationCache._categoryNameToNetIdMap[id.Category] = count;
        ModelIdSerializationCache._netIdToCategoryNameMap.Add(id.Category);
      }
      if (!ModelIdSerializationCache._entryNameToNetIdMap.ContainsKey(id.Entry))
      {
        int count = ModelIdSerializationCache._netIdToEntryNameMap.Count;
        ModelIdSerializationCache._entryNameToNetIdMap[id.Entry] = count;
        ModelIdSerializationCache._netIdToEntryNameMap.Add(id.Entry);
      }
      if (obj.mod?.manifest?.affectsGameplay ?? true)
      {
        int bytes1 = Encoding.UTF8.GetBytes(id.Category, 0, id.Category.Length, byteBuffer, 0);
        ((NonCryptographicHashAlgorithm) hasher).Append(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(byteBuffer, 0, bytes1)));
        int bytes2 = Encoding.UTF8.GetBytes(id.Entry, 0, id.Entry.Length, byteBuffer, 0);
        ((NonCryptographicHashAlgorithm) hasher).Append(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(byteBuffer, 0, bytes2)));
      }
    }
    foreach (ContentSorter<ModelId>.Item obj in objList)
    {
      if (obj.mod?.manifest?.affectsGameplay ?? true)
        ModelIdSerializationCache.CachePropertiesForType(obj.type, hasher, byteBuffer);
      else
        ModelIdSerializationCache.CachePropertiesForType(obj.type, (XxHash32) null, (byte[]) null);
    }
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    foreach (ContentSorter<string>.Item obj in (IEnumerable<ContentSorter<string>.Item>) ContentSorter<string>.Sort((IEnumerable<Type>) EpochModel.AllEpochs, ModelIdSerializationCache.\u003C\u003EO.\u003C1\u003E__GetId ?? (ModelIdSerializationCache.\u003C\u003EO.\u003C1\u003E__GetId = new Func<Type, string>(EpochModel.GetId))))
    {
      string id = obj.id;
      if (!ModelIdSerializationCache._epochNameToNetIdMap.ContainsKey(id))
      {
        int count = ModelIdSerializationCache._netIdToEpochNameMap.Count;
        ModelIdSerializationCache._epochNameToNetIdMap[id] = count;
        ModelIdSerializationCache._netIdToEpochNameMap.Add(id);
      }
      int bytes = Encoding.UTF8.GetBytes(id, 0, id.Length, byteBuffer, 0);
      ((NonCryptographicHashAlgorithm) hasher).Append(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(byteBuffer, 0, bytes)));
    }
    ModelIdSerializationCache._initialized = true;
    ModelIdSerializationCache.CategoryIdBitSize = Mathf.CeilToInt(Math.Log2((double) ModelIdSerializationCache._netIdToCategoryNameMap.Count));
    ModelIdSerializationCache.EntryIdBitSize = Mathf.CeilToInt(Math.Log2((double) ModelIdSerializationCache._netIdToEntryNameMap.Count));
    ModelIdSerializationCache.PropertyIdBitSize = Mathf.CeilToInt(Math.Log2((double) ModelIdSerializationCache._netIdToPropertyNameMap.Count));
    ModelIdSerializationCache.EpochIdBitSize = Mathf.CeilToInt(Math.Log2((double) ModelIdSerializationCache._netIdToEpochNameMap.Count));
    ModelIdSerializationCache.Hash = hasher.GetCurrentHashAsUInt32();
    Log.Info($"ModelIdSerializationCache initialized. Categories: {ModelIdSerializationCache._netIdToCategoryNameMap.Count} Entries: {ModelIdSerializationCache._netIdToEntryNameMap.Count} Epochs: {ModelIdSerializationCache._netIdToEpochNameMap.Count} Properties: {ModelIdSerializationCache._netIdToPropertyNameMap.Count} Hash: {ModelIdSerializationCache.Hash}");
    if (ModManager.State == ModManagerState.Initialized && ModManager.Mods.Any<Mod>((Func<Mod, bool>) (m =>
    {
      if (m.state != ModLoadState.Loaded)
        return false;
      ModManifest manifest = m.manifest;
      return ((object) manifest != null ? (manifest.affectsGameplay ? 1 : 0) : 1) == 0;
    })))
      Log.Info("  There are mods included that do not affect gameplay. Hash may not include all IDs.");
    ModelIdSerializationCache._initialized = true;
  }

  public static void ResetForTest()
  {
    ModelIdSerializationCache._savedPropertyCache.Clear();
    ModelIdSerializationCache._categoryNameToNetIdMap.Clear();
    ModelIdSerializationCache._netIdToCategoryNameMap.Clear();
    ModelIdSerializationCache._entryNameToNetIdMap.Clear();
    ModelIdSerializationCache._netIdToEntryNameMap.Clear();
    ModelIdSerializationCache._categoryNameToNetIdMap[ModelId.none.Category] = 0;
    ModelIdSerializationCache._netIdToCategoryNameMap.Add(ModelId.none.Category);
    ModelIdSerializationCache._entryNameToNetIdMap[ModelId.none.Entry] = 0;
    ModelIdSerializationCache._netIdToEntryNameMap.Add(ModelId.none.Entry);
    ModelIdSerializationCache._epochNameToNetIdMap.Clear();
    ModelIdSerializationCache._netIdToEpochNameMap.Clear();
    ModelIdSerializationCache._propertyNameToNetIdMap.Clear();
    ModelIdSerializationCache._netIdToPropertyNameMap.Clear();
    ModelIdSerializationCache.CategoryIdBitSize = 0;
    ModelIdSerializationCache.EntryIdBitSize = 0;
    ModelIdSerializationCache.EpochIdBitSize = 0;
    ModelIdSerializationCache.PropertyIdBitSize = 0;
    ModelIdSerializationCache.Hash = 0U;
    ModelIdSerializationCache._initialized = false;
  }

  private static void CachePropertiesForType([DynamicallyAccessedMembers] Type type, XxHash32? hasher, byte[]? byteBuffer)
  {
    List<PropertyInfo> list = ((IEnumerable<PropertyInfo>) type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)).Where<PropertyInfo>((Func<PropertyInfo, bool>) (p => CustomAttributeExtensions.GetCustomAttribute<SavedPropertyAttribute>((MemberInfo) p) != null)).ToList<PropertyInfo>();
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    list.Sort(ModelIdSerializationCache.\u003C\u003EO.\u003C2\u003E__CompareProperties ?? (ModelIdSerializationCache.\u003C\u003EO.\u003C2\u003E__CompareProperties = new Comparison<PropertyInfo>(ModelIdSerializationCache.CompareProperties)));
    if (list.Count > 0)
      ModelIdSerializationCache._savedPropertyCache[type] = list;
    foreach (PropertyInfo propertyInfo in list)
    {
      if (!ModelIdSerializationCache._propertyNameToNetIdMap.ContainsKey(((MemberInfo) propertyInfo).Name))
      {
        int count = ModelIdSerializationCache._netIdToPropertyNameMap.Count;
        ModelIdSerializationCache._propertyNameToNetIdMap[((MemberInfo) propertyInfo).Name] = count;
        ModelIdSerializationCache._netIdToPropertyNameMap.Add(((MemberInfo) propertyInfo).Name);
        if (hasher != null && byteBuffer != null)
        {
          int bytes = Encoding.UTF8.GetBytes(((MemberInfo) propertyInfo).Name, 0, ((MemberInfo) propertyInfo).Name.Length, byteBuffer, 0);
          ((NonCryptographicHashAlgorithm) hasher).Append(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(byteBuffer, 0, bytes)));
        }
      }
    }
  }

  private static int CompareProperties(PropertyInfo p1, PropertyInfo p2)
  {
    SavedPropertyAttribute customAttribute1 = CustomAttributeExtensions.GetCustomAttribute<SavedPropertyAttribute>((MemberInfo) p1);
    SavedPropertyAttribute customAttribute2 = CustomAttributeExtensions.GetCustomAttribute<SavedPropertyAttribute>((MemberInfo) p2);
    return customAttribute1.order != customAttribute2.order ? customAttribute1.order.CompareTo(customAttribute2.order) : string.CompareOrdinal(((MemberInfo) p1).Name, ((MemberInfo) p2).Name);
  }

  public static int GetNetIdForCategory(string category)
  {
    if (!ModelIdSerializationCache._initialized)
      throw new InvalidOperationException("ModelIdSerializationCache used before it was initialized!");
    int netIdForCategory;
    if (!ModelIdSerializationCache._categoryNameToNetIdMap.TryGetValue(category, out netIdForCategory))
      throw new ArgumentException($"ModelId category {category} could not be mapped to any net ID!");
    return netIdForCategory;
  }

  public static bool TryGetNetIdForCategory(string category, out int netId)
  {
    if (!ModelIdSerializationCache._initialized)
      throw new InvalidOperationException("ModelIdSerializationCache used before it was initialized!");
    return ModelIdSerializationCache._categoryNameToNetIdMap.TryGetValue(category, out netId);
  }

  public static string GetCategoryForNetId(int netId)
  {
    if (!ModelIdSerializationCache._initialized)
      throw new InvalidOperationException("ModelIdSerializationCache used before it was initialized!");
    if (netId < 0 || netId >= ModelIdSerializationCache._netIdToCategoryNameMap.Count)
      throw new ArgumentOutOfRangeException($"ModelId category ID {netId} is out of range! We have {ModelIdSerializationCache._netIdToCategoryNameMap.Count} categories");
    return ModelIdSerializationCache._netIdToCategoryNameMap[netId];
  }

  public static int GetNetIdForEntry(string entry)
  {
    if (!ModelIdSerializationCache._initialized)
      throw new InvalidOperationException("ModelIdSerializationCache used before it was initialized!");
    int netIdForEntry;
    if (!ModelIdSerializationCache._entryNameToNetIdMap.TryGetValue(entry, out netIdForEntry))
      throw new ArgumentException($"ModelId entry {entry} could not be mapped to any net ID!");
    return netIdForEntry;
  }

  public static bool TryGetNetIdForEntry(string entry, out int netId)
  {
    if (!ModelIdSerializationCache._initialized)
      throw new InvalidOperationException("ModelIdSerializationCache used before it was initialized!");
    return ModelIdSerializationCache._entryNameToNetIdMap.TryGetValue(entry, out netId);
  }

  public static string GetEntryForNetId(int netId)
  {
    if (!ModelIdSerializationCache._initialized)
      throw new InvalidOperationException("ModelIdSerializationCache used before it was initialized!");
    if (netId < 0 || netId >= ModelIdSerializationCache._netIdToEntryNameMap.Count)
      throw new ArgumentOutOfRangeException($"ModelId entry ID {netId} is out of range! We have {ModelIdSerializationCache._netIdToEntryNameMap.Count} entries");
    return ModelIdSerializationCache._netIdToEntryNameMap[netId];
  }

  public static int GetNetIdForEpochId(string epochId)
  {
    if (!ModelIdSerializationCache._initialized)
      throw new InvalidOperationException("ModelIdSerializationCache used before it was initialized!");
    int netIdForEpochId;
    if (!ModelIdSerializationCache._epochNameToNetIdMap.TryGetValue(epochId, out netIdForEpochId))
      throw new ArgumentException($"Epoch ID {epochId} could not be mapped to any net ID!");
    return netIdForEpochId;
  }

  public static string GetEpochIdForNetId(int netId)
  {
    if (!ModelIdSerializationCache._initialized)
      throw new InvalidOperationException("ModelIdSerializationCache used before it was initialized!");
    if (netId < 0 || netId >= ModelIdSerializationCache._netIdToEpochNameMap.Count)
      throw new ArgumentOutOfRangeException($"Epoch ID {netId} is out of range! We have {ModelIdSerializationCache._netIdToEpochNameMap.Count} entries");
    return ModelIdSerializationCache._netIdToEpochNameMap[netId];
  }

  public static int GetNetIdForPropertyName(string propertyName)
  {
    if (!ModelIdSerializationCache._initialized)
      throw new InvalidOperationException("ModelIdSerializationCache used before it was initialized!");
    int idForPropertyName;
    if (!ModelIdSerializationCache._propertyNameToNetIdMap.TryGetValue(propertyName, out idForPropertyName))
      throw new ArgumentException($"SavedProperty name {propertyName} could not be mapped to any net ID!");
    return idForPropertyName;
  }

  public static string GetPropertyNameForNetId(int netId)
  {
    if (!ModelIdSerializationCache._initialized)
      throw new InvalidOperationException("ModelIdSerializationCache used before it was initialized!");
    if (netId < 0 || netId >= ModelIdSerializationCache._netIdToPropertyNameMap.Count)
      throw new ArgumentOutOfRangeException($"SavedProperty net ID {netId} is out of range! We have {ModelIdSerializationCache._netIdToPropertyNameMap.Count} property names");
    return ModelIdSerializationCache._netIdToPropertyNameMap[netId];
  }

  public static List<PropertyInfo>? GetJsonPropertiesForType(Type t)
  {
    if (!ModelIdSerializationCache._initialized)
      throw new InvalidOperationException("ModelIdSerializationCache used before it was initialized!");
    List<PropertyInfo> propertyInfoList;
    return ModelIdSerializationCache._savedPropertyCache.TryGetValue(t, out propertyInfoList) ? propertyInfoList : (List<PropertyInfo>) null;
  }

  public static void CacheSavedPropertiesForTypeDebug([DynamicallyAccessedMembers] Type type)
  {
    ModelIdSerializationCache.CachePropertiesForType(type, (XxHash32) null, (byte[]) null);
  }

  public static string Dump()
  {
    if (!ModelIdSerializationCache._initialized)
      throw new InvalidOperationException("ModelIdSerializationCache used before it was initialized!");
    StringBuilder stringBuilder1 = new StringBuilder();
    stringBuilder1.AppendLine("CATEGORIES");
    StringBuilder.AppendInterpolatedStringHandler interpolatedStringHandler;
    for (int index = 0; index < ModelIdSerializationCache._netIdToCategoryNameMap.Count; ++index)
    {
      StringBuilder stringBuilder2 = stringBuilder1;
      StringBuilder stringBuilder3 = stringBuilder2;
      // ISSUE: explicit constructor call
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(2, 2, stringBuilder2);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(index.ToString().PadRight(3));
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(": ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(ModelIdSerializationCache._netIdToCategoryNameMap[index]);
      ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
      stringBuilder3.AppendLine(ref local);
    }
    stringBuilder1.AppendLine();
    stringBuilder1.AppendLine("ENTRIES");
    for (int index = 0; index < ModelIdSerializationCache._netIdToEntryNameMap.Count; ++index)
    {
      StringBuilder stringBuilder4 = stringBuilder1;
      StringBuilder stringBuilder5 = stringBuilder4;
      // ISSUE: explicit constructor call
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(2, 2, stringBuilder4);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(index.ToString().PadRight(3));
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(": ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(ModelIdSerializationCache._netIdToEntryNameMap[index]);
      ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
      stringBuilder5.AppendLine(ref local);
    }
    stringBuilder1.AppendLine();
    stringBuilder1.AppendLine("PROPERTIES");
    for (int index = 0; index < ModelIdSerializationCache._netIdToPropertyNameMap.Count; ++index)
    {
      StringBuilder stringBuilder6 = stringBuilder1;
      StringBuilder stringBuilder7 = stringBuilder6;
      // ISSUE: explicit constructor call
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(2, 2, stringBuilder6);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(index.ToString().PadRight(3));
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(": ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(ModelIdSerializationCache._netIdToPropertyNameMap[index]);
      ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
      stringBuilder7.AppendLine(ref local);
    }
    stringBuilder1.AppendLine();
    stringBuilder1.AppendLine("EPOCHS");
    for (int index = 0; index < ModelIdSerializationCache._netIdToEpochNameMap.Count; ++index)
    {
      StringBuilder stringBuilder8 = stringBuilder1;
      StringBuilder stringBuilder9 = stringBuilder8;
      // ISSUE: explicit constructor call
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(2, 2, stringBuilder8);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(index.ToString().PadRight(3));
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(": ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(ModelIdSerializationCache._netIdToEpochNameMap[index]);
      ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
      stringBuilder9.AppendLine(ref local);
    }
    stringBuilder1.AppendLine();
    StringBuilder stringBuilder10 = stringBuilder1;
    StringBuilder stringBuilder11 = stringBuilder10;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(6, 1, stringBuilder10);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("Hash: ");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<uint>(ModelIdSerializationCache.Hash);
    ref StringBuilder.AppendInterpolatedStringHandler local1 = ref interpolatedStringHandler;
    stringBuilder11.AppendLine(ref local1);
    return stringBuilder1.ToString();
  }

  static ModelIdSerializationCache()
  {
    int capacity1 = 1;
    List<string> stringList1 = new List<string>(capacity1);
    CollectionsMarshal.SetCount<string>(stringList1, capacity1);
    CollectionsMarshal.AsSpan<string>(stringList1)[0] = ModelId.none.Category;
    ModelIdSerializationCache._netIdToCategoryNameMap = stringList1;
    ModelIdSerializationCache._entryNameToNetIdMap = new Dictionary<string, int>()
    {
      [ModelId.none.Entry] = 0
    };
    int capacity2 = 1;
    List<string> stringList2 = new List<string>(capacity2);
    CollectionsMarshal.SetCount<string>(stringList2, capacity2);
    CollectionsMarshal.AsSpan<string>(stringList2)[0] = ModelId.none.Entry;
    ModelIdSerializationCache._netIdToEntryNameMap = stringList2;
    ModelIdSerializationCache._epochNameToNetIdMap = new Dictionary<string, int>();
    ModelIdSerializationCache._netIdToEpochNameMap = new List<string>();
    ModelIdSerializationCache._propertyNameToNetIdMap = new Dictionary<string, int>();
    ModelIdSerializationCache._netIdToPropertyNameMap = new List<string>();
  }
}
