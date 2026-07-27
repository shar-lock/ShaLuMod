// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Combat.CombatManager
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Combat.History;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Ftue;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Combat;

public class CombatManager
{
  public const int baseHandDrawCount = 5;
  private readonly Lock _playerReadyLock = new Lock();
  private readonly HashSet<Player> _playersReadyToEndTurn = new HashSet<Player>();
  private readonly HashSet<Player> _playersReadyToBeginEnemyTurn = new HashSet<Player>();
  private readonly List<Player> _playersTakingExtraTurn = new List<Player>();
  private bool _playerToEnemyTransitionFired;
  private bool _inPlayerTurnSetup;
  private Func<Task>? _deferredEndTurnTransition;
  private CombatState? _state;
  private CancellationTokenSource? _combatCts;
  private CombatManager.PendingLossState? _pendingLoss;
  private bool _playerActionsDisabled;
  private readonly Dictionary<Player, int> _cardOrPotionEffectDepth = new Dictionary<Player, int>();

  public static CombatManager Instance { get; } = new CombatManager();

  public event Action<CombatState>? CombatSetUp;

  public event Action<CombatState>? CombatBegan;

  public event Action<CombatRoom>? CombatEnded;

  public event Action<CombatRoom>? CombatWon;

  public event Action<CombatState>? CreaturesChanged;

  public event Action<CombatState>? TurnStarted;

  public event Action<CombatState>? TurnEnded;

  public event Action<Player, bool>? PlayerEndedTurn;

  public event Action<Player>? PlayerUnendedTurn;

  public event Action<CombatState>? AboutToSwitchToEnemyTurn;

  public event Action<CombatState>? PlayerActionsDisabledChanged;

  private CancellationToken CombatCt
  {
    get
    {
      CancellationTokenSource combatCts = this._combatCts;
      return combatCts == null ? new CancellationToken() : combatCts.Token;
    }
  }

  public CardModel? DebugForcedTopCardOnNextShuffle { get; private set; }

  public CombatState? DebugOnlyGetState() => this._state;

  public bool IsPaused { get; private set; }

  public bool PlayerActionsDisabled
  {
    get => this._playerActionsDisabled;
    private set
    {
      if (this._playerActionsDisabled == value)
        return;
      this._playerActionsDisabled = value;
      Action<CombatState> actionsDisabledChanged = this.PlayerActionsDisabledChanged;
      if (actionsDisabledChanged == null)
        return;
      actionsDisabledChanged(this._state);
    }
  }

  public IReadOnlyList<Player> PlayersTakingExtraTurn
  {
    get
    {
      Lock.Scope scope = this._playerReadyLock.EnterScope();
      try
      {
        return (IReadOnlyList<Player>) this._playersTakingExtraTurn.ToList<Player>();
      }
      finally
      {
        ((Lock.Scope) ref scope).Dispose();
      }
    }
  }

  private void SetPhaseForAllPlayers(PlayerTurnPhase phase)
  {
    if (this._state == null)
      return;
    foreach (Player player in (IEnumerable<Player>) this._state.Players)
    {
      if (player.PlayerCombatState != null)
        player.PlayerCombatState.Phase = phase;
    }
  }

  public bool IsEnemyTurnStarted { get; private set; }

  public bool EndingPlayerTurnPhaseTwo { get; private set; }

  public bool EndingPlayerTurnPhaseOne { get; private set; }

  public CombatStateTracker StateTracker { get; }

  public CombatHistory History { get; }

  public bool IsInProgress { get; private set; }

  public bool IsStarting { get; private set; }

  public bool IsExecutingCardOrPotionEffect(Player player)
  {
    return CollectionExtensions.GetValueOrDefault<Player, int>((IReadOnlyDictionary<Player, int>) this._cardOrPotionEffectDepth, player) > 0;
  }

  public void BeginCardOrPotionEffect(Player player)
  {
    this._cardOrPotionEffectDepth[player] = CollectionExtensions.GetValueOrDefault<Player, int>((IReadOnlyDictionary<Player, int>) this._cardOrPotionEffectDepth, player) + 1;
  }

  public async Task EndCardOrPotionEffect(Player player)
  {
    int num = CollectionExtensions.GetValueOrDefault<Player, int>((IReadOnlyDictionary<Player, int>) this._cardOrPotionEffectDepth, player) - 1;
    if (num <= 0)
    {
      this._cardOrPotionEffectDepth.Remove(player);
      if (!player.Creature.IsDead)
        return;
      await this.RemoveDeadPlayerCardsFromCombat(player);
    }
    else
      this._cardOrPotionEffectDepth[player] = num;
  }

  public bool IsAboutToLose => this._pendingLoss != (CombatManager.PendingLossState) null;

  public bool IsEnding
  {
    get
    {
      return this.IsInProgress && (this._pendingLoss != (CombatManager.PendingLossState) null || (this._state == null || !this._state.Enemies.Any<Creature>((Func<Creature, bool>) (e => e != null && e.IsAlive && e.IsPrimaryEnemy))) && !Hook.ShouldStopCombatFromEnding((ICombatState) this._state));
    }
  }

  public bool IsOverOrEnding => this.IsEnding || !this.IsInProgress;

  private CombatManager()
  {
    this.History = new CombatHistory();
    this.StateTracker = new CombatStateTracker(this);
  }

  public void SetUpCombat(CombatState state)
  {
    if (this._state != null)
      throw new InvalidOperationException("Make sure to reset the combat before setting up a new one.");
    this.IsStarting = true;
    this._state = state;
    this._state.MultiplayerScalingModel?.OnCombatEntered(this._state);
    this.StateTracker.SetState(state);
    Lock.Scope scope = this._playerReadyLock.EnterScope();
    try
    {
      this._playersTakingExtraTurn.Clear();
    }
    finally
    {
      ((Lock.Scope) ref scope).Dispose();
    }
    foreach (Player player in (IEnumerable<Player>) state.Players)
      player.ResetCombatState();
    foreach (Player player in (IEnumerable<Player>) state.Players)
      player.PopulateCombatState(player.RunState.Rng.Shuffle, state);
    NetCombatCardDb.Instance.StartCombat(state.Players);
    foreach (Creature creature in (IEnumerable<Creature>) state.Creatures)
      this.AddCreature(creature);
    Action<CombatState> combatSetUp = this.CombatSetUp;
    if (combatSetUp == null)
      return;
    combatSetUp(state);
  }

  public void AfterCombatRoomLoaded()
  {
    this._combatCts?.Cancel();
    this._combatCts = new CancellationTokenSource();
    RunManager.Instance.ActionQueueSynchronizer.SetCombatState(ActionSynchronizerCombatState.PreCombatSetup);
    TaskHelper.RunSafely(this.StartCombatInternal());
  }

