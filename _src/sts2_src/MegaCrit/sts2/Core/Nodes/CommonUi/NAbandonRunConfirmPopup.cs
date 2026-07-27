// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.CommonUi.NAbandonRunConfirmPopup
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
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

[ScriptPath("res://src/Core/Nodes/CommonUi/NAbandonRunConfirmPopup.cs")]
public class NAbandonRunConfirmPopup : Control, IScreenContext
{
  private NVerticalPopup _verticalPopup;
  private NMainMenu? _mainMenuNode;
  private Tween? _tween;
  private static readonly string _scenePath = SceneHelper.GetScenePath("ui/abandon_run_confirm_popup");

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NAbandonRunConfirmPopup._scenePath);
    }
  }

  public override void _Ready()
  {
    this._verticalPopup = ((Node) this).GetNode<NVerticalPopup>(NodePath.op_Implicit("VerticalPopup"));
    this._verticalPopup.SetText(new LocString("main_menu_ui", "ABANDON_RUN_CONFIRMATION.header"), new LocString("main_menu_ui", "ABANDON_RUN_CONFIRMATION.body"));
    this._verticalPopup.InitYesButton(new LocString("main_menu_ui", "GENERIC_POPUP.confirm"), new Action<NButton>(this.OnYesButtonPressed));
    this._verticalPopup.InitNoButton(new LocString("main_menu_ui", "GENERIC_POPUP.cancel"), new Action<NButton>(this.OnNoButtonPressed));
  }

  public override void _EnterTree()
  {
    ((CanvasItem) this).Modulate = StsColors.transparentBlack;
    float y = this.Position.Y;
    this.Position = Vector2.op_Addition(this.Position, new Vector2(0.0f, 100f));
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.1);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position:y"), Variant.op_Implicit(y), 0.3).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L);
  }

  public override void _ExitTree()
  {
    this._verticalPopup.DisconnectSignals();
    this._verticalPopup.DisconnectHotkeys();
  }

  public static NAbandonRunConfirmPopup? Create(NMainMenu? mainMenu)
  {
    if (TestMode.IsOn)
      return (NAbandonRunConfirmPopup) null;
    NAbandonRunConfirmPopup nabandonRunConfirmPopup = PreloadManager.Cache.GetScene(NAbandonRunConfirmPopup._scenePath).Instantiate<NAbandonRunConfirmPopup>((PackedScene.GenEditState) 0L);
    nabandonRunConfirmPopup._mainMenuNode = mainMenu;
    return nabandonRunConfirmPopup;
  }

  private void OnYesButtonPressed(NButton _)
  {
    if (this._mainMenuNode == null)
      RunManager.Instance.Abandon();
    else
      this._mainMenuNode.AbandonRun();
    ((Node) this).QueueFreeSafely();
  }

  private void OnNoButtonPressed(NButton _) => ((Node) this).QueueFreeSafely();

  public Control? DefaultFocusedControl => (Control) null;

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NAbandonRunConfirmPopup.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAbandonRunConfirmPopup.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAbandonRunConfirmPopup.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAbandonRunConfirmPopup.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("mainMenu"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NAbandonRunConfirmPopup.MethodName.OnYesButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NAbandonRunConfirmPopup.MethodName.OnNoButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NAbandonRunConfirmPopup.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAbandonRunConfirmPopup.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAbandonRunConfirmPopup.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAbandonRunConfirmPopup.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NAbandonRunConfirmPopup nabandonRunConfirmPopup = NAbandonRunConfirmPopup.Create(VariantUtils.ConvertTo<NMainMenu>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NAbandonRunConfirmPopup>(ref nabandonRunConfirmPopup);
      return true;
    }
    if (StringName.op_Equality(ref method, NAbandonRunConfirmPopup.MethodName.OnYesButtonPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnYesButtonPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NAbandonRunConfirmPopup.MethodName.OnNoButtonPressed) || ((NativeVariantPtrArgs) ref args).Count != 1)
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
    if (StringName.op_Equality(ref method, NAbandonRunConfirmPopup.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NAbandonRunConfirmPopup nabandonRunConfirmPopup = NAbandonRunConfirmPopup.Create(VariantUtils.ConvertTo<NMainMenu>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NAbandonRunConfirmPopup>(ref nabandonRunConfirmPopup);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NAbandonRunConfirmPopup.MethodName._Ready) || StringName.op_Equality(ref method, NAbandonRunConfirmPopup.MethodName._EnterTree) || StringName.op_Equality(ref method, NAbandonRunConfirmPopup.MethodName._ExitTree) || StringName.op_Equality(ref method, NAbandonRunConfirmPopup.MethodName.Create) || StringName.op_Equality(ref method, NAbandonRunConfirmPopup.MethodName.OnYesButtonPressed) || StringName.op_Equality(ref method, NAbandonRunConfirmPopup.MethodName.OnNoButtonPressed) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NAbandonRunConfirmPopup.PropertyName._verticalPopup))
    {
      this._verticalPopup = VariantUtils.ConvertTo<NVerticalPopup>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAbandonRunConfirmPopup.PropertyName._mainMenuNode))
    {
      this._mainMenuNode = VariantUtils.ConvertTo<NMainMenu>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NAbandonRunConfirmPopup.PropertyName._tween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._tween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NAbandonRunConfirmPopup.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NAbandonRunConfirmPopup.PropertyName._verticalPopup))
    {
      value = VariantUtils.CreateFrom<NVerticalPopup>(ref this._verticalPopup);
      return true;
    }
    if (StringName.op_Equality(ref name, NAbandonRunConfirmPopup.PropertyName._mainMenuNode))
    {
      value = VariantUtils.CreateFrom<NMainMenu>(ref this._mainMenuNode);
      return true;
    }
    if (!StringName.op_Equality(ref name, NAbandonRunConfirmPopup.PropertyName._tween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._tween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NAbandonRunConfirmPopup.PropertyName._verticalPopup, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAbandonRunConfirmPopup.PropertyName._mainMenuNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAbandonRunConfirmPopup.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAbandonRunConfirmPopup.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NAbandonRunConfirmPopup.PropertyName._verticalPopup, Variant.From<NVerticalPopup>(ref this._verticalPopup));
    info.AddProperty(NAbandonRunConfirmPopup.PropertyName._mainMenuNode, Variant.From<NMainMenu>(ref this._mainMenuNode));
    info.AddProperty(NAbandonRunConfirmPopup.PropertyName._tween, Variant.From<Tween>(ref this._tween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NAbandonRunConfirmPopup.PropertyName._verticalPopup, ref variant1))
      this._verticalPopup = ((Variant) ref variant1).As<NVerticalPopup>();
    Variant variant2;
    if (info.TryGetProperty(NAbandonRunConfirmPopup.PropertyName._mainMenuNode, ref variant2))
      this._mainMenuNode = ((Variant) ref variant2).As<NMainMenu>();
    Variant variant3;
    if (!info.TryGetProperty(NAbandonRunConfirmPopup.PropertyName._tween, ref variant3))
      return;
    this._tween = ((Variant) ref variant3).As<Tween>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName OnYesButtonPressed = StringName.op_Implicit(nameof (OnYesButtonPressed));
    public static readonly StringName OnNoButtonPressed = StringName.op_Implicit(nameof (OnNoButtonPressed));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _verticalPopup = StringName.op_Implicit(nameof (_verticalPopup));
    public static readonly StringName _mainMenuNode = StringName.op_Implicit(nameof (_mainMenuNode));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
  }

  public class SignalName : Control.SignalName
  {
  }
}
