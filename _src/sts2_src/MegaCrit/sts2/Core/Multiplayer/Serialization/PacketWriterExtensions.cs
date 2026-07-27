// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Serialization.PacketWriterExtensions
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Timeline;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Serialization;

public static class PacketWriterExtensions
{
  public static void WriteModel<T>(this PacketWriter writer, T model) where T : AbstractModel
  {
    if (model.IsMutable)
      throw new ArgumentException("Cannot serialize mutable models");
    writer.WriteModelEntry(model.Id);
  }

  public static void WriteModelEntry(this PacketWriter writer, ModelId id)
  {
    if (string.IsNullOrEmpty(id.Category) || string.IsNullOrEmpty(id.Entry))
      throw new InvalidOperationException("Tried to serialize an empty ModelId!");
    int netId;
    if (!ModelIdSerializationCache.TryGetNetIdForEntry(id.Entry, out netId))
    {
      Log.Warn($"Unknown ModelId entry '{id}' during serialization, writing NONE");
      netId = ModelIdSerializationCache.GetNetIdForEntry(ModelId.none.Entry);
    }
    writer.WriteInt(netId, ModelIdSerializationCache.EntryIdBitSize);
  }

  public static void WriteEpoch<T>(this PacketWriter writer) where T : EpochModel
  {
    writer.WriteInt(ModelIdSerializationCache.GetNetIdForEpochId(EpochModel.GetId<T>()), ModelIdSerializationCache.EpochIdBitSize);
  }

  public static void WriteEpoch(this PacketWriter writer, EpochModel epochModel)
  {
    writer.WriteInt(ModelIdSerializationCache.GetNetIdForEpochId(epochModel.Id), ModelIdSerializationCache.EpochIdBitSize);
  }

  public static void WriteEpochId(this PacketWriter writer, string epochId)
  {
    writer.WriteInt(ModelIdSerializationCache.GetNetIdForEpochId(epochId), ModelIdSerializationCache.EpochIdBitSize);
  }

  public static void WriteFullModelId(this PacketWriter writer, ModelId id)
  {
    int netId1;
    bool netIdForCategory = ModelIdSerializationCache.TryGetNetIdForCategory(id.Category, out netId1);
    int netId2;
    bool netIdForEntry = ModelIdSerializationCache.TryGetNetIdForEntry(id.Entry, out netId2);
    if (!netIdForCategory || !netIdForEntry)
    {
      Log.Warn($"Unknown ModelId '{id}' during serialization, writing NONE");
      netId1 = ModelIdSerializationCache.GetNetIdForCategory(ModelId.none.Category);
      netId2 = ModelIdSerializationCache.GetNetIdForEntry(ModelId.none.Entry);
    }
    writer.WriteInt(netId1, ModelIdSerializationCache.CategoryIdBitSize);
    writer.WriteInt(netId2, ModelIdSerializationCache.EntryIdBitSize);
  }

  public static void WriteFullModelIdList(
    this PacketWriter writer,
    IReadOnlyCollection<ModelId> models)
  {
    writer.WriteInt(models.Count);
    foreach (ModelId model in (IEnumerable<ModelId>) models)
      writer.WriteFullModelId(model);
  }

  public static void WriteModelList<T>(this PacketWriter writer, IReadOnlyCollection<T> models) where T : AbstractModel
  {
    writer.WriteInt(models.Count);
    foreach (T model in (IEnumerable<T>) models)
      writer.WriteModel<T>(model);
  }

  public static void WriteModelEntriesInList(
    this PacketWriter writer,
    IReadOnlyCollection<ModelId> modelIds)
  {
    writer.WriteInt(modelIds.Count);
    foreach (ModelId modelId in (IEnumerable<ModelId>) modelIds)
      writer.WriteModelEntry(modelId);
  }
}
