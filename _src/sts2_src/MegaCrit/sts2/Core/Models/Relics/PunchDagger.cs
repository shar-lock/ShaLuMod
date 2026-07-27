// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.PunchDagger
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

public sealed class PunchDagger : RelicModel
{
  private const string _momentumKey = "Momentum";

  public override RelicRarity Rarity => RelicRarity.Shop;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Momentum", 5M));
    }
  }

  public override bool HasUponPickupEffect => true;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get => HoverTipFactory.FromEnchantment<Momentum>(this.DynamicVars["Momentum"].IntValue);
  }

  public override async Task AfterObtained()
  {
    CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1);
    Momentum canonicalMomentum = ModelDb.Enchantment<Momentum>();
    foreach (CardModel card in await CardSelectCmd.FromDeckForEnchantment(this.Owner, (EnchantmentModel) canonicalMomentum, this.DynamicVars["Momentum"].IntValue, prefs))
    {
      CardCmd.Enchant(canonicalMomentum.ToMutable(), card, (Decimal) this.DynamicVars["Momentum"].IntValue);
      CardCmd.Preview(card);
    }
    canonicalMomentum = (Momentum) null;
  }
}
