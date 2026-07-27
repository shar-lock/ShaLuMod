// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Reaction.NReactionWheelWedge
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Reaction;

[ScriptPath("res://src/Core/Nodes/Reaction/NReactionWheelWedge.cs")]
public class NReactionWheelWedge : TextureRect
{
  private static readonly Color _defaultColor = new Color("e0f9ff40");
  private static readonly Color _selectedColor = new Color("c2f3ffc0");
  private TextureRect _textureRect;
  private Vector2 _normal;
  private Tween? _tween;
  private Vector2 _defaultPosition;

  public Texture2D Reaction => this._textureRect.Texture;

  public override void _Ready()
  {
    this._textureRect = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("TextureRect"));
    this._defaultPosition = ((Control) this).Position;
  }

  public void OnSelected()
  {
    Vector2 right = Vector2.Right;
    Vector2 vector2 = ((Vector2) ref right).Rotated(((Control) this).Rotation);
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween();
    this._tween.SetParallel(true);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position"), Variant.op_Implicit(Vector2.op_Addition(this._defaultPosition, Vector2.op_Multiply(vector2, 25f))), 0.05).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("self_modulate"), Variant.op_Implicit(NReactionWheelWedge._selectedColor), 0.05).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  public void OnDeselected()
  {
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween();
    this._tween.SetParallel(true);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position"), Variant.op_Implicit(this._defaultPosition), 0.2).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("self_modulate"), Variant.op_Implicit(NReactionWheelWedge._defaultColor), 0.2).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NReactionWheelWedge.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NReactionWheelWedge.MethodName.OnSelected, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NReactionWheelWedge.MethodName.OnDeselected, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NReactionWheelWedge.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NReactionWheelWedge.MethodName.OnSelected) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSelected();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NReactionWheelWedge.MethodName.OnDeselected) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnDeselected();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NReactionWheelWedge.MethodName._Ready) || StringName.op_Equality(ref method, NReactionWheelWedge.MethodName.OnSelected) || StringName.op_Equality(ref method, NReactionWheelWedge.MethodName.OnDeselected) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NReactionWheelWedge.PropertyName._textureRect))
    {
      this._textureRect = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NReactionWheelWedge.PropertyName._normal))
    {
      this._normal = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NReactionWheelWedge.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NReactionWheelWedge.PropertyName._defaultPosition))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._defaultPosition = VariantUtils.ConvertTo<Vector2>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NReactionWheelWedge.PropertyName.Reaction))
    {
      ref godot_variant local = ref value;
      Texture2D reaction = this.Reaction;
      godot_variant from = VariantUtils.CreateFrom<Texture2D>(ref reaction);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NReactionWheelWedge.PropertyName._textureRect))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._textureRect);
      return true;
    }
    if (StringName.op_Equality(ref name, NReactionWheelWedge.PropertyName._normal))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._normal);
      return true;
    }
    if (StringName.op_Equality(ref name, NReactionWheelWedge.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NReactionWheelWedge.PropertyName._defaultPosition))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Vector2>(ref this._defaultPosition);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NReactionWheelWedge.PropertyName._textureRect, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NReactionWheelWedge.PropertyName._normal, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NReactionWheelWedge.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NReactionWheelWedge.PropertyName._defaultPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NReactionWheelWedge.PropertyName.Reaction, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NReactionWheelWedge.PropertyName._textureRect, Variant.From<TextureRect>(ref this._textureRect));
    info.AddProperty(NReactionWheelWedge.PropertyName._normal, Variant.From<Vector2>(ref this._normal));
    info.AddProperty(NReactionWheelWedge.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NReactionWheelWedge.PropertyName._defaultPosition, Variant.From<Vector2>(ref this._defaultPosition));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NReactionWheelWedge.PropertyName._textureRect, ref variant1))
      this._textureRect = ((Variant) ref variant1).As<TextureRect>();
    Variant variant2;
    if (info.TryGetProperty(NReactionWheelWedge.PropertyName._normal, ref variant2))
      this._normal = ((Variant) ref variant2).As<Vector2>();
    Variant variant3;
    if (info.TryGetProperty(NReactionWheelWedge.PropertyName._tween, ref variant3))
      this._tween = ((Variant) ref variant3).As<Tween>();
    Variant variant4;
    if (!info.TryGetProperty(NReactionWheelWedge.PropertyName._defaultPosition, ref variant4))
      return;
    this._defaultPosition = ((Variant) ref variant4).As<Vector2>();
  }

  public class MethodName : TextureRect.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnSelected = StringName.op_Implicit(nameof (OnSelected));
    public static readonly StringName OnDeselected = StringName.op_Implicit(nameof (OnDeselected));
  }

  public class PropertyName : TextureRect.PropertyName
  {
    public static readonly StringName Reaction = StringName.op_Implicit(nameof (Reaction));
    public static readonly StringName _textureRect = StringName.op_Implicit(nameof (_textureRect));
    public static readonly StringName _normal = StringName.op_Implicit(nameof (_normal));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _defaultPosition = StringName.op_Implicit(nameof (_defaultPosition));
  }

  public class SignalName : TextureRect.SignalName
  {
  }
}
