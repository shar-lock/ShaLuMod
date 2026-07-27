// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.Claws
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class Claws : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Ancient;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new CardsVar(6));
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get => HoverTipFactory.FromCardWithCardHoverTips<Maul>();
  }

  public override async Task AfterObtained()
  {
    IEnumerable<CardPileAddResult> cardPileAddResults = await CardCmd.Transform((IEnumerable<CardTransformation>) (await CardSelectCmd.FromDeckForTransformation(this.Owner, new CardSelectorPrefs(this.SelectionScreenPrompt, 0, this.DynamicVars.Cards.IntValue)
    {
      Cancelable = false,
      RequireManualConfirmation = true
    }, (Func<CardModel, CardTransformation>) (c => new CardTransformation(c, this.CreateMaulFromOriginal(c, true))))).Select<CardModel, CardTransformation>((Func<CardModel, CardTransformation>) (original => new CardTransformation(original, this.CreateMaulFromOriginal(original, false)))).ToList<CardTransformation>(), this.Owner.PlayerRng.Transformations);
  }

  private CardModel CreateMaulFromOriginal(CardModel original, bool forPreview)
  {
    CardModel card = forPreview ? ModelDb.Card<Maul>().ToMutable() : (CardModel) this.Owner.RunState.CreateCard<Maul>(this.Owner);
    if (original.IsUpgraded && card.IsUpgradable)
    {
      if (forPreview)
        card.UpgradeInternal();
      else
        CardCmd.Upgrade(card);
    }
    if (original.Enchantment != null)
    {
      EnchantmentModel enchantment = (EnchantmentModel) original.Enchantment.MutableClone();
      if (enchantment.CanEnchant(card))
      {
        if (forPreview)
        {
          card.EnchantInternal(enchantment, (Decimal) enchantment.Amount);
          enchantment.ModifyCard();
        }
        else
          CardCmd.Enchant(enchantment, card, (Decimal) enchantment.Amount);
      }
    }
    return card;
  }
}