  public async Task StartCombatInternal()
  {
    RunManager.Instance.ActionExecutor.Unpause();
    await RunManager.Instance.ActionExecutor.FinishedExecutingActions();
    RunManager.Instance.ActionExecutor.Pause();
    if (this._state.Encounter.HasBgm)
      NRunMusicController.Instance?.PlayCustomMusic(this._state.Encounter.CustomBgm);
    foreach (Creature creature in (IEnumerable<Creature>) this._state.Creatures)
    {
      await this.AfterCreatureAdded(creature);
      this.CombatCt.ThrowIfCancellationRequested();
    }
    RunManager.Instance.ActionQueueSynchronizer.SetCombatState(ActionSynchronizerCombatState.NotPlayPhase);
    this.IsInProgress = true;
    this.IsStarting = false;
    await Hook.BeforeCombatStart(this._state.RunState, (ICombatState) this._state);
    CancellationToken cancellationToken = this.CombatCt;
    cancellationToken.ThrowIfCancellationRequested();
    Action<CombatState> combatBegan = this.CombatBegan;
    if (combatBegan != null)
      combatBegan(this._state);
    NRunMusicController.Instance?.UpdateTrack();
    NCombatRulesFtue ftue = (NCombatRulesFtue) null;
    if (SaveManager.Instance.SeenFtue("combat_rules_ftue"))
    {
      NCombatRoom instance = NCombatRoom.Instance;
      if (instance != null)
        ((Node) instance).AddChildSafely((Node) NCombatStartBanner.Create());
    }
    else
    {
      ftue = NCombatRulesFtue.Create();
      NModalContainer.Instance?.Add((Node) ftue, false);
    }
    cancellationToken = new CancellationToken();
    await Cmd.CustomScaledWait(0.5f, 1f, cancellationToken: cancellationToken);
    cancellationToken = this.CombatCt;
    cancellationToken.ThrowIfCancellationRequested();
    await this.StartTurn();
    NCombatRulesFtue ncombatRulesFtue = ftue;
    if (ncombatRulesFtue == null)
    {
      ftue = (NCombatRulesFtue) null;
    }
    else
    {
      ncombatRulesFtue.Start();
      ftue = (NCombatRulesFtue) null;
    }
  }

  private async Task StartTurn(Func<Task>? actionDuringEnemyTurn = null)
  {
    List<Creature> creaturesStartingTurn;
    List<Player> playersStartingTurn;
    List<(HookPlayerChoiceContext, Task)> setupPlayerTurnContext;
    if (!this.IsInProgress)
    {
      creaturesStartingTurn = (List<Creature>) null;
      playersStartingTurn = (List<Player>) null;
      setupPlayerTurnContext = (List<(HookPlayerChoiceContext, Task)>) null;
    }
    else
    {
      this.CombatCt.ThrowIfCancellationRequested();
      this.SetPhaseForAllPlayers(PlayerTurnPhase.None);
      Lock.Scope scope1 = this._playerReadyLock.EnterScope();
      bool isExtraPlayerTurn;
      try
      {
        isExtraPlayerTurn = this._playersTakingExtraTurn.Count > 0;
        CombatState state1 = this._state;
        // ISSUE: explicit non-virtual call
        if (((state1 != null ? (__nonvirtual (state1.CurrentSide) == CombatSide.Player ? 1 : 0) : 0) & (isExtraPlayerTurn ? 1 : 0)) != 0)
        {
          creaturesStartingTurn = this._playersTakingExtraTurn.Select<Player, Creature>((Func<Player, Creature>) (p => p.Creature)).ToList<Creature>();
          playersStartingTurn = this._playersTakingExtraTurn.ToList<Player>();
        }
        else
        {
          CombatState state2 = this._state;
          // ISSUE: explicit non-virtual call
          creaturesStartingTurn = (state2 != null ? __nonvirtual (state2.CreaturesOnCurrentSide).ToList<Creature>() : (List<Creature>) null) ?? new List<Creature>();
          CombatState state3 = this._state;
          List<Player> playerList;
          // ISSUE: explicit non-virtual call
          if ((state3 != null ? (__nonvirtual (state3.CurrentSide) == CombatSide.Player ? 1 : 0) : 0) == 0)
          {
            playerList = new List<Player>();
          }
          else
          {
            CombatState state4 = this._state;
            // ISSUE: explicit non-virtual call
            playerList = (state4 != null ? __nonvirtual (state4.Players).ToList<Player>() : (List<Player>) null) ?? new List<Player>();
          }
          playersStartingTurn = playerList;
        }
      }
      finally
      {
        ((Lock.Scope) ref scope1).Dispose();
      }
      foreach (Creature creature in creaturesStartingTurn)
      {
        if (this._state != null)
          creature.BeforeTurnStart(this._state.CurrentSide);
      }
      CancellationToken cancellationToken;
      if (this._state != null)
      {
        await Hook.BeforeSideTurnStart((ICombatState) this._state, this._state.CurrentSide, (IReadOnlyList<Creature>) creaturesStartingTurn);
        cancellationToken = this.CombatCt;
        cancellationToken.ThrowIfCancellationRequested();
      }
      CombatState state5 = this._state;
      // ISSUE: explicit non-virtual call
      if ((state5 != null ? (__nonvirtual (state5.CurrentSide) == CombatSide.Player ? 1 : 0) : 0) != 0)
      {
        this.SetPhaseForAllPlayers(PlayerTurnPhase.Start);
        this.PlayerActionsDisabled = false;
        Lock.Scope scope2 = this._playerReadyLock.EnterScope();
        try
        {
          this._playersReadyToEndTurn.Clear();
          this._playersReadyToBeginEnemyTurn.Clear();
          this._playerToEnemyTransitionFired = false;
          this._inPlayerTurnSetup = true;
          this._deferredEndTurnTransition = (Func<Task>) null;
        }
        finally
        {
          ((Lock.Scope) ref scope2).Dispose();
        }
        int roundNumber = LocalContext.GetMe((IEnumerable<Player>) playersStartingTurn)?.PlayerCombatState?.TurnNumber ?? -1;
        if (roundNumber > 1)
        {
          NCombatRoom instance = NCombatRoom.Instance;
          if (instance != null)
            ((Node) instance).AddChildSafely((Node) NPlayerTurnBanner.Create(roundNumber));
        }
        if (!isExtraPlayerTurn)
        {
          foreach (Creature enemy in (IEnumerable<Creature>) this._state.Enemies)
            enemy.PrepareForNextTurn((IEnumerable<Creature>) this._state.PlayerCreatures);
        }
      }
      else
      {
        NCombatRoom instance = NCombatRoom.Instance;
        if (instance != null)
          ((Node) instance).AddChildSafely((Node) NEnemyTurnBanner.Create());
      }
      cancellationToken = new CancellationToken();
      await Cmd.CustomScaledWait(0.5f, 0.8f, cancellationToken: cancellationToken);
      cancellationToken = this.CombatCt;
      cancellationToken.ThrowIfCancellationRequested();
      foreach (Creature creature in creaturesStartingTurn)
      {
        if (this._state != null)
        {
          await creature.AfterTurnStart(this._state.CurrentSide);
          cancellationToken = this.CombatCt;
          cancellationToken.ThrowIfCancellationRequested();
        }
      }
      foreach (Creature creature in creaturesStartingTurn)
      {
        if (this._state != null)
        {
          await Hook.AfterBlockCleared((ICombatState) this._state, creature);
          this.CombatCt.ThrowIfCancellationRequested();
        }
      }
      setupPlayerTurnContext = new List<(HookPlayerChoiceContext, Task)>();
      foreach (Player player in playersStartingTurn)
      {
        if (LocalContext.NetId.HasValue)
        {
          HookPlayerChoiceContext playerChoiceContext = new HookPlayerChoiceContext(player, LocalContext.NetId.Value, GameActionType.CombatPlayPhaseOnly);
          Task task = this.SetupPlayerTurn(player, playerChoiceContext);
          int num = await playerChoiceContext.WaitForPauseOrCompletionWithoutAssigningTask(task) ? 1 : 0;
          this.CombatCt.ThrowIfCancellationRequested();
          setupPlayerTurnContext.Add((playerChoiceContext, task));
          playerChoiceContext = (HookPlayerChoiceContext) null;
          task = (Task) null;
        }
      }
      CancellationToken combatCt;
      if (this._state != null)
      {
        await Hook.AfterSideTurnStart((ICombatState) this._state, this._state.CurrentSide, (IReadOnlyList<Creature>) creaturesStartingTurn);
        combatCt = this.CombatCt;
        combatCt.ThrowIfCancellationRequested();
      }
      CombatState state6 = this._state;
      // ISSUE: explicit non-virtual call
      if ((state6 != null ? (__nonvirtual (state6.CurrentSide) == CombatSide.Player ? 1 : 0) : 0) != 0)
      {
        foreach (Player player in playersStartingTurn)
        {
          if (player.PlayerCombatState != null)
          {
            ulong? netId = LocalContext.NetId;
            if (netId.HasValue)
            {
              Player owner = player;
              netId = LocalContext.NetId;
              long localPlayerId = (long) netId.Value;
              HookPlayerChoiceContext choiceContext = new HookPlayerChoiceContext(owner, (ulong) localPlayerId, GameActionType.CombatPlayPhaseOnly);
              Task task = player.PlayerCombatState.OrbQueue.AfterTurnStart((PlayerChoiceContext) choiceContext);
              int num = await choiceContext.AssignTaskAndWaitForPauseOrCompletion(task) ? 1 : 0;
              combatCt = this.CombatCt;
              combatCt.ThrowIfCancellationRequested();
            }
          }
        }
        RunManager.Instance.ChecksumTracker.GenerateChecksum("After player turn start", (GameAction) null);
        if (this._state == null)
        {
          creaturesStartingTurn = (List<Creature>) null;
          playersStartingTurn = (List<Player>) null;
          setupPlayerTurnContext = (List<(HookPlayerChoiceContext, Task)>) null;
        }
        else
        {
          foreach (Player player in (IEnumerable<Player>) this._state.Players)
          {
            if (player.Creature.IsDead || !playersStartingTurn.Contains(player))
            {
              Log.Info($"Setting player {player.NetId} to ready at start of turn. IsDead: {player.Creature.IsDead}. IsStartingTurn: {playersStartingTurn.Contains(player)}");
              this.SetReadyToEndTurn(player, false);
              if (this.AllPlayersReadyToEndTurn())
              {
                this.ReleaseDeferredEndTurnTransitionIfNeeded();
                creaturesStartingTurn = (List<Creature>) null;
                playersStartingTurn = (List<Player>) null;
                setupPlayerTurnContext = (List<(HookPlayerChoiceContext, Task)>) null;
                return;
              }
            }
          }
          foreach ((Player player, (HookPlayerChoiceContext playerChoiceContext, Task setupPlayerTurnTask)) in Enumerable.Zip<Player, (HookPlayerChoiceContext, Task)>((IEnumerable<Player>) playersStartingTurn, (IEnumerable<(HookPlayerChoiceContext, Task)>) setupPlayerTurnContext))
          {
            if (this._state == null)
            {
              this.ReleaseDeferredEndTurnTransitionIfNeeded();
              creaturesStartingTurn = (List<Creature>) null;
              playersStartingTurn = (List<Player>) null;
              setupPlayerTurnContext = (List<(HookPlayerChoiceContext, Task)>) null;
              return;
            }
            if (!player.Creature.IsDead)
            {
              Task task = this.RunAutoPrePlayPhase(playerChoiceContext, setupPlayerTurnTask, player);
              int num = await playerChoiceContext.AssignTaskAndWaitForPauseOrCompletion(task) ? 1 : 0;
              this.CombatCt.ThrowIfCancellationRequested();
            }
          }
          int num1 = await this.CheckWinCondition() ? 1 : 0;
          this.CombatCt.ThrowIfCancellationRequested();
          if (this.IsInProgress)
          {
            RunManager.Instance.ActionExecutor.Unpause();
            RunManager.Instance.ActionQueueSynchronizer.SetCombatState(ActionSynchronizerCombatState.PlayPhase);
            this.IsEnemyTurnStarted = false;
            Lock.Scope scope3 = this._playerReadyLock.EnterScope();
            try
            {
              this._inPlayerTurnSetup = false;
            }
            finally
            {
              ((Lock.Scope) ref scope3).Dispose();
            }
            Action<CombatState> turnStarted = this.TurnStarted;
            if (turnStarted != null)
              turnStarted(this._state);
          }
          this.ReleaseDeferredEndTurnTransitionIfNeeded();
          creaturesStartingTurn = (List<Creature>) null;
          playersStartingTurn = (List<Player>) null;
          setupPlayerTurnContext = (List<(HookPlayerChoiceContext, Task)>) null;
        }
      }
      else
      {
        this.IsEnemyTurnStarted = true;
        if (this._state != null)
        {
          Action<CombatState> turnStarted = this.TurnStarted;
          if (turnStarted != null)
            turnStarted(this._state);
        }
        RunManager.Instance.ChecksumTracker.GenerateChecksum("After enemy turn start", (GameAction) null);
        await this.WaitForUnpause();
        combatCt = this.CombatCt;
        combatCt.ThrowIfCancellationRequested();
        int num = await this.CheckWinCondition() ? 1 : 0;
        combatCt = this.CombatCt;
        combatCt.ThrowIfCancellationRequested();
        if (!this.IsInProgress)
        {
          creaturesStartingTurn = (List<Creature>) null;
          playersStartingTurn = (List<Player>) null;
          setupPlayerTurnContext = (List<(HookPlayerChoiceContext, Task)>) null;
        }
        else
        {
          await this.ExecuteEnemyTurn(actionDuringEnemyTurn);
          creaturesStartingTurn = (List<Creature>) null;
          playersStartingTurn = (List<Player>) null;
          setupPlayerTurnContext = (List<(HookPlayerChoiceContext, Task)>) null;
        }
      }
    }
  }

