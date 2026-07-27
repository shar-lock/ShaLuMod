// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Map.NMapDrawButton
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Map;

[ScriptPath("res://src/Core/Nodes/Screens/Map/NMapDrawButton.cs")]
public class NMapDrawButton : NButton
{
  private static readonly StringName _imagePath = StringName.op_Implicit("res://images/packed/map/drawing_quill.png");
  private static readonly StringName _glowImagePath = StringName.op_Implicit("res://images/packed/map/drawing_quill_glow.png");
  private Control _drawingToolHolder;
  private TextureRect _icon;
  private HoverTip _hoverTip;
  private Tween? _tween;
  private bool _isDrawing;
  private static readonly Color _activeColor = new Color("57C4FFFF");
  private static readonly Color _inactiveColor = new Color("FFFFFF80");

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      List<string> items = new List<string>();
      items.Add(StringName.op_Implicit(NMapDrawButton._imagePath));
      items.Add(StringName.op_Implicit(NMapDrawButton._glowImagePath));
      items.AddRange(NMapEraseButton.AssetPaths);
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyList<string>(items);
    }
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._drawingToolHolder = (Control) ((Node) this).GetParent();
    this._icon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("Icon"));
    NControllerManager instance = NControllerManager.Instance;
    ((GodotObject) instance).Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.OnControllerUpdated)), 0U);
    ((GodotObject) instance).Connect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.OnControllerUpdated)), 0U);
    this.OnControllerUpdated();
  }

  public void SetIsDrawing(bool isDrawing)
  {
    this._isDrawing = isDrawing;
    this._icon.Texture = PreloadManager.Cache.GetTexture2D(StringName.op_Implicit(isDrawing ? NMapDrawButton._glowImagePath : NMapDrawButton._imagePath));
    ((CanvasItem) this._icon).SelfModulate = isDrawing ? NMapDrawButton._activeColor : NMapDrawButton._inactiveColor;
  }

  protected override void OnFocus()
  {
    base.OnFocus();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._icon, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 1.2f)), 0.05);
    this._tween.TweenProperty((GodotObject) this._icon, NodePath.op_Implicit("self_modulate"), Variant.op_Implicit(NMapDrawButton._activeColor), 0.05);
    NHoverTipSet.CreateAndShow(this._drawingToolHolder, (IHoverTip) this._hoverTip)?.SetGlobalPosition(Vector2.op_Addition(this._drawingToolHolder.GlobalPosition, new Vector2(10f, -132f)), false);
  }

  protected override void OnUnfocus()
  {
    base.OnUnfocus();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._icon, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 1.1f)), 0.05);
    this._tween.TweenProperty((GodotObject) this._icon, NodePath.op_Implicit("self_modulate"), Variant.op_Implicit(this._isDrawing ? NMapDrawButton._activeColor : NMapDrawButton._inactiveColor), 0.05);
    NHoverTipSet.Remove(this._drawingToolHolder);
  }

  private void OnControllerUpdated()
  {
    LocString description = new LocString("map", "DRAWING_BUTTON.description");
    this._hoverTip = new HoverTip(!NControllerManager.Instance.IsUsingController ? new LocString("map", "DRAWING_BUTTON.title_mkb") : new LocString("map", "DRAWING_BUTTON.title_controller"), description);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NMapDrawButton.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapDrawButton.MethodName.SetIsDrawing, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("isDrawing"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMapDrawButton.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapDrawButton.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapDrawButton.MethodName.OnControllerUpdated, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMapDrawButton.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapDrawButton.MethodName.SetIsDrawing) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetIsDrawing(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapDrawButton.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapDrawButton.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMapDrawButton.MethodName.OnControllerUpdated) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnControllerUpdated();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMapDrawButton.MethodName._Ready) || StringName.op_Equality(ref method, NMapDrawButton.MethodName.SetIsDrawing) || StringName.op_Equality(ref method, NMapDrawButton.MethodName.OnFocus) || StringName.op_Equality(ref method, NMapDrawButton.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NMapDrawButton.MethodName.OnControllerUpdated) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMapDrawButton.PropertyName._drawingToolHolder))
    {
      this._drawingToolHolder = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapDrawButton.PropertyName._icon))
    {
      this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapDrawButton.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMapDrawButton.PropertyName._isDrawing))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._isDrawing = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMapDrawButton.PropertyName._drawingToolHolder))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._drawingToolHolder);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapDrawButton.PropertyName._icon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapDrawButton.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMapDrawButton.PropertyName._isDrawing))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<bool>(ref this._isDrawing);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NMapDrawButton.PropertyName._drawingToolHolder, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapDrawButton.PropertyName._icon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapDrawButton.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NMapDrawButton.PropertyName._isDrawing, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NMapDrawButton.PropertyName._drawingToolHolder, Variant.From<Control>(ref this._drawingToolHolder));
    info.AddProperty(NMapDrawButton.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
    info.AddProperty(NMapDrawButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NMapDrawButton.PropertyName._isDrawing, Variant.From<bool>(ref this._isDrawing));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMapDrawButton.PropertyName._drawingToolHolder, ref variant1))
      this._drawingToolHolder = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NMapDrawButton.PropertyName._icon, ref variant2))
      this._icon = ((Variant) ref variant2).As<TextureRect>();
    Variant variant3;
    if (info.TryGetProperty(NMapDrawButton.PropertyName._tween, ref variant3))
      this._tween = ((Variant) ref variant3).As<Tween>();
    Variant variant4;
    if (!info.TryGetProperty(NMapDrawButton.PropertyName._isDrawing, ref variant4))
      return;
    this._isDrawing = ((Variant) ref variant4).As<bool>();
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName SetIsDrawing = StringName.op_Implicit(nameof (SetIsDrawing));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public static readonly StringName OnControllerUpdated = StringName.op_Implicit(nameof (OnControllerUpdated));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public static readonly StringName _drawingToolHolder = StringName.op_Implicit(nameof (_drawingToolHolder));
    public static readonly StringName _icon = StringName.op_Implicit(nameof (_icon));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _isDrawing = StringName.op_Implicit(nameof (_isDrawing));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
