// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerPlayerState
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Potions;
using MegaCrit.Sts2.Core.Nodes.Relics;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Multiplayer;

[ScriptPath("res://src/Core/Nodes/Multiplayer/NMultiplayerPlayerState.cs")]
public class NMultiplayerPlayerState : Control
{
  private const ulong _delayBetweenTweensMsec = 500;
  private static readonly string _scenePath = SceneHelper.GetScenePath("ui/multiplayer_player_state");
  private static readonly string _cardScenePath = SceneHelper.GetScenePath("screens/run_history_screen/deck_history_entry");
  private const string _darkenedEnergyMatPath = "res://materials/ui/energy_orb_dark.tres";
  private const float _refHpBarWidth = 175f;
  private const float _refHpBarMaxHp = 80f;
  private const float _selectionReticlePadding = 6f;
  private NHealthBar _healthBar;
  private TextureRect _characterIcon;
  private MegaLabel _nameplateLabel;
  private HBoxContainer _topContainer;
  private TextureRect _turnEndIndicator;
  private TextureRect _disconnectedIndicator;
  private NMultiplayerNetworkProblemIndicator _networkProblemIndicator;
  private NSelectionReticle _selectionReticle;
  private TextureRect _locationIcon;
  private Control _locationContainer;
  private Control _energyContainer;
  private TextureRect _energyImage;
  private MegaLabel _energyCount;
  private Control _starContainer;
  private MegaLabel _starCount;
  private Control _cardContainer;
  private NTinyCard _cardImage;
  private MegaLabel _cardCount;
  private Tween? _locationIconTween;
  private bool _isMouseOver;
  private bool _isCreatureHovered;
  private bool _isHighlighted;
  private bool _focusedWhileTargeting;
  private ulong _nextTweenTime;
  private Texture2D? _currentLocationIcon;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[2]
      {
        NMultiplayerPlayerState._scenePath,
        NMultiplayerPlayerState._cardScenePath
      });
    }
  }

  public NButton Hitbox { get; private set; }

  public Player Player { get; private set; }

  public static NMultiplayerPlayerState Create(Player player)
  {
    NMultiplayerPlayerState nmultiplayerPlayerState = PreloadManager.Cache.GetScene(NMultiplayerPlayerState._scenePath).Instantiate<NMultiplayerPlayerState>((PackedScene.GenEditState) 0L);
    nmultiplayerPlayerState.Player = player;
    return nmultiplayerPlayerState;
  }

  public override void _Ready()
  {
    this._nameplateLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%NameplateLabel"));
    this._healthBar = ((Node) this).GetNode<NHealthBar>(NodePath.op_Implicit("%HealthBar"));
    this._characterIcon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%CharacterIcon"));
    this._turnEndIndicator = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%TurnEndIndicator"));
    this._disconnectedIndicator = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%DisconnectedIndicator"));
    this._networkProblemIndicator = ((Node) this).GetNode<NMultiplayerNetworkProblemIndicator>(NodePath.op_Implicit("%NetworkProblemIndicator"));
    this._selectionReticle = ((Node) this).GetNode<NSelectionReticle>(NodePath.op_Implicit("%SelectionReticle"));
    this.Hitbox = ((Node) this).GetNode<NButton>(NodePath.op_Implicit("%Hitbox"));
    this._locationIcon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%LocationIcon"));
    this._locationContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%LocationContainer"));
    this._topContainer = ((Node) this).GetNode<HBoxContainer>(NodePath.op_Implicit("TopInfoContainer"));
    this._energyContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%EnergyCountContainer"));
    this._energyImage = ((Node) this._energyContainer).GetNode<TextureRect>(NodePath.op_Implicit("Image"));
    this._energyCount = ((Node) this._energyContainer).GetNode<MegaLabel>(NodePath.op_Implicit("EnergyCount"));
    this._starContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%StarCountContainer"));
    this._starCount = ((Node) this._starContainer).GetNode<MegaLabel>(NodePath.op_Implicit("StarCount"));
    this._cardContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%CardCountContainer"));
    this._cardImage = ((Node) this._cardContainer).GetNode<NTinyCard>(NodePath.op_Implicit("TinyCard"));
    this._cardCount = ((Node) this._cardContainer).GetNode<MegaLabel>(NodePath.op_Implicit("CardCount"));
    ((CanvasItem) this._selectionReticle).Visible = true;
    this._characterIcon.Texture = this.Player.Character.IconTexture;
    this._nameplateLabel.SetTextAutoSize(PlatformUtil.GetPlayerNameRaw(RunManager.Instance.NetService.Platform, this.Player.NetId));
    this._healthBar.SetCreature(this.Player.Creature);
    this._networkProblemIndicator.Initialize(this.Player.NetId);
    ((CanvasItem) this._locationContainer).Visible = false;
    ((CanvasItem) this._energyContainer).Visible = false;
    this._energyImage.Texture = ResourceLoader.Load<Texture2D>(this.Player.Character.CardPool.EnergyIconPath, (string) null, (ResourceLoader.CacheMode) 1L);
    ((CanvasItem) this._starContainer).Visible = false;
    ((CanvasItem) this._cardContainer).Visible = false;
    this._cardImage.Set(this.Player.Character.CardPool, CardType.Attack, CardRarity.Common);
    ((CanvasItem) this._turnEndIndicator).Visible = false;
    this._healthBar.FadeOutHpLabel(0.0f, 0.0f);
    this.Player.Creature.BlockChanged += new Action<int, int>(this.BlockChanged);
    this.Player.Creature.CurrentHpChanged += new Action<int, int>(this.OnCreatureValueChanged);
    this.Player.Creature.MaxHpChanged += new Action<int, int>(this.OnCreatureValueChanged);
    this.Player.Creature.PowerApplied += new Action<PowerModel>(this.OnPowerAppliedOrRemoved);
    this.Player.Creature.PowerIncreased += new Action<PowerModel, int, bool>(this.OnPowerIncreased);
    this.Player.Creature.PowerDecreased += new Action<PowerModel, bool>(this.OnPowerDecreased);
    this.Player.Creature.PowerRemoved += new Action<PowerModel>(this.OnPowerAppliedOrRemoved);
    this.Player.Creature.Died += new Action<Creature>(this.OnCreatureChanged);
    this.Player.RelicObtained += new Action<RelicModel>(this.OnRelicObtained);
    this.Player.RelicRemoved += new Action<RelicModel>(this.OnRelicRemoved);
    this.Player.PotionProcured += new Action<PotionModel>(this.OnPotionProcured);
    this.Player.PotionDiscarded += new Action<PotionModel>(this.OnPotionDiscarded);
    this.Player.Deck.CardAdded += new Action<CardModel>(this.OnCardObtained);
    this.Player.Deck.CardRemoved += new Action<CardModel>(this.OnCardRemovedFromDeck);
    CombatManager.Instance.PlayerEndedTurn += new Action<Player, bool>(this.RefreshPlayerReadyIndicator);
    CombatManager.Instance.PlayerUnendedTurn += new Action<Player>(this.RefreshPlayerReadyIndicator);
    CombatManager.Instance.TurnStarted += new Action<CombatState>(this.OnTurnStarted);
    CombatManager.Instance.CombatSetUp += new Action<CombatState>(this.OnCombatSetUp);
    CombatManager.Instance.CombatEnded += new Action<CombatRoom>(this.OnCombatEnded);
    RunManager.Instance.FlavorSynchronizer.OnEndTurnPingReceived += new Action<ulong>(this.OnPlayerEndTurnPing);
    RunManager.Instance.InputSynchronizer.ScreenChanged += new Action<ulong, NetScreenType>(this.OnPlayerScreenChanged);
    RunManager.Instance.MapSelectionSynchronizer.PlayerVoteChanged += new Action<Player, MapVote?, MapVote?>(this.OnPlayerVoteChanged);
    RunManager.Instance.MapSelectionSynchronizer.PlayerVoteCancelled += new Action<Player>(this.RefreshPlayerReadyIndicator);
    RunManager.Instance.MapSelectionSynchronizer.PlayerVotesCleared += new Action(this.OnPlayerVotesCleared);
    if (RunManager.Instance.RunLobby != null)
    {
      RunManager.Instance.RunLobby.RemotePlayerDisconnected += new Action<ulong>(this.RefreshConnectedState);
      RunManager.Instance.RunLobby.LocalPlayerDisconnected += new Action(this.RefreshConnectedState);
      RunManager.Instance.RunLobby.PlayerRejoined += new Action<ulong>(this.RefreshConnectedState);
    }
    ((GodotObject) this.Hitbox).Connect(NClickableControl.SignalName.Focused, Callable.From<NButton>(new Action<NButton>(this.OnFocus)), 0U);
    ((GodotObject) this.Hitbox).Connect(NClickableControl.SignalName.Unfocused, Callable.From<NButton>(new Action<NButton>(this.OnUnfocus)), 0U);
    ((GodotObject) this.Hitbox).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnRelease)), 0U);
    this.RefreshValues();
  }

  public override void _ExitTree()
  {
    ((Node) this)._ExitTree();
    this.Player.Creature.BlockChanged -= new Action<int, int>(this.BlockChanged);
    this.Player.Creature.CurrentHpChanged -= new Action<int, int>(this.OnCreatureValueChanged);
    this.Player.Creature.MaxHpChanged -= new Action<int, int>(this.OnCreatureValueChanged);
    this.Player.Creature.PowerApplied -= new Action<PowerModel>(this.OnPowerAppliedOrRemoved);
    this.Player.Creature.PowerIncreased -= new Action<PowerModel, int, bool>(this.OnPowerIncreased);
    this.Player.Creature.PowerDecreased -= new Action<PowerModel, bool>(this.OnPowerDecreased);
    this.Player.Creature.PowerRemoved -= new Action<PowerModel>(this.OnPowerAppliedOrRemoved);
    this.Player.Creature.Died -= new Action<Creature>(this.OnCreatureChanged);
    this.Player.RelicObtained -= new Action<RelicModel>(this.OnRelicObtained);
    this.Player.RelicRemoved -= new Action<RelicModel>(this.OnRelicRemoved);
    this.Player.PotionProcured -= new Action<PotionModel>(this.OnPotionProcured);
    this.Player.PotionDiscarded -= new Action<PotionModel>(this.OnPotionDiscarded);
    this.Player.Deck.CardAdded -= new Action<CardModel>(this.OnCardObtained);
    this.Player.Deck.CardRemoved -= new Action<CardModel>(this.OnCardRemovedFromDeck);
    CombatManager.Instance.PlayerEndedTurn -= new Action<Player, bool>(this.RefreshPlayerReadyIndicator);
    CombatManager.Instance.PlayerUnendedTurn -= new Action<Player>(this.RefreshPlayerReadyIndicator);
    CombatManager.Instance.TurnStarted -= new Action<CombatState>(this.OnTurnStarted);
    CombatManager.Instance.CombatSetUp -= new Action<CombatState>(this.OnCombatSetUp);
    CombatManager.Instance.CombatEnded -= new Action<CombatRoom>(this.OnCombatEnded);
    RunManager.Instance.FlavorSynchronizer.OnEndTurnPingReceived -= new Action<ulong>(this.OnPlayerEndTurnPing);
    RunManager.Instance.InputSynchronizer.ScreenChanged -= new Action<ulong, NetScreenType>(this.OnPlayerScreenChanged);
    RunManager.Instance.MapSelectionSynchronizer.PlayerVoteChanged -= new Action<Player, MapVote?, MapVote?>(this.OnPlayerVoteChanged);
    RunManager.Instance.MapSelectionSynchronizer.PlayerVoteCancelled -= new Action<Player>(this.RefreshPlayerReadyIndicator);
    RunManager.Instance.MapSelectionSynchronizer.PlayerVotesCleared -= new Action(this.OnPlayerVotesCleared);
    if (RunManager.Instance.RunLobby == null)
      return;
    RunManager.Instance.RunLobby.RemotePlayerDisconnected -= new Action<ulong>(this.RefreshConnectedState);
    RunManager.Instance.RunLobby.LocalPlayerDisconnected -= new Action(this.RefreshConnectedState);
    RunManager.Instance.RunLobby.PlayerRejoined -= new Action<ulong>(this.RefreshConnectedState);
  }

  private void OnCombatSetUp(CombatState _)
  {
    if (LocalContext.IsMe(this.Player))
      return;
    ((CanvasItem) this._energyContainer).Visible = true;
    Control starContainer = this._starContainer;
    int num;
    if (!(this.Player.Character is Regent))
    {
      PlayerCombatState playerCombatState = this.Player.PlayerCombatState;
      num = playerCombatState != null ? (playerCombatState.Stars > 0 ? 1 : 0) : 0;
    }
    else
      num = 1;
    ((CanvasItem) starContainer).Visible = num != 0;
    ((CanvasItem) this._cardContainer).Visible = true;
    this.Player.PlayerCombatState.EnergyChanged += new Action<int, int>(this.OnEnergyChanged);
    this.Player.PlayerCombatState.StarsChanged += new Action<int, int>(this.OnStarsChanged);
    this.Player.PlayerCombatState.Hand.CardAdded += new Action<CardModel>(this.OnCardAdded);
    this.Player.PlayerCombatState.Hand.CardRemoved += new Action<CardModel>(this.OnCardRemoved);
  }

  private void OnCombatEnded(CombatRoom _)
  {
    ((CanvasItem) this._turnEndIndicator).Visible = false;
    if (LocalContext.IsMe(this.Player))
      return;
    ((CanvasItem) this._energyContainer).Visible = false;
    ((CanvasItem) this._starContainer).Visible = false;
    ((CanvasItem) this._cardContainer).Visible = false;
    if (this.Player.PlayerCombatState == null)
      return;
    this.Player.PlayerCombatState.EnergyChanged -= new Action<int, int>(this.OnEnergyChanged);
    this.Player.PlayerCombatState.StarsChanged -= new Action<int, int>(this.OnStarsChanged);
    this.Player.PlayerCombatState.Hand.CardAdded -= new Action<CardModel>(this.OnCardAdded);
    this.Player.PlayerCombatState.Hand.CardRemoved -= new Action<CardModel>(this.OnCardRemoved);
  }

  private void OnCreatureValueChanged(int _, int __) => this.RefreshValues();

  private void OnCreatureChanged(Creature _) => this.RefreshValues();

  private void OnPowerAppliedOrRemoved(PowerModel _) => this.RefreshValues();

  private void OnPowerDecreased(PowerModel _, bool __) => this.RefreshValues();

  private void OnPowerIncreased(PowerModel _, int __, bool ___) => this.RefreshValues();

  private void RefreshValues()
  {
    this.UpdateHealthBarWidth();
    this._healthBar.RefreshValues();
  }

  private void UpdateHealthBarWidth()
  {
    this._healthBar.UpdateWidthRelativeToReferenceValue(80f, 175f);
  }

  private void UpdateSelectionReticleWidth()
  {
    Control control1 = (Control) null;
    foreach (Control control2 in ((IEnumerable) ((Node) this._topContainer).GetChildren(false)).OfType<Control>())
    {
      if (((CanvasItem) control2).Visible && (double) control2.Size.X > 0.0)
        control1 = control2;
    }
    float num = Mathf.Max((float) ((double) control1.GlobalPosition.X - (double) this.GlobalPosition.X + (double) control1.Size.X + 6.0), (float) ((double) this._healthBar.HpBarContainer.GlobalPosition.X - (double) this.GlobalPosition.X + (double) this._healthBar.HpBarContainer.Size.X + 6.0));
    NSelectionReticle selectionReticle = this._selectionReticle;
    StringName size1 = Control.PropertyName.Size;
    Vector2 size2 = this._selectionReticle.Size;
    size2.X = num;
    Variant variant = Variant.op_Implicit(size2);
    ((GodotObject) selectionReticle).SetDeferred(size1, variant);
    ((GodotObject) this.Hitbox).SetDeferred(Control.PropertyName.Size, Variant.op_Implicit(new Vector2(num, this.Size.Y)));
  }

  private void OnEnergyChanged(int _, int __) => this.RefreshCombatValues();

  private void OnStarsChanged(int _, int __) => this.RefreshCombatValues();

  private void OnCardAdded(CardModel _) => this.RefreshCombatValues();

  private void OnCardRemoved(CardModel _) => this.RefreshCombatValues();

  private void RefreshCombatValues()
  {
    Control starContainer = this._starContainer;
    int num;
    if (!(this.Player.Character is Regent))
    {
      PlayerCombatState playerCombatState = this.Player.PlayerCombatState;
      num = playerCombatState != null ? (playerCombatState.Stars > 0 ? 1 : 0) : 0;
    }
    else
      num = 1;
    ((CanvasItem) starContainer).Visible = num != 0;
    this._energyCount.SetTextAutoSize(this.Player.PlayerCombatState.Energy.ToString());
    this._starCount.SetTextAutoSize(this.Player.PlayerCombatState.Stars.ToString());
    this._cardCount.SetTextAutoSize(this.Player.PlayerCombatState.Hand.Cards.Count.ToString());
    ((Control) this._energyCount).AddThemeColorOverride(ThemeConstants.Label.FontColor, this.Player.PlayerCombatState.Energy == 0 ? StsColors.red : StsColors.cream);
    ((Control) this._energyCount).AddThemeColorOverride(ThemeConstants.Label.FontOutlineColor, this.Player.PlayerCombatState.Energy == 0 ? StsColors.unplayableEnergyCostOutline : this.Player.Character.EnergyLabelOutlineColor);
    ((CanvasItem) this._energyImage).Material = this.Player.PlayerCombatState.Energy == 0 ? PreloadManager.Cache.GetMaterial("res://materials/ui/energy_orb_dark.tres") : (Material) null;
    ((CanvasItem) this._energyImage).Modulate = this.Player.PlayerCombatState.Energy == 0 ? Colors.DarkGray : Colors.White;
  }

  public void OnCreatureHovered()
  {
    this._isCreatureHovered = true;
    this.UpdateHighlightedState();
  }

  public void OnCreatureUnhovered()
  {
    this._isCreatureHovered = false;
    this.UpdateHighlightedState();
  }

  public void FlashPlayerReady() => this.FlashEndTurn();

  private void UpdateHighlightedState()
  {
    bool flag = this._isMouseOver || this._isCreatureHovered;
    if (NTargetManager.Instance.IsInSelection && !NTargetManager.Instance.AllowedToTargetNode((Node) this))
      flag = false;
    if (!NTargetManager.Instance.IsInSelection)
    {
      NPlayerHand instance = NPlayerHand.Instance;
      if ((instance != null ? (instance.InCardPlay ? 1 : 0) : 0) != 0)
        flag = false;
    }
    if (this._isHighlighted == flag)
      return;
    this._isHighlighted = flag;
    if (this._isHighlighted)
    {
      this._healthBar.FadeInHpLabel(0.1f);
      this.UpdateSelectionReticleWidth();
      this._selectionReticle.OnSelect();
      if (!this._networkProblemIndicator.IsShown)
        return;
      LocString description;
      LocString title;
      if (RunManager.Instance.NetService.Type == NetGameType.Client)
      {
        description = new LocString("static_hover_tips", "NETWORK_PROBLEM_CLIENT.description");
        title = new LocString("static_hover_tips", "NETWORK_PROBLEM_CLIENT.title");
      }
      else
      {
        description = new LocString("static_hover_tips", "NETWORK_PROBLEM_HOST.description");
        title = new LocString("static_hover_tips", "NETWORK_PROBLEM_HOST.title");
      }
      description.Add("Player", PlatformUtil.GetPlayerName(RunManager.Instance.NetService.Platform, this.Player.NetId));
      NHoverTipSet.CreateAndShow((Control) this, (IHoverTip) new HoverTip(title, description))?.SetGlobalPosition(Vector2.op_Addition(this.GlobalPosition, Vector2.op_Multiply(Vector2.Down, 80f)), false);
    }
    else
    {
      this._healthBar.FadeOutHpLabel(0.5f, 0.0f);
      this._selectionReticle.OnDeselect();
      NHoverTipSet.Remove((Control) this);
    }
  }

  private void BlockChanged(int oldBlock, int blockGain)
  {
    if (oldBlock == 0 && blockGain > 0)
      this._healthBar.AnimateInBlock(oldBlock, blockGain);
    this._healthBar.RefreshValues();
  }

  private void RefreshConnectedState(ulong _) => this.RefreshConnectedState();

  private void RefreshConnectedState()
  {
    bool flag = RunManager.Instance.RunLobby.ConnectedPlayerIds.Contains<ulong>(this.Player.NetId);
    ((CanvasItem) this._disconnectedIndicator).Visible = !flag;
    ((CanvasItem) this._characterIcon).SelfModulate = flag ? Colors.White : StsColors.gray;
  }

  private void OnRelicObtained(RelicModel relic)
  {
    if (LocalContext.IsMe(this.Player))
      return;
    TaskHelper.RunSafely(this.AnimateRelicObtained(relic));
  }

  private async Task AnimateRelicObtained(RelicModel relic)
  {
    await this.WaitUntilNextTweenTime();
    NRelic relicImage = NRelic.Create(relic, NRelic.IconSize.Small);
    relicImage.Model = relic;
    await this.ObtainedAnimation((Control) relicImage);
    ((Node) relicImage).QueueFreeSafely();
    relicImage = (NRelic) null;
  }

  private void OnRelicRemoved(RelicModel relic)
  {
    if (LocalContext.IsMe(this.Player))
      return;
    TaskHelper.RunSafely(this.AnimateRelicRemoved(relic));
  }

  private async Task AnimateRelicRemoved(RelicModel relic)
  {
    await this.WaitUntilNextTweenTime();
    NRelic relicImage = NRelic.Create(relic, NRelic.IconSize.Small);
    relicImage.Model = relic;
    await this.RemovedAnimation((Control) relicImage);
    ((Node) relicImage).QueueFreeSafely();
    relicImage = (NRelic) null;
  }

  private void OnCardObtained(CardModel card)
  {
    if (LocalContext.IsMe(this.Player))
      return;
    TaskHelper.RunSafely(this.AnimateCardObtained(card));
  }

  private async Task AnimateCardObtained(CardModel card)
  {
    await this.WaitUntilNextTweenTime();
    NDeckHistoryEntry cardNode = NDeckHistoryEntry.Create(card, 1);
    await this.ObtainedAnimation((Control) cardNode);
    ((Node) cardNode).QueueFreeSafely();
    cardNode = (NDeckHistoryEntry) null;
  }

  private void OnCardRemovedFromDeck(CardModel card)
  {
    if (LocalContext.IsMe(this.Player))
      return;
    TaskHelper.RunSafely(this.AnimateCardRemovedFromDeck(card));
  }

  private async Task AnimateCardRemovedFromDeck(CardModel card)
  {
    await this.WaitUntilNextTweenTime();
    NDeckHistoryEntry cardNode = NDeckHistoryEntry.Create(card, 1);
    await this.RemovedAnimation((Control) cardNode);
    ((Node) cardNode).QueueFreeSafely();
    cardNode = (NDeckHistoryEntry) null;
  }

  private void OnPotionProcured(PotionModel potion)
  {
    if (LocalContext.IsMe(this.Player))
      return;
    TaskHelper.RunSafely(this.AnimatePotionObtained(potion));
  }

  private async Task AnimatePotionObtained(PotionModel potion)
  {
    await this.WaitUntilNextTweenTime();
    NPotion node = NPotion.Create(potion);
    await this.ObtainedAnimation((Control) node);
    ((Node) node).QueueFreeSafely();
    node = (NPotion) null;
  }

  private void OnPotionDiscarded(PotionModel potion)
  {
    if (LocalContext.IsMe(this.Player))
      return;
    TaskHelper.RunSafely(this.AnimatePotionDiscarded(potion));
  }

  private async Task AnimatePotionDiscarded(PotionModel potion)
  {
    await this.WaitUntilNextTweenTime();
    NPotion node = NPotion.Create(potion);
    await this.RemovedAnimation((Control) node);
    ((Node) node).QueueFreeSafely();
    node = (NPotion) null;
  }

  private void OnPlayerVoteChanged(Player player, MapVote? _, MapVote? __)
  {
    this.RefreshPlayerReadyIndicator(player);
  }

  private void OnPlayerVotesCleared() => this.RefreshPlayerReadyIndicator(this.Player);

  private void RefreshPlayerReadyIndicator(Player player, bool _)
  {
    this.RefreshPlayerReadyIndicator(player);
  }

  private void RefreshPlayerReadyIndicator(Player player)
  {
    if (CombatManager.Instance.IsInProgress)
      ((CanvasItem) this._turnEndIndicator).Visible = CombatManager.Instance.IsPlayerReadyToEndTurn(this.Player);
    else
      ((CanvasItem) this._turnEndIndicator).Visible = RunManager.Instance.MapSelectionSynchronizer.GetVote(this.Player).HasValue;
    if (!((CanvasItem) this._turnEndIndicator).Visible || player != this.Player)
      return;
    this.FlashEndTurn();
  }

  private void OnPlayerEndTurnPing(ulong playerId)
  {
    if ((long) this.Player.NetId != (long) playerId)
      return;
    this.FlashEndTurn();
  }

  private void FlashEndTurn()
  {
    NUiFlashVfx child = NUiFlashVfx.Create(this._turnEndIndicator.Texture, ((CanvasItem) this._turnEndIndicator).SelfModulate);
    ((Node) this._turnEndIndicator).AddChildSafely((Node) child);
    ((GodotObject) child).SetDeferred(Control.PropertyName.Size, Variant.op_Implicit(((Control) this._turnEndIndicator).Size));
    child.Position = Vector2.Zero;
    TaskHelper.RunSafely(child.StartVfx());
  }

  private void OnTurnStarted(CombatState _)
  {
    ((CanvasItem) this._turnEndIndicator).Visible = CombatManager.Instance.IsPlayerReadyToEndTurn(this.Player);
  }

  private void SetNextTweenTime()
  {
    ulong ticksMsec = Time.GetTicksMsec();
    if (this._nextTweenTime > ticksMsec)
      this._nextTweenTime += 500UL;
    else
      this._nextTweenTime = ticksMsec + 500UL;
  }

  private async Task WaitUntilNextTweenTime()
  {
    ulong nextTweenTime = this._nextTweenTime;
    this.SetNextTweenTime();
    if (nextTweenTime < Time.GetTicksMsec())
      return;
    double num = (double) (nextTweenTime - Time.GetTicksMsec()) / 1000.0;
    await ((GodotObject) ((Node) this).GetTree().CreateTimer(num, true, false, false)).AwaitSignal(SceneTreeTimer.SignalName.Timeout, (Node) this);
  }

  private async Task ObtainedAnimation(Control node)
  {
    ((Node) this).AddChildSafely((Node) node);
    node.Position = new Vector2(this.Size.X + 40f, 0.0f);
    node.Scale = Vector2.op_Multiply(Vector2.One, 1.1f);
    Tween tween = ((Node) node).CreateTween();
    tween.TweenProperty((GodotObject) node, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.30000001192092896).SetEase((Tween.EaseType) 0L).SetTrans((Tween.TransitionType) 9L);
    tween.TweenProperty((GodotObject) node, NodePath.op_Implicit("position:x"), Variant.op_Implicit(this.Size.X - node.Size.X), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L).SetDelay(0.30000001192092896);
    tween.TweenProperty((GodotObject) node, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L).SetDelay(0.30000001192092896);
    bool flag = await tween.AwaitFinished((Node) this);
  }

  private async Task RemovedAnimation(Control node)
  {
    ((Node) this).AddChildSafely((Node) node);
    node.Position = new Vector2(this.Size.X - node.Size.X, 0.0f);
    Control control = node;
    Color modulate = ((CanvasItem) node).Modulate;
    modulate.A = 0.0f;
    Color color = modulate;
    ((CanvasItem) control).Modulate = color;
    Tween tween = ((Node) node).CreateTween();
    tween.TweenProperty((GodotObject) node, NodePath.op_Implicit("position:x"), Variant.op_Implicit(this.Size.X + 40f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    tween.TweenProperty((GodotObject) node, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    tween.TweenProperty((GodotObject) node, NodePath.op_Implicit("scale:y"), Variant.op_Implicit(0.0f), 0.30000001192092896).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L).SetDelay(0.5);
    bool flag = await tween.AwaitFinished((Node) this);
  }

  private void OnPlayerScreenChanged(ulong playerId, NetScreenType _)
  {
    if ((long) this.Player.NetId != (long) playerId || LocalContext.IsMe(this.Player))
      return;
    Texture2D locationIcon = RunManager.Instance.InputSynchronizer.GetScreenType(playerId).GetLocationIcon();
    if (this._currentLocationIcon == locationIcon)
      return;
    this._currentLocationIcon = locationIcon;
    if (locationIcon == null)
      this.TweenLocationIconAway();
    else
      this.TweenLocationIconIn(locationIcon);
  }

  private void TweenLocationIconAway()
  {
    this._locationIconTween?.Kill();
    this._locationIconTween = ((Node) this._locationIcon).CreateTween();
    this._locationIconTween.TweenProperty((GodotObject) this._locationIcon, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.Zero), 0.4).SetTrans((Tween.TransitionType) 10L).SetEase((Tween.EaseType) 0L);
    this._locationIconTween.TweenCallback(Callable.From<bool>((Func<bool>) (() => ((CanvasItem) this._locationContainer).Visible = false)));
  }

  private void TweenLocationIconIn(Texture2D? texture)
  {
    this._locationIconTween?.Kill();
    this._locationIconTween = ((Node) this._locationIcon).CreateTween();
    if (!((CanvasItem) this._locationContainer).Visible)
    {
      ((CanvasItem) this._locationContainer).Visible = true;
      this._locationIcon.Texture = texture;
      this._locationIconTween.TweenProperty((GodotObject) this._locationIcon, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.4).SetTrans((Tween.TransitionType) 10L).SetEase((Tween.EaseType) 1L);
    }
    else
    {
      this._locationIconTween.TweenProperty((GodotObject) this._locationIcon, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 0.5f)), 0.2).SetTrans((Tween.TransitionType) 10L).SetEase((Tween.EaseType) 0L);
      this._locationIconTween.TweenCallback(Callable.From<Texture2D>((Func<Texture2D>) (() => this._locationIcon.Texture = texture)));
      this._locationIconTween.TweenProperty((GodotObject) this._locationIcon, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.3).SetTrans((Tween.TransitionType) 10L).SetEase((Tween.EaseType) 1L);
    }
  }

  protected void OnFocus(NButton _)
  {
    this._isMouseOver = true;
    this.UpdateHighlightedState();
    if (!NTargetManager.Instance.IsInSelection || !NTargetManager.Instance.AllowedToTargetNode((Node) this))
      return;
    NTargetManager.Instance.OnNodeHovered((Node) this);
    this._focusedWhileTargeting = true;
  }

  protected void OnUnfocus(NButton _)
  {
    this._isMouseOver = false;
    this.UpdateHighlightedState();
    if (this._focusedWhileTargeting)
      NTargetManager.Instance.OnNodeUnhovered((Node) this);
    this._focusedWhileTargeting = false;
  }

  protected void OnRelease(NButton _)
  {
    if (NTargetManager.Instance.IsInSelection || NTargetManager.Instance.LastTargetingFinishedFrame == ((Node) this).GetTree().GetFrame())
      return;
    NCapstoneContainer.Instance.Open((ICapstoneScreen) NMultiplayerPlayerExpandedState.Create(this.Player));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(26)
    {
      new MethodInfo(NMultiplayerPlayerState.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerState.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerState.MethodName.OnCreatureValueChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("__"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerState.MethodName.RefreshValues, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerState.MethodName.UpdateHealthBarWidth, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerState.MethodName.UpdateSelectionReticleWidth, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerState.MethodName.OnEnergyChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("__"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerState.MethodName.OnStarsChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("__"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerState.MethodName.RefreshCombatValues, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerState.MethodName.OnCreatureHovered, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerState.MethodName.OnCreatureUnhovered, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerState.MethodName.FlashPlayerReady, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerState.MethodName.UpdateHighlightedState, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerState.MethodName.BlockChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("oldBlock"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("blockGain"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerState.MethodName.RefreshConnectedState, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerState.MethodName.RefreshConnectedState, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerState.MethodName.OnPlayerVotesCleared, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerState.MethodName.OnPlayerEndTurnPing, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("playerId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerState.MethodName.FlashEndTurn, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerState.MethodName.SetNextTweenTime, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerState.MethodName.OnPlayerScreenChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("playerId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerState.MethodName.TweenLocationIconAway, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerState.MethodName.TweenLocationIconIn, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("texture"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Texture2D"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerState.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerState.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerState.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.OnCreatureValueChanged) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.OnCreatureValueChanged(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.RefreshValues) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshValues();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.UpdateHealthBarWidth) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateHealthBarWidth();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.UpdateSelectionReticleWidth) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateSelectionReticleWidth();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.OnEnergyChanged) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.OnEnergyChanged(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.OnStarsChanged) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.OnStarsChanged(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.RefreshCombatValues) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshCombatValues();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.OnCreatureHovered) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnCreatureHovered();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.OnCreatureUnhovered) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnCreatureUnhovered();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.FlashPlayerReady) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.FlashPlayerReady();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.UpdateHighlightedState) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateHighlightedState();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.BlockChanged) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.BlockChanged(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.RefreshConnectedState) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.RefreshConnectedState(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.RefreshConnectedState) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshConnectedState();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.OnPlayerVotesCleared) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPlayerVotesCleared();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.OnPlayerEndTurnPing) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnPlayerEndTurnPing(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.FlashEndTurn) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.FlashEndTurn();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.SetNextTweenTime) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SetNextTweenTime();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.OnPlayerScreenChanged) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.OnPlayerScreenChanged(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<NetScreenType>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.TweenLocationIconAway) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TweenLocationIconAway();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.TweenLocationIconIn) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.TweenLocationIconIn(VariantUtils.ConvertTo<Texture2D>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnFocus(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnUnfocus(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.OnRelease) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnRelease(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName._Ready) || StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName._ExitTree) || StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.OnCreatureValueChanged) || StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.RefreshValues) || StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.UpdateHealthBarWidth) || StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.UpdateSelectionReticleWidth) || StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.OnEnergyChanged) || StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.OnStarsChanged) || StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.RefreshCombatValues) || StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.OnCreatureHovered) || StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.OnCreatureUnhovered) || StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.FlashPlayerReady) || StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.UpdateHighlightedState) || StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.BlockChanged) || StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.RefreshConnectedState) || StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.OnPlayerVotesCleared) || StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.OnPlayerEndTurnPing) || StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.FlashEndTurn) || StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.SetNextTweenTime) || StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.OnPlayerScreenChanged) || StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.TweenLocationIconAway) || StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.TweenLocationIconIn) || StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.OnFocus) || StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NMultiplayerPlayerState.MethodName.OnRelease) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName.Hitbox))
    {
      this.Hitbox = VariantUtils.ConvertTo<NButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._healthBar))
    {
      this._healthBar = VariantUtils.ConvertTo<NHealthBar>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._characterIcon))
    {
      this._characterIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._nameplateLabel))
    {
      this._nameplateLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._topContainer))
    {
      this._topContainer = VariantUtils.ConvertTo<HBoxContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._turnEndIndicator))
    {
      this._turnEndIndicator = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._disconnectedIndicator))
    {
      this._disconnectedIndicator = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._networkProblemIndicator))
    {
      this._networkProblemIndicator = VariantUtils.ConvertTo<NMultiplayerNetworkProblemIndicator>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._selectionReticle))
    {
      this._selectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._locationIcon))
    {
      this._locationIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._locationContainer))
    {
      this._locationContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._energyContainer))
    {
      this._energyContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._energyImage))
    {
      this._energyImage = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._energyCount))
    {
      this._energyCount = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._starContainer))
    {
      this._starContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._starCount))
    {
      this._starCount = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._cardContainer))
    {
      this._cardContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._cardImage))
    {
      this._cardImage = VariantUtils.ConvertTo<NTinyCard>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._cardCount))
    {
      this._cardCount = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._locationIconTween))
    {
      this._locationIconTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._isMouseOver))
    {
      this._isMouseOver = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._isCreatureHovered))
    {
      this._isCreatureHovered = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._isHighlighted))
    {
      this._isHighlighted = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._focusedWhileTargeting))
    {
      this._focusedWhileTargeting = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._nextTweenTime))
    {
      this._nextTweenTime = VariantUtils.ConvertTo<ulong>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._currentLocationIcon))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._currentLocationIcon = VariantUtils.ConvertTo<Texture2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName.Hitbox))
    {
      ref godot_variant local = ref value;
      NButton hitbox = this.Hitbox;
      godot_variant from = VariantUtils.CreateFrom<NButton>(ref hitbox);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._healthBar))
    {
      value = VariantUtils.CreateFrom<NHealthBar>(ref this._healthBar);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._characterIcon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._characterIcon);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._nameplateLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._nameplateLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._topContainer))
    {
      value = VariantUtils.CreateFrom<HBoxContainer>(ref this._topContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._turnEndIndicator))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._turnEndIndicator);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._disconnectedIndicator))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._disconnectedIndicator);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._networkProblemIndicator))
    {
      value = VariantUtils.CreateFrom<NMultiplayerNetworkProblemIndicator>(ref this._networkProblemIndicator);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._selectionReticle))
    {
      value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._selectionReticle);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._locationIcon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._locationIcon);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._locationContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._locationContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._energyContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._energyContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._energyImage))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._energyImage);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._energyCount))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._energyCount);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._starContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._starContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._starCount))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._starCount);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._cardContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._cardContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._cardImage))
    {
      value = VariantUtils.CreateFrom<NTinyCard>(ref this._cardImage);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._cardCount))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._cardCount);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._locationIconTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._locationIconTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._isMouseOver))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isMouseOver);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._isCreatureHovered))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isCreatureHovered);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._isHighlighted))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isHighlighted);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._focusedWhileTargeting))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._focusedWhileTargeting);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._nextTweenTime))
    {
      value = VariantUtils.CreateFrom<ulong>(ref this._nextTweenTime);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMultiplayerPlayerState.PropertyName._currentLocationIcon))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Texture2D>(ref this._currentLocationIcon);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerState.PropertyName._healthBar, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerState.PropertyName._characterIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerState.PropertyName._nameplateLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerState.PropertyName._topContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerState.PropertyName._turnEndIndicator, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerState.PropertyName._disconnectedIndicator, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerState.PropertyName._networkProblemIndicator, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerState.PropertyName._selectionReticle, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerState.PropertyName.Hitbox, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerState.PropertyName._locationIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerState.PropertyName._locationContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerState.PropertyName._energyContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerState.PropertyName._energyImage, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerState.PropertyName._energyCount, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerState.PropertyName._starContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerState.PropertyName._starCount, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerState.PropertyName._cardContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerState.PropertyName._cardImage, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerState.PropertyName._cardCount, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerState.PropertyName._locationIconTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NMultiplayerPlayerState.PropertyName._isMouseOver, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NMultiplayerPlayerState.PropertyName._isCreatureHovered, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NMultiplayerPlayerState.PropertyName._isHighlighted, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NMultiplayerPlayerState.PropertyName._focusedWhileTargeting, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NMultiplayerPlayerState.PropertyName._nextTweenTime, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerState.PropertyName._currentLocationIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName hitbox1 = NMultiplayerPlayerState.PropertyName.Hitbox;
    NButton hitbox2 = this.Hitbox;
    Variant variant = Variant.From<NButton>(ref hitbox2);
    serializationInfo.AddProperty(hitbox1, variant);
    info.AddProperty(NMultiplayerPlayerState.PropertyName._healthBar, Variant.From<NHealthBar>(ref this._healthBar));
    info.AddProperty(NMultiplayerPlayerState.PropertyName._characterIcon, Variant.From<TextureRect>(ref this._characterIcon));
    info.AddProperty(NMultiplayerPlayerState.PropertyName._nameplateLabel, Variant.From<MegaLabel>(ref this._nameplateLabel));
    info.AddProperty(NMultiplayerPlayerState.PropertyName._topContainer, Variant.From<HBoxContainer>(ref this._topContainer));
    info.AddProperty(NMultiplayerPlayerState.PropertyName._turnEndIndicator, Variant.From<TextureRect>(ref this._turnEndIndicator));
    info.AddProperty(NMultiplayerPlayerState.PropertyName._disconnectedIndicator, Variant.From<TextureRect>(ref this._disconnectedIndicator));
    info.AddProperty(NMultiplayerPlayerState.PropertyName._networkProblemIndicator, Variant.From<NMultiplayerNetworkProblemIndicator>(ref this._networkProblemIndicator));
    info.AddProperty(NMultiplayerPlayerState.PropertyName._selectionReticle, Variant.From<NSelectionReticle>(ref this._selectionReticle));
    info.AddProperty(NMultiplayerPlayerState.PropertyName._locationIcon, Variant.From<TextureRect>(ref this._locationIcon));
    info.AddProperty(NMultiplayerPlayerState.PropertyName._locationContainer, Variant.From<Control>(ref this._locationContainer));
    info.AddProperty(NMultiplayerPlayerState.PropertyName._energyContainer, Variant.From<Control>(ref this._energyContainer));
    info.AddProperty(NMultiplayerPlayerState.PropertyName._energyImage, Variant.From<TextureRect>(ref this._energyImage));
    info.AddProperty(NMultiplayerPlayerState.PropertyName._energyCount, Variant.From<MegaLabel>(ref this._energyCount));
    info.AddProperty(NMultiplayerPlayerState.PropertyName._starContainer, Variant.From<Control>(ref this._starContainer));
    info.AddProperty(NMultiplayerPlayerState.PropertyName._starCount, Variant.From<MegaLabel>(ref this._starCount));
    info.AddProperty(NMultiplayerPlayerState.PropertyName._cardContainer, Variant.From<Control>(ref this._cardContainer));
    info.AddProperty(NMultiplayerPlayerState.PropertyName._cardImage, Variant.From<NTinyCard>(ref this._cardImage));
    info.AddProperty(NMultiplayerPlayerState.PropertyName._cardCount, Variant.From<MegaLabel>(ref this._cardCount));
    info.AddProperty(NMultiplayerPlayerState.PropertyName._locationIconTween, Variant.From<Tween>(ref this._locationIconTween));
    info.AddProperty(NMultiplayerPlayerState.PropertyName._isMouseOver, Variant.From<bool>(ref this._isMouseOver));
    info.AddProperty(NMultiplayerPlayerState.PropertyName._isCreatureHovered, Variant.From<bool>(ref this._isCreatureHovered));
    info.AddProperty(NMultiplayerPlayerState.PropertyName._isHighlighted, Variant.From<bool>(ref this._isHighlighted));
    info.AddProperty(NMultiplayerPlayerState.PropertyName._focusedWhileTargeting, Variant.From<bool>(ref this._focusedWhileTargeting));
    info.AddProperty(NMultiplayerPlayerState.PropertyName._nextTweenTime, Variant.From<ulong>(ref this._nextTweenTime));
    info.AddProperty(NMultiplayerPlayerState.PropertyName._currentLocationIcon, Variant.From<Texture2D>(ref this._currentLocationIcon));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName.Hitbox, ref variant1))
      this.Hitbox = ((Variant) ref variant1).As<NButton>();
    Variant variant2;
    if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._healthBar, ref variant2))
      this._healthBar = ((Variant) ref variant2).As<NHealthBar>();
    Variant variant3;
    if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._characterIcon, ref variant3))
      this._characterIcon = ((Variant) ref variant3).As<TextureRect>();
    Variant variant4;
    if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._nameplateLabel, ref variant4))
      this._nameplateLabel = ((Variant) ref variant4).As<MegaLabel>();
    Variant variant5;
    if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._topContainer, ref variant5))
      this._topContainer = ((Variant) ref variant5).As<HBoxContainer>();
    Variant variant6;
    if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._turnEndIndicator, ref variant6))
      this._turnEndIndicator = ((Variant) ref variant6).As<TextureRect>();
    Variant variant7;
    if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._disconnectedIndicator, ref variant7))
      this._disconnectedIndicator = ((Variant) ref variant7).As<TextureRect>();
    Variant variant8;
    if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._networkProblemIndicator, ref variant8))
      this._networkProblemIndicator = ((Variant) ref variant8).As<NMultiplayerNetworkProblemIndicator>();
    Variant variant9;
    if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._selectionReticle, ref variant9))
      this._selectionReticle = ((Variant) ref variant9).As<NSelectionReticle>();
    Variant variant10;
    if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._locationIcon, ref variant10))
      this._locationIcon = ((Variant) ref variant10).As<TextureRect>();
    Variant variant11;
    if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._locationContainer, ref variant11))
      this._locationContainer = ((Variant) ref variant11).As<Control>();
    Variant variant12;
    if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._energyContainer, ref variant12))
      this._energyContainer = ((Variant) ref variant12).As<Control>();
    Variant variant13;
    if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._energyImage, ref variant13))
      this._energyImage = ((Variant) ref variant13).As<TextureRect>();
    Variant variant14;
    if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._energyCount, ref variant14))
      this._energyCount = ((Variant) ref variant14).As<MegaLabel>();
    Variant variant15;
    if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._starContainer, ref variant15))
      this._starContainer = ((Variant) ref variant15).As<Control>();
    Variant variant16;
    if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._starCount, ref variant16))
      this._starCount = ((Variant) ref variant16).As<MegaLabel>();
    Variant variant17;
    if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._cardContainer, ref variant17))
      this._cardContainer = ((Variant) ref variant17).As<Control>();
    Variant variant18;
    if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._cardImage, ref variant18))
      this._cardImage = ((Variant) ref variant18).As<NTinyCard>();
    Variant variant19;
    if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._cardCount, ref variant19))
      this._cardCount = ((Variant) ref variant19).As<MegaLabel>();
    Variant variant20;
    if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._locationIconTween, ref variant20))
      this._locationIconTween = ((Variant) ref variant20).As<Tween>();
    Variant variant21;
    if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._isMouseOver, ref variant21))
      this._isMouseOver = ((Variant) ref variant21).As<bool>();
    Variant variant22;
    if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._isCreatureHovered, ref variant22))
      this._isCreatureHovered = ((Variant) ref variant22).As<bool>();
    Variant variant23;
    if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._isHighlighted, ref variant23))
      this._isHighlighted = ((Variant) ref variant23).As<bool>();
    Variant variant24;
    if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._focusedWhileTargeting, ref variant24))
      this._focusedWhileTargeting = ((Variant) ref variant24).As<bool>();
    Variant variant25;
    if (info.TryGetProperty(NMultiplayerPlayerState.PropertyName._nextTweenTime, ref variant25))
      this._nextTweenTime = ((Variant) ref variant25).As<ulong>();
    Variant variant26;
    if (!info.TryGetProperty(NMultiplayerPlayerState.PropertyName._currentLocationIcon, ref variant26))
      return;
    this._currentLocationIcon = ((Variant) ref variant26).As<Texture2D>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName OnCreatureValueChanged = StringName.op_Implicit(nameof (OnCreatureValueChanged));
    public static readonly StringName RefreshValues = StringName.op_Implicit(nameof (RefreshValues));
    public static readonly StringName UpdateHealthBarWidth = StringName.op_Implicit(nameof (UpdateHealthBarWidth));
    public static readonly StringName UpdateSelectionReticleWidth = StringName.op_Implicit(nameof (UpdateSelectionReticleWidth));
    public static readonly StringName OnEnergyChanged = StringName.op_Implicit(nameof (OnEnergyChanged));
    public static readonly StringName OnStarsChanged = StringName.op_Implicit(nameof (OnStarsChanged));
    public static readonly StringName RefreshCombatValues = StringName.op_Implicit(nameof (RefreshCombatValues));
    public static readonly StringName OnCreatureHovered = StringName.op_Implicit(nameof (OnCreatureHovered));
    public static readonly StringName OnCreatureUnhovered = StringName.op_Implicit(nameof (OnCreatureUnhovered));
    public static readonly StringName FlashPlayerReady = StringName.op_Implicit(nameof (FlashPlayerReady));
    public static readonly StringName UpdateHighlightedState = StringName.op_Implicit(nameof (UpdateHighlightedState));
    public static readonly StringName BlockChanged = StringName.op_Implicit(nameof (BlockChanged));
    public static readonly StringName RefreshConnectedState = StringName.op_Implicit(nameof (RefreshConnectedState));
    public static readonly StringName OnPlayerVotesCleared = StringName.op_Implicit(nameof (OnPlayerVotesCleared));
    public static readonly StringName OnPlayerEndTurnPing = StringName.op_Implicit(nameof (OnPlayerEndTurnPing));
    public static readonly StringName FlashEndTurn = StringName.op_Implicit(nameof (FlashEndTurn));
    public static readonly StringName SetNextTweenTime = StringName.op_Implicit(nameof (SetNextTweenTime));
    public static readonly StringName OnPlayerScreenChanged = StringName.op_Implicit(nameof (OnPlayerScreenChanged));
    public static readonly StringName TweenLocationIconAway = StringName.op_Implicit(nameof (TweenLocationIconAway));
    public static readonly StringName TweenLocationIconIn = StringName.op_Implicit(nameof (TweenLocationIconIn));
    public static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName Hitbox = StringName.op_Implicit(nameof (Hitbox));
    public static readonly StringName _healthBar = StringName.op_Implicit(nameof (_healthBar));
    public static readonly StringName _characterIcon = StringName.op_Implicit(nameof (_characterIcon));
    public static readonly StringName _nameplateLabel = StringName.op_Implicit(nameof (_nameplateLabel));
    public static readonly StringName _topContainer = StringName.op_Implicit(nameof (_topContainer));
    public static readonly StringName _turnEndIndicator = StringName.op_Implicit(nameof (_turnEndIndicator));
    public static readonly StringName _disconnectedIndicator = StringName.op_Implicit(nameof (_disconnectedIndicator));
    public static readonly StringName _networkProblemIndicator = StringName.op_Implicit(nameof (_networkProblemIndicator));
    public static readonly StringName _selectionReticle = StringName.op_Implicit(nameof (_selectionReticle));
    public static readonly StringName _locationIcon = StringName.op_Implicit(nameof (_locationIcon));
    public static readonly StringName _locationContainer = StringName.op_Implicit(nameof (_locationContainer));
    public static readonly StringName _energyContainer = StringName.op_Implicit(nameof (_energyContainer));
    public static readonly StringName _energyImage = StringName.op_Implicit(nameof (_energyImage));
    public static readonly StringName _energyCount = StringName.op_Implicit(nameof (_energyCount));
    public static readonly StringName _starContainer = StringName.op_Implicit(nameof (_starContainer));
    public static readonly StringName _starCount = StringName.op_Implicit(nameof (_starCount));
    public static readonly StringName _cardContainer = StringName.op_Implicit(nameof (_cardContainer));
    public static readonly StringName _cardImage = StringName.op_Implicit(nameof (_cardImage));
    public static readonly StringName _cardCount = StringName.op_Implicit(nameof (_cardCount));
    public static readonly StringName _locationIconTween = StringName.op_Implicit(nameof (_locationIconTween));
    public static readonly StringName _isMouseOver = StringName.op_Implicit(nameof (_isMouseOver));
    public static readonly StringName _isCreatureHovered = StringName.op_Implicit(nameof (_isCreatureHovered));
    public static readonly StringName _isHighlighted = StringName.op_Implicit(nameof (_isHighlighted));
    public static readonly StringName _focusedWhileTargeting = StringName.op_Implicit(nameof (_focusedWhileTargeting));
    public static readonly StringName _nextTweenTime = StringName.op_Implicit(nameof (_nextTweenTime));
    public static readonly StringName _currentLocationIcon = StringName.op_Implicit(nameof (_currentLocationIcon));
  }

  public class SignalName : Control.SignalName
  {
  }
}
