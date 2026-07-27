// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Assets.AtlasResourceLoader
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;

#nullable enable
namespace MegaCrit.Sts2.Core.Assets;

[ScriptPath("res://src/Core/Assets/AtlasResourceLoader.cs")]
public class AtlasResourceLoader : ResourceFormatLoader
{
  private const string _atlasBasePath = "res://images/atlases/";
  private const string _spritesSuffix = ".sprites/";
  private static readonly StringName _typeAtlasTexture = new StringName("AtlasTexture");
  private static readonly StringName _typeTexture2D = new StringName("Texture2D");
  private static readonly StringName _typeResource = new StringName("Resource");
  private static readonly Regex _pathPattern = new Regex("^res://images/atlases/([^/]+)\\.sprites/(.+)\\.tres$", RegexOptions.Compiled);

  public override string[] _GetRecognizedExtensions()
  {
    return new string[1]{ "tres" };
  }

  public override bool _HandlesType(StringName type)
  {
    return StringName.op_Equality(type, AtlasResourceLoader._typeAtlasTexture) || StringName.op_Equality(type, AtlasResourceLoader._typeTexture2D) || StringName.op_Equality(type, AtlasResourceLoader._typeResource);
  }

  public override string _GetResourceType(string path)
  {
    return AtlasResourceLoader.IsSpritePath(path) ? "AtlasTexture" : "";
  }

  public override bool _RecognizePath(string path, StringName type)
  {
    return AtlasResourceLoader.IsSpritePath(path);
  }

  public override bool _Exists(string path)
  {
    if (!AtlasResourceLoader.IsSpritePath(path))
      return false;
    (string str1, string str2) = AtlasResourceLoader.ParsePath(path);
    if (str1 == null || str2 == null)
      return false;
    if (!AtlasManager.IsAtlasLoaded(str1))
      AtlasManager.LoadAtlas(str1);
    return AtlasManager.HasSprite(str1, str2) || AtlasResourceLoader.HasFallback(str1, str2);
  }

  public override Variant _Load(
    string path,
    string originalPath,
    bool useSubThreads,
    int cacheMode)
  {
    if (!AtlasResourceLoader.IsSpritePath(path))
      return new Variant();
    (string str1, string str2) = AtlasResourceLoader.ParsePath(path);
    if (str1 == null || str2 == null)
    {
      Log.Warn("AtlasResourceLoader: Failed to parse path: " + path);
      return Variant.op_Implicit(7L);
    }
    if (!AtlasManager.IsAtlasLoaded(str1))
      AtlasManager.LoadAtlas(str1);
    AtlasTexture sprite = AtlasManager.GetSprite(str1, str2);
    if (sprite != null)
      return Variant.op_Implicit((GodotObject) sprite);
    Texture2D texture2D = AtlasResourceLoader.LoadFallback(str1, str2);
    if (texture2D != null)
      return Variant.op_Implicit((GodotObject) texture2D);
    if (!str2.StartsWith("mock_"))
      Log.Warn($"AtlasResourceLoader: Missing sprite '{str2}' in {str1} (requested: {path})");
    return AtlasResourceLoader.GetMissingTexture(str1);
  }

  public override string[] _GetDependencies(string path, bool addTypes) => Array.Empty<string>();

  private static bool IsSpritePath(string path)
  {
    return path.StartsWith("res://images/atlases/") && path.Contains(".sprites/") && path.EndsWith(".tres");
  }

  public static (string? AtlasName, string? SpriteName) ParsePath(string path)
  {
    Match match = AtlasResourceLoader._pathPattern.Match(path);
    return !match.Success ? ((string) null, (string) null) : (match.Groups[1].Value, match.Groups[2].Value);
  }

  private static bool HasFallback(string atlasName, string spriteName)
  {
    string fallbackPath = AtlasResourceLoader.GetFallbackPath(atlasName, spriteName);
    return fallbackPath != null && ResourceLoader.Exists(fallbackPath, "");
  }

  private static Texture2D? LoadFallback(string atlasName, string spriteName)
  {
    string fallbackPath = AtlasResourceLoader.GetFallbackPath(atlasName, spriteName);
    if (fallbackPath == null)
      return (Texture2D) null;
    if (!ResourceLoader.Exists(fallbackPath, ""))
      return (Texture2D) null;
    Log.Debug($"AtlasResourceLoader: Using fallback for {atlasName}/{spriteName}: {fallbackPath}");
    return ResourceLoader.Load<Texture2D>(fallbackPath, (string) null, (ResourceLoader.CacheMode) 1L);
  }

