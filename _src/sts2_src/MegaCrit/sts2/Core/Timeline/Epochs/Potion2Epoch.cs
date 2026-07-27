// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Timeline.Epochs.Potion2Epoch
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#nullable enable
namespace MegaCrit.Sts2.Core.Timeline.Epochs;

public class Potion2Epoch : EpochModel
{
  public override string Id => "POTION2_EPOCH";

  public override EpochEra Era => EpochEra.Flourish0;

  public override int EraPosition => 0;

  public override string StoryId => "Reopening";

  public static List<PotionModel> Potions
  {
    get
    {
      int capacity = 3;
      List<PotionModel> potions = new List<PotionModel>(capacity);
      CollectionsMarshal.SetCount<PotionModel>(potions, capacity);
      Span<PotionModel> span = CollectionsMarshal.AsSpan<PotionModel>(potions);
      int num1 = 0;
      span[num1] = (PotionModel) ModelDb.Potion<PowderedDemise>();
      int num2 = num1 + 1;
      span[num2] = (PotionModel) ModelDb.Potion<ShipInABottle>();
      int num3 = num2 + 1;
      span[num3] = (PotionModel) ModelDb.Potion<TouchOfInsanity>();
      return potions;
    }
  }

  public override string UnlockText => this.CreatePotionUnlockText(Potion2Epoch.Potions);

  public override EpochModel[] GetTimelineExpansion()
  {
    return new EpochModel[3]
    {
      EpochModel.Get(EpochModel.GetId<Act2BEpoch>()),
      EpochModel.Get(EpochModel.GetId<Colorless3Epoch>()),
      EpochModel.Get(EpochModel.GetId<Relic3Epoch>())
    };
  }

  public override void QueueUnlocks()
  {
    NTimelineScreen.Instance.QueuePotionUnlock(Potion2Epoch.Potions);
    EpochModel.QueueTimelineExpansion(this.GetTimelineExpansion());
  }
}