  private async Task RunAutoPrePlayPhase(
    HookPlayerChoiceContext playerChoiceContext,
    Task setupPlayerTurnTask,
    Player player)
  {
    await setupPlayerTurnTask;
    player.PlayerCombatState.Phase = PlayerTurnPhase.AutoPrePlay;
    await this.CheckForEmptyHand((PlayerChoiceContext) playerChoiceContext, player);
    await Hook.AfterAutoPrePlayPhaseEntered(playerChoiceContext, (ICombatState) this._state, player);
    player.PlayerCombatState.Phase = PlayerTurnPhase.Play;
  }

  private async Task SetupPlayerTurn(Player player, HookPlayerChoiceContext playerChoiceContext)
  {
    CombatState state;
    if (player.Creature.IsDead)
      state = (CombatState) null;
    else if (this._state == null || player.PlayerCombatState == null)
    {
      Log.Warn($"Combat state is null. Assuming that the run has been cleaned up. (CombatState: {this._state} PlayerCombatState: {player.PlayerCombatState})");
      state = (CombatState) null;
    }
    else
    {
      state = this._state;
      if (Hook.ShouldPlayerResetEnergy((ICombatState) state, player))
      {
        SfxCmd.Play("event:/sfx/ui/gain_energy");
        player.PlayerCombatState.ResetEnergy();
      }
      else
        player.PlayerCombatState.AddMaxEnergyToCurrent();
      await Hook.AfterEnergyReset((ICombatState) state, player);
      CancellationToken combatCt = this.CombatCt;
      combatCt.ThrowIfCancellationRequested();
      await Hook.BeforeHandDraw((ICombatState) state, player, (PlayerChoiceContext) playerChoiceContext);
      combatCt = this.CombatCt;
      combatCt.ThrowIfCancellationRequested();
      IEnumerable<AbstractModel> modifiers;
      Decimal handDraw = Hook.ModifyHandDraw((ICombatState) state, player, 5M, out modifiers);
      await Hook.AfterModifyingHandDraw((ICombatState) state, modifiers);
      combatCt = this.CombatCt;
      combatCt.ThrowIfCancellationRequested();
      if (player.PlayerCombatState.TurnNumber == 1)
      {
        CardPile pile = PileType.Draw.GetPile(player);
        List<CardModel> list1 = pile.Cards.Where<CardModel>((Func<CardModel, bool>) (c =>
        {
          EnchantmentModel enchantment = c.Enchantment;
          return enchantment != null && enchantment.ShouldStartAtBottomOfDrawPile;
        })).ToList<CardModel>();
        foreach (CardModel card in list1)
          pile.MoveToBottomInternal(card);
        List<CardModel> list2 = pile.Cards.Where<CardModel>((Func<CardModel, bool>) (c => c.Keywords.Contains(CardKeyword.Innate))).Except<CardModel>((IEnumerable<CardModel>) list1).ToList<CardModel>();
        foreach (CardModel card in list2)
          pile.MoveToTopInternal(card);
        handDraw = Math.Max(handDraw, (Decimal) list2.Count);
        handDraw = Math.Min(handDraw, (Decimal) CardPile.MaxCardsInHand);
      }
      IEnumerable<CardModel> cardModels = await CardPileCmd.Draw((PlayerChoiceContext) playerChoiceContext, handDraw, player, true);
      combatCt = this.CombatCt;
      combatCt.ThrowIfCancellationRequested();
      await Hook.AfterPlayerTurnStart((ICombatState) state, (PlayerChoiceContext) playerChoiceContext, player);
      state = (CombatState) null;
    }
  }

