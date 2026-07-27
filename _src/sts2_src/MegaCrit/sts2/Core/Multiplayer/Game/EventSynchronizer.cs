// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Game.EventSynchronizer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Sync;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Game;

public class EventSynchronizer : IDisposable
{
  private readonly INetGameService _netService;
  private readonly RunLocationTargetedMessageBuffer _messageBuffer;
  private readonly IPlayerCollection _playerCollection;
  private readonly ulong _localPlayerId;
  private readonly List<EventModel> _events = new List<EventModel>();
  private EventModel? _canonicalEvent;
  private readonly List<uint?> _playerVotes = new List<uint?>();
  private uint _pageIndex;
  private readonly List<Task> _pendingOptionTasks = new List<Task>();
  private readonly Rng _multiplayerOptionSelectionRng;
  private readonly EventCombatSynchronizer _combatSynchronizer;
  private readonly Logger _logger = new Logger(nameof (EventSynchronizer), LogType.GameSync);

  public IReadOnlyList<EventModel> Events => (IReadOnlyList<EventModel>) this._events;

  public bool IsShared
  {
    get
    {
      return (this._canonicalEvent ?? throw new InvalidOperationException("Event is not in progress!")).IsShared;
    }
  }

  public event Action<Player>? PlayerVoteChanged;

  private Player LocalPlayer => this._playerCollection.GetPlayer(this._localPlayerId);

  public EventSynchronizer(
    RunLocationTargetedMessageBuffer messageBuffer,
    INetGameService netService,
    IPlayerCollection playerCollection,
    IRunState runState,
    ulong localPlayerId,
    ulong seed)
  {
    this._netService = netService;
    this._messageBuffer = messageBuffer;
    this._playerCollection = playerCollection;
    this._localPlayerId = localPlayerId;
    this._multiplayerOptionSelectionRng = new Rng(seed, "event_synchronizer");
    this._combatSynchronizer = new EventCombatSynchronizer(playerCollection, runState);
    this._messageBuffer.RegisterMessageHandler<OptionIndexChosenMessage>(new MessageHandlerDelegate<OptionIndexChosenMessage>(this.HandleEventOptionChosenMessage));
    this._messageBuffer.RegisterMessageHandler<VotedForSharedEventOptionMessage>(new MessageHandlerDelegate<VotedForSharedEventOptionMessage>(this.HandleVotedForSharedEventOptionMessage));
    this._messageBuffer.RegisterMessageHandler<SharedEventOptionChosenMessage>(new MessageHandlerDelegate<SharedEventOptionChosenMessage>(this.HandleSharedEventOptionChosenMessage));
  }

  public void Dispose()
  {
    this._messageBuffer.UnregisterMessageHandler<OptionIndexChosenMessage>(new MessageHandlerDelegate<OptionIndexChosenMessage>(this.HandleEventOptionChosenMessage));
    this._messageBuffer.UnregisterMessageHandler<VotedForSharedEventOptionMessage>(new MessageHandlerDelegate<VotedForSharedEventOptionMessage>(this.HandleVotedForSharedEventOptionMessage));
    this._messageBuffer.UnregisterMessageHandler<SharedEventOptionChosenMessage>(new MessageHandlerDelegate<SharedEventOptionChosenMessage>(this.HandleSharedEventOptionChosenMessage));
  }

  public void BeginEvent(
    EventModel canonicalEvent,
    bool isPrefinished = false,
    Action<EventModel>? debugOnStart = null)
  {
    this._logger.Debug($"Beginning event {canonicalEvent.Id}, shared: {canonicalEvent.IsShared}");
    for (int count = this._playerVotes.Count; count < this._playerCollection.Players.Count; ++count)
      this._playerVotes.Add(new uint?());
    foreach (EventModel eventModel in this._events)
    {
      if (!eventModel.IsFinished)
      {
        this._logger.Warn($"Beginning new event {canonicalEvent}, but event {eventModel} for player {eventModel.Owner.NetId} is not yet finished!");
        eventModel.EnsureCleanup();
      }
    }
    this._events.Clear();
    this._pendingOptionTasks.Clear();
    this.ClearPlayerVotes();
    this._pageIndex = 0U;
    this._canonicalEvent = canonicalEvent;
    foreach (Player player in (IEnumerable<Player>) this._playerCollection.Players)
    {
      EventModel mutable = canonicalEvent.ToMutable();
      if (debugOnStart != null)
        debugOnStart(mutable);
      this._events.Add(mutable);
      TaskHelper.RunSafely(mutable.BeginEvent(player, this._combatSynchronizer, isPrefinished));
      this._logger.VeryDebug($"Event {mutable.Id} began for player {player.NetId} with options: {string.Join<EventOption>(",", (IEnumerable<EventOption>) mutable.CurrentOptions)}");
    }
  }

