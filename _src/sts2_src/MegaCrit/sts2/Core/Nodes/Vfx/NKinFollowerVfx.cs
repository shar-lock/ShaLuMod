// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NKinFollowerVfx
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
[ScriptPath("res://src/Core/Nodes/Vfx/NKinFollowerVfx.cs")]
public class NKinFollowerVfx : Node
{
  private NBasicTrail _trail1;
  private NBasicTrail _trail2;
  private GpuParticles2D _hay;
  private Node2D _parent;
  private MegaSprite _animController;

  public override void _Ready()
  {
    this._parent = this.GetParent<Node2D>();
    this._animController = new MegaSprite(Variant.op_Implicit((GodotObject) this._parent));
    this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
    this._trail1 = ((Node) this._parent).GetNode<NBasicTrail>(NodePath.op_Implicit("Boomerang1Slot/Trail"));
    this._trail2 = ((Node) this._parent).GetNode<NBasicTrail>(NodePath.op_Implicit("Boomerang2Slot/Trail"));
    this._hay = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("HaySlot/HayParticles"));
    ((CanvasItem) this._trail1).Visible = false;
    ((CanvasItem) this._trail2).Visible = false;
    this._hay.Emitting = false;
    this._hay.OneShot = true;
  }

  private void OnAnimationEvent(
    GodotObject _,
    GodotObject __,
    GodotObject ___,
    GodotObject spineEvent)
  {
    switch (new MegaEvent(Variant.op_Implicit(spineEvent)).GetData().GetEventName())
    {
      case "start_trail1":
        this.StartTrail1();
        break;
      case "end_trail1":
        this.EndTrail1();
        break;
      case "start_trail2":
        this.StartTrail2();
        break;
      case "end_trail2":
        this.EndTrail2();
        break;
      case "start_hay":
        this.StartHay();
        break;
    }
  }

  private void StartTrail1()
  {
    this._trail1.ClearPoints();
    ((CanvasItem) this._trail1).Visible = true;
  }

  private void StartTrail2()
  {
    this._trail2.ClearPoints();
    ((CanvasItem) this._trail2).Visible = true;
  }

  private void EndTrail1() => ((CanvasItem) this._trail1).Visible = false;

  private void EndTrail2() => ((CanvasItem) this._trail2).Visible = false;

  private void StartHay() => this._hay.Restart();

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(7)
    {
      new MethodInfo(NKinFollowerVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NKinFollowerVfx.MethodName.OnAnimationEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("__"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("___"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NKinFollowerVfx.MethodName.StartTrail1, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NKinFollowerVfx.MethodName.StartTrail2, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NKinFollowerVfx.MethodName.EndTrail1, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NKinFollowerVfx.MethodName.EndTrail2, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NKinFollowerVfx.MethodName.StartHay, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NKinFollowerVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NKinFollowerVfx.MethodName.OnAnimationEvent) && ((NativeVariantPtrArgs) ref args).Count == 4)
    {
      this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[3]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NKinFollowerVfx.MethodName.StartTrail1) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StartTrail1();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NKinFollowerVfx.MethodName.StartTrail2) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StartTrail2();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NKinFollowerVfx.MethodName.EndTrail1) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.EndTrail1();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NKinFollowerVfx.MethodName.EndTrail2) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.EndTrail2();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NKinFollowerVfx.MethodName.StartHay) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.StartHay();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NKinFollowerVfx.MethodName._Ready) || StringName.op_Equality(ref method, NKinFollowerVfx.MethodName.OnAnimationEvent) || StringName.op_Equality(ref method, NKinFollowerVfx.MethodName.StartTrail1) || StringName.op_Equality(ref method, NKinFollowerVfx.MethodName.StartTrail2) || StringName.op_Equality(ref method, NKinFollowerVfx.MethodName.EndTrail1) || StringName.op_Equality(ref method, NKinFollowerVfx.MethodName.EndTrail2) || StringName.op_Equality(ref method, NKinFollowerVfx.MethodName.StartHay) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NKinFollowerVfx.PropertyName._trail1))
    {
      this._trail1 = VariantUtils.ConvertTo<NBasicTrail>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NKinFollowerVfx.PropertyName._trail2))
    {
      this._trail2 = VariantUtils.ConvertTo<NBasicTrail>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NKinFollowerVfx.PropertyName._hay))
    {
      this._hay = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NKinFollowerVfx.PropertyName._parent))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NKinFollowerVfx.PropertyName._trail1))
    {
      value = VariantUtils.CreateFrom<NBasicTrail>(ref this._trail1);
      return true;
    }
    if (StringName.op_Equality(ref name, NKinFollowerVfx.PropertyName._trail2))
    {
      value = VariantUtils.CreateFrom<NBasicTrail>(ref this._trail2);
      return true;
    }
    if (StringName.op_Equality(ref name, NKinFollowerVfx.PropertyName._hay))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._hay);
      return true;
    }
    if (!StringName.op_Equality(ref name, NKinFollowerVfx.PropertyName._parent))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NKinFollowerVfx.PropertyName._trail1, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NKinFollowerVfx.PropertyName._trail2, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NKinFollowerVfx.PropertyName._hay, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NKinFollowerVfx.PropertyName._parent, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NKinFollowerVfx.PropertyName._trail1, Variant.From<NBasicTrail>(ref this._trail1));
    info.AddProperty(NKinFollowerVfx.PropertyName._trail2, Variant.From<NBasicTrail>(ref this._trail2));
    info.AddProperty(NKinFollowerVfx.PropertyName._hay, Variant.From<GpuParticles2D>(ref this._hay));
    info.AddProperty(NKinFollowerVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NKinFollowerVfx.PropertyName._trail1, ref variant1))
      this._trail1 = ((Variant) ref variant1).As<NBasicTrail>();
    Variant variant2;
    if (info.TryGetProperty(NKinFollowerVfx.PropertyName._trail2, ref variant2))
      this._trail2 = ((Variant) ref variant2).As<NBasicTrail>();
    Variant variant3;
    if (info.TryGetProperty(NKinFollowerVfx.PropertyName._hay, ref variant3))
      this._hay = ((Variant) ref variant3).As<GpuParticles2D>();
    Variant variant4;
    if (!info.TryGetProperty(NKinFollowerVfx.PropertyName._parent, ref variant4))
      return;
    this._parent = ((Variant) ref variant4).As<Node2D>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnAnimationEvent = StringName.op_Implicit(nameof (OnAnimationEvent));
    public static readonly StringName StartTrail1 = StringName.op_Implicit(nameof (StartTrail1));
    public static readonly StringName StartTrail2 = StringName.op_Implicit(nameof (StartTrail2));
    public static readonly StringName EndTrail1 = StringName.op_Implicit(nameof (EndTrail1));
    public static readonly StringName EndTrail2 = StringName.op_Implicit(nameof (EndTrail2));
    public static readonly StringName StartHay = StringName.op_Implicit(nameof (StartHay));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _trail1 = StringName.op_Implicit(nameof (_trail1));
    public static readonly StringName _trail2 = StringName.op_Implicit(nameof (_trail2));
    public static readonly StringName _hay = StringName.op_Implicit(nameof (_hay));
    public static readonly StringName _parent = StringName.op_Implicit(nameof (_parent));
  }

  public class SignalName : Node.SignalName
  {
  }
}
