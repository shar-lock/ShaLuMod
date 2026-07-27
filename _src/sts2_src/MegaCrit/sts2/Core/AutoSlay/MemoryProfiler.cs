// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.AutoSlay.MemoryProfiler
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Nodes;
using System;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.AutoSlay;

public static class MemoryProfiler
{
  private static MemoryProfiler.MemorySnapshot? _baseline;
  private static MemoryProfiler.MemorySnapshot? _previous;
  private static int _snapshotCount;

  public static void SetBaseline()
  {
    MemoryProfiler.MemorySnapshot memorySnapshot = MemoryProfiler.Capture();
    MemoryProfiler._baseline = new MemoryProfiler.MemorySnapshot?(memorySnapshot);
    MemoryProfiler._previous = new MemoryProfiler.MemorySnapshot?(memorySnapshot);
    MemoryProfiler.LogLine("baseline", memorySnapshot, memorySnapshot);
  }

  public static void LogSnapshot(string context)
  {
    MemoryProfiler.MemorySnapshot current = MemoryProfiler.Capture();
    MemoryProfiler.MemorySnapshot baseline = MemoryProfiler._baseline ?? current;
    MemoryProfiler.LogLine(context, current, baseline);
    MemoryProfiler._previous = new MemoryProfiler.MemorySnapshot?(current);
  }

  public static void Reset()
  {
    MemoryProfiler._baseline = new MemoryProfiler.MemorySnapshot?();
    MemoryProfiler._previous = new MemoryProfiler.MemorySnapshot?();
    MemoryProfiler._snapshotCount = 0;
  }

  private static MemoryProfiler.MemorySnapshot Capture()
  {
    Window root = ((Node) NGame.Instance)?.GetTree()?.Root;
    int RootViewportSignals = 0;
    if (root != null)
      RootViewportSignals = ((GodotObject) root).GetSignalConnectionList(Viewport.SignalName.SizeChanged).Count;
    return new MemoryProfiler.MemorySnapshot(OS.GetStaticMemoryUsage(), RenderingServer.GetRenderingInfo((RenderingServer.RenderingInfo) 5L), GC.GetTotalMemory(false), (int) Performance.GetMonitor((Performance.Monitor) 7L), (int) Performance.GetMonitor((Performance.Monitor) 8L), (int) Performance.GetMonitor((Performance.Monitor) 9L), (int) Performance.GetMonitor((Performance.Monitor) 10L), PreloadManager.Cache.GetCacheKeys().Count<string>(), PreloadManager.Cache.MissedCacheAssetCount, RootViewportSignals, GC.CollectionCount(0), GC.CollectionCount(1), GC.CollectionCount(2));
  }

  private static void LogLine(
    string context,
    MemoryProfiler.MemorySnapshot current,
    MemoryProfiler.MemorySnapshot baseline)
  {
    MemoryProfiler.MemorySnapshot memorySnapshot = MemoryProfiler._previous ?? current;
    string message = $"[MemProfile] context={context} | StaticMem={MemoryProfiler.Fmt(current.StaticMemBytes)}({MemoryProfiler.Diff(current.StaticMemBytes, baseline.StaticMemBytes)}) VRAM={MemoryProfiler.Fmt(current.VramBytes)}({MemoryProfiler.Diff(current.VramBytes, baseline.VramBytes)}) GcMem={MemoryProfiler.Fmt((ulong) current.GcTotalMemory)}({MemoryProfiler.Diff((ulong) current.GcTotalMemory, (ulong) baseline.GcTotalMemory)}) Objects={current.ObjectCount}({MemoryProfiler.Diff(current.ObjectCount, baseline.ObjectCount)}) Resources={current.ResourceCount}({MemoryProfiler.Diff(current.ResourceCount, baseline.ResourceCount)}) Nodes={current.NodeCount}({MemoryProfiler.Diff(current.NodeCount, baseline.NodeCount)}) Orphans={current.OrphanNodeCount}({MemoryProfiler.Diff(current.OrphanNodeCount, baseline.OrphanNodeCount)}) CachedAssets={current.CachedAssets}({MemoryProfiler.Diff(current.CachedAssets, baseline.CachedAssets)}) MissedCache={current.MissedCacheAssets}({MemoryProfiler.Diff(current.MissedCacheAssets, baseline.MissedCacheAssets)}) RootSizeSignals={current.RootViewportSignals}({MemoryProfiler.Diff(current.RootViewportSignals, baseline.RootViewportSignals)}) GC0={current.GcGen0}({MemoryProfiler.Diff(current.GcGen0, baseline.GcGen0)}) GC1={current.GcGen1}({MemoryProfiler.Diff(current.GcGen1, baseline.GcGen1)}) GC2={current.GcGen2}({MemoryProfiler.Diff(current.GcGen2, baseline.GcGen2)})";
    if (MemoryProfiler._snapshotCount >= 2)
      message += $" | room-delta: StaticMem={MemoryProfiler.Diff(current.StaticMemBytes, memorySnapshot.StaticMemBytes)} VRAM={MemoryProfiler.Diff(current.VramBytes, memorySnapshot.VramBytes)} GcMem={MemoryProfiler.Diff((ulong) current.GcTotalMemory, (ulong) memorySnapshot.GcTotalMemory)} Objects={MemoryProfiler.Diff(current.ObjectCount, memorySnapshot.ObjectCount)} Nodes={MemoryProfiler.Diff(current.NodeCount, memorySnapshot.NodeCount)} Orphans={MemoryProfiler.Diff(current.OrphanNodeCount, memorySnapshot.OrphanNodeCount)}";
    ++MemoryProfiler._snapshotCount;
    AutoSlayLog.Info(message);
  }

  private static string Fmt(ulong bytes) => NGame.FormatBytes(bytes);

  private static string Diff(ulong current, ulong baseline)
  {
    long bytes = (long) current - (long) baseline;
    return bytes < 0L ? "-" + NGame.FormatBytes((ulong) -bytes) : "+" + NGame.FormatBytes((ulong) bytes);
  }

  private static string Diff(int current, int baseline)
  {
    int num = current - baseline;
    if (num < 0)
      return $"{num}";
    return $"+{num}";
  }

  private record struct MemorySnapshot(
    ulong StaticMemBytes,
    ulong VramBytes,
    long GcTotalMemory,
    int ObjectCount,
    int ResourceCount,
    int NodeCount,
    int OrphanNodeCount,
    int CachedAssets,
    int MissedCacheAssets,
    int RootViewportSignals,
    int GcGen0,
    int GcGen1,
    int GcGen2)
  ;
}
