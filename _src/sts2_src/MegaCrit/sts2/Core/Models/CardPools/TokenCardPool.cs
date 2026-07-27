// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.CardPools.TokenCardPool
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Models.Cards;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.CardPools;

public sealed class TokenCardPool : CardPoolModel
{
  public override string Title => "token";

  public override string EnergyColorName => "colorless";

  public override string CardFrameMaterialPath => "card_frame_colorless";

  public override Color DeckEntryCardColor => Colors.White;

  public override bool IsColorless => true;

  protected override CardModel[] GenerateAllCards()
  {
    return new CardModel[14]
    {
      (CardModel) ModelDb.Card<Disintegration>(),
      (CardModel) ModelDb.Card<Fuel>(),
      (CardModel) ModelDb.Card<GiantRock>(),
      (CardModel) ModelDb.Card<Luminesce>(),
      (CardModel) ModelDb.Card<MindRot>(),
      (CardModel) ModelDb.Card<MinionDiveBomb>(),
      (CardModel) ModelDb.Card<MinionSacrifice>(),
      (CardModel) ModelDb.Card<MinionStrike>(),
      (CardModel) ModelDb.Card<Shiv>(),
      (CardModel) ModelDb.Card<Sloth>(),
      (CardModel) ModelDb.Card<Soul>(),
      (CardModel) ModelDb.Card<SovereignBlade>(),
      (CardModel) ModelDb.Card<SweepingGaze>(),
      (CardModel) ModelDb.Card<WasteAway>()
    };
  }
}
