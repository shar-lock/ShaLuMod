// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerWarningPopup
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Multiplayer;

[ScriptPath("res://src/Core/Nodes/Multiplayer/NMultiplayerWarningPopup.cs")]
public class NMultiplayerWarningPopup : NVerticalPopup, IScreenContext
{
  public const string ftueId = "multiplayer_warning";
  private NVerticalPopup _verticalPopup;
  private static readonly string _scenePath = SceneHelper.GetScenePath("ui/multiplayer_warning_popup");

  public new static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NMultiplayerWarningPopup._scenePath);
    }
  }

  public static NMultiplayerWarningPopup? Create()
  {
    return TestMode.IsOn ? (NMultiplayerWarningPopup) null : PreloadManager.Cache.GetScene(NMultiplayerWarningPopup._scenePath).Instantiate<NMultiplayerWarningPopup>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready()
  {
    this._verticalPopup = ((Node) this).GetNode<NVerticalPopup>(NodePath.op_Implicit("VerticalPopup"));
    this._verticalPopup.SetText(new LocString("main_menu_ui", "MULTIPLAYER_WARNING_POPUP.title"), new LocString("main_menu_ui", "MULTIPLAYER_WARNING_POPUP.body"));
    this._verticalPopup.InitYesButton(new LocString("main_menu_ui", "MULTIPLAYER_WARNING_POPUP.back"), new Action<NButton>(this.OnBackButtonPressed));
    this._verticalPopup.InitNoButton(new LocString("main_menu_ui", "MULTIPLAYER_WARNING_POPUP.ignore"), new Action<NButton>(this.OnIgnoreButtonPressed));
  }

  private void OnBackButtonPressed(NButton _)
  {
    ((Node) this).QueueFreeSafely();
    NGame.Instance.MainMenu.SubmenuStack.Pop();
    NModalContainer.Instance.HideBackstop();
  }

  private void OnIgnoreButtonPressed(NButton _)
  {
    SaveManager.Instance.MarkFtueAsComplete("multiplayer_warning");
    ((Node) this).QueueFreeSafely();
    NModalContainer.Instance.HideBackstop();
  }

  public Control? DefaultFocusedControl => (Control) null;

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NMultiplayerWarningPopup.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerWarningPopup.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerWarningPopup.MethodName.OnBackButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMultiplayerWarningPopup.MethodName.OnIgnoreButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NMultiplayerWarningPopup.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NMultiplayerWarningPopup nmultiplayerWarningPopup = NMultiplayerWarningPopup.Create();
      ret = VariantUtils.CreateFrom<NMultiplayerWarningPopup>(ref nmultiplayerWarningPopup);
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerWarningPopup.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerWarningPopup.MethodName.OnBackButtonPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnBackButtonPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMultiplayerWarningPopup.MethodName.OnIgnoreButtonPressed) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnIgnoreButtonPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMultiplayerWarningPopup.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NMultiplayerWarningPopup nmultiplayerWarningPopup = NMultiplayerWarningPopup.Create();
      ret = VariantUtils.CreateFrom<NMultiplayerWarningPopup>(ref nmultiplayerWarningPopup);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMultiplayerWarningPopup.MethodName.Create) || StringName.op_Equality(ref method, NMultiplayerWarningPopup.MethodName._Ready) || StringName.op_Equality(ref method, NMultiplayerWarningPopup.MethodName.OnBackButtonPressed) || StringName.op_Equality(ref method, NMultiplayerWarningPopup.MethodName.OnIgnoreButtonPressed) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NMultiplayerWarningPopup.PropertyName._verticalPopup))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._verticalPopup = VariantUtils.ConvertTo<NVerticalPopup>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMultiplayerWarningPopup.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (!StringName.op_Equality(ref name, NMultiplayerWarningPopup.PropertyName._verticalPopup))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<NVerticalPopup>(ref this._verticalPopup);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NMultiplayerWarningPopup.PropertyName._verticalPopup, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerWarningPopup.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NMultiplayerWarningPopup.PropertyName._verticalPopup, Variant.From<NVerticalPopup>(ref this._verticalPopup));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(NMultiplayerWarningPopup.PropertyName._verticalPopup, ref variant))
      return;
    this._verticalPopup = ((Variant) ref variant).As<NVerticalPopup>();
  }

  public new class MethodName : NVerticalPopup.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnBackButtonPressed = StringName.op_Implicit(nameof (OnBackButtonPressed));
    public static readonly StringName OnIgnoreButtonPressed = StringName.op_Implicit(nameof (OnIgnoreButtonPressed));
  }

  public new class PropertyName : NVerticalPopup.PropertyName
  {
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _verticalPopup = StringName.op_Implicit(nameof (_verticalPopup));
  }

  public new class SignalName : NVerticalPopup.SignalName
  {
  }
}
