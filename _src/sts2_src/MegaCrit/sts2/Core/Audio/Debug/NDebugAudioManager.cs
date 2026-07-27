// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Audio.Debug.NDebugAudioManager
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Audio.Debug;

[ScriptPath("res://src/Core/Audio/Debug/NDebugAudioManager.cs")]
public class NDebugAudioManager : Node
{
  private static readonly StringName _sfx = new StringName("SFX");
  private static readonly StringName _master = new StringName("Master");
  private List<AudioStreamPlayer> _freeAudioPlayers = new List<AudioStreamPlayer>();
  private readonly List<NDebugAudioManager.PlayingSound> _playingSounds = new List<NDebugAudioManager.PlayingSound>();
  private int _nextId;

  public static NDebugAudioManager? Instance => NGame.Instance?.DebugAudio;

  public override void _Ready()
  {
    this._freeAudioPlayers.AddRange(((IEnumerable) this.GetChildren(false)).OfType<AudioStreamPlayer>());
  }

  public int Play(string streamName, float volume = 1f, PitchVariance variance = PitchVariance.None)
  {
    AudioStreamPlayer streamPlayer;
    if (this._freeAudioPlayers.Count > 0)
    {
      List<AudioStreamPlayer> freeAudioPlayers = this._freeAudioPlayers;
      streamPlayer = freeAudioPlayers[freeAudioPlayers.Count - 1];
      this._freeAudioPlayers.RemoveAt(this._freeAudioPlayers.Count - 1);
    }
    else
    {
      streamPlayer = new AudioStreamPlayer();
      this.AddChildSafely((Node) streamPlayer);
    }
    AudioStream asset = PreloadManager.Cache.GetAsset<AudioStream>(TmpSfx.GetPath(streamName));
    ((Node) streamPlayer).Name = StringName.op_Implicit("StreamPlayer-" + streamName);
    streamPlayer.Stream = asset;
    streamPlayer.VolumeLinear = volume;
    streamPlayer.PitchScale = this.GetRandomPitchScale(variance);
    streamPlayer.Bus = NDebugAudioManager._sfx;
    Callable callable = Callable.From((Action) (() => this.PlayerFinished(streamPlayer)));
    ((GodotObject) streamPlayer).Connect(AudioStreamPlayer.SignalName.Finished, callable, 0U);
    streamPlayer.Play(0.0f);
    NDebugAudioManager.PlayingSound playingSound = new NDebugAudioManager.PlayingSound()
    {
      id = this._nextId,
      player = streamPlayer,
      callable = callable
    };
    this._playingSounds.Add(playingSound);
    ++this._nextId;
    return playingSound.id;
  }

  public void StopAll()
  {
    foreach (NDebugAudioManager.PlayingSound playingSound in this._playingSounds.ToList<NDebugAudioManager.PlayingSound>())
      this.Stop(playingSound.id, 0.0f);
  }

  public void Stop(int id, float fadeTime = 0.5f)
  {
    for (int index = 0; index < this._playingSounds.Count; ++index)
    {
      NDebugAudioManager.PlayingSound playingSound = this._playingSounds[index];
      if (playingSound.id == id)
      {
        if ((double) fadeTime > 0.0)
        {
          Tween tween = this.CreateTween();
          tween.TweenProperty((GodotObject) playingSound.player, NodePath.op_Implicit("volume_linear"), Variant.op_Implicit(0.0f), (double) fadeTime);
          tween.TweenCallback(Callable.From((Action) (() => this.StopInternalById(id))));
          return;
        }
        this.StopInternal(index);
        return;
      }
    }
    Log.Warn($"Tried to stop sound with ID {id} but no sound with that ID was found!");
  }

  private void StopInternalById(int id)
  {
    for (int index = 0; index < this._playingSounds.Count; ++index)
    {
      if (this._playingSounds[index].id == id)
      {
        this.StopInternal(index);
        break;
      }
    }
  }

  private void StopInternal(int soundIndex)
  {
    NDebugAudioManager.PlayingSound playingSound = this._playingSounds[soundIndex];
    if (playingSound.player.IsPlaying())
      playingSound.player.Stop();
    ((GodotObject) playingSound.player).Disconnect(AudioStreamPlayer.SignalName.Finished, playingSound.callable);
    this._playingSounds.RemoveAt(soundIndex);
    this._freeAudioPlayers.Add(playingSound.player);
  }

  public void SetMasterAudioVolume(float linearVolume)
  {
    AudioServer.Singleton.SetBusVolumeDb(AudioServer.Singleton.GetBusIndex(NDebugAudioManager._master), Mathf.LinearToDb(Mathf.Pow(linearVolume, 2f)));
  }

  public void SetSfxAudioVolume(float linearVolume)
  {
    AudioServer.Singleton.SetBusVolumeDb(AudioServer.Singleton.GetBusIndex(NDebugAudioManager._sfx), Mathf.LinearToDb(Mathf.Pow(linearVolume, 2f)));
  }

  private void PlayerFinished(AudioStreamPlayer player)
  {
    for (int index = 0; index < this._playingSounds.Count; ++index)
    {
      if (this._playingSounds[index].player == player)
      {
        this.StopInternal(index);
        break;
      }
    }
  }

