// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Pooling.NodePool`1
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Collections;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Pooling;

public class NodePool<T> : INodePool where T : Node, IPoolable
{
  private static Variant _nameStr = Variant.CreateFrom("name");
  private static Variant _callableStr = Variant.CreateFrom("callable");
  private static Variant _signalStr = Variant.CreateFrom("signal");
  private string _scenePath;
  private readonly List<T> _freeObjects = new List<T>();
  private readonly HashSet<T> _usedObjects = new HashSet<T>();

  public IReadOnlyList<T> DebugFreeObjects => (IReadOnlyList<T>) this._freeObjects;

  public NodePool(string scenePath, int prewarmCount = 0)
  {
    this._scenePath = scenePath;
    for (int index = 0; index < prewarmCount; ++index)
      this._freeObjects.Add(this.Instantiate());
  }

  IPoolable INodePool.Get() => (IPoolable) this.Get();

  void INodePool.Free(IPoolable poolable) => this.Free((T) poolable);

  public T Get()
  {
    T obj;
    if (this._freeObjects.Count > 0)
    {
      List<T> freeObjects = this._freeObjects;
      obj = freeObjects[freeObjects.Count - 1];
      this._freeObjects.RemoveAt(this._freeObjects.Count - 1);
    }
    else
      obj = this.Instantiate();
    this._usedObjects.Add(obj);
    obj.OnReturnedFromPool();
    return obj;
  }

  public void Free(T obj)
  {
    if (!this._usedObjects.Contains(obj))
    {
      if (this._freeObjects.Contains(obj))
      {
        Log.Error($"Tried to free object {obj} ({obj.GetType()}) back to pool {typeof (NodePool<T>)} but it's already been freed!");
      }
      else
      {
        Log.Error($"Tried to free object {obj} ({obj.GetType()}) back to pool {typeof (NodePool<T>)} but it's not part of the pool!");
        obj.QueueFreeSafelyNoPool();
      }
    }
    else
    {
      this.DisconnectIncomingAndOutgoingSignals((Node) obj);
      this._usedObjects.Remove(obj);
      this._freeObjects.Add(obj);
      obj.OnFreedToPool();
    }
  }

  private T Instantiate()
  {
    T obj = PreloadManager.Cache.GetScene(this._scenePath).Instantiate<T>((PackedScene.GenEditState) 0L);
    obj.OnInstantiated();
    return obj;
  }

  private void DisconnectIncomingAndOutgoingSignals(Node obj)
  {
    foreach (Dictionary signal1 in ((GodotObject) obj).GetSignalList())
    {
      Variant variant = signal1[NodePool<T>._nameStr];
      StringName stringName = ((Variant) ref variant).AsStringName();
      foreach (Dictionary signalConnection in ((GodotObject) obj).GetSignalConnectionList(stringName))
      {
        variant = signalConnection[NodePool<T>._callableStr];
        Callable callable = ((Variant) ref variant).AsCallable();
        variant = signalConnection[NodePool<T>._signalStr];
        Signal signal2 = ((Variant) ref variant).AsSignal();
        this.DisconnectSignal(callable, signal2);
      }
    }
    foreach (Dictionary incomingConnection in ((GodotObject) obj).GetIncomingConnections())
    {
      Variant variant = incomingConnection[NodePool<T>._callableStr];
      Callable callable = ((Variant) ref variant).AsCallable();
      variant = incomingConnection[NodePool<T>._signalStr];
      Signal signal = ((Variant) ref variant).AsSignal();
      this.DisconnectSignal(callable, signal);
    }
    for (int index = 0; index < obj.GetChildCount(false); ++index)
      this.DisconnectIncomingAndOutgoingSignals(obj.GetChild(index, false));
  }

  private void DisconnectSignal(Callable callable, Signal signal)
  {
    GodotObject target = ((Callable) ref callable).Target;
    if (target == null && StringName.op_Equality(((Callable) ref callable).Method, (StringName) null) || target != null && !GodotObject.IsInstanceValid(target))
      return;
    StringName name = ((Signal) ref signal).Name;
    if (target is Node node1 && !node1.IsInsideTree())
      return;
    GodotObject owner = ((Signal) ref signal).Owner;
    if (!GodotObject.IsInstanceValid(owner))
      return;
    Node node2 = owner as Node;
    if (node1 != null && ((GodotObject) node1).HasSignal(name) && ((GodotObject) node1).IsConnected(name, callable))
    {
      ((GodotObject) node1).Disconnect(name, callable);
    }
    else
    {
      if (node2 == null || !((GodotObject) node2).HasSignal(name) || !((GodotObject) node2).IsConnected(name, callable))
        return;
      ((GodotObject) node2).Disconnect(name, callable);
    }
  }
}
