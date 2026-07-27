// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Game.RestSiteSynchronizer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Flavor;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Sync;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Game;

public class RestSiteSynchronizer : IDisposable
{
  public const int minHoverMessageMsec = 50;
  private readonly INetGameService _netService;
  private readonly RunLocationTargetedMessageBuffer _messageBuffer;
  private readonly IPlayerCollection _playerCollection;
  private readonly ulong _localPlayerId;
  private readonly RunLobby? _runLobby;
  private readonly List<RestSiteSynchronizer.PlayerRestSite> _restSites = new List<RestSiteSynchronizer.PlayerRestSite>();
  private readonly Logger _logger = new Logger(nameof (RestSiteSynchronizer), LogType.GameSync);
  private ulong _lastHoverMessageMsec;
  private RestSiteOptionHoveredMessage? _hoveredMessage;
  private Task? _hoverMessageTask;

  private Player LocalPlayer => this._playerCollection.GetPlayer(this._localPlayerId);

  public event Action<ulong>? PlayerHoverChanged;

  public event Action<RestSiteOption, ulong>? BeforePlayerOptionChosen;

  public event Action<RestSiteOption, bool, ulong>? AfterPlayerOptionChosen;

  public RestSiteSynchronizer(
    RunLocationTargetedMessageBuffer messageBuffer,
    INetGameService netService,
    IPlayerCollection playerCollection,
    ulong localPlayerId,
    RunLobby? runLobby)
  {
    this._netService = netService;
    this._messageBuffer = messageBuffer;
    this._playerCollection = playerCollection;
    this._localPlayerId = localPlayerId;
    this._runLobby = runLobby;
    this._messageBuffer.RegisterMessageHandler<OptionIndexChosenMessage>(new MessageHandlerDelegate<OptionIndexChosenMessage>(this.HandleRestSiteOptionChosenMessage));
    this._messageBuffer.RegisterMessageHandler<RestSiteOptionHoveredMessage>(new MessageHandlerDelegate<RestSiteOptionHoveredMessage>(this.HandleRestSiteOptionHoveredMessage));
    this._messageBuffer.RegisterMessageHandler<RestSiteSkippedMessage>(new MessageHandlerDelegate<RestSiteSkippedMessage>(this.HandleRestSiteSkippedMessage));
    if (this._runLobby == null)
      return;
    this._runLobby.RemotePlayerDisconnected += new Action<ulong>(this.OnPeerDisconnected);
  }

  public void Dispose()
  {
    this._messageBuffer.UnregisterMessageHandler<OptionIndexChosenMessage>(new MessageHandlerDelegate<OptionIndexChosenMessage>(this.HandleRestSiteOptionChosenMessage));
    this._messageBuffer.UnregisterMessageHandler<RestSiteOptionHoveredMessage>(new MessageHandlerDelegate<RestSiteOptionHoveredMessage>(this.HandleRestSiteOptionHoveredMessage));
    this._messageBuffer.UnregisterMessageHandler<RestSiteSkippedMessage>(new MessageHandlerDelegate<RestSiteSkippedMessage>(this.HandleRestSiteSkippedMessage));
    if (this._runLobby != null)
      this._runLobby.RemotePlayerDisconnected -= new Action<ulong>(this.OnPeerDisconnected);
    this._hoverMessageTask?.Dispose();
    this._hoverMessageTask = (Task) null;
  }

  public void BeginRestSite()
  {
    this._logger.Debug("Beginning rest site");
    this._restSites.Clear();
    foreach (Player player in (IEnumerable<Player>) this._playerCollection.Players)
    {
      List<RestSiteOption> values = RestSiteOption.Generate(player);
      this._restSites.Add(new RestSiteSynchronizer.PlayerRestSite()
      {
        options = values
      });
      this._logger.VeryDebug($"Rest site began for player {player.NetId} with options: {string.Join<RestSiteOption>(",", (IEnumerable<RestSiteOption>) values)}");
    }
  }

  private void HandleRestSiteOptionChosenMessage(OptionIndexChosenMessage message, ulong senderId)
  {
    if (message.type != OptionIndexType.RestSite)
      return;
    this._logger.Debug($"Player {senderId} chose rest site option index {message.optionIndex}");
    TaskHelper.RunSafely((Task) this.ChooseOption(this._playerCollection.GetPlayer(senderId) ?? throw new InvalidOperationException($"Received EventOptionChosenMessage for player {senderId} that doesn't exist!"), (int) message.optionIndex));
  }

