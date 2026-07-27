// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Commands.RelicCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Commands;

public static class RelicCmd
{
  public static async Task<T> Obtain<T>(Player player) where T : RelicModel
  {
    return (T) await RelicCmd.Obtain(ModelDb.Relic<T>().ToMutable(), player);
  }

  public static async Task<RelicModel> Obtain(RelicModel relic, Player player, int index = -1)
  {
    relic.AssertMutable();
    IRunState runState = player.RunState;
    runState.CurrentMapPointHistoryEntry?.GetEntry(player.NetId).RelicChoices.Add(new ModelChoiceHistoryEntry(relic.Id, true));
    player.AddRelicInternal(relic, index);
    if (!relic.IsStackable)
    {
      player.RelicGrabBag.Remove(relic);
      runState.SharedRelicGrabBag.Remove(relic);
    }
    if (LocalContext.IsMe(player))
    {
      NRun.Instance?.GlobalUi.RelicInventory.AnimateRelic(relic);
      NDebugAudioManager.Instance?.Play("relic_get.mp3");
      SaveManager.Instance.MarkRelicAsSeen(relic);
    }
    relic.FloorAddedToDeck = runState.TotalFloor;
    await relic.AfterObtained();
    return relic;
  }

  public static async Task Remove(RelicModel relic)
  {
    relic.Owner.RunState.CurrentMapPointHistoryEntry?.GetEntry(relic.Owner.NetId).RelicsRemoved.Add(relic.Id);
    relic.Owner.RemoveRelicInternal(relic);
    await relic.AfterRemoved();
  }

  public static async Task<RelicModel> Replace(RelicModel original, RelicModel replace)
  {
    original.AssertMutable();
    replace.AssertMutable();
    Player player = original.Owner;
    int indexOfOriginal = player.Relics.IndexOf<RelicModel>(original);
    await RelicCmd.Remove(original);
    RelicModel relicModel = await RelicCmd.Obtain(replace, player, indexOfOriginal);
    player = (Player) null;
    return relicModel;
  }

  public static async Task Melt(RelicModel relic)
  {
    relic.Owner.MeltRelicInternal(relic);
    await relic.AfterRemoved();
  }
}
