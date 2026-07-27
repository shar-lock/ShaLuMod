// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NHauntedShipVfx
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

[ScriptPath("res://src/Core/Nodes/Vfx/NHauntedShipVfx.cs")]
public class NHauntedShipVfx : Node
{
  private MegaSprite _megaSprite;
  private GpuParticles2D _eyeParticles1;
  private GpuParticles2D _eyeParticles2;
  private GpuParticles2D _eyeParticles3;
  private GpuParticles2D _headParticles1;
  private GpuParticles2D _headParticles2;

  public override void _Ready()
  {
    this._eyeParticles1 = this.GetNode<GpuParticles2D>(NodePath.op_Implicit("../EyeBone1/BubbleParticles"));
    this._eyeParticles2 = this.GetNode<GpuParticles2D>(NodePath.op_Implicit("../EyeBone2/BubbleParticles"));
    this._eyeParticles3 = this.GetNode<GpuParticles2D>(NodePath.op_Implicit("../EyeBone3/BubbleParticles"));
    this._headParticles1 = this.GetNode<GpuParticles2D>(NodePath.op_Implicit("../HeadSlot/BubbleParticles"));
    this._headParticles2 = this.GetNode<GpuParticles2D>(NodePath.op_Implicit("../HeadSlot/BubbleParticles2"));
    this._eyeParticles1.Emitting = false;
    this._eyeParticles2.Emitting = false;
    this._eyeParticles3.Emitting = false;
    this._megaSprite = new MegaSprite(Variant.op_Implicit((GodotObject) this.GetParent<Node2D>()));
    this._megaSprite.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
  }

  private void OnAnimationEvent(
    GodotObject _,
    GodotObject __,
    GodotObject ___,
    GodotObject spineEvent)
  {
    switch (new MegaEvent(Variant.op_Implicit(spineEvent)).GetData().GetEventName())
    {
      case "eye_bubbles_start":
        this.OnEyeBubblesStart();
        break;
      case "eye_bubbles_end":
        this.OnEyeBubblesEnd();
        break;
      case "head_bubbles_start":
        this.OnHeadBubblesStart();
        break;
      case "head_bubbles_end":
        this.OnHeadBubblesEnd();
        break;
    }
  }

  private void OnEyeBubblesStart()
  {
    this._eyeParticles1.Emitting = true;
    this._eyeParticles2.Emitting = true;
    this._eyeParticles3.Emitting = true;
  }

  private void OnEyeBubblesEnd()
  {
    this._eyeParticles1.Emitting = false;
    this._eyeParticles2.Emitting = false;
    this._eyeParticles3.Emitting = false;
  }

  private void OnHeadBubblesStart()
  {
    this._headParticles1.Emitting = true;
    this._headParticles2.Emitting = true;
  }

