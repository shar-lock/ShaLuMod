// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Timeline.Epochs.Relic4Epoch
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

public class Relic4Epoch : EpochModel
{
  public override string Id => "RELIC4_EPOCH";

  public override EpochEra Era => EpochEra.Blight0;

  public override int EraPosition => 0;

  public override string StoryId => "Magnum_Opus";

  public static List<RelicModel> Relics
  {
    get
    {
      int capacity = 3;
      List<RelicModel> relics = new List<RelicModel>(capacity);
      CollectionsMarshal.SetCount<RelicModel>(relics, capacity);
      Span<RelicModel> span = CollectionsMarshal.AsSpan<RelicModel>(relics);
      int num1 = 0;
      span[num1] = (RelicModel) ModelDb.Relic<MiniatureCannon>();
      int num2 = num1 + 1;
      span[num2] = (RelicModel) ModelDb.Relic<TungstenRod>();
      int num3 = num2 + 1;
      span[num3] = (RelicModel) ModelDb.Relic<WhiteStar>();
      return relics;
    }
  }

  public override string UnlockText => this.CreateRelicUnlockText(Relic4Epoch.Relics);

  public override EpochModel[] GetTimelineExpansion()
  {
    return new EpochModel[3]
    {
      EpochModel.Get(EpochModel.GetId<Event1Epoch>()),
      EpochModel.Get(EpochModel.GetId<Colorless5Epoch>()),
      EpochModel.Get(EpochModel.GetId<Relic5Epoch>())
    };
  }

  public override void QueueUnlocks()
  {
    NTimelineScreen.Instance.QueueRelicUnlock(Relic4Epoch.Relics);
    EpochModel.QueueTimelineExpansion(this.GetTimelineExpansion());
  }
}
