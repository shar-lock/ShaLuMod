// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Timeline.Stories.IroncladStory
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Timeline.Epochs;

#nullable enable
namespace MegaCrit.Sts2.Core.Timeline.Stories;

public sealed class IroncladStory : StoryModel
{
  protected override string Id => "IRONCLAD";

  public override EpochModel[] Epochs
  {
    get
    {
      return new EpochModel[6]
      {
        EpochModel.Get<Ironclad4Epoch>(),
        EpochModel.Get<Ironclad3Epoch>(),
        EpochModel.Get<Ironclad2Epoch>(),
        EpochModel.Get<Ironclad5Epoch>(),
        EpochModel.Get<Ironclad6Epoch>(),
        EpochModel.Get<Ironclad7Epoch>()
      };
    }
  }
}
