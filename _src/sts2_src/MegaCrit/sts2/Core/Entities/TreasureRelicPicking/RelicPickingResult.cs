// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.TreasureRelicPicking.RelicPickingResult
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.TreasureRelicPicking;

public class RelicPickingResult
{
  public RelicPickingResultType type;
  public required RelicModel relic;
  public required Player? player;
  public RelicPickingFight? fight;

  public static RelicPickingResult GenerateRelicFight(
    List<Player> players,
    RelicModel relic,
    Func<RelicPickingFightMove> generateMove)
  {
    RelicPickingFight relicPickingFight = new RelicPickingFight();
    relicPickingFight.playersInvolved.AddRange((IEnumerable<Player>) players);
    HashSet<Player> playerSet = new HashSet<Player>();
    foreach (Player player in players)
      playerSet.Add(player);
    HashSet<Player> source = playerSet;
    while (source.Count > 1)
    {
      RelicPickingFightRound pickingFightRound = new RelicPickingFightRound();
      RelicPickingFightMove? nullable1;
      foreach (Player player in players)
      {
        if (source.Contains(player))
        {
          RelicPickingFightMove pickingFightMove = generateMove();
          pickingFightRound.moves.Add(new RelicPickingFightMove?(pickingFightMove));
        }
        else
        {
          List<RelicPickingFightMove?> moves = pickingFightRound.moves;
          nullable1 = new RelicPickingFightMove?();
          RelicPickingFightMove? nullable2 = nullable1;
          moves.Add(nullable2);
        }
      }
      relicPickingFight.rounds.Add(pickingFightRound);
      List<RelicPickingFightMove> list = pickingFightRound.moves.OfType<RelicPickingFightMove>().Distinct<RelicPickingFightMove>().ToList<RelicPickingFightMove>();
      if (list.Count == 2)
      {
        RelicPickingFightMove losingMove = RelicPickingResult.GetLosingMove(list[0], list[1]);
        for (int index = 0; index < players.Count; ++index)
        {
          nullable1 = pickingFightRound.moves[index];
          RelicPickingFightMove pickingFightMove = losingMove;
          if (nullable1.GetValueOrDefault() == pickingFightMove & nullable1.HasValue)
            source.Remove(players[index]);
        }
      }
    }
    return new RelicPickingResult()
    {
      type = RelicPickingResultType.FoughtOver,
      player = source.First<Player>(),
      relic = relic,
      fight = relicPickingFight
    };
  }

  private static RelicPickingFightMove GetLosingMove(
    RelicPickingFightMove move1,
    RelicPickingFightMove move2)
  {
    return (RelicPickingFightMove) ((int) (move1 + 1) % 3) == move2 ? move1 : move2;
  }
}
