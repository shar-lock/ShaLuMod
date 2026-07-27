// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.GameActions.GenericHookGameAction
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.GameActions;

public class GenericHookGameAction : GameAction
{
  private readonly TaskCompletionSource _executionStartedSource = new TaskCompletionSource();
  private readonly TaskCompletionSource _choiceContextSetSource = new TaskCompletionSource();
  private readonly GameActionType _gameActionType;
  public Task? debugArtificialDelayAfterTask;

  public override bool RecordableToReplay => false;

  public HookPlayerChoiceContext? ChoiceContext { get; private set; }

  public override ulong OwnerId { get; }

  public override GameActionType ActionType => this._gameActionType;

  public uint HookId { get; }

  public Task ExecutionStartedTask => this._executionStartedSource.Task;

  public GenericHookGameAction(uint hookId, ulong ownerId, GameActionType gameActionType)
  {
    this._gameActionType = gameActionType;
    this.HookId = hookId;
    this.OwnerId = ownerId;
    if (this._gameActionType != GameActionType.Combat && this._gameActionType != GameActionType.CombatPlayPhaseOnly)
      throw new InvalidOperationException($"Unexpected GameActionType {this._gameActionType} received for GenericHookGameAction!");
  }

  public void SetChoiceContext(HookPlayerChoiceContext choiceContext)
  {
    ulong? nullable1 = choiceContext.Owner?.NetId;
    ulong ownerId = this.OwnerId;
    if (!((long) nullable1.GetValueOrDefault() == (long) ownerId & nullable1.HasValue))
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(73, 2);
      interpolatedStringHandler.AppendLiteral("Assigned choice context with owner ");
      ref DefaultInterpolatedStringHandler local = ref interpolatedStringHandler;
      Player owner = choiceContext.Owner;
      ulong? nullable2;
      if (owner == null)
      {
        nullable1 = new ulong?();
        nullable2 = nullable1;
      }
      else
        nullable2 = new ulong?(owner.NetId);
      local.AppendFormatted<ulong?>(nullable2);
      interpolatedStringHandler.AppendLiteral(" to GenericHookGameAction with owner ");
      interpolatedStringHandler.AppendFormatted<ulong>(this.OwnerId);
      interpolatedStringHandler.AppendLiteral("!");
      throw new InvalidOperationException(interpolatedStringHandler.ToStringAndClear());
    }
    this.ChoiceContext = choiceContext;
    this._choiceContextSetSource.SetResult();
  }

  protected override async Task ExecuteAction()
  {
    await this._choiceContextSetSource.Task;
    this._executionStartedSource.SetResult();
    await this.ChoiceContext.Task;
    if (this.debugArtificialDelayAfterTask == null)
      return;
    await this.debugArtificialDelayAfterTask;
  }

  public override INetAction ToNetAction() => throw new NotImplementedException();

  public override string ToString()
  {
    return $"{nameof (GenericHookGameAction)} id {this.HookId} owner {this.OwnerId} source {this.ChoiceContext?.Source} last involved {this.ChoiceContext?.LastInvolvedModel}";
  }
}
