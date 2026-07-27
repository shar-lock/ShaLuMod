// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.GameActions.ReadyToBeginEnemyTurnAction
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.GameActions;

public class ReadyToBeginEnemyTurnAction : GameAction
{
  private readonly Player _player;
  private readonly Func<Task>? _actionDuringEnemyTurn;

  public override ulong OwnerId => this._player.NetId;

  public override GameActionType ActionType => GameActionType.Combat;

  public ReadyToBeginEnemyTurnAction(Player player, Func<Task>? actionDuringEnemyTurn = null)
  {
    this._player = player;
    this._actionDuringEnemyTurn = actionDuringEnemyTurn;
  }

  protected override Task ExecuteAction()
  {
    CombatManager.Instance.SetReadyToBeginEnemyTurn(this._player, this._actionDuringEnemyTurn);
    return Task.CompletedTask;
  }

  public override INetAction ToNetAction() => (INetAction) new NetReadyToBeginEnemyTurnAction();

  protected override void CancelAction() => Log.Debug($"Cancel\n{new StackTrace()}");

  public override string ToString()
  {
    return $"{nameof (ReadyToBeginEnemyTurnAction)} {this._player.NetId}";
  }
}
