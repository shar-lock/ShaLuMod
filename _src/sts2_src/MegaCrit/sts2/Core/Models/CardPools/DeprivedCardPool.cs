// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.CardPools.DeprivedCardPool
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models.Cards.Mocks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.CardPools;

public sealed class DeprivedCardPool : CardPoolModel
{
  public override bool IsMock => true;

  public override string Title => "test";

  public override string EnergyColorName => "colorless";

  public override string CardFrameMaterialPath => "card_frame_colorless";

  public override Color DeckEntryCardColor => Colors.White;

  public override bool IsColorless => false;

  protected override CardModel[] GenerateAllCards()
  {
    return new CardModel[12]
    {
      (CardModel) DeprivedCardPool.MockCard<MockAttackCard>(CardRarity.Common),
      (CardModel) DeprivedCardPool.MockCard<MockAttackCard>(CardRarity.Uncommon),
      (CardModel) DeprivedCardPool.MockCard<MockAttackCard>(CardRarity.Rare),
      (CardModel) DeprivedCardPool.MockCard<MockPowerCard>(CardRarity.Common),
      (CardModel) DeprivedCardPool.MockCard<MockPowerCard>(CardRarity.Uncommon),
      (CardModel) DeprivedCardPool.MockCard<MockPowerCard>(CardRarity.Rare),
      (CardModel) DeprivedCardPool.MockCard<MockSkillCard>(CardRarity.Common),
      (CardModel) DeprivedCardPool.MockCard<MockSkillCard>(CardRarity.Uncommon),
      (CardModel) DeprivedCardPool.MockCard<MockSkillCard>(CardRarity.Rare),
      (CardModel) DeprivedCardPool.MockCard<MockQuestCard>(CardRarity.Quest),
      (CardModel) DeprivedCardPool.MockCard<MockCurseCard>(CardRarity.Curse),
      (CardModel) DeprivedCardPool.MockCard<MockStatusCard>(CardRarity.Status)
    };
  }

  private static MockCardModel MockCard<T>(CardRarity rarity) where T : MockCardModel
  {
    return ((MockCardModel) ModelDb.Card<T>().ToMutable()).MockRarity(rarity).MockCanonical();
  }
}
