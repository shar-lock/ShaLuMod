// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.CommonUi.NSettingsScreenPopup
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
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.CommonUi;

[ScriptPath("res://src/Core/Nodes/CommonUi/NSettingsScreenPopup.cs")]
public class NSettingsScreenPopup : Control, IScreenContext
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("ui/settings_screen_popup");
  private ColorRect _backstop;
  private LocString _header;
  private LocString _description;

  public NVerticalPopup VerticalPopup { get; private set; }

  public static NSettingsScreenPopup? Create(LocString header, LocString description)
  {
    if (TestMode.IsOn)
      return (NSettingsScreenPopup) null;
    NSettingsScreenPopup nsettingsScreenPopup = PreloadManager.Cache.GetScene(NSettingsScreenPopup._scenePath).Instantiate<NSettingsScreenPopup>((PackedScene.GenEditState) 0L);
    nsettingsScreenPopup._header = header;
    nsettingsScreenPopup._description = description;
    return nsettingsScreenPopup;
  }

  public override void _Ready()
  {
    this.VerticalPopup = ((Node) this).GetNode<NVerticalPopup>(NodePath.op_Implicit("VerticalPopup"));
    this.VerticalPopup.InitNoButton(new LocString("main_menu_ui", "GENERIC_POPUP.cancel"), (Action<NButton>) (_ => { }));
    this.VerticalPopup.InitYesButton(new LocString("main_menu_ui", "GENERIC_POPUP.confirm"), new Action<NButton>(this.OnYesButtonPressed));
    this.VerticalPopup.SetText(this._header, this._description);
  }

  private void OnYesButtonPressed(NButton _)
  {
    Log.Info("FTUEs have been reset!");
    SaveManager.Instance.ResetFtues();
  }

  public Control? DefaultFocusedControl => (Control) null;

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NSettingsScreenPopup.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSettingsScreenPopup.MethodName.OnYesButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NSettingsScreenPopup.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NSettingsScreenPopup.MethodName.OnYesButtonPressed) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnYesButtonPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSettingsScreenPopup.MethodName._Ready) || StringName.op_Equality(ref method, NSettingsScreenPopup.MethodName.OnYesButtonPressed) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSettingsScreenPopup.PropertyName.VerticalPopup))
    {
      this.VerticalPopup = VariantUtils.ConvertTo<NVerticalPopup>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSettingsScreenPopup.PropertyName._backstop))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._backstop = VariantUtils.ConvertTo<ColorRect>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSettingsScreenPopup.PropertyName.VerticalPopup))
    {
      ref godot_variant local = ref value;
      NVerticalPopup verticalPopup = this.VerticalPopup;
      godot_variant from = VariantUtils.CreateFrom<NVerticalPopup>(ref verticalPopup);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NSettingsScreenPopup.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (!StringName.op_Equality(ref name, NSettingsScreenPopup.PropertyName._backstop))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<ColorRect>(ref this._backstop);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NSettingsScreenPopup.PropertyName._backstop, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSettingsScreenPopup.PropertyName.VerticalPopup, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSettingsScreenPopup.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName verticalPopup1 = NSettingsScreenPopup.PropertyName.VerticalPopup;
    NVerticalPopup verticalPopup2 = this.VerticalPopup;
    Variant variant = Variant.From<NVerticalPopup>(ref verticalPopup2);
    serializationInfo.AddProperty(verticalPopup1, variant);
    info.AddProperty(NSettingsScreenPopup.PropertyName._backstop, Variant.From<ColorRect>(ref this._backstop));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NSettingsScreenPopup.PropertyName.VerticalPopup, ref variant1))
      this.VerticalPopup = ((Variant) ref variant1).As<NVerticalPopup>();
    Variant variant2;
    if (!info.TryGetProperty(NSettingsScreenPopup.PropertyName._backstop, ref variant2))
      return;
    this._backstop = ((Variant) ref variant2).As<ColorRect>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnYesButtonPressed = StringName.op_Implicit(nameof (OnYesButtonPressed));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName VerticalPopup = StringName.op_Implicit(nameof (VerticalPopup));
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _backstop = StringName.op_Implicit(nameof (_backstop));
  }

  public class SignalName : Control.SignalName
  {
  }
}
