// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Timeline.NEpochSlot
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Timeline;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Timeline;

[ScriptPath("res://src/Core/Nodes/Screens/Timeline/NEpochSlot.cs")]
public class NEpochSlot : NButton
{
  private static readonly StringName _lod = new StringName("lod");
  private static readonly StringName _v = new StringName("v");
  private static readonly StringName _s = new StringName("s");
  private const string _unlockIconPath = "res://images/packed/unlock_icon.png";
  private const string _scenePath = "res://scenes/timeline_screen/epoch_slot.tscn";
  public static readonly IEnumerable<string> assetPaths = (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[2]
  {
    "res://scenes/timeline_screen/epoch_slot.tscn",
    "res://images/packed/unlock_icon.png"
  });
  private TextureRect _slotImage;
  private TextureRect _portrait;
  private TextureRect _chains;
  private ShaderMaterial _hsv;
  private TextureRect _blurPortrait;
  private TextureRect _outline;
  private Control _blur;
  private ShaderMaterial _blurShader;
  private NSelectionReticle _selectionReticle;
  private NEpochOffscreenVfx? _offscreenVfx;
  private Control? _highlightVfx;
  private SubViewportContainer _subViewportContainer;
  private SubViewport _subViewport;
  private bool _isGlowPulsing;
  public EpochModel model;
  private bool _isComplete;
  private bool _isHovered;
  private EpochEra _era;
  public int eraPosition;
  private Tween? _glowTween;
  private Tween? _spawnTween;
  private Tween? _hoverTween;
  private static readonly Color _highlightSlotColor = StsColors.purple;
  private static readonly Color _defaultSlotOutlineColor = new Color("70a0ff18");
  private IHoverTip? _hoverTip;

  protected override string ClickedSfx => "event:/sfx/ui/timeline/ui_timeline_click";

  protected override string HoveredSfx
  {
    get
    {
      return this.State != EpochSlotState.NotObtained ? "event:/sfx/ui/timeline/ui_timeline_hover" : "event:/sfx/ui/timeline/ui_timeline_hover_locked";
    }
  }

  public EpochSlotState State { get; private set; }

  public bool HasSpawned { get; private set; }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._slotImage = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%SlotImage"));
    this._portrait = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Portrait"));
    this._chains = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Chains"));
    this._hsv = (ShaderMaterial) ((CanvasItem) this._portrait).GetMaterial();
    this._blurPortrait = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%BlurPortrait"));
    this._outline = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Outline"));
    this._subViewportContainer = ((Node) this).GetNode<SubViewportContainer>(NodePath.op_Implicit("%SubViewportContainer"));
    this._subViewport = ((Node) this).GetNode<SubViewport>(NodePath.op_Implicit("%SubViewport"));
    this._blurShader = (ShaderMaterial) ((CanvasItem) ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Blur"))).GetMaterial();
    this._selectionReticle = ((Node) this).GetNode<NSelectionReticle>(NodePath.op_Implicit("%SelectionReticle"));
    if (!NGame.IsReleaseGame())
    {
      MegaLabel node = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%DebugLabel"));
      node.Text = this.model.GetType().Name;
      ((CanvasItem) node).Visible = true;
    }
    this.SetState(this.State);
  }

  public static NEpochSlot Create(EpochSlotData data)
  {
    NEpochSlot nepochSlot = PreloadManager.Cache.GetScene("res://scenes/timeline_screen/epoch_slot.tscn").Instantiate<NEpochSlot>((PackedScene.GenEditState) 0L);
    nepochSlot._era = data.Era;
    nepochSlot.State = data.State;
    nepochSlot.eraPosition = data.EraPosition;
    nepochSlot.model = data.Model;
    return nepochSlot;
  }

