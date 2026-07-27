// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Timeline.Epochs.CustomAndSeedsEpoch
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;

#nullable enable
namespace MegaCrit.Sts2.Core.Timeline.Epochs;

public class CustomAndSeedsEpoch : EpochModel
{
  public override string Id => "CUSTOM_AND_SEEDS_EPOCH";

  public override EpochEra Era => EpochEra.Seeds0;

  public override int EraPosition => 0;

  public override string StoryId => "Magnum_Opus";

  public override void QueueUnlocks()
  {
    NTimelineScreen.Instance.QueueMiscUnlock(new LocString("epochs", this.Id + ".unlock").GetFormattedText() ?? "");
  }
}
