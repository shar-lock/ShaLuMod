// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.CardPools.StatusCardPool
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Models.Cards;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.CardPools;

public sealed class StatusCardPool : CardPoolModel
{
  public override string Title => "status";

  public override string EnergyColorName => "colorless";

  public override string CardFrameMaterialPath => "card_frame_colorless";

  public override Color DeckEntryCardColor => Colors.White;

  public override bool IsColorless => false;

  protected override CardModel[] GenerateAllCards()
  {
    return new CardModel[12]
    {
      (CardModel) ModelDb.Card<Beckon>(),
      (CardModel) ModelDb.Card<Burn>(),
      (CardModel) ModelDb.Card<Dazed>(),
      (CardModel) ModelDb.Card<Debris>(),
      (CardModel) ModelDb.Card<FranticEscape>(),
      (CardModel) ModelDb.Card<Infection>(),
      (CardModel) ModelDb.Card<Wither>(),
      (CardModel) ModelDb.Card<Slimed>(),
      (CardModel) ModelDb.Card<Soot>(),
      (CardModel) ModelDb.Card<Toxic>(),
      (CardModel) ModelDb.Card<Void>(),
      (CardModel) ModelDb.Card<Wound>()
    };
  }
}
