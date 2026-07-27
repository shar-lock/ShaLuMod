// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.ModdingScreen.NConfirmModLoadingPopup
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.ModdingScreen;

[ScriptPath("res://src/Core/Nodes/Screens/ModdingScreen/NConfirmModLoadingPopup.cs")]
public class NConfirmModLoadingPopup : Control, IScreenContext
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("ui/confirm_mod_loading_popup");
  private NVerticalPopup _verticalPopup;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NConfirmModLoadingPopup._scenePath);
    }
  }

  public Control DefaultFocusedControl => (Control) this._verticalPopup.NoButton;

  public static NConfirmModLoadingPopup? Create()
  {
    return TestMode.IsOn ? (NConfirmModLoadingPopup) null : PreloadManager.Cache.GetScene(NConfirmModLoadingPopup._scenePath).Instantiate<NConfirmModLoadingPopup>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready()
  {
    this._verticalPopup = ((Node) this).GetNode<NVerticalPopup>(NodePath.op_Implicit("VerticalPopup"));
    this._verticalPopup.SetText(new LocString("main_menu_ui", "MODDING_POPUP.title"), new LocString("main_menu_ui", "MODDING_POPUP.description"));
    this._verticalPopup.InitYesButton(new LocString("main_menu_ui", "MODDING_POPUP.load_mods"), new Action<NButton>(this.OnYesButtonPressed));
    this._verticalPopup.InitNoButton(new LocString("main_menu_ui", "MODDING_POPUP.cancel"), new Action<NButton>(this.OnNoButtonPressed));
  }

  private void OnYesButtonPressed(NButton _)
  {
    Log.Info("Player chose to load mods");
    SettingsSave settingsSave = SaveManager.Instance.SettingsSave;
    if (settingsSave.ModSettings == null)
    {
      ModSettings modSettings;
      settingsSave.ModSettings = modSettings = new ModSettings();
    }
    SaveManager.Instance.SettingsSave.ModSettings.PlayerAgreedToModLoading = true;
    SaveManager.Instance.SaveSettings();
    NGame.Instance.Quit();
    ((Node) this).QueueFreeSafely();
  }

  private void OnNoButtonPressed(NButton _)
  {
    Log.Info("Player chose not to load mods");
    SettingsSave settingsSave = SaveManager.Instance.SettingsSave;
    if (settingsSave.ModSettings == null)
    {
      ModSettings modSettings;
      settingsSave.ModSettings = modSettings = new ModSettings();
    }
    SaveManager.Instance.SettingsSave.ModSettings.PlayerAgreedToModLoading = false;
    SaveManager.Instance.SaveSettings();
    if (NGame.Instance?.MainMenu?.SubmenuStack.Peek() is NModdingScreen)
      NGame.Instance.MainMenu.SubmenuStack.Pop();
    ((Node) this).QueueFreeSafely();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NConfirmModLoadingPopup.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NConfirmModLoadingPopup.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NConfirmModLoadingPopup.MethodName.OnYesButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NConfirmModLoadingPopup.MethodName.OnNoButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NConfirmModLoadingPopup.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NConfirmModLoadingPopup nconfirmModLoadingPopup = NConfirmModLoadingPopup.Create();
      ret = VariantUtils.CreateFrom<NConfirmModLoadingPopup>(ref nconfirmModLoadingPopup);
      return true;
    }
    if (StringName.op_Equality(ref method, NConfirmModLoadingPopup.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NConfirmModLoadingPopup.MethodName.OnYesButtonPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnYesButtonPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NConfirmModLoadingPopup.MethodName.OnNoButtonPressed) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnNoButtonPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NConfirmModLoadingPopup.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NConfirmModLoadingPopup nconfirmModLoadingPopup = NConfirmModLoadingPopup.Create();
      ret = VariantUtils.CreateFrom<NConfirmModLoadingPopup>(ref nconfirmModLoadingPopup);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NConfirmModLoadingPopup.MethodName.Create) || StringName.op_Equality(ref method, NConfirmModLoadingPopup.MethodName._Ready) || StringName.op_Equality(ref method, NConfirmModLoadingPopup.MethodName.OnYesButtonPressed) || StringName.op_Equality(ref method, NConfirmModLoadingPopup.MethodName.OnNoButtonPressed) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NConfirmModLoadingPopup.PropertyName._verticalPopup))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._verticalPopup = VariantUtils.ConvertTo<NVerticalPopup>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NConfirmModLoadingPopup.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (!StringName.op_Equality(ref name, NConfirmModLoadingPopup.PropertyName._verticalPopup))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<NVerticalPopup>(ref this._verticalPopup);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NConfirmModLoadingPopup.PropertyName._verticalPopup, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NConfirmModLoadingPopup.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NConfirmModLoadingPopup.PropertyName._verticalPopup, Variant.From<NVerticalPopup>(ref this._verticalPopup));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(NConfirmModLoadingPopup.PropertyName._verticalPopup, ref variant))
      return;
    this._verticalPopup = ((Variant) ref variant).As<NVerticalPopup>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnYesButtonPressed = StringName.op_Implicit(nameof (OnYesButtonPressed));
    public static readonly StringName OnNoButtonPressed = StringName.op_Implicit(nameof (OnNoButtonPressed));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _verticalPopup = StringName.op_Implicit(nameof (_verticalPopup));
  }

  public class SignalName : Control.SignalName
  {
  }
}