  private void OnHeadBubblesEnd()
  {
    this._headParticles1.Emitting = false;
    this._headParticles2.Emitting = false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NHauntedShipVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHauntedShipVfx.MethodName.OnAnimationEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("__"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("___"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NHauntedShipVfx.MethodName.OnEyeBubblesStart, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHauntedShipVfx.MethodName.OnEyeBubblesEnd, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHauntedShipVfx.MethodName.OnHeadBubblesStart, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHauntedShipVfx.MethodName.OnHeadBubblesEnd, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NHauntedShipVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHauntedShipVfx.MethodName.OnAnimationEvent) && ((NativeVariantPtrArgs) ref args).Count == 4)
    {
      this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[3]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHauntedShipVfx.MethodName.OnEyeBubblesStart) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnEyeBubblesStart();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHauntedShipVfx.MethodName.OnEyeBubblesEnd) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnEyeBubblesEnd();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHauntedShipVfx.MethodName.OnHeadBubblesStart) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnHeadBubblesStart();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NHauntedShipVfx.MethodName.OnHeadBubblesEnd) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnHeadBubblesEnd();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NHauntedShipVfx.MethodName._Ready) || StringName.op_Equality(ref method, NHauntedShipVfx.MethodName.OnAnimationEvent) || StringName.op_Equality(ref method, NHauntedShipVfx.MethodName.OnEyeBubblesStart) || StringName.op_Equality(ref method, NHauntedShipVfx.MethodName.OnEyeBubblesEnd) || StringName.op_Equality(ref method, NHauntedShipVfx.MethodName.OnHeadBubblesStart) || StringName.op_Equality(ref method, NHauntedShipVfx.MethodName.OnHeadBubblesEnd) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NHauntedShipVfx.PropertyName._eyeParticles1))
    {
      this._eyeParticles1 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHauntedShipVfx.PropertyName._eyeParticles2))
    {
      this._eyeParticles2 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHauntedShipVfx.PropertyName._eyeParticles3))
    {
      this._eyeParticles3 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHauntedShipVfx.PropertyName._headParticles1))
    {
      this._headParticles1 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NHauntedShipVfx.PropertyName._headParticles2))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._headParticles2 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NHauntedShipVfx.PropertyName._eyeParticles1))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._eyeParticles1);
      return true;
    }
    if (StringName.op_Equality(ref name, NHauntedShipVfx.PropertyName._eyeParticles2))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._eyeParticles2);
      return true;
    }
    if (StringName.op_Equality(ref name, NHauntedShipVfx.PropertyName._eyeParticles3))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._eyeParticles3);
      return true;
    }
    if (StringName.op_Equality(ref name, NHauntedShipVfx.PropertyName._headParticles1))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._headParticles1);
      return true;
    }
    if (!StringName.op_Equality(ref name, NHauntedShipVfx.PropertyName._headParticles2))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._headParticles2);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NHauntedShipVfx.PropertyName._eyeParticles1, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NHauntedShipVfx.PropertyName._eyeParticles2, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NHauntedShipVfx.PropertyName._eyeParticles3, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NHauntedShipVfx.PropertyName._headParticles1, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NHauntedShipVfx.PropertyName._headParticles2, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NHauntedShipVfx.PropertyName._eyeParticles1, Variant.From<GpuParticles2D>(ref this._eyeParticles1));
    info.AddProperty(NHauntedShipVfx.PropertyName._eyeParticles2, Variant.From<GpuParticles2D>(ref this._eyeParticles2));
    info.AddProperty(NHauntedShipVfx.PropertyName._eyeParticles3, Variant.From<GpuParticles2D>(ref this._eyeParticles3));
    info.AddProperty(NHauntedShipVfx.PropertyName._headParticles1, Variant.From<GpuParticles2D>(ref this._headParticles1));
    info.AddProperty(NHauntedShipVfx.PropertyName._headParticles2, Variant.From<GpuParticles2D>(ref this._headParticles2));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NHauntedShipVfx.PropertyName._eyeParticles1, ref variant1))
      this._eyeParticles1 = ((Variant) ref variant1).As<GpuParticles2D>();
    Variant variant2;
    if (info.TryGetProperty(NHauntedShipVfx.PropertyName._eyeParticles2, ref variant2))
      this._eyeParticles2 = ((Variant) ref variant2).As<GpuParticles2D>();
    Variant variant3;
    if (info.TryGetProperty(NHauntedShipVfx.PropertyName._eyeParticles3, ref variant3))
      this._eyeParticles3 = ((Variant) ref variant3).As<GpuParticles2D>();
    Variant variant4;
    if (info.TryGetProperty(NHauntedShipVfx.PropertyName._headParticles1, ref variant4))
      this._headParticles1 = ((Variant) ref variant4).As<GpuParticles2D>();
    Variant variant5;
    if (!info.TryGetProperty(NHauntedShipVfx.PropertyName._headParticles2, ref variant5))
      return;
    this._headParticles2 = ((Variant) ref variant5).As<GpuParticles2D>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnAnimationEvent = StringName.op_Implicit(nameof (OnAnimationEvent));
    public static readonly StringName OnEyeBubblesStart = StringName.op_Implicit(nameof (OnEyeBubblesStart));
    public static readonly StringName OnEyeBubblesEnd = StringName.op_Implicit(nameof (OnEyeBubblesEnd));
    public static readonly StringName OnHeadBubblesStart = StringName.op_Implicit(nameof (OnHeadBubblesStart));
    public static readonly StringName OnHeadBubblesEnd = StringName.op_Implicit(nameof (OnHeadBubblesEnd));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _eyeParticles1 = StringName.op_Implicit(nameof (_eyeParticles1));
    public static readonly StringName _eyeParticles2 = StringName.op_Implicit(nameof (_eyeParticles2));
    public static readonly StringName _eyeParticles3 = StringName.op_Implicit(nameof (_eyeParticles3));
    public static readonly StringName _headParticles1 = StringName.op_Implicit(nameof (_headParticles1));
    public static readonly StringName _headParticles2 = StringName.op_Implicit(nameof (_headParticles2));
  }

  public class SignalName : Node.SignalName
  {
  }
}