  private static string? GetFallbackPath(string atlasName, string spriteName)
  {
    string fallbackPath;
    switch (atlasName)
    {
      case "relic_atlas":
      case "relic_outline_atlas":
        fallbackPath = AtlasResourceLoader.GetRelicFallbackPath(spriteName);
        break;
      case "power_atlas":
        fallbackPath = AtlasResourceLoader.GetPowerFallbackPath(spriteName);
        break;
      case "card_atlas":
        fallbackPath = AtlasResourceLoader.GetCardFallbackPath(spriteName);
        break;
      case "potion_atlas":
      case "potion_outline_atlas":
        fallbackPath = AtlasResourceLoader.GetPotionFallbackPath(spriteName);
        break;
      default:
        fallbackPath = (string) null;
        break;
    }
    return fallbackPath;
  }

  private static string? GetRelicFallbackPath(string spriteName)
  {
    string relicFallbackPath = $"res://images/relics/{spriteName}.png";
    if (ResourceLoader.Exists(relicFallbackPath, ""))
      return relicFallbackPath;
    string str = $"res://images/relics/beta/{spriteName}.png";
    return ResourceLoader.Exists(str, "") ? str : (string) null;
  }

  private static string? GetPowerFallbackPath(string spriteName)
  {
    string powerFallbackPath = $"res://images/powers/{spriteName}.png";
    if (ResourceLoader.Exists(powerFallbackPath, ""))
      return powerFallbackPath;
    string str = $"res://images/powers/beta/{spriteName}.png";
    return ResourceLoader.Exists(str, "") ? str : (string) null;
  }

  private static string? GetCardFallbackPath(string spriteName)
  {
    string cardFallbackPath1 = $"res://images/packed/card_portraits/{spriteName}.png";
    if (ResourceLoader.Exists(cardFallbackPath1, ""))
      return cardFallbackPath1;
    int length = spriteName.LastIndexOf('/');
    if (length > 0)
    {
      string str1 = spriteName.Substring(0, length);
      string str2 = spriteName;
      int startIndex = length + 1;
      string str3 = str2.Substring(startIndex, str2.Length - startIndex);
      string cardFallbackPath2 = $"res://images/packed/card_portraits/{str1}/beta/{str3}.png";
      if (ResourceLoader.Exists(cardFallbackPath2, ""))
        return cardFallbackPath2;
    }
    return (string) null;
  }

  private static string? GetPotionFallbackPath(string spriteName)
  {
    string str = $"res://images/potions/{spriteName}.png";
    return ResourceLoader.Exists(str, "") ? str : (string) null;
  }

