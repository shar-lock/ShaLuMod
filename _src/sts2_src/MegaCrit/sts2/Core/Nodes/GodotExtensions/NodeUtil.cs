// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.GodotExtensions.NodeUtil
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.GodotExtensions;

public static class NodeUtil
{
  public static async Task<float> AwaitProcessFrame(this Node node, CancellationToken ct = default (CancellationToken))
  {
    ct.ThrowIfCancellationRequested();
    SceneTree treeOrNull = node.GetTreeOrNull();
    if (treeOrNull == null)
      throw new TaskCanceledException();
    Variant[] signal = await ((GodotObject) treeOrNull).ToSignal((GodotObject) treeOrNull, SceneTree.SignalName.ProcessFrame);
    ct.ThrowIfCancellationRequested();
    return node.IsValid() && node.IsInsideTree() ? (float) node.GetProcessDeltaTime() : throw new TaskCanceledException();
  }

  public static async Task AwaitProcessFrameNonThrowing(this Node node, CancellationTokenSource cts)
  {
    if (cts.IsCancellationRequested)
      return;
    SceneTree treeOrNull = node.GetTreeOrNull();
    if (treeOrNull == null)
    {
      await cts.CancelAsync();
    }
    else
    {
      Variant[] signal = await ((GodotObject) treeOrNull).ToSignal((GodotObject) treeOrNull, SceneTree.SignalName.ProcessFrame);
      if (cts.IsCancellationRequested || node.IsValid() && node.IsInsideTree())
        return;
      await cts.CancelAsync();
    }
  }

  public static SceneTree? GetTreeOrNull(this Node node)
  {
    return !node.IsInsideTree() ? (SceneTree) null : node.GetTree();
  }

  public static bool IsDescendant(Node? parent, Node candidate)
  {
    if (parent == null)
      return false;
    for (Node parent1 = candidate.GetParent(); parent1 != null; parent1 = parent1.GetParent())
    {
      if (parent1 == parent)
        return true;
    }
    return false;
  }

  public static bool IsValid(this Node? node)
  {
    return node != null && GodotObject.IsInstanceValid((GodotObject) node) && !((GodotObject) node).IsQueuedForDeletion();
  }

  public static void TryGrabFocus(this Control control)
  {
    if (!NControllerManager.Instance.IsUsingController)
      return;
    if (((CanvasItem) control).IsVisibleInTree())
    {
      control.GrabFocus();
    }
    else
    {
      Callable callable = Callable.From((Action) (() =>
      {
        if (!((Node) control).IsValid() || !((Node) control).IsInsideTree())
          return;
        control.GrabFocus();
      }));
      ((Callable) ref callable).CallDeferred(Array.Empty<Variant>());
    }
  }

  public static T? GetAncestorOfType<T>(this Node node)
  {
    for (Node parent = node.GetParent(); parent != null; parent = parent.GetParent())
    {
      if (parent is T)
        return parent as T;
    }
    return default (T);
  }

  public static Task AwaitSignal(this GodotObject source, StringName signal, Node owner)
  {
    if (!GodotObject.IsInstanceValid(source))
      return Task.CompletedTask;
    TaskCompletionSource tcs = new TaskCompletionSource();
    bool resolved = false;
    Callable callable = new Callable();
    callable = Callable.From(new Action(OnSignal));
    source.Connect(signal, callable, 0U);
    owner.TreeExiting += new Action(OnExiting);
    return tcs.Task;

    void OnSignal()
    {
      if (resolved)
        return;
      resolved = true;
      if (GodotObject.IsInstanceValid(source))
        source.Disconnect(signal, callable);
      if (GodotObject.IsInstanceValid((GodotObject) owner))
        owner.TreeExiting -= new Action(OnExiting);
      tcs.TrySetResult();
    }

    void OnExiting()
    {
      if (resolved)
        return;
      resolved = true;
      if (GodotObject.IsInstanceValid(source))
        source.Disconnect(signal, callable);
      tcs.TrySetCanceled();
    }
  }

  public static Task<T?> AwaitSignal<[MustBeVariant] T>(
    this GodotObject source,
    StringName signal,
    Node owner)
    where T : class
  {
    if (!GodotObject.IsInstanceValid(source))
      return Task.FromResult<T>(default (T));
    TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();
    bool resolved = false;
    Callable callable = new Callable();
    callable = Callable.From<T>(new Action<T>(OnSignal));
    source.Connect(signal, callable, 0U);
    owner.TreeExiting += new Action(OnExiting);
    return tcs.Task;

    void OnSignal(T obj)
    {
      if (resolved)
        return;
      resolved = true;
      if (GodotObject.IsInstanceValid(source))
        source.Disconnect(signal, callable);
      if (GodotObject.IsInstanceValid((GodotObject) owner))
        owner.TreeExiting -= new Action(OnExiting);
      tcs.TrySetResult(obj);
    }

    void OnExiting()
    {
      if (resolved)
        return;
      resolved = true;
      if (GodotObject.IsInstanceValid(source))
        source.Disconnect(signal, callable);
      tcs.TrySetCanceled();
    }
  }

  public static IEnumerable<T> GetChildrenRecursive<T>(this Node node)
  {
    foreach (Node child in node.GetChildren(false))
    {
      IEnumerator<T> enumerator = child.GetChildrenRecursive<T>().GetEnumerator();
      while (enumerator.MoveNext())
        yield return enumerator.Current;
      enumerator = (IEnumerator<T>) null;
      if (child is T)
        yield return child as T;
    }
  }
}
