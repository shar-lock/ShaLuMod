// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.CommonUi.NDisconnectConfirmPopup
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.CommonUi;

[ScriptPath("res://src/Core/Nodes/CommonUi/NDisconnectConfirmPopup.cs")]
public class NDisconnectConfirmPopup : Control, IScreenContext
{
  private MegaLabel _header;
  private MegaRichTextLabel _description;
  private NButton _noButton;
  private NButton _yesButton;
  private NMainMenu? _mainMenuNode;
  private static readonly string _scenePath = SceneHelper.GetScenePath("ui/disconnect_confirm_popup");
  private NVerticalPopup _verticalPopup;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NDisconnectConfirmPopup._scenePath);
    }
  }

  public override void _Ready()
  {
    this._verticalPopup = ((Node) this).GetNode<NVerticalPopup>(NodePath.op_Implicit("VerticalPopup"));
    this._verticalPopup.SetText(new LocString("settings_ui", "DISCONNECT_CONFIRMATION.header"), new LocString("settings_ui", "DISCONNECT_CONFIRMATION.body"));
    this._verticalPopup.InitNoButton(new LocString("main_menu_ui", "GENERIC_POPUP.cancel"), new Action<NButton>(this.OnNoButtonPressed));
    this._verticalPopup.InitYesButton(new LocString("main_menu_ui", "GENERIC_POPUP.confirm"), new Action<NButton>(this.OnYesButtonPressed));
  }

  public static NDisconnectConfirmPopup? Create()
  {
    return TestMode.IsOn ? (NDisconnectConfirmPopup) null : PreloadManager.Cache.GetScene(NDisconnectConfirmPopup._scenePath).Instantiate<NDisconnectConfirmPopup>((PackedScene.GenEditState) 0L);
  }

  private void OnYesButtonPressed(NButton _)
  {
    RunManager.Instance.NetService.Disconnect(NetError.Quit);
    ((Node) this).QueueFreeSafely();
  }

  private void OnNoButtonPressed(NButton _) => ((Node) this).QueueFreeSafely();

  public Control? DefaultFocusedControl => (Control) null;

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NDisconnectConfirmPopup.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDisconnectConfirmPopup.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDisconnectConfirmPopup.MethodName.OnYesButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NDisconnectConfirmPopup.MethodName.OnNoButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NDisconnectConfirmPopup.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDisconnectConfirmPopup.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NDisconnectConfirmPopup ndisconnectConfirmPopup = NDisconnectConfirmPopup.Create();
      ret = VariantUtils.CreateFrom<NDisconnectConfirmPopup>(ref ndisconnectConfirmPopup);
      return true;
    }
    if (StringName.op_Equality(ref method, NDisconnectConfirmPopup.MethodName.OnYesButtonPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnYesButtonPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NDisconnectConfirmPopup.MethodName.OnNoButtonPressed) || ((NativeVariantPtrArgs) ref args).Count != 1)
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
    if (StringName.op_Equality(ref method, NDisconnectConfirmPopup.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NDisconnectConfirmPopup ndisconnectConfirmPopup = NDisconnectConfirmPopup.Create();
      ret = VariantUtils.CreateFrom<NDisconnectConfirmPopup>(ref ndisconnectConfirmPopup);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NDisconnectConfirmPopup.MethodName._Ready) || StringName.op_Equality(ref method, NDisconnectConfirmPopup.MethodName.Create) || StringName.op_Equality(ref method, NDisconnectConfirmPopup.MethodName.OnYesButtonPressed) || StringName.op_Equality(ref method, NDisconnectConfirmPopup.MethodName.OnNoButtonPressed) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDisconnectConfirmPopup.PropertyName._header))
    {
      this._header = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDisconnectConfirmPopup.PropertyName._description))
    {
      this._description = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDisconnectConfirmPopup.PropertyName._noButton))
    {
      this._noButton = VariantUtils.ConvertTo<NButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDisconnectConfirmPopup.PropertyName._yesButton))
    {
      this._yesButton = VariantUtils.ConvertTo<NButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDisconnectConfirmPopup.PropertyName._mainMenuNode))
    {
      this._mainMenuNode = VariantUtils.ConvertTo<NMainMenu>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDisconnectConfirmPopup.PropertyName._verticalPopup))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._verticalPopup = VariantUtils.ConvertTo<NVerticalPopup>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDisconnectConfirmPopup.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NDisconnectConfirmPopup.PropertyName._header))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._header);
      return true;
    }
    if (StringName.op_Equality(ref name, NDisconnectConfirmPopup.PropertyName._description))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._description);
      return true;
    }
    if (StringName.op_Equality(ref name, NDisconnectConfirmPopup.PropertyName._noButton))
    {
      value = VariantUtils.CreateFrom<NButton>(ref this._noButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NDisconnectConfirmPopup.PropertyName._yesButton))
    {
      value = VariantUtils.CreateFrom<NButton>(ref this._yesButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NDisconnectConfirmPopup.PropertyName._mainMenuNode))
    {
      value = VariantUtils.CreateFrom<NMainMenu>(ref this._mainMenuNode);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDisconnectConfirmPopup.PropertyName._verticalPopup))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<NVerticalPopup>(ref this._verticalPopup);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NDisconnectConfirmPopup.PropertyName._header, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDisconnectConfirmPopup.PropertyName._description, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDisconnectConfirmPopup.PropertyName._noButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDisconnectConfirmPopup.PropertyName._yesButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDisconnectConfirmPopup.PropertyName._mainMenuNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDisconnectConfirmPopup.PropertyName._verticalPopup, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDisconnectConfirmPopup.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NDisconnectConfirmPopup.PropertyName._header, Variant.From<MegaLabel>(ref this._header));
    info.AddProperty(NDisconnectConfirmPopup.PropertyName._description, Variant.From<MegaRichTextLabel>(ref this._description));
    info.AddProperty(NDisconnectConfirmPopup.PropertyName._noButton, Variant.From<NButton>(ref this._noButton));
    info.AddProperty(NDisconnectConfirmPopup.PropertyName._yesButton, Variant.From<NButton>(ref this._yesButton));
    info.AddProperty(NDisconnectConfirmPopup.PropertyName._mainMenuNode, Variant.From<NMainMenu>(ref this._mainMenuNode));
    info.AddProperty(NDisconnectConfirmPopup.PropertyName._verticalPopup, Variant.From<NVerticalPopup>(ref this._verticalPopup));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NDisconnectConfirmPopup.PropertyName._header, ref variant1))
      this._header = ((Variant) ref variant1).As<MegaLabel>();
    Variant variant2;
    if (info.TryGetProperty(NDisconnectConfirmPopup.PropertyName._description, ref variant2))
      this._description = ((Variant) ref variant2).As<MegaRichTextLabel>();
    Variant variant3;
    if (info.TryGetProperty(NDisconnectConfirmPopup.PropertyName._noButton, ref variant3))
      this._noButton = ((Variant) ref variant3).As<NButton>();
    Variant variant4;
    if (info.TryGetProperty(NDisconnectConfirmPopup.PropertyName._yesButton, ref variant4))
      this._yesButton = ((Variant) ref variant4).As<NButton>();
    Variant variant5;
    if (info.TryGetProperty(NDisconnectConfirmPopup.PropertyName._mainMenuNode, ref variant5))
      this._mainMenuNode = ((Variant) ref variant5).As<NMainMenu>();
    Variant variant6;
    if (!info.TryGetProperty(NDisconnectConfirmPopup.PropertyName._verticalPopup, ref variant6))
      return;
    this._verticalPopup = ((Variant) ref variant6).As<NVerticalPopup>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName OnYesButtonPressed = StringName.op_Implicit(nameof (OnYesButtonPressed));
    public static readonly StringName OnNoButtonPressed = StringName.op_Implicit(nameof (OnNoButtonPressed));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _header = StringName.op_Implicit(nameof (_header));
    public static readonly StringName _description = StringName.op_Implicit(nameof (_description));
    public static readonly StringName _noButton = StringName.op_Implicit(nameof (_noButton));
    public static readonly StringName _yesButton = StringName.op_Implicit(nameof (_yesButton));
    public static readonly StringName _mainMenuNode = StringName.op_Implicit(nameof (_mainMenuNode));
    public static readonly StringName _verticalPopup = StringName.op_Implicit(nameof (_verticalPopup));
  }

  public class SignalName : Control.SignalName
  {
  }
}
