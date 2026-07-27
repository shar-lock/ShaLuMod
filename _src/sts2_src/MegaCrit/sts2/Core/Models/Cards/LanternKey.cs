// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.LanternKey
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Rooms;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class LanternKey : CardModel
{
  private const int _gloryActIndex = 2;

  public LanternKey()
    : base(-1, CardType.Quest, CardRarity.Quest, TargetType.Self)
  {
  }

  public override int MaxUpgradeLevel => 0;

  public override IEnumerable<CardKeyword> CanonicalKeywords
  {
    get
    {
      return (IEnumerable<CardKeyword>) new \u003C\u003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Unplayable);
    }
  }

  public override IReadOnlySet<RoomType> ModifyUnknownMapPointRoomTypes(
    IReadOnlySet<RoomType> roomTypes)
  {
    if (2 != this.Owner.RunState.CurrentActIndex)
      return roomTypes;
    return (IReadOnlySet<RoomType>) new HashSet<RoomType>()
    {
      RoomType.Event
    };
  }

  public override EventModel ModifyNextEvent(EventModel currentEvent)
  {
    return 2 != this.Owner.RunState.CurrentActIndex ? currentEvent : (EventModel) ModelDb.Event<WarHistorianRepy>();
  }
}
