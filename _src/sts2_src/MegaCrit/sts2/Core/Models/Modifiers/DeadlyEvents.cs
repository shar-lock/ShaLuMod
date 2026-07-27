// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Modifiers.DeadlyEvents
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Modifiers;

public class DeadlyEvents : ModifierModel
{
  protected override void AfterRunCreated(RunState runState)
  {
    foreach (Player player in (IEnumerable<Player>) runState.Players)
      player.RelicGrabBag.Remove<JuzuBracelet>();
    runState.SharedRelicGrabBag.Remove<JuzuBracelet>();
    runState.Odds.UnknownMapPoint.EliteOdds = 0.1f;
    runState.Odds.UnknownMapPoint.SetBaseOdds(RoomType.Elite, 0.1f);
  }

  protected override void AfterRunLoaded(RunState runState)
  {
    runState.Odds.UnknownMapPoint.SetBaseOdds(RoomType.Elite, 0.1f);
  }

  public override float ModifyOddsIncreaseForUnrolledRoomType(RoomType roomType, float oddsIncrease)
  {
    return roomType != RoomType.Treasure ? oddsIncrease : oddsIncrease * 2f;
  }
}
