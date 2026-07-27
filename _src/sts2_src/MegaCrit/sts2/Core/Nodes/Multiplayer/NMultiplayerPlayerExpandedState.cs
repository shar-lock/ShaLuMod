// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerPlayerExpandedState
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Debug;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Potions;
using MegaCrit.Sts2.Core.Nodes.Relics;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Multiplayer;

[ScriptPath("res://src/Core/Nodes/Multiplayer/NMultiplayerPlayerExpandedState.cs")]
public class NMultiplayerPlayerExpandedState : Control, ICapstoneScreen, IScreenContext
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("ui/multiplayer_player_expanded_state");
  private MegaRichTextLabel _playerNameLabel;
  private MegaRichTextLabel _cardsHeader;
  private Control _cardContainer;
  private NBackButton _backButton;
  private MegaRichTextLabel _potionsHeader;
  private Control _potionContainer;
  private MegaRichTextLabel _relicsHeader;
  private Control _relicContainer;
  private Player _player;
  private List<CardModel> _cards = new List<CardModel>();

  public bool UseSharedBackstop => true;

  public NetScreenType ScreenType => NetScreenType.RemotePlayerExpandedState;

  public static NMultiplayerPlayerExpandedState Create(Player player)
  {
    NMultiplayerPlayerExpandedState playerExpandedState = PreloadManager.Cache.GetScene(NMultiplayerPlayerExpandedState._scenePath).Instantiate<NMultiplayerPlayerExpandedState>((PackedScene.GenEditState) 0L);
    playerExpandedState._player = player;
    return playerExpandedState;
  }

  public void AfterCapstoneOpened()
  {
    this._backButton.Enable();
    NGlobalUi globalUi = NRun.Instance.GlobalUi;
    globalUi.TopBar.AnimHide();
    globalUi.RelicInventory.AnimHide();
    globalUi.MultiplayerPlayerContainer.AnimHide();
    ((Node) globalUi).MoveChildSafely((Node) globalUi.AboveTopBarVfxContainer, ((Node) globalUi.CapstoneContainer).GetIndex(false));
    ((Node) globalUi).MoveChildSafely((Node) globalUi.CardPreviewContainer, ((Node) globalUi.CapstoneContainer).GetIndex(false));
    ((Node) globalUi).MoveChildSafely((Node) globalUi.MessyCardPreviewContainer, ((Node) globalUi.CapstoneContainer).GetIndex(false));
  }

  public void AfterCapstoneClosed()
  {
    NGlobalUi globalUi = NRun.Instance.GlobalUi;
    globalUi.TopBar.AnimShow();
    globalUi.RelicInventory.AnimShow();
    globalUi.MultiplayerPlayerContainer.AnimShow();
    ((Node) globalUi).MoveChildSafely((Node) globalUi.AboveTopBarVfxContainer, ((Node) globalUi.TopBar).GetIndex(false) + 1);
    ((Node) globalUi).MoveChildSafely((Node) globalUi.CardPreviewContainer, ((Node) globalUi.TopBar).GetIndex(false) + 1);
    ((Node) globalUi).MoveChildSafely((Node) globalUi.MessyCardPreviewContainer, ((Node) globalUi.TopBar).GetIndex(false) + 1);
    ((Node) this).QueueFreeSafely();
    this._backButton.Disable();
  }

  public override void _Ready()
  {
    this._playerNameLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%PlayerNameLabel"));
    this._cardsHeader = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%CardsHeader"));
    this._cardContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%CardContainer"));
    this._relicsHeader = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%RelicsHeader"));
    this._relicContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%RelicContainer"));
    this._potionsHeader = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%PotionsHeader"));
    this._potionContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%PotionContainer"));
    this._backButton = ((Node) this).GetNode<NBackButton>(NodePath.op_Implicit("%BackButton"));
    LocString locString = new LocString("gameplay_ui", "MULTIPLAYER_EXPANDED_STATE.title");
    locString.Add("PlayerName", PlatformUtil.GetPlayerName(RunManager.Instance.NetService.Platform, this._player.NetId));
    locString.Add("Character", this._player.Character.Title);
    this._playerNameLabel.Text = locString.GetFormattedText();
    this._relicsHeader.Text = new LocString("gameplay_ui", "MULTIPLAYER_EXPANDED_STATE.relicHeader").GetFormattedText();
    this._cardsHeader.Text = new LocString("gameplay_ui", "MULTIPLAYER_EXPANDED_STATE.cardHeader").GetFormattedText();
    this._potionsHeader.Text = new LocString("gameplay_ui", "MULTIPLAYER_EXPANDED_STATE.potionHeader").GetFormattedText();
    foreach (RelicModel relic in (IEnumerable<RelicModel>) this._player.Relics)
    {
      NRelicBasicHolder holder = NRelicBasicHolder.Create(relic);
      ((Node) this._relicContainer).AddChildSafely((Node) holder);
      ((GodotObject) holder).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>((Action<NButton>) (_ => this.OnRelicClicked(holder.Relic))), 0U);
      holder.MouseDefaultCursorShape = (Control.CursorShape) 16L /*0x10*/;
    }
    foreach (PotionModel potion1 in this._player.Potions)
    {
      NPotionHolder child = NPotionHolder.Create(false);
      NPotion potion2 = NPotion.Create(potion1);
      ((Node) this._potionContainer).AddChildSafely((Node) child);
      child.AddPotion(potion2);
      potion2.Position = Vector2.Zero;
    }
    this._cards.Clear();
    this._cards.AddRange((IEnumerable<CardModel>) this._player.Deck.Cards);
    foreach (IGrouping<NMultiplayerPlayerExpandedState.CardGroupKey, CardModel> source in this._player.Deck.Cards.GroupBy<CardModel, NMultiplayerPlayerExpandedState.CardGroupKey>((Func<CardModel, NMultiplayerPlayerExpandedState.CardGroupKey>) (x => new NMultiplayerPlayerExpandedState.CardGroupKey(x))))
    {
      NDeckHistoryEntry child = NDeckHistoryEntry.Create(source.First<CardModel>(), source.Count<CardModel>());
      ((GodotObject) child).Connect(NDeckHistoryEntry.SignalName.Clicked, Callable.From<NDeckHistoryEntry>(new Action<NDeckHistoryEntry>(this.ShowEntry)), 0U);
      ((Node) this._cardContainer).AddChildSafely((Node) child);
    }
    ((GodotObject) this._backButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.BackButtonPressed)), 0U);
    this.UpdateNavigation();
  }

  public override void _Input(InputEvent inputEvent)
  {
    if (!((CanvasItem) this).IsVisibleInTree() || NDevConsole.IsConsoleVisible || !NControllerManager.Instance.IsUsingController)
      return;
    bool flag;
    switch (((Node) this).GetViewport().GuiGetFocusOwner())
    {
      case TextEdit _:
      case LineEdit _:
        flag = true;
        break;
      default:
        flag = false;
        break;
    }
    if (flag || !ActiveScreenContext.Instance.IsCurrent((IScreenContext) this))
      return;
    Control focusOwner = ((Node) this).GetViewport().GuiGetFocusOwner();
    if (focusOwner != null && ((Node) this).IsAncestorOf((Node) focusOwner) || !inputEvent.IsActionPressed(MegaInput.left, false, false) && !inputEvent.IsActionPressed(MegaInput.right, false, false) && !inputEvent.IsActionPressed(MegaInput.up, false, false) && !inputEvent.IsActionPressed(MegaInput.down, false, false) && !inputEvent.IsActionPressed(MegaInput.select, false, false))
      return;
    ((Node) this._relicContainer).GetChild<NRelicBasicHolder>(0, false).TryGrabFocus();
    ((Node) this).GetViewport()?.SetInputAsHandled();
  }

  private void ShowEntry(NDeckHistoryEntry entry)
  {
    NGame.Instance.GetInspectCardScreen().Open(this._cards, this._cards.IndexOf(entry.Card));
  }

  private void BackButtonPressed(NButton _) => NCapstoneContainer.Instance.Close();

  private void OnRelicClicked(NRelic node)
  {
    List<RelicModel> relics = new List<RelicModel>();
    foreach (NRelicBasicHolder nrelicBasicHolder in ((IEnumerable) ((Node) this._relicContainer).GetChildren(false)).OfType<NRelicBasicHolder>())
      relics.Add(nrelicBasicHolder.Relic.Model);
    NGame.Instance.GetInspectRelicScreen().Open((IReadOnlyList<RelicModel>) relics, node.Model);
  }

  public Control? DefaultFocusedControl => (Control) null;

  private void UpdateNavigation()
  {
    for (int index = 0; index < ((Node) this._relicContainer).GetChildCount(false); ++index)
    {
      NRelicBasicHolder child = ((Node) this._relicContainer).GetChild<NRelicBasicHolder>(index, false);
      child.FocusNeighborLeft = index > 0 ? ((Node) ((Node) this._relicContainer).GetChild<NRelicBasicHolder>(index - 1, false)).GetPath() : ((Node) ((Node) this._relicContainer).GetChild<NRelicBasicHolder>(index, false)).GetPath();
      child.FocusNeighborRight = index < ((Node) this._relicContainer).GetChildCount(false) - 1 ? ((Node) ((Node) this._relicContainer).GetChild<NRelicBasicHolder>(index + 1, false)).GetPath() : ((Node) ((Node) this._relicContainer).GetChild<NRelicBasicHolder>(index, false)).GetPath();
      child.FocusNeighborTop = ((Node) child).GetPath();
      if (((Node) this._potionContainer).GetChildCount(false) > 0)
        child.FocusNeighborBottom = ((Node) ((Node) this._potionContainer).GetChild<Control>(Mathf.Min(index, ((Node) this._potionContainer).GetChildCount(false) - 1), false))?.GetPath();
      else if (((Node) this._cardContainer).GetChildCount(false) > 0)
        child.FocusNeighborBottom = ((Node) ((Node) this._cardContainer).GetChild<Control>(Mathf.Min(index, ((Node) this._cardContainer).GetChildCount(false) - 1), false))?.GetPath();
      else
        child.FocusNeighborBottom = ((Node) child).GetPath();
    }
    for (int index = 0; index < ((Node) this._potionContainer).GetChildCount(false); ++index)
    {
      NPotionHolder child = ((Node) this._potionContainer).GetChild<NPotionHolder>(index, false);
      child.FocusNeighborLeft = index > 0 ? ((Node) ((Node) this._potionContainer).GetChild<NPotionHolder>(index - 1, false)).GetPath() : ((Node) ((Node) this._potionContainer).GetChild<NPotionHolder>(index, false)).GetPath();
      child.FocusNeighborRight = index < ((Node) this._potionContainer).GetChildCount(false) - 1 ? ((Node) ((Node) this._potionContainer).GetChild<NPotionHolder>(index + 1, false)).GetPath() : ((Node) ((Node) this._potionContainer).GetChild<NPotionHolder>(index, false)).GetPath();
      if (((Node) this._relicContainer).GetChildCount(false) > 0)
        child.FocusNeighborTop = ((Node) ((Node) this._relicContainer).GetChild<Control>(Mathf.Min(index, ((Node) this._relicContainer).GetChildCount(false) - 1), false))?.GetPath();
      else
        child.FocusNeighborTop = ((Node) child).GetPath();
      if (((Node) this._cardContainer).GetChildCount(false) > 0)
        child.FocusNeighborBottom = ((Node) ((Node) this._cardContainer).GetChild<Control>(Mathf.Min(index, ((Node) this._cardContainer).GetChildCount(false) - 1), false))?.GetPath();
      else
        child.FocusNeighborBottom = ((Node) child).GetPath();
    }
    for (int index = 0; index < ((Node) this._cardContainer).GetChildCount(false); ++index)
    {
      NDeckHistoryEntry child = ((Node) this._cardContainer).GetChild<NDeckHistoryEntry>(index, false);
      child.FocusNeighborLeft = index > 0 ? ((Node) ((Node) this._cardContainer).GetChild<NDeckHistoryEntry>(index - 1, false)).GetPath() : ((Node) ((Node) this._cardContainer).GetChild<NDeckHistoryEntry>(index, false)).GetPath();
      child.FocusNeighborRight = index < ((Node) this._cardContainer).GetChildCount(false) - 1 ? ((Node) ((Node) this._cardContainer).GetChild<NDeckHistoryEntry>(index + 1, false)).GetPath() : ((Node) ((Node) this._cardContainer).GetChild<NDeckHistoryEntry>(index, false)).GetPath();
      if (((Node) this._potionContainer).GetChildCount(false) > 0)
        child.FocusNeighborTop = ((Node) ((Node) this._potionContainer).GetChild<Control>(Mathf.Min(index, ((Node) this._potionContainer).GetChildCount(false) - 1), false))?.GetPath();
      else if (((Node) this._relicContainer).GetChildCount(false) > 0)
        child.FocusNeighborTop = ((Node) ((Node) this._relicContainer).GetChild<Control>(Mathf.Min(index, ((Node) this._relicContainer).GetChildCount(false) - 1), false))?.GetPath();
      else
        child.FocusNeighborTop = ((Node) child).GetPath();
      child.FocusNeighborBottom = ((Node) child).GetPath();
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(8)
    {
      new MethodInfo(NMultiplayerPlayerExpandedState.MethodName.AfterCapstoneOpened, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerExpandedState.MethodName.AfterCapstoneClosed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerExpandedState.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerExpandedState.MethodName._Input, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerExpandedState.MethodName.ShowEntry, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("entry"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerExpandedState.MethodName.BackButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerExpandedState.MethodName.OnRelicClicked, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("node"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerExpandedState.MethodName.UpdateNavigation, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMultiplayerPlayerExpandedState.MethodName.AfterCapstoneOpened) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AfterCapstoneOpened();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerExpandedState.MethodName.AfterCapstoneClosed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AfterCapstoneClosed();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerExpandedState.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerExpandedState.MethodName._Input) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Input(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerExpandedState.MethodName.ShowEntry) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ShowEntry(VariantUtils.ConvertTo<NDeckHistoryEntry>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerExpandedState.MethodName.BackButtonPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.BackButtonPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerExpandedState.MethodName.OnRelicClicked) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnRelicClicked(VariantUtils.ConvertTo<NRelic>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMultiplayerPlayerExpandedState.MethodName.UpdateNavigation) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.UpdateNavigation();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMultiplayerPlayerExpandedState.MethodName.AfterCapstoneOpened) || StringName.op_Equality(ref method, NMultiplayerPlayerExpandedState.MethodName.AfterCapstoneClosed) || StringName.op_Equality(ref method, NMultiplayerPlayerExpandedState.MethodName._Ready) || StringName.op_Equality(ref method, NMultiplayerPlayerExpandedState.MethodName._Input) || StringName.op_Equality(ref method, NMultiplayerPlayerExpandedState.MethodName.ShowEntry) || StringName.op_Equality(ref method, NMultiplayerPlayerExpandedState.MethodName.BackButtonPressed) || StringName.op_Equality(ref method, NMultiplayerPlayerExpandedState.MethodName.OnRelicClicked) || StringName.op_Equality(ref method, NMultiplayerPlayerExpandedState.MethodName.UpdateNavigation) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMultiplayerPlayerExpandedState.PropertyName._playerNameLabel))
    {
      this._playerNameLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerExpandedState.PropertyName._cardsHeader))
    {
      this._cardsHeader = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerExpandedState.PropertyName._cardContainer))
    {
      this._cardContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerExpandedState.PropertyName._backButton))
    {
      this._backButton = VariantUtils.ConvertTo<NBackButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerExpandedState.PropertyName._potionsHeader))
    {
      this._potionsHeader = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerExpandedState.PropertyName._potionContainer))
    {
      this._potionContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerExpandedState.PropertyName._relicsHeader))
    {
      this._relicsHeader = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMultiplayerPlayerExpandedState.PropertyName._relicContainer))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._relicContainer = VariantUtils.ConvertTo<Control>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMultiplayerPlayerExpandedState.PropertyName.UseSharedBackstop))
    {
      ref godot_variant local = ref value;
      bool useSharedBackstop = this.UseSharedBackstop;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref useSharedBackstop);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerExpandedState.PropertyName.ScreenType))
    {
      ref godot_variant local = ref value;
      NetScreenType screenType = this.ScreenType;
      godot_variant from = VariantUtils.CreateFrom<NetScreenType>(ref screenType);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerExpandedState.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerExpandedState.PropertyName._playerNameLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._playerNameLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerExpandedState.PropertyName._cardsHeader))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._cardsHeader);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerExpandedState.PropertyName._cardContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._cardContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerExpandedState.PropertyName._backButton))
    {
      value = VariantUtils.CreateFrom<NBackButton>(ref this._backButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerExpandedState.PropertyName._potionsHeader))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._potionsHeader);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerExpandedState.PropertyName._potionContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._potionContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerExpandedState.PropertyName._relicsHeader))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._relicsHeader);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMultiplayerPlayerExpandedState.PropertyName._relicContainer))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Control>(ref this._relicContainer);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerExpandedState.PropertyName._playerNameLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerExpandedState.PropertyName._cardsHeader, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerExpandedState.PropertyName._cardContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerExpandedState.PropertyName._backButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerExpandedState.PropertyName._potionsHeader, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerExpandedState.PropertyName._potionContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerExpandedState.PropertyName._relicsHeader, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerExpandedState.PropertyName._relicContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NMultiplayerPlayerExpandedState.PropertyName.UseSharedBackstop, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NMultiplayerPlayerExpandedState.PropertyName.ScreenType, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerExpandedState.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NMultiplayerPlayerExpandedState.PropertyName._playerNameLabel, Variant.From<MegaRichTextLabel>(ref this._playerNameLabel));
    info.AddProperty(NMultiplayerPlayerExpandedState.PropertyName._cardsHeader, Variant.From<MegaRichTextLabel>(ref this._cardsHeader));
    info.AddProperty(NMultiplayerPlayerExpandedState.PropertyName._cardContainer, Variant.From<Control>(ref this._cardContainer));
    info.AddProperty(NMultiplayerPlayerExpandedState.PropertyName._backButton, Variant.From<NBackButton>(ref this._backButton));
    info.AddProperty(NMultiplayerPlayerExpandedState.PropertyName._potionsHeader, Variant.From<MegaRichTextLabel>(ref this._potionsHeader));
    info.AddProperty(NMultiplayerPlayerExpandedState.PropertyName._potionContainer, Variant.From<Control>(ref this._potionContainer));
    info.AddProperty(NMultiplayerPlayerExpandedState.PropertyName._relicsHeader, Variant.From<MegaRichTextLabel>(ref this._relicsHeader));
    info.AddProperty(NMultiplayerPlayerExpandedState.PropertyName._relicContainer, Variant.From<Control>(ref this._relicContainer));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMultiplayerPlayerExpandedState.PropertyName._playerNameLabel, ref variant1))
      this._playerNameLabel = ((Variant) ref variant1).As<MegaRichTextLabel>();
    Variant variant2;
    if (info.TryGetProperty(NMultiplayerPlayerExpandedState.PropertyName._cardsHeader, ref variant2))
      this._cardsHeader = ((Variant) ref variant2).As<MegaRichTextLabel>();
    Variant variant3;
    if (info.TryGetProperty(NMultiplayerPlayerExpandedState.PropertyName._cardContainer, ref variant3))
      this._cardContainer = ((Variant) ref variant3).As<Control>();
    Variant variant4;
    if (info.TryGetProperty(NMultiplayerPlayerExpandedState.PropertyName._backButton, ref variant4))
      this._backButton = ((Variant) ref variant4).As<NBackButton>();
    Variant variant5;
    if (info.TryGetProperty(NMultiplayerPlayerExpandedState.PropertyName._potionsHeader, ref variant5))
      this._potionsHeader = ((Variant) ref variant5).As<MegaRichTextLabel>();
    Variant variant6;
    if (info.TryGetProperty(NMultiplayerPlayerExpandedState.PropertyName._potionContainer, ref variant6))
      this._potionContainer = ((Variant) ref variant6).As<Control>();
    Variant variant7;
    if (info.TryGetProperty(NMultiplayerPlayerExpandedState.PropertyName._relicsHeader, ref variant7))
      this._relicsHeader = ((Variant) ref variant7).As<MegaRichTextLabel>();
    Variant variant8;
    if (!info.TryGetProperty(NMultiplayerPlayerExpandedState.PropertyName._relicContainer, ref variant8))
      return;
    this._relicContainer = ((Variant) ref variant8).As<Control>();
  }

  private class CardGroupKey
  {
    private readonly 
    #nullable enable
    CardModel _card;

    public CardGroupKey(CardModel card) => this._card = card;

    public override bool Equals(object? obj)
    {
      if (obj == null || obj.GetType() != this.GetType())
        return false;
      NMultiplayerPlayerExpandedState.CardGroupKey cardGroupKey = (NMultiplayerPlayerExpandedState.CardGroupKey) obj;
      if (!this._card.Id.Equals(cardGroupKey._card.Id) || this._card.CurrentUpgradeLevel != cardGroupKey._card.CurrentUpgradeLevel || !(this._card.Enchantment?.Id == cardGroupKey._card.Enchantment?.Id))
        return false;
      int? amount1 = this._card.Enchantment?.Amount;
      int? amount2 = cardGroupKey._card.Enchantment?.Amount;
      return amount1.GetValueOrDefault() == amount2.GetValueOrDefault() & amount1.HasValue == amount2.HasValue;
    }

    public override int GetHashCode()
    {
      return HashCode.Combine<ModelId, int, ModelId, int?>(this._card.Id, this._card.CurrentUpgradeLevel, this._card.Enchantment?.Id, this._card.Enchantment?.Amount);
    }
  }

  public class MethodName : Control.MethodName
  {
    public static readonly 
    #nullable disable
    StringName AfterCapstoneOpened = StringName.op_Implicit(nameof (AfterCapstoneOpened));
    public static readonly StringName AfterCapstoneClosed = StringName.op_Implicit(nameof (AfterCapstoneClosed));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _Input = StringName.op_Implicit(nameof (_Input));
    public static readonly StringName ShowEntry = StringName.op_Implicit(nameof (ShowEntry));
    public static readonly StringName BackButtonPressed = StringName.op_Implicit(nameof (BackButtonPressed));
    public static readonly StringName OnRelicClicked = StringName.op_Implicit(nameof (OnRelicClicked));
    public static readonly StringName UpdateNavigation = StringName.op_Implicit(nameof (UpdateNavigation));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName UseSharedBackstop = StringName.op_Implicit(nameof (UseSharedBackstop));
    public static readonly StringName ScreenType = StringName.op_Implicit(nameof (ScreenType));
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _playerNameLabel = StringName.op_Implicit(nameof (_playerNameLabel));
    public static readonly StringName _cardsHeader = StringName.op_Implicit(nameof (_cardsHeader));
    public static readonly StringName _cardContainer = StringName.op_Implicit(nameof (_cardContainer));
    public static readonly StringName _backButton = StringName.op_Implicit(nameof (_backButton));
    public static readonly StringName _potionsHeader = StringName.op_Implicit(nameof (_potionsHeader));
    public static readonly StringName _potionContainer = StringName.op_Implicit(nameof (_potionContainer));
    public static readonly StringName _relicsHeader = StringName.op_Implicit(nameof (_relicsHeader));
    public static readonly StringName _relicContainer = StringName.op_Implicit(nameof (_relicContainer));
  }

  public class SignalName : Control.SignalName
  {
  }
}
