// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NMainMenuBg
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;

[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NMainMenuBg.cs")]
public class NMainMenuBg : Control
{
  private Window _window;
  private Control _bg;
  private Node2D _logo;
  private Tween? _logoTween;
  private static readonly Vector2 _defaultBgScale = Vector2.op_Multiply(Vector2.One, 1.01f);
  private const float _bgScaleRatioThreshold = 1.5f;

  public override void _Ready()
  {
    this._bg = ((Node) this).GetNode<Control>(NodePath.op_Implicit("BgContainer"));
    this._logo = ((Node) this).GetNode<Node2D>(NodePath.op_Implicit("%Logo"));
    this._window = ((Node) this).GetTree().Root;
    ((GodotObject) this._window).Connect(Viewport.SignalName.SizeChanged, Callable.From(new Action(this.OnWindowChange)), 0U);
  }

  private void OnWindowChange()
  {
    this.ScaleBgIfNarrow((float) this._window.Size.X / (float) this._window.Size.Y);
  }

  private void ScaleBgIfNarrow(float ratio)
  {
    if ((double) ratio < 1.5)
      this._bg.Scale = Vector2.op_Multiply(Vector2.One, 1.04f);
    else
      this._bg.Scale = NMainMenuBg._defaultBgScale;
  }

  public void HideLogo()
  {
    this._logoTween?.Kill();
    this._logoTween = ((Node) this).CreateTween();
    this._logoTween.TweenProperty((GodotObject) this._logo, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
  }

  public void ShowLogo()
  {
    this._logoTween?.Kill();
    this._logoTween = ((Node) this).CreateTween();
    this._logoTween.TweenProperty((GodotObject) this._logo, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NMainMenuBg.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMainMenuBg.MethodName.OnWindowChange, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMainMenuBg.MethodName.ScaleBgIfNarrow, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("ratio"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMainMenuBg.MethodName.HideLogo, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMainMenuBg.MethodName.ShowLogo, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMainMenuBg.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenuBg.MethodName.OnWindowChange) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnWindowChange();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenuBg.MethodName.ScaleBgIfNarrow) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ScaleBgIfNarrow(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenuBg.MethodName.HideLogo) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.HideLogo();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMainMenuBg.MethodName.ShowLogo) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.ShowLogo();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMainMenuBg.MethodName._Ready) || StringName.op_Equality(ref method, NMainMenuBg.MethodName.OnWindowChange) || StringName.op_Equality(ref method, NMainMenuBg.MethodName.ScaleBgIfNarrow) || StringName.op_Equality(ref method, NMainMenuBg.MethodName.HideLogo) || StringName.op_Equality(ref method, NMainMenuBg.MethodName.ShowLogo) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMainMenuBg.PropertyName._window))
    {
      this._window = VariantUtils.ConvertTo<Window>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuBg.PropertyName._bg))
    {
      this._bg = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuBg.PropertyName._logo))
    {
      this._logo = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMainMenuBg.PropertyName._logoTween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._logoTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMainMenuBg.PropertyName._window))
    {
      value = VariantUtils.CreateFrom<Window>(ref this._window);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuBg.PropertyName._bg))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._bg);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuBg.PropertyName._logo))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._logo);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMainMenuBg.PropertyName._logoTween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._logoTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NMainMenuBg.PropertyName._window, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenuBg.PropertyName._bg, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenuBg.PropertyName._logo, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenuBg.PropertyName._logoTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NMainMenuBg.PropertyName._window, Variant.From<Window>(ref this._window));
    info.AddProperty(NMainMenuBg.PropertyName._bg, Variant.From<Control>(ref this._bg));
    info.AddProperty(NMainMenuBg.PropertyName._logo, Variant.From<Node2D>(ref this._logo));
    info.AddProperty(NMainMenuBg.PropertyName._logoTween, Variant.From<Tween>(ref this._logoTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMainMenuBg.PropertyName._window, ref variant1))
      this._window = ((Variant) ref variant1).As<Window>();
    Variant variant2;
    if (info.TryGetProperty(NMainMenuBg.PropertyName._bg, ref variant2))
      this._bg = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NMainMenuBg.PropertyName._logo, ref variant3))
      this._logo = ((Variant) ref variant3).As<Node2D>();
    Variant variant4;
    if (!info.TryGetProperty(NMainMenuBg.PropertyName._logoTween, ref variant4))
      return;
    this._logoTween = ((Variant) ref variant4).As<Tween>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnWindowChange = StringName.op_Implicit(nameof (OnWindowChange));
    public static readonly StringName ScaleBgIfNarrow = StringName.op_Implicit(nameof (ScaleBgIfNarrow));
    public static readonly StringName HideLogo = StringName.op_Implicit(nameof (HideLogo));
    public static readonly StringName ShowLogo = StringName.op_Implicit(nameof (ShowLogo));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _window = StringName.op_Implicit(nameof (_window));
    public static readonly StringName _bg = StringName.op_Implicit(nameof (_bg));
    public static readonly StringName _logo = StringName.op_Implicit(nameof (_logo));
    public static readonly StringName _logoTween = StringName.op_Implicit(nameof (_logoTween));
  }

  public class SignalName : Control.SignalName
  {
  }
}
