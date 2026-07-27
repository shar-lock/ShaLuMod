// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.GameActions.Multiplayer.GameActionPlayerChoiceContext
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Runs;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.GameActions.Multiplayer;

public class GameActionPlayerChoiceContext : PlayerChoiceContext
{
  private ActionQueueSynchronizer? _actionQueueSynchronizer;
  private ActionQueueSet? _actionQueueSet;
  private ActionExecutor? _actionExecutor;

  public GameAction Action { get; }

  public override ulong? OwnerId => new ulong?(this.Action.OwnerId);

  private ActionQueueSynchronizer ActionQueueSynchronizer
  {
    get => this._actionQueueSynchronizer ?? RunManager.Instance.ActionQueueSynchronizer;
  }

  private ActionQueueSet ActionQueueSet
  {
    get => this._actionQueueSet ?? RunManager.Instance.ActionQueueSet;
  }

  private ActionExecutor ActionExecutor
  {
    get => this._actionExecutor ?? RunManager.Instance.ActionExecutor;
  }

  public GameActionPlayerChoiceContext(GameAction action) => this.Action = action;

  public override Task SignalPlayerChoiceBegun(Player chooser, PlayerChoiceOptions options)
  {
    long netId = (long) chooser.NetId;
    ulong? ownerId = this.OwnerId;
    long valueOrDefault = (long) ownerId.GetValueOrDefault();
    if (!(netId == valueOrDefault & ownerId.HasValue))
      Log.Warn($"{nameof (GameActionPlayerChoiceContext)} is executing player choice owned by {chooser.NetId}, but the player choice began with owner {this.OwnerId}! This will work, but will likely result in some weird-looking user experience. See {"BlockingPlayerChoiceContext"} for a resolution.");
    if (this.ActionExecutor != null && this.ActionExecutor.CurrentlyRunningAction != this.Action)
    {
      Log.Error($"Tried to interrupt shared queue action {this.ActionExecutor.CurrentlyRunningAction} with a player choice context with action {this.Action}!");
      return Task.CompletedTask;
    }
    this.ActionQueueSet.PauseActionForPlayerChoice(this.Action, options);
    return Task.CompletedTask;
  }

  public override async Task SignalPlayerChoiceEnded()
  {
    long ownerId = (long) this.Action.OwnerId;
    ulong? netId = LocalContext.NetId;
    long valueOrDefault = (long) netId.GetValueOrDefault();
    if (ownerId == valueOrDefault & netId.HasValue)
      this.ActionQueueSynchronizer.RequestResumeActionAfterPlayerChoice(this.Action);
    await this.Action.WaitForActionToResumeExecutingAfterPlayerChoice();
  }

  public void MockDependenciesForTest(
    ActionQueueSynchronizer? actionQueueSynchronizer,
    ActionQueueSet? actionQueueSet,
    ActionExecutor? actionExecutor)
  {
    this._actionQueueSet = actionQueueSet;
    this._actionQueueSynchronizer = actionQueueSynchronizer;
    this._actionExecutor = actionExecutor;
  }
}
