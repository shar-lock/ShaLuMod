// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NEntomancerVfx
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
[ScriptPath("res://src/Core/Nodes/Vfx/NEntomancerVfx.cs")]
public class NEntomancerVfx : Node
{
  private GpuParticles2D _swarmParticles;
  private GpuParticles2D _attackingBugParticles;
  private Node2D _swarmTargetNode;
  private Vector2 _basePosition;
  private Tween? _swarmTween;
  private bool _swarming;
  private Node2D _parent;
  private MegaSprite _animController;

  public override void _Ready()
  {
    this._parent = this.GetParent<Node2D>();
    this._animController = new MegaSprite(Variant.op_Implicit((GodotObject) this._parent));
    this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
    this._swarmParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("SwarmParticles"));
    this._attackingBugParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("SwarmParticles/AttackingBugParticles"));
    this._swarmTargetNode = ((Node) this._parent).GetNode<Node2D>(NodePath.op_Implicit("SwarmTargetNode"));
    this._basePosition = ((Node2D) this._swarmParticles).Position;
    this._attackingBugParticles.Emitting = false;
  }

  private void LaunchSwarm()
  {
    this._swarming = true;
    this._swarmTween = this.CreateTween();
    this._attackingBugParticles.Emitting = true;
    this._swarmTween.SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    this._swarmTween.TweenProperty((GodotObject) this._swarmParticles, NodePath.op_Implicit("position"), Variant.op_Implicit(this._swarmTargetNode.Position), 1.0);
    this._swarmTween.SetEase((Tween.EaseType) 2L).SetTrans((Tween.TransitionType) 7L);
    this._swarmTween.TweenProperty((GodotObject) this._swarmParticles, NodePath.op_Implicit("position"), Variant.op_Implicit(this._basePosition), 1.5).SetDelay(0.0);
    this._swarmTween.TweenCallback(Callable.From(new Action(this.CompleteSwarmAttack))).SetDelay(0.0099999997764825821);
  }

  private void CompleteSwarmAttack()
  {
    this._attackingBugParticles.Emitting = false;
    this._swarming = false;
  }

  private void CancelSwarmAttack()
  {
    if (!this._swarming)
      return;
    this._swarmTween.Kill();
    ((Node2D) this._swarmParticles).Position = this._basePosition;
    this.CompleteSwarmAttack();
  }

  private void TurnOffSwarm() => this._swarmParticles.Emitting = false;

  private void OnAnimationEvent(
    GodotObject _,
    GodotObject __,
    GodotObject ___,
    GodotObject spineEvent)
  {
    switch (new MegaEvent(Variant.op_Implicit(spineEvent)).GetData().GetEventName())
    {
      case "launch_swarm":
        this.LaunchSwarm();
        break;
      case "turn_off_swarm":
        this.TurnOffSwarm();
        break;
    }
  }

  public override void _ExitTree() => this._swarmTween?.Kill();

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(7)
    {
      new MethodInfo(NEntomancerVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEntomancerVfx.MethodName.LaunchSwarm, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEntomancerVfx.MethodName.CompleteSwarmAttack, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEntomancerVfx.MethodName.CancelSwarmAttack, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEntomancerVfx.MethodName.TurnOffSwarm, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEntomancerVfx.MethodName.OnAnimationEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("__"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("___"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NEntomancerVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NEntomancerVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEntomancerVfx.MethodName.LaunchSwarm) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.LaunchSwarm();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEntomancerVfx.MethodName.CompleteSwarmAttack) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.CompleteSwarmAttack();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEntomancerVfx.MethodName.CancelSwarmAttack) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.CancelSwarmAttack();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEntomancerVfx.MethodName.TurnOffSwarm) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TurnOffSwarm();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEntomancerVfx.MethodName.OnAnimationEvent) && ((NativeVariantPtrArgs) ref args).Count == 4)
    {
      this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[3]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NEntomancerVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    base._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NEntomancerVfx.MethodName._Ready) || StringName.op_Equality(ref method, NEntomancerVfx.MethodName.LaunchSwarm) || StringName.op_Equality(ref method, NEntomancerVfx.MethodName.CompleteSwarmAttack) || StringName.op_Equality(ref method, NEntomancerVfx.MethodName.CancelSwarmAttack) || StringName.op_Equality(ref method, NEntomancerVfx.MethodName.TurnOffSwarm) || StringName.op_Equality(ref method, NEntomancerVfx.MethodName.OnAnimationEvent) || StringName.op_Equality(ref method, NEntomancerVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NEntomancerVfx.PropertyName._swarmParticles))
    {
      this._swarmParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEntomancerVfx.PropertyName._attackingBugParticles))
    {
      this._attackingBugParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEntomancerVfx.PropertyName._swarmTargetNode))
    {
      this._swarmTargetNode = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEntomancerVfx.PropertyName._basePosition))
    {
      this._basePosition = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEntomancerVfx.PropertyName._swarmTween))
    {
      this._swarmTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEntomancerVfx.PropertyName._swarming))
    {
      this._swarming = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NEntomancerVfx.PropertyName._parent))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NEntomancerVfx.PropertyName._swarmParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._swarmParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NEntomancerVfx.PropertyName._attackingBugParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._attackingBugParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NEntomancerVfx.PropertyName._swarmTargetNode))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._swarmTargetNode);
      return true;
    }
    if (StringName.op_Equality(ref name, NEntomancerVfx.PropertyName._basePosition))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._basePosition);
      return true;
    }
    if (StringName.op_Equality(ref name, NEntomancerVfx.PropertyName._swarmTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._swarmTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NEntomancerVfx.PropertyName._swarming))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._swarming);
      return true;
    }
    if (!StringName.op_Equality(ref name, NEntomancerVfx.PropertyName._parent))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NEntomancerVfx.PropertyName._swarmParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEntomancerVfx.PropertyName._attackingBugParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEntomancerVfx.PropertyName._swarmTargetNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NEntomancerVfx.PropertyName._basePosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEntomancerVfx.PropertyName._swarmTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NEntomancerVfx.PropertyName._swarming, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEntomancerVfx.PropertyName._parent, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NEntomancerVfx.PropertyName._swarmParticles, Variant.From<GpuParticles2D>(ref this._swarmParticles));
    info.AddProperty(NEntomancerVfx.PropertyName._attackingBugParticles, Variant.From<GpuParticles2D>(ref this._attackingBugParticles));
    info.AddProperty(NEntomancerVfx.PropertyName._swarmTargetNode, Variant.From<Node2D>(ref this._swarmTargetNode));
    info.AddProperty(NEntomancerVfx.PropertyName._basePosition, Variant.From<Vector2>(ref this._basePosition));
    info.AddProperty(NEntomancerVfx.PropertyName._swarmTween, Variant.From<Tween>(ref this._swarmTween));
    info.AddProperty(NEntomancerVfx.PropertyName._swarming, Variant.From<bool>(ref this._swarming));
    info.AddProperty(NEntomancerVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NEntomancerVfx.PropertyName._swarmParticles, ref variant1))
      this._swarmParticles = ((Variant) ref variant1).As<GpuParticles2D>();
    Variant variant2;
    if (info.TryGetProperty(NEntomancerVfx.PropertyName._attackingBugParticles, ref variant2))
      this._attackingBugParticles = ((Variant) ref variant2).As<GpuParticles2D>();
    Variant variant3;
    if (info.TryGetProperty(NEntomancerVfx.PropertyName._swarmTargetNode, ref variant3))
      this._swarmTargetNode = ((Variant) ref variant3).As<Node2D>();
    Variant variant4;
    if (info.TryGetProperty(NEntomancerVfx.PropertyName._basePosition, ref variant4))
      this._basePosition = ((Variant) ref variant4).As<Vector2>();
    Variant variant5;
    if (info.TryGetProperty(NEntomancerVfx.PropertyName._swarmTween, ref variant5))
      this._swarmTween = ((Variant) ref variant5).As<Tween>();
    Variant variant6;
    if (info.TryGetProperty(NEntomancerVfx.PropertyName._swarming, ref variant6))
      this._swarming = ((Variant) ref variant6).As<bool>();
    Variant variant7;
    if (!info.TryGetProperty(NEntomancerVfx.PropertyName._parent, ref variant7))
      return;
    this._parent = ((Variant) ref variant7).As<Node2D>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName LaunchSwarm = StringName.op_Implicit(nameof (LaunchSwarm));
    public static readonly StringName CompleteSwarmAttack = StringName.op_Implicit(nameof (CompleteSwarmAttack));
    public static readonly StringName CancelSwarmAttack = StringName.op_Implicit(nameof (CancelSwarmAttack));
    public static readonly StringName TurnOffSwarm = StringName.op_Implicit(nameof (TurnOffSwarm));
    public static readonly StringName OnAnimationEvent = StringName.op_Implicit(nameof (OnAnimationEvent));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _swarmParticles = StringName.op_Implicit(nameof (_swarmParticles));
    public static readonly StringName _attackingBugParticles = StringName.op_Implicit(nameof (_attackingBugParticles));
    public static readonly StringName _swarmTargetNode = StringName.op_Implicit(nameof (_swarmTargetNode));
    public static readonly StringName _basePosition = StringName.op_Implicit(nameof (_basePosition));
    public static readonly StringName _swarmTween = StringName.op_Implicit(nameof (_swarmTween));
    public static readonly StringName _swarming = StringName.op_Implicit(nameof (_swarming));
    public static readonly StringName _parent = StringName.op_Implicit(nameof (_parent));
  }

  public class SignalName : Node.SignalName
  {
  }
}
