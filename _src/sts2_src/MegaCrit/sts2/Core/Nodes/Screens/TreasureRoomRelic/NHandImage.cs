// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.TreasureRoomRelic.NHandImage
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.TreasureRelicPicking;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.TreasureRoomRelic;

[ScriptPath("res://src/Core/Nodes/Screens/TreasureRoomRelic/NHandImage.cs")]
public class NHandImage : Control
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("ui/hand_image");
  private static readonly Vector2 _pointingPivot = new Vector2(163f, 10f);
  private static readonly Vector2 _fightingPivot = new Vector2(197f, 600f);
  private CancellationTokenSource _cts = new CancellationTokenSource();
  private Marker2D _grabMarker;
  private TextureRect _textureRect;
  private Vector2 _currentVelocity;
  private Vector2 _desiredPosition;
  private Tween? _downTween;
  private NHandImage.State _state;
  private bool _isInFight;
  private Vector2 _originalPosition;
  private float _handAnimateInProgress;

  public Player Player { get; private set; }

  public int Index { get; private set; }

  public bool IsDown { get; private set; }

  public bool IsShown { get; private set; }

  public static NHandImage Create(Player player, int slotIndex)
  {
    NHandImage nhandImage = PreloadManager.Cache.GetScene(NHandImage._scenePath).Instantiate<NHandImage>((PackedScene.GenEditState) 0L);
    nhandImage.Player = player;
    nhandImage.Index = slotIndex;
    return nhandImage;
  }

  public override void _Ready()
  {
    this._textureRect = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("TextureRect"));
    this._grabMarker = ((Node) this).GetNode<Marker2D>(NodePath.op_Implicit("GrabMarker"));
    this._originalPosition = ((Control) this._textureRect).Position;
    float num;
    switch (this.Index % 4)
    {
      case 0:
        num = 0.0f;
        break;
      case 1:
        num = 1.57079637f;
        break;
      case 2:
        num = -1.57079637f;
        break;
      case 3:
        num = 3.14159274f;
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
    this.Rotation = num;
    if (!LocalContext.IsMe(this.Player))
      ((CanvasItem) this).Modulate = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    this._textureRect.Texture = this.Player.Character.ArmPointingTexture;
  }

  public override void _EnterTree() => this._cts = new CancellationTokenSource();

  public override void _ExitTree() => this._cts.Cancel();

  public void SetIsInFight(bool inFight)
  {
    this._isInFight = inFight;
    if (this._isInFight)
    {
      ((Control) this._textureRect).PivotOffset = NHandImage._fightingPivot;
      ((CanvasItem) this).Modulate = Colors.White;
    }
    else
    {
      ((Control) this._textureRect).PivotOffset = NHandImage._pointingPivot;
      if (!LocalContext.IsMe(this.Player))
        ((CanvasItem) this).Modulate = new Color(0.5f, 0.5f, 0.5f, 0.5f);
      else
        ((CanvasItem) this).Modulate = Colors.White;
    }
  }

  public void SetFrozenForRelicAwards(bool frozenForRelicAwards)
  {
    if (frozenForRelicAwards)
    {
      this._state = NHandImage.State.Frozen;
      this._desiredPosition = this.GetFrozenPosition();
    }
    else
      this._state = NHandImage.State.None;
  }

  private Vector2 GetFrozenPosition()
  {
    Rect2 viewportRect = ((CanvasItem) this).GetViewportRect();
    Vector2 down = Vector2.Down;
    Vector2 vector2 = ((Vector2) ref down).Rotated(this.Rotation);
    return Vector2.op_Addition(Vector2.op_Division(((Rect2) ref viewportRect).Size, 2f), Vector2.op_Multiply(Vector2.op_Multiply(((Rect2) ref viewportRect).Size, vector2), 0.1667f));
  }

  public Tween DoFightMove(RelicPickingFightMove move, float duration)
  {
    float num1 = (float) (0.66600000858306885 * (double) duration / 3.0);
    float num2 = (float) (0.33300000429153442 * (double) duration / 3.0);
    int capacity = 6;
    List<float> floatList1 = new List<float>(capacity);
    CollectionsMarshal.SetCount<float>(floatList1, capacity);
    Span<float> span = CollectionsMarshal.AsSpan<float>(floatList1);
    int num3 = 0;
    span[num3] = num1;
    int num4 = num3 + 1;
    span[num4] = num2;
    int num5 = num4 + 1;
    span[num5] = num1;
    int num6 = num5 + 1;
    span[num6] = num2;
    int num7 = num6 + 1;
    span[num7] = num1;
    int num8 = num7 + 1;
    span[num8] = num2;
    List<float> floatList2 = floatList1;
    for (int index = 0; index < floatList2.Count - 1; ++index)
    {
      float num9 = Rng.Chaotic.NextFloat((float) (-(double) duration / 25.0), duration / 25f);
      floatList2[index] += num9;
      floatList2[index + 1] -= num9;
    }
    this.SetTextureToFightMove(RelicPickingFightMove.Rock);
    Tween tween = ((Node) this).CreateTween();
    tween.Chain().TweenProperty((GodotObject) this._textureRect, NodePath.op_Implicit("rotation"), Variant.op_Implicit(Rng.Chaotic.NextFloat(-0.05f, 0.05f) - 0.314159274f), (double) floatList2[0]).SetTrans((Tween.TransitionType) 0L).SetEase((Tween.EaseType) 0L);
    tween.Chain().TweenProperty((GodotObject) this._textureRect, NodePath.op_Implicit("rotation"), Variant.op_Implicit(Rng.Chaotic.NextFloat(-0.02f, 0.02f)), (double) floatList2[1]).SetTrans((Tween.TransitionType) 5L).SetEase((Tween.EaseType) 0L);
    tween.Chain().TweenProperty((GodotObject) this._textureRect, NodePath.op_Implicit("rotation"), Variant.op_Implicit(Rng.Chaotic.NextFloat(-0.05f, 0.05f) - 0.314159274f), (double) floatList2[2]).SetTrans((Tween.TransitionType) 0L).SetEase((Tween.EaseType) 0L);
    tween.Chain().TweenProperty((GodotObject) this._textureRect, NodePath.op_Implicit("rotation"), Variant.op_Implicit(Rng.Chaotic.NextFloat(-0.02f, 0.02f)), (double) floatList2[3]).SetTrans((Tween.TransitionType) 5L).SetEase((Tween.EaseType) 0L);
    tween.Chain().TweenProperty((GodotObject) this._textureRect, NodePath.op_Implicit("rotation"), Variant.op_Implicit(Rng.Chaotic.NextFloat(-0.05f, 0.05f) - 0.314159274f), (double) floatList2[4]).SetTrans((Tween.TransitionType) 0L).SetEase((Tween.EaseType) 0L);
    tween.Chain().TweenProperty((GodotObject) this._textureRect, NodePath.op_Implicit("rotation"), Variant.op_Implicit(Rng.Chaotic.NextFloat(-0.02f, 0.02f)), (double) floatList2[5]).SetTrans((Tween.TransitionType) 5L).SetEase((Tween.EaseType) 0L);
    tween.TweenCallback(Callable.From((Action) (() => this.SetTextureToFightMove(move))));
    return tween;
  }

  private void SetTextureToFightMove(RelicPickingFightMove move)
  {
    TextureRect textureRect = this._textureRect;
    Texture2D texture2D;
    switch (move)
    {
      case RelicPickingFightMove.Rock:
        texture2D = this.Player.Character.ArmRockTexture;
        break;
      case RelicPickingFightMove.Paper:
        texture2D = this.Player.Character.ArmPaperTexture;
        break;
      case RelicPickingFightMove.Scissors:
        texture2D = this.Player.Character.ArmScissorsTexture;
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof (move), (object) move, (string) null);
    }
    textureRect.Texture = texture2D;
  }

  public void SetPointingPosition(Vector2 position)
  {
    if (this._state != NHandImage.State.None)
      return;
    this._desiredPosition = position;
  }

  public void AnimateAway()
  {
    Rect2 viewportRect = ((CanvasItem) this).GetViewportRect();
    Vector2 down = Vector2.Down;
    Vector2 vector2 = ((Vector2) ref down).Rotated(this.Rotation);
    this._desiredPosition = Vector2.op_Addition(Vector2.op_Division(((Rect2) ref viewportRect).Size, 2f), Vector2.op_Multiply(Vector2.op_Multiply(((Rect2) ref viewportRect).Size, vector2), 0.8f));
    this.IsShown = false;
  }

  public void AnimateIn()
  {
    Tween tween = ((Node) this).CreateTween();
    this._handAnimateInProgress = 0.0f;
    tween.TweenMethod(Callable.From<float>((Action<float>) (v => this._handAnimateInProgress = v)), Variant.op_Implicit(0.0f), Variant.op_Implicit(1f), 0.60000002384185791).SetTrans((Tween.TransitionType) 5L).SetEase((Tween.EaseType) 1L);
    this.IsShown = true;
    if (this._state != NHandImage.State.Frozen)
      return;
    this._desiredPosition = this.GetFrozenPosition();
  }

  public void SetIsDown(bool isDown)
  {
    if (this.IsDown == isDown)
      return;
    this.IsDown = isDown;
    this._downTween?.Kill();
    if (isDown)
    {
      ((Control) this._textureRect).Scale = Vector2.op_Multiply(Vector2.One, 0.98f);
    }
    else
    {
      this._downTween = ((Node) this).CreateTween();
      this._downTween.TweenProperty((GodotObject) this._textureRect, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.20000000298023224).SetTrans((Tween.TransitionType) 5L).SetEase((Tween.EaseType) 1L);
    }
  }

  public async Task DoLoseShake(float duration)
  {
    ((Node) this).CreateTween().TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate"), Variant.op_Implicit(new Color(0.5f, 0.5f, 0.5f, 0.5f)), (double) duration * 0.33300000429153442).SetDelay((double) duration * 0.66699999570846558);
    ScreenRumbleInstance rumble = new ScreenRumbleInstance(100f, (double) duration, 5f, RumbleStyle.Rumble);
    while (!rumble.IsDone)
    {
      ((Control) this._textureRect).Position = Vector2.op_Addition(this._originalPosition, rumble.Update(((Node) this).GetProcessDeltaTime()));
      double num = (double) await ((Node) this).AwaitProcessFrame(this._cts.Token);
    }
    ((Control) this._textureRect).Position = this._originalPosition;
    rumble = (ScreenRumbleInstance) null;
  }

  public async Task GrabRelic(NTreasureRoomRelicHolder holder)
  {
    NHandImage.State oldState = this._state;
    this._state = NHandImage.State.GrabbingRelic;
    this.SetTextureToFightMove(RelicPickingFightMove.Paper);
    Tween tween1 = ((Node) this).CreateTween();
    Tween tween2 = tween1;
    NodePath nodePath = NodePath.op_Implicit("global_position");
    Vector2 globalPosition = holder.GlobalPosition;
    Vector2 position = ((Node2D) this._grabMarker).Position;
    Vector2 vector2 = ((Vector2) ref position).Rotated(this.Rotation);
    Variant variant = Variant.op_Implicit(Vector2.op_Addition(Vector2.op_Subtraction(globalPosition, vector2), Vector2.op_Multiply(holder.Size, 0.5f)));
    tween2.TweenProperty((GodotObject) this, nodePath, variant, 0.5).SetTrans((Tween.TransitionType) 1L).SetEase((Tween.EaseType) 2L);
    await tween1.AwaitFinished(this._cts.Token);
    this.SetTextureToFightMove(RelicPickingFightMove.Rock);
    ((Node) holder).Reparent((Node) this, true);
    holder.Rotation = -this.Rotation;
    holder.Position = Vector2.op_Subtraction(((Node2D) this._grabMarker).Position, Vector2.op_Multiply(holder.Size, 0.5f));
    Tween tween3 = ((Node) this).CreateTween();
    tween3.TweenProperty((GodotObject) this, NodePath.op_Implicit("global_position"), Variant.op_Implicit(this._desiredPosition), 0.5).SetTrans((Tween.TransitionType) 1L).SetEase((Tween.EaseType) 2L);
    await tween3.AwaitFinished(this._cts.Token);
    this._state = oldState;
  }

  public void SetSkipped() => this.SetTextureToFightMove(RelicPickingFightMove.Rock);

  public override void _Process(double delta)
  {
    Rect2 viewportRect1 = ((CanvasItem) this).GetViewportRect();
    Vector2 size = ((Rect2) ref viewportRect1).Size;
    int num = this.Index % 4;
    if (this._state != NHandImage.State.GrabbingRelic)
    {
      Vector2 vector2_1 = num != 0 && num != 3 ? Vector2.op_Multiply(num == 1 ? 1f : -1f, Vector2.Left) : Vector2.op_Multiply(num == 0 ? 1f : -1f, Vector2.Down);
      Rect2 viewportRect2 = ((CanvasItem) this).GetViewportRect();
      Vector2 vector2_2 = Vector2.op_Addition(Vector2.op_Division(((Rect2) ref viewportRect2).Size, 2f), Vector2.op_Multiply(((Rect2) ref viewportRect2).Size, vector2_1));
      float smoothTime = this._state != NHandImage.State.Frozen ? (!LocalContext.IsMe(this.Player) ? 0.07f : 0.01f) : 0.25f;
      this.GlobalPosition = MathHelper.SmoothDamp(this.GlobalPosition, ((Vector2) ref vector2_2).Lerp(this._desiredPosition, this._handAnimateInProgress), ref this._currentVelocity, smoothTime, (float) delta);
    }
    if (this._state == NHandImage.State.None)
    {
      if (num == 0 || num == 3)
        ((Control) this._textureRect).Rotation = (float) ((num == 0 ? 1.0 : -1.0) * ((double) this.GlobalPosition.X - (double) size.X / 2.0) / 2000.0);
      else
        ((Control) this._textureRect).Rotation = (float) ((num == 1 ? 1.0 : -1.0) * ((double) this.GlobalPosition.Y - (double) size.Y / 2.0) / 1000.0);
    }
    else
      ((Control) this._textureRect).Rotation = 0.0f;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(14)
    {
      new MethodInfo(NHandImage.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHandImage.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHandImage.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHandImage.MethodName.SetIsInFight, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("inFight"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NHandImage.MethodName.SetFrozenForRelicAwards, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("frozenForRelicAwards"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NHandImage.MethodName.GetFrozenPosition, new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHandImage.MethodName.DoFightMove, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Tween"), false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("move"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("duration"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NHandImage.MethodName.SetTextureToFightMove, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("move"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NHandImage.MethodName.SetPointingPosition, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("position"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NHandImage.MethodName.AnimateAway, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHandImage.MethodName.AnimateIn, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHandImage.MethodName.SetIsDown, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("isDown"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NHandImage.MethodName.SetSkipped, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHandImage.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NHandImage.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHandImage.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHandImage.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHandImage.MethodName.SetIsInFight) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetIsInFight(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHandImage.MethodName.SetFrozenForRelicAwards) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetFrozenForRelicAwards(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHandImage.MethodName.GetFrozenPosition) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      Vector2 frozenPosition = this.GetFrozenPosition();
      ret = VariantUtils.CreateFrom<Vector2>(ref frozenPosition);
      return true;
    }
    if (StringName.op_Equality(ref method, NHandImage.MethodName.DoFightMove) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      Tween tween = this.DoFightMove(VariantUtils.ConvertTo<RelicPickingFightMove>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<Tween>(ref tween);
      return true;
    }
    if (StringName.op_Equality(ref method, NHandImage.MethodName.SetTextureToFightMove) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetTextureToFightMove(VariantUtils.ConvertTo<RelicPickingFightMove>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHandImage.MethodName.SetPointingPosition) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetPointingPosition(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHandImage.MethodName.AnimateAway) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimateAway();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHandImage.MethodName.AnimateIn) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimateIn();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHandImage.MethodName.SetIsDown) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetIsDown(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHandImage.MethodName.SetSkipped) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SetSkipped();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NHandImage.MethodName._Process) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NHandImage.MethodName._Ready) || StringName.op_Equality(ref method, NHandImage.MethodName._EnterTree) || StringName.op_Equality(ref method, NHandImage.MethodName._ExitTree) || StringName.op_Equality(ref method, NHandImage.MethodName.SetIsInFight) || StringName.op_Equality(ref method, NHandImage.MethodName.SetFrozenForRelicAwards) || StringName.op_Equality(ref method, NHandImage.MethodName.GetFrozenPosition) || StringName.op_Equality(ref method, NHandImage.MethodName.DoFightMove) || StringName.op_Equality(ref method, NHandImage.MethodName.SetTextureToFightMove) || StringName.op_Equality(ref method, NHandImage.MethodName.SetPointingPosition) || StringName.op_Equality(ref method, NHandImage.MethodName.AnimateAway) || StringName.op_Equality(ref method, NHandImage.MethodName.AnimateIn) || StringName.op_Equality(ref method, NHandImage.MethodName.SetIsDown) || StringName.op_Equality(ref method, NHandImage.MethodName.SetSkipped) || StringName.op_Equality(ref method, NHandImage.MethodName._Process) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NHandImage.PropertyName.Index))
    {
      this.Index = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHandImage.PropertyName.IsDown))
    {
      this.IsDown = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHandImage.PropertyName.IsShown))
    {
      this.IsShown = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHandImage.PropertyName._grabMarker))
    {
      this._grabMarker = VariantUtils.ConvertTo<Marker2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHandImage.PropertyName._textureRect))
    {
      this._textureRect = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHandImage.PropertyName._currentVelocity))
    {
      this._currentVelocity = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHandImage.PropertyName._desiredPosition))
    {
      this._desiredPosition = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHandImage.PropertyName._downTween))
    {
      this._downTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHandImage.PropertyName._state))
    {
      this._state = VariantUtils.ConvertTo<NHandImage.State>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHandImage.PropertyName._isInFight))
    {
      this._isInFight = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHandImage.PropertyName._originalPosition))
    {
      this._originalPosition = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NHandImage.PropertyName._handAnimateInProgress))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._handAnimateInProgress = VariantUtils.ConvertTo<float>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NHandImage.PropertyName.Index))
    {
      ref godot_variant local = ref value;
      int index = this.Index;
      godot_variant from = VariantUtils.CreateFrom<int>(ref index);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NHandImage.PropertyName.IsDown))
    {
      ref godot_variant local = ref value;
      bool isDown = this.IsDown;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isDown);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NHandImage.PropertyName.IsShown))
    {
      ref godot_variant local = ref value;
      bool isShown = this.IsShown;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isShown);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NHandImage.PropertyName._grabMarker))
    {
      value = VariantUtils.CreateFrom<Marker2D>(ref this._grabMarker);
      return true;
    }
    if (StringName.op_Equality(ref name, NHandImage.PropertyName._textureRect))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._textureRect);
      return true;
    }
    if (StringName.op_Equality(ref name, NHandImage.PropertyName._currentVelocity))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._currentVelocity);
      return true;
    }
    if (StringName.op_Equality(ref name, NHandImage.PropertyName._desiredPosition))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._desiredPosition);
      return true;
    }
    if (StringName.op_Equality(ref name, NHandImage.PropertyName._downTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._downTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NHandImage.PropertyName._state))
    {
      value = VariantUtils.CreateFrom<NHandImage.State>(ref this._state);
      return true;
    }
    if (StringName.op_Equality(ref name, NHandImage.PropertyName._isInFight))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isInFight);
      return true;
    }
    if (StringName.op_Equality(ref name, NHandImage.PropertyName._originalPosition))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._originalPosition);
      return true;
    }
    if (!StringName.op_Equality(ref name, NHandImage.PropertyName._handAnimateInProgress))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<float>(ref this._handAnimateInProgress);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NHandImage.PropertyName._grabMarker, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NHandImage.PropertyName._textureRect, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NHandImage.PropertyName._currentVelocity, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NHandImage.PropertyName._desiredPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NHandImage.PropertyName._downTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NHandImage.PropertyName._state, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NHandImage.PropertyName._isInFight, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NHandImage.PropertyName._originalPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NHandImage.PropertyName._handAnimateInProgress, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NHandImage.PropertyName.Index, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NHandImage.PropertyName.IsDown, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NHandImage.PropertyName.IsShown, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo1 = info;
    StringName index1 = NHandImage.PropertyName.Index;
    int index2 = this.Index;
    Variant variant1 = Variant.From<int>(ref index2);
    serializationInfo1.AddProperty(index1, variant1);
    GodotSerializationInfo serializationInfo2 = info;
    StringName isDown1 = NHandImage.PropertyName.IsDown;
    bool isDown2 = this.IsDown;
    Variant variant2 = Variant.From<bool>(ref isDown2);
    serializationInfo2.AddProperty(isDown1, variant2);
    GodotSerializationInfo serializationInfo3 = info;
    StringName isShown1 = NHandImage.PropertyName.IsShown;
    bool isShown2 = this.IsShown;
    Variant variant3 = Variant.From<bool>(ref isShown2);
    serializationInfo3.AddProperty(isShown1, variant3);
    info.AddProperty(NHandImage.PropertyName._grabMarker, Variant.From<Marker2D>(ref this._grabMarker));
    info.AddProperty(NHandImage.PropertyName._textureRect, Variant.From<TextureRect>(ref this._textureRect));
    info.AddProperty(NHandImage.PropertyName._currentVelocity, Variant.From<Vector2>(ref this._currentVelocity));
    info.AddProperty(NHandImage.PropertyName._desiredPosition, Variant.From<Vector2>(ref this._desiredPosition));
    info.AddProperty(NHandImage.PropertyName._downTween, Variant.From<Tween>(ref this._downTween));
    info.AddProperty(NHandImage.PropertyName._state, Variant.From<NHandImage.State>(ref this._state));
    info.AddProperty(NHandImage.PropertyName._isInFight, Variant.From<bool>(ref this._isInFight));
    info.AddProperty(NHandImage.PropertyName._originalPosition, Variant.From<Vector2>(ref this._originalPosition));
    info.AddProperty(NHandImage.PropertyName._handAnimateInProgress, Variant.From<float>(ref this._handAnimateInProgress));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NHandImage.PropertyName.Index, ref variant1))
      this.Index = ((Variant) ref variant1).As<int>();
    Variant variant2;
    if (info.TryGetProperty(NHandImage.PropertyName.IsDown, ref variant2))
      this.IsDown = ((Variant) ref variant2).As<bool>();
    Variant variant3;
    if (info.TryGetProperty(NHandImage.PropertyName.IsShown, ref variant3))
      this.IsShown = ((Variant) ref variant3).As<bool>();
    Variant variant4;
    if (info.TryGetProperty(NHandImage.PropertyName._grabMarker, ref variant4))
      this._grabMarker = ((Variant) ref variant4).As<Marker2D>();
    Variant variant5;
    if (info.TryGetProperty(NHandImage.PropertyName._textureRect, ref variant5))
      this._textureRect = ((Variant) ref variant5).As<TextureRect>();
    Variant variant6;
    if (info.TryGetProperty(NHandImage.PropertyName._currentVelocity, ref variant6))
      this._currentVelocity = ((Variant) ref variant6).As<Vector2>();
    Variant variant7;
    if (info.TryGetProperty(NHandImage.PropertyName._desiredPosition, ref variant7))
      this._desiredPosition = ((Variant) ref variant7).As<Vector2>();
    Variant variant8;
    if (info.TryGetProperty(NHandImage.PropertyName._downTween, ref variant8))
      this._downTween = ((Variant) ref variant8).As<Tween>();
    Variant variant9;
    if (info.TryGetProperty(NHandImage.PropertyName._state, ref variant9))
      this._state = ((Variant) ref variant9).As<NHandImage.State>();
    Variant variant10;
    if (info.TryGetProperty(NHandImage.PropertyName._isInFight, ref variant10))
      this._isInFight = ((Variant) ref variant10).As<bool>();
    Variant variant11;
    if (info.TryGetProperty(NHandImage.PropertyName._originalPosition, ref variant11))
      this._originalPosition = ((Variant) ref variant11).As<Vector2>();
    Variant variant12;
    if (!info.TryGetProperty(NHandImage.PropertyName._handAnimateInProgress, ref variant12))
      return;
    this._handAnimateInProgress = ((Variant) ref variant12).As<float>();
  }

  private enum State
  {
    None,
    Frozen,
    GrabbingRelic,
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName SetIsInFight = StringName.op_Implicit(nameof (SetIsInFight));
    public static readonly StringName SetFrozenForRelicAwards = StringName.op_Implicit(nameof (SetFrozenForRelicAwards));
    public static readonly StringName GetFrozenPosition = StringName.op_Implicit(nameof (GetFrozenPosition));
    public static readonly StringName DoFightMove = StringName.op_Implicit(nameof (DoFightMove));
    public static readonly StringName SetTextureToFightMove = StringName.op_Implicit(nameof (SetTextureToFightMove));
    public static readonly StringName SetPointingPosition = StringName.op_Implicit(nameof (SetPointingPosition));
    public static readonly StringName AnimateAway = StringName.op_Implicit(nameof (AnimateAway));
    public static readonly StringName AnimateIn = StringName.op_Implicit(nameof (AnimateIn));
    public static readonly StringName SetIsDown = StringName.op_Implicit(nameof (SetIsDown));
    public static readonly StringName SetSkipped = StringName.op_Implicit(nameof (SetSkipped));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName Index = StringName.op_Implicit(nameof (Index));
    public static readonly StringName IsDown = StringName.op_Implicit(nameof (IsDown));
    public static readonly StringName IsShown = StringName.op_Implicit(nameof (IsShown));
    public static readonly StringName _grabMarker = StringName.op_Implicit(nameof (_grabMarker));
    public static readonly StringName _textureRect = StringName.op_Implicit(nameof (_textureRect));
    public static readonly StringName _currentVelocity = StringName.op_Implicit(nameof (_currentVelocity));
    public static readonly StringName _desiredPosition = StringName.op_Implicit(nameof (_desiredPosition));
    public static readonly StringName _downTween = StringName.op_Implicit(nameof (_downTween));
    public static readonly StringName _state = StringName.op_Implicit(nameof (_state));
    public static readonly StringName _isInFight = StringName.op_Implicit(nameof (_isInFight));
    public static readonly StringName _originalPosition = StringName.op_Implicit(nameof (_originalPosition));
    public static readonly StringName _handAnimateInProgress = StringName.op_Implicit(nameof (_handAnimateInProgress));
  }

  public class SignalName : Control.SignalName
  {
  }
}
