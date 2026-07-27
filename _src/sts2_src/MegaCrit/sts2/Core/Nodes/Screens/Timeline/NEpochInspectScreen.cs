// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Timeline.NEpochInspectScreen
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Nodes.Vfx.Ui;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Timeline;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Timeline;

[ScriptPath("res://src/Core/Nodes/Screens/Timeline/NEpochInspectScreen.cs")]
public class NEpochInspectScreen : NClickableControl, IScreenContext
{
  private static readonly LocString _placeholderLoc = new LocString("timeline", "PLACEHOLDER_PORTRAIT");
  public static readonly string lockedImagePath = ImageHelper.GetImagePath("packed/timeline/epoch_slot_locked.png");
  private NButton _closeButton;
  private TextureRect _portrait;
  private TextureRect _portraitFlash;
  private TextureRect _mask;
  private NEpochChains _chains;
  private ShaderMaterial _portraitHsv;
  private MegaRichTextLabel _fancyText;
  private MegaLabel _storyLabel;
  private MegaLabel _chapterLabel;
  private MegaLabel _closeLabel;
  private MegaLabel _placeholderLabel;
  private NEpochPaginateButton _nextChapterButton;
  private NEpochPaginateButton _prevChapterButton;
  private NUnlockInfo _unlockInfo;
  private List<SerializableEpoch> _allEpochs;
  private EpochModel _epoch;
  private EpochModel? _prevChapterEpoch;
  private EpochModel? _nextChapterEpoch;
  private LocString _chapterLoc;
  private bool _hasStory;
  private bool _wasRevealed;
  private float _prevChapterButtonOffsetX;
  private float _nextChapterButtonOffsetX;
  private float _maskOffsetX;
  private float _maskOffsetY;
  private float _closeButtonY;
  private Tween? _unlockTween;
  private Tween? _buttonTween;
  private Tween? _tween;
  private Tween? _textTween;
  private static readonly StringName _v = new StringName("v");
  private static readonly StringName _s = new StringName("s");

  public override void _Ready()
  {
    this._storyLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%StoryLabel"));
    this._chapterLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%ChapterLabel"));
    this._placeholderLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%PlaceholderLabel"));
    this._portrait = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Portrait"));
    this._portraitFlash = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%PortraitFlash"));
    this._mask = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Mask"));
    this._maskOffsetY = ((Control) this._mask).OffsetTop;
    this._maskOffsetX = ((Control) this._mask).OffsetLeft;
    this._chains = ((Node) this).GetNode<NEpochChains>(NodePath.op_Implicit("%Chains"));
    this._portraitHsv = (ShaderMaterial) ((CanvasItem) this._portrait).Material;
    this._closeButton = ((Node) this).GetNode<NButton>(NodePath.op_Implicit("%CloseButton"));
    this._fancyText = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%FancyText"));
    this._closeLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%CloseLabel"));
    this._chapterLoc = new LocString("timeline", "EPOCH_INSPECT.chapterFormat");
    this._unlockInfo = ((Node) this).GetNode<NUnlockInfo>(NodePath.op_Implicit("%UnlockInfo"));
    this._nextChapterButton = ((Node) this).GetNode<NEpochPaginateButton>(NodePath.op_Implicit("%NextChapterButton"));
    this._prevChapterButton = ((Node) this).GetNode<NEpochPaginateButton>(NodePath.op_Implicit("%PrevChapterButton"));
    this._prevChapterButtonOffsetX = this._prevChapterButton.OffsetLeft;
    this._nextChapterButtonOffsetX = this._nextChapterButton.OffsetLeft;
    ((GodotObject) this._nextChapterButton).Connect(NClickableControl.SignalName.MouseReleased, Callable.From<InputEvent>((Action<InputEvent>) (_ => this.NextChapter())), 0U);
    ((GodotObject) this._prevChapterButton).Connect(NClickableControl.SignalName.MouseReleased, Callable.From<InputEvent>((Action<InputEvent>) (_ => this.PrevChapter())), 0U);
    ((GodotObject) this).Connect(NClickableControl.SignalName.MouseReleased, Callable.From<InputEvent>(new Action<InputEvent>(this.OnMouseReleased)), 0U);
    this._closeButton.Disable();
  }

