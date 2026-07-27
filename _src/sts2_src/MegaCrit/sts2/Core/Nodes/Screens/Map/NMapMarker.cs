// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Map.NMapMarker
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Map;

[ScriptPath("res://src/Core/Nodes/Screens/Map/NMapMarker.cs")]
public class NMapMarker : TextureRect
{
  private Tween? _tween;
  private Vector2 _posOffset;
  private bool _isEnabled;

  public override void _Ready()
  {
    this._posOffset = new Vector2((float) (-(double) ((Control) this).Size.X / 2.0), -35f);
  }

  public void Initialize(Player player)
  {
    this._isEnabled = player.RunState.Players.Count == 1;
    this.Texture = (Texture2D) player.Character.MapMarker;
    ((CanvasItem) this).Visible = false;
  }

  public void ResetMapPoint() => ((CanvasItem) this).Visible = false;

  public void HideMapPoint()
  {
    if (!this._isEnabled)
      return;
    Tween tween = this._tween;
    if (tween != null)
      tween.FastForwardToCompletion();
    this._tween = ((Node) this).CreateTween();
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.Zero), 0.20000000298023224).From(Variant.op_Implicit(Vector2.One));
    this._tween.TweenCallback(Callable.From((Action) (() => ((CanvasItem) this).Visible = false)));
  }

  public void SetMapPoint(NMapPoint node)
  {
    if (!this._isEnabled)
      return;
    Tween tween = this._tween;
    if (tween != null)
      tween.FastForwardToCompletion();
    if (((CanvasItem) this).Visible)
      return;
    ((CanvasItem) this).Visible = true;
    Vector2 vector2;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2).\u002Ector(node.Size.X / 2f, 0.0f);
    ((Control) this).Position = Vector2.op_Addition(Vector2.op_Addition(node.Position, vector2), this._posOffset);
    this._tween = ((Node) this).CreateTween();
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.20000000298023224).From(Variant.op_Implicit(Vector2.Down));
    this._tween.Parallel().TweenProperty((GodotObject) this, NodePath.op_Implicit("position"), Variant.op_Implicit(Vector2.op_Addition(((Control) this).Position, Vector2.op_Multiply(Vector2.Up, 25f))), 0.20000000298023224).SetEase((Tween.EaseType) 0L).SetTrans((Tween.TransitionType) 1L).FromCurrent();
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position"), Variant.op_Implicit(((Control) this).Position), 0.75).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 6L);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NMapMarker.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapMarker.MethodName.ResetMapPoint, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapMarker.MethodName.HideMapPoint, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapMarker.MethodName.SetMapPoint, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("node"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMapMarker.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapMarker.MethodName.ResetMapPoint) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ResetMapPoint();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapMarker.MethodName.HideMapPoint) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.HideMapPoint();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMapMarker.MethodName.SetMapPoint) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.SetMapPoint(VariantUtils.ConvertTo<NMapPoint>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMapMarker.MethodName._Ready) || StringName.op_Equality(ref method, NMapMarker.MethodName.ResetMapPoint) || StringName.op_Equality(ref method, NMapMarker.MethodName.HideMapPoint) || StringName.op_Equality(ref method, NMapMarker.MethodName.SetMapPoint) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMapMarker.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapMarker.PropertyName._posOffset))
    {
      this._posOffset = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMapMarker.PropertyName._isEnabled))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._isEnabled = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMapMarker.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapMarker.PropertyName._posOffset))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._posOffset);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMapMarker.PropertyName._isEnabled))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._isEnabled);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NMapMarker.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NMapMarker.PropertyName._posOffset, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NMapMarker.PropertyName._isEnabled, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NMapMarker.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NMapMarker.PropertyName._posOffset, Variant.From<Vector2>(ref this._posOffset));
    info.AddProperty(NMapMarker.PropertyName._isEnabled, Variant.From<bool>(ref this._isEnabled));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMapMarker.PropertyName._tween, ref variant1))
      this._tween = ((Variant) ref variant1).As<Tween>();
    Variant variant2;
    if (info.TryGetProperty(NMapMarker.PropertyName._posOffset, ref variant2))
      this._posOffset = ((Variant) ref variant2).As<Vector2>();
    Variant variant3;
    if (!info.TryGetProperty(NMapMarker.PropertyName._isEnabled, ref variant3))
      return;
    this._isEnabled = ((Variant) ref variant3).As<bool>();
  }

  public class MethodName : TextureRect.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName ResetMapPoint = StringName.op_Implicit(nameof (ResetMapPoint));
    public static readonly StringName HideMapPoint = StringName.op_Implicit(nameof (HideMapPoint));
    public static readonly StringName SetMapPoint = StringName.op_Implicit(nameof (SetMapPoint));
  }

  public class PropertyName : TextureRect.PropertyName
  {
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _posOffset = StringName.op_Implicit(nameof (_posOffset));
    public static readonly StringName _isEnabled = StringName.op_Implicit(nameof (_isEnabled));
  }

  public class SignalName : TextureRect.SignalName
  {
  }
}
