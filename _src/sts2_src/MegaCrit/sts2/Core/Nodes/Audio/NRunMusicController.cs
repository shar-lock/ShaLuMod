// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Audio.NRunMusicController
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Audio;

[ScriptPath("res://src/Core/Nodes/Audio/NRunMusicController.cs")]
public class NRunMusicController : Node
{
  private static readonly StringName _stopAmbience = new StringName("stop_ambience");
  private static readonly StringName _stopMusic = new StringName("stop_music");
  private const string _musicProgressParameter = "Progress";
  private const string _updateGlobalParameterCallback = "update_global_parameter";
  private const string _updateMusicParameterCallback = "update_music_parameter";
  private const string _updateMusicCallback = "update_music";
  private const string _updateAmbienceCallback = "update_ambience";
  private const string _updateCampfireAmbienceCallback = "update_campfire_ambience";
  private const string _updateCustomTrack = "update_custom_track";
  private const string _loadActBankCallback = "load_act_bank";
  private const string _unloadActBanksCallback = "unload_act_banks";
  private const string _bgMusicRngName = "bg_music";
  private IRunState _runState = (IRunState) NullRunState.Instance;
  private Node _proxy;
  private string? _currentTrack;
  private string _currentAmbience;
  private string? _failedTrack;

  public static NRunMusicController? Instance => NRun.Instance?.RunMusicController;

  private NRunMusicController.MusicProgressTrack GetTrack(RoomType roomType)
  {
    if (roomType.IsCombatRoom() && !CombatManager.Instance.IsInProgress)
      return NRunMusicController.MusicProgressTrack.CombatEnd;
    switch (roomType)
    {
      case RoomType.Monster:
        return NRunMusicController.MusicProgressTrack.Enemy;
      case RoomType.Elite:
        return NRunMusicController.MusicProgressTrack.Elite;
      case RoomType.Boss:
        return NRunMusicController.MusicProgressTrack.Elite;
      case RoomType.Treasure:
        return NRunMusicController.MusicProgressTrack.Treasure;
      case RoomType.Shop:
        return NRunMusicController.MusicProgressTrack.Merchant;
      case RoomType.Event:
        return this._runState.CurrentRoom is EventRoom currentRoom && currentRoom.CanonicalEvent is AncientEventModel ? NRunMusicController.MusicProgressTrack.Init : NRunMusicController.MusicProgressTrack.Unknown;
      case RoomType.RestSite:
        return NRunMusicController.MusicProgressTrack.Rest;
      default:
        return NRunMusicController.MusicProgressTrack.Init;
    }
  }

  public override void _Ready() => this._proxy = this.GetNode<Node>(NodePath.op_Implicit("Proxy"));

  public override void _ExitTree() => this.StopMusic();

  public void SetRunState(IRunState runState) => this._runState = runState;

  public static NRunMusicController.MusicSelection? ResolveMusic(
    string? currentTrack,
    string[] options,
    string[] bankPaths,
    ulong seed)
  {
    if (options.Length == 0)
      return new NRunMusicController.MusicSelection?();
    int index = new Rng(seed, "bg_music").NextInt(0, options.Length);
    string option = options[index];
    return option == currentTrack ? new NRunMusicController.MusicSelection?() : new NRunMusicController.MusicSelection?(new NRunMusicController.MusicSelection(option, bankPaths[index]));
  }

