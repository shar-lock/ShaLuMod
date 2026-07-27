// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Timeline.Epochs.Necrobinder3Epoch
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#nullable enable
namespace MegaCrit.Sts2.Core.Timeline.Epochs;

public class Necrobinder3Epoch : EpochModel
{
  public override string Id => "NECROBINDER3_EPOCH";

  public override EpochEra Era => EpochEra.Invitation1;

  public override int EraPosition => 3;

  public override string StoryId => "Necrobinder";

  public static List<RelicModel> Relics
  {
    get
    {
      int capacity = 3;
      List<RelicModel> relics = new List<RelicModel>(capacity);
      CollectionsMarshal.SetCount<RelicModel>(relics, capacity);
      Span<RelicModel> span = CollectionsMarshal.AsSpan<RelicModel>(relics);
      int num1 = 0;
      span[num1] = (RelicModel) ModelDb.Relic<BoneFlute>();
      int num2 = num1 + 1;
      span[num2] = (RelicModel) ModelDb.Relic<FuneraryMask>();
      int num3 = num2 + 1;
      span[num3] = (RelicModel) ModelDb.Relic<BookRepairKnife>();
      return relics;
    }
  }

  public override string UnlockText => this.CreateRelicUnlockText(Necrobinder3Epoch.Relics);

  public override void QueueUnlocks()
  {
    NTimelineScreen.Instance.QueueRelicUnlock(Necrobinder3Epoch.Relics);
  }
}
