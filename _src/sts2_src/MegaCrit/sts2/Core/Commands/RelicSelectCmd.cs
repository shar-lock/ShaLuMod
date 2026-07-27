// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Commands.RelicSelectCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Commands;

public static class RelicSelectCmd
{
  private static bool ShouldSelectLocalRelic(Player player)
  {
    return LocalContext.IsMe(player) && RunManager.Instance.NetService.Type != NetGameType.Replay;
  }

  public static async Task<RelicModel?> FromChooseARelicScreen(
    Player player,
    IReadOnlyList<RelicModel> relics)
  {
    uint choiceId = RunManager.Instance.PlayerChoiceSynchronizer.ReserveChoiceId(player);
    RelicModel relicModel;
    if (RelicSelectCmd.ShouldSelectLocalRelic(player))
    {
      NChooseARelicSelection nchooseArelicSelection = NChooseARelicSelection.ShowScreen(relics);
      if (LocalContext.IsMe(player))
      {
        foreach (RelicModel relic in (IEnumerable<RelicModel>) relics)
          SaveManager.Instance.MarkRelicAsSeen(relic);
      }
      relicModel = (await nchooseArelicSelection.RelicsSelected()).FirstOrDefault<RelicModel>();
      RunManager.Instance.PlayerChoiceSynchronizer.SyncLocalChoice(player, choiceId, PlayerChoiceResult.FromIndex(new int?(relics.IndexOf<RelicModel>(relicModel))));
    }
    else
    {
      int index = (await RunManager.Instance.PlayerChoiceSynchronizer.WaitForRemoteChoice(player, choiceId)).AsIndex();
      relicModel = index < 0 ? (RelicModel) null : relics[index];
    }
    return relicModel;
  }
}