  public void SetReadyToEndTurn(Player player, bool canBackOut, Func<Task>? actionDuringEnemyTurn = null)
  {
    Lock.Scope scope1 = this._playerReadyLock.EnterScope();
    try
    {
      if (this._playersReadyToEndTurn.Contains(player))
        return;
      this._playersReadyToEndTurn.Add(player);
    }
    finally
    {
      ((Lock.Scope) ref scope1).Dispose();
    }
    Action<Player, bool> playerEndedTurn = this.PlayerEndedTurn;
    if (playerEndedTurn != null)
      playerEndedTurn(player, canBackOut);
    if (!this.AllPlayersReadyToEndTurn())
      return;
    Log.Debug("All players ready to end turn");
    GameAction runningAction = RunManager.Instance.ActionExecutor.CurrentlyRunningAction;
    CombatState scheduledCombat = this._state;
    PlayerCombatState playerCombatState = player.PlayerCombatState;
    int scheduledTurnNumber = playerCombatState != null ? playerCombatState.TurnNumber : -1;
    Func<Task> func = runningAction == null || !ActionQueueSet.IsGameActionPlayerDriven(runningAction) ? (Func<Task>) (() => this.AfterAllPlayersReadyToEndTurn(scheduledCombat, scheduledTurnNumber, player, actionDuringEnemyTurn)) : (Func<Task>) (() => this.WaitForActionThenEndTurn(runningAction, scheduledCombat, scheduledTurnNumber, player, actionDuringEnemyTurn));
    Lock.Scope scope2 = this._playerReadyLock.EnterScope();
    bool inPlayerTurnSetup;
    try
    {
      inPlayerTurnSetup = this._inPlayerTurnSetup;
      if (inPlayerTurnSetup)
        this._deferredEndTurnTransition = func;
    }
    finally
    {
      ((Lock.Scope) ref scope2).Dispose();
    }
    if (inPlayerTurnSetup)
      return;
    TaskHelper.RunSafely(func());
  }

  private void ReleaseDeferredEndTurnTransitionIfNeeded()
  {
    Lock.Scope scope = this._playerReadyLock.EnterScope();
    Func<Task> endTurnTransition;
    try
    {
      this._inPlayerTurnSetup = false;
      endTurnTransition = this._deferredEndTurnTransition;
      this._deferredEndTurnTransition = (Func<Task>) null;
    }
    finally
    {
      ((Lock.Scope) ref scope).Dispose();
    }
    if (endTurnTransition == null)
      return;
    TaskHelper.RunSafely(endTurnTransition());
  }

  public void UndoReadyToEndTurn(Player player)
  {
    Lock.Scope scope = this._playerReadyLock.EnterScope();
    try
    {
      this._playersReadyToEndTurn.Remove(player);
    }
    finally
    {
      ((Lock.Scope) ref scope).Dispose();
    }
    if (LocalContext.IsMe(player))
      this.PlayerActionsDisabled = false;
    Action<Player> playerUnendedTurn = this.PlayerUnendedTurn;
    if (playerUnendedTurn == null)
      return;
    playerUnendedTurn(player);
  }

  public void OnEndedTurnLocally() => this.PlayerActionsDisabled = true;

  public void SetReadyToBeginEnemyTurn(Player player, Func<Task>? actionDuringEnemyTurn = null)
  {
    if (!this.IsInProgress)
    {
      Log.Error("Trying to set player ready to begin enemy turn, but combat is over!");
    }
    else
    {
      Lock.Scope scope = this._playerReadyLock.EnterScope();
      bool flag1;
      try
      {
        if (!this._playersReadyToBeginEnemyTurn.Add(player))
          return;
        bool flag2 = this._state.CurrentSide == CombatSide.Player;
        bool flag3 = this._playersReadyToBeginEnemyTurn.Count == this._state.Players.Count & flag2 || flag2 && RunManager.Instance.NetService.Type == NetGameType.Singleplayer;
        flag1 = flag3 && !this._playerToEnemyTransitionFired;
        if (flag1)
          this._playerToEnemyTransitionFired = true;
        else if (flag3)
          Log.Warn($"Ignoring ready-to-begin-enemy-turn for player {player.NetId}: a player-to-enemy transition has already been launched for this turn.");
      }
      finally
      {
        ((Lock.Scope) ref scope).Dispose();
      }
      if (!flag1)
        return;
      TaskHelper.RunSafely(this.AfterAllPlayersReadyToBeginEnemyTurn(actionDuringEnemyTurn));
    }
  }

  public bool IsPlayerReadyToEndTurn(Player player)
  {
    Lock.Scope scope = this._playerReadyLock.EnterScope();
    try
    {
      return this._playersReadyToEndTurn.Contains(player);
    }
    finally
    {
      ((Lock.Scope) ref scope).Dispose();
    }
  }

  public bool AllPlayersReadyToEndTurn()
  {
    Lock.Scope scope = this._playerReadyLock.EnterScope();
    bool flag;
    try
    {
      flag = this._playersReadyToEndTurn.Count == this._state.Players.Count;
    }
    finally
    {
      ((Lock.Scope) ref scope).Dispose();
    }
    if (RunManager.Instance.IsSingleplayerOrFakeMultiplayer)
      return true;
    return flag && this._state.CurrentSide == CombatSide.Player;
  }

  private async Task EndEnemyTurn(CancellationToken? combatCt = null)
  {
    CancellationToken ct;
    if (!this.IsInProgress)
    {
      ct = new CancellationToken();
    }
    else
    {
      ct = combatCt ?? this.CombatCt;
      ct.ThrowIfCancellationRequested();
      if (this._state.CurrentSide != CombatSide.Enemy)
        throw new InvalidOperationException($"EndEnemyTurn called while the current side is {this._state.CurrentSide}!");
      await this.WaitForUnpause();
      ct.ThrowIfCancellationRequested();
      await this.EndEnemyTurnInternal();
      ct.ThrowIfCancellationRequested();
      int num = await this.CheckWinCondition() ? 1 : 0;
      if (this.IsEnding)
      {
        ct = new CancellationToken();
      }
      else
      {
        this.SwitchSides();
        await this.WaitForUnpause();
        this.CombatCt.ThrowIfCancellationRequested();
        await this.StartTurn();
        ct = new CancellationToken();
      }
    }
  }