  private float GetRandomPitchScale(PitchVariance variance)
  {
    float num;
    switch (variance)
    {
      case PitchVariance.None:
        num = 0.0f;
        break;
      case PitchVariance.Small:
        num = 0.02f;
        break;
      case PitchVariance.Medium:
        num = 0.05f;
        break;
      case PitchVariance.Large:
        num = 0.1f;
        break;
      case PitchVariance.TooMuch:
        num = 0.2f;
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof (variance), (object) variance, (string) null);
    }
    float max = num;
    return (double) max == 0.0 ? 1f : 1f + Rng.Chaotic.NextFloat(-max, max);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(10)
    {
      new MethodInfo(NDebugAudioManager.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDebugAudioManager.MethodName.Play, new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("streamName"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("volume"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("variance"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NDebugAudioManager.MethodName.StopAll, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDebugAudioManager.MethodName.Stop, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("id"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("fadeTime"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NDebugAudioManager.MethodName.StopInternalById, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("id"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NDebugAudioManager.MethodName.StopInternal, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("soundIndex"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NDebugAudioManager.MethodName.SetMasterAudioVolume, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("linearVolume"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NDebugAudioManager.MethodName.SetSfxAudioVolume, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("linearVolume"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NDebugAudioManager.MethodName.PlayerFinished, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("player"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("AudioStreamPlayer"), false)
      }, (List<Variant>) null),
      new MethodInfo(NDebugAudioManager.MethodName.GetRandomPitchScale, new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("variance"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NDebugAudioManager.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDebugAudioManager.MethodName.Play) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      int num = this.Play(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<PitchVariance>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = VariantUtils.CreateFrom<int>(ref num);
      return true;
    }
    if (StringName.op_Equality(ref method, NDebugAudioManager.MethodName.StopAll) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StopAll();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDebugAudioManager.MethodName.Stop) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.Stop(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDebugAudioManager.MethodName.StopInternalById) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.StopInternalById(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDebugAudioManager.MethodName.StopInternal) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.StopInternal(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDebugAudioManager.MethodName.SetMasterAudioVolume) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetMasterAudioVolume(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDebugAudioManager.MethodName.SetSfxAudioVolume) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetSfxAudioVolume(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDebugAudioManager.MethodName.PlayerFinished) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.PlayerFinished(VariantUtils.ConvertTo<AudioStreamPlayer>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NDebugAudioManager.MethodName.GetRandomPitchScale) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    float randomPitchScale = this.GetRandomPitchScale(VariantUtils.ConvertTo<PitchVariance>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = VariantUtils.CreateFrom<float>(ref randomPitchScale);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NDebugAudioManager.MethodName._Ready) || StringName.op_Equality(ref method, NDebugAudioManager.MethodName.Play) || StringName.op_Equality(ref method, NDebugAudioManager.MethodName.StopAll) || StringName.op_Equality(ref method, NDebugAudioManager.MethodName.Stop) || StringName.op_Equality(ref method, NDebugAudioManager.MethodName.StopInternalById) || StringName.op_Equality(ref method, NDebugAudioManager.MethodName.StopInternal) || StringName.op_Equality(ref method, NDebugAudioManager.MethodName.SetMasterAudioVolume) || StringName.op_Equality(ref method, NDebugAudioManager.MethodName.SetSfxAudioVolume) || StringName.op_Equality(ref method, NDebugAudioManager.MethodName.PlayerFinished) || StringName.op_Equality(ref method, NDebugAudioManager.MethodName.GetRandomPitchScale) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NDebugAudioManager.PropertyName._nextId))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._nextId = VariantUtils.ConvertTo<int>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NDebugAudioManager.PropertyName._nextId))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<int>(ref this._nextId);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 2L, NDebugAudioManager.PropertyName._nextId, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NDebugAudioManager.PropertyName._nextId, Variant.From<int>(ref this._nextId));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(NDebugAudioManager.PropertyName._nextId, ref variant))
      return;
    this._nextId = ((Variant) ref variant).As<int>();
  }

  private struct PlayingSound
  {
    public int id;
    public 
    #nullable enable
    AudioStreamPlayer player;
    public Callable callable;
  }

  public class MethodName : Node.MethodName
  {
    public static readonly 
    #nullable disable
    StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName Play = StringName.op_Implicit(nameof (Play));
    public static readonly StringName StopAll = StringName.op_Implicit(nameof (StopAll));
    public static readonly StringName Stop = StringName.op_Implicit(nameof (Stop));
    public static readonly StringName StopInternalById = StringName.op_Implicit(nameof (StopInternalById));
    public static readonly StringName StopInternal = StringName.op_Implicit(nameof (StopInternal));
    public static readonly StringName SetMasterAudioVolume = StringName.op_Implicit(nameof (SetMasterAudioVolume));
    public static readonly StringName SetSfxAudioVolume = StringName.op_Implicit(nameof (SetSfxAudioVolume));
    public static readonly StringName PlayerFinished = StringName.op_Implicit(nameof (PlayerFinished));
    public static readonly StringName GetRandomPitchScale = StringName.op_Implicit(nameof (GetRandomPitchScale));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _nextId = StringName.op_Implicit(nameof (_nextId));
  }

  public class SignalName : Node.SignalName
  {
  }
}
