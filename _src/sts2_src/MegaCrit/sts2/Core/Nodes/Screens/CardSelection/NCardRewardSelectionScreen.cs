// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.CardSelection.NCardRewardSelectionScreen
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.CardRewardAlternatives;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Ftue;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;

[ScriptPath("res://src/Core/Nodes/Screens/CardSelection/NCardRewardSelectionScreen.cs")]
public class NCardRewardSelectionScreen : Control, IOverlayScreen, IScreenContext
{
  private const ulong _noSelectionTimeMsec = 350;
  private Control _ui;
  private NCommonBanner _banner;
  private Control _cardRow;
  private IReadOnlyList<CardCreationResult> _options;
  private IReadOnlyList<CardRewardAlternative> _extraOptions;
  private Control _rewardAlternativesContainer;
  private Control _inspectPrompt;
  private TaskCompletionSource<int?>? _completionSource;
  private Tween? _cardTween;
  private Tween? _buttonTween;
  private const float _cardXOffset = 350f;
  private static readonly Vector2 _bannerAnimPosOffset = new Vector2(0.0f, 50f);
  private CancellationTokenSource _cts = new CancellationTokenSource();
  private Control? _lastFocusedControl;

  private static string ScenePath
  {
    get => SceneHelper.GetScenePath("screens/card_selection/card_reward_selection_screen");
  }

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return ((IEnumerable<string>) new string[1]
      {
        NCardRewardSelectionScreen.ScenePath
      }).Concat<string>(NCardRewardAlternativeButton.AssetPaths);
    }
  }

  public NetScreenType ScreenType => NetScreenType.CardSelection;

  public static NCardRewardSelectionScreen? ShowScreen(
    IReadOnlyList<CardCreationResult> options,
    IReadOnlyList<CardRewardAlternative> extraOptions)
  {
    if (TestMode.IsOn)
      return (NCardRewardSelectionScreen) null;
    NCardRewardSelectionScreen screen = PreloadManager.Cache.GetScene(NCardRewardSelectionScreen.ScenePath).Instantiate<NCardRewardSelectionScreen>((PackedScene.GenEditState) 0L);
    ((Node) screen).Name = StringName.op_Implicit(nameof (NCardRewardSelectionScreen));
    screen._options = options;
    screen._extraOptions = extraOptions;
    NOverlayStack.Instance.Push((IOverlayScreen) screen);
    return screen;
  }

  public override void _EnterTree() => this._cts = new CancellationTokenSource();

  public override void _Ready()
  {
    this._ui = ((Node) this).GetNode<Control>(NodePath.op_Implicit("UI"));
    this._cardRow = ((Node) this).GetNode<Control>(NodePath.op_Implicit("UI/CardRow"));
    this._banner = ((Node) this).GetNode<NCommonBanner>(NodePath.op_Implicit("UI/Banner"));
    this._rewardAlternativesContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("UI/RewardAlternatives"));
    this._inspectPrompt = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%InspectPrompt"));
    this._banner.label.SetTextAutoSize(new LocString("gameplay_ui", "CHOOSE_CARD_HEADER").GetRawText());
    this._banner.AnimateIn();
    this.RefreshOptions(this._options, this._extraOptions);
    this.UpdateControllerIcons();
    ((GodotObject) NControllerManager.Instance).Connect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.UpdateControllerIcons)), 0U);
    ((GodotObject) NControllerManager.Instance).Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.UpdateControllerIcons)), 0U);
    ((GodotObject) NInputManager.Instance).Connect(NInputManager.SignalName.InputRebound, Callable.From(new Action(this.UpdateControllerIcons)), 0U);
  }

  public void RefreshOptions(
    IReadOnlyList<CardCreationResult> options,
    IReadOnlyList<CardRewardAlternative> extraOptions)
  {
    this._options = options;
    this._extraOptions = extraOptions;
    Vector2 vector2 = Vector2.op_Multiply(Vector2.op_Multiply(Vector2.op_Multiply(Vector2.Left, (float) (this._options.Count - 1)), 350f), 0.5f);
    this._lastFocusedControl = (Control) null;
    foreach (Node node in ((IEnumerable) ((Node) this._cardRow).GetChildren(false)).OfType<NGridCardHolder>())
      node.QueueFreeSafely();
    foreach (Node node in ((IEnumerable) ((Node) this._rewardAlternativesContainer).GetChildren(false)).OfType<NCardRewardAlternativeButton>())
      node.QueueFreeSafely();
    this._cardTween = ((Node) this).CreateTween().SetParallel(true);
    for (int index = 0; index < this._options.Count; ++index)
    {
      CardCreationResult option = this._options[index];
      NCard cardNode = NCard.Create(option.Card);
      NGridCardHolder holder = NGridCardHolder.Create(cardNode);
      ((Node) this._cardRow).AddChildSafely((Node) holder);
      ((GodotObject) holder).Connect(NCardHolder.SignalName.Pressed, Callable.From<NCardHolder>(new Action<NCardHolder>(this.SelectCard)), 0U);
      ((GodotObject) holder).Connect(NCardHolder.SignalName.AltPressed, Callable.From<NCardHolder>(new Action<NCardHolder>(this.InspectCard)), 0U);
      ((GodotObject) holder).Connect(Control.SignalName.FocusEntered, Callable.From<Control>((Func<Control>) (() => this._lastFocusedControl = (Control) holder)), 0U);
      cardNode.UpdateVisuals(PileType.None, CardPreviewMode.Normal);
      holder.Scale = holder.SmallScale;
      this._cardTween.TweenProperty((GodotObject) holder, NodePath.op_Implicit("position"), Variant.op_Implicit(Vector2.op_Addition(vector2, Vector2.op_Multiply(Vector2.op_Multiply(Vector2.Right, 350f), (float) index))), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
      this._cardTween.TweenProperty((GodotObject) holder, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L).From(Variant.op_Implicit(Colors.Black));
      cardNode.ActivateRewardScreenGlow();
      foreach (RelicModel modifyingRelic in option.ModifyingRelics)
      {
        modifyingRelic.Flash();
        cardNode.FlashRelicOnCard(modifyingRelic);
      }
    }
    for (int index = 0; index < this._extraOptions.Count; ++index)
    {
      int capturedIndex = index;
      CardRewardAlternative extraOption = this._extraOptions[index];
      NCardRewardAlternativeButton child = NCardRewardAlternativeButton.Create(extraOption.Title.GetFormattedText(), extraOption.Hotkey);
      ((Node) this._rewardAlternativesContainer).AddChildSafely((Node) child);
      ((GodotObject) child).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>((Action<NButton>) (_ => this.OnAlternateRewardSelected(capturedIndex))), 0U);
    }
    for (int index = 0; index < ((Node) this._cardRow).GetChildCount(false); ++index)
    {
      Control child = ((Node) this._cardRow).GetChild<Control>(index, false);
      child.FocusNeighborBottom = ((Node) child).GetPath();
      child.FocusNeighborTop = ((Node) child).GetPath();
      child.FocusNeighborLeft = index > 0 ? ((Node) this._cardRow).GetChild(index - 1, false).GetPath() : ((Node) this._cardRow).GetChild(((Node) this._cardRow).GetChildCount(false) - 1, false).GetPath();
      child.FocusNeighborRight = index < ((Node) this._cardRow).GetChildCount(false) - 1 ? ((Node) this._cardRow).GetChild(index + 1, false).GetPath() : ((Node) this._cardRow).GetChild(0, false).GetPath();
    }
    ActiveScreenContext.Instance.FocusOnDefaultControl();
  }

  public override void _ExitTree()
  {
    this._cts.Cancel();
    TaskCompletionSource<int?> completionSource = this._completionSource;
    if (completionSource != null)
    {
      Task<int?> task = completionSource.Task;
      if (task != null && !((Task) task).IsCompleted)
        this._completionSource.SetException((Exception) new TaskCanceledException());
    }
    foreach (Node node in ((IEnumerable) ((Node) this._cardRow).GetChildren(false)).OfType<NGridCardHolder>())
      node.QueueFreeSafely();
  }

  public NCardHolder GetCardHolder(CardModel card)
  {
    return (NCardHolder) ((IEnumerable) ((Node) this._cardRow).GetChildren(false)).OfType<NGridCardHolder>().First<NGridCardHolder>((Func<NGridCardHolder, bool>) (h => h.CardModel == card));
  }

  private void OnAlternateRewardSelected(int index)
  {
    this._completionSource?.SetResult(new int?(this._options.Count + index));
  }

  private void SelectCard(NCardHolder cardHolder)
  {
    if (this._completionSource == null)
      throw new InvalidOperationException("CardsSelected must be awaited before a card is selected!");
    this._completionSource.SetResult(new int?(this._options.FirstIndex<CardCreationResult>((Predicate<CardCreationResult>) (o => o.Card == cardHolder.CardModel))));
  }

  private void InspectCard(NCardHolder cardHolder)
  {
    if (((Task) this._completionSource.Task).IsCompleted)
      return;
    NInspectCardScreen inspectCardScreen = NGame.Instance.GetInspectCardScreen();
    int capacity = 1;
    List<CardModel> cards = new List<CardModel>(capacity);
    CollectionsMarshal.SetCount<CardModel>(cards, capacity);
    CollectionsMarshal.AsSpan<CardModel>(cards)[0] = cardHolder.CardNode.Model;
    inspectCardScreen.Open(cards, 0);
  }

  public async Task<int?> OptionSelected()
  {
    this._completionSource = new TaskCompletionSource<int?>();
    return await this._completionSource.Task;
  }

  public void AfterOverlayOpened()
  {
    this.PowerCardFtueCheck();
    this._banner.AnimateIn();
    this._buttonTween = ((Node) this).CreateTween();
    this._buttonTween.SetParallel(true);
    this._buttonTween.TweenProperty((GodotObject) this._rewardAlternativesContainer, NodePath.op_Implicit("position"), Variant.op_Implicit(this._rewardAlternativesContainer.Position), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L).From(Variant.op_Implicit(Vector2.op_Subtraction(this._rewardAlternativesContainer.Position, NCardRewardSelectionScreen._bannerAnimPosOffset)));
    TaskHelper.RunSafely(this.DisableCardsForShortTimeAfterOpening());
  }

  private async Task DisableCardsForShortTimeAfterOpening()
  {
    foreach (NCardHolder ncardHolder in ((IEnumerable) ((Node) this._cardRow).GetChildren(false)).OfType<NGridCardHolder>())
      ncardHolder.SetClickable(false);
    await Cmd.Wait(0.35f, this._cts.Token);
    if (!((Node) this._cardRow).IsValid())
      return;
    foreach (NCardHolder ncardHolder in ((IEnumerable) ((Node) this._cardRow).GetChildren(false)).OfType<NGridCardHolder>())
      ncardHolder.SetClickable(true);
  }

  private void PowerCardFtueCheck()
  {
    if (SaveManager.Instance.SeenFtue("power_card_ftue"))
      return;
    NGridCardHolder card = ((IEnumerable) ((Node) this._cardRow).GetChildren(false)).OfType<NGridCardHolder>().FirstOrDefault<NGridCardHolder>((Func<NGridCardHolder, bool>) (h => h.CardModel.Type == CardType.Power));
    if (card == null)
      return;
    NModalContainer.Instance.Add((Node) NPowerCardFtue.Create((Control) card));
    SaveManager.Instance.MarkFtueAsComplete("power_card_ftue");
  }

  public void AfterOverlayClosed() => ((Node) this).QueueFreeSafely();

  public void AfterOverlayShown() => ((CanvasItem) this).Visible = true;

  public void AfterOverlayHidden() => ((CanvasItem) this).Visible = false;

  public bool UseSharedBackstop => true;

  public Control DefaultFocusedControl
  {
    get
    {
      if (this._lastFocusedControl != null)
        return this._lastFocusedControl;
      List<NGridCardHolder> list = ((IEnumerable) ((Node) this._cardRow).GetChildren(false)).OfType<NGridCardHolder>().ToList<NGridCardHolder>();
      return (Control) list[list.Count / 2];
    }
  }

  private void UpdateControllerIcons()
  {
    ((CanvasItem) this._inspectPrompt).Visible = NControllerManager.Instance.IsUsingController;
    ((Node) this._inspectPrompt).GetNode<TextureRect>(NodePath.op_Implicit("ControllerIcon")).Texture = NInputManager.Instance.GetHotkeyIcon(StringName.op_Implicit(MegaInput.accept));
    ((Node) this._inspectPrompt).GetNode<MegaLabel>(NodePath.op_Implicit("Label")).SetTextAutoSize(new LocString("gameplay_ui", "TO_INSPECT_PROMPT").GetFormattedText());
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(12)
    {
      new MethodInfo(NCardRewardSelectionScreen.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardRewardSelectionScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardRewardSelectionScreen.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardRewardSelectionScreen.MethodName.OnAlternateRewardSelected, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("index"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCardRewardSelectionScreen.MethodName.SelectCard, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("cardHolder"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCardRewardSelectionScreen.MethodName.InspectCard, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("cardHolder"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCardRewardSelectionScreen.MethodName.AfterOverlayOpened, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardRewardSelectionScreen.MethodName.PowerCardFtueCheck, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardRewardSelectionScreen.MethodName.AfterOverlayClosed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardRewardSelectionScreen.MethodName.AfterOverlayShown, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardRewardSelectionScreen.MethodName.AfterOverlayHidden, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardRewardSelectionScreen.MethodName.UpdateControllerIcons, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCardRewardSelectionScreen.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardRewardSelectionScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardRewardSelectionScreen.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardRewardSelectionScreen.MethodName.OnAlternateRewardSelected) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnAlternateRewardSelected(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardRewardSelectionScreen.MethodName.SelectCard) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SelectCard(VariantUtils.ConvertTo<NCardHolder>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardRewardSelectionScreen.MethodName.InspectCard) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.InspectCard(VariantUtils.ConvertTo<NCardHolder>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardRewardSelectionScreen.MethodName.AfterOverlayOpened) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AfterOverlayOpened();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardRewardSelectionScreen.MethodName.PowerCardFtueCheck) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.PowerCardFtueCheck();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardRewardSelectionScreen.MethodName.AfterOverlayClosed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AfterOverlayClosed();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardRewardSelectionScreen.MethodName.AfterOverlayShown) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AfterOverlayShown();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardRewardSelectionScreen.MethodName.AfterOverlayHidden) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AfterOverlayHidden();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCardRewardSelectionScreen.MethodName.UpdateControllerIcons) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.UpdateControllerIcons();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCardRewardSelectionScreen.MethodName._EnterTree) || StringName.op_Equality(ref method, NCardRewardSelectionScreen.MethodName._Ready) || StringName.op_Equality(ref method, NCardRewardSelectionScreen.MethodName._ExitTree) || StringName.op_Equality(ref method, NCardRewardSelectionScreen.MethodName.OnAlternateRewardSelected) || StringName.op_Equality(ref method, NCardRewardSelectionScreen.MethodName.SelectCard) || StringName.op_Equality(ref method, NCardRewardSelectionScreen.MethodName.InspectCard) || StringName.op_Equality(ref method, NCardRewardSelectionScreen.MethodName.AfterOverlayOpened) || StringName.op_Equality(ref method, NCardRewardSelectionScreen.MethodName.PowerCardFtueCheck) || StringName.op_Equality(ref method, NCardRewardSelectionScreen.MethodName.AfterOverlayClosed) || StringName.op_Equality(ref method, NCardRewardSelectionScreen.MethodName.AfterOverlayShown) || StringName.op_Equality(ref method, NCardRewardSelectionScreen.MethodName.AfterOverlayHidden) || StringName.op_Equality(ref method, NCardRewardSelectionScreen.MethodName.UpdateControllerIcons) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardRewardSelectionScreen.PropertyName._ui))
    {
      this._ui = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardRewardSelectionScreen.PropertyName._banner))
    {
      this._banner = VariantUtils.ConvertTo<NCommonBanner>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardRewardSelectionScreen.PropertyName._cardRow))
    {
      this._cardRow = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardRewardSelectionScreen.PropertyName._rewardAlternativesContainer))
    {
      this._rewardAlternativesContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardRewardSelectionScreen.PropertyName._inspectPrompt))
    {
      this._inspectPrompt = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardRewardSelectionScreen.PropertyName._cardTween))
    {
      this._cardTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardRewardSelectionScreen.PropertyName._buttonTween))
    {
      this._buttonTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardRewardSelectionScreen.PropertyName._lastFocusedControl))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._lastFocusedControl = VariantUtils.ConvertTo<Control>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardRewardSelectionScreen.PropertyName.ScreenType))
    {
      ref godot_variant local = ref value;
      NetScreenType screenType = this.ScreenType;
      godot_variant from = VariantUtils.CreateFrom<NetScreenType>(ref screenType);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardRewardSelectionScreen.PropertyName.UseSharedBackstop))
    {
      ref godot_variant local = ref value;
      bool useSharedBackstop = this.UseSharedBackstop;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref useSharedBackstop);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardRewardSelectionScreen.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardRewardSelectionScreen.PropertyName._ui))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._ui);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardRewardSelectionScreen.PropertyName._banner))
    {
      value = VariantUtils.CreateFrom<NCommonBanner>(ref this._banner);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardRewardSelectionScreen.PropertyName._cardRow))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._cardRow);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardRewardSelectionScreen.PropertyName._rewardAlternativesContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._rewardAlternativesContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardRewardSelectionScreen.PropertyName._inspectPrompt))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._inspectPrompt);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardRewardSelectionScreen.PropertyName._cardTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._cardTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardRewardSelectionScreen.PropertyName._buttonTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._buttonTween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardRewardSelectionScreen.PropertyName._lastFocusedControl))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Control>(ref this._lastFocusedControl);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCardRewardSelectionScreen.PropertyName._ui, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardRewardSelectionScreen.PropertyName._banner, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardRewardSelectionScreen.PropertyName._cardRow, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardRewardSelectionScreen.PropertyName._rewardAlternativesContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardRewardSelectionScreen.PropertyName._inspectPrompt, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardRewardSelectionScreen.PropertyName._cardTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardRewardSelectionScreen.PropertyName._buttonTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardRewardSelectionScreen.PropertyName._lastFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NCardRewardSelectionScreen.PropertyName.ScreenType, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCardRewardSelectionScreen.PropertyName.UseSharedBackstop, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardRewardSelectionScreen.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NCardRewardSelectionScreen.PropertyName._ui, Variant.From<Control>(ref this._ui));
    info.AddProperty(NCardRewardSelectionScreen.PropertyName._banner, Variant.From<NCommonBanner>(ref this._banner));
    info.AddProperty(NCardRewardSelectionScreen.PropertyName._cardRow, Variant.From<Control>(ref this._cardRow));
    info.AddProperty(NCardRewardSelectionScreen.PropertyName._rewardAlternativesContainer, Variant.From<Control>(ref this._rewardAlternativesContainer));
    info.AddProperty(NCardRewardSelectionScreen.PropertyName._inspectPrompt, Variant.From<Control>(ref this._inspectPrompt));
    info.AddProperty(NCardRewardSelectionScreen.PropertyName._cardTween, Variant.From<Tween>(ref this._cardTween));
    info.AddProperty(NCardRewardSelectionScreen.PropertyName._buttonTween, Variant.From<Tween>(ref this._buttonTween));
    info.AddProperty(NCardRewardSelectionScreen.PropertyName._lastFocusedControl, Variant.From<Control>(ref this._lastFocusedControl));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCardRewardSelectionScreen.PropertyName._ui, ref variant1))
      this._ui = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NCardRewardSelectionScreen.PropertyName._banner, ref variant2))
      this._banner = ((Variant) ref variant2).As<NCommonBanner>();
    Variant variant3;
    if (info.TryGetProperty(NCardRewardSelectionScreen.PropertyName._cardRow, ref variant3))
      this._cardRow = ((Variant) ref variant3).As<Control>();
    Variant variant4;
    if (info.TryGetProperty(NCardRewardSelectionScreen.PropertyName._rewardAlternativesContainer, ref variant4))
      this._rewardAlternativesContainer = ((Variant) ref variant4).As<Control>();
    Variant variant5;
    if (info.TryGetProperty(NCardRewardSelectionScreen.PropertyName._inspectPrompt, ref variant5))
      this._inspectPrompt = ((Variant) ref variant5).As<Control>();
    Variant variant6;
    if (info.TryGetProperty(NCardRewardSelectionScreen.PropertyName._cardTween, ref variant6))
      this._cardTween = ((Variant) ref variant6).As<Tween>();
    Variant variant7;
    if (info.TryGetProperty(NCardRewardSelectionScreen.PropertyName._buttonTween, ref variant7))
      this._buttonTween = ((Variant) ref variant7).As<Tween>();
    Variant variant8;
    if (!info.TryGetProperty(NCardRewardSelectionScreen.PropertyName._lastFocusedControl, ref variant8))
      return;
    this._lastFocusedControl = ((Variant) ref variant8).As<Control>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName OnAlternateRewardSelected = StringName.op_Implicit(nameof (OnAlternateRewardSelected));
    public static readonly StringName SelectCard = StringName.op_Implicit(nameof (SelectCard));
    public static readonly StringName InspectCard = StringName.op_Implicit(nameof (InspectCard));
    public static readonly StringName AfterOverlayOpened = StringName.op_Implicit(nameof (AfterOverlayOpened));
    public static readonly StringName PowerCardFtueCheck = StringName.op_Implicit(nameof (PowerCardFtueCheck));
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
    public static readonly StringName _ui = StringName.op_Implicit(nameof (_ui));
    public static readonly StringName _banner = StringName.op_Implicit(nameof (_banner));
    public static readonly StringName _cardRow = StringName.op_Implicit(nameof (_cardRow));
    public static readonly StringName _rewardAlternativesContainer = StringName.op_Implicit(nameof (_rewardAlternativesContainer));
    public static readonly StringName _inspectPrompt = StringName.op_Implicit(nameof (_inspectPrompt));
    public static readonly StringName _cardTween = StringName.op_Implicit(nameof (_cardTween));
    public static readonly StringName _buttonTween = StringName.op_Implicit(nameof (_buttonTween));
    public static readonly StringName _lastFocusedControl = StringName.op_Implicit(nameof (_lastFocusedControl));
  }

  public class SignalName : Control.SignalName
  {
  }
}