  public void AddCreature(Creature creature)
  {
    if (!this._state.ContainsCreature(creature))
      throw new InvalidOperationException("CombatState must already contain creature.");
    creature.Monster?.SetUpForCombat();
    if (creature.SlotName != null)
      this._state.SortEnemiesBySlotName();
    this.StateTracker.Subscribe(creature);
    Action<CombatState> creaturesChanged = this.CreaturesChanged;
    if (creaturesChanged == null)
      return;
    creaturesChanged(this._state);
  }

  public async Task AfterCreatureAdded(Creature creature)
  {
    await creature.AfterAddedToRoom();
    if (!creature.IsEnemy || this._state.CurrentSide != CombatSide.Player)
      return;
    creature.Monster.RollMove(this._state.Players.Select<Player, Creature>((Func<Player, Creature>) (p => p.Creature)));
  }

  public async Task CheckForEmptyHand(PlayerChoiceContext choiceContext, Player player)
  {
    if (!this.IsInProgress || this.IsExecutingCardOrPotionEffect(player) || PileType.Hand.GetPile(player).Cards.Any<CardModel>())
      return;
    await Hook.AfterHandEmptied((ICombatState) this._state, choiceContext, player);
  }

  public void Reset(bool graceful)
  {
    this._combatCts?.Cancel();
    if (graceful && this._state != null)
    {
      this.SetPhaseForAllPlayers(PlayerTurnPhase.None);
      foreach (Creature creature in this._state.Creatures.ToList<Creature>())
      {
        creature.Reset();
        this.RemoveCreature(creature);
        this._state.RemoveCreature(creature, true);
      }
      this._state = (CombatState) null;
    }
    this._pendingLoss = (CombatManager.PendingLossState) null;
    this.DebugForcedTopCardOnNextShuffle = (CardModel) null;
    this.IsInProgress = false;
    this.IsStarting = false;
    this.IsEnemyTurnStarted = false;
    this.EndingPlayerTurnPhaseOne = false;
    this.EndingPlayerTurnPhaseTwo = false;
    Lock.Scope scope = this._playerReadyLock.EnterScope();
    try
    {
      this._playersReadyToEndTurn.Clear();
      this._playersReadyToBeginEnemyTurn.Clear();
      this._playerToEnemyTransitionFired = false;
    }
    finally
    {
      ((Lock.Scope) ref scope).Dispose();
    }
    this.History.Clear();
    this._cardOrPotionEffectDepth.Clear();
    RunManager.Instance.ActionQueueSynchronizer.SetCombatState(ActionSynchronizerCombatState.NotInCombat);
  }

  public async Task HandlePlayerDeath(Player player)
  {
    if (!this.IsInProgress)
      return;
    Log.Info($"Player {player.NetId} died, doing death handling");
    if (!this.IsExecutingCardOrPotionEffect(player))
      await this.RemoveDeadPlayerCardsFromCombat(player);
    await PlayerCmd.SetEnergy(0M, player);
    await PlayerCmd.SetStars(0M, player);
  }

  public async Task RemoveDeadPlayerCardsFromCombat(Player player)
  {
    if (!this.IsInProgress || player.PlayerCombatState == null || this._state == null || this._state.Players.All<Player>((Func<Player, bool>) (p => p.Creature.IsDead)))
      return;
    List<CardModel> cardModelList = new List<CardModel>();
    cardModelList.AddRange((IEnumerable<CardModel>) player.PlayerCombatState.Hand.Cards);
    cardModelList.AddRange((IEnumerable<CardModel>) player.PlayerCombatState.DrawPile.Cards);
    cardModelList.AddRange((IEnumerable<CardModel>) player.PlayerCombatState.DiscardPile.Cards);
    cardModelList.AddRange((IEnumerable<CardModel>) player.PlayerCombatState.ExhaustPile.Cards);
    cardModelList.AddRange((IEnumerable<CardModel>) player.PlayerCombatState.PlayPile.Cards);
    await CardPileCmd.RemoveFromCombat((IEnumerable<CardModel>) cardModelList.ToArray());
  }

  public void LoseCombat()
  {
    if (this._pendingLoss != (CombatManager.PendingLossState) null)
      return;
    this._pendingLoss = new CombatManager.PendingLossState(this._state, (CombatRoom) this._state.RunState.CurrentRoom);
  }

  private void ProcessPendingLoss()
  {
    if (this._pendingLoss == (CombatManager.PendingLossState) null)
      return;
    CombatManager.PendingLossState pendingLoss = this._pendingLoss;
    this._pendingLoss = (CombatManager.PendingLossState) null;
    this.IsInProgress = false;
    Action<CombatRoom> combatEnded = this.CombatEnded;
    if (combatEnded == null)
      return;
    combatEnded(pendingLoss.Room);
  }

  public async Task EndCombatInternal()
  {
    CombatState combatState = this._state;
    Player localPlayer = LocalContext.GetMe((ICombatState) combatState);
    int turnsTaken = localPlayer.PlayerCombatState.TurnNumber;
    IRunState runState = combatState.RunState;
    CombatRoom room = (CombatRoom) runState.CurrentRoom;
    this.IsInProgress = false;
    this.SetPhaseForAllPlayers(PlayerTurnPhase.None);
    this.PlayerActionsDisabled = false;
    Lock.Scope scope = this._playerReadyLock.EnterScope();
    try
    {
      this._playersTakingExtraTurn.Clear();
    }
    finally
    {
      ((Lock.Scope) ref scope).Dispose();
    }
    foreach (Player player in (IEnumerable<Player>) combatState.Players)
      await player.ReviveBeforeCombatEnd();
    await Hook.AfterCombatEnd(runState, (ICombatState) combatState, room);
    this.History.Clear();
    room.OnCombatEnded();
    if (RunManager.Instance.NetService.Type != NetGameType.Replay)
      RunManager.Instance.WriteReplay(true);
    foreach (Player player in (IEnumerable<Player>) combatState.Players)
      player.AfterCombatEnd();
    await Hook.AfterCombatVictory(runState, (ICombatState) combatState, room);
    NHoverTipSet.Clear();
    if (runState.CurrentMapPointHistoryEntry != null)
      runState.CurrentMapPointHistoryEntry.Rooms.Last<MapPointRoomHistoryEntry>().TurnsTaken = turnsTaken;
    int num1;
    if (runState.Map.SecondBossMapPoint != null)
    {
      MapCoord? currentMapCoord = runState.CurrentMapCoord;
      MapCoord coord = runState.Map.SecondBossMapPoint.coord;
      num1 = currentMapCoord.HasValue ? (currentMapCoord.GetValueOrDefault() == coord ? 1 : 0) : 0;
    }
    else
      num1 = 0;
    bool flag1 = num1 != 0;
    int num2;
    if (runState.Map.SecondBossMapPoint == null)
    {
      MapCoord? currentMapCoord = runState.CurrentMapCoord;
      MapCoord coord = runState.Map.BossMapPoint.coord;
      num2 = currentMapCoord.HasValue ? (currentMapCoord.GetValueOrDefault() == coord ? 1 : 0) : 0;
    }
    else
      num2 = 0;
    bool flag2 = num2 != 0;
    if (room.RoomType == RoomType.Boss && runState.CurrentActIndex == runState.Acts.Count - 1 && flag1 | flag2)
      RunManager.Instance.WinTime = RunManager.Instance.RunTime;
    room.MarkPreFinished();
    await SaveManager.Instance.SaveRun((AbstractRoom) room, false);
    NMapScreen.Instance?.SetTravelEnabled(true);
    SaveManager.Instance.UpdateProgressAfterCombatWon(localPlayer, room);
    AchievementsHelper.CheckForDefeatedAllEnemiesAchievement(runState.Act, localPlayer);
    SaveManager.Instance.SaveProgressFile();
    if (room.RoomType == RoomType.Boss)
      AchievementsHelper.AfterBossDefeated(localPlayer);
    combatState.MultiplayerScalingModel?.OnCombatFinished();
    if (this._state != null)
    {
      Action<CombatRoom> combatWon = this.CombatWon;
      if (combatWon != null)
        combatWon(room);
    }
    RunManager.Instance.ActionExecutor.Unpause();
    RunManager.Instance.ActionQueueSynchronizer.SetCombatState(ActionSynchronizerCombatState.NotInCombat);
    NRunMusicController.Instance?.UpdateTrack();
    if (this._state == null)
    {
      combatState = (CombatState) null;
      localPlayer = (Player) null;
      runState = (IRunState) null;
      room = (CombatRoom) null;
    }
    else
    {
      Action<CombatRoom> combatEnded = this.CombatEnded;
      if (combatEnded == null)
      {
        combatState = (CombatState) null;
        localPlayer = (Player) null;
        runState = (IRunState) null;
        room = (CombatRoom) null;
      }
      else
      {
        combatEnded(room);
        combatState = (CombatState) null;
        localPlayer = (Player) null;
        runState = (IRunState) null;
        room = (CombatRoom) null;
      }
    }
  }

