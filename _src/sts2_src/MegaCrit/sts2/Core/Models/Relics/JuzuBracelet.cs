// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.JuzuBracelet
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class JuzuBracelet : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Common;

  public override bool IsAllowed(IRunState runState)
  {
    return RelicModel.IsBeforeAct3TreasureChest(runState);
  }

  public override IReadOnlySet<RoomType> ModifyUnknownMapPointRoomTypes(
    IReadOnlySet<RoomType> roomTypes)
  {
    HashSet<RoomType> roomTypeSet1 = new HashSet<RoomType>();
    foreach (RoomType roomType in (IEnumerable<RoomType>) roomTypes)
      roomTypeSet1.Add(roomType);
    HashSet<RoomType> roomTypeSet2 = roomTypeSet1;
    roomTypeSet2.Remove(RoomType.Monster);
    return (IReadOnlySet<RoomType>) roomTypeSet2;
  }
}