  private void HandleRestSiteOptionHoveredMessage(
    RestSiteOptionHoveredMessage message,
    ulong senderId)
  {
    this._restSites[this._playerCollection.GetPlayerSlotIndex(this._playerCollection.GetPlayer(senderId))].hoveredOptionIndex = message.optionIndex;
    Action<ulong> playerHoverChanged = this.PlayerHoverChanged;
    if (playerHoverChanged == null)
      return;
    playerHoverChanged(senderId);
  }

  private void HandleRestSiteSkippedMessage(RestSiteSkippedMessage message, ulong senderId)
  {
    this._logger.Debug($"Player {senderId} skipped remaining options at rest site");
    RestSiteSynchronizer.PlayerRestSite restSite = this._restSites[this._playerCollection.GetPlayerSlotIndex(this._playerCollection.GetPlayer(senderId))];
    if (restSite.completionTaskSource.Task.IsCompleted)
      return;
    this._logger.Debug($"Skipping {restSite.options.Count} options");
    restSite.options.Clear();
    restSite.completionTaskSource.TrySetResult();
  }

  private void OnPeerDisconnected(ulong peerId)
  {
    Player player = this._playerCollection.GetPlayer(peerId);
    if (player == null)
      return;
    int playerSlotIndex = this._playerCollection.GetPlayerSlotIndex(player);
    if (playerSlotIndex >= this._restSites.Count)
      return;
    RestSiteSynchronizer.PlayerRestSite restSite = this._restSites[playerSlotIndex];
    if (restSite.completionTaskSource.Task.IsCompleted)
      return;
    this._logger.Debug($"Player {peerId} disconnected with {restSite.options.Count} rest site options remaining; completing their rest site");
    restSite.options.Clear();
    restSite.completionTaskSource.SetResult();
  }

  public Task<bool> ChooseLocalOption(int index)
  {
    this._logger.Debug($"Local player chose rest site option index {index}");
    this._netService.SendMessage<OptionIndexChosenMessage>(new OptionIndexChosenMessage()
    {
      type = OptionIndexType.RestSite,
      optionIndex = (uint) index,
      location = this._messageBuffer.CurrentLocation
    });
    return this.ChooseOption(this.LocalPlayer, index);
  }

  private async Task<bool> ChooseOption(Player player, int optionIndex)
  {
    RestSiteSynchronizer.PlayerRestSite restSite = this._restSites[this._playerCollection.GetPlayerSlotIndex(player)];
    if (restSite.completionTaskSource.Task.IsCompleted)
      throw new InvalidOperationException($"Player {player.NetId} attempted to choose rest site option index {optionIndex}, but the rest site has already been completed!");
    if (optionIndex >= restSite.options.Count)
      throw new InvalidOperationException($"Player {player.NetId} attempted to choose rest site option index {optionIndex}, but there were only {restSite.options.Count} options available!");
    RestSiteOption option = restSite.options[optionIndex];
    Action<RestSiteOption, ulong> playerOptionChosen1 = this.BeforePlayerOptionChosen;
    if (playerOptionChosen1 != null)
      playerOptionChosen1(option, player.NetId);
    bool flag = await option.OnSelect();
    this._logger.Debug($"Rest site option index {optionIndex} chosen for player {player.NetId} with success {flag}. Option: {option.OptionId}");
    restSite.lastChosenOptionIndex = new uint?((uint) optionIndex);
    Action<RestSiteOption, bool, ulong> playerOptionChosen2 = this.AfterPlayerOptionChosen;
    if (playerOptionChosen2 != null)
      playerOptionChosen2(option, flag, player.NetId);
    if (!flag)
      return false;
    player.RunState.CurrentMapPointHistoryEntry?.GetEntry(player.NetId).RestSiteChoices.Add(option.OptionId);
    if (restSite.completionTaskSource.Task.IsCompleted)
      return true;
    if (Hook.ShouldDisableRemainingRestSiteOptions(player.RunState, player))
    {
      this._logger.Debug($"Clearing all remaining rest site options for player {player.NetId}");
      restSite.options.Clear();
    }
    else
    {
      this._logger.Debug($"Leaving remaining rest site options enabled for player {player.NetId}");
      restSite.options.RemoveAt(optionIndex);
    }
    if (restSite.options.Count == 0)
    {
      this._logger.Debug($"Completing rest site for player {player.NetId} because there are no options left");
      restSite.completionTaskSource.SetResult();
    }
    return true;
  }

