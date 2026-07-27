// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Timeline.Stories.SilentStory
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Timeline.Epochs;

#nullable enable
namespace MegaCrit.Sts2.Core.Timeline.Stories;

public sealed class SilentStory : StoryModel
{
  protected override string Id => "SILENT";

  public override EpochModel[] Epochs
  {
    get
    {
      return new EpochModel[7]
      {
        EpochModel.Get<Silent6Epoch>(),
        EpochModel.Get<Silent4Epoch>(),
        EpochModel.Get<Silent5Epoch>(),
        EpochModel.Get<Silent2Epoch>(),
        EpochModel.Get<Silent3Epoch>(),
        EpochModel.Get<Silent1Epoch>(),
        EpochModel.Get<Silent7Epoch>()
      };
    }
  }
}
