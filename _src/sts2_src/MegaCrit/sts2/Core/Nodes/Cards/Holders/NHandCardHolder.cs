// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Cards.Holders.NHandCardHolder
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Cards.Holders;

[ScriptPath("res://src/Core/Nodes/Cards/Holders/NHandCardHolder.cs")]
public class NHandCardHolder : NCardHolder
{
  private Control _flash;
  private Tween? _flashTween;
  private MegaLabel _handIndexLabel;
  private const float _rotateSpeed = 10f;
  private const float _angleSnapThreshold = 0.1f;
  private const float _scaleSpeed = 8f;
  private const float _scaleSnapThreshold = 0.002f;
  private const float _moveSpeed = 7f;
  private const float _positionSnapThreshold = 1f;
  private const float _reenableHitboxThreshold = 200f;
  private Vector2 _targetPosition;
  private float _targetAngle;
  private Vector2 _targetScale;
  private CancellationTokenSource? _angleCancelToken;
  private CancellationTokenSource? _positionCancelToken;
  private CancellationTokenSource? _scaleCancelToken;
  private NPlayerHand _hand;
  private 
  #nullable disable
  NHandCardHolder.HolderFocusedEventHandler backing_HolderFocused;
  private NHandCardHolder.HolderUnfocusedEventHandler backing_HolderUnfocused;
  private NHandCardHolder.HolderMouseClickedEventHandler backing_HolderMouseClicked;

  public bool InSelectMode { get; set; }

  public Vector2 TargetPosition => this._targetPosition;

  public float TargetAngle => this._targetAngle;

