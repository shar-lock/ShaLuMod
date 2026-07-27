// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Rooms.BackgroundAssets
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Rooms;

public class BackgroundAssets
{
  public string BackgroundScenePath { get; }

  public List<string> BgLayers { get; }

  public string? FgLayer { get; }

  public BackgroundAssets(string title, Rng rng)
  {
    string str = $"res://scenes/backgrounds/{title}/layers";
    using (DirAccess dirAccess = DirAccess.Open(str))
    {
      if (dirAccess == null)
        throw new InvalidOperationException("could not find directory " + str);
      Dictionary<string, List<string>> bgLayers = new Dictionary<string, List<string>>();
      List<string> stringList = new List<string>();
      dirAccess.ListDirBegin();
      for (string next = dirAccess.GetNext(); next != ""; next = dirAccess.GetNext())
      {
        if (dirAccess.CurrentIsDir())
          throw new InvalidOperationException("there should be no other directories within the layers directory");
        if (next.Contains("_fg_"))
        {
          stringList.Add($"{str}/{next}");
        }
        else
        {
          string key = next.Contains("_bg_") ? next.Split("_bg_", StringSplitOptions.None)[1].Split("_", StringSplitOptions.None)[0] : throw new InvalidOperationException("files must either contain '_fg_' or '_bg_'");
          if (!bgLayers.ContainsKey(key))
            bgLayers.Add(key, new List<string>());
          bgLayers[key].Add($"{str}/{next}");
        }
      }
      this.BackgroundScenePath = SceneHelper.GetScenePath($"backgrounds/{title}/{title}_background");
      this.BgLayers = BackgroundAssets.SelectRandomBackgroundAssetLayers(rng, bgLayers);
      this.FgLayer = BackgroundAssets.SelectRandomForegroundAssetLayer(rng, (IEnumerable<string>) stringList.ToArray());
    }
  }

  public IEnumerable<string> AssetPaths
  {
    get
    {
      return ((IEnumerable<string>) new string[2]
      {
        this.BackgroundScenePath,
        this.FgLayer ?? string.Empty
      }).Concat<string>((IEnumerable<string>) this.BgLayers).Where<string>((Func<string, bool>) (s => !string.IsNullOrWhiteSpace(s)));
    }
  }

  private static List<string> SelectRandomBackgroundAssetLayers(
    Rng rng,
    Dictionary<string, List<string>> bgLayers)
  {
    List<string> stringList1 = new List<string>();
    foreach (KeyValuePair<string, List<string>> keyValuePair in (IEnumerable<KeyValuePair<string, List<string>>>) bgLayers.OrderBy<KeyValuePair<string, List<string>>, string>((Func<KeyValuePair<string, List<string>>, string>) (kv => kv.Key)))
    {
      string str1;
      List<string> stringList2;
      keyValuePair.Deconstruct(ref str1, ref stringList2);
      List<string> items = stringList2;
      string str2 = rng.NextItem<string>((IEnumerable<string>) items);
      stringList1.Add(str2);
    }
    return stringList1;
  }

  private static string? SelectRandomForegroundAssetLayer(Rng rng, IEnumerable<string> fgLayer)
  {
    return rng.NextItem<string>(fgLayer);
  }
}
