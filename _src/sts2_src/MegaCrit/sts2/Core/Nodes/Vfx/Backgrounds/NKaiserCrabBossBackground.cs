// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.Backgrounds.NKaiserCrabBossBackground
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx.Backgrounds;

[GlobalClass]
[ScriptPath("res://src/Core/Nodes/Vfx/Backgrounds/NKaiserCrabBossBackground.cs")]
public class NKaiserCrabBossBackground : Node2D
{
  private const int _bodyTrack = 0;
  private const int _leftArmTrack = 1;
  private const int _rightArmTrack = 2;
  private const int _reactionTrack = 3;
  private MegaSprite _animController;
  private Node2D _leftArm;
  private Node2D _rightArm;
  private NKaiserCrabBossBackground.RightArmState _rightArmState;
  private CancellationTokenSource? _cts;

  public override void _ExitTree() => this._cts?.Cancel();

  public override void _Ready()
  {
    this._animController = new MegaSprite(Variant.op_Implicit((GodotObject) ((Node) this).GetNode<Node2D>(NodePath.op_Implicit("%Visuals"))));
    this._leftArm = ((Node) this).GetNode<Node2D>(NodePath.op_Implicit("%ArmBoneL"));
    this._rightArm = ((Node) this).GetNode<Node2D>(NodePath.op_Implicit("%ArmBoneR"));
    ((Node) this).RunWhenSpineReady(this._animController, (Action<MegaAnimationState>) (state =>
    {
      state.SetAnimation("right/idle_loop", trackId: 2);
      state.SetAnimation("body/idle_loop");
      state.SetAnimation("left/idle_loop", trackId: 1);
    }));
    ((CanvasItem) this).SetVisible(false);
  }

  public async Task PlayAttackAnim(
    NKaiserCrabBossBackground.ArmSide side,
    string animation,
    float duration)
  {
    this._cts = new CancellationTokenSource();
    string lowerInvariant = side.ToString().ToLowerInvariant();
    MegaAnimationState animationState = this._animController.GetAnimationState();
    animationState.SetAnimation($"{lowerInvariant}/{animation}", false, side == NKaiserCrabBossBackground.ArmSide.Left ? 1 : 2);
    animationState.SetAnimation("reactions/attack_" + lowerInvariant, false, 3);
    this.AddEmptyReactionAnimation(animationState);
    animationState.AddAnimation(lowerInvariant + "/idle_loop", trackId: side == NKaiserCrabBossBackground.ArmSide.Left ? 1 : 2);
    await Cmd.Wait(duration, this._cts.Token);
  }

  public void PlayHurtAnim(NKaiserCrabBossBackground.ArmSide side)
  {
    string lowerInvariant = side.ToString().ToLowerInvariant();
    MegaAnimationState animationState = this._animController.GetAnimationState();
    animationState.SetAnimation("reactions/hurt_" + lowerInvariant, false, 3);
    this.AddEmptyReactionAnimation(animationState);
    if (side == NKaiserCrabBossBackground.ArmSide.Left)
    {
      animationState.SetAnimation(lowerInvariant + "/hurt", false, 1);
      animationState.AddAnimation(lowerInvariant + "/idle_loop", trackId: 1);
    }
    else
    {
      switch (this._rightArmState)
      {
        case NKaiserCrabBossBackground.RightArmState.Default:
          animationState.SetAnimation(lowerInvariant + "/hurt", false, 2);
          animationState.AddAnimation("right/idle_loop", trackId: 2);
          break;
        case NKaiserCrabBossBackground.RightArmState.Charging:
          animationState.SetAnimation(lowerInvariant + "/hurt_charged", false, 2);
          animationState.AddAnimation("right/charged_loop", trackId: 2);
          break;
        case NKaiserCrabBossBackground.RightArmState.Resting:
          animationState.SetAnimation(lowerInvariant + "/hurt_resting", false, 2);
          animationState.AddAnimation("right/rest_loop", trackId: 2);
          break;
      }
    }
  }

  public void PlayArmDeathAnim(NKaiserCrabBossBackground.ArmSide side)
  {
    string lowerInvariant = side.ToString().ToLowerInvariant();
    MegaAnimationState animationState = this._animController.GetAnimationState();
    animationState.SetAnimation("reactions/hurt_" + lowerInvariant, false, 3);
    this.AddEmptyReactionAnimation(animationState);
    if (side == NKaiserCrabBossBackground.ArmSide.Left)
    {
      animationState.SetAnimation(lowerInvariant + "/die", false, 1);
    }
    else
    {
      switch (this._rightArmState)
      {
        case NKaiserCrabBossBackground.RightArmState.Default:
        case NKaiserCrabBossBackground.RightArmState.Charging:
          animationState.SetAnimation("right/die", false, 2);
          break;
        case NKaiserCrabBossBackground.RightArmState.Resting:
          animationState.SetAnimation("right/die_resting", false, 2);
          break;
      }
    }
  }

  public async Task PlayRightSideChargeUpAnim(float duration)
  {
    this._cts = new CancellationTokenSource();
    this._rightArmState = NKaiserCrabBossBackground.RightArmState.Charging;
    MegaAnimationState animationState = this._animController.GetAnimationState();
    animationState.SetAnimation("right/charge_up", false, 2);
    animationState.AddAnimation("right/charged_loop", trackId: 2);
    animationState.SetAnimation("reactions/attack_right", false, 3);
    this.AddEmptyReactionAnimation(animationState);
    await Cmd.Wait(duration, this._cts.Token);
  }

