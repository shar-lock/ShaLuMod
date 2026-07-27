// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NSovereignBladeVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NSovereignBladeVfx.cs")]
public class NSovereignBladeVfx : Node2D
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/sovereign_blade");
  private Player _owner;
  private Node2D _spineNode;
  private MegaSprite _animController;
  private Node2D _bladeGlow;
  private GpuParticles2D _forgeSparks;
  private GpuParticles2D _spawnFlames;
  private GpuParticles2D _spawnFlamesBack;
  private GpuParticles2D _slashParticles;
  private GpuParticles2D _chargeParticles;
  private GpuParticles2D _spikeParticles;
  private GpuParticles2D _spikeParticles2;
  private GpuParticles2D _spikeCircle;
  private GpuParticles2D _spikeCircle2;
  private TextureRect _hilt;
  private TextureRect _hilt2;
  private TextureRect _detail;
  private Line2D _trail;
  private Path2D _orbitPath;
  private Control _hitbox;
  private NSelectionReticle _selectionReticle;
  private Tween? _attackTween;
  private Tween? _scaleTween;
  private Tween? _sparkDelay;
  private Tween? _glowTween;
  private Tween? _trailFadeTween;
  private Vector2 _trailStart;
  private float _bladeSize;
  private const float _orbitSpeed = 60f;
  private Vector2 _targetOrbitPosition;
  private bool _isBehindCharacter;
  private const float _hiltThreshold = 0.3f;
  private const float _detailThreshold = 0.66f;
  private bool _isFocused;
  private NHoverTipSet? _hoverTip;
  private bool _isForging;
  private bool _isAttacking;
  private bool _isKeyPressed;
  private float _testCharge;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NSovereignBladeVfx._scenePath);
    }
  }

  public CardModel Card { get; private set; }

  public double OrbitProgress { get; set; }

  public override void _Ready()
  {
    this._spineNode = ((Node) this).GetNode<Node2D>(NodePath.op_Implicit("SpineSword"));
    this._animController = new MegaSprite(Variant.op_Implicit((GodotObject) this._spineNode));
    this._bladeGlow = ((Node) this).GetNode<Node2D>(NodePath.op_Implicit("SpineSword/SwordBone/ScaleContainer/BladeGlow"));
    this._forgeSparks = ((Node) this).GetNode<GpuParticles2D>(NodePath.op_Implicit("SpineSword/SwordBone/ScaleContainer/ForgeSparks"));
    this._spawnFlames = ((Node) this).GetNode<GpuParticles2D>(NodePath.op_Implicit("SpineSword/SwordBone/ScaleContainer/SpawnFlames"));
    this._spawnFlamesBack = ((Node) this).GetNode<GpuParticles2D>(NodePath.op_Implicit("SpineSword/SwordBone/ScaleContainer/SpawnFlamesBack"));
    this._slashParticles = ((Node) this).GetNode<GpuParticles2D>(NodePath.op_Implicit("SpineSword/SlashParticles"));
    this._chargeParticles = ((Node) this).GetNode<GpuParticles2D>(NodePath.op_Implicit("SpineSword/SwordBone/ScaleContainer/ChargeParticles"));
    this._spikeParticles = ((Node) this).GetNode<GpuParticles2D>(NodePath.op_Implicit("SpineSword/SwordBone/ScaleContainer/Spikes"));
    this._spikeParticles2 = ((Node) this).GetNode<GpuParticles2D>(NodePath.op_Implicit("SpineSword/SwordBone/ScaleContainer/Spikes2"));
    this._spikeCircle = ((Node) this).GetNode<GpuParticles2D>(NodePath.op_Implicit("SpineSword/SwordBone/ScaleContainer/SpikeCircle"));
    this._spikeCircle2 = ((Node) this).GetNode<GpuParticles2D>(NodePath.op_Implicit("SpineSword/SwordBone/ScaleContainer/SpikeCircle2"));
    this._hilt = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("SpineSword/SwordBone/ScaleContainer/Hilt"));
    this._hilt2 = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("SpineSword/SwordBone/ScaleContainer/Hilt2"));
    this._detail = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("SpineSword/SwordBone/ScaleContainer/Detail"));
    this._trail = ((Node) this).GetNode<Line2D>(NodePath.op_Implicit("Trail"));
    this._orbitPath = ((Node) this).GetNode<Path2D>(NodePath.op_Implicit("%Path"));
    this._hitbox = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Hitbox"));
    this._selectionReticle = ((Node) this).GetNode<NSelectionReticle>(NodePath.op_Implicit("%SelectionReticle"));
    ((GodotObject) this._hitbox).Connect(Control.SignalName.MouseEntered, Callable.From(new Action(this.OnFocused)), 0U);
    ((GodotObject) this._hitbox).Connect(Control.SignalName.MouseExited, Callable.From(new Action(this.OnUnfocused)), 0U);
    ((GodotObject) this._hitbox).Connect(Control.SignalName.FocusEntered, Callable.From(new Action(this.OnFocused)), 0U);
    ((GodotObject) this._hitbox).Connect(Control.SignalName.FocusExited, Callable.From(new Action(this.OnUnfocused)), 0U);
    this._forgeSparks.Emitting = false;
    this._forgeSparks.OneShot = true;
    this._spawnFlames.Emitting = false;
    this._spawnFlames.OneShot = true;
    this._spawnFlamesBack.Emitting = false;
    this._spawnFlamesBack.OneShot = true;
    this._slashParticles.Emitting = false;
    this._slashParticles.OneShot = true;
    this._chargeParticles.Emitting = false;
    this._spikeParticles2.Emitting = false;
    this._spikeCircle2.Emitting = false;
    ((CanvasItem) this._bladeGlow).Modulate = Colors.Transparent;
    ((CanvasItem) this._bladeGlow).Visible = false;
    ((Node2D) this._trail).GlobalPosition = Vector2.Zero;
    this._trail.ClearPoints();
    ((Node) this).RunWhenSpineReady(this._animController, (Action<MegaAnimationState>) (animState => animState.SetAnimation("idle_loop")));
    this._spineNode.Scale = Vector2.Zero;
    ((CanvasItem) this._spineNode).Visible = true;
    ((GodotObject) NTargetManager.Instance).Connect(NTargetManager.SignalName.TargetingBegan, Callable.From(new Action(this.OnTargetingBegan)), 0U);
    ((GodotObject) NTargetManager.Instance).Connect(NTargetManager.SignalName.TargetingEnded, Callable.From(new Action(this.OnTargetingEnded)), 0U);
    this._owner = this.Card.Owner;
    this._owner.Creature.Died += new Action<Creature>(this.OnOwnerDied);
  }

  public override void _ExitTree()
  {
    this._attackTween?.Kill();
    this._scaleTween?.Kill();
    this._sparkDelay?.Kill();
    this._glowTween?.Kill();
    this._trailFadeTween?.Kill();
    this._owner.Creature.Died -= new Action<Creature>(this.OnOwnerDied);
  }

  public static NSovereignBladeVfx? Create(CardModel card)
  {
    if (TestMode.IsOn)
      return (NSovereignBladeVfx) null;
    NSovereignBladeVfx nsovereignBladeVfx = PreloadManager.Cache.GetScene(NSovereignBladeVfx._scenePath).Instantiate<NSovereignBladeVfx>((PackedScene.GenEditState) 0L);
    nsovereignBladeVfx.Card = card;
    return nsovereignBladeVfx;
  }

  public override void _Process(double delta)
  {
    float bakedLength = this._orbitPath.Curve.GetBakedLength();
    if (this._hoverTip == null)
      this.OrbitProgress += 60.0 * delta / (double) bakedLength;
    double num = this.OrbitProgress % 1.0;
    bool flag = num > 0.25 && num < 0.77999997138977051;
    if (flag != this._isBehindCharacter && (double) this._bladeSize < 0.60000002384185791)
    {
      this._isBehindCharacter = !this._isBehindCharacter;
      ((Node) this).GetParent().MoveChildSafely((Node) this, flag ? 0 : ((Node) this).GetParent().GetChildCount(false) - 1);
    }
    Vector2 vector2_1 = Transform2D.op_Multiply(((Node2D) this._orbitPath).GlobalTransform, this._orbitPath.Curve.SampleBakedWithRotation((float) (this.OrbitProgress % 1.0) * bakedLength, false).Origin);
    vector2_1.X = Mathf.Lerp(vector2_1.X, this.GlobalPosition.X + 200f, Mathf.Clamp(this._bladeSize / 1.25f, 0.0f, 1f));
    this._targetOrbitPosition = Vector2.op_Addition(vector2_1, Vector2.op_Multiply(Vector2.op_Multiply(Vector2.Up, this._spineNode.Scale.Y - 1f), 100f));
    if (this._isAttacking)
      return;
    Node2D spineNode = this._spineNode;
    Vector2 globalPosition = this._spineNode.GlobalPosition;
    Vector2 vector2_2 = ((Vector2) ref globalPosition).Lerp(this._targetOrbitPosition, (float) delta * 7f);
    spineNode.GlobalPosition = vector2_2;
  }

  public void Forge(float bladeDamage = 0.0f, bool showFlames = false)
  {
    if (this._isForging)
      this.CleanupForge();
    this._bladeSize = Mathf.Clamp(Mathf.Lerp(0.0f, 1f, bladeDamage / 200f), 0.0f, 1f);
    this._isForging = true;
    int num = (int) ((double) this._bladeSize * 30.0);
    if (num > 0)
    {
      this._chargeParticles.Amount = num;
      this._chargeParticles.Emitting = true;
    }
    else
      this._chargeParticles.Emitting = false;
    ((CanvasItem) this._hilt).Visible = (double) this._bladeSize < 0.30000001192092896;
    ((CanvasItem) this._hilt2).Visible = !((CanvasItem) this._hilt).Visible;
    this._spikeParticles.Emitting = ((CanvasItem) this._spikeParticles).Visible = ((CanvasItem) this._hilt).Visible;
    this._spikeParticles2.Emitting = ((CanvasItem) this._spikeParticles2).Visible = !((CanvasItem) this._hilt).Visible;
    this._spikeCircle.Emitting = ((CanvasItem) this._spikeCircle).Visible = ((CanvasItem) this._hilt).Visible;
    this._spikeCircle2.Emitting = ((CanvasItem) this._spikeCircle2).Visible = !((CanvasItem) this._hilt).Visible;
    ((CanvasItem) this._detail).Visible = (double) bladeDamage >= 0.6600000262260437;
    ((CanvasItem) this._bladeGlow).Visible = true;
    Color color1 = Color.FromHtml(string.op_Implicit("#ff7300"));
    Color color2 = color1;
    color2.A = 0.0f;
    this._glowTween = ((Node) this).CreateTween();
    if (showFlames)
      this.FireFlames();
    this._glowTween.TweenProperty((GodotObject) this._bladeGlow, NodePath.op_Implicit("modulate"), Variant.op_Implicit(color1), 0.05).SetEase((Tween.EaseType) 1L);
    this._glowTween.Chain().TweenProperty((GodotObject) this._bladeGlow, NodePath.op_Implicit("modulate"), Variant.op_Implicit(color2), 0.5).SetEase((Tween.EaseType) 0L).SetTrans((Tween.TransitionType) 7L);
    this._glowTween.Chain().TweenCallback(Callable.From(new Action(this.CleanupForge)));
    Vector2 vector2 = Vector2.op_Multiply(Vector2.One, Mathf.Lerp(0.9f, 2f, this._bladeSize));
    this._scaleTween = ((Node) this).CreateTween();
    this._scaleTween.TweenProperty((GodotObject) this._spineNode, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(vector2, 1.2f)), 0.05000000074505806).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    this._scaleTween.Chain().TweenCallback(Callable.From(new Action(this.FireSparks)));
    this._scaleTween.Chain().TweenProperty((GodotObject) this._spineNode, NodePath.op_Implicit("scale"), Variant.op_Implicit(vector2), 0.30000001192092896).SetEase((Tween.EaseType) 2L).SetTrans((Tween.TransitionType) 7L);
  }

  public void Attack(Vector2 targetPos)
  {
    if (this._isAttacking)
      this.CleanupAttack();
    this._isAttacking = true;
    this._animController.GetAnimationState().SetAnimation("attack", false);
    this._attackTween = ((Node) this).CreateTween();
    Vector2 vector2;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2).\u002Ector(this._spineNode.GlobalPosition.X - 50f, this._spineNode.GlobalPosition.Y);
    ((CanvasItem) this._trail).Visible = true;
    this._trailStart = vector2;
    this._attackTween.TweenProperty((GodotObject) this._spineNode, NodePath.op_Implicit("rotation"), Variant.op_Implicit(this._spineNode.GetAngleTo(targetPos)), 0.05000000074505806);
    this._attackTween.Parallel().TweenProperty((GodotObject) this._spineNode, NodePath.op_Implicit("global_position"), Variant.op_Implicit(vector2), 0.079999998211860657).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._attackTween.Chain().TweenProperty((GodotObject) this._spineNode, NodePath.op_Implicit("rotation"), Variant.op_Implicit(this._spineNode.GetAngleTo(targetPos)), 0.0);
    this._attackTween.Parallel().TweenProperty((GodotObject) this._spineNode, NodePath.op_Implicit("global_position"), Variant.op_Implicit(targetPos), 0.05000000074505806).SetEase((Tween.EaseType) 0L).SetTrans((Tween.TransitionType) 5L);
    this._attackTween.Chain().TweenCallback(Callable.From(new Action(this.EndSlash)));
    this._attackTween.TweenInterval(0.25);
    this._attackTween.Chain().TweenCallback(Callable.From(new Action(this.FireSparks))).SetDelay(0.30000001192092896);
    this._attackTween.Chain().TweenCallback(Callable.From(new Action(this.CleanupAttack)));
    this.UpdateHoverTip();
  }

  private void OnTargetingBegan()
  {
    this._hitbox.MouseFilter = (Control.MouseFilterEnum) 2L;
    this.UpdateHoverTip();
  }

  private void OnTargetingEnded()
  {
    this._hitbox.MouseFilter = (Control.MouseFilterEnum) 0L;
    this.UpdateHoverTip();
  }

  private void OnFocused()
  {
    this._isFocused = true;
    if (NCombatRoom.Instance.Ui.Hand.InCardPlay)
      return;
    this.UpdateHoverTip();
  }

  private void OnUnfocused()
  {
    this._isFocused = false;
    this.UpdateHoverTip();
  }

  private void UpdateHoverTip()
  {
    bool flag = this._isFocused && !this._isAttacking && !NTargetManager.Instance.IsInSelection && this._hitbox.MouseFilter != 2L;
    if (flag)
    {
      this._selectionReticle.OnSelect();
      if (this._hoverTip != null)
        return;
      this._hoverTip = NHoverTipSet.CreateAndShow(this._hitbox, HoverTipFactory.FromCard(this.Card));
      this._hoverTip?.SetGlobalPosition(Vector2.op_Addition(this._hitbox.GlobalPosition, Vector2.op_Multiply(Vector2.Right, this._hitbox.Size.X)), false);
    }
    else
    {
      if (flag)
        return;
      this._selectionReticle.OnDeselect();
      if (this._hoverTip == null)
        return;
      NHoverTipSet.Remove(this._hitbox);
      this._hoverTip = (NHoverTipSet) null;
    }
  }

  private void FireSparks() => this._forgeSparks.Restart();

  private void FireFlames()
  {
    this._spawnFlames.Restart();
    this._spawnFlamesBack.Restart();
  }

  private void EndSlash()
  {
    this._chargeParticles.Emitting = false;
    this._chargeParticles.Restart();
    ((Node2D) this._slashParticles).Rotation = this._spineNode.GetAngleTo(this._trailStart) - 1.5708f;
    this._slashParticles.Restart();
    this._trail.AddPoint(this._trailStart, -1);
    this._trail.AddPoint(((Node) this).GetNode<Node2D>(NodePath.op_Implicit("SpineSword/SwordBone/ScaleContainer/SpikeCircle")).GlobalPosition, -1);
    ((CanvasItem) this._trail).Modulate = Colors.White;
    this._trailFadeTween = ((Node) this).CreateTween();
    this._trailFadeTween.TweenProperty((GodotObject) this._trail, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.20000000298023224);
  }

  private void CleanupForge()
  {
    this._isForging = false;
    this._scaleTween?.Kill();
    this._glowTween?.Kill();
  }

  private void CleanupAttack()
  {
    this._isAttacking = false;
    this._attackTween?.Kill();
    this._animController.GetAnimationState().SetAnimation("idle_loop");
    this._spineNode.Rotation = 0.0f;
    this._trail.ClearPoints();
  }

  public void RemoveSovereignBlade()
  {
    this._scaleTween?.Kill();
    this._scaleTween = ((Node) this).CreateTween();
    this._scaleTween.TweenProperty((GodotObject) this._spineNode, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.Zero), 0.20000000298023224).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    this._scaleTween.Chain().TweenCallback(Callable.From(new Action(((GodotTreeExtensions) this).QueueFreeSafely)));
  }

  private void OnOwnerDied(Creature creature)
  {
    this._hitbox.MouseFilter = (Control.MouseFilterEnum) 2L;
    this.UpdateHoverTip();
    this.RemoveSovereignBlade();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(16 /*0x10*/)
    {
      new MethodInfo(NSovereignBladeVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSovereignBladeVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSovereignBladeVfx.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NSovereignBladeVfx.MethodName.Forge, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("bladeDamage"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("showFlames"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NSovereignBladeVfx.MethodName.Attack, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("targetPos"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NSovereignBladeVfx.MethodName.OnTargetingBegan, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSovereignBladeVfx.MethodName.OnTargetingEnded, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSovereignBladeVfx.MethodName.OnFocused, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSovereignBladeVfx.MethodName.OnUnfocused, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSovereignBladeVfx.MethodName.UpdateHoverTip, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSovereignBladeVfx.MethodName.FireSparks, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSovereignBladeVfx.MethodName.FireFlames, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSovereignBladeVfx.MethodName.EndSlash, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSovereignBladeVfx.MethodName.CleanupForge, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSovereignBladeVfx.MethodName.CleanupAttack, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSovereignBladeVfx.MethodName.RemoveSovereignBlade, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSovereignBladeVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSovereignBladeVfx.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSovereignBladeVfx.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSovereignBladeVfx.MethodName.Forge) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.Forge(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSovereignBladeVfx.MethodName.Attack) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.Attack(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSovereignBladeVfx.MethodName.OnTargetingBegan) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnTargetingBegan();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSovereignBladeVfx.MethodName.OnTargetingEnded) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnTargetingEnded();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSovereignBladeVfx.MethodName.OnFocused) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocused();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSovereignBladeVfx.MethodName.OnUnfocused) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocused();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSovereignBladeVfx.MethodName.UpdateHoverTip) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateHoverTip();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSovereignBladeVfx.MethodName.FireSparks) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.FireSparks();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSovereignBladeVfx.MethodName.FireFlames) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.FireFlames();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSovereignBladeVfx.MethodName.EndSlash) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.EndSlash();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSovereignBladeVfx.MethodName.CleanupForge) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.CleanupForge();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSovereignBladeVfx.MethodName.CleanupAttack) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.CleanupAttack();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NSovereignBladeVfx.MethodName.RemoveSovereignBlade) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.RemoveSovereignBlade();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSovereignBladeVfx.MethodName._Ready) || StringName.op_Equality(ref method, NSovereignBladeVfx.MethodName._ExitTree) || StringName.op_Equality(ref method, NSovereignBladeVfx.MethodName._Process) || StringName.op_Equality(ref method, NSovereignBladeVfx.MethodName.Forge) || StringName.op_Equality(ref method, NSovereignBladeVfx.MethodName.Attack) || StringName.op_Equality(ref method, NSovereignBladeVfx.MethodName.OnTargetingBegan) || StringName.op_Equality(ref method, NSovereignBladeVfx.MethodName.OnTargetingEnded) || StringName.op_Equality(ref method, NSovereignBladeVfx.MethodName.OnFocused) || StringName.op_Equality(ref method, NSovereignBladeVfx.MethodName.OnUnfocused) || StringName.op_Equality(ref method, NSovereignBladeVfx.MethodName.UpdateHoverTip) || StringName.op_Equality(ref method, NSovereignBladeVfx.MethodName.FireSparks) || StringName.op_Equality(ref method, NSovereignBladeVfx.MethodName.FireFlames) || StringName.op_Equality(ref method, NSovereignBladeVfx.MethodName.EndSlash) || StringName.op_Equality(ref method, NSovereignBladeVfx.MethodName.CleanupForge) || StringName.op_Equality(ref method, NSovereignBladeVfx.MethodName.CleanupAttack) || StringName.op_Equality(ref method, NSovereignBladeVfx.MethodName.RemoveSovereignBlade) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName.OrbitProgress))
    {
      this.OrbitProgress = VariantUtils.ConvertTo<double>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._spineNode))
    {
      this._spineNode = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._bladeGlow))
    {
      this._bladeGlow = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._forgeSparks))
    {
      this._forgeSparks = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._spawnFlames))
    {
      this._spawnFlames = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._spawnFlamesBack))
    {
      this._spawnFlamesBack = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._slashParticles))
    {
      this._slashParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._chargeParticles))
    {
      this._chargeParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._spikeParticles))
    {
      this._spikeParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._spikeParticles2))
    {
      this._spikeParticles2 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._spikeCircle))
    {
      this._spikeCircle = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._spikeCircle2))
    {
      this._spikeCircle2 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._hilt))
    {
      this._hilt = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._hilt2))
    {
      this._hilt2 = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._detail))
    {
      this._detail = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._trail))
    {
      this._trail = VariantUtils.ConvertTo<Line2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._orbitPath))
    {
      this._orbitPath = VariantUtils.ConvertTo<Path2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._hitbox))
    {
      this._hitbox = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._selectionReticle))
    {
      this._selectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._attackTween))
    {
      this._attackTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._scaleTween))
    {
      this._scaleTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._sparkDelay))
    {
      this._sparkDelay = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._glowTween))
    {
      this._glowTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._trailFadeTween))
    {
      this._trailFadeTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._trailStart))
    {
      this._trailStart = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._bladeSize))
    {
      this._bladeSize = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._targetOrbitPosition))
    {
      this._targetOrbitPosition = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._isBehindCharacter))
    {
      this._isBehindCharacter = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._isFocused))
    {
      this._isFocused = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._hoverTip))
    {
      this._hoverTip = VariantUtils.ConvertTo<NHoverTipSet>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._isForging))
    {
      this._isForging = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._isAttacking))
    {
      this._isAttacking = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._isKeyPressed))
    {
      this._isKeyPressed = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._testCharge))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._testCharge = VariantUtils.ConvertTo<float>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName.OrbitProgress))
    {
      ref godot_variant local = ref value;
      double orbitProgress = this.OrbitProgress;
      godot_variant from = VariantUtils.CreateFrom<double>(ref orbitProgress);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._spineNode))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._spineNode);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._bladeGlow))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._bladeGlow);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._forgeSparks))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._forgeSparks);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._spawnFlames))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._spawnFlames);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._spawnFlamesBack))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._spawnFlamesBack);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._slashParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._slashParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._chargeParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._chargeParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._spikeParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._spikeParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._spikeParticles2))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._spikeParticles2);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._spikeCircle))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._spikeCircle);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._spikeCircle2))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._spikeCircle2);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._hilt))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._hilt);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._hilt2))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._hilt2);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._detail))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._detail);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._trail))
    {
      value = VariantUtils.CreateFrom<Line2D>(ref this._trail);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._orbitPath))
    {
      value = VariantUtils.CreateFrom<Path2D>(ref this._orbitPath);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._hitbox))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._hitbox);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._selectionReticle))
    {
      value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._selectionReticle);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._attackTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._attackTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._scaleTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._scaleTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._sparkDelay))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._sparkDelay);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._glowTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._glowTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._trailFadeTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._trailFadeTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._trailStart))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._trailStart);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._bladeSize))
    {
      value = VariantUtils.CreateFrom<float>(ref this._bladeSize);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._targetOrbitPosition))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._targetOrbitPosition);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._isBehindCharacter))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isBehindCharacter);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._isFocused))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isFocused);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._hoverTip))
    {
      value = VariantUtils.CreateFrom<NHoverTipSet>(ref this._hoverTip);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._isForging))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isForging);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._isAttacking))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isAttacking);
      return true;
    }
    if (StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._isKeyPressed))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isKeyPressed);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSovereignBladeVfx.PropertyName._testCharge))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<float>(ref this._testCharge);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NSovereignBladeVfx.PropertyName._spineNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSovereignBladeVfx.PropertyName._bladeGlow, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSovereignBladeVfx.PropertyName._forgeSparks, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSovereignBladeVfx.PropertyName._spawnFlames, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSovereignBladeVfx.PropertyName._spawnFlamesBack, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSovereignBladeVfx.PropertyName._slashParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSovereignBladeVfx.PropertyName._chargeParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSovereignBladeVfx.PropertyName._spikeParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSovereignBladeVfx.PropertyName._spikeParticles2, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSovereignBladeVfx.PropertyName._spikeCircle, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSovereignBladeVfx.PropertyName._spikeCircle2, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSovereignBladeVfx.PropertyName._hilt, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSovereignBladeVfx.PropertyName._hilt2, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSovereignBladeVfx.PropertyName._detail, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSovereignBladeVfx.PropertyName._trail, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSovereignBladeVfx.PropertyName._orbitPath, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSovereignBladeVfx.PropertyName._hitbox, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSovereignBladeVfx.PropertyName._selectionReticle, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSovereignBladeVfx.PropertyName._attackTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSovereignBladeVfx.PropertyName._scaleTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSovereignBladeVfx.PropertyName._sparkDelay, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSovereignBladeVfx.PropertyName._glowTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSovereignBladeVfx.PropertyName._trailFadeTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NSovereignBladeVfx.PropertyName._trailStart, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NSovereignBladeVfx.PropertyName._bladeSize, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NSovereignBladeVfx.PropertyName.OrbitProgress, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NSovereignBladeVfx.PropertyName._targetOrbitPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NSovereignBladeVfx.PropertyName._isBehindCharacter, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NSovereignBladeVfx.PropertyName._isFocused, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSovereignBladeVfx.PropertyName._hoverTip, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NSovereignBladeVfx.PropertyName._isForging, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NSovereignBladeVfx.PropertyName._isAttacking, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NSovereignBladeVfx.PropertyName._isKeyPressed, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NSovereignBladeVfx.PropertyName._testCharge, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName orbitProgress1 = NSovereignBladeVfx.PropertyName.OrbitProgress;
    double orbitProgress2 = this.OrbitProgress;
    Variant variant = Variant.From<double>(ref orbitProgress2);
    serializationInfo.AddProperty(orbitProgress1, variant);
    info.AddProperty(NSovereignBladeVfx.PropertyName._spineNode, Variant.From<Node2D>(ref this._spineNode));
    info.AddProperty(NSovereignBladeVfx.PropertyName._bladeGlow, Variant.From<Node2D>(ref this._bladeGlow));
    info.AddProperty(NSovereignBladeVfx.PropertyName._forgeSparks, Variant.From<GpuParticles2D>(ref this._forgeSparks));
    info.AddProperty(NSovereignBladeVfx.PropertyName._spawnFlames, Variant.From<GpuParticles2D>(ref this._spawnFlames));
    info.AddProperty(NSovereignBladeVfx.PropertyName._spawnFlamesBack, Variant.From<GpuParticles2D>(ref this._spawnFlamesBack));
    info.AddProperty(NSovereignBladeVfx.PropertyName._slashParticles, Variant.From<GpuParticles2D>(ref this._slashParticles));
    info.AddProperty(NSovereignBladeVfx.PropertyName._chargeParticles, Variant.From<GpuParticles2D>(ref this._chargeParticles));
    info.AddProperty(NSovereignBladeVfx.PropertyName._spikeParticles, Variant.From<GpuParticles2D>(ref this._spikeParticles));
    info.AddProperty(NSovereignBladeVfx.PropertyName._spikeParticles2, Variant.From<GpuParticles2D>(ref this._spikeParticles2));
    info.AddProperty(NSovereignBladeVfx.PropertyName._spikeCircle, Variant.From<GpuParticles2D>(ref this._spikeCircle));
    info.AddProperty(NSovereignBladeVfx.PropertyName._spikeCircle2, Variant.From<GpuParticles2D>(ref this._spikeCircle2));
    info.AddProperty(NSovereignBladeVfx.PropertyName._hilt, Variant.From<TextureRect>(ref this._hilt));
    info.AddProperty(NSovereignBladeVfx.PropertyName._hilt2, Variant.From<TextureRect>(ref this._hilt2));
    info.AddProperty(NSovereignBladeVfx.PropertyName._detail, Variant.From<TextureRect>(ref this._detail));
    info.AddProperty(NSovereignBladeVfx.PropertyName._trail, Variant.From<Line2D>(ref this._trail));
    info.AddProperty(NSovereignBladeVfx.PropertyName._orbitPath, Variant.From<Path2D>(ref this._orbitPath));
    info.AddProperty(NSovereignBladeVfx.PropertyName._hitbox, Variant.From<Control>(ref this._hitbox));
    info.AddProperty(NSovereignBladeVfx.PropertyName._selectionReticle, Variant.From<NSelectionReticle>(ref this._selectionReticle));
    info.AddProperty(NSovereignBladeVfx.PropertyName._attackTween, Variant.From<Tween>(ref this._attackTween));
    info.AddProperty(NSovereignBladeVfx.PropertyName._scaleTween, Variant.From<Tween>(ref this._scaleTween));
    info.AddProperty(NSovereignBladeVfx.PropertyName._sparkDelay, Variant.From<Tween>(ref this._sparkDelay));
    info.AddProperty(NSovereignBladeVfx.PropertyName._glowTween, Variant.From<Tween>(ref this._glowTween));
    info.AddProperty(NSovereignBladeVfx.PropertyName._trailFadeTween, Variant.From<Tween>(ref this._trailFadeTween));
    info.AddProperty(NSovereignBladeVfx.PropertyName._trailStart, Variant.From<Vector2>(ref this._trailStart));
    info.AddProperty(NSovereignBladeVfx.PropertyName._bladeSize, Variant.From<float>(ref this._bladeSize));
    info.AddProperty(NSovereignBladeVfx.PropertyName._targetOrbitPosition, Variant.From<Vector2>(ref this._targetOrbitPosition));
    info.AddProperty(NSovereignBladeVfx.PropertyName._isBehindCharacter, Variant.From<bool>(ref this._isBehindCharacter));
    info.AddProperty(NSovereignBladeVfx.PropertyName._isFocused, Variant.From<bool>(ref this._isFocused));
    info.AddProperty(NSovereignBladeVfx.PropertyName._hoverTip, Variant.From<NHoverTipSet>(ref this._hoverTip));
    info.AddProperty(NSovereignBladeVfx.PropertyName._isForging, Variant.From<bool>(ref this._isForging));
    info.AddProperty(NSovereignBladeVfx.PropertyName._isAttacking, Variant.From<bool>(ref this._isAttacking));
    info.AddProperty(NSovereignBladeVfx.PropertyName._isKeyPressed, Variant.From<bool>(ref this._isKeyPressed));
    info.AddProperty(NSovereignBladeVfx.PropertyName._testCharge, Variant.From<float>(ref this._testCharge));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NSovereignBladeVfx.PropertyName.OrbitProgress, ref variant1))
      this.OrbitProgress = ((Variant) ref variant1).As<double>();
    Variant variant2;
    if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._spineNode, ref variant2))
      this._spineNode = ((Variant) ref variant2).As<Node2D>();
    Variant variant3;
    if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._bladeGlow, ref variant3))
      this._bladeGlow = ((Variant) ref variant3).As<Node2D>();
    Variant variant4;
    if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._forgeSparks, ref variant4))
      this._forgeSparks = ((Variant) ref variant4).As<GpuParticles2D>();
    Variant variant5;
    if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._spawnFlames, ref variant5))
      this._spawnFlames = ((Variant) ref variant5).As<GpuParticles2D>();
    Variant variant6;
    if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._spawnFlamesBack, ref variant6))
      this._spawnFlamesBack = ((Variant) ref variant6).As<GpuParticles2D>();
    Variant variant7;
    if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._slashParticles, ref variant7))
      this._slashParticles = ((Variant) ref variant7).As<GpuParticles2D>();
    Variant variant8;
    if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._chargeParticles, ref variant8))
      this._chargeParticles = ((Variant) ref variant8).As<GpuParticles2D>();
    Variant variant9;
    if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._spikeParticles, ref variant9))
      this._spikeParticles = ((Variant) ref variant9).As<GpuParticles2D>();
    Variant variant10;
    if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._spikeParticles2, ref variant10))
      this._spikeParticles2 = ((Variant) ref variant10).As<GpuParticles2D>();
    Variant variant11;
    if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._spikeCircle, ref variant11))
      this._spikeCircle = ((Variant) ref variant11).As<GpuParticles2D>();
    Variant variant12;
    if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._spikeCircle2, ref variant12))
      this._spikeCircle2 = ((Variant) ref variant12).As<GpuParticles2D>();
    Variant variant13;
    if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._hilt, ref variant13))
      this._hilt = ((Variant) ref variant13).As<TextureRect>();
    Variant variant14;
    if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._hilt2, ref variant14))
      this._hilt2 = ((Variant) ref variant14).As<TextureRect>();
    Variant variant15;
    if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._detail, ref variant15))
      this._detail = ((Variant) ref variant15).As<TextureRect>();
    Variant variant16;
    if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._trail, ref variant16))
      this._trail = ((Variant) ref variant16).As<Line2D>();
    Variant variant17;
    if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._orbitPath, ref variant17))
      this._orbitPath = ((Variant) ref variant17).As<Path2D>();
    Variant variant18;
    if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._hitbox, ref variant18))
      this._hitbox = ((Variant) ref variant18).As<Control>();
    Variant variant19;
    if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._selectionReticle, ref variant19))
      this._selectionReticle = ((Variant) ref variant19).As<NSelectionReticle>();
    Variant variant20;
    if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._attackTween, ref variant20))
      this._attackTween = ((Variant) ref variant20).As<Tween>();
    Variant variant21;
    if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._scaleTween, ref variant21))
      this._scaleTween = ((Variant) ref variant21).As<Tween>();
    Variant variant22;
    if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._sparkDelay, ref variant22))
      this._sparkDelay = ((Variant) ref variant22).As<Tween>();
    Variant variant23;
    if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._glowTween, ref variant23))
      this._glowTween = ((Variant) ref variant23).As<Tween>();
    Variant variant24;
    if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._trailFadeTween, ref variant24))
      this._trailFadeTween = ((Variant) ref variant24).As<Tween>();
    Variant variant25;
    if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._trailStart, ref variant25))
      this._trailStart = ((Variant) ref variant25).As<Vector2>();
    Variant variant26;
    if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._bladeSize, ref variant26))
      this._bladeSize = ((Variant) ref variant26).As<float>();
    Variant variant27;
    if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._targetOrbitPosition, ref variant27))
      this._targetOrbitPosition = ((Variant) ref variant27).As<Vector2>();
    Variant variant28;
    if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._isBehindCharacter, ref variant28))
      this._isBehindCharacter = ((Variant) ref variant28).As<bool>();
    Variant variant29;
    if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._isFocused, ref variant29))
      this._isFocused = ((Variant) ref variant29).As<bool>();
    Variant variant30;
    if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._hoverTip, ref variant30))
      this._hoverTip = ((Variant) ref variant30).As<NHoverTipSet>();
    Variant variant31;
    if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._isForging, ref variant31))
      this._isForging = ((Variant) ref variant31).As<bool>();
    Variant variant32;
    if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._isAttacking, ref variant32))
      this._isAttacking = ((Variant) ref variant32).As<bool>();
    Variant variant33;
    if (info.TryGetProperty(NSovereignBladeVfx.PropertyName._isKeyPressed, ref variant33))
      this._isKeyPressed = ((Variant) ref variant33).As<bool>();
    Variant variant34;
    if (!info.TryGetProperty(NSovereignBladeVfx.PropertyName._testCharge, ref variant34))
      return;
    this._testCharge = ((Variant) ref variant34).As<float>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName Forge = StringName.op_Implicit(nameof (Forge));
    public static readonly StringName Attack = StringName.op_Implicit(nameof (Attack));
    public static readonly StringName OnTargetingBegan = StringName.op_Implicit(nameof (OnTargetingBegan));
    public static readonly StringName OnTargetingEnded = StringName.op_Implicit(nameof (OnTargetingEnded));
    public static readonly StringName OnFocused = StringName.op_Implicit(nameof (OnFocused));
    public static readonly StringName OnUnfocused = StringName.op_Implicit(nameof (OnUnfocused));
    public static readonly StringName UpdateHoverTip = StringName.op_Implicit(nameof (UpdateHoverTip));
    public static readonly StringName FireSparks = StringName.op_Implicit(nameof (FireSparks));
    public static readonly StringName FireFlames = StringName.op_Implicit(nameof (FireFlames));
    public static readonly StringName EndSlash = StringName.op_Implicit(nameof (EndSlash));
    public static readonly StringName CleanupForge = StringName.op_Implicit(nameof (CleanupForge));
    public static readonly StringName CleanupAttack = StringName.op_Implicit(nameof (CleanupAttack));
    public static readonly StringName RemoveSovereignBlade = StringName.op_Implicit(nameof (RemoveSovereignBlade));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName OrbitProgress = StringName.op_Implicit(nameof (OrbitProgress));
    public static readonly StringName _spineNode = StringName.op_Implicit(nameof (_spineNode));
    public static readonly StringName _bladeGlow = StringName.op_Implicit(nameof (_bladeGlow));
    public static readonly StringName _forgeSparks = StringName.op_Implicit(nameof (_forgeSparks));
    public static readonly StringName _spawnFlames = StringName.op_Implicit(nameof (_spawnFlames));
    public static readonly StringName _spawnFlamesBack = StringName.op_Implicit(nameof (_spawnFlamesBack));
    public static readonly StringName _slashParticles = StringName.op_Implicit(nameof (_slashParticles));
    public static readonly StringName _chargeParticles = StringName.op_Implicit(nameof (_chargeParticles));
    public static readonly StringName _spikeParticles = StringName.op_Implicit(nameof (_spikeParticles));
    public static readonly StringName _spikeParticles2 = StringName.op_Implicit(nameof (_spikeParticles2));
    public static readonly StringName _spikeCircle = StringName.op_Implicit(nameof (_spikeCircle));
    public static readonly StringName _spikeCircle2 = StringName.op_Implicit(nameof (_spikeCircle2));
    public static readonly StringName _hilt = StringName.op_Implicit(nameof (_hilt));
    public static readonly StringName _hilt2 = StringName.op_Implicit(nameof (_hilt2));
    public static readonly StringName _detail = StringName.op_Implicit(nameof (_detail));
    public static readonly StringName _trail = StringName.op_Implicit(nameof (_trail));
    public static readonly StringName _orbitPath = StringName.op_Implicit(nameof (_orbitPath));
    public static readonly StringName _hitbox = StringName.op_Implicit(nameof (_hitbox));
    public static readonly StringName _selectionReticle = StringName.op_Implicit(nameof (_selectionReticle));
    public static readonly StringName _attackTween = StringName.op_Implicit(nameof (_attackTween));
    public static readonly StringName _scaleTween = StringName.op_Implicit(nameof (_scaleTween));
    public static readonly StringName _sparkDelay = StringName.op_Implicit(nameof (_sparkDelay));
    public static readonly StringName _glowTween = StringName.op_Implicit(nameof (_glowTween));
    public static readonly StringName _trailFadeTween = StringName.op_Implicit(nameof (_trailFadeTween));
    public static readonly StringName _trailStart = StringName.op_Implicit(nameof (_trailStart));
    public static readonly StringName _bladeSize = StringName.op_Implicit(nameof (_bladeSize));
    public static readonly StringName _targetOrbitPosition = StringName.op_Implicit(nameof (_targetOrbitPosition));
    public static readonly StringName _isBehindCharacter = StringName.op_Implicit(nameof (_isBehindCharacter));
    public static readonly StringName _isFocused = StringName.op_Implicit(nameof (_isFocused));
    public static readonly StringName _hoverTip = StringName.op_Implicit(nameof (_hoverTip));
    public static readonly StringName _isForging = StringName.op_Implicit(nameof (_isForging));
    public static readonly StringName _isAttacking = StringName.op_Implicit(nameof (_isAttacking));
    public static readonly StringName _isKeyPressed = StringName.op_Implicit(nameof (_isKeyPressed));
    public static readonly StringName _testCharge = StringName.op_Implicit(nameof (_testCharge));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