  public void UpdateMusic()
  {
    if (NonInteractiveMode.IsActive)
      return;
    NRunMusicController.MusicSelection? nullable = NRunMusicController.ResolveMusic(this._currentTrack, this._runState.Act.BgMusicOptions, this._runState.Act.MusicBankPaths, this._runState.Rng.Seed);
    if (!nullable.HasValue)
      return;
    NRunMusicController.MusicSelection musicSelection = nullable.Value;
    if (musicSelection.Track == this._failedTrack)
      return;
    musicSelection = nullable.Value;
    string bankPath = musicSelection.BankPath;
    musicSelection = nullable.Value;
    string track = musicSelection.Track;
    if (!this.LoadActBank(bankPath, track))
    {
      musicSelection = nullable.Value;
      this._failedTrack = musicSelection.Track;
    }
    else
    {
      this._failedTrack = (string) null;
      musicSelection = nullable.Value;
      this._currentTrack = musicSelection.Track;
      ((GodotObject) this._proxy).Call(StringName.op_Implicit("update_music"), new Variant[1]
      {
        Variant.op_Implicit(this._currentTrack)
      });
      ((GodotObject) this._proxy).Call(StringName.op_Implicit("update_global_parameter"), new Variant[2]
      {
        Variant.op_Implicit("Progress"),
        Variant.op_Implicit(0)
      });
      this.UpdateAmbience();
    }
  }

  public void PlayCustomMusic(string customMusic)
  {
    if (NonInteractiveMode.IsActive)
      return;
    ((GodotObject) this._proxy).Call(NRunMusicController._stopMusic, Array.Empty<Variant>());
    ((GodotObject) this._proxy).Call(StringName.op_Implicit("update_music"), new Variant[1]
    {
      Variant.op_Implicit(customMusic)
    });
  }

  public void UpdateCustomTrack(string customTrack, float label)
  {
    if (NonInteractiveMode.IsActive || !RunManager.Instance.IsInProgress)
      return;
    ((GodotObject) this._proxy).Call(StringName.op_Implicit("update_custom_track"), new Variant[2]
    {
      Variant.op_Implicit(customTrack),
      Variant.op_Implicit(label)
    });
  }

  public void StopCustomMusic()
  {
    if (NonInteractiveMode.IsActive)
      return;
    ((GodotObject) this._proxy).Call(NRunMusicController._stopMusic, Array.Empty<Variant>());
    if (this._currentTrack == null)
      return;
    ((GodotObject) this._proxy).Call(StringName.op_Implicit("update_music"), new Variant[1]
    {
      Variant.op_Implicit(this._currentTrack)
    });
    ((GodotObject) this._proxy).Call(StringName.op_Implicit("update_global_parameter"), new Variant[2]
    {
      Variant.op_Implicit("Progress"),
      Variant.op_Implicit(7)
    });
  }

  public void UpdateAmbience()
  {
    if (NonInteractiveMode.IsActive)
      return;
    string ambientSfx = this._runState.Act.AmbientSfx;
    // ISSUE: explicit non-virtual call
    EncounterModel encounter = this._runState.CurrentRoom is CombatRoom currentRoom ? __nonvirtual (currentRoom.Encounter) : (EncounterModel) null;
    if (encounter != null && encounter.HasAmbientSfx)
      ambientSfx = encounter.AmbientSfx;
    if (!(this._currentAmbience != ambientSfx))
      return;
    this._currentAmbience = ambientSfx;
    ((GodotObject) this._proxy).Call(StringName.op_Implicit("update_ambience"), new Variant[1]
    {
      Variant.op_Implicit(this._currentAmbience)
    });
  }

  public void UpdateTrack()
  {
    if (NonInteractiveMode.IsActive)
      return;
    this.UpdateTrack("Progress", (float) this.GetTrack(this._runState.CurrentRoom.RoomType));
    if (!(this._runState.CurrentRoom is RestSiteRoom))
      return;
    ((GodotObject) this._proxy).Call(StringName.op_Implicit("update_campfire_ambience"), new Variant[1]
    {
      Variant.op_Implicit(0)
    });
  }

  private void UpdateTrack(string label, float trackIndex)
  {
    ((GodotObject) this._proxy).Call(StringName.op_Implicit("update_global_parameter"), new Variant[2]
    {
      Variant.op_Implicit(label),
      Variant.op_Implicit(trackIndex)
    });
  }

  public void UpdateMusicParameter(string label, float trackIndex)
  {
    if (NonInteractiveMode.IsActive)
      return;
    ((GodotObject) this._proxy).Call(StringName.op_Implicit("update_music_parameter"), new Variant[2]
    {
      Variant.op_Implicit(label),
      Variant.op_Implicit(trackIndex)
    });
  }

