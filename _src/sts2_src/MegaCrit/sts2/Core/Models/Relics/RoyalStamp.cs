// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.RoyalStamp
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class RoyalStamp : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Shop;

  public override bool HasUponPickupEffect => true;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new CardsVar(1),
        (DynamicVar) new StringVar("Enchantment", ModelDb.Enchantment<RoyallyApproved>().Title.GetFormattedText())
      });
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get => HoverTipFactory.FromEnchantment<RoyallyApproved>();
  }

  public override async Task AfterObtained()
  {
    EnchantmentModel royalStamp = (EnchantmentModel) ModelDb.Enchantment<RoyallyApproved>();
    List<CardModel> list = PileType.Deck.GetPile(this.Owner).Cards.Where<CardModel>((Func<CardModel, bool>) (c => royalStamp.CanEnchant(c))).ToList<CardModel>();
    CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1);
    CardModel card = (await CardSelectCmd.FromDeckForEnchantment((IReadOnlyList<CardModel>) list.UnstableShuffle<CardModel>(this.Owner.RunState.Rng.Niche).ToList<CardModel>(), royalStamp, 1, prefs)).FirstOrDefault<CardModel>();
    if (card == null)
      return;
    CardCmd.Enchant<RoyallyApproved>(card, 1M);
    NCardEnchantVfx child = NCardEnchantVfx.Create(card);
    if (child == null)
      return;
    NRun instance = NRun.Instance;
    if (instance == null)
      return;
    ((Node) instance.GlobalUi.CardPreviewContainer).AddChildSafely((Node) child);
  }
}
