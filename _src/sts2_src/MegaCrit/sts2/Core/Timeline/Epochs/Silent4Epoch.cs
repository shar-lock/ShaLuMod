// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Timeline.Epochs.Silent4Epoch
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#nullable enable
namespace MegaCrit.Sts2.Core.Timeline.Epochs;

public class Silent4Epoch : EpochModel
{
  public override string Id => "SILENT4_EPOCH";

  public override EpochEra Era => EpochEra.Blight1;

  public override int EraPosition => 3;

  public override string StoryId => "Silent";

  public static List<PotionModel> Potions
  {
    get
    {
      int capacity = 3;
      List<PotionModel> potions = new List<PotionModel>(capacity);
      CollectionsMarshal.SetCount<PotionModel>(potions, capacity);
      Span<PotionModel> span = CollectionsMarshal.AsSpan<PotionModel>(potions);
      int num1 = 0;
      span[num1] = (PotionModel) ModelDb.Potion<PoisonPotion>();
      int num2 = num1 + 1;
      span[num2] = (PotionModel) ModelDb.Potion<GhostInAJar>();
      int num3 = num2 + 1;
      span[num3] = (PotionModel) ModelDb.Potion<CunningPotion>();
      return potions;
    }
  }

  public override string UnlockText => this.CreatePotionUnlockText(Silent4Epoch.Potions);

  public override void QueueUnlocks()
  {
    NTimelineScreen.Instance.QueuePotionUnlock(Silent4Epoch.Potions);
    NTimelineScreen.Instance.QueueMiscUnlock(new LocString("epochs", this.Id + ".unlock").GetFormattedText() ?? "");
  }
}
