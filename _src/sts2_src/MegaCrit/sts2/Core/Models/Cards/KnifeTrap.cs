// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.KnifeTrap
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class KnifeTrap : CardModel
{
  private const string _calculatedShivsKey = "CalculatedShivs";

  public KnifeTrap()
    : base(2, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
  {
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Shiv>(this.IsUpgraded));
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[3]
      {
        (DynamicVar) new CalculationBaseVar(0M),
        (DynamicVar) new CalculationExtraVar(1M),
        (DynamicVar) new CalculatedVar("CalculatedShivs").WithMultiplier((Func<CardModel, Creature, Decimal>) ((card, _) => (Decimal) PileType.Exhaust.GetPile(card.Owner).Cards.Count<CardModel>((Func<CardModel, bool>) (c => c.Tags.Contains<CardTag>(CardTag.Shiv)))))
      });
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    IEnumerable<CardModel> list = (IEnumerable<CardModel>) PileType.Exhaust.GetPile(this.Owner).Cards.Where<CardModel>((Func<CardModel, bool>) (c => c.Tags.Contains<CardTag>(CardTag.Shiv))).ToList<CardModel>();
    bool flag = true;
    foreach (CardModel card in list)
    {
      if (this.IsUpgraded)
        CardCmd.Upgrade(card, CardPreviewStyle.None);
      await CardCmd.AutoPlay(choiceContext, card, cardPlay.Target, skipCardPileVisuals: !flag);
      flag = false;
    }
  }
}