  private void HandleVotedForSharedEventOptionMessage(
    VotedForSharedEventOptionMessage message,
    ulong senderId)
  {
    this._logger.Debug($"Received {"VotedForSharedEventOptionMessage"} from player {senderId} for option {message.optionIndex} on page {message.pageIndex}");
    if (!this.IsShared)
      throw new InvalidOperationException("Received VotedForSharedEventOptionMessage during a non-shared event!");
    this.PlayerVotedForSharedOptionIndex(this._playerCollection.GetPlayer(senderId) ?? throw new InvalidOperationException($"Received {"VotedForSharedEventOptionMessage"} for player {senderId} that doesn't exist!"), message.optionIndex, message.pageIndex);
  }

  private void PlayerVotedForSharedOptionIndex(Player player, uint optionIndex, uint pageIndex)
  {
    if (pageIndex < this._pageIndex)
      this._logger.Warn($"Received message from player {player.NetId} voting for option {optionIndex} on page {pageIndex}, but we are on greater page {this._pageIndex}. Ignoring vote");
    else if (pageIndex > this._pageIndex)
    {
      this._logger.Error($"Received message from player {player.NetId} voting for option {optionIndex} on page {pageIndex}, but we are on lesser page {this._pageIndex}. This is a bug!");
    }
    else
    {
      this._playerVotes[this._playerCollection.GetPlayerSlotIndex(player)] = new uint?(optionIndex);
      Action<Player> playerVoteChanged = this.PlayerVoteChanged;
      if (playerVoteChanged != null)
        playerVoteChanged(player);
      if (!this._playerVotes.All<uint?>((Func<uint?, bool>) (p => p.HasValue)) || this._netService.Type == NetGameType.Client)
        return;
      this._logger.Debug("All votes received and we are host. Choosing shared event option");
      this.ChooseSharedEventOption();
    }
  }

  private void ChooseSharedEventOption()
  {
    if (this._netService.Type == NetGameType.Client)
      throw new InvalidOperationException("Only host should be choosing shared event option!");
    this._logger.Debug($"All votes received on host for shared event {this._canonicalEvent}, choosing option");
    uint optionIndex = this._multiplayerOptionSelectionRng.NextItem<uint?>((IEnumerable<uint?>) this._playerVotes).Value;
    this._logger.Debug($"Shared event option {optionIndex} was chosen");
    this._netService.SendMessage<SharedEventOptionChosenMessage>(new SharedEventOptionChosenMessage()
    {
      optionIndex = optionIndex,
      pageIndex = this._pageIndex,
      location = this._messageBuffer.CurrentLocation
    });
    this.ChooseOptionForSharedEvent(optionIndex);
  }

  private void HandleSharedEventOptionChosenMessage(
    SharedEventOptionChosenMessage message,
    ulong senderId)
  {
    this._logger.Debug($"Received {"SharedEventOptionChosenMessage"} for option {message.optionIndex} on page {message.pageIndex}");
    if (this._netService.Type != NetGameType.Client)
      throw new InvalidOperationException($"Received {"SharedEventOptionChosenMessage"} on non-client! {this._netService.Type}");
    if ((int) this._pageIndex != (int) message.pageIndex)
      throw new InvalidOperationException($"Received {"SharedEventOptionChosenMessage"} for page {message.pageIndex} while we were on page {this._pageIndex}!");
    this.ChooseOptionForSharedEvent(message.optionIndex);
  }

  private void HandleEventOptionChosenMessage(OptionIndexChosenMessage message, ulong senderId)
  {
    if (message.type != OptionIndexType.Event)
      return;
    this._logger.Debug($"Received {"OptionIndexChosenMessage"} from player {senderId} for event option index {message.optionIndex}");
    if (this.IsShared)
      throw new InvalidOperationException("Received OptionIndexChosenMessage during a shared event!");
    this.ChooseOptionForEvent(this._playerCollection.GetPlayer(senderId) ?? throw new InvalidOperationException($"Received EventOptionChosenMessage for player {senderId} that doesn't exist!"), (int) message.optionIndex);
  }

