// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Timeline.Epochs.Silent1Epoch
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using MegaCrit.Sts2.Core.Saves;

#nullable enable
namespace MegaCrit.Sts2.Core.Timeline.Epochs;

public class Silent1Epoch : EpochModel
{
  public override string Id => "SILENT1_EPOCH";

  public override EpochEra Era => EpochEra.Invitation0;

  public override int EraPosition => 3;

  public override string StoryId => "Silent";

  public override EpochModel[] GetTimelineExpansion()
  {
    return new EpochModel[7]
    {
      EpochModel.Get(EpochModel.GetId<Regent1Epoch>()),
      EpochModel.Get(EpochModel.GetId<Silent2Epoch>()),
      EpochModel.Get(EpochModel.GetId<Silent3Epoch>()),
      EpochModel.Get(EpochModel.GetId<Silent4Epoch>()),
      EpochModel.Get(EpochModel.GetId<Silent5Epoch>()),
      EpochModel.Get(EpochModel.GetId<Silent6Epoch>()),
      EpochModel.Get(EpochModel.GetId<Silent7Epoch>())
    };
  }

  public override void QueueUnlocks()
  {
    NTimelineScreen.Instance.QueueCharacterUnlock<Silent>((EpochModel) this);
    SaveManager.Instance.Progress.PendingCharacterUnlock = ModelDb.Character<Silent>().Id;
    EpochModel.QueueTimelineExpansion(this.GetTimelineExpansion());
  }
}
