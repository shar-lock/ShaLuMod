// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.GameActions.Multiplayer.BranchingPlayerChoiceContext
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using System;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.GameActions.Multiplayer;

public class BranchingPlayerChoiceContext : PlayerChoiceContext
{
  private PlayerChoiceContext _originalContext;
  private readonly GameActionType _gameActionType;
  private readonly ulong _localPlayerId;
  private HookPlayerChoiceContext? _createdContext;
  private TaskCompletionSource _pausedCompletionSource = new TaskCompletionSource();

  public event Action<HookPlayerChoiceContext>? AfterBranched;

  public override ulong? OwnerId => this._originalContext.OwnerId;

  public BranchingPlayerChoiceContext(
    ulong localPlayerId,
    GameActionType gameActionType,
    PlayerChoiceContext existing)
  {
    this._originalContext = existing;
    this._gameActionType = gameActionType;
    this._localPlayerId = localPlayerId;
  }

  public override async Task SignalPlayerChoiceBegun(Player chooser, PlayerChoiceOptions options)
  {
    ulong? ownerId;
    PlayerChoiceContext playerChoiceContext;
    if (this._originalContext.OwnerId.HasValue)
    {
      long netId = (long) chooser.NetId;
      ownerId = this._originalContext.OwnerId;
      long valueOrDefault = (long) ownerId.GetValueOrDefault();
      if (!(netId == valueOrDefault & ownerId.HasValue))
      {
        if (this._createdContext != null)
        {
          Log.Warn($"{nameof (BranchingPlayerChoiceContext)} has been used twice! We switched owners from {this._originalContext.OwnerId} to {this._createdContext.OwnerId}, and now we are trying to switch to {chooser.NetId}. Re-using the existing created context.");
          playerChoiceContext = (PlayerChoiceContext) this._createdContext;
          goto label_10;
        }
        Log.LogMessage(LogLevel.Debug, LogType.GameSync, $"Branching context began choice for {chooser.NetId} who is not the owner ({this._originalContext.OwnerId}). Using new {"HookPlayerChoiceContext"}");
        this._createdContext = new HookPlayerChoiceContext(chooser, this._localPlayerId, this._gameActionType);
        Action<HookPlayerChoiceContext> afterBranched = this.AfterBranched;
        if (afterBranched != null)
          afterBranched(this._createdContext);
        this._pausedCompletionSource.SetResult();
        playerChoiceContext = (PlayerChoiceContext) this._createdContext;
        goto label_10;
      }
    }
    ownerId = this._originalContext.OwnerId;
    if (!ownerId.HasValue)
      Log.LogMessage(LogLevel.Debug, LogType.GameSync, $"Branching context began choice for {chooser.NetId} and there is no owner. Using existing {this._originalContext}");
    playerChoiceContext = this._originalContext;
label_10:
    await playerChoiceContext.SignalPlayerChoiceBegun(chooser, options);
  }

  public async Task AssignTaskAndWaitForPauseOrCompletion(Task task)
  {
    await TaskHelper.WhenAny(task, this._pausedCompletionSource.Task);
    if (!this._pausedCompletionSource.Task.IsCompleted)
      return;
    int num = await this._createdContext.AssignTaskAndWaitForPauseOrCompletion(task) ? 1 : 0;
  }

  public override Task SignalPlayerChoiceEnded()
  {
    return ((PlayerChoiceContext) this._createdContext ?? this._originalContext).SignalPlayerChoiceEnded();
  }
}