  public void ChooseLocalOption(int index)
  {
    if (this.IsShared)
    {
      this._logger.Debug($"Local player voted for shared event option index {index}");
      this.PlayerVotedForSharedOptionIndex(this.LocalPlayer, (uint) index, this._pageIndex);
      this._netService.SendMessage<VotedForSharedEventOptionMessage>(new VotedForSharedEventOptionMessage()
      {
        optionIndex = (uint) index,
        pageIndex = this._pageIndex,
        location = this._messageBuffer.CurrentLocation
      });
    }
    else
    {
      this._logger.Debug($"Local player chose event option index {index}");
      this.ChooseOptionForEvent(this.LocalPlayer, index);
      this._netService.SendMessage<OptionIndexChosenMessage>(new OptionIndexChosenMessage()
      {
        type = OptionIndexType.Event,
        optionIndex = (uint) index,
        location = this._messageBuffer.CurrentLocation
      });
    }
  }

  private void ChooseOptionForSharedEvent(uint optionIndex)
  {
    if (!this.IsShared)
      throw new InvalidOperationException("ChooseOptionForSharedEvent called during non-shared event!");
    this._logger.Debug($"Choosing option index {optionIndex} for shared event on page {this._pageIndex}");
    this.ClearPlayerVotes();
    ++this._pageIndex;
    foreach (Player player in (IEnumerable<Player>) this._playerCollection.Players)
      this.ChooseOptionForEvent(player, (int) optionIndex);
  }

  private void ChooseOptionForEvent(Player player, int optionIndex)
  {
    EventModel eventForPlayer = this.GetEventForPlayer(player);
    if (eventForPlayer.IsFinished)
      throw new InvalidOperationException($"Option chosen for player {player} on {eventForPlayer}, but it is already finished!");
    if (optionIndex >= eventForPlayer.CurrentOptions.Count)
      throw new InvalidOperationException($"Player {player.NetId} attempted to choose option index {optionIndex} in event {eventForPlayer.Id}, but there were only {eventForPlayer.CurrentOptions.Count} options available!");
    this._logger.Debug($"Option index {optionIndex} chosen for player {player.NetId} in event {eventForPlayer.Id}. Choice key: {eventForPlayer.CurrentOptions[optionIndex].TextKey}");
    EventOption currentOption = eventForPlayer.CurrentOptions[optionIndex];
    this._pendingOptionTasks.Add(TaskHelper.RunSafely(currentOption.Chosen()));
    this.SaveEventOptionToHistory(player, currentOption);
  }

  private void SaveEventOptionToHistory(Player player, EventOption option)
  {
    if (!option.ShouldSaveChoiceToHistory)
      return;
    EventOptionHistoryEntry optionHistoryEntry = new EventOptionHistoryEntry()
    {
      Title = option.HistoryName,
      Variables = new Dictionary<string, object>()
    };
    if (option.ShouldSaveVariablesToHistory)
    {
      foreach (KeyValuePair<string, object> variable in (IEnumerable<KeyValuePair<string, object>>) option.HistoryName.Variables)
        optionHistoryEntry.Variables[variable.Key] = variable.Value;
    }
    player.RunState.CurrentMapPointHistoryEntry?.GetEntry(player.NetId).EventChoices.Add(optionHistoryEntry);
  }

  private void ClearPlayerVotes()
  {
    for (int index = 0; index < this._playerVotes.Count; ++index)
      this._playerVotes[index] = new uint?();
  }

  public uint? GetPlayerVote(Player player)
  {
    return this._playerVotes[this._playerCollection.GetPlayerSlotIndex(player)];
  }

  public EventModel GetLocalEvent() => this.GetEventForPlayer(this.LocalPlayer);

  public EventModel GetEventForPlayer(Player player)
  {
    return this._events[this._playerCollection.GetPlayerSlotIndex(player)];
  }

  public void ResumeEvents(AbstractRoom exitedRoom)
  {
    foreach (EventModel eventModel in this._events)
      TaskHelper.RunSafely(eventModel.Resume(exitedRoom));
  }

  public void GenerateInternalCombatStateIfNecessary(EventModel localEvent)
  {
    this._combatSynchronizer.InitializeForEvent(localEvent);
  }

  public void BeforeExitingRoom() => this._combatSynchronizer.ResetState();

  public async Task AwaitPendingOptionTasks()
  {
    try
    {
      await Task.WhenAll((IEnumerable<Task>) this._pendingOptionTasks);
    }
    catch (Exception ex)
    {
    }
    this._pendingOptionTasks.Clear();
  }
}