  public void RemoveCreature(Creature creature)
  {
    if (creature.IsMonster)
    {
      creature.Monster.BeforeRemovedFromRoom();
      creature.Monster.ResetStateMachine();
    }
    this.StateTracker.Unsubscribe(creature);
    Action<CombatState> creaturesChanged = this.CreaturesChanged;
    if (creaturesChanged == null)
      return;
    creaturesChanged(this._state);
  }

  public async Task<bool> CheckWinCondition()
  {
    if (this._pendingLoss != (CombatManager.PendingLossState) null)
    {
      this.ProcessPendingLoss();
      return true;
    }
    if (!this.IsEnding)
      return false;
    await this.EndCombatInternal();
    return true;
  }

  private async Task ExecuteEnemyTurn(Func<Task>? actionDuringEnemyTurn = null)
  {
    CancellationToken ct;
    if (!this.IsInProgress)
    {
      ct = new CancellationToken();
    }
    else
    {
      ct = this.CombatCt;
      ct.ThrowIfCancellationRequested();
      if (actionDuringEnemyTurn != null)
      {
        await actionDuringEnemyTurn();
        ct.ThrowIfCancellationRequested();
      }
      foreach (Creature creature in this._state.Enemies.ToList<Creature>())
      {
        Creature enemy = creature;
        if (this._state.ContainsCreature(enemy))
        {
          NCreature creatureNode = NCombatRoom.Instance?.GetCreatureNode(enemy);
          if (creatureNode != null)
            await creatureNode.PerformIntent();
          await enemy.TakeTurn();
          ct.ThrowIfCancellationRequested();
          await this.WaitForUnpause();
          int num = await this.CheckWinCondition() ? 1 : 0;
          if (!this.IsInProgress)
          {
            ct = new CancellationToken();
            return;
          }
          enemy = (Creature) null;
        }
      }
      RunManager.Instance.ChecksumTracker.GenerateChecksum("After enemy turn end", (GameAction) null);
      await this.EndEnemyTurn(new CancellationToken?(ct));
      ct = new CancellationToken();
    }
  }

  private async Task WaitForActionThenEndTurn(
    GameAction action,
    CombatState? scheduledCombat,
    int scheduledTurnNumber,
    Player scheduledPlayer,
    Func<Task>? actionDuringEnemyTurn)
  {
    await action.CompletionTask;
    await this.AfterAllPlayersReadyToEndTurn(scheduledCombat, scheduledTurnNumber, scheduledPlayer, actionDuringEnemyTurn);
  }

  private async Task AfterAllPlayersReadyToEndTurn(
    CombatState? scheduledCombat,
    int scheduledTurnNumber,
    Player scheduledPlayer,
    Func<Task>? actionDuringEnemyTurn = null)
  {
    if (!this.IsInProgress)
      return;
    if (this._state == scheduledCombat)
    {
      PlayerCombatState playerCombatState = scheduledPlayer.PlayerCombatState;
      if ((playerCombatState != null ? playerCombatState.TurnNumber : -1) == scheduledTurnNumber)
      {
        this.CombatCt.ThrowIfCancellationRequested();
        this.EndingPlayerTurnPhaseOne = true;
        RunManager.Instance.ActionQueueSynchronizer.SetCombatState(ActionSynchronizerCombatState.EndTurnPhaseOne);
        await this.WaitUntilQueueIsEmptyOrWaitingOnNonPlayerDrivenAction();
        await this.EndPlayerTurnPhaseOneInternal();
        if (this.IsInProgress && RunManager.Instance.NetService.Type != NetGameType.Replay)
          RunManager.Instance.ActionQueueSynchronizer.RequestEnqueue((GameAction) new ReadyToBeginEnemyTurnAction(LocalContext.GetMe((ICombatState) this._state), actionDuringEnemyTurn));
        this.EndingPlayerTurnPhaseOne = false;
        return;
      }
    }
    Log.Info($"Dropping stale player-turn-end transition for player {scheduledPlayer.NetId}: the combat or turn it was scheduled for has ended");
  }

  private async Task WaitUntilQueueIsEmptyOrWaitingOnNonPlayerDrivenAction()
  {
    GameAction currentlyRunningAction = RunManager.Instance.ActionExecutor.CurrentlyRunningAction;
    if (currentlyRunningAction == null)
      ;
    else if (!ActionQueueSet.IsGameActionPlayerDriven(currentlyRunningAction))
      ;
    else
    {
      TaskCompletionSource completionSource = new TaskCompletionSource();
      RunManager.Instance.ActionExecutor.AfterActionExecuted += new Action<GameAction>(AfterActionExecuted);
      await completionSource.Task;
      RunManager.Instance.ActionExecutor.AfterActionExecuted -= new Action<GameAction>(AfterActionExecuted);

      void AfterActionExecuted(GameAction action)
      {
        GameAction readyAction = RunManager.Instance.ActionQueueSet.GetReadyAction();
        if (readyAction != null && ActionQueueSet.IsGameActionPlayerDriven(readyAction))
          return;
        completionSource.SetResult();
      }
    }
  }

