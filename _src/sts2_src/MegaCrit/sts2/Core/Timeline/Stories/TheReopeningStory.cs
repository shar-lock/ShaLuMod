// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Timeline.Stories.TheReopeningStory
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Timeline.Epochs;

#nullable enable
namespace MegaCrit.Sts2.Core.Timeline.Stories;

public sealed class TheReopeningStory : StoryModel
{
  protected override string Id => "REOPENING";

  public override EpochModel[] Epochs
  {
    get
    {
      return new EpochModel[6]
      {
        EpochModel.Get<Potion2Epoch>(),
        EpochModel.Get<Relic5Epoch>(),
        EpochModel.Get<Relic3Epoch>(),
        EpochModel.Get<Potion1Epoch>(),
        EpochModel.Get<NeowEpoch>(),
        EpochModel.Get<Event2Epoch>()
      };
    }
  }
}
