// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.NMuteInBackgroundHandler
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes;

[ScriptPath("res://src/Core/Nodes/NMuteInBackgroundHandler.cs")]
public class NMuteInBackgroundHandler : Node
{
  private Tween? _tween;
  private ulong _lastFocusOutMsec;
  private bool _loggedEnvironment;

  public override void _Notification(int what)
  {
    switch (what)
    {
      case 1004:
        Log.Info($"MuteInBackground: FocusIn received [tween={this._tween != null}, msSinceFocusOut={Time.GetTicksMsec() - this._lastFocusOutMsec}, windowFocused={this.GetWindow().HasFocus()}]");
        this.Unmute();
        break;
      case 1005:
        if (!this._loggedEnvironment)
        {
          this._loggedEnvironment = true;
          Log.Info($"MuteInBackground: environment [displayServer={DisplayServer.GetName()}, os={OS.GetName()}]");
        }
        this._lastFocusOutMsec = Time.GetTicksMsec();
        Log.Info($"MuteInBackground: FocusOut received [tween={this._tween != null}, windowFocused={this.GetWindow().HasFocus()}]");
        this.Mute();
        break;
    }
  }

  private void Mute()
  {
    PrefsSave prefsSave = SaveManager.Instance.PrefsSave;
    SettingsSave settingsSave = SaveManager.Instance.SettingsSave;
    if (prefsSave != null && settingsSave != null && prefsSave.MuteInBackground)
    {
      bool flag = this._tween != null;
      this._tween?.Kill();
      this._tween = this.CreateTween();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this._tween.TweenMethod(Callable.From<float>(NMuteInBackgroundHandler.\u003C\u003EO.\u003C0\u003E__SetMasterVolume ?? (NMuteInBackgroundHandler.\u003C\u003EO.\u003C0\u003E__SetMasterVolume = new Action<float>(NMuteInBackgroundHandler.SetMasterVolume))), Variant.op_Implicit(settingsSave.VolumeMaster), Variant.op_Implicit(0.0f), 1.0);
      if (!flag)
        return;
      Log.Info("MuteInBackground: Muting (replaced existing tween)");
    }
    else
      Log.Info("MuteInBackground: FocusOut ignored (setting off or saves null)");
  }

  private void Unmute()
  {
    if (this._tween == null)
      return;
    this._tween?.Kill();
    this._tween = (Tween) null;
    float volumeMaster = SaveManager.Instance.SettingsSave.VolumeMaster;
    NMuteInBackgroundHandler.SetMasterVolume(volumeMaster);
    Log.Info($"MuteInBackground: Unmuted, restored volume to {volumeMaster}");
  }

  private static void SetMasterVolume(float volume)
  {
    NGame.Instance.AudioManager.SetMasterVol(volume);
    NGame.Instance.DebugAudio.SetMasterAudioVolume(volume);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NMuteInBackgroundHandler.MethodName._Notification, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("what"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMuteInBackgroundHandler.MethodName.Mute, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMuteInBackgroundHandler.MethodName.Unmute, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMuteInBackgroundHandler.MethodName.SetMasterVolume, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NMuteInBackgroundHandler.MethodName._Notification) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((GodotObject) this)._Notification(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMuteInBackgroundHandler.MethodName.Mute) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Mute();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMuteInBackgroundHandler.MethodName.Unmute) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Unmute();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMuteInBackgroundHandler.MethodName.SetMasterVolume) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    NMuteInBackgroundHandler.SetMasterVolume(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMuteInBackgroundHandler.MethodName.SetMasterVolume) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NMuteInBackgroundHandler.SetMasterVolume(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMuteInBackgroundHandler.MethodName._Notification) || StringName.op_Equality(ref method, NMuteInBackgroundHandler.MethodName.Mute) || StringName.op_Equality(ref method, NMuteInBackgroundHandler.MethodName.Unmute) || StringName.op_Equality(ref method, NMuteInBackgroundHandler.MethodName.SetMasterVolume) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMuteInBackgroundHandler.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMuteInBackgroundHandler.PropertyName._lastFocusOutMsec))
    {
      this._lastFocusOutMsec = VariantUtils.ConvertTo<ulong>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMuteInBackgroundHandler.PropertyName._loggedEnvironment))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._loggedEnvironment = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMuteInBackgroundHandler.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (StringName.op_Equality(ref name, NMuteInBackgroundHandler.PropertyName._lastFocusOutMsec))
    {
      value = VariantUtils.CreateFrom<ulong>(ref this._lastFocusOutMsec);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMuteInBackgroundHandler.PropertyName._loggedEnvironment))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._loggedEnvironment);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NMuteInBackgroundHandler.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NMuteInBackgroundHandler.PropertyName._lastFocusOutMsec, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NMuteInBackgroundHandler.PropertyName._loggedEnvironment, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NMuteInBackgroundHandler.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NMuteInBackgroundHandler.PropertyName._lastFocusOutMsec, Variant.From<ulong>(ref this._lastFocusOutMsec));
    info.AddProperty(NMuteInBackgroundHandler.PropertyName._loggedEnvironment, Variant.From<bool>(ref this._loggedEnvironment));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMuteInBackgroundHandler.PropertyName._tween, ref variant1))
      this._tween = ((Variant) ref variant1).As<Tween>();
    Variant variant2;
    if (info.TryGetProperty(NMuteInBackgroundHandler.PropertyName._lastFocusOutMsec, ref variant2))
      this._lastFocusOutMsec = ((Variant) ref variant2).As<ulong>();
    Variant variant3;
    if (!info.TryGetProperty(NMuteInBackgroundHandler.PropertyName._loggedEnvironment, ref variant3))
      return;
    this._loggedEnvironment = ((Variant) ref variant3).As<bool>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Notification = StringName.op_Implicit(nameof (_Notification));
    public static readonly StringName Mute = StringName.op_Implicit(nameof (Mute));
    public static readonly StringName Unmute = StringName.op_Implicit(nameof (Unmute));
    public static readonly StringName SetMasterVolume = StringName.op_Implicit(nameof (SetMasterVolume));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _lastFocusOutMsec = StringName.op_Implicit(nameof (_lastFocusOutMsec));
    public static readonly StringName _loggedEnvironment = StringName.op_Implicit(nameof (_loggedEnvironment));
  }

  public class SignalName : Node.SignalName
  {
  }
}
