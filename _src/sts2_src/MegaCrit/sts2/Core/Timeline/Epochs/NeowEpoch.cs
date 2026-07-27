// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Timeline.Epochs.NeowEpoch
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using MegaCrit.Sts2.Core.Saves;

#nullable enable
namespace MegaCrit.Sts2.Core.Timeline.Epochs;

public class NeowEpoch : EpochModel
{
  public override string Id => "NEOW_EPOCH";

  public override EpochEra Era => EpochEra.Invitation0;

  public override int EraPosition => 1;

  public override string StoryId => "Reopening";

  public override EpochModel[] GetTimelineExpansion()
  {
    return new EpochModel[12]
    {
      EpochModel.Get(EpochModel.GetId<Colorless1Epoch>()),
      EpochModel.Get(EpochModel.GetId<CustomAndSeedsEpoch>()),
      EpochModel.Get(EpochModel.GetId<DailyRunEpoch>()),
      EpochModel.Get(EpochModel.GetId<DarvEpoch>()),
      EpochModel.Get(EpochModel.GetId<Ironclad2Epoch>()),
      EpochModel.Get(EpochModel.GetId<Ironclad3Epoch>()),
      EpochModel.Get(EpochModel.GetId<Ironclad4Epoch>()),
      EpochModel.Get(EpochModel.GetId<Ironclad5Epoch>()),
      EpochModel.Get(EpochModel.GetId<Ironclad6Epoch>()),
      EpochModel.Get(EpochModel.GetId<Ironclad7Epoch>()),
      EpochModel.Get(EpochModel.GetId<OrobasEpoch>()),
      EpochModel.Get(EpochModel.GetId<Silent1Epoch>())
    };
  }

  public override void QueueUnlocks()
  {
    NTimelineScreen.Instance.QueueMiscUnlock(new LocString("epochs", this.Id + ".unlock").GetFormattedText() ?? "");
    SaveManager.Instance.ObtainEpochOverride(EpochModel.GetId<Silent1Epoch>(), EpochState.ObtainedNoSlot);
    EpochModel.QueueTimelineExpansion(this.GetTimelineExpansion());
  }
}
