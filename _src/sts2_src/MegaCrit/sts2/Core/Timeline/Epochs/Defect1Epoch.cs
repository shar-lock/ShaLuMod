// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Timeline.Epochs.Defect1Epoch
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using MegaCrit.Sts2.Core.Saves;

#nullable enable
namespace MegaCrit.Sts2.Core.Timeline.Epochs;

public class Defect1Epoch : EpochModel
{
  public override string Id => "DEFECT1_EPOCH";

  public override EpochEra Era => EpochEra.Invitation1;

  public override int EraPosition => 2;

  public override string StoryId => "Defect";

  public override EpochModel[] GetTimelineExpansion()
  {
    return new EpochModel[6]
    {
      EpochModel.Get(EpochModel.GetId<Defect2Epoch>()),
      EpochModel.Get(EpochModel.GetId<Defect3Epoch>()),
      EpochModel.Get(EpochModel.GetId<Defect4Epoch>()),
      EpochModel.Get(EpochModel.GetId<Defect5Epoch>()),
      EpochModel.Get(EpochModel.GetId<Defect6Epoch>()),
      EpochModel.Get(EpochModel.GetId<Defect7Epoch>())
    };
  }

  public override void QueueUnlocks()
  {
    NTimelineScreen.Instance.QueueCharacterUnlock<Defect>((EpochModel) this);
    SaveManager.Instance.Progress.PendingCharacterUnlock = ModelDb.Character<Defect>().Id;
    EpochModel.QueueTimelineExpansion(this.GetTimelineExpansion());
  }
}
