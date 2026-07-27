// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.SilkenTress
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class SilkenTress : RelicModel
{
  private bool _isUsed;

  public override bool IsUsedUp => this.IsUsed;

  public override RelicRarity Rarity => RelicRarity.Ancient;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get => HoverTipFactory.FromEnchantment<Glam>();
  }

  [SavedProperty]
  private bool IsUsed
  {
    get => this._isUsed;
    set
    {
      this.AssertMutable();
      this._isUsed = value;
      if (!this.IsUsedUp)
        return;
      this.Status = RelicStatus.Disabled;
    }
  }

  public override async Task AfterObtained()
  {
    await PlayerCmd.LoseGold((Decimal) this.Owner.Gold, this.Owner);
  }

  public override bool TryModifyCardRewardOptionsLate(
    Player player,
    List<CardCreationResult> cardRewards,
    CardCreationOptions options)
  {
    if (player != this.Owner || !options.Flags.HasFlag((Enum) CardCreationFlags.IsCardReward) || this.IsUsed)
      return false;
    Glam glam = ModelDb.Enchantment<Glam>();
    foreach (CardCreationResult cardReward in cardRewards)
    {
      CardModel card1 = cardReward.Card;
      if (glam.CanEnchant(card1))
      {
        CardModel card2 = this.Owner.RunState.CloneCard(card1);
        CardCmd.Enchant<Glam>(card2, 1M);
        cardReward.ModifyCard(card2, (RelicModel) this);
      }
    }
    return true;
  }

  public override Task AfterModifyingCardRewardOptions()
  {
    if (this.IsUsed)
      return Task.CompletedTask;
    this.IsUsed = true;
    return Task.CompletedTask;
  }
}
