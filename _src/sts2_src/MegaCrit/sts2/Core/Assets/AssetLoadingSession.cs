// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Assets.AssetLoadingSession
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Collections;
using MegaCrit.Sts2.Core.Logging;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Assets;

public class AssetLoadingSession
{
  private const int _batchSize = 128 /*0x80*/;
  private readonly string _name;
  private readonly ConcurrentDictionary<string, Resource> _cache;
  private readonly AssetCache? _assetCache;
  private readonly Queue<string> _toLoad = new Queue<string>();
  private readonly Queue<string> _loading = new Queue<string>();
  private readonly Queue<string> _finalizing = new Queue<string>();
  private readonly Queue<string> _vfxScenes = new Queue<string>();
  private readonly TaskCompletionSource<bool> _completionSource = new TaskCompletionSource<bool>();
  private readonly Stopwatch _stopwatch = new Stopwatch();
  private int _totalLoaded;
  private bool _vfxLoading;
  private string? _currentVfxPath;

  public System.Threading.Tasks.Task<bool> Task => this._completionSource.Task;

  public bool IsCompleted => ((System.Threading.Tasks.Task) this._completionSource.Task).IsCompleted;

  public AssetLoadingSession(
    string name,
    IEnumerable<string> paths,
    ConcurrentDictionary<string, Resource> cache,
    AssetCache? assetCache = null)
  {
    this._cache = cache;
    this._assetCache = assetCache;
    this._name = name;
    foreach (string path in paths)
    {
      if (AssetLoadingSession.IsVfxScene(path))
        this._vfxScenes.Enqueue(path);
      else
        this._toLoad.Enqueue(path);
    }
    this._stopwatch.Start();
    Log.Info($"Preloading '{name}' assets... count={this._toLoad.Count} vfx={this._vfxScenes.Count}");
  }

  private AssetLoadingSession()
  {
    this._name = "EMPTY";
    this._cache = (ConcurrentDictionary<string, Resource>) null;
    this._toLoad = new Queue<string>();
    this._vfxScenes = new Queue<string>();
    this._completionSource.SetResult(true);
  }

  private static bool IsVfxScene(string path) => path.EndsWith(".tscn") && path.Contains("/vfx/");

  public static AssetLoadingSession Empty() => new AssetLoadingSession();

  public void Process()
  {
    this.FinalizeLoading();
    this.ProcessLoadingQueue();
    this.CheckLoadingStatus();
    if (this._toLoad.Count == 0 && this._loading.Count == 0 && this._finalizing.Count == 0)
      this.ProcessVfxQueue();
    Log.Debug($"Preloading '{this._name}' Process: toLoad={this._toLoad.Count} loading={this._loading.Count} finalizing={this._finalizing.Count} vfx={this._vfxScenes.Count}");
    if (this._toLoad.Count != 0 || this._loading.Count != 0 || this._finalizing.Count != 0 || this._vfxScenes.Count != 0 || this._vfxLoading)
      return;
    Log.Info($"Preloading '{this._name}' Complete: assets={this._totalLoaded} time_elapsed={this._stopwatch.ElapsedMilliseconds:N0}ms");
    this._stopwatch.Stop();
    this._completionSource.TrySetResult(true);
  }

  private void ProcessVfxQueue()
  {
    if (this._vfxLoading)
    {
      ResourceLoader.ThreadLoadStatus status = ResourceLoader.LoadThreadedGetStatus(this._currentVfxPath, (Array) null);
      if (status == 3L)
      {
        this.AddToCache(ResourceLoader.LoadThreadedGet(this._currentVfxPath), this._currentVfxPath);
        this._vfxLoading = false;
      }
      else
      {
        if (status != 2L && status != null)
          return;
        Log.Error("Failed to load VFX scene: " + this._currentVfxPath);
        this._vfxLoading = false;
      }
    }
    else
    {
      string key;
      while (this._vfxScenes.TryDequeue(ref key))
      {
        if (!this._cache.ContainsKey(key))
        {
          if (ResourceLoader.LoadThreadedRequest(key, "", false, (ResourceLoader.CacheMode) 1L) == null)
          {
            this._currentVfxPath = key;
            this._vfxLoading = true;
            break;
          }
          Log.Error("Error requesting VFX load for path: " + key);
        }
      }
    }
  }

  private void FinalizeLoading()
  {
    while (this._finalizing.Count != 0)
    {
      string path;
      if (!this._finalizing.TryDequeue(ref path))
        Log.Error("Failed to dequeue finalizing asset!");
      else
        this.AddToCache(ResourceLoader.LoadThreadedGet(path), path);
    }
  }

  private void AddToCache(Resource? resource, string path)
  {
    if (resource == null)
    {
      Log.Error("Resource loaded as null for path: " + path);
    }
    else
    {
      ++this._totalLoaded;
      this._cache[path] = resource;
    }
  }

  private void ProcessLoadingQueue()
  {
    string key;
    while (this._loading.Count < 128 /*0x80*/ && this._toLoad.TryDequeue(ref key))
    {
      if (!this._cache.ContainsKey(key))
      {
        if (ResourceLoader.LoadThreadedRequest(key, "", false, (ResourceLoader.CacheMode) 1L) == null)
          this._loading.Enqueue(key);
        else
          Log.Error("Error requesting load for path: " + key);
      }
    }
  }

  private void CheckLoadingStatus()
  {
    int count = this._loading.Count;
    for (int index = 0; index < count; ++index)
    {
      string path;
      if (!this._loading.TryDequeue(ref path))
      {
        Log.Error("Failed to dequeue loading asset!");
        break;
      }
      ResourceLoader.ThreadLoadStatus status = ResourceLoader.LoadThreadedGetStatus(path, (Array) null);
      ResourceLoader.ThreadLoadStatus threadLoadStatus = status;
      if (threadLoadStatus <= 3L)
      {
        switch ((uint) threadLoadStatus)
        {
          case 0:
          case 2:
            Log.Warn($"Threaded load status {status} for {path}, falling back to sync load");
            Resource resource = ResourceLoader.Load<Resource>(path, (string) null, (ResourceLoader.CacheMode) 1L);
            if (resource != null)
            {
              this.AddToCache(resource, path);
              continue;
            }
            Log.Error("Failed to load resource synchronously: " + path);
            AssetCache assetCache = this._assetCache;
            if (assetCache != null)
            {
              assetCache.MarkAssetFailed(path);
              continue;
            }
            continue;
          case 1:
            this._loading.Enqueue(path);
            continue;
          case 3:
            this._finalizing.Enqueue(path);
            continue;
        }
      }
      Log.Error("Unexpected thread load status for path: " + path);
    }
  }

  public void PrintStatus()
  {
    Log.Info($"LOADING_STATUS: ToLoad={this._toLoad.Count} Loading={this._loading.Count} Finishing={this._finalizing.Count} VfxScenes={this._vfxScenes.Count}");
  }

  public System.Threading.Tasks.Task WaitForCompletion() => (System.Threading.Tasks.Task) this._completionSource.Task;
}
