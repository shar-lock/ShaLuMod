// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.GnarledHammer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Enchantments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class GnarledHammer : RelicModel
{
  private const string _sharpAmountKey = "SharpAmount";

  public override RelicRarity Rarity => RelicRarity.Shop;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new CardsVar(3),
        new DynamicVar("SharpAmount", 3M)
      });
    }
  }

  public override bool HasUponPickupEffect => true;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get => HoverTipFactory.FromEnchantment<Sharp>(this.DynamicVars["SharpAmount"].IntValue);
  }

  public override async Task AfterObtained()
  {
    CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 0, this.DynamicVars.Cards.IntValue)
    {
      Cancelable = false,
      RequireManualConfirmation = true
    };
    Sharp canonicalEnchantment = ModelDb.Enchantment<Sharp>();
    foreach (CardModel card in await CardSelectCmd.FromDeckForEnchantment(this.Owner, (EnchantmentModel) canonicalEnchantment, this.DynamicVars["SharpAmount"].IntValue, prefs))
    {
      CardCmd.Enchant(canonicalEnchantment.ToMutable(), card, (Decimal) this.DynamicVars["SharpAmount"].IntValue);
      CardCmd.Preview(card);
    }
    canonicalEnchantment = (Sharp) null;
  }
}
