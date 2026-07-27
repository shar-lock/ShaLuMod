// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Timeline.Epochs.Necrobinder2Epoch
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#nullable enable
namespace MegaCrit.Sts2.Core.Timeline.Epochs;

public class Necrobinder2Epoch : EpochModel
{
  public override string Id => "NECROBINDER2_EPOCH";

  public override EpochEra Era => EpochEra.Invitation0;

  public override int EraPosition => 2;

  public override string StoryId => "Necrobinder";

  public static List<CardModel> Cards
  {
    get
    {
      int capacity = 3;
      List<CardModel> cards = new List<CardModel>(capacity);
      CollectionsMarshal.SetCount<CardModel>(cards, capacity);
      Span<CardModel> span = CollectionsMarshal.AsSpan<CardModel>(cards);
      int num1 = 0;
      span[num1] = (CardModel) ModelDb.Card<Scourge>();
      int num2 = num1 + 1;
      span[num2] = (CardModel) ModelDb.Card<Oblivion>();
      int num3 = num2 + 1;
      span[num3] = (CardModel) ModelDb.Card<Countdown>();
      return cards;
    }
  }

  public override string UnlockText => this.CreateCardUnlockText(Necrobinder2Epoch.Cards);

  public override void QueueUnlocks()
  {
    NTimelineScreen.Instance.QueueCardUnlock((IReadOnlyList<CardModel>) Necrobinder2Epoch.Cards);
  }
}
