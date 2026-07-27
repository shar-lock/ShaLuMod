// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.AutoSlay.Helpers.UiHelper
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.AutoSlay.Helpers;

public static class UiHelper
{
  public static async Task Click(NClickableControl button, int delayMs = 100)
  {
    button.ForceClick();
    await Task.Delay(delayMs);
  }

  public static List<T> FindAll<T>(Node start) where T : Node
  {
    List<T> found = new List<T>();
    if (GodotObject.IsInstanceValid((GodotObject) start))
      UiHelper.FindAllRecursive<T>(start, found);
    return found;
  }

  private static void FindAllRecursive<T>(Node node, List<T> found) where T : Node
  {
    if (!GodotObject.IsInstanceValid((GodotObject) node))
      return;
    if (node is T obj)
      found.Add(obj);
    foreach (Node child in node.GetChildren(false))
      UiHelper.FindAllRecursive<T>(child, found);
  }

  public static T? FindFirst<T>(Node start) where T : Node
  {
    if (!GodotObject.IsInstanceValid((GodotObject) start))
      return default (T);
    if (start is T first1)
      return first1;
    foreach (Node child in start.GetChildren(false))
    {
      T first2 = UiHelper.FindFirst<T>(child);
      if ((object) first2 != null)
        return first2;
    }
    return default (T);
  }
}
