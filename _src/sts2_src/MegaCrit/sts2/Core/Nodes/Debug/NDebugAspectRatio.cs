// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Debug.NDebugAspectRatio
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Debug;

[ScriptPath("res://src/Core/Nodes/Debug/NDebugAspectRatio.cs")]
public class NDebugAspectRatio : Control
{
  private Window _window;
  private Label _infoLabel;
  private TextureRect _bg;
  private const float _maxNarrowRatio = 1.33333337f;
  private const float _maxWideRatio = 2.38888884f;
  private static readonly Vector2 _defaultBgScale = Vector2.op_Multiply(Vector2.One, 1.01f);
  private const float _bgScaleRatioThreshold = 1.5f;

  public override void _Ready()
  {
    this._bg = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("EventBg"));
    this._infoLabel = ((Node) this).GetNode<Label>(NodePath.op_Implicit("Anchors/Label"));
    this._window = ((Node) this).GetTree().Root;
    ((GodotObject) this._window).Connect(Viewport.SignalName.SizeChanged, Callable.From(new Action(this.OnWindowChange)), 0U);
  }

  private void OnWindowChange()
  {
    float ratio = (float) this._window.Size.X / (float) this._window.Size.Y;
    string str = ratio.ToString("0.000");
    this.ScaleBgIfNarrow(ratio);
    if ((double) ratio > 2.3888888359069824)
    {
      this._window.ContentScaleAspect = (Window.ContentScaleAspectEnum) 2L;
      this._window.ContentScaleSize = new Vector2I(2580, 1080);
      this._infoLabel.Text = $"{str}: {this._window.Size}";
      ((CanvasItem) this._infoLabel).Modulate = StsColors.red;
    }
    else if ((double) ratio < 1.3333333730697632)
    {
      this._window.ContentScaleAspect = (Window.ContentScaleAspectEnum) 3L;
      this._window.ContentScaleSize = new Vector2I(1680, 1260);
      this._infoLabel.Text = $"{str}: {this._window.Size}";
      ((CanvasItem) this._infoLabel).Modulate = StsColors.red;
    }
    else
    {
      this._window.ContentScaleAspect = (Window.ContentScaleAspectEnum) 4L;
      this._window.ContentScaleSize = new Vector2I(1680, 1080);
      this._infoLabel.Text = $"{str}: {this._window.Size}";
      ((CanvasItem) this._infoLabel).Modulate = StsColors.cream;
    }
  }

  private void ScaleBgIfNarrow(float ratio)
  {
    if ((double) ratio < 1.5)
      ((Control) this._bg).Scale = Vector2.op_Multiply(Vector2.One, 1.05f);
    else
      ((Control) this._bg).Scale = NDebugAspectRatio._defaultBgScale;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NDebugAspectRatio.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDebugAspectRatio.MethodName.OnWindowChange, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDebugAspectRatio.MethodName.ScaleBgIfNarrow, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("ratio"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NDebugAspectRatio.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDebugAspectRatio.MethodName.OnWindowChange) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnWindowChange();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NDebugAspectRatio.MethodName.ScaleBgIfNarrow) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.ScaleBgIfNarrow(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NDebugAspectRatio.MethodName._Ready) || StringName.op_Equality(ref method, NDebugAspectRatio.MethodName.OnWindowChange) || StringName.op_Equality(ref method, NDebugAspectRatio.MethodName.ScaleBgIfNarrow) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDebugAspectRatio.PropertyName._window))
    {
      this._window = VariantUtils.ConvertTo<Window>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDebugAspectRatio.PropertyName._infoLabel))
    {
      this._infoLabel = VariantUtils.ConvertTo<Label>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDebugAspectRatio.PropertyName._bg))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._bg = VariantUtils.ConvertTo<TextureRect>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDebugAspectRatio.PropertyName._window))
    {
      value = VariantUtils.CreateFrom<Window>(ref this._window);
      return true;
    }
    if (StringName.op_Equality(ref name, NDebugAspectRatio.PropertyName._infoLabel))
    {
      value = VariantUtils.CreateFrom<Label>(ref this._infoLabel);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDebugAspectRatio.PropertyName._bg))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<TextureRect>(ref this._bg);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NDebugAspectRatio.PropertyName._window, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDebugAspectRatio.PropertyName._infoLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDebugAspectRatio.PropertyName._bg, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NDebugAspectRatio.PropertyName._window, Variant.From<Window>(ref this._window));
    info.AddProperty(NDebugAspectRatio.PropertyName._infoLabel, Variant.From<Label>(ref this._infoLabel));
    info.AddProperty(NDebugAspectRatio.PropertyName._bg, Variant.From<TextureRect>(ref this._bg));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NDebugAspectRatio.PropertyName._window, ref variant1))
      this._window = ((Variant) ref variant1).As<Window>();
    Variant variant2;
    if (info.TryGetProperty(NDebugAspectRatio.PropertyName._infoLabel, ref variant2))
      this._infoLabel = ((Variant) ref variant2).As<Label>();
    Variant variant3;
    if (!info.TryGetProperty(NDebugAspectRatio.PropertyName._bg, ref variant3))
      return;
    this._bg = ((Variant) ref variant3).As<TextureRect>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnWindowChange = StringName.op_Implicit(nameof (OnWindowChange));
    public static readonly StringName ScaleBgIfNarrow = StringName.op_Implicit(nameof (ScaleBgIfNarrow));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _window = StringName.op_Implicit(nameof (_window));
    public static readonly StringName _infoLabel = StringName.op_Implicit(nameof (_infoLabel));
    public static readonly StringName _bg = StringName.op_Implicit(nameof (_bg));
  }

  public class SignalName : Control.SignalName
  {
  }
}
