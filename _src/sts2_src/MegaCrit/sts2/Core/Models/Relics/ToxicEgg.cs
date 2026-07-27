// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.ToxicEgg
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class ToxicEgg : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Rare;

  public override bool IsAllowed(IRunState runState)
  {
    return RelicModel.IsBeforeAct3TreasureChest(runState);
  }

  public override bool TryModifyCardRewardOptionsLate(
    Player player,
    List<CardCreationResult> cardRewards,
    CardCreationOptions options)
  {
    if (player != this.Owner || options.Flags.HasFlag((Enum) CardCreationFlags.NoHookUpgrades))
      return false;
    EggRelicHelper.UpgradeValidCards(cardRewards, CardType.Skill, (RelicModel) this);
    return true;
  }

  public override void ModifyMerchantCardCreationResults(
    Player player,
    List<CardCreationResult> cards)
  {
    if (player != this.Owner)
      return;
    EggRelicHelper.UpgradeValidCards(cards, CardType.Skill, (RelicModel) this);
  }

  public override bool TryModifyCardBeingAddedToDeck(CardModel card, out CardModel? newCard)
  {
    newCard = (CardModel) null;
    if (card.Owner != this.Owner || card.Type != CardType.Skill || !card.IsUpgradable)
      return false;
    newCard = this.Owner.RunState.CloneCard(card);
    CardCmd.Upgrade(newCard, CardPreviewStyle.None);
    return true;
  }
}