  public async Task EndPlayerTurnPhaseOneInternal()
  {
    List<Player> playersEndingTurn;
    List<(Player, HookPlayerChoiceContext)> autoPostPlayContexts;
    List<HookPlayerChoiceContext> playerEndContexts;
    if (this._state == null)
    {
      playersEndingTurn = (List<Player>) null;
      autoPostPlayContexts = (List<(Player, HookPlayerChoiceContext)>) null;
      playerEndContexts = (List<HookPlayerChoiceContext>) null;
    }
    else
    {
      CombatState state1 = this._state;
      // ISSUE: explicit non-virtual call
      if ((state1 != null ? (__nonvirtual (state1.CurrentSide) != CombatSide.Player ? 1 : 0) : 1) != 0)
        throw new InvalidOperationException($"EndPlayerTurn called while the current side is {this._state?.CurrentSide}!");
      await this.WaitForUnpause();
      Lock.Scope scope = this._playerReadyLock.EnterScope();
      try
      {
        List<Player> playerList;
        if (this._playersTakingExtraTurn.Count <= 0)
        {
          CombatState state2 = this._state;
          // ISSUE: explicit non-virtual call
          playerList = (state2 != null ? __nonvirtual (state2.Players).ToList<Player>() : (List<Player>) null) ?? new List<Player>();
        }
        else
          playerList = this._playersTakingExtraTurn.ToList<Player>();
        playersEndingTurn = playerList;
      }
      finally
      {
        ((Lock.Scope) ref scope).Dispose();
      }
      autoPostPlayContexts = new List<(Player, HookPlayerChoiceContext)>();
      Player player;
      HookPlayerChoiceContext playerChoiceContext;
      foreach (Player player1 in playersEndingTurn)
      {
        player = player1;
        if (this._state != null)
        {
          ulong? netId = LocalContext.NetId;
          if (netId.HasValue)
          {
            player.PlayerCombatState.Phase = PlayerTurnPhase.AutoPostPlay;
            Player owner = player;
            netId = LocalContext.NetId;
            long localPlayerId = (long) netId.Value;
            playerChoiceContext = new HookPlayerChoiceContext(owner, (ulong) localPlayerId, GameActionType.CombatPlayPhaseOnly);
            int num = await playerChoiceContext.AssignTaskAndWaitForPauseOrCompletion(Hook.AfterAutoPostPlayPhaseEntered(playerChoiceContext, (ICombatState) this._state, player)) ? 1 : 0;
            autoPostPlayContexts.Add((player, playerChoiceContext));
            playerChoiceContext = (HookPlayerChoiceContext) null;
            player = (Player) null;
          }
        }
      }
      foreach ((Player, HookPlayerChoiceContext) valueTuple in autoPostPlayContexts)
      {
        player = valueTuple.Item1;
        await valueTuple.Item2.WaitForCompletion();
        player.PlayerCombatState.Phase = PlayerTurnPhase.End;
        player = (Player) null;
      }
      if (this._state != null)
        await Hook.BeforeSideTurnEnd((ICombatState) this._state, this._state.CurrentSide, playersEndingTurn.Select<Player, Creature>((Func<Player, Creature>) (p => p.Creature)));
      if (await this.CheckWinCondition())
      {
        playersEndingTurn = (List<Player>) null;
        autoPostPlayContexts = (List<(Player, HookPlayerChoiceContext)>) null;
        playerEndContexts = (List<HookPlayerChoiceContext>) null;
      }
      else
      {
        playerEndContexts = new List<HookPlayerChoiceContext>();
        foreach (Player player2 in playersEndingTurn)
        {
          if (LocalContext.NetId.HasValue)
          {
            playerChoiceContext = new HookPlayerChoiceContext(player2, LocalContext.NetId.Value, GameActionType.Combat);
            int num = await playerChoiceContext.AssignTaskAndWaitForPauseOrCompletion(this.DoTurnEnd(player2, (PlayerChoiceContext) playerChoiceContext)) ? 1 : 0;
            playerEndContexts.Add(playerChoiceContext);
            playerChoiceContext = (HookPlayerChoiceContext) null;
          }
        }
        foreach (HookPlayerChoiceContext playerChoiceContext1 in playerEndContexts)
          await playerChoiceContext1.WaitForCompletion();
        if (await this.CheckWinCondition())
        {
          playersEndingTurn = (List<Player>) null;
          autoPostPlayContexts = (List<(Player, HookPlayerChoiceContext)>) null;
          playerEndContexts = (List<HookPlayerChoiceContext>) null;
        }
        else
        {
          if (this._state != null)
          {
            foreach (Player player3 in playersEndingTurn)
              await Hook.BeforeFlush((ICombatState) this._state, player3);
          }
          RunManager.Instance.ChecksumTracker.GenerateChecksum("After player turn phase one end", (GameAction) null);
          int num = await this.CheckWinCondition() ? 1 : 0;
          playersEndingTurn = (List<Player>) null;
          autoPostPlayContexts = (List<(Player, HookPlayerChoiceContext)>) null;
          playerEndContexts = (List<HookPlayerChoiceContext>) null;
        }
      }
    }
  }

  private async Task DoTurnEnd(Player player, PlayerChoiceContext choiceContext)
  {
    await player.PlayerCombatState.OrbQueue.BeforeTurnEnd(choiceContext);
    List<CardModel> turnEndCards;
    if (this.IsOverOrEnding)
    {
      turnEndCards = (List<CardModel>) null;
    }
    else
    {
      CardPile pile = PileType.Hand.GetPile(player);
      turnEndCards = new List<CardModel>();
      List<CardModel> cardModelList = new List<CardModel>();
      foreach (CardModel card in (IEnumerable<CardModel>) pile.Cards)
      {
        if (card.HasTurnEndInHandEffect)
          turnEndCards.Add(card);
        else if (card.Keywords.Contains(CardKeyword.Ethereal) && Hook.ShouldEtherealTrigger(player.Creature.CombatState, card))
          cardModelList.Add(card);
      }
      foreach (CardModel card in cardModelList)
        await CardCmd.Exhaust(choiceContext, card, true);
      foreach (CardModel cardModel in turnEndCards)
        await cardModel.OnTurnEndInHandWrapper(choiceContext);
      turnEndCards = (List<CardModel>) null;
    }
  }

  private async Task EndEnemyTurnInternal()
  {
    List<Creature> enemies = this._state.CreaturesOnCurrentSide.ToList<Creature>();
    await Hook.BeforeSideTurnEnd((ICombatState) this._state, this._state.CurrentSide, (IEnumerable<Creature>) enemies);
    foreach (Player player in (IEnumerable<Player>) this._state.Players)
      player.PlayerCombatState.EndOfTurnCleanup();
    await Hook.AfterSideTurnEnd((ICombatState) this._state, this._state.CurrentSide, (IEnumerable<Creature>) enemies);
    enemies = (List<Creature>) null;
  }

  private async Task AfterAllPlayersReadyToBeginEnemyTurn(Func<Task>? actionDuringEnemyTurn = null)
  {
    CancellationToken ct;
    if (!this.IsInProgress)
    {
      ct = new CancellationToken();
    }
    else
    {
      ct = this.CombatCt;
      ct.ThrowIfCancellationRequested();
      this.EndingPlayerTurnPhaseTwo = true;
      try
      {
        RunManager.Instance.ActionQueueSynchronizer.SetCombatState(ActionSynchronizerCombatState.NotPlayPhase);
        Action<CombatState> switchToEnemyTurn = this.AboutToSwitchToEnemyTurn;
        if (switchToEnemyTurn != null)
          switchToEnemyTurn(this._state);
        await Task.Yield();
        if (!this.IsInProgress)
          ct = new CancellationToken();
        else if (ct.IsCancellationRequested)
        {
          ct = new CancellationToken();
        }
        else
        {
          CombatState state = this._state;
          // ISSUE: explicit non-virtual call
          if ((state != null ? (__nonvirtual (state.CurrentSide) != CombatSide.Player ? 1 : 0) : 1) != 0)
          {
            ct = new CancellationToken();
          }
          else
          {
            await this.EndPlayerTurnPhaseTwoInternal(new CancellationToken?(ct));
            await this.SwitchFromPlayerToEnemySide(actionDuringEnemyTurn);
            ct = new CancellationToken();
          }
        }
      }
      finally
      {
        this.EndingPlayerTurnPhaseTwo = false;
      }
    }
  }