  public async Task PlayRightSideHeavy(float duration)
  {
    this._cts = new CancellationTokenSource();
    this._rightArmState = NKaiserCrabBossBackground.RightArmState.Resting;
    MegaAnimationState animationState = this._animController.GetAnimationState();
    animationState.SetAnimation("right/attack_heavy", false, 2);
    animationState.AddAnimation("right/rest_loop", trackId: 2);
    animationState.SetAnimation("reactions/attack_right", false, 3);
    this.AddEmptyReactionAnimation(animationState);
    await Cmd.Wait(duration, this._cts.Token);
  }

  public async Task PlayRightRecharge(float duration)
  {
    this._cts = new CancellationTokenSource();
    this._rightArmState = NKaiserCrabBossBackground.RightArmState.Default;
    MegaAnimationState animationState = this._animController.GetAnimationState();
    animationState.SetAnimation("right/wake_up", false, 2);
    animationState.AddAnimation("right/idle_loop", trackId: 2);
    await Cmd.Wait(duration, this._cts.Token);
  }

  public void PlayBodyDeathAnim()
  {
    this._animController.GetAnimationState().SetAnimation("body/die", false);
  }

  private void AddEmptyReactionAnimation(MegaAnimationState state) => state.AddEmptyAnimation(3);

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NKaiserCrabBossBackground.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NKaiserCrabBossBackground.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NKaiserCrabBossBackground.MethodName.PlayHurtAnim, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("side"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NKaiserCrabBossBackground.MethodName.PlayArmDeathAnim, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("side"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NKaiserCrabBossBackground.MethodName.PlayBodyDeathAnim, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NKaiserCrabBossBackground.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NKaiserCrabBossBackground.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NKaiserCrabBossBackground.MethodName.PlayHurtAnim) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.PlayHurtAnim(VariantUtils.ConvertTo<NKaiserCrabBossBackground.ArmSide>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NKaiserCrabBossBackground.MethodName.PlayArmDeathAnim) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.PlayArmDeathAnim(VariantUtils.ConvertTo<NKaiserCrabBossBackground.ArmSide>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NKaiserCrabBossBackground.MethodName.PlayBodyDeathAnim) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.PlayBodyDeathAnim();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NKaiserCrabBossBackground.MethodName._ExitTree) || StringName.op_Equality(ref method, NKaiserCrabBossBackground.MethodName._Ready) || StringName.op_Equality(ref method, NKaiserCrabBossBackground.MethodName.PlayHurtAnim) || StringName.op_Equality(ref method, NKaiserCrabBossBackground.MethodName.PlayArmDeathAnim) || StringName.op_Equality(ref method, NKaiserCrabBossBackground.MethodName.PlayBodyDeathAnim) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NKaiserCrabBossBackground.PropertyName._leftArm))
    {
      this._leftArm = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NKaiserCrabBossBackground.PropertyName._rightArm))
    {
      this._rightArm = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NKaiserCrabBossBackground.PropertyName._rightArmState))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._rightArmState = VariantUtils.ConvertTo<NKaiserCrabBossBackground.RightArmState>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NKaiserCrabBossBackground.PropertyName._leftArm))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._leftArm);
      return true;
    }
    if (StringName.op_Equality(ref name, NKaiserCrabBossBackground.PropertyName._rightArm))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._rightArm);
      return true;
    }
    if (!StringName.op_Equality(ref name, NKaiserCrabBossBackground.PropertyName._rightArmState))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<NKaiserCrabBossBackground.RightArmState>(ref this._rightArmState);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NKaiserCrabBossBackground.PropertyName._leftArm, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NKaiserCrabBossBackground.PropertyName._rightArm, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NKaiserCrabBossBackground.PropertyName._rightArmState, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NKaiserCrabBossBackground.PropertyName._leftArm, Variant.From<Node2D>(ref this._leftArm));
    info.AddProperty(NKaiserCrabBossBackground.PropertyName._rightArm, Variant.From<Node2D>(ref this._rightArm));
    info.AddProperty(NKaiserCrabBossBackground.PropertyName._rightArmState, Variant.From<NKaiserCrabBossBackground.RightArmState>(ref this._rightArmState));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NKaiserCrabBossBackground.PropertyName._leftArm, ref variant1))
      this._leftArm = ((Variant) ref variant1).As<Node2D>();
    Variant variant2;
    if (info.TryGetProperty(NKaiserCrabBossBackground.PropertyName._rightArm, ref variant2))
      this._rightArm = ((Variant) ref variant2).As<Node2D>();
    Variant variant3;
    if (!info.TryGetProperty(NKaiserCrabBossBackground.PropertyName._rightArmState, ref variant3))
      return;
    this._rightArmState = ((Variant) ref variant3).As<NKaiserCrabBossBackground.RightArmState>();
  }

  private enum RightArmState
  {
    Default,
    Charging,
    Resting,
  }

  public enum ArmSide
  {
    Left,
    Right,
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName PlayHurtAnim = StringName.op_Implicit(nameof (PlayHurtAnim));
    public static readonly StringName PlayArmDeathAnim = StringName.op_Implicit(nameof (PlayArmDeathAnim));
    public static readonly StringName PlayBodyDeathAnim = StringName.op_Implicit(nameof (PlayBodyDeathAnim));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _leftArm = StringName.op_Implicit(nameof (_leftArm));
    public static readonly StringName _rightArm = StringName.op_Implicit(nameof (_rightArm));
    public static readonly StringName _rightArmState = StringName.op_Implicit(nameof (_rightArmState));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
