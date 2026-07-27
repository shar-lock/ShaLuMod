// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Timeline.Stories.RegentStory
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Timeline.Epochs;

#nullable enable
namespace MegaCrit.Sts2.Core.Timeline.Stories;

public sealed class RegentStory : StoryModel
{
  protected override string Id => "REGENT";

  public override EpochModel[] Epochs
  {
    get
    {
      return new EpochModel[7]
      {
        EpochModel.Get<Regent6Epoch>(),
        EpochModel.Get<Regent1Epoch>(),
        EpochModel.Get<Regent5Epoch>(),
        EpochModel.Get<Regent2Epoch>(),
        EpochModel.Get<Regent3Epoch>(),
        EpochModel.Get<Regent4Epoch>(),
        EpochModel.Get<Regent7Epoch>()
      };
    }
  }
}
