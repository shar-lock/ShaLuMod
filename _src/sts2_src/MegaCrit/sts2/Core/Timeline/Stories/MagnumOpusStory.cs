// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Timeline.Stories.MagnumOpusStory
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Timeline.Epochs;

#nullable enable
namespace MegaCrit.Sts2.Core.Timeline.Stories;

public sealed class MagnumOpusStory : StoryModel
{
  protected override string Id => "MAGNUM_OPUS";

  public override EpochModel[] Epochs
  {
    get
    {
      return new EpochModel[9]
      {
        EpochModel.Get<Colorless1Epoch>(),
        EpochModel.Get<Relic1Epoch>(),
        EpochModel.Get<Colorless2Epoch>(),
        EpochModel.Get<CustomAndSeedsEpoch>(),
        EpochModel.Get<Relic2Epoch>(),
        EpochModel.Get<Colorless3Epoch>(),
        EpochModel.Get<Colorless4Epoch>(),
        EpochModel.Get<Relic4Epoch>(),
        EpochModel.Get<Event3Epoch>()
      };
    }
  }
}
