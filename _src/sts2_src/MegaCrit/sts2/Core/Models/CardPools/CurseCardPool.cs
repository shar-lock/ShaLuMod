// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.CardPools.CurseCardPool
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Models.Cards;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.CardPools;

public sealed class CurseCardPool : CardPoolModel
{
  public override string Title => "curse";

  public override string EnergyColorName => "colorless";

  public override string CardFrameMaterialPath => "card_frame_curse";

  public override Color DeckEntryCardColor => new Color("585B61FF");

  public override bool IsColorless => false;

  protected override CardModel[] GenerateAllCards()
  {
    return new CardModel[18]
    {
      (CardModel) ModelDb.Card<AscendersBane>(),
      (CardModel) ModelDb.Card<BadLuck>(),
      (CardModel) ModelDb.Card<Clumsy>(),
      (CardModel) ModelDb.Card<CurseOfTheBell>(),
      (CardModel) ModelDb.Card<Debt>(),
      (CardModel) ModelDb.Card<Decay>(),
      (CardModel) ModelDb.Card<Doubt>(),
      (CardModel) ModelDb.Card<Enthralled>(),
      (CardModel) ModelDb.Card<Folly>(),
      (CardModel) ModelDb.Card<Greed>(),
      (CardModel) ModelDb.Card<Guilty>(),
      (CardModel) ModelDb.Card<Injury>(),
      (CardModel) ModelDb.Card<Normality>(),
      (CardModel) ModelDb.Card<PoorSleep>(),
      (CardModel) ModelDb.Card<Regret>(),
      (CardModel) ModelDb.Card<Shame>(),
      (CardModel) ModelDb.Card<SporeMind>(),
      (CardModel) ModelDb.Card<Writhe>()
    };
  }
}
