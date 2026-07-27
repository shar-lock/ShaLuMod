// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Timeline.Stories.NecrobinderStory
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Timeline.Epochs;

#nullable enable
namespace MegaCrit.Sts2.Core.Timeline.Stories;

public sealed class NecrobinderStory : StoryModel
{
  protected override string Id => "NECROBINDER";

  public override EpochModel[] Epochs
  {
    get
    {
      return new EpochModel[7]
      {
        EpochModel.Get<Necrobinder2Epoch>(),
        EpochModel.Get<Necrobinder3Epoch>(),
        EpochModel.Get<Necrobinder4Epoch>(),
        EpochModel.Get<Necrobinder5Epoch>(),
        EpochModel.Get<Necrobinder6Epoch>(),
        EpochModel.Get<Necrobinder1Epoch>(),
        EpochModel.Get<Necrobinder7Epoch>()
      };
    }
  }
}