  public void ToggleMerchantTrack()
  {
    if (NonInteractiveMode.IsActive || this._runState.CurrentRoom == null)
      return;
    if (this._runState.CurrentRoom.RoomType != RoomType.Shop)
      throw new InvalidOperationException("You can only trigger the merchant transition in a merchant room");
    NMapScreen instance = NMapScreen.Instance;
    NRunMusicController.MusicProgressTrack musicProgressTrack = (instance != null ? (((CanvasItem) instance).IsVisible() ? 1 : 0) : 0) != 0 ? NRunMusicController.MusicProgressTrack.MerchantEnd : NRunMusicController.MusicProgressTrack.Merchant;
    ((GodotObject) this._proxy).Call(StringName.op_Implicit("update_global_parameter"), new Variant[2]
    {
      Variant.op_Implicit("Progress"),
      Variant.op_Implicit((int) musicProgressTrack)
    });
  }

  public void TriggerEliteSecondPhase()
  {
    if (NonInteractiveMode.IsActive)
      return;
    if (this._runState.CurrentRoom.RoomType != RoomType.Elite)
      throw new InvalidOperationException("You can only trigger the elite transition in an elite room");
    ((GodotObject) this._proxy).Call(StringName.op_Implicit("update_global_parameter"), new Variant[2]
    {
      Variant.op_Implicit("Progress"),
      Variant.op_Implicit(8)
    });
  }

  public void TriggerCampfireGoingOut()
  {
    if (NonInteractiveMode.IsActive)
      return;
    if (this._runState.CurrentRoom.RoomType != RoomType.RestSite)
      throw new InvalidOperationException("You can only trigger the rest site transition in a rest site room");
    ((GodotObject) this._proxy).Call(StringName.op_Implicit("update_campfire_ambience"), new Variant[1]
    {
      Variant.op_Implicit(1)
    });
  }

  public void StopMusic()
  {
    if (NonInteractiveMode.IsActive)
      return;
    ((GodotObject) this._proxy).Call(StringName.op_Implicit("update_global_parameter"), new Variant[2]
    {
      Variant.op_Implicit("Progress"),
      Variant.op_Implicit(0)
    });
    ((GodotObject) this._proxy).Call(NRunMusicController._stopMusic, Array.Empty<Variant>());
    ((GodotObject) this._proxy).Call(NRunMusicController._stopAmbience, Array.Empty<Variant>());
    this._currentTrack = (string) null;
    this._failedTrack = (string) null;
    this.UnloadActBanks();
  }

  private bool LoadActBank(string bankPath, string verifyEvent)
  {
    return ActBankLoadRetry.Run(bankPath, (Func<bool>) (() =>
    {
      Variant variant = ((GodotObject) this._proxy).Call(StringName.op_Implicit("load_act_bank"), new Variant[2]
      {
        Variant.op_Implicit(bankPath),
        Variant.op_Implicit(verifyEvent)
      });
      return ((Variant) ref variant).AsBool();
    }));
  }

