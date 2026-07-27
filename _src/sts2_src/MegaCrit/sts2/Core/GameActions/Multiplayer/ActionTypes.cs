// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.GameActions.Multiplayer.ActionTypes
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.GameActions.Multiplayer;

public static class ActionTypes
{
  private static NetTypeCache<INetAction>? _cache;

  public static void Initialize()
  {
    List<Type> types = new List<Type>();
    types.AddRange((IEnumerable<Type>) INetActionSubtypes.All);
    types.AddRange(ReflectionHelper.GetSubtypesInMods<INetAction>());
    ActionTypes._cache = new NetTypeCache<INetAction>(types);
  }

  public static int TypeToId<T>() where T : INetAction
  {
    return (ActionTypes._cache ?? throw new InvalidOperationException()).TypeToId<T>();
  }

  private static int TypeToId(Type type)
  {
    return (ActionTypes._cache ?? throw new InvalidOperationException()).TypeToId(type);
  }

  public static int ToId(this INetAction message)
  {
    return (ActionTypes._cache ?? throw new InvalidOperationException()).TypeToId(message.GetType());
  }

  public static bool TryGetActionType(int id, out Type? type)
  {
    return (ActionTypes._cache ?? throw new InvalidOperationException()).TryGetTypeFromId(id, out type);
  }
}
