// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NSewerClamVfx
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

[ScriptPath("res://src/Core/Nodes/Vfx/NSewerClamVfx.cs")]
public class NSewerClamVfx : Node
{
  private const float _coralScaleAmount = 0.2f;
  private const float _maxCoralScale = 1.5f;
  private const float _coralTweenDelay = 0.5f;
  private MegaSprite _megaSprite;
  private GpuParticles2D _deathParticles;
  private GpuParticles2D _buffParticles;
  private GpuParticles2D _chompParticles;
  private Node2D _scaleNode;
  private bool _keyDown;
  private bool _onState;

  public override void _Ready()
  {
    this._megaSprite = new MegaSprite(Variant.op_Implicit((GodotObject) this.GetParent<Node2D>()));
    this._megaSprite.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
    this._deathParticles = this.GetParent().GetNode<GpuParticles2D>(NodePath.op_Implicit("MouthSlot/DeathParticles"));
    this._buffParticles = this.GetParent().GetNode<GpuParticles2D>(NodePath.op_Implicit("MouthSlot/BuffParticles"));
    this._chompParticles = this.GetParent().GetNode<GpuParticles2D>(NodePath.op_Implicit("MouthSlot/ChompParticles"));
    this._scaleNode = this.GetParent().GetNode<Node2D>(NodePath.op_Implicit("CoralScaleBone"));
    this._deathParticles.Emitting = false;
    this._deathParticles.OneShot = true;
    this._buffParticles.Emitting = false;
    this._chompParticles.OneShot = true;
    this._chompParticles.Emitting = false;
    this._scaleNode.Scale = new Vector2(0.1f, 0.1f);
  }

