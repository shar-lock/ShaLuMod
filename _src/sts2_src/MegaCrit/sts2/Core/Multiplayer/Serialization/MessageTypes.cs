// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Serialization.MessageTypes
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Helpers;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Serialization;

public static class MessageTypes
{
  private static NetTypeCache<INetMessage>? _cache;

  public static int Count => (MessageTypes._cache ?? throw new InvalidOperationException()).Count;

  public static void Initialize()
  {
    List<Type> types = new List<Type>();
    types.AddRange((IEnumerable<Type>) INetMessageSubtypes.All);
    types.AddRange(ReflectionHelper.GetSubtypesInMods<INetMessage>());
    MessageTypes._cache = new NetTypeCache<INetMessage>(types);
  }

  public static int TypeToId<T>() where T : INetMessage
  {
    return (MessageTypes._cache ?? throw new InvalidOperationException()).TypeToId<T>();
  }

  private static int TypeToId(Type type)
  {
    return (MessageTypes._cache ?? throw new InvalidOperationException()).TypeToId(type);
  }

  public static int ToId(this INetMessage message)
  {
    return (MessageTypes._cache ?? throw new InvalidOperationException()).TypeToId(message.GetType());
  }

  public static bool TryGetMessageType(int id, out Type? type)
  {
    return (MessageTypes._cache ?? throw new InvalidOperationException()).TryGetTypeFromId(id, out type);
  }
}