  public void BeforeLocalRestSiteExited()
  {
    Player me = LocalContext.GetMe(this._playerCollection);
    RestSiteSynchronizer.PlayerRestSite restSite = this._restSites[this._playerCollection.GetPlayerSlotIndex(me)];
    if (restSite.options.Count <= 0)
      return;
    this._logger.Debug($"Skipping remaining options ({restSite.options.Count}) in rest site for local player {me.NetId} because we're exiting the rest site");
    restSite.options.Clear();
    restSite.completionTaskSource.SetResult();
    this._netService.SendMessage<RestSiteSkippedMessage>(new RestSiteSkippedMessage()
    {
      Location = this._messageBuffer.CurrentLocation
    });
  }

  public async Task AfterAllRestSitesCompleted()
  {
    foreach (RestSiteSynchronizer.PlayerRestSite restSite in this._restSites)
      await restSite.completionTaskSource.Task;
  }

  public void LocalOptionHovered(RestSiteOption? option)
  {
    RestSiteSynchronizer.PlayerRestSite restSite = this._restSites[this._playerCollection.GetPlayerSlotIndex(LocalContext.GetMe(this._playerCollection))];
    restSite.hoveredOptionIndex = !(option != (RestSiteOption) null) ? new uint?() : new uint?((uint) restSite.options.IndexOf(option));
    this._hoveredMessage.GetValueOrDefault();
    if (!this._hoveredMessage.HasValue)
      this._hoveredMessage = new RestSiteOptionHoveredMessage?(new RestSiteOptionHoveredMessage()
      {
        Location = this._messageBuffer.CurrentLocation,
        optionIndex = restSite.hoveredOptionIndex
      });
    this.TrySendHoverMessage();
    Action<ulong> playerHoverChanged = this.PlayerHoverChanged;
    if (playerHoverChanged == null)
      return;
    playerHoverChanged(LocalContext.NetId.Value);
  }

  public int? GetHoveredOptionIndex(ulong playerId)
  {
    uint? hoveredOptionIndex = this._restSites[this._playerCollection.GetPlayerSlotIndex(this._playerCollection.GetPlayer(playerId))].hoveredOptionIndex;
    return !hoveredOptionIndex.HasValue ? new int?() : new int?((int) hoveredOptionIndex.GetValueOrDefault());
  }

  public int? GetChosenOptionIndex(ulong playerId)
  {
    uint? chosenOptionIndex = this._restSites[this._playerCollection.GetPlayerSlotIndex(this._playerCollection.GetPlayer(playerId))].lastChosenOptionIndex;
    return !chosenOptionIndex.HasValue ? new int?() : new int?((int) chosenOptionIndex.GetValueOrDefault());
  }

  public IReadOnlyList<RestSiteOption> GetLocalOptions()
  {
    return this.GetOptionsForPlayer(this.LocalPlayer);
  }

  public IReadOnlyList<RestSiteOption> GetOptionsForPlayer(ulong playerId)
  {
    return this.GetOptionsForPlayer(this._playerCollection.GetPlayer(playerId));
  }

  public IReadOnlyList<RestSiteOption> GetOptionsForPlayer(Player player)
  {
    return (IReadOnlyList<RestSiteOption>) this._restSites[this._playerCollection.GetPlayerSlotIndex(player)].options;
  }

  private void TrySendHoverMessage()
  {
    if (this._hoverMessageTask != null)
      return;
    int delayMsec = (int) ((long) this._lastHoverMessageMsec + 50L - (long) Time.GetTicksMsec());
    if (delayMsec <= 0)
      this._hoverMessageTask = TaskHelper.RunSafely(this.SendHoverMessageAfterSmallDelay());
    else
      this._hoverMessageTask = TaskHelper.RunSafely(this.QueueHoverMessage(delayMsec));
  }

  private async Task QueueHoverMessage(int delayMsec)
  {
    await Task.Delay(delayMsec);
    this.SendHoverMessage();
  }

  private async Task SendHoverMessageAfterSmallDelay()
  {
    await Task.Yield();
    this.SendHoverMessage();
  }

  private void SendHoverMessage()
  {
    if (!this._netService.IsConnected)
      return;
    this._netService.SendMessage<RestSiteOptionHoveredMessage>(this._hoveredMessage.Value);
    this._lastHoverMessageMsec = Time.GetTicksMsec();
    this._hoveredMessage = new RestSiteOptionHoveredMessage?();
    this._hoverMessageTask = (Task) null;
  }

  private class PlayerRestSite
  {
    public List<RestSiteOption> options;
    public uint? lastChosenOptionIndex;
    public uint? hoveredOptionIndex;
    public TaskCompletionSource completionTaskSource = new TaskCompletionSource();
  }
}