  protected override void OnRelease()
  {
    if (!NGame.IsReleaseGame() && this.State == EpochSlotState.NotObtained && Input.IsKeyPressed((Key) 4194326L))
    {
      NHoverTipSet.Remove((Control) this);
      this.State = EpochSlotState.Obtained;
    }
    base.OnRelease();
    if (this.State == EpochSlotState.Obtained)
    {
      NTimelineScreen.Instance.DisableInput();
      ((Node) this).GetViewport().GuiReleaseFocus();
      this.RevealEpoch();
      this.SetState(EpochSlotState.Complete);
    }
    else if (this.State == EpochSlotState.Complete)
    {
      NTimelineScreen.Instance.DisableInput();
      NTimelineScreen.Instance.OpenInspectScreen(this, false);
    }
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween().SetParallel(true);
    this._hoverTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 1.05f)), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L);
    this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NEpochSlot._s), Variant.op_Implicit(1f), 0.05);
  }

  private void RevealEpoch()
  {
    this.State = EpochSlotState.Complete;
    ((CanvasItem) this._portrait).Visible = true;
    this._portrait.Texture = this.model.Portrait;
    this.DisableHighlight();
    SaveManager.Instance.RevealEpoch(this.model.Id);
    ((CanvasItem) this._slotImage).Modulate = Colors.White;
    ((CanvasItem) this._slotImage).ClipChildren = (CanvasItem.ClipChildrenMode) 2L;
    ((CanvasItem) this._chains).Visible = false;
    NTimelineScreen.Instance.OpenInspectScreen(this, true);
  }

  protected override void OnFocus()
  {
    base.OnFocus();
    this._isGlowPulsing = false;
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween().SetParallel(true);
    if (NControllerManager.Instance.IsUsingController)
      this._selectionReticle.OnSelect();
    if (this.State != EpochSlotState.NotObtained)
    {
      this._hoverTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 1.05f)), 0.05);
      if (this.State == EpochSlotState.Complete)
      {
        this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NEpochSlot._s), Variant.op_Implicit(1.1f), 0.05);
        this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NEpochSlot._v), Variant.op_Implicit(1.1f), 0.05);
        LocString unlockInfo = this.model.UnlockInfo;
        unlockInfo.Add("IsRevealed", true);
        this._hoverTip = (IHoverTip) new HoverTip(this.model.Title, unlockInfo, PreloadManager.Cache.GetTexture2D("res://images/packed/unlock_icon.png"));
      }
      else if (this.State == EpochSlotState.Obtained)
      {
        this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NEpochSlot._v), Variant.op_Implicit(0.65f), 0.05);
        LocString unlockInfo = this.model.UnlockInfo;
        unlockInfo.Add("IsRevealed", false);
        this._hoverTip = (IHoverTip) new HoverTip(this.model.Title, unlockInfo);
      }
    }
    else
    {
      this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NEpochSlot._s), Variant.op_Implicit(0.25f), 0.05);
      this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NEpochSlot._v), Variant.op_Implicit(1.2f), 0.05);
      LocString unlockInfo = this.model.UnlockInfo;
      unlockInfo.Add("IsRevealed", false);
      this._hoverTip = (IHoverTip) new HoverTip(this.model.Title, unlockInfo);
    }
    if (this._hoverTip == null)
      return;
    NHoverTipSet andShow = NHoverTipSet.CreateAndShow((Control) this, this._hoverTip);
    double num1 = (double) this.Size.X * 0.5 + 6.0;
    Transform2D globalTransform1 = ((CanvasItem) this).GetGlobalTransform();
    double x1 = (double) ((Transform2D) ref globalTransform1).Scale.X;
    float num2 = (float) (num1 * x1);
    double x2 = (double) this.GlobalPosition.X;
    double num3 = (double) this.Size.X * 0.5 + 6.0;
    Transform2D globalTransform2 = ((CanvasItem) this).GetGlobalTransform();
    double x3 = (double) ((Transform2D) ref globalTransform2).Scale.X;
    double num4 = num3 * x3;
    float num5 = (float) (x2 + num4);
    double x4 = (double) this.GlobalPosition.X;
    Rect2 viewportRect = ((CanvasItem) NGame.Instance).GetViewportRect();
    double num6 = (double) ((Rect2) ref viewportRect).Size.X * 0.699999988079071;
    if (x4 > num6)
      andShow?.SetGlobalPosition(new Vector2((float) ((double) num5 - (double) num2 - 360.0), this.GlobalPosition.Y), false);
    else
      andShow?.SetGlobalPosition(new Vector2(num5 + num2, this.GlobalPosition.Y), false);
    andShow?.SetFollowOwner();
  }

  protected override void OnUnfocus()
  {
    base.OnUnfocus();
    this._selectionReticle.OnDeselect();
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween().SetParallel(true);
    this._hoverTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    if (this.State != EpochSlotState.NotObtained)
    {
      if (this.State == EpochSlotState.Obtained)
      {
        this._isGlowPulsing = true;
        this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NEpochSlot._v), Variant.op_Implicit(0.5f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
      }
      else if (this.State == EpochSlotState.Complete)
      {
        this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NEpochSlot._s), Variant.op_Implicit(1f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
        this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NEpochSlot._v), Variant.op_Implicit(1f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
      }
    }
    else
    {
      this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NEpochSlot._s), Variant.op_Implicit(0.25f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
      this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NEpochSlot._v), Variant.op_Implicit(1.1f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    }
    NHoverTipSet.Remove((Control) this);
  }

  protected override void OnPress()
  {
    if (this.State == EpochSlotState.NotObtained)
      return;
    base.OnPress();
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween().SetParallel(true);
    this._hoverTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 0.95f)), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    if (this.State == EpochSlotState.Complete)
    {
      this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NEpochSlot._s), Variant.op_Implicit(1f), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
      this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NEpochSlot._v), Variant.op_Implicit(1f), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    }
    else
    {
      if (this.State != EpochSlotState.Obtained)
        return;
      this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NEpochSlot._v), Variant.op_Implicit(0.5f), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    }
  }

  public async Task SpawnSlot()
  {
    this._spawnTween = ((Node) this).CreateTween().SetParallel(true);
    this._spawnTween.Chain();
    this._spawnTween.TweenInterval(Rng.Chaotic.NextDouble(0.0, 0.3));
    this._spawnTween.Chain();
    this._spawnTween.TweenProperty((GodotObject) this._slotImage, NodePath.op_Implicit("position"), Variant.op_Implicit(Vector2.Zero), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L).From(Variant.op_Implicit(new Vector2(0.0f, 64f)));
    this._spawnTween.TweenProperty((GodotObject) this._outline, NodePath.op_Implicit("position"), Variant.op_Implicit(Vector2.Zero), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L).From(Variant.op_Implicit(new Vector2(0.0f, 64f)));
    this._spawnTween.TweenProperty((GodotObject) this._slotImage, NodePath.op_Implicit("self_modulate:a"), Variant.op_Implicit(1f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    if (!await this._spawnTween.AwaitFinished((Node) this))
      return;
    this.HasSpawned = true;
    if (this.State == EpochSlotState.Obtained)
      this.EnableHighlight();
    else
      ((CanvasItem) this._outline).Modulate = NEpochSlot._defaultSlotOutlineColor;
  }

  public override void _Process(double delta)
  {
    if (!this._isGlowPulsing)
      return;
    ((CanvasItem) this._outline).Modulate = new Color(((CanvasItem) this._outline).Modulate.R, ((CanvasItem) this._outline).Modulate.G, ((CanvasItem) this._outline).Modulate.B, (float) (((double) Mathf.Sin((float) Time.GetTicksMsec() * 0.005f) + 2.0) * 0.25));
  }

  private void DisableHighlight()
  {
    this._isGlowPulsing = false;
    ((CanvasItem) this._outline).Modulate = Colors.Transparent;
    Control highlightVfx = this._highlightVfx;
    if (highlightVfx != null)
      ((Node) highlightVfx).QueueFreeSafely();
    NEpochOffscreenVfx offscreenVfx = this._offscreenVfx;
    if (offscreenVfx == null)
      return;
    ((Node) offscreenVfx).QueueFreeSafely();
  }

  private void EnableHighlight()
  {
    this._isGlowPulsing = true;
    ((CanvasItem) this._outline).Modulate = NEpochSlot._highlightSlotColor;
    ((CanvasItem) this._outline).SelfModulate = StsColors.transparentWhite;
    this._glowTween?.Kill();
    this._glowTween = ((Node) this).CreateTween();
    this._glowTween.TweenProperty((GodotObject) this._outline, NodePath.op_Implicit("self_modulate:a"), Variant.op_Implicit(1f), 1.0);
    this._offscreenVfx = NEpochOffscreenVfx.Create(this);
    this._highlightVfx = (Control) NEpochHighlightVfx.Create();
    ((Node) this).AddChildSafely((Node) this._highlightVfx);
    ((Node) NTimelineScreen.Instance.GetReminderVfxHolder()).AddChildSafely((Node) this._offscreenVfx);
    ((Node) this).MoveChildSafely((Node) this._highlightVfx, 0);
  }

  public void SetState(EpochSlotState setState)
  {
    this.State = setState;
    if (this.State == EpochSlotState.None)
    {
      Log.Error("Slot State is invalid.");
    }
    else
    {
      ((CanvasItem) this._slotImage).Modulate = Colors.White;
      ((CanvasItem) this._slotImage).ClipChildren = (CanvasItem.ClipChildrenMode) 2L;
      ((CanvasItem) this._portrait).Visible = true;
      if (this.State == EpochSlotState.Complete)
      {
        this.DisableHighlight();
        this._portrait.Texture = this.model.Portrait;
        this.UpdateShaderS(1f);
        this.UpdateShaderV(1f);
      }
      else if (this.State == EpochSlotState.Obtained)
      {
        this._portrait.Texture = this.model.Portrait;
        this.UpdateShaderS(0.0f);
        this.UpdateShaderV(0.5f);
        ((CanvasItem) this._chains).Visible = true;
      }
      else if (this.State == EpochSlotState.NotObtained)
      {
        this._blurShader.SetShaderParameter(NEpochSlot._lod, Variant.op_Implicit(2f));
        this.UpdateShaderS(0.25f);
        this.UpdateShaderV(1f);
        this._blurPortrait.Texture = this.model.Portrait;
        ((CanvasItem) this._subViewportContainer).Visible = true;
        this._portrait.Texture = (Texture2D) ((Viewport) this._subViewport).GetTexture();
      }
      this.MouseDefaultCursorShape = this.State == EpochSlotState.Complete ? (Control.CursorShape) 16L /*0x10*/ : (Control.CursorShape) 0L;
    }
  }

  private void UpdateShaderS(float value)
  {
    this._hsv.SetShaderParameter(NEpochSlot._s, Variant.op_Implicit(value));
  }

  private void UpdateShaderV(float value)
  {
    this._hsv.SetShaderParameter(NEpochSlot._v, Variant.op_Implicit(value));
  }

  private void UpdateBlurLod(float value)
  {
    this._blurShader.SetShaderParameter(NEpochSlot._lod, Variant.op_Implicit(value));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(13)
    {
      new MethodInfo(NEpochSlot.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEpochSlot.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEpochSlot.MethodName.RevealEpoch, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEpochSlot.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEpochSlot.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEpochSlot.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEpochSlot.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NEpochSlot.MethodName.DisableHighlight, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEpochSlot.MethodName.EnableHighlight, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEpochSlot.MethodName.SetState, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("setState"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NEpochSlot.MethodName.UpdateShaderS, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("value"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NEpochSlot.MethodName.UpdateShaderV, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("value"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NEpochSlot.MethodName.UpdateBlurLod, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("value"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NEpochSlot.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEpochSlot.MethodName.OnRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRelease();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEpochSlot.MethodName.RevealEpoch) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RevealEpoch();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEpochSlot.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEpochSlot.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEpochSlot.MethodName.OnPress) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPress();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEpochSlot.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEpochSlot.MethodName.DisableHighlight) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DisableHighlight();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEpochSlot.MethodName.EnableHighlight) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.EnableHighlight();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEpochSlot.MethodName.SetState) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetState(VariantUtils.ConvertTo<EpochSlotState>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEpochSlot.MethodName.UpdateShaderS) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateShaderS(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEpochSlot.MethodName.UpdateShaderV) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateShaderV(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NEpochSlot.MethodName.UpdateBlurLod) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.UpdateBlurLod(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NEpochSlot.MethodName._Ready) || StringName.op_Equality(ref method, NEpochSlot.MethodName.OnRelease) || StringName.op_Equality(ref method, NEpochSlot.MethodName.RevealEpoch) || StringName.op_Equality(ref method, NEpochSlot.MethodName.OnFocus) || StringName.op_Equality(ref method, NEpochSlot.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NEpochSlot.MethodName.OnPress) || StringName.op_Equality(ref method, NEpochSlot.MethodName._Process) || StringName.op_Equality(ref method, NEpochSlot.MethodName.DisableHighlight) || StringName.op_Equality(ref method, NEpochSlot.MethodName.EnableHighlight) || StringName.op_Equality(ref method, NEpochSlot.MethodName.SetState) || StringName.op_Equality(ref method, NEpochSlot.MethodName.UpdateShaderS) || StringName.op_Equality(ref method, NEpochSlot.MethodName.UpdateShaderV) || StringName.op_Equality(ref method, NEpochSlot.MethodName.UpdateBlurLod) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName.State))
    {
      this.State = VariantUtils.ConvertTo<EpochSlotState>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName.HasSpawned))
    {
      this.HasSpawned = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName._slotImage))
    {
      this._slotImage = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName._portrait))
    {
      this._portrait = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName._chains))
    {
      this._chains = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName._hsv))
    {
      this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName._blurPortrait))
    {
      this._blurPortrait = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName._outline))
    {
      this._outline = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName._blur))
    {
      this._blur = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName._blurShader))
    {
      this._blurShader = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName._selectionReticle))
    {
      this._selectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName._offscreenVfx))
    {
      this._offscreenVfx = VariantUtils.ConvertTo<NEpochOffscreenVfx>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName._highlightVfx))
    {
      this._highlightVfx = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName._subViewportContainer))
    {
      this._subViewportContainer = VariantUtils.ConvertTo<SubViewportContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName._subViewport))
    {
      this._subViewport = VariantUtils.ConvertTo<SubViewport>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName._isGlowPulsing))
    {
      this._isGlowPulsing = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName._isComplete))
    {
      this._isComplete = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName._isHovered))
    {
      this._isHovered = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName._era))
    {
      this._era = VariantUtils.ConvertTo<EpochEra>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName.eraPosition))
    {
      this.eraPosition = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName._glowTween))
    {
      this._glowTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName._spawnTween))
    {
      this._spawnTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NEpochSlot.PropertyName._hoverTween))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName.ClickedSfx))
    {
      ref godot_variant local = ref value;
      string clickedSfx = this.ClickedSfx;
      godot_variant from = VariantUtils.CreateFrom<string>(ref clickedSfx);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName.HoveredSfx))
    {
      ref godot_variant local = ref value;
      string hoveredSfx = this.HoveredSfx;
      godot_variant from = VariantUtils.CreateFrom<string>(ref hoveredSfx);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName.State))
    {
      ref godot_variant local = ref value;
      EpochSlotState state = this.State;
      godot_variant from = VariantUtils.CreateFrom<EpochSlotState>(ref state);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName.HasSpawned))
    {
      ref godot_variant local = ref value;
      bool hasSpawned = this.HasSpawned;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref hasSpawned);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName._slotImage))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._slotImage);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName._portrait))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._portrait);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName._chains))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._chains);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName._hsv))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName._blurPortrait))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._blurPortrait);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName._outline))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._outline);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName._blur))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._blur);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName._blurShader))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._blurShader);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName._selectionReticle))
    {
      value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._selectionReticle);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName._offscreenVfx))
    {
      value = VariantUtils.CreateFrom<NEpochOffscreenVfx>(ref this._offscreenVfx);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName._highlightVfx))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._highlightVfx);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName._subViewportContainer))
    {
      value = VariantUtils.CreateFrom<SubViewportContainer>(ref this._subViewportContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName._subViewport))
    {
      value = VariantUtils.CreateFrom<SubViewport>(ref this._subViewport);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName._isGlowPulsing))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isGlowPulsing);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName._isComplete))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isComplete);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName._isHovered))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isHovered);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName._era))
    {
      value = VariantUtils.CreateFrom<EpochEra>(ref this._era);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName.eraPosition))
    {
      value = VariantUtils.CreateFrom<int>(ref this.eraPosition);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName._glowTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._glowTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlot.PropertyName._spawnTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._spawnTween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NEpochSlot.PropertyName._hoverTween))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 4L, NEpochSlot.PropertyName.ClickedSfx, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, NEpochSlot.PropertyName.HoveredSfx, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochSlot.PropertyName._slotImage, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochSlot.PropertyName._portrait, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochSlot.PropertyName._chains, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochSlot.PropertyName._hsv, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochSlot.PropertyName._blurPortrait, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochSlot.PropertyName._outline, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochSlot.PropertyName._blur, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochSlot.PropertyName._blurShader, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochSlot.PropertyName._selectionReticle, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochSlot.PropertyName._offscreenVfx, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochSlot.PropertyName._highlightVfx, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochSlot.PropertyName._subViewportContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochSlot.PropertyName._subViewport, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NEpochSlot.PropertyName._isGlowPulsing, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NEpochSlot.PropertyName._isComplete, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NEpochSlot.PropertyName._isHovered, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NEpochSlot.PropertyName.State, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NEpochSlot.PropertyName._era, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NEpochSlot.PropertyName.eraPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochSlot.PropertyName._glowTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochSlot.PropertyName._spawnTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochSlot.PropertyName._hoverTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NEpochSlot.PropertyName.HasSpawned, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo1 = info;
    StringName state1 = NEpochSlot.PropertyName.State;
    EpochSlotState state2 = this.State;
    Variant variant1 = Variant.From<EpochSlotState>(ref state2);
    serializationInfo1.AddProperty(state1, variant1);
    GodotSerializationInfo serializationInfo2 = info;
    StringName hasSpawned1 = NEpochSlot.PropertyName.HasSpawned;
    bool hasSpawned2 = this.HasSpawned;
    Variant variant2 = Variant.From<bool>(ref hasSpawned2);
    serializationInfo2.AddProperty(hasSpawned1, variant2);
    info.AddProperty(NEpochSlot.PropertyName._slotImage, Variant.From<TextureRect>(ref this._slotImage));
    info.AddProperty(NEpochSlot.PropertyName._portrait, Variant.From<TextureRect>(ref this._portrait));
    info.AddProperty(NEpochSlot.PropertyName._chains, Variant.From<TextureRect>(ref this._chains));
    info.AddProperty(NEpochSlot.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
    info.AddProperty(NEpochSlot.PropertyName._blurPortrait, Variant.From<TextureRect>(ref this._blurPortrait));
    info.AddProperty(NEpochSlot.PropertyName._outline, Variant.From<TextureRect>(ref this._outline));
    info.AddProperty(NEpochSlot.PropertyName._blur, Variant.From<Control>(ref this._blur));
    info.AddProperty(NEpochSlot.PropertyName._blurShader, Variant.From<ShaderMaterial>(ref this._blurShader));
    info.AddProperty(NEpochSlot.PropertyName._selectionReticle, Variant.From<NSelectionReticle>(ref this._selectionReticle));
    info.AddProperty(NEpochSlot.PropertyName._offscreenVfx, Variant.From<NEpochOffscreenVfx>(ref this._offscreenVfx));
    info.AddProperty(NEpochSlot.PropertyName._highlightVfx, Variant.From<Control>(ref this._highlightVfx));
    info.AddProperty(NEpochSlot.PropertyName._subViewportContainer, Variant.From<SubViewportContainer>(ref this._subViewportContainer));
    info.AddProperty(NEpochSlot.PropertyName._subViewport, Variant.From<SubViewport>(ref this._subViewport));
    info.AddProperty(NEpochSlot.PropertyName._isGlowPulsing, Variant.From<bool>(ref this._isGlowPulsing));
    info.AddProperty(NEpochSlot.PropertyName._isComplete, Variant.From<bool>(ref this._isComplete));
    info.AddProperty(NEpochSlot.PropertyName._isHovered, Variant.From<bool>(ref this._isHovered));
    info.AddProperty(NEpochSlot.PropertyName._era, Variant.From<EpochEra>(ref this._era));
    info.AddProperty(NEpochSlot.PropertyName.eraPosition, Variant.From<int>(ref this.eraPosition));
    info.AddProperty(NEpochSlot.PropertyName._glowTween, Variant.From<Tween>(ref this._glowTween));
    info.AddProperty(NEpochSlot.PropertyName._spawnTween, Variant.From<Tween>(ref this._spawnTween));
    info.AddProperty(NEpochSlot.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NEpochSlot.PropertyName.State, ref variant1))
      this.State = ((Variant) ref variant1).As<EpochSlotState>();
    Variant variant2;
    if (info.TryGetProperty(NEpochSlot.PropertyName.HasSpawned, ref variant2))
      this.HasSpawned = ((Variant) ref variant2).As<bool>();
    Variant variant3;
    if (info.TryGetProperty(NEpochSlot.PropertyName._slotImage, ref variant3))
      this._slotImage = ((Variant) ref variant3).As<TextureRect>();
    Variant variant4;
    if (info.TryGetProperty(NEpochSlot.PropertyName._portrait, ref variant4))
      this._portrait = ((Variant) ref variant4).As<TextureRect>();
    Variant variant5;
    if (info.TryGetProperty(NEpochSlot.PropertyName._chains, ref variant5))
      this._chains = ((Variant) ref variant5).As<TextureRect>();
    Variant variant6;
    if (info.TryGetProperty(NEpochSlot.PropertyName._hsv, ref variant6))
      this._hsv = ((Variant) ref variant6).As<ShaderMaterial>();
    Variant variant7;
    if (info.TryGetProperty(NEpochSlot.PropertyName._blurPortrait, ref variant7))
      this._blurPortrait = ((Variant) ref variant7).As<TextureRect>();
    Variant variant8;
    if (info.TryGetProperty(NEpochSlot.PropertyName._outline, ref variant8))
      this._outline = ((Variant) ref variant8).As<TextureRect>();
    Variant variant9;
    if (info.TryGetProperty(NEpochSlot.PropertyName._blur, ref variant9))
      this._blur = ((Variant) ref variant9).As<Control>();
    Variant variant10;
    if (info.TryGetProperty(NEpochSlot.PropertyName._blurShader, ref variant10))
      this._blurShader = ((Variant) ref variant10).As<ShaderMaterial>();
    Variant variant11;
    if (info.TryGetProperty(NEpochSlot.PropertyName._selectionReticle, ref variant11))
      this._selectionReticle = ((Variant) ref variant11).As<NSelectionReticle>();
    Variant variant12;
    if (info.TryGetProperty(NEpochSlot.PropertyName._offscreenVfx, ref variant12))
      this._offscreenVfx = ((Variant) ref variant12).As<NEpochOffscreenVfx>();
    Variant variant13;
    if (info.TryGetProperty(NEpochSlot.PropertyName._highlightVfx, ref variant13))
      this._highlightVfx = ((Variant) ref variant13).As<Control>();
    Variant variant14;
    if (info.TryGetProperty(NEpochSlot.PropertyName._subViewportContainer, ref variant14))
      this._subViewportContainer = ((Variant) ref variant14).As<SubViewportContainer>();
    Variant variant15;
    if (info.TryGetProperty(NEpochSlot.PropertyName._subViewport, ref variant15))
      this._subViewport = ((Variant) ref variant15).As<SubViewport>();
    Variant variant16;
    if (info.TryGetProperty(NEpochSlot.PropertyName._isGlowPulsing, ref variant16))
      this._isGlowPulsing = ((Variant) ref variant16).As<bool>();
    Variant variant17;
    if (info.TryGetProperty(NEpochSlot.PropertyName._isComplete, ref variant17))
      this._isComplete = ((Variant) ref variant17).As<bool>();
    Variant variant18;
    if (info.TryGetProperty(NEpochSlot.PropertyName._isHovered, ref variant18))
      this._isHovered = ((Variant) ref variant18).As<bool>();
    Variant variant19;
    if (info.TryGetProperty(NEpochSlot.PropertyName._era, ref variant19))
      this._era = ((Variant) ref variant19).As<EpochEra>();
    Variant variant20;
    if (info.TryGetProperty(NEpochSlot.PropertyName.eraPosition, ref variant20))
      this.eraPosition = ((Variant) ref variant20).As<int>();
    Variant variant21;
    if (info.TryGetProperty(NEpochSlot.PropertyName._glowTween, ref variant21))
      this._glowTween = ((Variant) ref variant21).As<Tween>();
    Variant variant22;
    if (info.TryGetProperty(NEpochSlot.PropertyName._spawnTween, ref variant22))
      this._spawnTween = ((Variant) ref variant22).As<Tween>();
    Variant variant23;
    if (!info.TryGetProperty(NEpochSlot.PropertyName._hoverTween, ref variant23))
      return;
    this._hoverTween = ((Variant) ref variant23).As<Tween>();
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
    public static readonly StringName RevealEpoch = StringName.op_Implicit(nameof (RevealEpoch));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public new static readonly StringName OnPress = StringName.op_Implicit(nameof (OnPress));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName DisableHighlight = StringName.op_Implicit(nameof (DisableHighlight));
    public static readonly StringName EnableHighlight = StringName.op_Implicit(nameof (EnableHighlight));
    public static readonly StringName SetState = StringName.op_Implicit(nameof (SetState));
    public static readonly StringName UpdateShaderS = StringName.op_Implicit(nameof (UpdateShaderS));
    public static readonly StringName UpdateShaderV = StringName.op_Implicit(nameof (UpdateShaderV));
    public static readonly StringName UpdateBlurLod = StringName.op_Implicit(nameof (UpdateBlurLod));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public new static readonly StringName ClickedSfx = StringName.op_Implicit(nameof (ClickedSfx));
    public new static readonly StringName HoveredSfx = StringName.op_Implicit(nameof (HoveredSfx));
    public static readonly StringName State = StringName.op_Implicit(nameof (State));
    public static readonly StringName HasSpawned = StringName.op_Implicit(nameof (HasSpawned));
    public static readonly StringName _slotImage = StringName.op_Implicit(nameof (_slotImage));
    public static readonly StringName _portrait = StringName.op_Implicit(nameof (_portrait));
    public static readonly StringName _chains = StringName.op_Implicit(nameof (_chains));
    public static readonly StringName _hsv = StringName.op_Implicit(nameof (_hsv));
    public static readonly StringName _blurPortrait = StringName.op_Implicit(nameof (_blurPortrait));
    public static readonly StringName _outline = StringName.op_Implicit(nameof (_outline));
    public static readonly StringName _blur = StringName.op_Implicit(nameof (_blur));
    public static readonly StringName _blurShader = StringName.op_Implicit(nameof (_blurShader));
    public static readonly StringName _selectionReticle = StringName.op_Implicit(nameof (_selectionReticle));
    public static readonly StringName _offscreenVfx = StringName.op_Implicit(nameof (_offscreenVfx));
    public static readonly StringName _highlightVfx = StringName.op_Implicit(nameof (_highlightVfx));
    public static readonly StringName _subViewportContainer = StringName.op_Implicit(nameof (_subViewportContainer));
    public static readonly StringName _subViewport = StringName.op_Implicit(nameof (_subViewport));
    public static readonly StringName _isGlowPulsing = StringName.op_Implicit(nameof (_isGlowPulsing));
    public static readonly StringName _isComplete = StringName.op_Implicit(nameof (_isComplete));
    public new static readonly StringName _isHovered = StringName.op_Implicit(nameof (_isHovered));
    public static readonly StringName _era = StringName.op_Implicit(nameof (_era));
    public static readonly StringName eraPosition = StringName.op_Implicit(nameof (eraPosition));
    public static readonly StringName _glowTween = StringName.op_Implicit(nameof (_glowTween));
    public static readonly StringName _spawnTween = StringName.op_Implicit(nameof (_spawnTween));
    public static readonly StringName _hoverTween = StringName.op_Implicit(nameof (_hoverTween));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
