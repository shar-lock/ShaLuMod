// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.GameActions.EndPlayerTurnAction
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Runs;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.GameActions;

public class EndPlayerTurnAction : GameAction
{
  private readonly Player _player;
  private readonly int _turnNumber;

  public override ulong OwnerId => this._player.NetId;

  public override GameActionType ActionType => GameActionType.CombatPlayPhaseOnly;

  public EndPlayerTurnAction(Player player, int turnNumber)
  {
    this._player = player;
    this._turnNumber = turnNumber;
  }

  protected override Task ExecuteAction()
  {
    int turnNumber = this._player.PlayerCombatState.TurnNumber;
    if (turnNumber == this._turnNumber)
      PlayerCmd.EndTurn(this._player, true);
    else
      Log.Info($"Ignoring end turn action. Current turn number: {turnNumber} action turn number: {this._turnNumber} CombatState: {RunManager.Instance.ActionQueueSynchronizer.CombatState}");
    return Task.CompletedTask;
  }

  public override INetAction ToNetAction()
  {
    return (INetAction) new NetEndPlayerTurnAction()
    {
      turnNumber = this._turnNumber
    };
  }

  public override string ToString()
  {
    return $"{nameof (EndPlayerTurnAction)} for player {this._player.NetId} turn {this._turnNumber}";
  }
}