  private void UnloadActBanks()
  {
    ((GodotObject) this._proxy).Call(StringName.op_Implicit("unload_act_banks"), Array.Empty<Variant>());
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(17)
    {
      new MethodInfo(NRunMusicController.MethodName.GetTrack, new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("roomType"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NRunMusicController.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRunMusicController.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRunMusicController.MethodName.UpdateMusic, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRunMusicController.MethodName.PlayCustomMusic, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("customMusic"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NRunMusicController.MethodName.UpdateCustomTrack, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("customTrack"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("label"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NRunMusicController.MethodName.StopCustomMusic, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRunMusicController.MethodName.UpdateAmbience, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRunMusicController.MethodName.UpdateTrack, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRunMusicController.MethodName.UpdateTrack, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("label"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("trackIndex"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NRunMusicController.MethodName.UpdateMusicParameter, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("label"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("trackIndex"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NRunMusicController.MethodName.ToggleMerchantTrack, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRunMusicController.MethodName.TriggerEliteSecondPhase, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRunMusicController.MethodName.TriggerCampfireGoingOut, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRunMusicController.MethodName.StopMusic, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRunMusicController.MethodName.LoadActBank, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("bankPath"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("verifyEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NRunMusicController.MethodName.UnloadActBanks, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NRunMusicController.MethodName.GetTrack) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NRunMusicController.MusicProgressTrack track = this.GetTrack(VariantUtils.ConvertTo<RoomType>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NRunMusicController.MusicProgressTrack>(ref track);
      return true;
    }
    if (StringName.op_Equality(ref method, NRunMusicController.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRunMusicController.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRunMusicController.MethodName.UpdateMusic) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateMusic();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRunMusicController.MethodName.PlayCustomMusic) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.PlayCustomMusic(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRunMusicController.MethodName.UpdateCustomTrack) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.UpdateCustomTrack(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRunMusicController.MethodName.StopCustomMusic) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StopCustomMusic();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRunMusicController.MethodName.UpdateAmbience) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateAmbience();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRunMusicController.MethodName.UpdateTrack) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateTrack();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRunMusicController.MethodName.UpdateTrack) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.UpdateTrack(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRunMusicController.MethodName.UpdateMusicParameter) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.UpdateMusicParameter(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRunMusicController.MethodName.ToggleMerchantTrack) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ToggleMerchantTrack();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRunMusicController.MethodName.TriggerEliteSecondPhase) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TriggerEliteSecondPhase();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRunMusicController.MethodName.TriggerCampfireGoingOut) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TriggerCampfireGoingOut();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRunMusicController.MethodName.StopMusic) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StopMusic();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRunMusicController.MethodName.LoadActBank) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      bool flag = this.LoadActBank(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    if (!StringName.op_Equality(ref method, NRunMusicController.MethodName.UnloadActBanks) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.UnloadActBanks();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NRunMusicController.MethodName.GetTrack) || StringName.op_Equality(ref method, NRunMusicController.MethodName._Ready) || StringName.op_Equality(ref method, NRunMusicController.MethodName._ExitTree) || StringName.op_Equality(ref method, NRunMusicController.MethodName.UpdateMusic) || StringName.op_Equality(ref method, NRunMusicController.MethodName.PlayCustomMusic) || StringName.op_Equality(ref method, NRunMusicController.MethodName.UpdateCustomTrack) || StringName.op_Equality(ref method, NRunMusicController.MethodName.StopCustomMusic) || StringName.op_Equality(ref method, NRunMusicController.MethodName.UpdateAmbience) || StringName.op_Equality(ref method, NRunMusicController.MethodName.UpdateTrack) || StringName.op_Equality(ref method, NRunMusicController.MethodName.UpdateMusicParameter) || StringName.op_Equality(ref method, NRunMusicController.MethodName.ToggleMerchantTrack) || StringName.op_Equality(ref method, NRunMusicController.MethodName.TriggerEliteSecondPhase) || StringName.op_Equality(ref method, NRunMusicController.MethodName.TriggerCampfireGoingOut) || StringName.op_Equality(ref method, NRunMusicController.MethodName.StopMusic) || StringName.op_Equality(ref method, NRunMusicController.MethodName.LoadActBank) || StringName.op_Equality(ref method, NRunMusicController.MethodName.UnloadActBanks) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRunMusicController.PropertyName._proxy))
    {
      this._proxy = VariantUtils.ConvertTo<Node>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunMusicController.PropertyName._currentTrack))
    {
      this._currentTrack = VariantUtils.ConvertTo<string>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunMusicController.PropertyName._currentAmbience))
    {
      this._currentAmbience = VariantUtils.ConvertTo<string>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRunMusicController.PropertyName._failedTrack))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._failedTrack = VariantUtils.ConvertTo<string>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRunMusicController.PropertyName._proxy))
    {
      value = VariantUtils.CreateFrom<Node>(ref this._proxy);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunMusicController.PropertyName._currentTrack))
    {
      value = VariantUtils.CreateFrom<string>(ref this._currentTrack);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunMusicController.PropertyName._currentAmbience))
    {
      value = VariantUtils.CreateFrom<string>(ref this._currentAmbience);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRunMusicController.PropertyName._failedTrack))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<string>(ref this._failedTrack);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NRunMusicController.PropertyName._proxy, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, NRunMusicController.PropertyName._currentTrack, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, NRunMusicController.PropertyName._currentAmbience, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, NRunMusicController.PropertyName._failedTrack, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NRunMusicController.PropertyName._proxy, Variant.From<Node>(ref this._proxy));
    info.AddProperty(NRunMusicController.PropertyName._currentTrack, Variant.From<string>(ref this._currentTrack));
    info.AddProperty(NRunMusicController.PropertyName._currentAmbience, Variant.From<string>(ref this._currentAmbience));
    info.AddProperty(NRunMusicController.PropertyName._failedTrack, Variant.From<string>(ref this._failedTrack));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NRunMusicController.PropertyName._proxy, ref variant1))
      this._proxy = ((Variant) ref variant1).As<Node>();
    Variant variant2;
    if (info.TryGetProperty(NRunMusicController.PropertyName._currentTrack, ref variant2))
      this._currentTrack = ((Variant) ref variant2).As<string>();
    Variant variant3;
    if (info.TryGetProperty(NRunMusicController.PropertyName._currentAmbience, ref variant3))
      this._currentAmbience = ((Variant) ref variant3).As<string>();
    Variant variant4;
    if (!info.TryGetProperty(NRunMusicController.PropertyName._failedTrack, ref variant4))
      return;
    this._failedTrack = ((Variant) ref variant4).As<string>();
  }

  private enum MusicProgressTrack
  {
    Init,
    Enemy,
    Merchant,
    Rest,
    Unknown,
    Treasure,
    Elite,
    CombatEnd,
    Elite2,
    MerchantEnd,
  }

  private enum CampfireState
  {
    On,
    Off,
  }

  public readonly record struct MusicSelection(
  #nullable enable
  string Track, string BankPath);

  public class MethodName : Node.MethodName
  {
    public static readonly 
    #nullable disable
    StringName GetTrack = StringName.op_Implicit(nameof (GetTrack));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName UpdateMusic = StringName.op_Implicit(nameof (UpdateMusic));
    public static readonly StringName PlayCustomMusic = StringName.op_Implicit(nameof (PlayCustomMusic));
    public static readonly StringName UpdateCustomTrack = StringName.op_Implicit(nameof (UpdateCustomTrack));
    public static readonly StringName StopCustomMusic = StringName.op_Implicit(nameof (StopCustomMusic));
    public static readonly StringName UpdateAmbience = StringName.op_Implicit(nameof (UpdateAmbience));
    public static readonly StringName UpdateTrack = StringName.op_Implicit(nameof (UpdateTrack));
    public static readonly StringName UpdateMusicParameter = StringName.op_Implicit(nameof (UpdateMusicParameter));
    public static readonly StringName ToggleMerchantTrack = StringName.op_Implicit(nameof (ToggleMerchantTrack));
    public static readonly StringName TriggerEliteSecondPhase = StringName.op_Implicit(nameof (TriggerEliteSecondPhase));
    public static readonly StringName TriggerCampfireGoingOut = StringName.op_Implicit(nameof (TriggerCampfireGoingOut));
    public static readonly StringName StopMusic = StringName.op_Implicit(nameof (StopMusic));
    public static readonly StringName LoadActBank = StringName.op_Implicit(nameof (LoadActBank));
    public static readonly StringName UnloadActBanks = StringName.op_Implicit(nameof (UnloadActBanks));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _proxy = StringName.op_Implicit(nameof (_proxy));
    public static readonly StringName _currentTrack = StringName.op_Implicit(nameof (_currentTrack));
    public static readonly StringName _currentAmbience = StringName.op_Implicit(nameof (_currentAmbience));
    public static readonly StringName _failedTrack = StringName.op_Implicit(nameof (_failedTrack));
  }

  public class SignalName : Node.SignalName
  {
  }
}
