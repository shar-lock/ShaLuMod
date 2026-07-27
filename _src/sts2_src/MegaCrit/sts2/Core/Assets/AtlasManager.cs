// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Assets.AtlasManager
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;

#nullable enable
namespace MegaCrit.Sts2.Core.Assets;

public static class AtlasManager
{
  private static readonly string[] _knownAtlases = new string[12]
  {
    "ui_atlas",
    "compressed",
    "epoch_atlas",
    "relic_atlas",
    "relic_outline_atlas",
    "power_atlas",
    "card_atlas",
    "potion_atlas",
    "potion_outline_atlas",
    "stats_screen_atlas",
    "intent_atlas",
    "era_atlas"
  };
  private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions()
  {
    PropertyNameCaseInsensitive = true
  };
  private static readonly ConcurrentDictionary<string, AtlasManager.AtlasData> _atlases = new ConcurrentDictionary<string, AtlasManager.AtlasData>();
  private static readonly ConcurrentDictionary<string, AtlasTexture> _spriteCache = new ConcurrentDictionary<string, AtlasTexture>();
  private static readonly Lock _loadLock = new Lock();
  private static readonly string[] _essentialAtlases = new string[2]
  {
    "ui_atlas",
    "compressed"
  };

  public static void LoadAllAtlases()
  {
    foreach (string knownAtlase in AtlasManager._knownAtlases)
      AtlasManager.LoadAtlas(knownAtlase);
  }

  public static void LoadEssentialAtlases()
  {
    foreach (string essentialAtlase in AtlasManager._essentialAtlases)
      AtlasManager.LoadAtlas(essentialAtlase);
  }

  public static void LoadAtlas(string atlasName)
  {
    if (AtlasManager._atlases.ContainsKey(atlasName))
      return;
    Lock.Scope scope = AtlasManager._loadLock.EnterScope();
    try
    {
      if (AtlasManager._atlases.ContainsKey(atlasName))
        return;
      AtlasManager.LoadAtlasInternal(atlasName);
    }
    finally
    {
      ((Lock.Scope) ref scope).Dispose();
    }
  }

  private static void LoadAtlasInternal(string atlasName)
  {
    string str1 = $"res://images/atlases/{atlasName}.tpsheet";
    if (!FileAccess.FileExists(str1))
    {
      Log.Warn("AtlasManager: tpsheet not found: " + str1);
    }
    else
    {
      using (FileAccess fileAccess = FileAccess.Open(str1, (FileAccess.ModeFlags) 1L))
      {
        if (fileAccess == null)
        {
          Log.Warn("AtlasManager: Failed to open " + str1);
        }
        else
        {
          TpSheetData tpSheetData = JsonSerializer.Deserialize<TpSheetData>(fileAccess.GetAsText(false), AtlasManager._jsonOptions);
          if (tpSheetData == null)
          {
            Log.Warn("AtlasManager: Failed to parse " + str1);
          }
          else
          {
            Dictionary<string, Texture2D> dictionary1 = new Dictionary<string, Texture2D>();
            Dictionary<string, AtlasManager.SpriteInfo> dictionary2 = new Dictionary<string, AtlasManager.SpriteInfo>();
            foreach (TpSheetTexture texture in tpSheetData.Textures)
            {
              string str2 = "res://images/atlases/" + texture.Image;
              Texture2D Atlas = ResourceLoader.Load<Texture2D>(str2, (string) null, (ResourceLoader.CacheMode) 1L);
              if (Atlas == null)
              {
                Log.Warn("AtlasManager: Failed to load texture: " + str2);
              }
              else
              {
                dictionary1[texture.Image] = Atlas;
                foreach (TpSheetSprite sprite in texture.Sprites)
                {
                  string key = AtlasManager.NormalizeSpriteKey(sprite.Filename);
                  dictionary2[key] = new AtlasManager.SpriteInfo(Atlas, sprite);
                }
              }
            }
            AtlasManager.AtlasData atlasData = new AtlasManager.AtlasData()
            {
              TpSheet = tpSheetData,
              PageTextures = dictionary1,
              SpriteMap = dictionary2
            };
            AtlasManager._atlases[atlasName] = atlasData;
            Log.Info($"AtlasManager: Loaded {atlasName} with {dictionary2.Count} sprites");
          }
        }
      }
    }
  }

  public static AtlasTexture? GetSprite(string atlasName, string spriteName)
  {
    AtlasManager.AtlasData atlasData;
    if (!AtlasManager._atlases.TryGetValue(atlasName, out atlasData))
      return (AtlasTexture) null;
    string key1 = AtlasManager.NormalizeSpriteKey(spriteName);
    AtlasManager.SpriteInfo spriteInfo;
    if (!atlasData.SpriteMap.TryGetValue(key1, out spriteInfo))
      return (AtlasTexture) null;
    string key2 = $"{atlasName}/{spriteName}";
    AtlasTexture sprite;
    if (AtlasManager._spriteCache.TryGetValue(key2, out sprite))
    {
      if (GodotObject.IsInstanceValid((GodotObject) sprite))
        return sprite;
      AtlasManager._spriteCache.TryRemove(key2, out AtlasTexture _);
    }
    return AtlasManager._spriteCache.GetOrAdd(key2, (Func<string, AtlasTexture>) (_ => AtlasManager.CreateAtlasTexture(spriteInfo)));
  }

  public static bool HasSprite(string atlasName, string spriteName)
  {
    AtlasManager.AtlasData atlasData;
    if (!AtlasManager._atlases.TryGetValue(atlasName, out atlasData))
      return false;
    string key = AtlasManager.NormalizeSpriteKey(spriteName);
    return atlasData.SpriteMap.ContainsKey(key);
  }

  public static int GetSpriteCount(string atlasName)
  {
    AtlasManager.AtlasData atlasData;
    return !AtlasManager._atlases.TryGetValue(atlasName, out atlasData) ? 0 : atlasData.SpriteMap.Count;
  }

  public static bool IsAtlasLoaded(string atlasName)
  {
    return AtlasManager._atlases.ContainsKey(atlasName);
  }

  public static void Clear()
  {
    AtlasManager._atlases.Clear();
    AtlasManager._spriteCache.Clear();
  }

  private static AtlasTexture CreateAtlasTexture(AtlasManager.SpriteInfo spriteInfo)
  {
    TpSheetSprite sprite = spriteInfo.Sprite;
    AtlasTexture atlasTexture = new AtlasTexture()
    {
      Atlas = spriteInfo.Atlas,
      Region = new Rect2((float) sprite.Region.X, (float) sprite.Region.Y, (float) sprite.Region.W, (float) sprite.Region.H)
    };
    if (sprite.Margin.X != 0 || sprite.Margin.Y != 0 || sprite.Margin.W != 0 || sprite.Margin.H != 0)
      atlasTexture.Margin = new Rect2((float) sprite.Margin.X, (float) sprite.Margin.Y, (float) sprite.Margin.W, (float) sprite.Margin.H);
    return atlasTexture;
  }

  private static string NormalizeSpriteKey(string filename)
  {
    if (!filename.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
      return filename;
    string str = filename;
    return str.Substring(0, str.Length - 4);
  }

  private class AtlasData
  {
    public required TpSheetData TpSheet { get; init; }

    public required Dictionary<string, Texture2D> PageTextures { get; init; }

    public required Dictionary<string, AtlasManager.SpriteInfo> SpriteMap { get; init; }
  }

  private record SpriteInfo(Texture2D Atlas, TpSheetSprite Sprite);
}
