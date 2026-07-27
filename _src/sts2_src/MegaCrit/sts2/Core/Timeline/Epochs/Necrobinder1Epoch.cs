// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Timeline.Epochs.Necrobinder1Epoch
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using MegaCrit.Sts2.Core.Saves;

#nullable enable
namespace MegaCrit.Sts2.Core.Timeline.Epochs;

public class Necrobinder1Epoch : EpochModel
{
  public override string Id => "NECROBINDER1_EPOCH";

  public override EpochEra Era => EpochEra.Invitation5;

  public override int EraPosition => 0;

  public override string StoryId => "Necrobinder";

  public override EpochModel[] GetTimelineExpansion()
  {
    return new EpochModel[7]
    {
      EpochModel.Get(EpochModel.GetId<Necrobinder2Epoch>()),
      EpochModel.Get(EpochModel.GetId<Necrobinder3Epoch>()),
      EpochModel.Get(EpochModel.GetId<Necrobinder4Epoch>()),
      EpochModel.Get(EpochModel.GetId<Necrobinder5Epoch>()),
      EpochModel.Get(EpochModel.GetId<Necrobinder6Epoch>()),
      EpochModel.Get(EpochModel.GetId<Necrobinder7Epoch>()),
      EpochModel.Get(EpochModel.GetId<Defect1Epoch>())
    };
  }

  public override void QueueUnlocks()
  {
    NTimelineScreen.Instance.QueueCharacterUnlock<Necrobinder>((EpochModel) this);
    SaveManager.Instance.Progress.PendingCharacterUnlock = ModelDb.Character<Necrobinder>().Id;
    EpochModel.QueueTimelineExpansion(this.GetTimelineExpansion());
  }
}
