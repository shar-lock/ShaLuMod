// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Helpers.GodotTreeExtensions
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Pooling;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Helpers;

public static class GodotTreeExtensions
{
  public static void AddChildSafely(this Node parent, Node? child)
  {
    if (child == null)
      return;
    if (NGame.IsMainThread() && (parent.IsNodeReady() || !parent.IsInsideTree()))
      parent.AddChild(child, false, (Node.InternalMode) 0L);
    else
      ((GodotObject) parent).CallDeferred(Node.MethodName.AddChild, new Variant[1]
      {
        Variant.op_Implicit((GodotObject) child)
      });
  }

  public static void AddSiblingSafely(this Node sibling, Node? child)
  {
    if (child == null)
      return;
    Node parent = sibling.GetParent();
    if (parent == null)
      return;
    if (NGame.IsMainThread() && (parent.IsNodeReady() || !parent.IsInsideTree()))
      sibling.AddSibling(child, false);
    else
      ((GodotObject) sibling).CallDeferred(Node.MethodName.AddSibling, new Variant[1]
      {
        Variant.op_Implicit((GodotObject) child)
      });
  }

  public static void MoveChildSafely(this Node parent, Node? child, int index)
  {
    if (child == null)
      return;
    if (NGame.IsMainThread() && (parent.IsNodeReady() || !parent.IsInsideTree()))
      parent.MoveChild(child, index);
    else
      ((GodotObject) parent).CallDeferred(Node.MethodName.MoveChild, new Variant[2]
      {
        Variant.op_Implicit((GodotObject) child),
        Variant.op_Implicit(index)
      });
  }

  public static void MoveToFrontSafely(this CanvasItem node)
  {
    Node parent = ((Node) node).GetParent();
    if (parent == null)
      return;
    if (NGame.IsMainThread() && (parent.IsNodeReady() || !parent.IsInsideTree()))
      node.MoveToFront();
    else
      ((GodotObject) parent).CallDeferred(Node.MethodName.MoveChild, new Variant[2]
      {
        Variant.op_Implicit((GodotObject) node),
        Variant.op_Implicit(-1)
      });
  }

  public static void RemoveChildSafely(this Node parent, Node? child)
  {
    if (child == null)
      return;
    if (NGame.IsMainThread() && (parent.IsNodeReady() || !parent.IsInsideTree()))
      parent.RemoveChild(child);
    else
      ((GodotObject) parent).CallDeferred(Node.MethodName.RemoveChild, new Variant[1]
      {
        Variant.op_Implicit((GodotObject) child)
      });
  }

  public static void QueueFreeSafely(this Node node)
  {
    if (!GodotObject.IsInstanceValid((GodotObject) node))
      return;
    IPoolable poolable = node as IPoolable;
    if (poolable != null)
    {
      Node parent = node.GetParent();
      if (parent != null)
        parent.RemoveChildSafely(node);
      Callable callable = Callable.From((Action) (() => NodePool.Free(poolable)));
      ((Callable) ref callable).CallDeferred(Array.Empty<Variant>());
    }
    else
      node.QueueFreeSafelyNoPool();
  }

  public static void QueueFreeSafelyNoPool(this Node node)
  {
    if (NGame.IsMainThread())
      node.QueueFree();
    else
      ((GodotObject) node).CallDeferred(Node.MethodName.QueueFree, Array.Empty<Variant>());
  }

  public static void FreeChildren(this Node node)
  {
    foreach (Node child in node.GetChildren(false))
      child.QueueFreeSafely();
  }
}
