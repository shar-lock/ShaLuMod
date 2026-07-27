// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NCardFlyVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NCardFlyVfx.cs")]
public class NCardFlyVfx : Node2D
{
  private NCard _card;
  private string _trailPath;
  private NCardTrailVfx? _vfx;
  private Tween? _fadeOutTween;
  private bool _vfxFading;
  private bool _isAddingToPile;
  private Vector2 _startPos;
  private Vector2 _endPos;
  private float _controlPointOffset;
  private float _duration;
  private float _speed;
  private float _accel;
  private float _arcDir;
  private readonly CancellationTokenSource _cancelToken = new CancellationTokenSource();
  private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/vfx_card_fly");

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NCardFlyVfx._scenePath);
    }
  }

  public TaskCompletionSource? SwooshAwayCompletion { get; private set; }

  public static NCardFlyVfx? Create(
    NCard card,
    PileType pileType,
    bool isAddingToPile,
    string trailPath)
  {
    if (TestMode.IsOn)
      return (NCardFlyVfx) null;
    NCardFlyVfx ncardFlyVfx = PreloadManager.Cache.GetScene(NCardFlyVfx._scenePath).Instantiate<NCardFlyVfx>((PackedScene.GenEditState) 0L);
    ncardFlyVfx._startPos = card.GlobalPosition;
    ncardFlyVfx._endPos = pileType.GetTargetPosition(card);
    ncardFlyVfx._card = card;
    ncardFlyVfx._isAddingToPile = isAddingToPile;
    ncardFlyVfx._trailPath = trailPath;
    return ncardFlyVfx;
  }

  public static NCardFlyVfx? Create(NCard card, Creature target, string trailPath)
  {
    if (TestMode.IsOn)
      return (NCardFlyVfx) null;
    if (NCombatRoom.Instance == null)
      return (NCardFlyVfx) null;
    NCreature creatureNode = NCombatRoom.Instance.GetCreatureNode(target);
    if (creatureNode == null)
      return (NCardFlyVfx) null;
    NCardFlyVfx ncardFlyVfx = PreloadManager.Cache.GetScene(NCardFlyVfx._scenePath).Instantiate<NCardFlyVfx>((PackedScene.GenEditState) 0L);
    ncardFlyVfx._startPos = card.GlobalPosition;
    ncardFlyVfx._endPos = creatureNode.VfxSpawnPosition;
    ncardFlyVfx._card = card;
    ncardFlyVfx._isAddingToPile = false;
    ncardFlyVfx._trailPath = trailPath;
    return ncardFlyVfx;
  }

  public override void _Ready()
  {
    this._vfx = NCardTrailVfx.Create((Control) this._card, this._trailPath);
    if (this._vfx != null)
      ((Node) this).GetParent().AddChildSafely((Node) this._vfx);
    this._controlPointOffset = Rng.Chaotic.NextFloat(100f, 400f);
    this._speed = Rng.Chaotic.NextFloat(1.1f, 1.25f);
    this._accel = Rng.Chaotic.NextFloat(2f, 2.5f);
    double y = (double) this._endPos.Y;
    Rect2 viewportRect = ((CanvasItem) this).GetViewportRect();
    double num = (double) ((Rect2) ref viewportRect).Size.Y * 0.5;
    this._arcDir = y < num ? -500f : 500f + this._controlPointOffset;
    this._duration = Rng.Chaotic.NextFloat(1f, 1.75f);
    ((GodotObject) this._card).Connect(Node.SignalName.TreeExited, Callable.From(new Action(this.OnCardExitedTree)), 0U);
    if (NCombatUi.IsDebugHidingPlayContainer)
    {
      ((CanvasItem) this._card).Modulate = Colors.Transparent;
      ((CanvasItem) this._card).Visible = false;
      ((CanvasItem) this).Visible = false;
    }
    TaskHelper.RunSafely(this.PlayAnim());
  }

  public override void _ExitTree()
  {
    this._fadeOutTween?.Kill();
    this._cancelToken.Cancel();
  }

  private void OnCardExitedTree()
  {
    try
    {
      NCardTrailVfx vfx = this._vfx;
      if (vfx != null)
        ((Node) vfx).QueueFreeSafely();
    }
    catch (ObjectDisposedException ex)
    {
    }
    this.SwooshAwayCompletion?.TrySetResult();
    ((Node) this).QueueFreeSafely();
  }

  private async Task PlayAnim()
  {
    CardPile pile = this._card.Model.Pile;
    if (pile != null)
      SfxCmd.PlayCardSwooshSfx(pile);
    this.SwooshAwayCompletion = new TaskCompletionSource();
    float time = 0.0f;
    while ((double) time / (double) this._duration <= 1.0)
    {
      double num1 = (double) await ((Node) this).AwaitProcessFrame();
      if (this._cancelToken.IsCancellationRequested)
      {
        this.SwooshAwayCompletion?.SetResult();
        return;
      }
      float processDeltaTime = (float) ((Node) this).GetProcessDeltaTime();
      time += this._speed * processDeltaTime;
      this._speed += this._accel * processDeltaTime;
      Vector2 c0 = Vector2.op_Addition(this._startPos, Vector2.op_Multiply(Vector2.op_Subtraction(this._endPos, this._startPos), 0.5f));
      c0.Y -= this._arcDir;
      Vector2 vector2_1 = MathHelper.BezierCurve(this._startPos, this._endPos, c0, (time + 0.05f) / this._duration);
      this._card.GlobalPosition = MathHelper.BezierCurve(this._startPos, this._endPos, c0, time / this._duration);
      Vector2 vector2_2 = Vector2.op_Subtraction(vector2_1, this._card.GlobalPosition);
      float num2 = ((Vector2) ref vector2_2).Angle() + 1.57079637f;
      switch (((Node) this._card).GetParent())
      {
        case Control control:
          num2 -= control.Rotation;
          break;
        case Node2D node2D:
          num2 -= node2D.Rotation;
          break;
      }
      this._card.Rotation = Mathf.LerpAngle(this._card.Rotation, num2, processDeltaTime * 12f);
      Control body = this._card.Body;
      Color white = Colors.White;
      Color color = ((Color) ref white).Lerp(Colors.Black, Mathf.Clamp(time * 3f / this._duration, 0.0f, 1f));
      ((CanvasItem) body).Modulate = color;
      this._card.Body.Scale = Vector2.op_Multiply(Vector2.One, Mathf.Lerp(1f, 0.1f, Mathf.Clamp(time * 3f / this._duration, 0.0f, 1f)));
    }
    this._card.GlobalPosition = this._endPos;
    if (this._isAddingToPile)
      this._card.Model.Pile?.InvokeCardAddFinished();
    time = 0.0f;
    while ((double) time / (double) this._duration <= 1.0)
    {
      double num = (double) await ((Node) this).AwaitProcessFrame();
      if (this._cancelToken.IsCancellationRequested)
      {
        this.SwooshAwayCompletion?.SetResult();
        return;
      }
      float processDeltaTime = (float) ((Node) this).GetProcessDeltaTime();
      time += this._speed * processDeltaTime;
      if ((double) time / (double) this._duration > 0.25 && !this._vfxFading)
      {
        if (this._vfx != null)
          TaskHelper.RunSafely(this._vfx.FadeOut());
        this._vfxFading = true;
      }
      this._card.Body.Scale = Vector2.op_Multiply(Vector2.One, Mathf.Max(Mathf.Lerp(0.1f, -0.15f, time / this._duration), 0.0f));
    }
    this.SwooshAwayCompletion?.SetResult();
    ((Node) this._card).QueueFreeSafely();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NCardFlyVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node2D"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("card"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("pileType"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("isAddingToPile"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("trailPath"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCardFlyVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardFlyVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardFlyVfx.MethodName.OnCardExitedTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCardFlyVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 4)
    {
      NCardFlyVfx ncardFlyVfx = NCardFlyVfx.Create(VariantUtils.ConvertTo<NCard>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<PileType>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[3]));
      ret = VariantUtils.CreateFrom<NCardFlyVfx>(ref ncardFlyVfx);
      return true;
    }
    if (StringName.op_Equality(ref method, NCardFlyVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardFlyVfx.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCardFlyVfx.MethodName.OnCardExitedTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnCardExitedTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCardFlyVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 4)
    {
      NCardFlyVfx ncardFlyVfx = NCardFlyVfx.Create(VariantUtils.ConvertTo<NCard>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<PileType>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[3]));
      ret = VariantUtils.CreateFrom<NCardFlyVfx>(ref ncardFlyVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCardFlyVfx.MethodName.Create) || StringName.op_Equality(ref method, NCardFlyVfx.MethodName._Ready) || StringName.op_Equality(ref method, NCardFlyVfx.MethodName._ExitTree) || StringName.op_Equality(ref method, NCardFlyVfx.MethodName.OnCardExitedTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardFlyVfx.PropertyName._card))
    {
      this._card = VariantUtils.ConvertTo<NCard>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyVfx.PropertyName._trailPath))
    {
      this._trailPath = VariantUtils.ConvertTo<string>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyVfx.PropertyName._vfx))
    {
      this._vfx = VariantUtils.ConvertTo<NCardTrailVfx>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyVfx.PropertyName._fadeOutTween))
    {
      this._fadeOutTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyVfx.PropertyName._vfxFading))
    {
      this._vfxFading = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyVfx.PropertyName._isAddingToPile))
    {
      this._isAddingToPile = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyVfx.PropertyName._startPos))
    {
      this._startPos = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyVfx.PropertyName._endPos))
    {
      this._endPos = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyVfx.PropertyName._controlPointOffset))
    {
      this._controlPointOffset = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyVfx.PropertyName._duration))
    {
      this._duration = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyVfx.PropertyName._speed))
    {
      this._speed = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyVfx.PropertyName._accel))
    {
      this._accel = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardFlyVfx.PropertyName._arcDir))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._arcDir = VariantUtils.ConvertTo<float>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardFlyVfx.PropertyName._card))
    {
      value = VariantUtils.CreateFrom<NCard>(ref this._card);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyVfx.PropertyName._trailPath))
    {
      value = VariantUtils.CreateFrom<string>(ref this._trailPath);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyVfx.PropertyName._vfx))
    {
      value = VariantUtils.CreateFrom<NCardTrailVfx>(ref this._vfx);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyVfx.PropertyName._fadeOutTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._fadeOutTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyVfx.PropertyName._vfxFading))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._vfxFading);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyVfx.PropertyName._isAddingToPile))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isAddingToPile);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyVfx.PropertyName._startPos))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._startPos);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyVfx.PropertyName._endPos))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._endPos);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyVfx.PropertyName._controlPointOffset))
    {
      value = VariantUtils.CreateFrom<float>(ref this._controlPointOffset);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyVfx.PropertyName._duration))
    {
      value = VariantUtils.CreateFrom<float>(ref this._duration);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyVfx.PropertyName._speed))
    {
      value = VariantUtils.CreateFrom<float>(ref this._speed);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyVfx.PropertyName._accel))
    {
      value = VariantUtils.CreateFrom<float>(ref this._accel);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardFlyVfx.PropertyName._arcDir))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<float>(ref this._arcDir);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCardFlyVfx.PropertyName._card, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, NCardFlyVfx.PropertyName._trailPath, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardFlyVfx.PropertyName._vfx, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardFlyVfx.PropertyName._fadeOutTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCardFlyVfx.PropertyName._vfxFading, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCardFlyVfx.PropertyName._isAddingToPile, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NCardFlyVfx.PropertyName._startPos, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NCardFlyVfx.PropertyName._endPos, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NCardFlyVfx.PropertyName._controlPointOffset, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NCardFlyVfx.PropertyName._duration, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NCardFlyVfx.PropertyName._speed, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NCardFlyVfx.PropertyName._accel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NCardFlyVfx.PropertyName._arcDir, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NCardFlyVfx.PropertyName._card, Variant.From<NCard>(ref this._card));
    info.AddProperty(NCardFlyVfx.PropertyName._trailPath, Variant.From<string>(ref this._trailPath));
    info.AddProperty(NCardFlyVfx.PropertyName._vfx, Variant.From<NCardTrailVfx>(ref this._vfx));
    info.AddProperty(NCardFlyVfx.PropertyName._fadeOutTween, Variant.From<Tween>(ref this._fadeOutTween));
    info.AddProperty(NCardFlyVfx.PropertyName._vfxFading, Variant.From<bool>(ref this._vfxFading));
    info.AddProperty(NCardFlyVfx.PropertyName._isAddingToPile, Variant.From<bool>(ref this._isAddingToPile));
    info.AddProperty(NCardFlyVfx.PropertyName._startPos, Variant.From<Vector2>(ref this._startPos));
    info.AddProperty(NCardFlyVfx.PropertyName._endPos, Variant.From<Vector2>(ref this._endPos));
    info.AddProperty(NCardFlyVfx.PropertyName._controlPointOffset, Variant.From<float>(ref this._controlPointOffset));
    info.AddProperty(NCardFlyVfx.PropertyName._duration, Variant.From<float>(ref this._duration));
    info.AddProperty(NCardFlyVfx.PropertyName._speed, Variant.From<float>(ref this._speed));
    info.AddProperty(NCardFlyVfx.PropertyName._accel, Variant.From<float>(ref this._accel));
    info.AddProperty(NCardFlyVfx.PropertyName._arcDir, Variant.From<float>(ref this._arcDir));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCardFlyVfx.PropertyName._card, ref variant1))
      this._card = ((Variant) ref variant1).As<NCard>();
    Variant variant2;
    if (info.TryGetProperty(NCardFlyVfx.PropertyName._trailPath, ref variant2))
      this._trailPath = ((Variant) ref variant2).As<string>();
    Variant variant3;
    if (info.TryGetProperty(NCardFlyVfx.PropertyName._vfx, ref variant3))
      this._vfx = ((Variant) ref variant3).As<NCardTrailVfx>();
    Variant variant4;
    if (info.TryGetProperty(NCardFlyVfx.PropertyName._fadeOutTween, ref variant4))
      this._fadeOutTween = ((Variant) ref variant4).As<Tween>();
    Variant variant5;
    if (info.TryGetProperty(NCardFlyVfx.PropertyName._vfxFading, ref variant5))
      this._vfxFading = ((Variant) ref variant5).As<bool>();
    Variant variant6;
    if (info.TryGetProperty(NCardFlyVfx.PropertyName._isAddingToPile, ref variant6))
      this._isAddingToPile = ((Variant) ref variant6).As<bool>();
    Variant variant7;
    if (info.TryGetProperty(NCardFlyVfx.PropertyName._startPos, ref variant7))
      this._startPos = ((Variant) ref variant7).As<Vector2>();
    Variant variant8;
    if (info.TryGetProperty(NCardFlyVfx.PropertyName._endPos, ref variant8))
      this._endPos = ((Variant) ref variant8).As<Vector2>();
    Variant variant9;
    if (info.TryGetProperty(NCardFlyVfx.PropertyName._controlPointOffset, ref variant9))
      this._controlPointOffset = ((Variant) ref variant9).As<float>();
    Variant variant10;
    if (info.TryGetProperty(NCardFlyVfx.PropertyName._duration, ref variant10))
      this._duration = ((Variant) ref variant10).As<float>();
    Variant variant11;
    if (info.TryGetProperty(NCardFlyVfx.PropertyName._speed, ref variant11))
      this._speed = ((Variant) ref variant11).As<float>();
    Variant variant12;
    if (info.TryGetProperty(NCardFlyVfx.PropertyName._accel, ref variant12))
      this._accel = ((Variant) ref variant12).As<float>();
    Variant variant13;
    if (!info.TryGetProperty(NCardFlyVfx.PropertyName._arcDir, ref variant13))
      return;
    this._arcDir = ((Variant) ref variant13).As<float>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName OnCardExitedTree = StringName.op_Implicit(nameof (OnCardExitedTree));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _card = StringName.op_Implicit(nameof (_card));
    public static readonly StringName _trailPath = StringName.op_Implicit(nameof (_trailPath));
    public static readonly StringName _vfx = StringName.op_Implicit(nameof (_vfx));
    public static readonly StringName _fadeOutTween = StringName.op_Implicit(nameof (_fadeOutTween));
    public static readonly StringName _vfxFading = StringName.op_Implicit(nameof (_vfxFading));
    public static readonly StringName _isAddingToPile = StringName.op_Implicit(nameof (_isAddingToPile));
    public static readonly StringName _startPos = StringName.op_Implicit(nameof (_startPos));
    public static readonly StringName _endPos = StringName.op_Implicit(nameof (_endPos));
    public static readonly StringName _controlPointOffset = StringName.op_Implicit(nameof (_controlPointOffset));
    public static readonly StringName _duration = StringName.op_Implicit(nameof (_duration));
    public static readonly StringName _speed = StringName.op_Implicit(nameof (_speed));
    public static readonly StringName _accel = StringName.op_Implicit(nameof (_accel));
    public static readonly StringName _arcDir = StringName.op_Implicit(nameof (_arcDir));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
