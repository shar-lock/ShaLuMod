// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.GoldenCompass
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class GoldenCompass : RelicModel
{
  private int _goldenPathAct = -1;

  public override RelicRarity Rarity => RelicRarity.Ancient;

  public override bool HasUponPickupEffect => true;

  [SavedProperty]
  public int GoldenPathAct
  {
    get => this._goldenPathAct;
    set
    {
      this.AssertMutable();
      this._goldenPathAct = value;
    }
  }

  public override async Task AfterObtained()
  {
    this.GoldenPathAct = this.Owner.RunState.CurrentActIndex;
    await RunManager.Instance.GenerateMap();
  }

  public override ActMap ModifyGeneratedMap(IRunState runState, ActMap map, int actIndex)
  {
    return this.GoldenPathAct != actIndex ? map : (ActMap) new GoldenPathActMap(runState);
  }

  public override IReadOnlySet<RoomType> ModifyUnknownMapPointRoomTypes(
    IReadOnlySet<RoomType> roomTypes)
  {
    if (this.GoldenPathAct != this.Owner.RunState.CurrentActIndex)
      return roomTypes;
    return (IReadOnlySet<RoomType>) new HashSet<RoomType>()
    {
      RoomType.Event
    };
  }
}
