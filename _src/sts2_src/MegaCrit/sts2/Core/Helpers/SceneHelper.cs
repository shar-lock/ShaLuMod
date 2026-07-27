// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Helpers.SceneHelper
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;

#nullable enable
namespace MegaCrit.Sts2.Core.Helpers;

public static class SceneHelper
{
  public static string GetScenePath(string innerPath)
  {
    if (innerPath.StartsWith('/'))
    {
      string str = innerPath;
      innerPath = str.Substring(1, str.Length - 1);
    }
    return $"res://scenes/{innerPath}.tscn";
  }

  private static PackedScene Load(string innerPath)
  {
    return ResourceLoader.Load<PackedScene>(SceneHelper.GetScenePath(innerPath), (string) null, (ResourceLoader.CacheMode) 1L);
  }

  public static T Instantiate<T>(string innerPath) where T : Node
  {
    return SceneHelper.Load(innerPath).Instantiate<T>((PackedScene.GenEditState) 0L);
  }
}
