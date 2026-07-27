// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.AutoSlay.Helpers.WaitHelper
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.AutoSlay.Helpers;

public static class WaitHelper
{
  public static async Task Until(
    Func<bool> condition,
    CancellationToken ct,
    TimeSpan? timeout = null,
    string? timeoutMessage = null)
  {
    TimeSpan actualTimeout = timeout ?? AutoSlayConfig.nodeWaitTimeout;
    CancellationTokenSource linkedCts;
    using (CancellationTokenSource timeoutCts = new CancellationTokenSource(actualTimeout))
    {
      linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, timeoutCts.Token);
      try
      {
        while (!condition())
        {
          AutoSlayer.CurrentWatchdog?.Check();
          await Task.Delay(AutoSlayConfig.pollingInterval, linkedCts.Token);
        }
      }
      catch (OperationCanceledException ex) when (timeoutCts.IsCancellationRequested)
      {
        string message = timeoutMessage;
        if (message == null)
          message = $"Condition not met after {actualTimeout.TotalSeconds}s";
        throw new AutoSlayTimeoutException(message);
      }
      finally
      {
        ((IDisposable) linkedCts)?.Dispose();
      }
    }
    linkedCts = (CancellationTokenSource) null;
  }

  public static async Task<T> ForNode<T>(
    Node root,
    string nodePath,
    CancellationToken ct,
    TimeSpan? timeout = null)
    where T : Node
  {
    try
    {
      await WaitHelper.Until((Func<bool>) (() => root.HasNode(NodePath.op_Implicit(nodePath))), ct, timeout, $"Node {nodePath} of type {typeof (T).Name} not found");
    }
    catch (TimeoutException ex)
    {
      WaitHelper.DumpSceneTreeContext(root, nodePath, "not found");
      throw;
    }
    T node = root.GetNode<T>(NodePath.op_Implicit(nodePath));
    Control control = (object) node as Control;
    if (control != null)
    {
      try
      {
        await WaitHelper.Until((Func<bool>) (() => ((CanvasItem) control).Visible), ct, timeout, $"Node {nodePath} not visible");
      }
      catch (TimeoutException ex)
      {
        WaitHelper.DumpSceneTreeContext(root, nodePath, "not visible");
        throw;
      }
      NButton nButton = (object) node as NButton;
      if (nButton != null)
      {
        try
        {
          await WaitHelper.Until((Func<bool>) (() => nButton.IsEnabled), ct, timeout, $"Button {nodePath} not enabled");
        }
        catch (TimeoutException ex)
        {
          WaitHelper.DumpSceneTreeContext(root, nodePath, "not enabled");
          throw;
        }
      }
    }
    T obj = node;
    node = default (T);
    return obj;
  }

  private static void DumpSceneTreeContext(Node root, string nodePath, string reason)
  {
    StringBuilder sb = new StringBuilder();
    StringBuilder stringBuilder1 = sb;
    StringBuilder stringBuilder2 = stringBuilder1;
    StringBuilder.AppendInterpolatedStringHandler interpolatedStringHandler;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(35, 1, stringBuilder1);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("[AutoSlay] Scene tree dump (node ");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(reason);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("):");
    ref StringBuilder.AppendInterpolatedStringHandler local1 = ref interpolatedStringHandler;
    stringBuilder2.AppendLine(ref local1);
    StringBuilder stringBuilder3 = sb;
    StringBuilder stringBuilder4 = stringBuilder3;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(15, 1, stringBuilder3);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("  Looking for: ");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(nodePath);
    ref StringBuilder.AppendInterpolatedStringHandler local2 = ref interpolatedStringHandler;
    stringBuilder4.AppendLine(ref local2);
    StringBuilder stringBuilder5 = sb;
    StringBuilder stringBuilder6 = stringBuilder5;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(11, 2, stringBuilder5);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("  Root: ");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<StringName>(root.Name);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(" (");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(root.GetType().Name);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(")");
    ref StringBuilder.AppendInterpolatedStringHandler local3 = ref interpolatedStringHandler;
    stringBuilder6.AppendLine(ref local3);
    int length = nodePath.LastIndexOf('/');
    string str1 = length > 0 ? nodePath.Substring(0, length) : "";
    string str2;
    if (length <= 0)
    {
      str2 = nodePath;
    }
    else
    {
      string str3 = nodePath;
      int startIndex = length + 1;
      str2 = str3.Substring(startIndex, str3.Length - startIndex);
    }
    string str4 = str2;
    Node node1 = string.IsNullOrEmpty(str1) ? root : root.GetNodeOrNull(NodePath.op_Implicit(str1));
    if (node1 == null)
    {
      StringBuilder stringBuilder7 = sb;
      StringBuilder stringBuilder8 = stringBuilder7;
      // ISSUE: explicit constructor call
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(32 /*0x20*/, 1, stringBuilder7);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("  Parent path '");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(str1);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("' does not exist!");
      ref StringBuilder.AppendInterpolatedStringHandler local4 = ref interpolatedStringHandler;
      stringBuilder8.AppendLine(ref local4);
      string[] strArray = str1.Split('/', StringSplitOptions.None);
      Node node2 = root;
      string str5 = "";
      foreach (string str6 in strArray)
      {
        if (!string.IsNullOrEmpty(str6))
        {
          str5 = str5 + (str5.Length > 0 ? "/" : "") + str6;
          Node nodeOrNull = node2.GetNodeOrNull(NodePath.op_Implicit(str6));
          if (nodeOrNull == null)
          {
            StringBuilder stringBuilder9 = sb;
            StringBuilder stringBuilder10 = stringBuilder9;
            // ISSUE: explicit constructor call
            ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(18, 1, stringBuilder9);
            ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("  Path broken at: ");
            ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(str5);
            ref StringBuilder.AppendInterpolatedStringHandler local5 = ref interpolatedStringHandler;
            stringBuilder10.AppendLine(ref local5);
            StringBuilder stringBuilder11 = sb;
            StringBuilder stringBuilder12 = stringBuilder11;
            // ISSUE: explicit constructor call
            ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(15, 1, stringBuilder11);
            ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("  Children of ");
            ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<StringName>(node2.Name);
            ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(":");
            ref StringBuilder.AppendInterpolatedStringHandler local6 = ref interpolatedStringHandler;
            stringBuilder12.AppendLine(ref local6);
            WaitHelper.DumpChildren(node2, sb, 2, 2);
            break;
          }
          node2 = nodeOrNull;
        }
      }
    }
    else
    {
      StringBuilder stringBuilder13 = sb;
      StringBuilder stringBuilder14 = stringBuilder13;
      // ISSUE: explicit constructor call
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(13, 2, stringBuilder13);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("  Parent: ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<StringName>(node1.Name);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(" (");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(node1.GetType().Name);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(")");
      ref StringBuilder.AppendInterpolatedStringHandler local7 = ref interpolatedStringHandler;
      stringBuilder14.AppendLine(ref local7);
      StringBuilder stringBuilder15 = sb;
      StringBuilder stringBuilder16 = stringBuilder15;
      // ISSUE: explicit constructor call
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(21, 1, stringBuilder15);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("  Looking for child: ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(str4);
      ref StringBuilder.AppendInterpolatedStringHandler local8 = ref interpolatedStringHandler;
      stringBuilder16.AppendLine(ref local8);
      StringBuilder stringBuilder17 = sb;
      StringBuilder stringBuilder18 = stringBuilder17;
      // ISSUE: explicit constructor call
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(15, 1, stringBuilder17);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("  Children of ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<StringName>(node1.Name);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(":");
      ref StringBuilder.AppendInterpolatedStringHandler local9 = ref interpolatedStringHandler;
      stringBuilder18.AppendLine(ref local9);
      WaitHelper.DumpChildren(node1, sb, 2, 3);
    }
    AutoSlayLog.Info(sb.ToString().TrimEnd());
  }

  private static void DumpChildren(
    Node node,
    StringBuilder sb,
    int depth,
    int maxDepth,
    string indent = "    ")
  {
    foreach (Node child in node.GetChildren(false))
    {
      string str = $"{child.Name} ({child.GetType().Name})";
      if (child is Control control)
        str += ((CanvasItem) control).Visible ? " [visible]" : " [hidden]";
      if (child is NButton nbutton)
        str += nbutton.IsEnabled ? " [enabled]" : " [disabled]";
      StringBuilder stringBuilder1 = sb;
      StringBuilder stringBuilder2 = stringBuilder1;
      StringBuilder.AppendInterpolatedStringHandler interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(0, 2, stringBuilder1);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(indent);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(str);
      ref StringBuilder.AppendInterpolatedStringHandler local1 = ref interpolatedStringHandler;
      stringBuilder2.AppendLine(ref local1);
      if (depth < maxDepth)
        WaitHelper.DumpChildren(child, sb, depth + 1, maxDepth, indent + "  ");
      else if (child.GetChildCount(false) > 0)
      {
        StringBuilder stringBuilder3 = sb;
        StringBuilder stringBuilder4 = stringBuilder3;
        interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(17, 2, stringBuilder3);
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(indent);
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("  ... (");
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<int>(child.GetChildCount(false));
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(" children)");
        ref StringBuilder.AppendInterpolatedStringHandler local2 = ref interpolatedStringHandler;
        stringBuilder4.AppendLine(ref local2);
      }
    }
  }

  public static async Task ForTask(
    Task task,
    CancellationToken ct,
    TimeSpan? timeout = null,
    string? timeoutMessage = null)
  {
    TimeSpan actualTimeout = timeout ?? AutoSlayConfig.nodeWaitTimeout;
    CancellationTokenSource linkedCts;
    using (CancellationTokenSource timeoutCts = new CancellationTokenSource(actualTimeout))
    {
      linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, timeoutCts.Token);
      try
      {
        while (!task.IsCompleted)
        {
          AutoSlayer.CurrentWatchdog?.Check();
          Task task1 = await Task.WhenAny(task, Task.Delay(AutoSlayConfig.pollingInterval, linkedCts.Token));
        }
        await task;
      }
      catch (OperationCanceledException ex) when (timeoutCts.IsCancellationRequested)
      {
        string message = timeoutMessage;
        if (message == null)
          message = $"Task not completed after {actualTimeout.TotalSeconds}s";
        throw new AutoSlayTimeoutException(message);
      }
      finally
      {
        ((IDisposable) linkedCts)?.Dispose();
      }
    }
    linkedCts = (CancellationTokenSource) null;
  }

  public static async Task WithTimeout(
    Func<CancellationToken, Task> action,
    TimeSpan timeout,
    CancellationToken ct)
  {
    Task task;
    Task completedTask;
    using (CancellationTokenSource linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct))
    {
      task = action(linkedCts.Token);
      completedTask = await Task.WhenAny(task, Task.Delay(timeout, linkedCts.Token));
      await linkedCts.CancelAsync();
      if (completedTask == task || ct.IsCancellationRequested)
        await task;
      else
        throw new AutoSlayTimeoutException($"Operation timed out after {timeout.TotalSeconds}s");
    }
    task = (Task) null;
    completedTask = (Task) null;
  }
}
