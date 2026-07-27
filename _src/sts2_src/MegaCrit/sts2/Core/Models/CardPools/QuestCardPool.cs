// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.CardPools.QuestCardPool
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Models.Cards;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.CardPools;

public sealed class QuestCardPool : CardPoolModel
{
  public override string Title => "quest";

  public override string EnergyColorName => "colorless";

  public override string CardFrameMaterialPath => "card_frame_quest";

  public override Color DeckEntryCardColor => new Color("24476A");

  public override Color EnergyOutlineColor => new Color("431E14");

  public override bool IsColorless => false;

  protected override CardModel[] GenerateAllCards()
  {
    return new CardModel[4]
    {
      (CardModel) ModelDb.Card<ByrdonisEgg>(),
      (CardModel) ModelDb.Card<Dowsing>(),
      (CardModel) ModelDb.Card<LanternKey>(),
      (CardModel) ModelDb.Card<SpoilsMap>()
    };
  }
}
