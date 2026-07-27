// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Mocks.MockQuestCard
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards.Mocks;

public sealed class MockQuestCard : MockCardModel
{
  protected override int CanonicalEnergyCost => -1;

  public override CardType Type => CardType.Quest;

  public override CardRarity Rarity => CardRarity.Quest;

  public override TargetType TargetType => TargetType.None;

  public override int MaxUpgradeLevel => 0;

  public override IEnumerable<CardKeyword> CanonicalKeywords
  {
    get
    {
      return (IEnumerable<CardKeyword>) new \u003C\u003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Unplayable);
    }
  }

  public override MockCardModel MockBlock(int block) => throw new NotImplementedException();

  protected override int GetBaseBlock() => 0;
}
