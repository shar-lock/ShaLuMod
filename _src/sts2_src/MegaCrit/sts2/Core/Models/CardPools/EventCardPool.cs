// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.CardPools.EventCardPool
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Models.Cards;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.CardPools;

public sealed class EventCardPool : CardPoolModel
{
  public override string Title => "event";

  public override string EnergyColorName => "colorless";

  public override string CardFrameMaterialPath => "card_frame_colorless";

  public override Color DeckEntryCardColor => new Color("A3A3A3FF");

  public override bool IsColorless => true;

  protected override CardModel[] GenerateAllCards()
  {
    return new CardModel[28]
    {
      (CardModel) ModelDb.Card<Abundance>(),
      (CardModel) ModelDb.Card<Apotheosis>(),
      (CardModel) ModelDb.Card<Apparition>(),
      (CardModel) ModelDb.Card<BrightestFlame>(),
      (CardModel) ModelDb.Card<ByrdSwoop>(),
      (CardModel) ModelDb.Card<Caltrops>(),
      (CardModel) ModelDb.Card<Clash>(),
      (CardModel) ModelDb.Card<Distraction>(),
      (CardModel) ModelDb.Card<DualWield>(),
      (CardModel) ModelDb.Card<Enlightenment>(),
      (CardModel) ModelDb.Card<Entrench>(),
      (CardModel) ModelDb.Card<Exterminate>(),
      (CardModel) ModelDb.Card<FeedingFrenzy>(),
      (CardModel) ModelDb.Card<HelloWorld>(),
      (CardModel) ModelDb.Card<MadScience>(),
      (CardModel) ModelDb.Card<Maul>(),
      (CardModel) ModelDb.Card<Metamorphosis>(),
      (CardModel) ModelDb.Card<NeowsFury>(),
      (CardModel) ModelDb.Card<Outmaneuver>(),
      (CardModel) ModelDb.Card<Peck>(),
      (CardModel) ModelDb.Card<Rebound>(),
      (CardModel) ModelDb.Card<Relax>(),
      (CardModel) ModelDb.Card<RipAndTear>(),
      (CardModel) ModelDb.Card<Squash>(),
      (CardModel) ModelDb.Card<Stack>(),
      (CardModel) ModelDb.Card<ToricToughness>(),
      (CardModel) ModelDb.Card<Wish>(),
      (CardModel) ModelDb.Card<Whistle>()
    };
  }
}
