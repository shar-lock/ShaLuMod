// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NEarlyAccessDisclaimer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;

[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NEarlyAccessDisclaimer.cs")]
public class NEarlyAccessDisclaimer : Control, IScreenContext
{
  private Tween? _tween;
  private Control _image;

  public static NEarlyAccessDisclaimer Create()
  {
    NEarlyAccessDisclaimer screen = ResourceLoader.Load<PackedScene>("res://scenes/screens/main_menu/early_access_disclaimer.tscn", (string) null, (ResourceLoader.CacheMode) 1L).Instantiate<NEarlyAccessDisclaimer>((PackedScene.GenEditState) 0L);
    NHotkeyManager.Instance?.AddBlockingScreen((Node) screen);
    return screen;
  }

  public override void _Ready()
  {
    string formattedText = new LocString("main_menu_ui", "EARLY_ACCESS_DISCLAIMER.header").GetFormattedText();
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%Header")).SetTextAutoSize(formattedText);
    this._image = (Control) ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("Image"));
    this.UpdateEaDisclaimerDescription();
    ((GodotObject) NControllerManager.Instance).Connect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.UpdateEaDisclaimerDescription)), 0U);
    ((GodotObject) NControllerManager.Instance).Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.UpdateEaDisclaimerDescription)), 0U);
  }

  public async Task CloseScreen()
  {
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.5);
    this._tween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("position:y"), Variant.op_Implicit(this._image.Position.Y - 1000f), 0.5).SetEase((Tween.EaseType) 0L).SetTrans((Tween.TransitionType) 10L);
    SaveManager.Instance.SettingsSave.SeenEaDisclaimer = true;
    bool flag = await this._tween.AwaitFinished((Node) this);
    ((Node) this).QueueFreeSafely();
    NHotkeyManager.Instance?.RemoveBlockingScreen((Node) this);
    NModalContainer.Instance.Clear();
  }

  private void UpdateEaDisclaimerDescription()
  {
    string text = !NControllerManager.Instance.IsUsingController ? new LocString("main_menu_ui", "EARLY_ACCESS_DISCLAIMER.description_mkb").GetFormattedText() : new LocString("main_menu_ui", "EARLY_ACCESS_DISCLAIMER.description_controller").GetFormattedText();
    ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%Description")).SetTextAutoSize(text);
  }

  public Control? DefaultFocusedControl => (Control) null;

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NEarlyAccessDisclaimer.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEarlyAccessDisclaimer.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEarlyAccessDisclaimer.MethodName.UpdateEaDisclaimerDescription, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NEarlyAccessDisclaimer.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NEarlyAccessDisclaimer accessDisclaimer = NEarlyAccessDisclaimer.Create();
      ret = VariantUtils.CreateFrom<NEarlyAccessDisclaimer>(ref accessDisclaimer);
      return true;
    }
    if (StringName.op_Equality(ref method, NEarlyAccessDisclaimer.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NEarlyAccessDisclaimer.MethodName.UpdateEaDisclaimerDescription) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.UpdateEaDisclaimerDescription();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NEarlyAccessDisclaimer.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NEarlyAccessDisclaimer accessDisclaimer = NEarlyAccessDisclaimer.Create();
      ret = VariantUtils.CreateFrom<NEarlyAccessDisclaimer>(ref accessDisclaimer);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NEarlyAccessDisclaimer.MethodName.Create) || StringName.op_Equality(ref method, NEarlyAccessDisclaimer.MethodName._Ready) || StringName.op_Equality(ref method, NEarlyAccessDisclaimer.MethodName.UpdateEaDisclaimerDescription) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NEarlyAccessDisclaimer.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NEarlyAccessDisclaimer.PropertyName._image))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._image = VariantUtils.ConvertTo<Control>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NEarlyAccessDisclaimer.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NEarlyAccessDisclaimer.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NEarlyAccessDisclaimer.PropertyName._image))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Control>(ref this._image);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NEarlyAccessDisclaimer.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEarlyAccessDisclaimer.PropertyName._image, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEarlyAccessDisclaimer.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NEarlyAccessDisclaimer.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NEarlyAccessDisclaimer.PropertyName._image, Variant.From<Control>(ref this._image));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NEarlyAccessDisclaimer.PropertyName._tween, ref variant1))
      this._tween = ((Variant) ref variant1).As<Tween>();
    Variant variant2;
    if (!info.TryGetProperty(NEarlyAccessDisclaimer.PropertyName._image, ref variant2))
      return;
    this._image = ((Variant) ref variant2).As<Control>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName UpdateEaDisclaimerDescription = StringName.op_Implicit(nameof (UpdateEaDisclaimerDescription));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _image = StringName.op_Implicit(nameof (_image));
  }

  public class SignalName : Control.SignalName
  {
  }
}