  public async Task EndPlayerTurnPhaseTwoInternal(CancellationToken? combatCt = null)
  {
    CancellationToken ct = combatCt ?? this.CombatCt;
    ct.ThrowIfCancellationRequested();
    if (this._state.CurrentSide != CombatSide.Player)
      throw new InvalidOperationException($"EndPlayerTurnPhaseTwo called while the current side is {this._state.CurrentSide}!");
    Lock.Scope scope = this._playerReadyLock.EnterScope();
    List<Player> playersEndingTurn;
    try
    {
      playersEndingTurn = this._playersTakingExtraTurn.Count > 0 ? this._playersTakingExtraTurn.ToList<Player>() : this._state.Players.ToList<Player>();
    }
    finally
    {
      ((Lock.Scope) ref scope).Dispose();
    }
    List<HookPlayerChoiceContext> flushPlayerHandContexts = new List<HookPlayerChoiceContext>();
    foreach (Player player in playersEndingTurn)
    {
      if (this._state != null)
      {
        ulong? netId = LocalContext.NetId;
        if (netId.HasValue)
        {
          Player owner = player;
          netId = LocalContext.NetId;
          long localPlayerId = (long) netId.Value;
          HookPlayerChoiceContext playerChoiceContext = new HookPlayerChoiceContext(owner, (ulong) localPlayerId, GameActionType.CombatPlayPhaseOnly);
          int num = await playerChoiceContext.AssignTaskAndWaitForPauseOrCompletion(this.FlushPlayerHand(player, playerChoiceContext)) ? 1 : 0;
          flushPlayerHandContexts.Add(playerChoiceContext);
          playerChoiceContext = (HookPlayerChoiceContext) null;
        }
      }
    }
    foreach (HookPlayerChoiceContext playerChoiceContext in flushPlayerHandContexts)
      await playerChoiceContext.WaitForCompletion();
    if (this._state != null)
    {
      await Hook.AfterSideTurnEnd((ICombatState) this._state, this._state.CurrentSide, playersEndingTurn.Select<Player, Creature>((Func<Player, Creature>) (p => p.Creature)));
      ct.ThrowIfCancellationRequested();
    }
    RunManager.Instance.ChecksumTracker.GenerateChecksum("after player turn phase two end", (GameAction) null);
    ct = new CancellationToken();
    playersEndingTurn = (List<Player>) null;
    flushPlayerHandContexts = (List<HookPlayerChoiceContext>) null;
  }

  private async Task FlushPlayerHand(Player player, HookPlayerChoiceContext playerChoiceContext)
  {
    CombatState state;
    List<CardModel> cardsToFlush;
    List<CardModel> cardsToRetain;
    if (player.Creature.IsDead)
    {
      state = (CombatState) null;
      cardsToFlush = (List<CardModel>) null;
      cardsToRetain = (List<CardModel>) null;
    }
    else if (this._state == null || player.PlayerCombatState == null)
    {
      Log.Warn($"Combat state is null. Assuming that the run has been cleaned up. (CombatState: {this._state} PlayerCombatState: {player.PlayerCombatState})");
      state = (CombatState) null;
      cardsToFlush = (List<CardModel>) null;
      cardsToRetain = (List<CardModel>) null;
    }
    else
    {
      state = this._state;
      cardsToFlush = new List<CardModel>();
      cardsToRetain = new List<CardModel>();
      bool flag = Hook.ShouldFlush((ICombatState) state, player);
      foreach (CardModel card in (IEnumerable<CardModel>) PileType.Hand.GetPile(player).Cards)
      {
        if (!flag || card.ShouldRetainThisTurn)
          cardsToRetain.Add(card);
        else
          cardsToFlush.Add(card);
      }
      if (cardsToFlush.Count > 0)
      {
        IReadOnlyList<CardPileAddResult> cardPileAddResultList = await CardPileCmd.Add((IEnumerable<CardModel>) cardsToFlush, PileType.Discard);
        this.CombatCt.ThrowIfCancellationRequested();
      }
      await Hook.AfterFlush((ICombatState) state, player, (PlayerChoiceContext) playerChoiceContext, (IReadOnlyCollection<CardModel>) cardsToFlush, (IReadOnlyCollection<CardModel>) cardsToRetain);
      this.CombatCt.ThrowIfCancellationRequested();
      player.PlayerCombatState.EndOfTurnCleanup();
      state = (CombatState) null;
      cardsToFlush = (List<CardModel>) null;
      cardsToRetain = (List<CardModel>) null;
    }
  }

  public async Task SwitchFromPlayerToEnemySide(Func<Task>? actionDuringEnemyTurn = null)
  {
    if (this._state == null)
      return;
    Lock.Scope scope = this._playerReadyLock.EnterScope();
    List<Player> list;
    try
    {
      this._playersTakingExtraTurn.Clear();
      foreach (Player player in (IEnumerable<Player>) this._state.Players)
      {
        if (Hook.ShouldTakeExtraTurn((ICombatState) this._state, player))
        {
          Log.Info($"Player {player.NetId} ({player.Character.Id.Entry}) is taking an extra turn");
          this._playersTakingExtraTurn.Add(player);
        }
      }
      list = this._playersTakingExtraTurn.ToList<Player>();
    }
    finally
    {
      ((Lock.Scope) ref scope).Dispose();
    }
    this.SwitchSides();
    foreach (Player player in list)
    {
      if (this._state == null)
        return;
      await Hook.AfterTakingExtraTurn((ICombatState) this._state, player);
    }
    await this.WaitForUnpause();
    await this.StartTurn(actionDuringEnemyTurn);
  }

  private void SwitchSides()
  {
    if (this._state == null)
      return;
    Lock.Scope scope = this._playerReadyLock.EnterScope();
    bool flag;
    try
    {
      flag = this._playersTakingExtraTurn.Count > 0;
    }
    finally
    {
      ((Lock.Scope) ref scope).Dispose();
    }
    if (this._state.CurrentSide == CombatSide.Player && !flag)
    {
      this._state.CurrentSide = CombatSide.Enemy;
    }
    else
    {
      this._state.CurrentSide = CombatSide.Player;
      IReadOnlyList<Player> playerList;
      if (flag)
      {
        playerList = (IReadOnlyList<Player>) this._playersTakingExtraTurn;
      }
      else
      {
        playerList = this._state.Players;
        ++this._state.RoundNumber;
      }
      foreach (Player player in (IEnumerable<Player>) playerList)
        player.PlayerCombatState.IncrementTurnNumber();
    }
    foreach (Creature creature in (IEnumerable<Creature>) this._state.Creatures)
      creature.OnSideSwitch();
    Action<CombatState> turnEnded = this.TurnEnded;
    if (turnEnded == null)
      return;
    turnEnded(this._state);
  }

  public void Pause()
  {
    if (NonInteractiveMode.IsActive || !this.IsInProgress)
      return;
    this.IsPaused = true;
  }

  public void Unpause()
  {
    if (NonInteractiveMode.IsActive)
      return;
    this.IsPaused = false;
  }

  public bool IsPartOfPlayerTurn(Player player)
  {
    CombatState state = this._state;
    // ISSUE: explicit non-virtual call
    if ((state != null ? (__nonvirtual (state.CurrentSide) != CombatSide.Player ? 1 : 0) : 1) != 0)
      return false;
    return this._playersTakingExtraTurn.Count == 0 || this._playersTakingExtraTurn.Contains(player);
  }

  public async Task WaitForUnpause()
  {
    if (NonInteractiveMode.IsActive)
      return;
    while (this.IsPaused && this.IsInProgress && this._state != null)
    {
      double num = (double) await ((Node) NGame.Instance).AwaitProcessFrame();
    }
  }

  public void DebugForceTopCardOnNextShuffle(CardModel card)
  {
    card.AssertMutable();
    this.DebugForcedTopCardOnNextShuffle = card;
  }

  public void DebugClearForcedTopCardOnNextShuffle()
  {
    this.DebugForcedTopCardOnNextShuffle = (CardModel) null;
  }

  private sealed record PendingLossState(CombatState State, CombatRoom Room);
}
