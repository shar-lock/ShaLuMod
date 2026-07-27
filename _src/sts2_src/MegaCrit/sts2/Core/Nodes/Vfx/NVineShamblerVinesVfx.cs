// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NVineShamblerVinesVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NVineShamblerVinesVfx.cs")]
public class NVineShamblerVinesVfx : Node2D
{
  private Node2D _frontVinesNode;
  private MegaSprite _frontVinesAnimController;
  private Node2D _backVinesNode;
  private MegaSprite _backVinesAnimController;
  private GpuParticles2D _dirtBlast1;
  private GpuParticles2D _dirtBlast2;
  private GpuParticles2D _dirtBlast3;
  private GpuParticles2D _dirtBlast4;

  public override void _Ready()
  {
    this._frontVinesNode = ((Node) this).GetNode<Node2D>(NodePath.op_Implicit("VinesFront"));
    this._frontVinesAnimController = new MegaSprite(Variant.op_Implicit((GodotObject) this._frontVinesNode));
    this._frontVinesAnimController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnFrontEvent)));
    this._frontVinesAnimController.ConnectAnimationCompleted(Callable.From<GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject>(this.AnimationEnded)));
    this._backVinesNode = ((Node) this).GetNode<Node2D>(NodePath.op_Implicit("VinesBackScene/VinesBack"));
    this._backVinesAnimController = new MegaSprite(Variant.op_Implicit((GodotObject) this._backVinesNode));
    this._dirtBlast1 = ((Node) this).GetNode<GpuParticles2D>(NodePath.op_Implicit("DirtBlast1"));
    this._dirtBlast3 = ((Node) this).GetNode<GpuParticles2D>(NodePath.op_Implicit("DirtBlast3"));
    this._dirtBlast2 = ((Node) this).GetNode<GpuParticles2D>(NodePath.op_Implicit("VinesBackScene/DirtBlast2"));
    this._dirtBlast4 = ((Node) this).GetNode<GpuParticles2D>(NodePath.op_Implicit("VinesBackScene/DirtBlast4"));
    this._dirtBlast1.Emitting = false;
    this._dirtBlast1.OneShot = true;
    this._dirtBlast2.Emitting = false;
    this._dirtBlast2.OneShot = true;
    this._dirtBlast3.Emitting = false;
    this._dirtBlast3.OneShot = true;
    this._dirtBlast4.Emitting = false;
    this._dirtBlast4.OneShot = true;
    Vector2 backVineOffset = Vector2.op_Subtraction(this._backVinesNode.GlobalPosition, this._frontVinesNode.GlobalPosition);
    ((Node) this._backVinesNode).Reparent((Node) NCombatRoom.Instance.BackCombatVfxContainer, true);
    Callable callable = Callable.From((Action) (() => this._backVinesNode.GlobalPosition = Vector2.op_Addition(this._frontVinesNode.GlobalPosition, backVineOffset)));
    ((Callable) ref callable).CallDeferred(Array.Empty<Variant>());
    ((Node) this).RunWhenSpineReady(this._frontVinesAnimController, (Action<MegaAnimationState>) (animState => animState.SetAnimation("animation")));
    ((Node) this).RunWhenSpineReady(this._backVinesAnimController, (Action<MegaAnimationState>) (animState => animState.SetAnimation("animation")));
  }

  private void OnFrontEvent(
    GodotObject _,
    GodotObject __,
    GodotObject ___,
    GodotObject spineEvent)
  {
    switch (new MegaEvent(Variant.op_Implicit(spineEvent)).GetData().GetEventName())
    {
      case "dirt_1":
        this._dirtBlast1.Restart();
        break;
      case "dirt_2":
        this._dirtBlast2.Restart();
        break;
      case "dirt_3":
        this._dirtBlast3.Restart();
        break;
      case "dirt_4":
        this._dirtBlast4.Restart();
        break;
    }
  }

  private void AnimationEnded(GodotObject _, GodotObject __, GodotObject ___)
  {
    ((Node) this).QueueFreeSafely();
    ((Node) this._backVinesNode).QueueFreeSafely();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NVineShamblerVinesVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NVineShamblerVinesVfx.MethodName.OnFrontEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("__"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("___"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NVineShamblerVinesVfx.MethodName.AnimationEnded, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("__"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("___"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NVineShamblerVinesVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NVineShamblerVinesVfx.MethodName.OnFrontEvent) && ((NativeVariantPtrArgs) ref args).Count == 4)
    {
      this.OnFrontEvent(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[3]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NVineShamblerVinesVfx.MethodName.AnimationEnded) || ((NativeVariantPtrArgs) ref args).Count != 3)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.AnimationEnded(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NVineShamblerVinesVfx.MethodName._Ready) || StringName.op_Equality(ref method, NVineShamblerVinesVfx.MethodName.OnFrontEvent) || StringName.op_Equality(ref method, NVineShamblerVinesVfx.MethodName.AnimationEnded) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NVineShamblerVinesVfx.PropertyName._frontVinesNode))
    {
      this._frontVinesNode = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NVineShamblerVinesVfx.PropertyName._backVinesNode))
    {
      this._backVinesNode = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NVineShamblerVinesVfx.PropertyName._dirtBlast1))
    {
      this._dirtBlast1 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NVineShamblerVinesVfx.PropertyName._dirtBlast2))
    {
      this._dirtBlast2 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NVineShamblerVinesVfx.PropertyName._dirtBlast3))
    {
      this._dirtBlast3 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NVineShamblerVinesVfx.PropertyName._dirtBlast4))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._dirtBlast4 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NVineShamblerVinesVfx.PropertyName._frontVinesNode))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._frontVinesNode);
      return true;
    }
    if (StringName.op_Equality(ref name, NVineShamblerVinesVfx.PropertyName._backVinesNode))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._backVinesNode);
      return true;
    }
    if (StringName.op_Equality(ref name, NVineShamblerVinesVfx.PropertyName._dirtBlast1))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._dirtBlast1);
      return true;
    }
    if (StringName.op_Equality(ref name, NVineShamblerVinesVfx.PropertyName._dirtBlast2))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._dirtBlast2);
      return true;
    }
    if (StringName.op_Equality(ref name, NVineShamblerVinesVfx.PropertyName._dirtBlast3))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._dirtBlast3);
      return true;
    }
    if (!StringName.op_Equality(ref name, NVineShamblerVinesVfx.PropertyName._dirtBlast4))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._dirtBlast4);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NVineShamblerVinesVfx.PropertyName._frontVinesNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NVineShamblerVinesVfx.PropertyName._backVinesNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NVineShamblerVinesVfx.PropertyName._dirtBlast1, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NVineShamblerVinesVfx.PropertyName._dirtBlast2, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NVineShamblerVinesVfx.PropertyName._dirtBlast3, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NVineShamblerVinesVfx.PropertyName._dirtBlast4, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NVineShamblerVinesVfx.PropertyName._frontVinesNode, Variant.From<Node2D>(ref this._frontVinesNode));
    info.AddProperty(NVineShamblerVinesVfx.PropertyName._backVinesNode, Variant.From<Node2D>(ref this._backVinesNode));
    info.AddProperty(NVineShamblerVinesVfx.PropertyName._dirtBlast1, Variant.From<GpuParticles2D>(ref this._dirtBlast1));
    info.AddProperty(NVineShamblerVinesVfx.PropertyName._dirtBlast2, Variant.From<GpuParticles2D>(ref this._dirtBlast2));
    info.AddProperty(NVineShamblerVinesVfx.PropertyName._dirtBlast3, Variant.From<GpuParticles2D>(ref this._dirtBlast3));
    info.AddProperty(NVineShamblerVinesVfx.PropertyName._dirtBlast4, Variant.From<GpuParticles2D>(ref this._dirtBlast4));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NVineShamblerVinesVfx.PropertyName._frontVinesNode, ref variant1))
      this._frontVinesNode = ((Variant) ref variant1).As<Node2D>();
    Variant variant2;
    if (info.TryGetProperty(NVineShamblerVinesVfx.PropertyName._backVinesNode, ref variant2))
      this._backVinesNode = ((Variant) ref variant2).As<Node2D>();
    Variant variant3;
    if (info.TryGetProperty(NVineShamblerVinesVfx.PropertyName._dirtBlast1, ref variant3))
      this._dirtBlast1 = ((Variant) ref variant3).As<GpuParticles2D>();
    Variant variant4;
    if (info.TryGetProperty(NVineShamblerVinesVfx.PropertyName._dirtBlast2, ref variant4))
      this._dirtBlast2 = ((Variant) ref variant4).As<GpuParticles2D>();
    Variant variant5;
    if (info.TryGetProperty(NVineShamblerVinesVfx.PropertyName._dirtBlast3, ref variant5))
      this._dirtBlast3 = ((Variant) ref variant5).As<GpuParticles2D>();
    Variant variant6;
    if (!info.TryGetProperty(NVineShamblerVinesVfx.PropertyName._dirtBlast4, ref variant6))
      return;
    this._dirtBlast4 = ((Variant) ref variant6).As<GpuParticles2D>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnFrontEvent = StringName.op_Implicit(nameof (OnFrontEvent));
    public static readonly StringName AnimationEnded = StringName.op_Implicit(nameof (AnimationEnded));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _frontVinesNode = StringName.op_Implicit(nameof (_frontVinesNode));
    public static readonly StringName _backVinesNode = StringName.op_Implicit(nameof (_backVinesNode));
    public static readonly StringName _dirtBlast1 = StringName.op_Implicit(nameof (_dirtBlast1));
    public static readonly StringName _dirtBlast2 = StringName.op_Implicit(nameof (_dirtBlast2));
    public static readonly StringName _dirtBlast3 = StringName.op_Implicit(nameof (_dirtBlast3));
    public static readonly StringName _dirtBlast4 = StringName.op_Implicit(nameof (_dirtBlast4));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