  public async Task Open(NEpochSlot slot, EpochModel epoch, bool wasRevealed)
  {
    Tween buttonTween = this._buttonTween;
    if (buttonTween != null)
      buttonTween.FastForwardToCompletion();
    this._wasRevealed = wasRevealed;
    this._epoch = epoch;
    if (!this._epoch.HasRealPortrait)
    {
      ((CanvasItem) this._placeholderLabel).Visible = true;
      this._placeholderLabel.Text = NEpochInspectScreen._placeholderLoc.GetRawText();
    }
    else
      ((CanvasItem) this._placeholderLabel).Visible = false;
    ((CanvasItem) this).Modulate = Colors.White;
    this._portrait.Texture = epoch.RealPortrait;
    ((CanvasItem) this._fancyText).Modulate = StsColors.transparentWhite;
    this._fancyText.Text = epoch.Description;
    this._hasStory = epoch.StoryTitle != null;
    SfxCmd.Play("event:/sfx/ui/timeline/ui_timeline_open_epoch");
    if (this._hasStory)
    {
      this._storyLabel.SetTextAutoSize(epoch.StoryTitle ?? string.Empty);
      this._chapterLoc.Add("ChapterIndex", (Decimal) epoch.ChapterIndex);
      this._chapterLoc.Add("ChapterName", epoch.Title);
      this._chapterLabel.SetTextAutoSize(this._chapterLoc.GetFormattedText());
      this._chapterLabel.VerticalAlignment = (VerticalAlignment) 1L;
      this._nextChapterButton.Enable();
      this._prevChapterButton.Enable();
    }
    else
    {
      this._storyLabel.SetTextAutoSize(string.Empty);
      this._chapterLabel.SetTextAutoSize(epoch.Title.GetFormattedText());
      this._chapterLabel.VerticalAlignment = (VerticalAlignment) 2L;
      this._nextChapterButton.Disable();
      this._prevChapterButton.Disable();
    }
    this._closeButton.MouseFilter = (Control.MouseFilterEnum) 0L;
    this._closeButton.Scale = Vector2.One;
    this._closeButtonY = this._closeButton.Position.Y + 180f;
    this._closeButton.Position = new Vector2(this._closeButton.Position.X, this._closeButtonY);
    ((CanvasItem) this._closeButton).Modulate = StsColors.transparentWhite;
    this._closeButton.Disable();
    NTimelineScreen.Instance.ShowBackstopAndHideUi();
    ((CanvasItem) this).Visible = true;
    Vector2 size1 = ((Control) this._mask).Size;
    ((Control) this._mask).GlobalPosition = slot.GlobalPosition;
    TextureRect mask = this._mask;
    StringName size2 = Control.PropertyName.Size;
    Vector2 size3 = slot.Size;
    Transform2D globalTransform = ((CanvasItem) slot).GetGlobalTransform();
    Vector2 scale = ((Transform2D) ref globalTransform).Scale;
    Variant variant = Variant.op_Implicit(Vector2.op_Multiply(size3, scale));
    ((GodotObject) mask).SetDeferred(size2, variant);
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._mask, NodePath.op_Implicit("offset_left"), Variant.op_Implicit(this._maskOffsetX), 0.4).SetTrans((Tween.TransitionType) 7L).SetEase((Tween.EaseType) 1L);
    this._tween.TweenProperty((GodotObject) this._mask, NodePath.op_Implicit("offset_top"), Variant.op_Implicit(this._maskOffsetY), 0.4).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenProperty((GodotObject) this._mask, NodePath.op_Implicit("size"), Variant.op_Implicit(size1), 0.4).SetTrans((Tween.TransitionType) 7L).SetEase((Tween.EaseType) 1L);
    ((CanvasItem) this._storyLabel).Modulate = StsColors.transparentWhite;
    ((CanvasItem) this._chapterLabel).Modulate = StsColors.transparentWhite;
    ((CanvasItem) this._nextChapterButton).Modulate = StsColors.transparentWhite;
    ((CanvasItem) this._prevChapterButton).Modulate = StsColors.transparentWhite;
    this._tween.TweenProperty((GodotObject) this._storyLabel, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.5).SetDelay(0.4);
    this._tween.TweenProperty((GodotObject) this._chapterLabel, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.5).SetDelay(0.2);
    this._tween.TweenProperty((GodotObject) this._prevChapterButton, NodePath.op_Implicit("offset_left"), Variant.op_Implicit(this._prevChapterButtonOffsetX), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L).From(Variant.op_Implicit(this._prevChapterButtonOffsetX + 100f)).SetDelay(0.25);
    this._tween.TweenProperty((GodotObject) this._prevChapterButton, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.25).SetDelay(0.25);
    this._tween.TweenProperty((GodotObject) this._nextChapterButton, NodePath.op_Implicit("offset_left"), Variant.op_Implicit(this._nextChapterButtonOffsetX), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L).From(Variant.op_Implicit(this._nextChapterButtonOffsetX - 100f)).SetDelay(0.25);
    this._tween.TweenProperty((GodotObject) this._nextChapterButton, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.25).SetDelay(0.25);
    if (wasRevealed)
    {
      await TaskHelper.RunSafely(this.UnlockAnimation(epoch));
    }
    else
    {
      this._closeLabel.SetTextAutoSize(new LocString("timeline", "EPOCH_INSPECT.closeButton").GetRawText());
      this._fancyText.Text = epoch.Description;
      this._textTween?.Kill();
      this._textTween = ((Node) this).CreateTween().SetParallel(true);
      this._textTween.TweenProperty((GodotObject) this._fancyText, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.5).SetDelay(0.1);
      this._fancyText.VisibleRatio = 1f;
      this._buttonTween?.Kill();
      this._buttonTween = ((Node) this).CreateTween().SetParallel(true);
      this._buttonTween.TweenProperty((GodotObject) this._closeButton, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.3).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L).SetDelay(0.1);
      this._buttonTween.TweenProperty((GodotObject) this._closeButton, NodePath.op_Implicit("position:y"), Variant.op_Implicit(this._closeButtonY - 180f), 0.3).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L).SetDelay(0.1);
      this._buttonTween.TweenCallback(Callable.From(new Action(((NClickableControl) this._closeButton).Enable)));
      this.RefreshChapterPaginators();
      this._unlockInfo.AnimIn(epoch.UnlockText);
    }
  }

  private void HidePaginators()
  {
    this._hasStory = false;
    this._nextChapterButton.Disable();
    this._prevChapterButton.Disable();
  }

  private void OpenViaPaginator(EpochModel epoch)
  {
    this._epoch = epoch;
    ((CanvasItem) this).Modulate = Colors.White;
    this._fancyText.Text = epoch.Description;
    this._portrait.Texture = epoch.RealPortrait;
    this._hasStory = epoch.StoryTitle != null;
    ((CanvasItem) this._storyLabel).Modulate = Colors.White;
    ((CanvasItem) this._chapterLabel).Modulate = Colors.White;
    if (!this._epoch.HasRealPortrait)
    {
      ((CanvasItem) this._placeholderLabel).Visible = true;
      this._placeholderLabel.Text = NEpochInspectScreen._placeholderLoc.GetRawText();
    }
    else
      ((CanvasItem) this._placeholderLabel).Visible = false;
    if (this._hasStory)
    {
      this._storyLabel.SetTextAutoSize(epoch.StoryTitle ?? string.Empty);
      this._chapterLoc.Add("ChapterIndex", (Decimal) epoch.ChapterIndex);
      this._chapterLoc.Add("ChapterName", epoch.Title);
      this._chapterLabel.SetTextAutoSize(this._chapterLoc.GetFormattedText());
      this._chapterLabel.VerticalAlignment = (VerticalAlignment) 1L;
      this._nextChapterButton.Enable();
      this._prevChapterButton.Enable();
    }
    else
    {
      this._storyLabel.SetTextAutoSize(string.Empty);
      this._chapterLabel.SetTextAutoSize(epoch.Title.GetFormattedText());
      this._chapterLabel.VerticalAlignment = (VerticalAlignment) 2L;
      this._nextChapterButton.Disable();
      this._prevChapterButton.Disable();
    }
    ((CanvasItem) this._fancyText).Modulate = StsColors.transparentWhite;
    NTimelineScreen.Instance.ShowBackstopAndHideUi();
    ((CanvasItem) this).Visible = true;
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._textTween?.Kill();
    this._textTween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._mask, NodePath.op_Implicit("offset_top"), Variant.op_Implicit(this._maskOffsetY), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._textTween.TweenProperty((GodotObject) this._fancyText, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.5).SetDelay(0.1);
    this._fancyText.VisibleRatio = 1f;
    TaskHelper.RunSafely(this._unlockInfo.AnimInViaPaginator(epoch.UnlockText));
  }

  public void Close()
  {
    if (NTimelineScreen.Instance.IsScreenQueued())
    {
      NTimelineScreen.Instance.OpenQueuedScreen();
    }
    else
    {
      NTimelineScreen.Instance.EnableInput();
      NTimelineScreen.Instance.HideBackstopAndShowUi(true);
    }
    this.FocusMode = (Control.FocusModeEnum) 0L;
    Tween buttonTween = this._buttonTween;
    if (buttonTween != null)
      buttonTween.FastForwardToCompletion();
    Tween unlockTween = this._unlockTween;
    if (unlockTween != null)
      unlockTween.FastForwardToCompletion();
    Tween tween = this._tween;
    if (tween != null)
      tween.FastForwardToCompletion();
    Tween textTween = this._textTween;
    if (textTween != null)
      textTween.FastForwardToCompletion();
    this._buttonTween = ((Node) this).CreateTween().SetParallel(true);
    this._buttonTween.TweenProperty((GodotObject) this._closeButton, NodePath.op_Implicit("scale"), Variant.op_Implicit(new Vector2(3f, 0.1f)), 0.3).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._buttonTween.TweenProperty((GodotObject) this._closeButton, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    this._buttonTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate"), Variant.op_Implicit(new Color(0.0f, 0.0f, 0.0f, 0.0f)), 0.5);
    this._buttonTween.TweenCallback(Callable.From((Action) (() =>
    {
      ((CanvasItem) this).Visible = false;
      this._closeButton.Disable();
      if (!this._wasRevealed)
        return;
      AchievementsHelper.CheckTimelineComplete();
    })));
    NHotkeyManager.Instance.RemoveHotkeyPressedBinding(StringName.op_Implicit(MegaInput.right), new Action(this.NextChapter));
    NHotkeyManager.Instance.RemoveHotkeyPressedBinding(StringName.op_Implicit(MegaInput.left), new Action(this.PrevChapter));
  }

  public async Task UnlockAnimation(EpochModel epoch)
  {
    this.HidePaginators();
    epoch.QueueUnlocks();
    SaveManager.Instance.SaveProgressFile();
    this._unlockInfo.HideImmediately();
    this._closeLabel.SetTextAutoSize(new LocString("timeline", "EPOCH_INSPECT.continueButton").GetRawText());
    this._fancyText.VisibleRatio = 0.0f;
    ((CanvasItem) this._fancyText).Modulate = StsColors.transparentWhite;
    this._portraitHsv.SetShaderParameter(NEpochInspectScreen._s, Variant.op_Implicit(0.0f));
    this._portraitHsv.SetShaderParameter(NEpochInspectScreen._v, Variant.op_Implicit(0.75f));
    this._chains.Texture = PreloadManager.Cache.GetTexture2D(NEpochInspectScreen.lockedImagePath);
    ((CanvasItem) this._chains).Visible = true;
    ((CanvasItem) this._chains).Modulate = Colors.White;
    ((CanvasItem) this._chains).SelfModulate = Colors.White;
    ((CanvasItem) this._portraitFlash).Modulate = new Color(1f, 1f, 1f, 0.0f);
    this._unlockTween?.Kill();
    this._unlockTween = ((Node) this).CreateTween().SetParallel(true);
    this._unlockTween.TweenProperty((GodotObject) this._chains, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 0.98f)), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L).SetDelay(0.5);
    if (!await this._unlockTween.AwaitFinished((Node) this))
      return;
    this._chains.Unlock();
    await ((GodotObject) this._chains).AwaitSignal(NEpochChains.SignalName.OnAnimationFinished, (Node) this);
    ((CanvasItem) this._portraitFlash).Modulate = Colors.White;
    this._unlockTween = ((Node) this).CreateTween().SetParallel(true);
    this._unlockTween.TweenProperty((GodotObject) this._portraitFlash, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.5);
    this._unlockTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._portraitHsv.GetShaderParameter(NEpochInspectScreen._s), Variant.op_Implicit(1f), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._unlockTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._portraitHsv.GetShaderParameter(NEpochInspectScreen._v), Variant.op_Implicit(1f), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._textTween?.Kill();
    this._textTween = ((Node) this).CreateTween().SetParallel(true);
    this._textTween.TweenProperty((GodotObject) this._fancyText, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 2.0).SetDelay(0.25);
    this._textTween.TweenProperty((GodotObject) this._fancyText, NodePath.op_Implicit("visible_ratio"), Variant.op_Implicit(1f), (double) this._fancyText.GetTotalCharacterCount() * 0.015).SetDelay(0.5);
    this._buttonTween?.Kill();
    this._buttonTween = ((Node) this).CreateTween().SetParallel(true);
    this._buttonTween.TweenProperty((GodotObject) this._closeButton, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.3).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L).SetDelay(1.0);
    this._buttonTween.TweenProperty((GodotObject) this._closeButton, NodePath.op_Implicit("position:y"), Variant.op_Implicit(this._closeButtonY - 180f), 0.3).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L).SetDelay(1.0);
    this._buttonTween.TweenCallback(Callable.From(new Action(((NClickableControl) this._closeButton).Enable)));
    bool flag = await this._unlockTween.AwaitFinished((Node) this);
  }

  private void UpdateShaderS(float value)
  {
    this._portraitHsv.SetShaderParameter(NEpochInspectScreen._s, Variant.op_Implicit(value));
  }

  private void UpdateShaderV(float value)
  {
    this._portraitHsv.SetShaderParameter(NEpochInspectScreen._v, Variant.op_Implicit(value));
  }

  public override void _Input(InputEvent inputEvent)
  {
    if (!inputEvent.IsActionPressed(MegaInput.select, false, false) && !inputEvent.IsActionPressed(MegaInput.accept, false, false))
      return;
    this.SpeedUpTextAnimation();
  }

  private void OnMouseReleased(InputEvent obj) => this.SpeedUpTextAnimation();

  private void SpeedUpTextAnimation()
  {
    if (this._textTween == null || !this._textTween.IsRunning())
      return;
    this._textTween.Kill();
    ((CanvasItem) this._fancyText).Modulate = Colors.White;
    this._fancyText.VisibleRatio = 1f;
  }

  private void NextChapter()
  {
    this.OpenViaPaginator(this._nextChapterEpoch);
    this.RefreshChapterPaginators();
  }

  private void PrevChapter()
  {
    this.OpenViaPaginator(this._prevChapterEpoch);
    this.RefreshChapterPaginators();
  }

  private void RefreshChapterPaginators()
  {
    if (!this._hasStory)
      return;
    this._nextChapterEpoch = StoryModel.NextChapter(this._epoch);
    this._prevChapterEpoch = StoryModel.PrevChapter(this._epoch);
    if (this._nextChapterEpoch != null)
    {
      NHotkeyManager.Instance.PushHotkeyPressedBinding(StringName.op_Implicit(MegaInput.right), new Action(this.NextChapter));
      ((CanvasItem) this._nextChapterButton).Visible = true;
    }
    else
      ((CanvasItem) this._nextChapterButton).Visible = false;
    if (this._prevChapterEpoch != null)
    {
      NHotkeyManager.Instance.PushHotkeyPressedBinding(StringName.op_Implicit(MegaInput.left), new Action(this.PrevChapter));
      ((CanvasItem) this._prevChapterButton).Visible = true;
    }
    else
      ((CanvasItem) this._prevChapterButton).Visible = false;
  }

  public Control? DefaultFocusedControl => (Control) null;

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(11)
    {
      new MethodInfo(NEpochInspectScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEpochInspectScreen.MethodName.HidePaginators, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEpochInspectScreen.MethodName.Close, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEpochInspectScreen.MethodName.UpdateShaderS, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("value"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NEpochInspectScreen.MethodName.UpdateShaderV, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("value"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NEpochInspectScreen.MethodName._Input, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NEpochInspectScreen.MethodName.OnMouseReleased, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("obj"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NEpochInspectScreen.MethodName.SpeedUpTextAnimation, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEpochInspectScreen.MethodName.NextChapter, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEpochInspectScreen.MethodName.PrevChapter, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEpochInspectScreen.MethodName.RefreshChapterPaginators, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NEpochInspectScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEpochInspectScreen.MethodName.HidePaginators) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.HidePaginators();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEpochInspectScreen.MethodName.Close) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Close();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEpochInspectScreen.MethodName.UpdateShaderS) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateShaderS(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEpochInspectScreen.MethodName.UpdateShaderV) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateShaderV(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEpochInspectScreen.MethodName._Input) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Input(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEpochInspectScreen.MethodName.OnMouseReleased) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnMouseReleased(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEpochInspectScreen.MethodName.SpeedUpTextAnimation) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SpeedUpTextAnimation();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEpochInspectScreen.MethodName.NextChapter) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.NextChapter();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEpochInspectScreen.MethodName.PrevChapter) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.PrevChapter();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NEpochInspectScreen.MethodName.RefreshChapterPaginators) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.RefreshChapterPaginators();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NEpochInspectScreen.MethodName._Ready) || StringName.op_Equality(ref method, NEpochInspectScreen.MethodName.HidePaginators) || StringName.op_Equality(ref method, NEpochInspectScreen.MethodName.Close) || StringName.op_Equality(ref method, NEpochInspectScreen.MethodName.UpdateShaderS) || StringName.op_Equality(ref method, NEpochInspectScreen.MethodName.UpdateShaderV) || StringName.op_Equality(ref method, NEpochInspectScreen.MethodName._Input) || StringName.op_Equality(ref method, NEpochInspectScreen.MethodName.OnMouseReleased) || StringName.op_Equality(ref method, NEpochInspectScreen.MethodName.SpeedUpTextAnimation) || StringName.op_Equality(ref method, NEpochInspectScreen.MethodName.NextChapter) || StringName.op_Equality(ref method, NEpochInspectScreen.MethodName.PrevChapter) || StringName.op_Equality(ref method, NEpochInspectScreen.MethodName.RefreshChapterPaginators) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._closeButton))
    {
      this._closeButton = VariantUtils.ConvertTo<NButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._portrait))
    {
      this._portrait = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._portraitFlash))
    {
      this._portraitFlash = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._mask))
    {
      this._mask = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._chains))
    {
      this._chains = VariantUtils.ConvertTo<NEpochChains>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._portraitHsv))
    {
      this._portraitHsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._fancyText))
    {
      this._fancyText = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._storyLabel))
    {
      this._storyLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._chapterLabel))
    {
      this._chapterLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._closeLabel))
    {
      this._closeLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._placeholderLabel))
    {
      this._placeholderLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._nextChapterButton))
    {
      this._nextChapterButton = VariantUtils.ConvertTo<NEpochPaginateButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._prevChapterButton))
    {
      this._prevChapterButton = VariantUtils.ConvertTo<NEpochPaginateButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._unlockInfo))
    {
      this._unlockInfo = VariantUtils.ConvertTo<NUnlockInfo>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._hasStory))
    {
      this._hasStory = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._wasRevealed))
    {
      this._wasRevealed = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._prevChapterButtonOffsetX))
    {
      this._prevChapterButtonOffsetX = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._nextChapterButtonOffsetX))
    {
      this._nextChapterButtonOffsetX = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._maskOffsetX))
    {
      this._maskOffsetX = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._maskOffsetY))
    {
      this._maskOffsetY = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._closeButtonY))
    {
      this._closeButtonY = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._unlockTween))
    {
      this._unlockTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._buttonTween))
    {
      this._buttonTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._textTween))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._textTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._closeButton))
    {
      value = VariantUtils.CreateFrom<NButton>(ref this._closeButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._portrait))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._portrait);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._portraitFlash))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._portraitFlash);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._mask))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._mask);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._chains))
    {
      value = VariantUtils.CreateFrom<NEpochChains>(ref this._chains);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._portraitHsv))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._portraitHsv);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._fancyText))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._fancyText);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._storyLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._storyLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._chapterLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._chapterLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._closeLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._closeLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._placeholderLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._placeholderLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._nextChapterButton))
    {
      value = VariantUtils.CreateFrom<NEpochPaginateButton>(ref this._nextChapterButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._prevChapterButton))
    {
      value = VariantUtils.CreateFrom<NEpochPaginateButton>(ref this._prevChapterButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._unlockInfo))
    {
      value = VariantUtils.CreateFrom<NUnlockInfo>(ref this._unlockInfo);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._hasStory))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._hasStory);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._wasRevealed))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._wasRevealed);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._prevChapterButtonOffsetX))
    {
      value = VariantUtils.CreateFrom<float>(ref this._prevChapterButtonOffsetX);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._nextChapterButtonOffsetX))
    {
      value = VariantUtils.CreateFrom<float>(ref this._nextChapterButtonOffsetX);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._maskOffsetX))
    {
      value = VariantUtils.CreateFrom<float>(ref this._maskOffsetX);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._maskOffsetY))
    {
      value = VariantUtils.CreateFrom<float>(ref this._maskOffsetY);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._closeButtonY))
    {
      value = VariantUtils.CreateFrom<float>(ref this._closeButtonY);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._unlockTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._unlockTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._buttonTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._buttonTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NEpochInspectScreen.PropertyName._textTween))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Tween>(ref this._textTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NEpochInspectScreen.PropertyName._closeButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochInspectScreen.PropertyName._portrait, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochInspectScreen.PropertyName._portraitFlash, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochInspectScreen.PropertyName._mask, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochInspectScreen.PropertyName._chains, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochInspectScreen.PropertyName._portraitHsv, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochInspectScreen.PropertyName._fancyText, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochInspectScreen.PropertyName._storyLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochInspectScreen.PropertyName._chapterLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochInspectScreen.PropertyName._closeLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochInspectScreen.PropertyName._placeholderLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochInspectScreen.PropertyName._nextChapterButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochInspectScreen.PropertyName._prevChapterButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochInspectScreen.PropertyName._unlockInfo, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NEpochInspectScreen.PropertyName._hasStory, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NEpochInspectScreen.PropertyName._wasRevealed, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NEpochInspectScreen.PropertyName._prevChapterButtonOffsetX, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NEpochInspectScreen.PropertyName._nextChapterButtonOffsetX, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NEpochInspectScreen.PropertyName._maskOffsetX, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NEpochInspectScreen.PropertyName._maskOffsetY, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NEpochInspectScreen.PropertyName._closeButtonY, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochInspectScreen.PropertyName._unlockTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochInspectScreen.PropertyName._buttonTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochInspectScreen.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochInspectScreen.PropertyName._textTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochInspectScreen.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NEpochInspectScreen.PropertyName._closeButton, Variant.From<NButton>(ref this._closeButton));
    info.AddProperty(NEpochInspectScreen.PropertyName._portrait, Variant.From<TextureRect>(ref this._portrait));
    info.AddProperty(NEpochInspectScreen.PropertyName._portraitFlash, Variant.From<TextureRect>(ref this._portraitFlash));
    info.AddProperty(NEpochInspectScreen.PropertyName._mask, Variant.From<TextureRect>(ref this._mask));
    info.AddProperty(NEpochInspectScreen.PropertyName._chains, Variant.From<NEpochChains>(ref this._chains));
    info.AddProperty(NEpochInspectScreen.PropertyName._portraitHsv, Variant.From<ShaderMaterial>(ref this._portraitHsv));
    info.AddProperty(NEpochInspectScreen.PropertyName._fancyText, Variant.From<MegaRichTextLabel>(ref this._fancyText));
    info.AddProperty(NEpochInspectScreen.PropertyName._storyLabel, Variant.From<MegaLabel>(ref this._storyLabel));
    info.AddProperty(NEpochInspectScreen.PropertyName._chapterLabel, Variant.From<MegaLabel>(ref this._chapterLabel));
    info.AddProperty(NEpochInspectScreen.PropertyName._closeLabel, Variant.From<MegaLabel>(ref this._closeLabel));
    info.AddProperty(NEpochInspectScreen.PropertyName._placeholderLabel, Variant.From<MegaLabel>(ref this._placeholderLabel));
    info.AddProperty(NEpochInspectScreen.PropertyName._nextChapterButton, Variant.From<NEpochPaginateButton>(ref this._nextChapterButton));
    info.AddProperty(NEpochInspectScreen.PropertyName._prevChapterButton, Variant.From<NEpochPaginateButton>(ref this._prevChapterButton));
    info.AddProperty(NEpochInspectScreen.PropertyName._unlockInfo, Variant.From<NUnlockInfo>(ref this._unlockInfo));
    info.AddProperty(NEpochInspectScreen.PropertyName._hasStory, Variant.From<bool>(ref this._hasStory));
    info.AddProperty(NEpochInspectScreen.PropertyName._wasRevealed, Variant.From<bool>(ref this._wasRevealed));
    info.AddProperty(NEpochInspectScreen.PropertyName._prevChapterButtonOffsetX, Variant.From<float>(ref this._prevChapterButtonOffsetX));
    info.AddProperty(NEpochInspectScreen.PropertyName._nextChapterButtonOffsetX, Variant.From<float>(ref this._nextChapterButtonOffsetX));
    info.AddProperty(NEpochInspectScreen.PropertyName._maskOffsetX, Variant.From<float>(ref this._maskOffsetX));
    info.AddProperty(NEpochInspectScreen.PropertyName._maskOffsetY, Variant.From<float>(ref this._maskOffsetY));
    info.AddProperty(NEpochInspectScreen.PropertyName._closeButtonY, Variant.From<float>(ref this._closeButtonY));
    info.AddProperty(NEpochInspectScreen.PropertyName._unlockTween, Variant.From<Tween>(ref this._unlockTween));
    info.AddProperty(NEpochInspectScreen.PropertyName._buttonTween, Variant.From<Tween>(ref this._buttonTween));
    info.AddProperty(NEpochInspectScreen.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NEpochInspectScreen.PropertyName._textTween, Variant.From<Tween>(ref this._textTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NEpochInspectScreen.PropertyName._closeButton, ref variant1))
      this._closeButton = ((Variant) ref variant1).As<NButton>();
    Variant variant2;
    if (info.TryGetProperty(NEpochInspectScreen.PropertyName._portrait, ref variant2))
      this._portrait = ((Variant) ref variant2).As<TextureRect>();
    Variant variant3;
    if (info.TryGetProperty(NEpochInspectScreen.PropertyName._portraitFlash, ref variant3))
      this._portraitFlash = ((Variant) ref variant3).As<TextureRect>();
    Variant variant4;
    if (info.TryGetProperty(NEpochInspectScreen.PropertyName._mask, ref variant4))
      this._mask = ((Variant) ref variant4).As<TextureRect>();
    Variant variant5;
    if (info.TryGetProperty(NEpochInspectScreen.PropertyName._chains, ref variant5))
      this._chains = ((Variant) ref variant5).As<NEpochChains>();
    Variant variant6;
    if (info.TryGetProperty(NEpochInspectScreen.PropertyName._portraitHsv, ref variant6))
      this._portraitHsv = ((Variant) ref variant6).As<ShaderMaterial>();
    Variant variant7;
    if (info.TryGetProperty(NEpochInspectScreen.PropertyName._fancyText, ref variant7))
      this._fancyText = ((Variant) ref variant7).As<MegaRichTextLabel>();
    Variant variant8;
    if (info.TryGetProperty(NEpochInspectScreen.PropertyName._storyLabel, ref variant8))
      this._storyLabel = ((Variant) ref variant8).As<MegaLabel>();
    Variant variant9;
    if (info.TryGetProperty(NEpochInspectScreen.PropertyName._chapterLabel, ref variant9))
      this._chapterLabel = ((Variant) ref variant9).As<MegaLabel>();
    Variant variant10;
    if (info.TryGetProperty(NEpochInspectScreen.PropertyName._closeLabel, ref variant10))
      this._closeLabel = ((Variant) ref variant10).As<MegaLabel>();
    Variant variant11;
    if (info.TryGetProperty(NEpochInspectScreen.PropertyName._placeholderLabel, ref variant11))
      this._placeholderLabel = ((Variant) ref variant11).As<MegaLabel>();
    Variant variant12;
    if (info.TryGetProperty(NEpochInspectScreen.PropertyName._nextChapterButton, ref variant12))
      this._nextChapterButton = ((Variant) ref variant12).As<NEpochPaginateButton>();
    Variant variant13;
    if (info.TryGetProperty(NEpochInspectScreen.PropertyName._prevChapterButton, ref variant13))
      this._prevChapterButton = ((Variant) ref variant13).As<NEpochPaginateButton>();
    Variant variant14;
    if (info.TryGetProperty(NEpochInspectScreen.PropertyName._unlockInfo, ref variant14))
      this._unlockInfo = ((Variant) ref variant14).As<NUnlockInfo>();
    Variant variant15;
    if (info.TryGetProperty(NEpochInspectScreen.PropertyName._hasStory, ref variant15))
      this._hasStory = ((Variant) ref variant15).As<bool>();
    Variant variant16;
    if (info.TryGetProperty(NEpochInspectScreen.PropertyName._wasRevealed, ref variant16))
      this._wasRevealed = ((Variant) ref variant16).As<bool>();
    Variant variant17;
    if (info.TryGetProperty(NEpochInspectScreen.PropertyName._prevChapterButtonOffsetX, ref variant17))
      this._prevChapterButtonOffsetX = ((Variant) ref variant17).As<float>();
    Variant variant18;
    if (info.TryGetProperty(NEpochInspectScreen.PropertyName._nextChapterButtonOffsetX, ref variant18))
      this._nextChapterButtonOffsetX = ((Variant) ref variant18).As<float>();
    Variant variant19;
    if (info.TryGetProperty(NEpochInspectScreen.PropertyName._maskOffsetX, ref variant19))
      this._maskOffsetX = ((Variant) ref variant19).As<float>();
    Variant variant20;
    if (info.TryGetProperty(NEpochInspectScreen.PropertyName._maskOffsetY, ref variant20))
      this._maskOffsetY = ((Variant) ref variant20).As<float>();
    Variant variant21;
    if (info.TryGetProperty(NEpochInspectScreen.PropertyName._closeButtonY, ref variant21))
      this._closeButtonY = ((Variant) ref variant21).As<float>();
    Variant variant22;
    if (info.TryGetProperty(NEpochInspectScreen.PropertyName._unlockTween, ref variant22))
      this._unlockTween = ((Variant) ref variant22).As<Tween>();
    Variant variant23;
    if (info.TryGetProperty(NEpochInspectScreen.PropertyName._buttonTween, ref variant23))
      this._buttonTween = ((Variant) ref variant23).As<Tween>();
    Variant variant24;
    if (info.TryGetProperty(NEpochInspectScreen.PropertyName._tween, ref variant24))
      this._tween = ((Variant) ref variant24).As<Tween>();
    Variant variant25;
    if (!info.TryGetProperty(NEpochInspectScreen.PropertyName._textTween, ref variant25))
      return;
    this._textTween = ((Variant) ref variant25).As<Tween>();
  }

  public new class MethodName : NClickableControl.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName HidePaginators = StringName.op_Implicit(nameof (HidePaginators));
    public static readonly StringName Close = StringName.op_Implicit(nameof (Close));
    public static readonly StringName UpdateShaderS = StringName.op_Implicit(nameof (UpdateShaderS));
    public static readonly StringName UpdateShaderV = StringName.op_Implicit(nameof (UpdateShaderV));
    public static readonly StringName _Input = StringName.op_Implicit(nameof (_Input));
    public static readonly StringName OnMouseReleased = StringName.op_Implicit(nameof (OnMouseReleased));
    public static readonly StringName SpeedUpTextAnimation = StringName.op_Implicit(nameof (SpeedUpTextAnimation));
    public static readonly StringName NextChapter = StringName.op_Implicit(nameof (NextChapter));
    public static readonly StringName PrevChapter = StringName.op_Implicit(nameof (PrevChapter));
    public static readonly StringName RefreshChapterPaginators = StringName.op_Implicit(nameof (RefreshChapterPaginators));
  }

  public new class PropertyName : NClickableControl.PropertyName
  {
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _closeButton = StringName.op_Implicit(nameof (_closeButton));
    public static readonly StringName _portrait = StringName.op_Implicit(nameof (_portrait));
    public static readonly StringName _portraitFlash = StringName.op_Implicit(nameof (_portraitFlash));
    public static readonly StringName _mask = StringName.op_Implicit(nameof (_mask));
    public static readonly StringName _chains = StringName.op_Implicit(nameof (_chains));
    public static readonly StringName _portraitHsv = StringName.op_Implicit(nameof (_portraitHsv));
    public static readonly StringName _fancyText = StringName.op_Implicit(nameof (_fancyText));
    public static readonly StringName _storyLabel = StringName.op_Implicit(nameof (_storyLabel));
    public static readonly StringName _chapterLabel = StringName.op_Implicit(nameof (_chapterLabel));
    public static readonly StringName _closeLabel = StringName.op_Implicit(nameof (_closeLabel));
    public static readonly StringName _placeholderLabel = StringName.op_Implicit(nameof (_placeholderLabel));
    public static readonly StringName _nextChapterButton = StringName.op_Implicit(nameof (_nextChapterButton));
    public static readonly StringName _prevChapterButton = StringName.op_Implicit(nameof (_prevChapterButton));
    public static readonly StringName _unlockInfo = StringName.op_Implicit(nameof (_unlockInfo));
    public static readonly StringName _hasStory = StringName.op_Implicit(nameof (_hasStory));
    public static readonly StringName _wasRevealed = StringName.op_Implicit(nameof (_wasRevealed));
    public static readonly StringName _prevChapterButtonOffsetX = StringName.op_Implicit(nameof (_prevChapterButtonOffsetX));
    public static readonly StringName _nextChapterButtonOffsetX = StringName.op_Implicit(nameof (_nextChapterButtonOffsetX));
    public static readonly StringName _maskOffsetX = StringName.op_Implicit(nameof (_maskOffsetX));
    public static readonly StringName _maskOffsetY = StringName.op_Implicit(nameof (_maskOffsetY));
    public static readonly StringName _closeButtonY = StringName.op_Implicit(nameof (_closeButtonY));
    public static readonly StringName _unlockTween = StringName.op_Implicit(nameof (_unlockTween));
    public static readonly StringName _buttonTween = StringName.op_Implicit(nameof (_buttonTween));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _textTween = StringName.op_Implicit(nameof (_textTween));
  }

  public new class SignalName : NClickableControl.SignalName
  {
  }
}
