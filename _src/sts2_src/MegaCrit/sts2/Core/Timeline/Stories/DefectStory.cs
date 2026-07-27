// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Timeline.Stories.DefectStory
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Timeline.Epochs;

#nullable enable
namespace MegaCrit.Sts2.Core.Timeline.Stories;

public sealed class DefectStory : StoryModel
{
  protected override string Id => "DEFECT";

  public override EpochModel[] Epochs
  {
    get
    {
      return new EpochModel[7]
      {
        EpochModel.Get<Defect6Epoch>(),
        EpochModel.Get<Defect3Epoch>(),
        EpochModel.Get<Defect4Epoch>(),
        EpochModel.Get<Defect5Epoch>(),
        EpochModel.Get<Defect1Epoch>(),
        EpochModel.Get<Defect2Epoch>(),
        EpochModel.Get<Defect7Epoch>()
      };
    }
  }
}
