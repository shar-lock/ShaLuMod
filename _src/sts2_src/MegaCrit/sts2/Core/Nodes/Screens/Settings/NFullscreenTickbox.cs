// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Settings.NFullscreenTickbox
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings;

[ScriptPath("res://src/Core/Nodes/Screens/Settings/NFullscreenTickbox.cs")]
public class NFullscreenTickbox : NSettingsTickbox
{
  public override void _Ready()
  {
    this.ConnectSignals();
    ((GodotObject) NGame.Instance).Connect(NGame.SignalName.WindowChange, Callable.From<bool>(new Action<bool>(this.OnWindowChange)), 0U);
    this.OnWindowChange(SaveManager.Instance.SettingsSave.AspectRatioSetting == AspectRatioSetting.Auto);
    if (!PlatformUtil.GetSupportedWindowMode().ShouldForceFullscreen())
      return;
    this.Disable();
  }

  protected override void OnTick() => NFullscreenTickbox.SetFullscreen(true);

  protected override void OnUntick() => NFullscreenTickbox.SetFullscreen(false);

  private void OnWindowChange(bool _)
  {
    this.IsTicked = SaveManager.Instance.SettingsSave.Fullscreen;
  }

  public static void SetFullscreen(bool fullscreen)
  {
    if (PlatformUtil.GetSupportedWindowMode().ShouldForceFullscreen() && !fullscreen)
    {
      Log.Warn($"Tried to go to windowed mode, but the current platform doesn't support it ({PlatformUtil.GetSupportedWindowMode()})");
    }
    else
    {
      int currentScreen = DisplayServer.WindowGetCurrentScreen(0);
      SettingsSave settingsSave = SaveManager.Instance.SettingsSave;
      if (fullscreen)
      {
        Log.Info($"Setting FULLSCREEN for display [{currentScreen}]");
        settingsSave.TargetDisplay = currentScreen;
        settingsSave.Fullscreen = true;
        settingsSave.WindowSize = DisplayServer.WindowGetSize(0);
        settingsSave.WindowPosition = new Vector2I(-1, -1);
      }
      else
      {
        Log.Info($"Exiting FULLSCREEN for display [{currentScreen}]");
        if (Vector2I.op_GreaterThanOrEqual(settingsSave.WindowSize, DisplayServer.ScreenGetSize(currentScreen)))
        {
          settingsSave.WindowSize = Vector2I.op_Subtraction(DisplayServer.ScreenGetSize(currentScreen), new Vector2I(8, 48 /*0x30*/));
          settingsSave.WindowPosition = new Vector2I(4, 44);
        }
        settingsSave.Fullscreen = false;
      }
      NGame.Instance.ApplyDisplaySettings();
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NFullscreenTickbox.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NFullscreenTickbox.MethodName.OnTick, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NFullscreenTickbox.MethodName.OnUntick, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NFullscreenTickbox.MethodName.OnWindowChange, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NFullscreenTickbox.MethodName.SetFullscreen, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("fullscreen"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NFullscreenTickbox.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NFullscreenTickbox.MethodName.OnTick) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnTick();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NFullscreenTickbox.MethodName.OnUntick) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUntick();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NFullscreenTickbox.MethodName.OnWindowChange) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnWindowChange(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NFullscreenTickbox.MethodName.SetFullscreen) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    NFullscreenTickbox.SetFullscreen(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NFullscreenTickbox.MethodName.SetFullscreen) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NFullscreenTickbox.SetFullscreen(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NFullscreenTickbox.MethodName._Ready) || StringName.op_Equality(ref method, NFullscreenTickbox.MethodName.OnTick) || StringName.op_Equality(ref method, NFullscreenTickbox.MethodName.OnUntick) || StringName.op_Equality(ref method, NFullscreenTickbox.MethodName.OnWindowChange) || StringName.op_Equality(ref method, NFullscreenTickbox.MethodName.SetFullscreen) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
  }

  public new class MethodName : NSettingsTickbox.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName OnTick = StringName.op_Implicit(nameof (OnTick));
    public new static readonly StringName OnUntick = StringName.op_Implicit(nameof (OnUntick));
    public static readonly StringName OnWindowChange = StringName.op_Implicit(nameof (OnWindowChange));
    public static readonly StringName SetFullscreen = StringName.op_Implicit(nameof (SetFullscreen));
  }

  public new class PropertyName : NSettingsTickbox.PropertyName
  {
  }

  public new class SignalName : NSettingsTickbox.SignalName
  {
  }
}