  private void ScaleCoralTo(float targetScale)
  {
    Vector2 vector2_1;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2_1).\u002Ector(targetScale - 0.2f, targetScale - 0.2f);
    Vector2 vector2_2;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2_2).\u002Ector(targetScale, targetScale);
    Tween tween = this.CreateTween();
    tween.SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 6L);
    tween.TweenProperty((GodotObject) this._scaleNode, NodePath.op_Implicit("scale"), Variant.op_Implicit(vector2_2), 0.5).From(Variant.op_Implicit(vector2_1)).SetDelay(0.5);
  }

  private void OnAnimationEvent(
    GodotObject _,
    GodotObject __,
    GodotObject ___,
    GodotObject spineEvent)
  {
    switch (new MegaEvent(Variant.op_Implicit(spineEvent)).GetData().GetEventName())
    {
      case "death_explode":
        this.OnDeathStart();
        break;
      case "darkness_start":
        this.OnDarknessStart();
        break;
      case "darkness_end":
        this.OnDarknessEnd();
        break;
      case "chomp":
        this.OnChomp();
        break;
      case "grow":
        this.OnGrow();
        break;
    }
  }

  private void OnDeathStart() => this._deathParticles.Restart();

  private void OnDeathEnd() => this._deathParticles.Emitting = false;

  private void OnDarknessStart() => this._buffParticles.Restart();

  private void OnDarknessEnd() => this._buffParticles.Emitting = false;

  private void OnChomp() => this._chompParticles.Restart();

  private void OnGrow()
  {
    float targetScale = this._scaleNode.Scale.X + 0.2f;
    if ((double) targetScale >= 1.5)
      targetScale = 1.5f;
    this.ScaleCoralTo(targetScale);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(9)
    {
      new MethodInfo(NSewerClamVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSewerClamVfx.MethodName.ScaleCoralTo, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("targetScale"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NSewerClamVfx.MethodName.OnAnimationEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("__"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("___"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NSewerClamVfx.MethodName.OnDeathStart, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSewerClamVfx.MethodName.OnDeathEnd, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSewerClamVfx.MethodName.OnDarknessStart, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSewerClamVfx.MethodName.OnDarknessEnd, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSewerClamVfx.MethodName.OnChomp, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSewerClamVfx.MethodName.OnGrow, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSewerClamVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSewerClamVfx.MethodName.ScaleCoralTo) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ScaleCoralTo(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSewerClamVfx.MethodName.OnAnimationEvent) && ((NativeVariantPtrArgs) ref args).Count == 4)
    {
      this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[3]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSewerClamVfx.MethodName.OnDeathStart) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnDeathStart();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSewerClamVfx.MethodName.OnDeathEnd) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnDeathEnd();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSewerClamVfx.MethodName.OnDarknessStart) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnDarknessStart();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSewerClamVfx.MethodName.OnDarknessEnd) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnDarknessEnd();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSewerClamVfx.MethodName.OnChomp) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnChomp();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NSewerClamVfx.MethodName.OnGrow) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnGrow();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSewerClamVfx.MethodName._Ready) || StringName.op_Equality(ref method, NSewerClamVfx.MethodName.ScaleCoralTo) || StringName.op_Equality(ref method, NSewerClamVfx.MethodName.OnAnimationEvent) || StringName.op_Equality(ref method, NSewerClamVfx.MethodName.OnDeathStart) || StringName.op_Equality(ref method, NSewerClamVfx.MethodName.OnDeathEnd) || StringName.op_Equality(ref method, NSewerClamVfx.MethodName.OnDarknessStart) || StringName.op_Equality(ref method, NSewerClamVfx.MethodName.OnDarknessEnd) || StringName.op_Equality(ref method, NSewerClamVfx.MethodName.OnChomp) || StringName.op_Equality(ref method, NSewerClamVfx.MethodName.OnGrow) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSewerClamVfx.PropertyName._deathParticles))
    {
      this._deathParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSewerClamVfx.PropertyName._buffParticles))
    {
      this._buffParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSewerClamVfx.PropertyName._chompParticles))
    {
      this._chompParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSewerClamVfx.PropertyName._scaleNode))
    {
      this._scaleNode = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSewerClamVfx.PropertyName._keyDown))
    {
      this._keyDown = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSewerClamVfx.PropertyName._onState))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._onState = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSewerClamVfx.PropertyName._deathParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._deathParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NSewerClamVfx.PropertyName._buffParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._buffParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NSewerClamVfx.PropertyName._chompParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._chompParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NSewerClamVfx.PropertyName._scaleNode))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._scaleNode);
      return true;
    }
    if (StringName.op_Equality(ref name, NSewerClamVfx.PropertyName._keyDown))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._keyDown);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSewerClamVfx.PropertyName._onState))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._onState);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NSewerClamVfx.PropertyName._deathParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSewerClamVfx.PropertyName._buffParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSewerClamVfx.PropertyName._chompParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSewerClamVfx.PropertyName._scaleNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NSewerClamVfx.PropertyName._keyDown, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NSewerClamVfx.PropertyName._onState, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NSewerClamVfx.PropertyName._deathParticles, Variant.From<GpuParticles2D>(ref this._deathParticles));
    info.AddProperty(NSewerClamVfx.PropertyName._buffParticles, Variant.From<GpuParticles2D>(ref this._buffParticles));
    info.AddProperty(NSewerClamVfx.PropertyName._chompParticles, Variant.From<GpuParticles2D>(ref this._chompParticles));
    info.AddProperty(NSewerClamVfx.PropertyName._scaleNode, Variant.From<Node2D>(ref this._scaleNode));
    info.AddProperty(NSewerClamVfx.PropertyName._keyDown, Variant.From<bool>(ref this._keyDown));
    info.AddProperty(NSewerClamVfx.PropertyName._onState, Variant.From<bool>(ref this._onState));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NSewerClamVfx.PropertyName._deathParticles, ref variant1))
      this._deathParticles = ((Variant) ref variant1).As<GpuParticles2D>();
    Variant variant2;
    if (info.TryGetProperty(NSewerClamVfx.PropertyName._buffParticles, ref variant2))
      this._buffParticles = ((Variant) ref variant2).As<GpuParticles2D>();
    Variant variant3;
    if (info.TryGetProperty(NSewerClamVfx.PropertyName._chompParticles, ref variant3))
      this._chompParticles = ((Variant) ref variant3).As<GpuParticles2D>();
    Variant variant4;
    if (info.TryGetProperty(NSewerClamVfx.PropertyName._scaleNode, ref variant4))
      this._scaleNode = ((Variant) ref variant4).As<Node2D>();
    Variant variant5;
    if (info.TryGetProperty(NSewerClamVfx.PropertyName._keyDown, ref variant5))
      this._keyDown = ((Variant) ref variant5).As<bool>();
    Variant variant6;
    if (!info.TryGetProperty(NSewerClamVfx.PropertyName._onState, ref variant6))
      return;
    this._onState = ((Variant) ref variant6).As<bool>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName ScaleCoralTo = StringName.op_Implicit(nameof (ScaleCoralTo));
    public static readonly StringName OnAnimationEvent = StringName.op_Implicit(nameof (OnAnimationEvent));
    public static readonly StringName OnDeathStart = StringName.op_Implicit(nameof (OnDeathStart));
    public static readonly StringName OnDeathEnd = StringName.op_Implicit(nameof (OnDeathEnd));
    public static readonly StringName OnDarknessStart = StringName.op_Implicit(nameof (OnDarknessStart));
    public static readonly StringName OnDarknessEnd = StringName.op_Implicit(nameof (OnDarknessEnd));
    public static readonly StringName OnChomp = StringName.op_Implicit(nameof (OnChomp));
    public static readonly StringName OnGrow = StringName.op_Implicit(nameof (OnGrow));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _deathParticles = StringName.op_Implicit(nameof (_deathParticles));
    public static readonly StringName _buffParticles = StringName.op_Implicit(nameof (_buffParticles));
    public static readonly StringName _chompParticles = StringName.op_Implicit(nameof (_chompParticles));
    public static readonly StringName _scaleNode = StringName.op_Implicit(nameof (_scaleNode));
    public static readonly StringName _keyDown = StringName.op_Implicit(nameof (_keyDown));
    public static readonly StringName _onState = StringName.op_Implicit(nameof (_onState));
  }

  public class SignalName : Node.SignalName
  {
  }
}
