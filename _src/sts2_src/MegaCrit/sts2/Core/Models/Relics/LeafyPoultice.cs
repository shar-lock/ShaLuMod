// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.LeafyPoultice
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class LeafyPoultice : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Ancient;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new MaxHpVar(12M));
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Transform));
    }
  }

  public override async Task AfterObtained()
  {
    await CreatureCmd.LoseMaxHp((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Owner.Creature, this.DynamicVars.MaxHp.BaseValue, false);
    List<CardModel> list = PileType.Deck.GetPile(this.Owner).Cards.Where<CardModel>((Func<CardModel, bool>) (c => c.Rarity == CardRarity.Basic)).ToList<CardModel>();
    CardModel original1 = list.FirstOrDefault<CardModel>((Func<CardModel, bool>) (c => c.Tags.Contains<CardTag>(CardTag.Strike)));
    CardModel original2 = list.FirstOrDefault<CardModel>((Func<CardModel, bool>) (c => c.Tags.Contains<CardTag>(CardTag.Defend)));
    List<CardTransformation> transformations = new List<CardTransformation>();
    if (original1 != null)
      transformations.Add(new CardTransformation(original1));
    if (original2 != null)
      transformations.Add(new CardTransformation(original2));
    IEnumerable<CardPileAddResult> cardPileAddResults = await CardCmd.Transform((IEnumerable<CardTransformation>) transformations, this.Owner.PlayerRng.Transformations);
  }
}
