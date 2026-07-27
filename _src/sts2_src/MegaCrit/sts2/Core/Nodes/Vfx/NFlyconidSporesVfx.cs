// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NFlyconidSporesVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[GlobalClass]
[ScriptPath("res://src/Core/Nodes/Vfx/NFlyconidSporesVfx.cs")]
public class NFlyconidSporesVfx : Node
{
  private CpuParticles2D _frailSpores;
  private CpuParticles2D _vulnerableSpores;
  private Node2D _parent;
  private MegaSprite _animController;

  public override void _Ready()
  {
    this._parent = this.GetParent<Node2D>();
    this._animController = new MegaSprite(Variant.op_Implicit((GodotObject) this._parent));
    this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
    this._animController.ConnectAnimationStarted(Callable.From<GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject>(this.OnAnimationStart)));
    this._frailSpores = ((Node) this._parent).GetNode<CpuParticles2D>(NodePath.op_Implicit("SporesAttach/FlyconidSporesVfx/FrailSpores"));
    this._frailSpores.Emitting = false;
    this._vulnerableSpores = ((Node) this._parent).GetNode<CpuParticles2D>(NodePath.op_Implicit("SporesAttach/FlyconidSporesVfx/VulnerableSpores"));
    this._vulnerableSpores.Emitting = false;
  }

  private void OnAnimationEvent(
    GodotObject _,
    GodotObject __,
    GodotObject ___,
    GodotObject spineEvent)
  {
    switch (new MegaEvent(Variant.op_Implicit(spineEvent)).GetData().GetEventName())
    {
      case "cast_start":
        this._frailSpores.Restart();
        this._vulnerableSpores.Restart();
        break;
      case "cast_end":
        this._frailSpores.Emitting = false;
        this._vulnerableSpores.Emitting = false;
        break;
    }
  }

  public void SetSporeTypeIsVulnerable(bool isVulnerable)
  {
    ((CanvasItem) this._frailSpores).Visible = !isVulnerable;
    ((CanvasItem) this._vulnerableSpores).Visible = isVulnerable;
  }

  private void OnAnimationStart(
    GodotObject spineSprite,
    GodotObject animationState,
    GodotObject trackEntry)
  {
    if (!(new MegaAnimationState(Variant.op_Implicit(animationState)).GetCurrentAnimationName() != "attack"))
      return;
    this._frailSpores.Emitting = false;
    this._vulnerableSpores.Emitting = false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NFlyconidSporesVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NFlyconidSporesVfx.MethodName.OnAnimationEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("__"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("___"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NFlyconidSporesVfx.MethodName.SetSporeTypeIsVulnerable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("isVulnerable"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NFlyconidSporesVfx.MethodName.OnAnimationStart, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineSprite"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("animationState"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("trackEntry"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NFlyconidSporesVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NFlyconidSporesVfx.MethodName.OnAnimationEvent) && ((NativeVariantPtrArgs) ref args).Count == 4)
    {
      this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[3]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NFlyconidSporesVfx.MethodName.SetSporeTypeIsVulnerable) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetSporeTypeIsVulnerable(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NFlyconidSporesVfx.MethodName.OnAnimationStart) || ((NativeVariantPtrArgs) ref args).Count != 3)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnAnimationStart(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NFlyconidSporesVfx.MethodName._Ready) || StringName.op_Equality(ref method, NFlyconidSporesVfx.MethodName.OnAnimationEvent) || StringName.op_Equality(ref method, NFlyconidSporesVfx.MethodName.SetSporeTypeIsVulnerable) || StringName.op_Equality(ref method, NFlyconidSporesVfx.MethodName.OnAnimationStart) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NFlyconidSporesVfx.PropertyName._frailSpores))
    {
      this._frailSpores = VariantUtils.ConvertTo<CpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NFlyconidSporesVfx.PropertyName._vulnerableSpores))
    {
      this._vulnerableSpores = VariantUtils.ConvertTo<CpuParticles2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NFlyconidSporesVfx.PropertyName._parent))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NFlyconidSporesVfx.PropertyName._frailSpores))
    {
      value = VariantUtils.CreateFrom<CpuParticles2D>(ref this._frailSpores);
      return true;
    }
    if (StringName.op_Equality(ref name, NFlyconidSporesVfx.PropertyName._vulnerableSpores))
    {
      value = VariantUtils.CreateFrom<CpuParticles2D>(ref this._vulnerableSpores);
      return true;
    }
    if (!StringName.op_Equality(ref name, NFlyconidSporesVfx.PropertyName._parent))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NFlyconidSporesVfx.PropertyName._frailSpores, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NFlyconidSporesVfx.PropertyName._vulnerableSpores, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NFlyconidSporesVfx.PropertyName._parent, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NFlyconidSporesVfx.PropertyName._frailSpores, Variant.From<CpuParticles2D>(ref this._frailSpores));
    info.AddProperty(NFlyconidSporesVfx.PropertyName._vulnerableSpores, Variant.From<CpuParticles2D>(ref this._vulnerableSpores));
    info.AddProperty(NFlyconidSporesVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NFlyconidSporesVfx.PropertyName._frailSpores, ref variant1))
      this._frailSpores = ((Variant) ref variant1).As<CpuParticles2D>();
    Variant variant2;
    if (info.TryGetProperty(NFlyconidSporesVfx.PropertyName._vulnerableSpores, ref variant2))
      this._vulnerableSpores = ((Variant) ref variant2).As<CpuParticles2D>();
    Variant variant3;
    if (!info.TryGetProperty(NFlyconidSporesVfx.PropertyName._parent, ref variant3))
      return;
    this._parent = ((Variant) ref variant3).As<Node2D>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnAnimationEvent = StringName.op_Implicit(nameof (OnAnimationEvent));
    public static readonly StringName SetSporeTypeIsVulnerable = StringName.op_Implicit(nameof (SetSporeTypeIsVulnerable));
    public static readonly StringName OnAnimationStart = StringName.op_Implicit(nameof (OnAnimationStart));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _frailSpores = StringName.op_Implicit(nameof (_frailSpores));
    public static readonly StringName _vulnerableSpores = StringName.op_Implicit(nameof (_vulnerableSpores));
    public static readonly StringName _parent = StringName.op_Implicit(nameof (_parent));
  }

  public class SignalName : Node.SignalName
  {
  }
}