  private static 
  #nullable enable
  string ScenePath => SceneHelper.GetScenePath("cards/holders/hand_card_holder");

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NHandCardHolder.ScenePath);
    }
  }

  public static NHandCardHolder Create(NCard card, NPlayerHand hand)
  {
    NHandCardHolder nhandCardHolder = PreloadManager.Cache.GetScene(NHandCardHolder.ScenePath).Instantiate<NHandCardHolder>((PackedScene.GenEditState) 0L);
    ((Node) nhandCardHolder).Name = StringName.op_Implicit($"{((object) nhandCardHolder).GetType().Name}-{card.Model.Id}");
    nhandCardHolder.SetCard(card);
    nhandCardHolder._hand = hand;
    return nhandCardHolder;
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._flash = ((Node) this).GetNode<Control>(NodePath.op_Implicit("Flash"));
    ((CanvasItem) this._flash).Modulate = new Color(((CanvasItem) this._flash).Modulate.R, ((CanvasItem) this._flash).Modulate.G, ((CanvasItem) this._flash).Modulate.B, 0.0f);
    this._handIndexLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%HandIndex"));
    this.UpdateCard();
    this.Hitbox.SetEnabled(false);
  }

  public override void _ExitTree()
  {
    ((Node) this)._ExitTree();
    this.UnsubscribeFromEvents(this.CardNode?.Model);
    this.StopAnimations();
  }

  public override void Clear()
  {
    this.UnsubscribeFromEvents(this.CardNode?.Model);
    base.Clear();
    this.StopAnimations();
  }

  protected override void OnFocus()
  {
    ((GodotObject) this).EmitSignal(NHandCardHolder.SignalName.HolderFocused, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) this)
    });
    base.OnFocus();
  }

  protected override void OnUnfocus()
  {
    ((GodotObject) this).EmitSignal(NHandCardHolder.SignalName.HolderUnfocused, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) this)
    });
    base.OnUnfocus();
  }

  protected override void OnMousePressed(InputEvent inputEvent)
  {
    if (!(inputEvent is InputEventMouseButton eventMouseButton) || eventMouseButton.ButtonIndex != 1L || !this._isClickable)
      return;
    SfxCmd.Play("event:/sfx/ui/clicks/ui_click");
    ((GodotObject) this).EmitSignal(NHandCardHolder.SignalName.HolderMouseClicked, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) this)
    });
  }

  protected override void OnMouseReleased(InputEvent inputEvent)
  {
  }

  protected override void DoCardHoverEffects(bool isHovered)
  {
    ((CanvasItem) this).ZIndex = isHovered ? 1 : 0;
    if (isHovered)
      this.CreateHoverTips();
    else
      this.ClearHoverTips();
  }

  public void SetIndexLabel(int i)
  {
    this._handIndexLabel.SetTextAutoSize(i.ToString());
    ((CanvasItem) this._handIndexLabel).Visible = i > 0 && SaveManager.Instance.PrefsSave.ShowCardIndices;
  }

  public void SetTargetAngle(float angle)
  {
    this._targetAngle = angle;
    this._angleCancelToken?.Cancel();
    this._angleCancelToken = new CancellationTokenSource();
    TaskHelper.RunSafely(this.AnimAngle(this._angleCancelToken));
  }

  public void SetTargetPosition(Vector2 position)
  {
    this._targetPosition = position;
    this._positionCancelToken?.Cancel();
    this._positionCancelToken = new CancellationTokenSource();
    TaskHelper.RunSafely(this.AnimPosition(this._positionCancelToken));
  }

  public void SetTargetScale(Vector2 scale)
  {
    this._targetScale = scale;
    this._scaleCancelToken?.Cancel();
    this._scaleCancelToken = new CancellationTokenSource();
    TaskHelper.RunSafely(this.AnimScale(this._scaleCancelToken));
  }

  public void SetAngleInstantly(float setAngle)
  {
    this._angleCancelToken?.Cancel();
    this.RotationDegrees = setAngle;
  }

  public void SetScaleInstantly(Vector2 setScale)
  {
    this._scaleCancelToken?.Cancel();
    this.Scale = setScale;
  }

  private void StopAnimations()
  {
    this._angleCancelToken?.Cancel();
    this._positionCancelToken?.Cancel();
    this._scaleCancelToken?.Cancel();
  }

  private async Task AnimAngle(CancellationTokenSource cancelToken)
  {
    while (!cancelToken.IsCancellationRequested)
    {
      this.RotationDegrees = Mathf.Lerp(this.RotationDegrees, this._targetAngle, (float) ((Node) this).GetProcessDeltaTime() * 10f);
      if ((double) Mathf.Abs(this.RotationDegrees - this._targetAngle) < 0.10000000149011612)
      {
        this.RotationDegrees = this._targetAngle;
        break;
      }
      await ((Node) this).AwaitProcessFrameNonThrowing(cancelToken);
    }
  }

  private async Task AnimScale(CancellationTokenSource cancelToken)
  {
    while (!cancelToken.IsCancellationRequested)
    {
      Vector2 scale = this.Scale;
      this.Scale = ((Vector2) ref scale).Lerp(this._targetScale, (float) ((Node) this).GetProcessDeltaTime() * 8f);
      if ((double) Mathf.Abs(this._targetScale.X - this.Scale.X) < 1.0 / 500.0)
      {
        this.Scale = this._targetScale;
        break;
      }
      await ((Node) this).AwaitProcessFrameNonThrowing(cancelToken);
    }
  }

  private async Task AnimPosition(CancellationTokenSource cancelToken)
  {
    while (!cancelToken.IsCancellationRequested)
    {
      Vector2 position1 = this.Position;
      this.Position = ((Vector2) ref position1).Lerp(this._targetPosition, (float) ((Node) this).GetProcessDeltaTime() * 7f);
      float num = Mathf.Abs(this.Position.X - this._targetPosition.X);
      if (!this.Hitbox.IsEnabled && (double) num < 200.0)
        this.Hitbox.SetEnabled(true);
      Vector2 position2 = this.Position;
      if ((double) ((Vector2) ref position2).DistanceSquaredTo(this._targetPosition) < 1.0)
      {
        this.Position = this._targetPosition;
        return;
      }
      await ((Node) this).AwaitProcessFrameNonThrowing(cancelToken);
    }
    if (!((Node) this).IsValid() || this.Hitbox.IsEnabled)
      return;
    Vector2 position = this.Position;
    if ((double) ((Vector2) ref position).DistanceSquaredTo(this._targetPosition) >= 200.0)
      return;
    this.Hitbox.SetEnabled(true);
  }

  protected override void SetCard(NCard node)
  {
    if (this.CardNode != null)
      this.CardNode.ModelChanged -= new Action<CardModel>(this.OnModelChanged);
    this.UnsubscribeFromEvents(this.CardNode?.Model);
    base.SetCard(node);
    this.UpdateCard();
    this.SubscribeToEvents(this.CardNode?.Model);
    if (this.CardNode != null)
      this.CardNode.ModelChanged += new Action<CardModel>(this.OnModelChanged);
    if (!Vector2.op_Inequality(node.Scale, Vector2.One))
      return;
    ((Node) node).CreateTween().TweenProperty((GodotObject) node, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.25);
  }

  public void UpdateCard()
  {
    if (!((Node) this).IsNodeReady() || this.CardNode == null)
      return;
    this.CardNode.UpdateVisuals(PileType.Hand, CardPreviewMode.Normal);
    if (!CombatManager.Instance.IsInProgress)
      return;
    if (this.CardNode.Model.CanPlay() || this.ShouldGlowRed || this.ShouldGlowGold)
    {
      this.CardNode.CardHighlight.AnimShow();
      ((CanvasItem) this.CardNode.CardHighlight).Modulate = NCardHighlight.playableColor;
      if (this.ShouldGlowRed)
      {
        ((CanvasItem) this.CardNode.CardHighlight).Modulate = NCardHighlight.red;
      }
      else
      {
        if (!this.ShouldGlowGold)
          return;
        ((CanvasItem) this.CardNode.CardHighlight).Modulate = NCardHighlight.gold;
      }
    }
    else
      this.CardNode.CardHighlight.AnimHide();
  }

  public void BeginDrag()
  {
    this.SetAngleInstantly(0.0f);
    this.SetScaleInstantly(this.HoverScale);
  }

  public void CancelDrag()
  {
    ((CanvasItem) this).ZIndex = 0;
    this.SetAngleInstantly(0.0f);
    this.SetScaleInstantly(Vector2.One);
  }

  public void SetDefaultTargets()
  {
    ((CanvasItem) this).ZIndex = 0;
    IReadOnlyList<NHandCardHolder> activeHolders = this._hand.ActiveHolders;
    int cardIndex = activeHolders.IndexOf<NHandCardHolder>(this);
    int count = activeHolders.Count;
    if (cardIndex < 0)
      return;
    this.SetTargetPosition(HandPosHelper.GetPosition(count, cardIndex));
    this.SetTargetAngle(HandPosHelper.GetAngle(count, cardIndex));
    this.SetTargetScale(HandPosHelper.GetScale(count));
  }

  public void Flash()
  {
    if (!GodotObject.IsInstanceValid((GodotObject) this._flash))
      return;
    this._flash.Scale = Vector2.One;
    ((CanvasItem) this._flash).Modulate = NCardHighlight.playableColor;
    if (this.ShouldGlowGold)
      ((CanvasItem) this._flash).Modulate = NCardHighlight.gold;
    else if (this.ShouldGlowRed)
      ((CanvasItem) this._flash).Modulate = NCardHighlight.red;
    this._flashTween?.Kill();
    this._flashTween = ((Node) this).CreateTween();
    this._flashTween.TweenProperty((GodotObject) this._flash, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.6), 0.15);
    this._flashTween.TweenProperty((GodotObject) this._flash, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0), 0.3);
  }

  private bool ShouldGlowGold
  {
    get
    {
      CardModel model = this.CardNode?.Model;
      if (model == null)
        return false;
      if (this._hand.SelectModeGoldGlowOverride != null)
        return this._hand.SelectModeGoldGlowOverride(model);
      if (!model.CanPlay() || model == null || !model.ShouldGlowGold)
        return false;
      Player owner = model.Owner;
      if (owner != null)
      {
        PlayerCombatState playerCombatState = owner.PlayerCombatState;
        if (playerCombatState != null)
          return playerCombatState.Phase == PlayerTurnPhase.Play;
      }
      return false;
    }
  }

  private bool ShouldGlowRed
  {
    get
    {
      CardModel model = this.CardNode?.Model;
      if (model != null && model.ShouldGlowRed)
      {
        Player owner = model.Owner;
        if (owner != null)
        {
          PlayerCombatState playerCombatState = owner.PlayerCombatState;
          if (playerCombatState != null)
            return playerCombatState.Phase == PlayerTurnPhase.Play;
        }
      }
      return false;
    }
  }

  private void SubscribeToEvents(CardModel? card)
  {
    if (card == null || !((Node) this).IsInsideTree())
      return;
    card.Upgraded += new Action(this.Flash);
    card.KeywordsChanged += new Action(this.Flash);
    card.ReplayCountChanged += new Action(this.Flash);
    card.AfflictionChanged += new Action(this.Flash);
    card.EnergyCostChanged += new Action(this.Flash);
    card.StarCostChanged += new Action(this.Flash);
  }

  private void UnsubscribeFromEvents(CardModel? card)
  {
    if (card == null)
      return;
    card.Upgraded -= new Action(this.Flash);
    card.KeywordsChanged -= new Action(this.Flash);
    card.ReplayCountChanged -= new Action(this.Flash);
    card.AfflictionChanged -= new Action(this.Flash);
    card.EnergyCostChanged -= new Action(this.Flash);
    card.StarCostChanged -= new Action(this.Flash);
  }

  private void OnModelChanged(CardModel? oldModel)
  {
    this.UnsubscribeFromEvents(oldModel);
    this.SubscribeToEvents(this.CardNode?.Model);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(22)
    {
      new MethodInfo(NHandCardHolder.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("card"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("hand"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NHandCardHolder.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHandCardHolder.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHandCardHolder.MethodName.Clear, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHandCardHolder.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHandCardHolder.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHandCardHolder.MethodName.OnMousePressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NHandCardHolder.MethodName.OnMouseReleased, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NHandCardHolder.MethodName.DoCardHoverEffects, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("isHovered"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NHandCardHolder.MethodName.SetIndexLabel, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("i"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NHandCardHolder.MethodName.SetTargetAngle, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("angle"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NHandCardHolder.MethodName.SetTargetPosition, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("position"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NHandCardHolder.MethodName.SetTargetScale, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("scale"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NHandCardHolder.MethodName.SetAngleInstantly, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("setAngle"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NHandCardHolder.MethodName.SetScaleInstantly, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("setScale"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NHandCardHolder.MethodName.StopAnimations, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHandCardHolder.MethodName.SetCard, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("node"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NHandCardHolder.MethodName.UpdateCard, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHandCardHolder.MethodName.BeginDrag, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHandCardHolder.MethodName.CancelDrag, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHandCardHolder.MethodName.SetDefaultTargets, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHandCardHolder.MethodName.Flash, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NHandCardHolder.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      NHandCardHolder nhandCardHolder = NHandCardHolder.Create(VariantUtils.ConvertTo<NCard>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<NPlayerHand>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<NHandCardHolder>(ref nhandCardHolder);
      return true;
    }
    if (StringName.op_Equality(ref method, NHandCardHolder.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHandCardHolder.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHandCardHolder.MethodName.Clear) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Clear();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHandCardHolder.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHandCardHolder.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHandCardHolder.MethodName.OnMousePressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnMousePressed(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHandCardHolder.MethodName.OnMouseReleased) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnMouseReleased(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHandCardHolder.MethodName.DoCardHoverEffects) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.DoCardHoverEffects(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHandCardHolder.MethodName.SetIndexLabel) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetIndexLabel(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHandCardHolder.MethodName.SetTargetAngle) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetTargetAngle(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHandCardHolder.MethodName.SetTargetPosition) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetTargetPosition(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHandCardHolder.MethodName.SetTargetScale) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetTargetScale(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHandCardHolder.MethodName.SetAngleInstantly) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetAngleInstantly(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHandCardHolder.MethodName.SetScaleInstantly) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetScaleInstantly(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHandCardHolder.MethodName.StopAnimations) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StopAnimations();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHandCardHolder.MethodName.SetCard) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetCard(VariantUtils.ConvertTo<NCard>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHandCardHolder.MethodName.UpdateCard) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateCard();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHandCardHolder.MethodName.BeginDrag) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.BeginDrag();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHandCardHolder.MethodName.CancelDrag) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.CancelDrag();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHandCardHolder.MethodName.SetDefaultTargets) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SetDefaultTargets();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NHandCardHolder.MethodName.Flash) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.Flash();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NHandCardHolder.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      NHandCardHolder nhandCardHolder = NHandCardHolder.Create(VariantUtils.ConvertTo<NCard>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<NPlayerHand>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<NHandCardHolder>(ref nhandCardHolder);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NHandCardHolder.MethodName.Create) || StringName.op_Equality(ref method, NHandCardHolder.MethodName._Ready) || StringName.op_Equality(ref method, NHandCardHolder.MethodName._ExitTree) || StringName.op_Equality(ref method, NHandCardHolder.MethodName.Clear) || StringName.op_Equality(ref method, NHandCardHolder.MethodName.OnFocus) || StringName.op_Equality(ref method, NHandCardHolder.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NHandCardHolder.MethodName.OnMousePressed) || StringName.op_Equality(ref method, NHandCardHolder.MethodName.OnMouseReleased) || StringName.op_Equality(ref method, NHandCardHolder.MethodName.DoCardHoverEffects) || StringName.op_Equality(ref method, NHandCardHolder.MethodName.SetIndexLabel) || StringName.op_Equality(ref method, NHandCardHolder.MethodName.SetTargetAngle) || StringName.op_Equality(ref method, NHandCardHolder.MethodName.SetTargetPosition) || StringName.op_Equality(ref method, NHandCardHolder.MethodName.SetTargetScale) || StringName.op_Equality(ref method, NHandCardHolder.MethodName.SetAngleInstantly) || StringName.op_Equality(ref method, NHandCardHolder.MethodName.SetScaleInstantly) || StringName.op_Equality(ref method, NHandCardHolder.MethodName.StopAnimations) || StringName.op_Equality(ref method, NHandCardHolder.MethodName.SetCard) || StringName.op_Equality(ref method, NHandCardHolder.MethodName.UpdateCard) || StringName.op_Equality(ref method, NHandCardHolder.MethodName.BeginDrag) || StringName.op_Equality(ref method, NHandCardHolder.MethodName.CancelDrag) || StringName.op_Equality(ref method, NHandCardHolder.MethodName.SetDefaultTargets) || StringName.op_Equality(ref method, NHandCardHolder.MethodName.Flash) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NHandCardHolder.PropertyName.InSelectMode))
    {
      this.InSelectMode = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHandCardHolder.PropertyName._flash))
    {
      this._flash = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHandCardHolder.PropertyName._flashTween))
    {
      this._flashTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHandCardHolder.PropertyName._handIndexLabel))
    {
      this._handIndexLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHandCardHolder.PropertyName._targetPosition))
    {
      this._targetPosition = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHandCardHolder.PropertyName._targetAngle))
    {
      this._targetAngle = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHandCardHolder.PropertyName._targetScale))
    {
      this._targetScale = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NHandCardHolder.PropertyName._hand))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._hand = VariantUtils.ConvertTo<NPlayerHand>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NHandCardHolder.PropertyName.InSelectMode))
    {
      ref godot_variant local = ref value;
      bool inSelectMode = this.InSelectMode;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref inSelectMode);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NHandCardHolder.PropertyName.TargetPosition))
    {
      ref godot_variant local = ref value;
      Vector2 targetPosition = this.TargetPosition;
      godot_variant from = VariantUtils.CreateFrom<Vector2>(ref targetPosition);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NHandCardHolder.PropertyName.TargetAngle))
    {
      ref godot_variant local = ref value;
      float targetAngle = this.TargetAngle;
      godot_variant from = VariantUtils.CreateFrom<float>(ref targetAngle);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NHandCardHolder.PropertyName.ShouldGlowGold))
    {
      ref godot_variant local = ref value;
      bool shouldGlowGold = this.ShouldGlowGold;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref shouldGlowGold);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NHandCardHolder.PropertyName.ShouldGlowRed))
    {
      ref godot_variant local = ref value;
      bool shouldGlowRed = this.ShouldGlowRed;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref shouldGlowRed);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NHandCardHolder.PropertyName._flash))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._flash);
      return true;
    }
    if (StringName.op_Equality(ref name, NHandCardHolder.PropertyName._flashTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._flashTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NHandCardHolder.PropertyName._handIndexLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._handIndexLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NHandCardHolder.PropertyName._targetPosition))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._targetPosition);
      return true;
    }
    if (StringName.op_Equality(ref name, NHandCardHolder.PropertyName._targetAngle))
    {
      value = VariantUtils.CreateFrom<float>(ref this._targetAngle);
      return true;
    }
    if (StringName.op_Equality(ref name, NHandCardHolder.PropertyName._targetScale))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._targetScale);
      return true;
    }
    if (!StringName.op_Equality(ref name, NHandCardHolder.PropertyName._hand))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<NPlayerHand>(ref this._hand);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NHandCardHolder.PropertyName._flash, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NHandCardHolder.PropertyName._flashTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NHandCardHolder.PropertyName._handIndexLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NHandCardHolder.PropertyName._targetPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NHandCardHolder.PropertyName._targetAngle, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NHandCardHolder.PropertyName._targetScale, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NHandCardHolder.PropertyName._hand, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NHandCardHolder.PropertyName.InSelectMode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NHandCardHolder.PropertyName.TargetPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NHandCardHolder.PropertyName.TargetAngle, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NHandCardHolder.PropertyName.ShouldGlowGold, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NHandCardHolder.PropertyName.ShouldGlowRed, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName inSelectMode1 = NHandCardHolder.PropertyName.InSelectMode;
    bool inSelectMode2 = this.InSelectMode;
    Variant variant = Variant.From<bool>(ref inSelectMode2);
    serializationInfo.AddProperty(inSelectMode1, variant);
    info.AddProperty(NHandCardHolder.PropertyName._flash, Variant.From<Control>(ref this._flash));
    info.AddProperty(NHandCardHolder.PropertyName._flashTween, Variant.From<Tween>(ref this._flashTween));
    info.AddProperty(NHandCardHolder.PropertyName._handIndexLabel, Variant.From<MegaLabel>(ref this._handIndexLabel));
    info.AddProperty(NHandCardHolder.PropertyName._targetPosition, Variant.From<Vector2>(ref this._targetPosition));
    info.AddProperty(NHandCardHolder.PropertyName._targetAngle, Variant.From<float>(ref this._targetAngle));
    info.AddProperty(NHandCardHolder.PropertyName._targetScale, Variant.From<Vector2>(ref this._targetScale));
    info.AddProperty(NHandCardHolder.PropertyName._hand, Variant.From<NPlayerHand>(ref this._hand));
    info.AddSignalEventDelegate(NHandCardHolder.SignalName.HolderFocused, (Delegate) this.backing_HolderFocused);
    info.AddSignalEventDelegate(NHandCardHolder.SignalName.HolderUnfocused, (Delegate) this.backing_HolderUnfocused);
    info.AddSignalEventDelegate(NHandCardHolder.SignalName.HolderMouseClicked, (Delegate) this.backing_HolderMouseClicked);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NHandCardHolder.PropertyName.InSelectMode, ref variant1))
      this.InSelectMode = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (info.TryGetProperty(NHandCardHolder.PropertyName._flash, ref variant2))
      this._flash = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NHandCardHolder.PropertyName._flashTween, ref variant3))
      this._flashTween = ((Variant) ref variant3).As<Tween>();
    Variant variant4;
    if (info.TryGetProperty(NHandCardHolder.PropertyName._handIndexLabel, ref variant4))
      this._handIndexLabel = ((Variant) ref variant4).As<MegaLabel>();
    Variant variant5;
    if (info.TryGetProperty(NHandCardHolder.PropertyName._targetPosition, ref variant5))
      this._targetPosition = ((Variant) ref variant5).As<Vector2>();
    Variant variant6;
    if (info.TryGetProperty(NHandCardHolder.PropertyName._targetAngle, ref variant6))
      this._targetAngle = ((Variant) ref variant6).As<float>();
    Variant variant7;
    if (info.TryGetProperty(NHandCardHolder.PropertyName._targetScale, ref variant7))
      this._targetScale = ((Variant) ref variant7).As<Vector2>();
    Variant variant8;
    if (info.TryGetProperty(NHandCardHolder.PropertyName._hand, ref variant8))
      this._hand = ((Variant) ref variant8).As<NPlayerHand>();
    NHandCardHolder.HolderFocusedEventHandler focusedEventHandler;
    if (info.TryGetSignalEventDelegate<NHandCardHolder.HolderFocusedEventHandler>(NHandCardHolder.SignalName.HolderFocused, ref focusedEventHandler))
      this.backing_HolderFocused = focusedEventHandler;
    NHandCardHolder.HolderUnfocusedEventHandler unfocusedEventHandler;
    if (info.TryGetSignalEventDelegate<NHandCardHolder.HolderUnfocusedEventHandler>(NHandCardHolder.SignalName.HolderUnfocused, ref unfocusedEventHandler))
      this.backing_HolderUnfocused = unfocusedEventHandler;
    NHandCardHolder.HolderMouseClickedEventHandler clickedEventHandler;
    if (!info.TryGetSignalEventDelegate<NHandCardHolder.HolderMouseClickedEventHandler>(NHandCardHolder.SignalName.HolderMouseClicked, ref clickedEventHandler))
      return;
    this.backing_HolderMouseClicked = clickedEventHandler;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<MethodInfo> GetGodotSignalList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NHandCardHolder.SignalName.HolderFocused, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("cardHolder"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NHandCardHolder.SignalName.HolderUnfocused, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("cardHolder"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NHandCardHolder.SignalName.HolderMouseClicked, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("cardHolder"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  public event NHandCardHolder.HolderFocusedEventHandler HolderFocused
  {
    add => this.backing_HolderFocused += value;
    remove => this.backing_HolderFocused -= value;
  }

  protected void EmitSignalHolderFocused(NHandCardHolder cardHolder)
  {
    ((GodotObject) this).EmitSignal(NHandCardHolder.SignalName.HolderFocused, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) cardHolder)
    });
  }

  public event NHandCardHolder.HolderUnfocusedEventHandler HolderUnfocused
  {
    add => this.backing_HolderUnfocused += value;
    remove => this.backing_HolderUnfocused -= value;
  }

  protected void EmitSignalHolderUnfocused(NHandCardHolder cardHolder)
  {
    ((GodotObject) this).EmitSignal(NHandCardHolder.SignalName.HolderUnfocused, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) cardHolder)
    });
  }

  public event NHandCardHolder.HolderMouseClickedEventHandler HolderMouseClicked
  {
    add => this.backing_HolderMouseClicked += value;
    remove => this.backing_HolderMouseClicked -= value;
  }

  protected void EmitSignalHolderMouseClicked(NCardHolder cardHolder)
  {
    ((GodotObject) this).EmitSignal(NHandCardHolder.SignalName.HolderMouseClicked, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) cardHolder)
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RaiseGodotClassSignalCallbacks(
    in godot_string_name signal,
    NativeVariantPtrArgs args)
  {
    if (StringName.op_Equality(ref signal, NHandCardHolder.SignalName.HolderFocused) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NHandCardHolder.HolderFocusedEventHandler backingHolderFocused = this.backing_HolderFocused;
      if (backingHolderFocused == null)
        return;
      backingHolderFocused(VariantUtils.ConvertTo<NHandCardHolder>(ref ((NativeVariantPtrArgs) ref args)[0]));
    }
    else if (StringName.op_Equality(ref signal, NHandCardHolder.SignalName.HolderUnfocused) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NHandCardHolder.HolderUnfocusedEventHandler backingHolderUnfocused = this.backing_HolderUnfocused;
      if (backingHolderUnfocused == null)
        return;
      backingHolderUnfocused(VariantUtils.ConvertTo<NHandCardHolder>(ref ((NativeVariantPtrArgs) ref args)[0]));
    }
    else if (StringName.op_Equality(ref signal, NHandCardHolder.SignalName.HolderMouseClicked) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NHandCardHolder.HolderMouseClickedEventHandler holderMouseClicked = this.backing_HolderMouseClicked;
      if (holderMouseClicked == null)
        return;
      holderMouseClicked(VariantUtils.ConvertTo<NCardHolder>(ref ((NativeVariantPtrArgs) ref args)[0]));
    }
    else
      base.RaiseGodotClassSignalCallbacks(in signal, args);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassSignal(in godot_string_name signal)
  {
    return StringName.op_Equality(ref signal, NHandCardHolder.SignalName.HolderFocused) || StringName.op_Equality(ref signal, NHandCardHolder.SignalName.HolderUnfocused) || StringName.op_Equality(ref signal, NHandCardHolder.SignalName.HolderMouseClicked) || base.HasGodotClassSignal(in signal);
  }

  [Signal]
  public delegate void HolderFocusedEventHandler(
  #nullable enable
  NHandCardHolder cardHolder);

  [Signal]
  public delegate void HolderUnfocusedEventHandler(NHandCardHolder cardHolder);

  [Signal]
  public delegate void HolderMouseClickedEventHandler(NCardHolder cardHolder);

  public new class MethodName : NCardHolder.MethodName
  {
    public static readonly 
    #nullable disable
    StringName Create = StringName.op_Implicit(nameof (Create));
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public new static readonly StringName Clear = StringName.op_Implicit(nameof (Clear));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public new static readonly StringName OnMousePressed = StringName.op_Implicit(nameof (OnMousePressed));
    public new static readonly StringName OnMouseReleased = StringName.op_Implicit(nameof (OnMouseReleased));
    public new static readonly StringName DoCardHoverEffects = StringName.op_Implicit(nameof (DoCardHoverEffects));
    public static readonly StringName SetIndexLabel = StringName.op_Implicit(nameof (SetIndexLabel));
    public static readonly StringName SetTargetAngle = StringName.op_Implicit(nameof (SetTargetAngle));
    public static readonly StringName SetTargetPosition = StringName.op_Implicit(nameof (SetTargetPosition));
    public static readonly StringName SetTargetScale = StringName.op_Implicit(nameof (SetTargetScale));
    public static readonly StringName SetAngleInstantly = StringName.op_Implicit(nameof (SetAngleInstantly));
    public static readonly StringName SetScaleInstantly = StringName.op_Implicit(nameof (SetScaleInstantly));
    public static readonly StringName StopAnimations = StringName.op_Implicit(nameof (StopAnimations));
    public new static readonly StringName SetCard = StringName.op_Implicit(nameof (SetCard));
    public static readonly StringName UpdateCard = StringName.op_Implicit(nameof (UpdateCard));
    public static readonly StringName BeginDrag = StringName.op_Implicit(nameof (BeginDrag));
    public static readonly StringName CancelDrag = StringName.op_Implicit(nameof (CancelDrag));
    public static readonly StringName SetDefaultTargets = StringName.op_Implicit(nameof (SetDefaultTargets));
    public static readonly StringName Flash = StringName.op_Implicit(nameof (Flash));
  }

  public new class PropertyName : NCardHolder.PropertyName
  {
    public static readonly StringName InSelectMode = StringName.op_Implicit(nameof (InSelectMode));
    public static readonly StringName TargetPosition = StringName.op_Implicit(nameof (TargetPosition));
    public static readonly StringName TargetAngle = StringName.op_Implicit(nameof (TargetAngle));
    public static readonly StringName ShouldGlowGold = StringName.op_Implicit(nameof (ShouldGlowGold));
    public static readonly StringName ShouldGlowRed = StringName.op_Implicit(nameof (ShouldGlowRed));
    public static readonly StringName _flash = StringName.op_Implicit(nameof (_flash));
    public static readonly StringName _flashTween = StringName.op_Implicit(nameof (_flashTween));
    public static readonly StringName _handIndexLabel = StringName.op_Implicit(nameof (_handIndexLabel));
    public static readonly StringName _targetPosition = StringName.op_Implicit(nameof (_targetPosition));
    public static readonly StringName _targetAngle = StringName.op_Implicit(nameof (_targetAngle));
    public static readonly StringName _targetScale = StringName.op_Implicit(nameof (_targetScale));
    public static readonly StringName _hand = StringName.op_Implicit(nameof (_hand));
  }

  public new class SignalName : NCardHolder.SignalName
  {
    public static readonly StringName HolderFocused = StringName.op_Implicit(nameof (HolderFocused));
    public static readonly StringName HolderUnfocused = StringName.op_Implicit(nameof (HolderUnfocused));
    public static readonly StringName HolderMouseClicked = StringName.op_Implicit(nameof (HolderMouseClicked));
  }
}
