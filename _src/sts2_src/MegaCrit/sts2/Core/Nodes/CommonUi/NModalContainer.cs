// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.CommonUi.NModalContainer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.CommonUi;

[ScriptPath("res://src/Core/Nodes/CommonUi/NModalContainer.cs")]
public class NModalContainer : Control
{
  private ColorRect _backstop;
  private Tween? _backstopTween;

  public static NModalContainer? Instance { get; private set; }

  public IScreenContext? OpenModal { get; private set; }

  public override void _Ready()
  {
    if (NModalContainer.Instance != null)
    {
      Log.Error("NModalContainer already exists.");
      ((Node) this).QueueFreeSafely();
    }
    else
    {
      NModalContainer.Instance = this;
      this._backstop = ((Node) this).GetNode<ColorRect>(NodePath.op_Implicit("Backstop"));
    }
  }

  public void Add(Node modalToCreate, bool showBackstop = true)
  {
    if (this.OpenModal != null)
    {
      Log.Warn("There's another modal already open.");
    }
    else
    {
      this.OpenModal = (IScreenContext) modalToCreate;
      ((Node) this).AddChildSafely(modalToCreate);
      ActiveScreenContext.Instance.Update();
      if (!showBackstop)
        return;
      this.ShowBackstop();
    }
  }

  public void Clear()
  {
    foreach (Node child in ((Node) this).GetChildren(false))
    {
      if (child != this._backstop)
        child.QueueFreeSafely();
    }
    this.OpenModal = (IScreenContext) null;
    ActiveScreenContext.Instance.Update();
    this.HideBackstop();
  }

  public void ShowBackstop()
  {
    this.MouseFilter = (Control.MouseFilterEnum) 0L;
    ((CanvasItem) this._backstop).Visible = true;
    this._backstopTween?.Kill();
    this._backstopTween = ((Node) this).CreateTween();
    this._backstopTween.TweenProperty((GodotObject) this._backstop, NodePath.op_Implicit("color:a"), Variant.op_Implicit(0.85f), 0.3);
  }

  public void HideBackstop()
  {
    this.MouseFilter = (Control.MouseFilterEnum) 2L;
    this._backstopTween?.Kill();
    this._backstopTween = ((Node) this).CreateTween();
    this._backstopTween.TweenProperty((GodotObject) this._backstop, NodePath.op_Implicit("color:a"), Variant.op_Implicit(0.0f), 0.3);
    this._backstopTween.TweenCallback(Callable.From<bool>((Func<bool>) (() => ((CanvasItem) this._backstop).Visible = false)));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NModalContainer.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NModalContainer.MethodName.Add, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("modalToCreate"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node"), false),
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("showBackstop"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NModalContainer.MethodName.Clear, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NModalContainer.MethodName.ShowBackstop, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NModalContainer.MethodName.HideBackstop, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NModalContainer.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NModalContainer.MethodName.Add) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.Add(VariantUtils.ConvertTo<Node>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NModalContainer.MethodName.Clear) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Clear();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NModalContainer.MethodName.ShowBackstop) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ShowBackstop();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NModalContainer.MethodName.HideBackstop) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.HideBackstop();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NModalContainer.MethodName._Ready) || StringName.op_Equality(ref method, NModalContainer.MethodName.Add) || StringName.op_Equality(ref method, NModalContainer.MethodName.Clear) || StringName.op_Equality(ref method, NModalContainer.MethodName.ShowBackstop) || StringName.op_Equality(ref method, NModalContainer.MethodName.HideBackstop) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NModalContainer.PropertyName._backstop))
    {
      this._backstop = VariantUtils.ConvertTo<ColorRect>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NModalContainer.PropertyName._backstopTween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._backstopTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NModalContainer.PropertyName._backstop))
    {
      value = VariantUtils.CreateFrom<ColorRect>(ref this._backstop);
      return true;
    }
    if (!StringName.op_Equality(ref name, NModalContainer.PropertyName._backstopTween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._backstopTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NModalContainer.PropertyName._backstop, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NModalContainer.PropertyName._backstopTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NModalContainer.PropertyName._backstop, Variant.From<ColorRect>(ref this._backstop));
    info.AddProperty(NModalContainer.PropertyName._backstopTween, Variant.From<Tween>(ref this._backstopTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NModalContainer.PropertyName._backstop, ref variant1))
      this._backstop = ((Variant) ref variant1).As<ColorRect>();
    Variant variant2;
    if (!info.TryGetProperty(NModalContainer.PropertyName._backstopTween, ref variant2))
      return;
    this._backstopTween = ((Variant) ref variant2).As<Tween>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName Add = StringName.op_Implicit(nameof (Add));
    public static readonly StringName Clear = StringName.op_Implicit(nameof (Clear));
    public static readonly StringName ShowBackstop = StringName.op_Implicit(nameof (ShowBackstop));
    public static readonly StringName HideBackstop = StringName.op_Implicit(nameof (HideBackstop));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _backstop = StringName.op_Implicit(nameof (_backstop));
    public static readonly StringName _backstopTween = StringName.op_Implicit(nameof (_backstopTween));
  }

  public class SignalName : Control.SignalName
  {
  }
}
