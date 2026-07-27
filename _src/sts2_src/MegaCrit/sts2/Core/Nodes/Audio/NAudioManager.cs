// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Audio.NAudioManager
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Audio;

[ScriptPath("res://src/Core/Nodes/Audio/NAudioManager.cs")]
public class NAudioManager : Node
{
  private static readonly StringName _setBgmVolume = new StringName("set_bgm_volume");
  private static readonly StringName _setAmbienceVolume = new StringName("set_ambience_volume");
  private static readonly StringName _setSfxVolume = new StringName("set_sfx_volume");
  private static readonly StringName _setMasterVolume = new StringName("set_master_volume");
  private static readonly StringName _stopMusic = new StringName("stop_music");
  private static readonly StringName _playMusic = new StringName("play_music");
  private static readonly StringName _playOneShot = new StringName("play_one_shot");
  private static readonly StringName _stopAllLoops = new StringName("stop_all_loops");
  private static readonly StringName _setParam = new StringName("set_param");
  private static readonly StringName _stopLoop = new StringName("stop_loop");
  private static readonly StringName _playLoop = new StringName("play_loop");
  private static readonly StringName _updateMusicParameterCallback = new StringName("update_music_parameter");
  private Node _audioNode;

  public static NAudioManager? Instance => NGame.Instance?.AudioManager;

  public override void _EnterTree()
  {
    this._audioNode = this.GetNode<Node>(NodePath.op_Implicit("Proxy"));
  }

  public void PlayLoop(string path, bool usesLoopParam)
  {
    if (TestMode.IsOn)
      return;
    ((GodotObject) this._audioNode).Call(NAudioManager._playLoop, new Variant[2]
    {
      Variant.op_Implicit(path),
      Variant.op_Implicit(usesLoopParam)
    });
  }

  public void StopLoop(string path)
  {
    if (TestMode.IsOn)
      return;
    ((GodotObject) this._audioNode).Call(NAudioManager._stopLoop, new Variant[1]
    {
      Variant.op_Implicit(path)
    });
  }

  public void SetParam(string path, string param, float value)
  {
    if (TestMode.IsOn)
      return;
    ((GodotObject) this._audioNode).Call(NAudioManager._setParam, new Variant[3]
    {
      Variant.op_Implicit(path),
      Variant.op_Implicit(param),
      Variant.op_Implicit(value)
    });
  }

  public void StopAllLoops()
  {
    if (TestMode.IsOn)
      return;
    ((GodotObject) this._audioNode).Call(NAudioManager._stopAllLoops, Array.Empty<Variant>());
  }

  public void PlayOneShot(string path, Dictionary<string, float> parameters, float volume = 1f)
  {
    if (TestMode.IsOn)
      return;
    Dictionary dictionary = new Dictionary();
    foreach (KeyValuePair<string, float> parameter in parameters)
      dictionary.Add(Variant.op_Implicit(parameter.Key), Variant.op_Implicit(parameter.Value));
    ((GodotObject) this._audioNode).Call(NAudioManager._playOneShot, new Variant[3]
    {
      Variant.op_Implicit(path),
      Variant.op_Implicit(dictionary),
      Variant.op_Implicit(volume)
    });
  }

  public void PlayOneShot(string path, float volume = 1f)
  {
    if (TestMode.IsOn)
      return;
    this.PlayOneShot(path, new Dictionary<string, float>(), volume);
  }

  public void PlayMusic(string music)
  {
    if (TestMode.IsOn)
      return;
    ((GodotObject) this._audioNode).Call(NAudioManager._playMusic, new Variant[1]
    {
      Variant.op_Implicit(music)
    });
  }

  public void UpdateMusicParameter(string parameter, string value)
  {
    if (NonInteractiveMode.IsActive)
      return;
    ((GodotObject) this._audioNode).Call(NAudioManager._updateMusicParameterCallback, new Variant[2]
    {
      Variant.op_Implicit(parameter),
      Variant.op_Implicit(value)
    });
  }

  public void StopMusic()
  {
    if (TestMode.IsOn)
      return;
    ((GodotObject) this._audioNode).Call(NAudioManager._stopMusic, Array.Empty<Variant>());
  }

  public void SetMasterVol(float volume)
  {
    if (TestMode.IsOn)
      return;
    ((GodotObject) this._audioNode).Call(NAudioManager._setMasterVolume, new Variant[1]
    {
      Variant.op_Implicit(Mathf.Pow(volume, 2f))
    });
  }

  public void SetSfxVol(float volume)
  {
    if (TestMode.IsOn)
      return;
    ((GodotObject) this._audioNode).Call(NAudioManager._setSfxVolume, new Variant[1]
    {
      Variant.op_Implicit(Mathf.Pow(volume, 2f))
    });
  }

  public void SetAmbienceVol(float volume)
  {
    if (TestMode.IsOn)
      return;
    ((GodotObject) this._audioNode).Call(NAudioManager._setAmbienceVolume, new Variant[1]
    {
      Variant.op_Implicit(Mathf.Pow(volume, 2f))
    });
  }

