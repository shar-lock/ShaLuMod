// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.AutoSlay.Helpers.Watchdog
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Text;

#nullable enable
namespace MegaCrit.Sts2.Core.AutoSlay.Helpers;

public class Watchdog
{
  private DateTime _lastProgressTime;
  private DateTime _lastLogTime;
  private string _lastActivity = "Starting";

  public Watchdog() => this.Reset("Initialized");

  public void Reset(string activity)
  {
    this._lastProgressTime = DateTime.UtcNow;
    this._lastActivity = activity;
  }

  public void Check()
  {
    TimeSpan timeSpan = DateTime.UtcNow - this._lastProgressTime;
    if (timeSpan > AutoSlayConfig.watchdogLogInterval && DateTime.UtcNow - this._lastLogTime > AutoSlayConfig.watchdogLogInterval)
    {
      this._lastLogTime = DateTime.UtcNow;
      AutoSlayLog.Info($"[Watchdog] No progress for {timeSpan.TotalSeconds:F1}s (last: {this._lastActivity})");
    }
    if (timeSpan > AutoSlayConfig.watchdogTimeout)
    {
      string str = Watchdog.DumpState();
      AutoSlayLog.Error($"[Watchdog] Stuck detected! No progress for {timeSpan.TotalSeconds:F1}s\nLast activity: {this._lastActivity}\n{str}");
      throw new AutoSlayTimeoutException($"Watchdog timeout: No progress for {timeSpan.TotalSeconds:F1}s. Last activity: {this._lastActivity}");
    }
  }

  public static string DumpState()
  {
    StringBuilder stringBuilder1 = new StringBuilder();
    stringBuilder1.AppendLine("=== AutoSlay State Dump ===");
    StringBuilder.AppendInterpolatedStringHandler interpolatedStringHandler;
    try
    {
      RunState state = RunManager.Instance.DebugOnlyGetState();
      if (state != null)
      {
        stringBuilder1.AppendLine("Run State:");
        StringBuilder stringBuilder2 = stringBuilder1;
        StringBuilder stringBuilder3 = stringBuilder2;
        interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(24, 3, stringBuilder2);
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("  Floor: ");
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<int>(state.TotalFloor);
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(" (Act ");
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<int>(state.CurrentActIndex + 1);
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(", Floor ");
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<int>(state.ActFloor);
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(")");
        ref StringBuilder.AppendInterpolatedStringHandler local1 = ref interpolatedStringHandler;
        stringBuilder3.AppendLine(ref local1);
        StringBuilder stringBuilder4 = stringBuilder1;
        StringBuilder stringBuilder5 = stringBuilder4;
        interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(8, 1, stringBuilder4);
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("  Room: ");
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(state.CurrentRoom?.RoomType.ToString() ?? "null");
        ref StringBuilder.AppendInterpolatedStringHandler local2 = ref interpolatedStringHandler;
        stringBuilder5.AppendLine(ref local2);
      }
      else
        stringBuilder1.AppendLine("Run State: Not initialized");
      if (NOverlayStack.Instance != null)
      {
        StringBuilder stringBuilder6 = stringBuilder1;
        StringBuilder stringBuilder7 = stringBuilder6;
        interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(23, 1, stringBuilder6);
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("Overlay Stack: ");
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<int>(NOverlayStack.Instance.ScreenCount);
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(" screens");
        ref StringBuilder.AppendInterpolatedStringHandler local3 = ref interpolatedStringHandler;
        stringBuilder7.AppendLine(ref local3);
        IOverlayScreen overlayScreen = NOverlayStack.Instance.Peek();
        if (overlayScreen != null)
        {
          StringBuilder stringBuilder8 = stringBuilder1;
          StringBuilder stringBuilder9 = stringBuilder8;
          interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(11, 1, stringBuilder8);
          ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("  Current: ");
          ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(overlayScreen.GetType().Name);
          ref StringBuilder.AppendInterpolatedStringHandler local4 = ref interpolatedStringHandler;
          stringBuilder9.AppendLine(ref local4);
        }
      }
      Node instance = (Node) NGame.Instance;
      if (instance != null)
      {
        stringBuilder1.AppendLine("Scene Container:");
        Node nodeOrNull = instance.GetNodeOrNull(NodePath.op_Implicit("RootSceneContainer"));
        if (nodeOrNull != null)
        {
          foreach (Node child in nodeOrNull.GetChildren(false))
          {
            string str = child is Control control ? (((CanvasItem) control).Visible ? "visible" : "hidden") : "";
            StringBuilder stringBuilder10 = stringBuilder1;
            StringBuilder stringBuilder11 = stringBuilder10;
            interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(10, 3, stringBuilder10);
            ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("  - ");
            ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<StringName>(child.Name);
            ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(" (");
            ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(child.GetType().Name);
            ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(") [");
            ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(str);
            ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("]");
            ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
            stringBuilder11.AppendLine(ref local);
          }
        }
      }
    }
    catch (Exception ex)
    {
      StringBuilder stringBuilder12 = stringBuilder1;
      StringBuilder stringBuilder13 = stringBuilder12;
      // ISSUE: explicit constructor call
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(21, 1, stringBuilder12);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("Error dumping state: ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(ex.Message);
      ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
      stringBuilder13.AppendLine(ref local);
    }
    stringBuilder1.AppendLine("=== End State Dump ===");
    return stringBuilder1.ToString();
  }
}
