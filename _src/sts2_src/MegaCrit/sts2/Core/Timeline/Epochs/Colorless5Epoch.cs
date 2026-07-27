// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Timeline.Epochs.Colorless5Epoch
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

public class Colorless5Epoch : EpochModel
{
  public override string Id => "COLORLESS5_EPOCH";

  public override EpochEra Era => EpochEra.Blight1;

  public override int EraPosition => 2;

  public override string StoryId => "Tales_From_The_Spire";

  public static List<CardModel> Cards
  {
    get
    {
      int capacity = 3;
      List<CardModel> cards = new List<CardModel>(capacity);
      CollectionsMarshal.SetCount<CardModel>(cards, capacity);
      Span<CardModel> span = CollectionsMarshal.AsSpan<CardModel>(cards);
      int num1 = 0;
      span[num1] = (CardModel) ModelDb.Card<Splash>();
      int num2 = num1 + 1;
      span[num2] = (CardModel) ModelDb.Card<Anointed>();
      int num3 = num2 + 1;
      span[num3] = (CardModel) ModelDb.Card<Calamity>();
      return cards;
    }
  }

  public override string UnlockText => this.CreateCardUnlockText(Colorless5Epoch.Cards);

  public override void QueueUnlocks()
  {
    NTimelineScreen.Instance.QueueCardUnlock((IReadOnlyList<CardModel>) Colorless5Epoch.Cards);
  }
}