  private static Variant GetMissingTexture(string atlasName)
  {
    string str1;
    switch (atlasName)
    {
      case "card_atlas":
        str1 = "res://images/packed/card_portraits/beta.png";
        break;
      case "potion_atlas":
      case "potion_outline_atlas":
        str1 = "res://images/potions/missing_potion.png";
        break;
      default:
        str1 = "res://images/powers/missing_power.png";
        break;
    }
    string str2 = str1;
    if (ResourceLoader.Exists(str2, ""))
    {
      Texture2D texture2D = ResourceLoader.Load<Texture2D>(str2, (string) null, (ResourceLoader.CacheMode) 1L);
      if (texture2D != null)
        return Variant.op_Implicit((GodotObject) texture2D);
    }
    return Variant.op_Implicit(7L);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(16 /*0x10*/)
    {
      new MethodInfo(AtlasResourceLoader.MethodName._GetRecognizedExtensions, new PropertyInfo((Variant.Type) 34L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(AtlasResourceLoader.MethodName._HandlesType, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 21L, StringName.op_Implicit("type"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(AtlasResourceLoader.MethodName._GetResourceType, new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("path"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(AtlasResourceLoader.MethodName._RecognizePath, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("path"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 21L, StringName.op_Implicit("type"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(AtlasResourceLoader.MethodName._Exists, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("path"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(AtlasResourceLoader.MethodName._Load, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 131078L /*0x020006*/, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("path"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("originalPath"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("useSubThreads"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("cacheMode"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(AtlasResourceLoader.MethodName._GetDependencies, new PropertyInfo((Variant.Type) 34L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("path"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("addTypes"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(AtlasResourceLoader.MethodName.IsSpritePath, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("path"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(AtlasResourceLoader.MethodName.HasFallback, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("atlasName"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("spriteName"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(AtlasResourceLoader.MethodName.LoadFallback, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Texture2D"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("atlasName"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("spriteName"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(AtlasResourceLoader.MethodName.GetFallbackPath, new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("atlasName"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("spriteName"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(AtlasResourceLoader.MethodName.GetRelicFallbackPath, new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("spriteName"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(AtlasResourceLoader.MethodName.GetPowerFallbackPath, new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("spriteName"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(AtlasResourceLoader.MethodName.GetCardFallbackPath, new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("spriteName"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(AtlasResourceLoader.MethodName.GetPotionFallbackPath, new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("spriteName"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(AtlasResourceLoader.MethodName.GetMissingTexture, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 131078L /*0x020006*/, false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("atlasName"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, AtlasResourceLoader.MethodName._GetRecognizedExtensions) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      string[] recognizedExtensions = base._GetRecognizedExtensions();
      ret = VariantUtils.CreateFrom<string[]>(ref recognizedExtensions);
      return true;
    }
    if (StringName.op_Equality(ref method, AtlasResourceLoader.MethodName._HandlesType) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      bool flag = base._HandlesType(VariantUtils.ConvertTo<StringName>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    if (StringName.op_Equality(ref method, AtlasResourceLoader.MethodName._GetResourceType) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string resourceType = base._GetResourceType(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref resourceType);
      return true;
    }
    if (StringName.op_Equality(ref method, AtlasResourceLoader.MethodName._RecognizePath) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      bool flag = base._RecognizePath(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<StringName>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    if (StringName.op_Equality(ref method, AtlasResourceLoader.MethodName._Exists) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      bool flag = base._Exists(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    if (StringName.op_Equality(ref method, AtlasResourceLoader.MethodName._Load) && ((NativeVariantPtrArgs) ref args).Count == 4)
    {
      Variant variant = base._Load(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[3]));
      ret = VariantUtils.CreateFrom<Variant>(ref variant);
      return true;
    }
    if (StringName.op_Equality(ref method, AtlasResourceLoader.MethodName._GetDependencies) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      string[] dependencies = base._GetDependencies(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<string[]>(ref dependencies);
      return true;
    }
    if (StringName.op_Equality(ref method, AtlasResourceLoader.MethodName.IsSpritePath) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      bool flag = AtlasResourceLoader.IsSpritePath(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    if (StringName.op_Equality(ref method, AtlasResourceLoader.MethodName.HasFallback) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      bool flag = AtlasResourceLoader.HasFallback(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    if (StringName.op_Equality(ref method, AtlasResourceLoader.MethodName.LoadFallback) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      Texture2D texture2D = AtlasResourceLoader.LoadFallback(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<Texture2D>(ref texture2D);
      return true;
    }
    if (StringName.op_Equality(ref method, AtlasResourceLoader.MethodName.GetFallbackPath) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      string fallbackPath = AtlasResourceLoader.GetFallbackPath(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<string>(ref fallbackPath);
      return true;
    }
    if (StringName.op_Equality(ref method, AtlasResourceLoader.MethodName.GetRelicFallbackPath) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string relicFallbackPath = AtlasResourceLoader.GetRelicFallbackPath(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref relicFallbackPath);
      return true;
    }
    if (StringName.op_Equality(ref method, AtlasResourceLoader.MethodName.GetPowerFallbackPath) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string powerFallbackPath = AtlasResourceLoader.GetPowerFallbackPath(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref powerFallbackPath);
      return true;
    }
    if (StringName.op_Equality(ref method, AtlasResourceLoader.MethodName.GetCardFallbackPath) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string cardFallbackPath = AtlasResourceLoader.GetCardFallbackPath(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref cardFallbackPath);
      return true;
    }
    if (StringName.op_Equality(ref method, AtlasResourceLoader.MethodName.GetPotionFallbackPath) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string potionFallbackPath = AtlasResourceLoader.GetPotionFallbackPath(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref potionFallbackPath);
      return true;
    }
    if (!StringName.op_Equality(ref method, AtlasResourceLoader.MethodName.GetMissingTexture) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    Variant missingTexture = AtlasResourceLoader.GetMissingTexture(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = VariantUtils.CreateFrom<Variant>(ref missingTexture);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, AtlasResourceLoader.MethodName.IsSpritePath) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      bool flag = AtlasResourceLoader.IsSpritePath(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    if (StringName.op_Equality(ref method, AtlasResourceLoader.MethodName.HasFallback) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      bool flag = AtlasResourceLoader.HasFallback(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    if (StringName.op_Equality(ref method, AtlasResourceLoader.MethodName.LoadFallback) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      Texture2D texture2D = AtlasResourceLoader.LoadFallback(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<Texture2D>(ref texture2D);
      return true;
    }
    if (StringName.op_Equality(ref method, AtlasResourceLoader.MethodName.GetFallbackPath) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      string fallbackPath = AtlasResourceLoader.GetFallbackPath(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<string>(ref fallbackPath);
      return true;
    }
    if (StringName.op_Equality(ref method, AtlasResourceLoader.MethodName.GetRelicFallbackPath) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string relicFallbackPath = AtlasResourceLoader.GetRelicFallbackPath(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref relicFallbackPath);
      return true;
    }
    if (StringName.op_Equality(ref method, AtlasResourceLoader.MethodName.GetPowerFallbackPath) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string powerFallbackPath = AtlasResourceLoader.GetPowerFallbackPath(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref powerFallbackPath);
      return true;
    }
    if (StringName.op_Equality(ref method, AtlasResourceLoader.MethodName.GetCardFallbackPath) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string cardFallbackPath = AtlasResourceLoader.GetCardFallbackPath(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref cardFallbackPath);
      return true;
    }
    if (StringName.op_Equality(ref method, AtlasResourceLoader.MethodName.GetPotionFallbackPath) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string potionFallbackPath = AtlasResourceLoader.GetPotionFallbackPath(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref potionFallbackPath);
      return true;
    }
    if (StringName.op_Equality(ref method, AtlasResourceLoader.MethodName.GetMissingTexture) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      Variant missingTexture = AtlasResourceLoader.GetMissingTexture(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<Variant>(ref missingTexture);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, AtlasResourceLoader.MethodName._GetRecognizedExtensions) || StringName.op_Equality(ref method, AtlasResourceLoader.MethodName._HandlesType) || StringName.op_Equality(ref method, AtlasResourceLoader.MethodName._GetResourceType) || StringName.op_Equality(ref method, AtlasResourceLoader.MethodName._RecognizePath) || StringName.op_Equality(ref method, AtlasResourceLoader.MethodName._Exists) || StringName.op_Equality(ref method, AtlasResourceLoader.MethodName._Load) || StringName.op_Equality(ref method, AtlasResourceLoader.MethodName._GetDependencies) || StringName.op_Equality(ref method, AtlasResourceLoader.MethodName.IsSpritePath) || StringName.op_Equality(ref method, AtlasResourceLoader.MethodName.HasFallback) || StringName.op_Equality(ref method, AtlasResourceLoader.MethodName.LoadFallback) || StringName.op_Equality(ref method, AtlasResourceLoader.MethodName.GetFallbackPath) || StringName.op_Equality(ref method, AtlasResourceLoader.MethodName.GetRelicFallbackPath) || StringName.op_Equality(ref method, AtlasResourceLoader.MethodName.GetPowerFallbackPath) || StringName.op_Equality(ref method, AtlasResourceLoader.MethodName.GetCardFallbackPath) || StringName.op_Equality(ref method, AtlasResourceLoader.MethodName.GetPotionFallbackPath) || StringName.op_Equality(ref method, AtlasResourceLoader.MethodName.GetMissingTexture) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
  }

  public class MethodName : ResourceFormatLoader.MethodName
  {
    public static readonly StringName _GetRecognizedExtensions = StringName.op_Implicit(nameof (_GetRecognizedExtensions));
    public static readonly StringName _HandlesType = StringName.op_Implicit(nameof (_HandlesType));
    public static readonly StringName _GetResourceType = StringName.op_Implicit(nameof (_GetResourceType));
    public static readonly StringName _RecognizePath = StringName.op_Implicit(nameof (_RecognizePath));
    public static readonly StringName _Exists = StringName.op_Implicit(nameof (_Exists));
    public static readonly StringName _Load = StringName.op_Implicit(nameof (_Load));
    public static readonly StringName _GetDependencies = StringName.op_Implicit(nameof (_GetDependencies));
    public static readonly StringName IsSpritePath = StringName.op_Implicit(nameof (IsSpritePath));
    public static readonly StringName HasFallback = StringName.op_Implicit(nameof (HasFallback));
    public static readonly StringName LoadFallback = StringName.op_Implicit(nameof (LoadFallback));
    public static readonly StringName GetFallbackPath = StringName.op_Implicit(nameof (GetFallbackPath));
    public static readonly StringName GetRelicFallbackPath = StringName.op_Implicit(nameof (GetRelicFallbackPath));
    public static readonly StringName GetPowerFallbackPath = StringName.op_Implicit(nameof (GetPowerFallbackPath));
    public static readonly StringName GetCardFallbackPath = StringName.op_Implicit(nameof (GetCardFallbackPath));
    public static readonly StringName GetPotionFallbackPath = StringName.op_Implicit(nameof (GetPotionFallbackPath));
    public static readonly StringName GetMissingTexture = StringName.op_Implicit(nameof (GetMissingTexture));
  }

  public class PropertyName : ResourceFormatLoader.PropertyName
  {
  }

  public class SignalName : ResourceFormatLoader.SignalName
  {
  }
}
