// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Serialization.PacketReaderExtensions
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Timeline;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Serialization;

public static class PacketReaderExtensions
{
  public static T ReadModel<T>(this PacketReader reader) where T : AbstractModel
  {
    if (typeof (T) == typeof (AbstractModel))
      throw new ArgumentException("T must not be AbstractModel!");
    return ModelDb.GetById<T>(reader.ReadModelIdAssumingType<T>());
  }

  public static ModelId ReadModelIdAssumingType<T>(this PacketReader reader) where T : AbstractModel
  {
    if (typeof (T) == typeof (AbstractModel))
      throw new ArgumentException("T must not be AbstractModel!");
    string entryForNetId = ModelIdSerializationCache.GetEntryForNetId(reader.ReadInt(ModelIdSerializationCache.EntryIdBitSize));
    return entryForNetId == ModelId.none.Entry ? ModelId.none : new ModelId(ModelId.SlugifyCategory(ModelDb.GetCategoryType(typeof (T)).Name), entryForNetId);
  }

  public static ModelId ReadFullModelId(this PacketReader reader)
  {
    string categoryForNetId = ModelIdSerializationCache.GetCategoryForNetId(reader.ReadInt(ModelIdSerializationCache.CategoryIdBitSize));
    string entryForNetId = ModelIdSerializationCache.GetEntryForNetId(reader.ReadInt(ModelIdSerializationCache.EntryIdBitSize));
    return entryForNetId == ModelId.none.Entry || categoryForNetId == ModelId.none.Category ? ModelId.none : new ModelId(categoryForNetId, entryForNetId);
  }

  public static EpochModel ReadEpoch(this PacketReader reader)
  {
    return EpochModel.Get(reader.ReadEpochId());
  }

  public static string ReadEpochId(this PacketReader reader)
  {
    return ModelIdSerializationCache.GetEpochIdForNetId(reader.ReadInt(ModelIdSerializationCache.EpochIdBitSize));
  }

  public static List<ModelId> ReadFullModelIdList(this PacketReader reader)
  {
    List<ModelId> modelIdList = new List<ModelId>();
    int num = reader.ReadInt();
    for (int index = 0; index < num; ++index)
      modelIdList.Add(reader.ReadFullModelId());
    return modelIdList;
  }

  public static List<T> ReadModelList<T>(this PacketReader reader) where T : AbstractModel
  {
    List<T> objList = new List<T>();
    int num = reader.ReadInt();
    for (int index = 0; index < num; ++index)
      objList.Add(reader.ReadModel<T>());
    return objList;
  }

  public static List<ModelId> ReadModelIdListAssumingType<T>(this PacketReader reader) where T : AbstractModel
  {
    List<ModelId> modelIdList = new List<ModelId>();
    int num = reader.ReadInt();
    for (int index = 0; index < num; ++index)
      modelIdList.Add(reader.ReadModelIdAssumingType<T>());
    return modelIdList;
  }
}
