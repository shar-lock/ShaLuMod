// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Timeline.EpochSlotData
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;

#nullable enable
namespace MegaCrit.Sts2.Core.Timeline;

public class EpochSlotData
{
  public EpochModel Model { get; }

  public EpochSlotState State { get; }

  public EpochEra Era { get; }

  public int EraPosition { get; }

  public EpochSlotData(string modelId, EpochSlotState state)
  {
    this.Model = EpochModel.Get(modelId);
    this.Era = this.Model.Era;
    this.EraPosition = this.Model.EraPosition;
    this.State = state;
  }

  public EpochSlotData(EpochModel model, EpochSlotState state)
  {
    this.Model = model;
    this.Era = this.Model.Era;
    this.EraPosition = this.Model.EraPosition;
    this.State = state;
  }
}
