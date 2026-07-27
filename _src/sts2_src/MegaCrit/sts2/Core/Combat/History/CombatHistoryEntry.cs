// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Combat.History.CombatHistoryEntry
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Combat.History;

public abstract class CombatHistoryEntry
{
  private readonly Dictionary<ulong, int> _playerTurnNumbers = new Dictionary<ulong, int>();

  public Creature Actor { get; }

  private int RoundNumber { get; }

  private CombatSide CurrentSide { get; }

  public CombatHistory History { get; }

  protected CombatHistoryEntry(
    Creature actor,
    int roundNumber,
    CombatSide currentSide,
    CombatHistory history,
    IEnumerable<Player> players)
  {
    this.Actor = actor;
    this.RoundNumber = roundNumber;
    this.CurrentSide = currentSide;
    this.History = history;
    foreach (Player player in players)
      this._playerTurnNumbers[player.NetId] = player.PlayerCombatState.TurnNumber;
  }

  public bool HappenedThisTurn(ICombatState? state)
  {
    if (state == null || this.RoundNumber != state.RoundNumber || this.CurrentSide != state.CurrentSide)
      return false;
    foreach (KeyValuePair<ulong, int> playerTurnNumber in this._playerTurnNumbers)
    {
      ulong num1;
      int num2;
      playerTurnNumber.Deconstruct(ref num1, ref num2);
      ulong playerId = num1;
      int num3 = num2;
      Player player = state.GetPlayer(playerId);
      int num4;
      if (player == null)
      {
        num4 = 1;
      }
      else
      {
        int? turnNumber = player.PlayerCombatState?.TurnNumber;
        num2 = num3;
        num4 = !(turnNumber.GetValueOrDefault() == num2 & turnNumber.HasValue) ? 1 : 0;
      }
      if (num4 != 0)
        return false;
    }
    return true;
  }

  public bool HappenedLastPlayerTurn(Player player)
  {
    int num1;
    if (!this._playerTurnNumbers.TryGetValue(player.NetId, out num1))
      return false;
    int num2 = num1;
    PlayerCombatState playerCombatState = player.PlayerCombatState;
    int? nullable = playerCombatState != null ? new int?(playerCombatState.TurnNumber - 1) : new int?();
    int valueOrDefault = nullable.GetValueOrDefault();
    return num2 == valueOrDefault & nullable.HasValue;
  }

  public string HumanReadableString
  {
    get => $"Rd {this.RoundNumber} ({this.CurrentSide} turn): {this.Description}.";
  }

  public abstract string Description { get; }
}
