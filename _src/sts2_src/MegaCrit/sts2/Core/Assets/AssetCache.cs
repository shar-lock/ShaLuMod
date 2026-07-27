// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Assets.AssetCache
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Assets;

public class AssetCache
{
  private readonly ConcurrentDictionary<string, Resource> _cache = new ConcurrentDictionary<string, Resource>();
  private readonly HashSet<string> _missedCacheAssets = new HashSet<string>();
  private readonly HashSet<string> _failedAssets = new HashSet<string>();

  public int MissedCacheAssetCount => this._missedCacheAssets.Count;

  private Resource GetAsset(string path)
  {
    Resource asset;
    if (!this._cache.TryGetValue(path, out asset))
      return this.LoadAsset(path);
    if (GodotObject.IsInstanceValid((GodotObject) asset))
      return asset;
    this._cache[path] = ResourceLoader.Load<Resource>(path, (string) null, (ResourceLoader.CacheMode) 1L);
    return this._cache[path];
  }

  public TS GetAsset<TS>(string path) where TS : Resource => (TS) this.GetAsset(path);

  private Resource LoadAsset(string path)
  {
    Resource resource = !this._failedAssets.Contains(path) ? ResourceLoader.Load<Resource>(path, (string) null, (ResourceLoader.CacheMode) 1L) : throw new AssetLoadException($"Asset previously failed to load: {path}. The game installation may be corrupted.");
    if (resource is AtlasTexture)
      return resource;
    this._missedCacheAssets.Add(path);
    Log.Warn("Asset not cached: " + path);
    this._cache[path] = resource;
    return resource;
  }

  public void MarkAssetFailed(string path) => this._failedAssets.Add(path);

  public AssetLoadingSession CreateSession(string name, IEnumerable<string> paths)
  {
    return new AssetLoadingSession(name, paths, this._cache, this);
  }

  public void UnloadAssets(IEnumerable<string> assetsToUnloadSet)
  {
    foreach (string assetsToUnload in assetsToUnloadSet)
    {
      if (!this._missedCacheAssets.Contains(assetsToUnload))
      {
        Resource resource = this.RemoveAndGetResource(assetsToUnload);
        if (resource != null && GodotObject.IsInstanceValid((GodotObject) resource))
        {
          Callable callable = Callable.From(new Action(((GodotObject) resource).Dispose));
          ((Callable) ref callable).CallDeferred(Array.Empty<Variant>());
        }
      }
    }
  }

  public void UnloadMissedCacheAssets()
  {
    if (this._missedCacheAssets.Count == 0)
      return;
    Log.Info($"Unloading {this._missedCacheAssets.Count} missed cache assets");
    foreach (string missedCacheAsset in this._missedCacheAssets)
    {
      Resource resource = this.RemoveAndGetResource(missedCacheAsset);
      if (resource != null && GodotObject.IsInstanceValid((GodotObject) resource))
      {
        Callable callable = Callable.From(new Action(((GodotObject) resource).Dispose));
        ((Callable) ref callable).CallDeferred(Array.Empty<Variant>());
      }
    }
    this._missedCacheAssets.Clear();
  }

  public IReadOnlySet<string> GetLoadedCacheAssets()
  {
    HashSet<string> loadedCacheAssets = new HashSet<string>();
    foreach (KeyValuePair<string, Resource> keyValuePair in this._cache)
    {
      if (GodotObject.IsInstanceValid((GodotObject) keyValuePair.Value))
        loadedCacheAssets.Add(keyValuePair.Key);
      else
        this._cache.TryRemove(keyValuePair.Key, out Resource _);
    }
    return (IReadOnlySet<string>) loadedCacheAssets;
  }

  public IEnumerable<string> GetCacheKeys() => (IEnumerable<string>) this._cache.Keys;

  private Resource? RemoveAndGetResource(string key)
  {
    Resource resource;
    return this._cache.TryRemove(key, out resource) ? resource : (Resource) null;
  }

  public PackedScene GetScene(string path) => (PackedScene) this.GetAsset(path);

  public Texture2D GetTexture2D(string path) => (Texture2D) this.GetAsset(path);

  public Material GetMaterial(string path) => (Material) this.GetAsset(path);

  public CompressedTexture2D GetCompressedTexture2D(string path)
  {
    return (CompressedTexture2D) this.GetAsset(path);
  }

  public bool ContainsKey(string s) => this._cache.ContainsKey(s);

  public void SetAsset(string path, Resource resource) => this._cache[path] = resource;
}
