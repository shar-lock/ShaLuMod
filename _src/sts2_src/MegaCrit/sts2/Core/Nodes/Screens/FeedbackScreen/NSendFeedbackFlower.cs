// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.NSendFeedbackFlower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen;

[ScriptPath("res://src/Core/Nodes/Screens/FeedbackScreen/NSendFeedbackFlower.cs")]
public class NSendFeedbackFlower : Control
{
  private const string _normalImage = "res://images/atlases/compressed.sprites/feedback/flower.tres";
  private const string _noddingImage = "res://images/atlases/compressed.sprites/feedback/flower_happy.tres";
  private const string _anticipationImage = "res://images/atlases/compressed.sprites/feedback/flower_anticipation.tres";
  private Tween? _tween;
  private Vector2 _originalPosition;

  public NSendFeedbackCartoon Cartoon { get; private set; }

  public NSendFeedbackFlower.State MyState { get; private set; }

  public override void _Ready()
  {
    this._originalPosition = this.Position;
    this.Cartoon = ((Node) this).GetNode<NSendFeedbackCartoon>(NodePath.op_Implicit("Flower"));
  }

  public void SetState(NSendFeedbackFlower.State state)
  {
    switch (state)
    {
      case NSendFeedbackFlower.State.Nodding:
        this._tween?.Kill();
        this._tween = ((Node) this).CreateTween();
        this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("rotation"), Variant.op_Implicit(Mathf.DegToRad(8f)), 0.5);
        this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("rotation"), Variant.op_Implicit(Mathf.DegToRad(-8f)), 0.5);
        this._tween.SetLoops(0);
        this.Cartoon.Texture = PreloadManager.Cache.GetTexture2D("res://images/atlases/compressed.sprites/feedback/flower_happy.tres");
        break;
      case NSendFeedbackFlower.State.Anticipation:
        this._tween?.Kill();
        this._tween = (Tween) null;
        this.Cartoon.Texture = PreloadManager.Cache.GetTexture2D("res://images/atlases/compressed.sprites/feedback/flower_anticipation.tres");
        this._tween = ((Node) this).CreateTween();
        this._tween.TweenInterval(0.05000000074505806);
        this._tween.TweenCallback(Callable.From(new Action(this.SetRandomPosition)));
        this._tween.SetLoops(0);
        break;
      case NSendFeedbackFlower.State.NoddingFast:
        this._tween?.Kill();
        this._tween = ((Node) this).CreateTween();
        this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("rotation"), Variant.op_Implicit(Mathf.DegToRad(8f)), 0.2);
        this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("rotation"), Variant.op_Implicit(Mathf.DegToRad(-8f)), 0.2);
        this._tween.SetLoops(0);
        this.Cartoon.Texture = PreloadManager.Cache.GetTexture2D("res://images/atlases/compressed.sprites/feedback/flower_happy.tres");
        break;
      default:
        this.Rotation = 0.0f;
        this._tween?.Kill();
        this._tween = (Tween) null;
        this.Cartoon.Texture = PreloadManager.Cache.GetTexture2D("res://images/atlases/compressed.sprites/feedback/flower.tres");
        break;
    }
    this.MyState = state;
  }

  private void SetRandomPosition()
  {
    this.Position = Vector2.op_Addition(this._originalPosition, new Vector2(Rng.Chaotic.NextFloat(-3f, 3f), Rng.Chaotic.NextFloat(-3f, 3f)));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NSendFeedbackFlower.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSendFeedbackFlower.MethodName.SetState, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("state"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NSendFeedbackFlower.MethodName.SetRandomPosition, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSendFeedbackFlower.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSendFeedbackFlower.MethodName.SetState) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetState(VariantUtils.ConvertTo<NSendFeedbackFlower.State>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NSendFeedbackFlower.MethodName.SetRandomPosition) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.SetRandomPosition();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSendFeedbackFlower.MethodName._Ready) || StringName.op_Equality(ref method, NSendFeedbackFlower.MethodName.SetState) || StringName.op_Equality(ref method, NSendFeedbackFlower.MethodName.SetRandomPosition) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSendFeedbackFlower.PropertyName.Cartoon))
    {
      this.Cartoon = VariantUtils.ConvertTo<NSendFeedbackCartoon>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackFlower.PropertyName.MyState))
    {
      this.MyState = VariantUtils.ConvertTo<NSendFeedbackFlower.State>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackFlower.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSendFeedbackFlower.PropertyName._originalPosition))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._originalPosition = VariantUtils.ConvertTo<Vector2>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSendFeedbackFlower.PropertyName.Cartoon))
    {
      ref godot_variant local = ref value;
      NSendFeedbackCartoon cartoon = this.Cartoon;
      godot_variant from = VariantUtils.CreateFrom<NSendFeedbackCartoon>(ref cartoon);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackFlower.PropertyName.MyState))
    {
      ref godot_variant local = ref value;
      NSendFeedbackFlower.State myState = this.MyState;
      godot_variant from = VariantUtils.CreateFrom<NSendFeedbackFlower.State>(ref myState);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackFlower.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSendFeedbackFlower.PropertyName._originalPosition))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Vector2>(ref this._originalPosition);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NSendFeedbackFlower.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NSendFeedbackFlower.PropertyName._originalPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSendFeedbackFlower.PropertyName.Cartoon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NSendFeedbackFlower.PropertyName.MyState, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo1 = info;
    StringName cartoon1 = NSendFeedbackFlower.PropertyName.Cartoon;
    NSendFeedbackCartoon cartoon2 = this.Cartoon;
    Variant variant1 = Variant.From<NSendFeedbackCartoon>(ref cartoon2);
    serializationInfo1.AddProperty(cartoon1, variant1);
    GodotSerializationInfo serializationInfo2 = info;
    StringName state = NSendFeedbackFlower.PropertyName.MyState;
    NSendFeedbackFlower.State myState = this.MyState;
    Variant variant2 = Variant.From<NSendFeedbackFlower.State>(ref myState);
    serializationInfo2.AddProperty(state, variant2);
    info.AddProperty(NSendFeedbackFlower.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NSendFeedbackFlower.PropertyName._originalPosition, Variant.From<Vector2>(ref this._originalPosition));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NSendFeedbackFlower.PropertyName.Cartoon, ref variant1))
      this.Cartoon = ((Variant) ref variant1).As<NSendFeedbackCartoon>();
    Variant variant2;
    if (info.TryGetProperty(NSendFeedbackFlower.PropertyName.MyState, ref variant2))
      this.MyState = ((Variant) ref variant2).As<NSendFeedbackFlower.State>();
    Variant variant3;
    if (info.TryGetProperty(NSendFeedbackFlower.PropertyName._tween, ref variant3))
      this._tween = ((Variant) ref variant3).As<Tween>();
    Variant variant4;
    if (!info.TryGetProperty(NSendFeedbackFlower.PropertyName._originalPosition, ref variant4))
      return;
    this._originalPosition = ((Variant) ref variant4).As<Vector2>();
  }

  public enum State
  {
    None,
    Nodding,
    Anticipation,
    NoddingFast,
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName SetState = StringName.op_Implicit(nameof (SetState));
    public static readonly StringName SetRandomPosition = StringName.op_Implicit(nameof (SetRandomPosition));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName Cartoon = StringName.op_Implicit(nameof (Cartoon));
    public static readonly StringName MyState = StringName.op_Implicit(nameof (MyState));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _originalPosition = StringName.op_Implicit(nameof (_originalPosition));
  }

  public class SignalName : Control.SignalName
  {
  }
}
