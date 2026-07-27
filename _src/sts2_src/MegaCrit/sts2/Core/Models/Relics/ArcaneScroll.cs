// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.ArcaneScroll
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class ArcaneScroll : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Ancient;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new CardsVar(1));
    }
  }

  public override async Task AfterObtained()
  {
    // ISSUE: object of a compiler-generated type is created
    List<CardModel> list = CardFactory.CreateForReward(this.Owner, this.DynamicVars.Cards.IntValue, new CardCreationOptions((IEnumerable<CardPoolModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardPoolModel>(this.Owner.Character.CardPool), CardCreationSource.Other, CardRarityOddsType.Uniform, (Func<CardModel, bool>) (c => c.Rarity == CardRarity.Rare)).WithFlags(CardCreationFlags.NoUpgradeRoll)).Select<CardCreationResult, CardModel>((Func<CardCreationResult, CardModel>) (r => r.Card)).ToList<CardModel>();
    if (list.Count <= 0)
      return;
    CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(list[0], PileType.Deck));
  }
}