  public void SetBgmVol(float volume)
  {
    if (TestMode.IsOn)
      return;
    ((GodotObject) this._audioNode).Call(NAudioManager._setBgmVolume, new Variant[1]
    {
      Variant.op_Implicit(Mathf.Pow(volume, 2f))
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(13)
    {
      new MethodInfo(NAudioManager.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAudioManager.MethodName.PlayLoop, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("path"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("usesLoopParam"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NAudioManager.MethodName.StopLoop, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("path"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NAudioManager.MethodName.SetParam, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("path"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("param"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("value"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NAudioManager.MethodName.StopAllLoops, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAudioManager.MethodName.PlayOneShot, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("path"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("volume"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NAudioManager.MethodName.PlayMusic, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("music"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NAudioManager.MethodName.UpdateMusicParameter, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("parameter"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("value"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NAudioManager.MethodName.StopMusic, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAudioManager.MethodName.SetMasterVol, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("volume"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NAudioManager.MethodName.SetSfxVol, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("volume"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NAudioManager.MethodName.SetAmbienceVol, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("volume"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NAudioManager.MethodName.SetBgmVol, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("volume"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NAudioManager.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAudioManager.MethodName.PlayLoop) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.PlayLoop(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAudioManager.MethodName.StopLoop) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.StopLoop(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAudioManager.MethodName.SetParam) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      this.SetParam(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAudioManager.MethodName.StopAllLoops) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StopAllLoops();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAudioManager.MethodName.PlayOneShot) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.PlayOneShot(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAudioManager.MethodName.PlayMusic) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.PlayMusic(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAudioManager.MethodName.UpdateMusicParameter) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.UpdateMusicParameter(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAudioManager.MethodName.StopMusic) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StopMusic();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAudioManager.MethodName.SetMasterVol) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetMasterVol(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAudioManager.MethodName.SetSfxVol) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetSfxVol(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAudioManager.MethodName.SetAmbienceVol) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetAmbienceVol(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NAudioManager.MethodName.SetBgmVol) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.SetBgmVol(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NAudioManager.MethodName._EnterTree) || StringName.op_Equality(ref method, NAudioManager.MethodName.PlayLoop) || StringName.op_Equality(ref method, NAudioManager.MethodName.StopLoop) || StringName.op_Equality(ref method, NAudioManager.MethodName.SetParam) || StringName.op_Equality(ref method, NAudioManager.MethodName.StopAllLoops) || StringName.op_Equality(ref method, NAudioManager.MethodName.PlayOneShot) || StringName.op_Equality(ref method, NAudioManager.MethodName.PlayMusic) || StringName.op_Equality(ref method, NAudioManager.MethodName.UpdateMusicParameter) || StringName.op_Equality(ref method, NAudioManager.MethodName.StopMusic) || StringName.op_Equality(ref method, NAudioManager.MethodName.SetMasterVol) || StringName.op_Equality(ref method, NAudioManager.MethodName.SetSfxVol) || StringName.op_Equality(ref method, NAudioManager.MethodName.SetAmbienceVol) || StringName.op_Equality(ref method, NAudioManager.MethodName.SetBgmVol) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NAudioManager.PropertyName._audioNode))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._audioNode = VariantUtils.ConvertTo<Node>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NAudioManager.PropertyName._audioNode))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Node>(ref this._audioNode);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NAudioManager.PropertyName._audioNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NAudioManager.PropertyName._audioNode, Variant.From<Node>(ref this._audioNode));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(NAudioManager.PropertyName._audioNode, ref variant))
      return;
    this._audioNode = ((Variant) ref variant).As<Node>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName PlayLoop = StringName.op_Implicit(nameof (PlayLoop));
    public static readonly StringName StopLoop = StringName.op_Implicit(nameof (StopLoop));
    public static readonly StringName SetParam = StringName.op_Implicit(nameof (SetParam));
    public static readonly StringName StopAllLoops = StringName.op_Implicit(nameof (StopAllLoops));
    public static readonly StringName PlayOneShot = StringName.op_Implicit(nameof (PlayOneShot));
    public static readonly StringName PlayMusic = StringName.op_Implicit(nameof (PlayMusic));
    public static readonly StringName UpdateMusicParameter = StringName.op_Implicit(nameof (UpdateMusicParameter));
    public static readonly StringName StopMusic = StringName.op_Implicit(nameof (StopMusic));
    public static readonly StringName SetMasterVol = StringName.op_Implicit(nameof (SetMasterVol));
    public static readonly StringName SetSfxVol = StringName.op_Implicit(nameof (SetSfxVol));
    public static readonly StringName SetAmbienceVol = StringName.op_Implicit(nameof (SetAmbienceVol));
    public static readonly StringName SetBgmVol = StringName.op_Implicit(nameof (SetBgmVol));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _audioNode = StringName.op_Implicit(nameof (_audioNode));
  }

  public class SignalName : Node.SignalName
  {
  }
}
