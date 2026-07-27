// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Timeline.Epochs.Event1Epoch
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#nullable enable
namespace MegaCrit.Sts2.Core.Timeline.Epochs;

public class Event1Epoch : EpochModel
{
  public override string Id => "EVENT1_EPOCH";

  public override EpochEra Era => EpochEra.Flourish1;

  public override int EraPosition => 0;

  public override string StoryId => "Tales_From_The_Spire";

  public static List<EventModel> Events
  {
    get
    {
      int capacity = 1;
      List<EventModel> events = new List<EventModel>(capacity);
      CollectionsMarshal.SetCount<EventModel>(events, capacity);
      CollectionsMarshal.AsSpan<EventModel>(events)[0] = (EventModel) ModelDb.Event<TrashHeap>();
      return events;
    }
  }

  public override string UnlockText
  {
    get
    {
      LocString locString = new LocString("epochs", this.Id + ".unlockText");
      locString.Add("Event", Event1Epoch.Events[0].Title);
      return locString.GetFormattedText();
    }
  }

  public override void QueueUnlocks() => NTimelineScreen.Instance.QueueMiscUnlock(this.UnlockText);
}
