// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Pooling.NodePool
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Pooling;

public class NodePool
{
  private static Dictionary<Type, INodePool> _pools = new Dictionary<Type, INodePool>();

  public static NodePool<T> Init<T>(string scenePath, int prewarmCount) where T : Node, IPoolable
  {
    Type key = typeof (T);
    if (NodePool._pools.TryGetValue(key, out INodePool _))
      throw new InvalidOperationException($"Tried to init NodePool for type {typeof (T)} but it's already initialized!");
    NodePool<T> nodePool = new NodePool<T>(scenePath, prewarmCount);
    NodePool._pools[key] = (INodePool) nodePool;
    return nodePool;
  }

  public static IPoolable Get(Type type)
  {
    INodePool nodePool;
    if (!NodePool._pools.TryGetValue(type, out nodePool))
      throw new InvalidOperationException($"Tried to get pool for type {type} before it was initialized!");
    return nodePool.Get();
  }

  public static void Free(IPoolable poolable)
  {
    Type type = poolable.GetType();
    INodePool nodePool;
    if (!NodePool._pools.TryGetValue(type, out nodePool))
      throw new InvalidOperationException($"Tried to get pool for type {type} before it was initialized!");
    nodePool.Free(poolable);
  }

  public static T Get<T>() where T : Node, IPoolable => (T) NodePool.Get(typeof (T));

  public static void Free<T>(T obj) where T : Node, IPoolable => NodePool.Free((IPoolable) obj);
}
