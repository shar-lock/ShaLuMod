// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.CardSelection.NChooseACardSelectionScreen
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;

[ScriptPath("res://src/Core/Nodes/Screens/CardSelection/NChooseACardSelectionScreen.cs")]
public class NChooseACardSelectionScreen : Control, IOverlayScreen, IScreenContext, ICardSelector
{
  private const float _cardXSpacing = 340f;
  private const ulong _noSelectionTimeMsec = 350;
  private NCommonBanner _banner;
  private Control _cardRow;
  private NChoiceSelectionSkipButton _skipButton;
  private NCombatPilesContainer _combatPiles;
  private Control _inspectPrompt;
  private NPeekButton _peekButton;
  private readonly TaskCompletionSource<IEnumerable<CardModel>> _completionSource = new TaskCompletionSource<IEnumerable<CardModel>>();
  private ulong _openedTicks;
  private bool _screenComplete;
  private bool _cardSelected;
  private bool _canSkip;
  private Tween? _cardTween;
  private Tween? _fadeTween;
  private IReadOnlyList<CardModel> _cards;

  private static string ScenePath
  {
    get => SceneHelper.GetScenePath("screens/card_selection/choose_a_card_selection_screen");
  }

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NChooseACardSelectionScreen.ScenePath);
    }
  }

  public NetScreenType ScreenType => NetScreenType.CardSelection;

  public static NChooseACardSelectionScreen? ShowScreen(
    IReadOnlyList<CardModel> cards,
    bool canSkip)
  {
    if (TestMode.IsOn)
      return (NChooseACardSelectionScreen) null;
    NChooseACardSelectionScreen screen = PreloadManager.Cache.GetScene(NChooseACardSelectionScreen.ScenePath).Instantiate<NChooseACardSelectionScreen>((PackedScene.GenEditState) 0L);
    ((Node) screen).Name = StringName.op_Implicit(nameof (NChooseACardSelectionScreen));
    screen._cards = cards;
    screen._canSkip = canSkip;
    NOverlayStack.Instance.Push((IOverlayScreen) screen);
    return screen;
  }

  public override void _Ready()
  {
    this._banner = ((Node) this).GetNode<NCommonBanner>(NodePath.op_Implicit("Banner"));
    this._banner.label.SetTextAutoSize(new LocString("gameplay_ui", "CHOOSE_CARD_HEADER").GetRawText());
    this._banner.AnimateIn();
    this._cardRow = ((Node) this).GetNode<Control>(NodePath.op_Implicit("CardRow"));
    this._combatPiles = ((Node) this).GetNode<NCombatPilesContainer>(NodePath.op_Implicit("%CombatPiles"));
    if (CombatManager.Instance.IsInProgress)
      this._combatPiles.Initialize(this._cards.First<CardModel>().Owner);
    this._combatPiles.Disable();
    ((CanvasItem) this._combatPiles).Visible = false;
    this._inspectPrompt = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%InspectPrompt"));
    Vector2 vector2 = Vector2.op_Multiply(Vector2.op_Multiply(Vector2.op_Multiply(Vector2.Left, (float) (this._cards.Count - 1)), 340f), 0.5f);
    this._cardTween = ((Node) this).CreateTween().SetParallel(true);
    for (int index = 0; index < this._cards.Count; ++index)
    {
      NCard cardNode = NCard.Create(this._cards[index]);
      NGridCardHolder child = NGridCardHolder.Create(cardNode);
      ((Node) this._cardRow).AddChildSafely((Node) child);
      ((GodotObject) child).Connect(NCardHolder.SignalName.Pressed, Callable.From<NCardHolder>(new Action<NCardHolder>(this.SelectHolder)), 0U);
      ((GodotObject) child).Connect(NCardHolder.SignalName.AltPressed, Callable.From<NCardHolder>(new Action<NCardHolder>(this.OpenPreviewScreen)), 0U);
      cardNode.UpdateVisuals(PileType.None, CardPreviewMode.Normal);
      child.Scale = child.SmallScale;
      this._cardTween.TweenProperty((GodotObject) child, NodePath.op_Implicit("position"), Variant.op_Implicit(Vector2.op_Addition(vector2, Vector2.op_Multiply(Vector2.op_Multiply(Vector2.Right, 340f), (float) index))), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
      this._cardTween.TweenProperty((GodotObject) child, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L).From(Variant.op_Implicit(Colors.Black));
      cardNode.ActivateRewardScreenGlow();
    }
    this._skipButton = ((Node) this).GetNode<NChoiceSelectionSkipButton>(NodePath.op_Implicit("SkipButton"));
    if (this._canSkip)
    {
      ((GodotObject) this._skipButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnSkipButtonReleased)), 0U);
      this._skipButton.AnimateIn();
    }
    else
    {
      this._skipButton.Disable();
      ((CanvasItem) this._skipButton).Visible = false;
    }
    this._peekButton = ((Node) this).GetNode<NPeekButton>(NodePath.op_Implicit("%PeekButton"));
    this._peekButton.AddTargets((Control) this._banner, this._cardRow, (Control) this._skipButton, this._inspectPrompt);
    ((GodotObject) this._peekButton).Connect(NPeekButton.SignalName.Toggled, Callable.From<NPeekButton>((Action<NPeekButton>) (_ =>
    {
      if (this._peekButton.IsPeeking)
      {
        this.MouseFilter = (Control.MouseFilterEnum) 2L;
        ((CanvasItem) this._combatPiles).Visible = true;
        this._combatPiles.Enable();
        this._skipButton.Disable();
      }
      else
      {
        this.MouseFilter = (Control.MouseFilterEnum) 0L;
        ((CanvasItem) this._combatPiles).Visible = false;
        this._combatPiles.Disable();
        if (!this._canSkip)
          return;
        this._skipButton.Enable();
      }
    })), 0U);
    for (int index = 0; index < ((Node) this._cardRow).GetChildCount(false); ++index)
    {
      Control child = ((Node) this._cardRow).GetChild<Control>(index, false);
      child.FocusNeighborBottom = ((Node) child).GetPath();
      child.FocusNeighborTop = ((Node) child).GetPath();
      child.FocusNeighborLeft = index > 0 ? ((Node) this._cardRow).GetChild(index - 1, false).GetPath() : ((Node) this._cardRow).GetChild(((Node) this._cardRow).GetChildCount(false) - 1, false).GetPath();
      child.FocusNeighborRight = index < ((Node) this._cardRow).GetChildCount(false) - 1 ? ((Node) this._cardRow).GetChild(index + 1, false).GetPath() : ((Node) this._cardRow).GetChild(0, false).GetPath();
    }
    this.UpdateControllerIcons();
    ((GodotObject) NControllerManager.Instance).Connect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.UpdateControllerIcons)), 0U);
    ((GodotObject) NControllerManager.Instance).Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.UpdateControllerIcons)), 0U);
    ((GodotObject) NInputManager.Instance).Connect(NInputManager.SignalName.InputRebound, Callable.From(new Action(this.UpdateControllerIcons)), 0U);
  }

  public override void _ExitTree()
  {
    if (!((Task) this._completionSource.Task).IsCompleted)
      this._completionSource.SetCanceled();
    foreach (Node node in ((IEnumerable) ((Node) this._cardRow).GetChildren(false)).OfType<NGridCardHolder>())
      node.QueueFreeSafely();
  }

  private void SelectHolder(NCardHolder cardHolder)
  {
    if (this._completionSource == null)
      throw new InvalidOperationException("CardsSelected must be awaited before a card is selected!");
    if (Time.GetTicksMsec() - this._openedTicks <= 350UL)
      return;
    CardModel cardModel = cardHolder.CardModel;
    this._screenComplete = true;
    this._cardSelected = true;
    this._completionSource.SetResult((IEnumerable<CardModel>) new CardModel[1]
    {
      cardModel
    });
  }

  private void OpenPreviewScreen(NCardHolder cardHolder)
  {
    NInspectCardScreen inspectCardScreen = NGame.Instance.GetInspectCardScreen();
    int capacity = 1;
    List<CardModel> cards = new List<CardModel>(capacity);
    CollectionsMarshal.SetCount<CardModel>(cards, capacity);
    CollectionsMarshal.AsSpan<CardModel>(cards)[0] = cardHolder.CardModel;
    inspectCardScreen.Open(cards, 0);
  }

  public async Task<IEnumerable<CardModel>> CardsSelected()
  {
    IEnumerable<CardModel> task = await this._completionSource.Task;
    NOverlayStack.Instance.Remove((IOverlayScreen) this);
    return task;
  }

  private void OnSkipButtonReleased(NButton _)
  {
    this._screenComplete = true;
    this._completionSource.SetResult((IEnumerable<CardModel>) Array.Empty<CardModel>());
  }

  public void AfterOverlayOpened()
  {
    ((CanvasItem) this).Modulate = Colors.Transparent;
    this._openedTicks = Time.GetTicksMsec();
    this._fadeTween?.Kill();
    this._fadeTween = ((Node) this).CreateTween();
    this._fadeTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.2);
  }

  public void AfterOverlayClosed()
  {
    this._fadeTween?.Kill();
    this._peekButton.SetPeeking(false);
    ((Node) this).QueueFreeSafely();
  }

  public void AfterOverlayShown()
  {
    ((CanvasItem) this).Visible = true;
    if (CombatManager.Instance.IsInProgress)
      this._peekButton.Enable();
    if (!this._canSkip || this._peekButton.IsPeeking)
      return;
    this._skipButton.Enable();
  }

  public void AfterOverlayHidden()
  {
    this._peekButton.Disable();
    this._skipButton.Disable();
    ((CanvasItem) this).Visible = false;
  }

  public bool UseSharedBackstop => true;

  public Control DefaultFocusedControl
  {
    get
    {
      if (this._peekButton.IsPeeking && NCombatRoom.Instance != null)
        return NCombatRoom.Instance.DefaultFocusedControl;
      List<NGridCardHolder> list = ((IEnumerable) ((Node) this._cardRow).GetChildren(false)).OfType<NGridCardHolder>().ToList<NGridCardHolder>();
      return (Control) list[list.Count / 2];
    }
  }

  private void UpdateControllerIcons()
  {
    ((CanvasItem) this._inspectPrompt).Modulate = NControllerManager.Instance.IsUsingController ? Colors.White : Colors.Transparent;
    ((Node) this._inspectPrompt).GetNode<TextureRect>(NodePath.op_Implicit("ControllerIcon")).Texture = NInputManager.Instance.GetHotkeyIcon(StringName.op_Implicit(MegaInput.accept));
    ((Node) this._inspectPrompt).GetNode<MegaLabel>(NodePath.op_Implicit("Label")).SetTextAutoSize(new LocString("gameplay_ui", "TO_INSPECT_PROMPT").GetFormattedText());
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(10)
    {
      new MethodInfo(NChooseACardSelectionScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NChooseACardSelectionScreen.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NChooseACardSelectionScreen.MethodName.SelectHolder, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("cardHolder"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NChooseACardSelectionScreen.MethodName.OpenPreviewScreen, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("cardHolder"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NChooseACardSelectionScreen.MethodName.OnSkipButtonReleased, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NChooseACardSelectionScreen.MethodName.AfterOverlayOpened, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NChooseACardSelectionScreen.MethodName.AfterOverlayClosed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NChooseACardSelectionScreen.MethodName.AfterOverlayShown, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NChooseACardSelectionScreen.MethodName.AfterOverlayHidden, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NChooseACardSelectionScreen.MethodName.UpdateControllerIcons, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NChooseACardSelectionScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NChooseACardSelectionScreen.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NChooseACardSelectionScreen.MethodName.SelectHolder) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SelectHolder(VariantUtils.ConvertTo<NCardHolder>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NChooseACardSelectionScreen.MethodName.OpenPreviewScreen) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OpenPreviewScreen(VariantUtils.ConvertTo<NCardHolder>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NChooseACardSelectionScreen.MethodName.OnSkipButtonReleased) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnSkipButtonReleased(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NChooseACardSelectionScreen.MethodName.AfterOverlayOpened) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AfterOverlayOpened();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NChooseACardSelectionScreen.MethodName.AfterOverlayClosed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AfterOverlayClosed();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NChooseACardSelectionScreen.MethodName.AfterOverlayShown) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AfterOverlayShown();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NChooseACardSelectionScreen.MethodName.AfterOverlayHidden) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AfterOverlayHidden();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NChooseACardSelectionScreen.MethodName.UpdateControllerIcons) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.UpdateControllerIcons();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NChooseACardSelectionScreen.MethodName._Ready) || StringName.op_Equality(ref method, NChooseACardSelectionScreen.MethodName._ExitTree) || StringName.op_Equality(ref method, NChooseACardSelectionScreen.MethodName.SelectHolder) || StringName.op_Equality(ref method, NChooseACardSelectionScreen.MethodName.OpenPreviewScreen) || StringName.op_Equality(ref method, NChooseACardSelectionScreen.MethodName.OnSkipButtonReleased) || StringName.op_Equality(ref method, NChooseACardSelectionScreen.MethodName.AfterOverlayOpened) || StringName.op_Equality(ref method, NChooseACardSelectionScreen.MethodName.AfterOverlayClosed) || StringName.op_Equality(ref method, NChooseACardSelectionScreen.MethodName.AfterOverlayShown) || StringName.op_Equality(ref method, NChooseACardSelectionScreen.MethodName.AfterOverlayHidden) || StringName.op_Equality(ref method, NChooseACardSelectionScreen.MethodName.UpdateControllerIcons) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NChooseACardSelectionScreen.PropertyName._banner))
    {
      this._banner = VariantUtils.ConvertTo<NCommonBanner>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseACardSelectionScreen.PropertyName._cardRow))
    {
      this._cardRow = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseACardSelectionScreen.PropertyName._skipButton))
    {
      this._skipButton = VariantUtils.ConvertTo<NChoiceSelectionSkipButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseACardSelectionScreen.PropertyName._combatPiles))
    {
      this._combatPiles = VariantUtils.ConvertTo<NCombatPilesContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseACardSelectionScreen.PropertyName._inspectPrompt))
    {
      this._inspectPrompt = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseACardSelectionScreen.PropertyName._peekButton))
    {
      this._peekButton = VariantUtils.ConvertTo<NPeekButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseACardSelectionScreen.PropertyName._openedTicks))
    {
      this._openedTicks = VariantUtils.ConvertTo<ulong>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseACardSelectionScreen.PropertyName._screenComplete))
    {
      this._screenComplete = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseACardSelectionScreen.PropertyName._cardSelected))
    {
      this._cardSelected = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseACardSelectionScreen.PropertyName._canSkip))
    {
      this._canSkip = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseACardSelectionScreen.PropertyName._cardTween))
    {
      this._cardTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NChooseACardSelectionScreen.PropertyName._fadeTween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._fadeTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NChooseACardSelectionScreen.PropertyName.ScreenType))
    {
      ref godot_variant local = ref value;
      NetScreenType screenType = this.ScreenType;
      godot_variant from = VariantUtils.CreateFrom<NetScreenType>(ref screenType);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseACardSelectionScreen.PropertyName.UseSharedBackstop))
    {
      ref godot_variant local = ref value;
      bool useSharedBackstop = this.UseSharedBackstop;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref useSharedBackstop);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseACardSelectionScreen.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseACardSelectionScreen.PropertyName._banner))
    {
      value = VariantUtils.CreateFrom<NCommonBanner>(ref this._banner);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseACardSelectionScreen.PropertyName._cardRow))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._cardRow);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseACardSelectionScreen.PropertyName._skipButton))
    {
      value = VariantUtils.CreateFrom<NChoiceSelectionSkipButton>(ref this._skipButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseACardSelectionScreen.PropertyName._combatPiles))
    {
      value = VariantUtils.CreateFrom<NCombatPilesContainer>(ref this._combatPiles);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseACardSelectionScreen.PropertyName._inspectPrompt))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._inspectPrompt);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseACardSelectionScreen.PropertyName._peekButton))
    {
      value = VariantUtils.CreateFrom<NPeekButton>(ref this._peekButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseACardSelectionScreen.PropertyName._openedTicks))
    {
      value = VariantUtils.CreateFrom<ulong>(ref this._openedTicks);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseACardSelectionScreen.PropertyName._screenComplete))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._screenComplete);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseACardSelectionScreen.PropertyName._cardSelected))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._cardSelected);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseACardSelectionScreen.PropertyName._canSkip))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._canSkip);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseACardSelectionScreen.PropertyName._cardTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._cardTween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NChooseACardSelectionScreen.PropertyName._fadeTween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._fadeTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NChooseACardSelectionScreen.PropertyName._banner, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NChooseACardSelectionScreen.PropertyName._cardRow, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NChooseACardSelectionScreen.PropertyName._skipButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NChooseACardSelectionScreen.PropertyName._combatPiles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NChooseACardSelectionScreen.PropertyName._inspectPrompt, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NChooseACardSelectionScreen.PropertyName._peekButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NChooseACardSelectionScreen.PropertyName._openedTicks, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NChooseACardSelectionScreen.PropertyName._screenComplete, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NChooseACardSelectionScreen.PropertyName._cardSelected, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NChooseACardSelectionScreen.PropertyName._canSkip, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NChooseACardSelectionScreen.PropertyName._cardTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NChooseACardSelectionScreen.PropertyName._fadeTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NChooseACardSelectionScreen.PropertyName.ScreenType, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NChooseACardSelectionScreen.PropertyName.UseSharedBackstop, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NChooseACardSelectionScreen.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NChooseACardSelectionScreen.PropertyName._banner, Variant.From<NCommonBanner>(ref this._banner));
    info.AddProperty(NChooseACardSelectionScreen.PropertyName._cardRow, Variant.From<Control>(ref this._cardRow));
    info.AddProperty(NChooseACardSelectionScreen.PropertyName._skipButton, Variant.From<NChoiceSelectionSkipButton>(ref this._skipButton));
    info.AddProperty(NChooseACardSelectionScreen.PropertyName._combatPiles, Variant.From<NCombatPilesContainer>(ref this._combatPiles));
    info.AddProperty(NChooseACardSelectionScreen.PropertyName._inspectPrompt, Variant.From<Control>(ref this._inspectPrompt));
    info.AddProperty(NChooseACardSelectionScreen.PropertyName._peekButton, Variant.From<NPeekButton>(ref this._peekButton));
    info.AddProperty(NChooseACardSelectionScreen.PropertyName._openedTicks, Variant.From<ulong>(ref this._openedTicks));
    info.AddProperty(NChooseACardSelectionScreen.PropertyName._screenComplete, Variant.From<bool>(ref this._screenComplete));
    info.AddProperty(NChooseACardSelectionScreen.PropertyName._cardSelected, Variant.From<bool>(ref this._cardSelected));
    info.AddProperty(NChooseACardSelectionScreen.PropertyName._canSkip, Variant.From<bool>(ref this._canSkip));
    info.AddProperty(NChooseACardSelectionScreen.PropertyName._cardTween, Variant.From<Tween>(ref this._cardTween));
    info.AddProperty(NChooseACardSelectionScreen.PropertyName._fadeTween, Variant.From<Tween>(ref this._fadeTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NChooseACardSelectionScreen.PropertyName._banner, ref variant1))
      this._banner = ((Variant) ref variant1).As<NCommonBanner>();
    Variant variant2;
    if (info.TryGetProperty(NChooseACardSelectionScreen.PropertyName._cardRow, ref variant2))
      this._cardRow = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NChooseACardSelectionScreen.PropertyName._skipButton, ref variant3))
      this._skipButton = ((Variant) ref variant3).As<NChoiceSelectionSkipButton>();
    Variant variant4;
    if (info.TryGetProperty(NChooseACardSelectionScreen.PropertyName._combatPiles, ref variant4))
      this._combatPiles = ((Variant) ref variant4).As<NCombatPilesContainer>();
    Variant variant5;
    if (info.TryGetProperty(NChooseACardSelectionScreen.PropertyName._inspectPrompt, ref variant5))
      this._inspectPrompt = ((Variant) ref variant5).As<Control>();
    Variant variant6;
    if (info.TryGetProperty(NChooseACardSelectionScreen.PropertyName._peekButton, ref variant6))
      this._peekButton = ((Variant) ref variant6).As<NPeekButton>();
    Variant variant7;
    if (info.TryGetProperty(NChooseACardSelectionScreen.PropertyName._openedTicks, ref variant7))
      this._openedTicks = ((Variant) ref variant7).As<ulong>();
    Variant variant8;
    if (info.TryGetProperty(NChooseACardSelectionScreen.PropertyName._screenComplete, ref variant8))
      this._screenComplete = ((Variant) ref variant8).As<bool>();
    Variant variant9;
    if (info.TryGetProperty(NChooseACardSelectionScreen.PropertyName._cardSelected, ref variant9))
      this._cardSelected = ((Variant) ref variant9).As<bool>();
    Variant variant10;
    if (info.TryGetProperty(NChooseACardSelectionScreen.PropertyName._canSkip, ref variant10))
      this._canSkip = ((Variant) ref variant10).As<bool>();
    Variant variant11;
    if (info.TryGetProperty(NChooseACardSelectionScreen.PropertyName._cardTween, ref variant11))
      this._cardTween = ((Variant) ref variant11).As<Tween>();
    Variant variant12;
    if (!info.TryGetProperty(NChooseACardSelectionScreen.PropertyName._fadeTween, ref variant12))
      return;
    this._fadeTween = ((Variant) ref variant12).As<Tween>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName SelectHolder = StringName.op_Implicit(nameof (SelectHolder));
    public static readonly StringName OpenPreviewScreen = StringName.op_Implicit(nameof (OpenPreviewScreen));
    public static readonly StringName OnSkipButtonReleased = StringName.op_Implicit(nameof (OnSkipButtonReleased));
    public static readonly StringName AfterOverlayOpened = StringName.op_Implicit(nameof (AfterOverlayOpened));
    public static readonly StringName AfterOverlayClosed = StringName.op_Implicit(nameof (AfterOverlayClosed));
    public static readonly StringName AfterOverlayShown = StringName.op_Implicit(nameof (AfterOverlayShown));
    public static readonly StringName AfterOverlayHidden = StringName.op_Implicit(nameof (AfterOverlayHidden));
    public static readonly StringName UpdateControllerIcons = StringName.op_Implicit(nameof (UpdateControllerIcons));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName ScreenType = StringName.op_Implicit(nameof (ScreenType));
    public static readonly StringName UseSharedBackstop = StringName.op_Implicit(nameof (UseSharedBackstop));
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _banner = StringName.op_Implicit(nameof (_banner));
    public static readonly StringName _cardRow = StringName.op_Implicit(nameof (_cardRow));
    public static readonly StringName _skipButton = StringName.op_Implicit(nameof (_skipButton));
    public static readonly StringName _combatPiles = StringName.op_Implicit(nameof (_combatPiles));
    public static readonly StringName _inspectPrompt = StringName.op_Implicit(nameof (_inspectPrompt));
    public static readonly StringName _peekButton = StringName.op_Implicit(nameof (_peekButton));
    public static readonly StringName _openedTicks = StringName.op_Implicit(nameof (_openedTicks));
    public static readonly StringName _screenComplete = StringName.op_Implicit(nameof (_screenComplete));
    public static readonly StringName _cardSelected = StringName.op_Implicit(nameof (_cardSelected));
    public static readonly StringName _canSkip = StringName.op_Implicit(nameof (_canSkip));
    public static readonly StringName _cardTween = StringName.op_Implicit(nameof (_cardTween));
    public static readonly StringName _fadeTween = StringName.op_Implicit(nameof (_fadeTween));
  }

  public class SignalName : Control.SignalName
  {
  }
}
