// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.CardPools.MockCardPool
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.CardPools;

public sealed class MockCardPool : CardPoolModel
{
  private List<CardModel>? _customCards;

  public override bool IsMock => true;

  public override string Title => "test";

  public override string EnergyColorName => "colorless";

  public override string CardFrameMaterialPath => "card_frame_colorless";

  public override Color DeckEntryCardColor => Colors.White;

  public override bool IsColorless => false;

  protected override CardModel[] GenerateAllCards()
  {
    return this._customCards?.ToArray() ?? Array.Empty<CardModel>();
  }

  protected override void DeepCloneFields()
  {
    base.DeepCloneFields();
    this._customCards = new List<CardModel>();
  }

  public void Add(CardModel card)
  {
    this.AssertMutable();
    card.AssertCanonical();
    this._customCards.Add(card);
    this.InvalidateCardCache();
  }

  public static MockCardPool Create(params CardModel[] cards)
  {
    MockCardPool mutable = (MockCardPool) ModelDb.CardPool<MockCardPool>().ToMutable();
    foreach (CardModel card in cards)
      mutable.Add(card);
    return mutable;
  }
}
