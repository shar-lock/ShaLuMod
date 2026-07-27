// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.NBackgroundModeHandler
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
namespace MegaCrit.Sts2.Core.Nodes;

[ScriptPath("res://src/Core/Nodes/NBackgroundModeHandler.cs")]
public class NBackgroundModeHandler : Node
{
  private const int _backgroundFps = 30;
  private int _savedMaxFps;
  private bool _isBackgrounded;

  private static bool IsHeadless
  {
    get => DisplayServer.GetName().Equals("headless", StringComparison.OrdinalIgnoreCase);
  }

  private static bool IsEditor => OS.HasFeature("editor");

  public override void _Notification(int what)
  {
    if (NBackgroundModeHandler.IsHeadless || NBackgroundModeHandler.IsEditor || NonInteractiveMode.IsActive)
      return;
    if (what == 1005)
    {
      this.EnterBackgroundMode();
    }
    else
    {
      if (what != 1004)
        return;
      this.ExitBackgroundMode();
    }
  }

  private void EnterBackgroundMode()
  {
    if (this._isBackgrounded)
    {
      Log.Info("BackgroundMode: duplicate FocusOut (already backgrounded)");
    }
    else
    {
      if (!SaveManager.Instance.SettingsSave.LimitFpsInBackground)
        return;
      INetGameService netService = RunManager.Instance.NetService;
      if (netService != null && netService.Type.IsMultiplayer())
        return;
      this._isBackgrounded = true;
      this._savedMaxFps = Engine.MaxFps;
      Engine.MaxFps = 30;
      Log.Info($"Limiting background FPS to {30}");
    }
  }

  private void ExitBackgroundMode()
  {
    if (!this._isBackgrounded)
      return;
    this._isBackgrounded = false;
    Engine.MaxFps = this._savedMaxFps;
    Log.Info("Restored foreground FPS");
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NBackgroundModeHandler.MethodName._Notification, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("what"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NBackgroundModeHandler.MethodName.EnterBackgroundMode, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBackgroundModeHandler.MethodName.ExitBackgroundMode, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NBackgroundModeHandler.MethodName._Notification) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((GodotObject) this)._Notification(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBackgroundModeHandler.MethodName.EnterBackgroundMode) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.EnterBackgroundMode();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NBackgroundModeHandler.MethodName.ExitBackgroundMode) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.ExitBackgroundMode();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NBackgroundModeHandler.MethodName._Notification) || StringName.op_Equality(ref method, NBackgroundModeHandler.MethodName.EnterBackgroundMode) || StringName.op_Equality(ref method, NBackgroundModeHandler.MethodName.ExitBackgroundMode) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NBackgroundModeHandler.PropertyName._savedMaxFps))
    {
      this._savedMaxFps = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NBackgroundModeHandler.PropertyName._isBackgrounded))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._isBackgrounded = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NBackgroundModeHandler.PropertyName._savedMaxFps))
    {
      value = VariantUtils.CreateFrom<int>(ref this._savedMaxFps);
      return true;
    }
    if (!StringName.op_Equality(ref name, NBackgroundModeHandler.PropertyName._isBackgrounded))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._isBackgrounded);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 2L, NBackgroundModeHandler.PropertyName._savedMaxFps, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NBackgroundModeHandler.PropertyName._isBackgrounded, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NBackgroundModeHandler.PropertyName._savedMaxFps, Variant.From<int>(ref this._savedMaxFps));
    info.AddProperty(NBackgroundModeHandler.PropertyName._isBackgrounded, Variant.From<bool>(ref this._isBackgrounded));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NBackgroundModeHandler.PropertyName._savedMaxFps, ref variant1))
      this._savedMaxFps = ((Variant) ref variant1).As<int>();
    Variant variant2;
    if (!info.TryGetProperty(NBackgroundModeHandler.PropertyName._isBackgrounded, ref variant2))
      return;
    this._isBackgrounded = ((Variant) ref variant2).As<bool>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Notification = StringName.op_Implicit(nameof (_Notification));
    public static readonly StringName EnterBackgroundMode = StringName.op_Implicit(nameof (EnterBackgroundMode));
    public static readonly StringName ExitBackgroundMode = StringName.op_Implicit(nameof (ExitBackgroundMode));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _savedMaxFps = StringName.op_Implicit(nameof (_savedMaxFps));
    public static readonly StringName _isBackgrounded = StringName.op_Implicit(nameof (_isBackgrounded));
  }

  public class SignalName : Node.SignalName
  {
  }
}
