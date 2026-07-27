// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.WingCharm
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class WingCharm : RelicModel
{
  private const string _swiftAmountKey = "SwiftAmount";

  public override RelicRarity Rarity => RelicRarity.Shop;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("SwiftAmount", 1M));
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get => HoverTipFactory.FromEnchantment<Swift>(this.DynamicVars["SwiftAmount"].IntValue);
  }

  public override bool TryModifyCardRewardOptionsLate(
    Player player,
    List<CardCreationResult> cardRewards,
    CardCreationOptions options)
  {
    if (player != this.Owner)
      return false;
    Swift canonicalSwift = ModelDb.Enchantment<Swift>();
    List<CardCreationResult> list = cardRewards.Where<CardCreationResult>((Func<CardCreationResult, bool>) (r => canonicalSwift.CanEnchant(r.Card))).ToList<CardCreationResult>();
    if (list.Count == 0)
      return false;
    CardCreationResult cardCreationResult = this.Owner.RunState.Rng.Niche.NextItem<CardCreationResult>((IEnumerable<CardCreationResult>) list);
    if (cardCreationResult == null)
      return false;
    CardModel card = this.Owner.RunState.CloneCard(cardCreationResult.Card);
    CardCmd.Enchant<Swift>(card, this.DynamicVars["SwiftAmount"].BaseValue);
    cardCreationResult.ModifyCard(card, (RelicModel) this);
    return true;
  }
}
