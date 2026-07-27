// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.LavaLamp
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class LavaLamp : RelicModel
{
  private bool _tookDamageThisCombat;

  public override RelicRarity Rarity => RelicRarity.Shop;

  [SavedProperty]
  public bool TookDamageThisCombat
  {
    get => this._tookDamageThisCombat;
    set
    {
      this.AssertMutable();
      this._tookDamageThisCombat = value;
    }
  }

  public override Task AfterRoomEntered(AbstractRoom room)
  {
    this.TookDamageThisCombat = false;
    return Task.CompletedTask;
  }

  public override Task AfterDamageReceived(
    PlayerChoiceContext choiceContext,
    Creature target,
    DamageResult result,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource)
  {
    if (!(this.Owner.RunState.CurrentRoom is CombatRoom) || target != this.Owner.Creature || result.UnblockedDamage <= 0 || props.HasFlag((Enum) ValueProp.Unblockable))
      return Task.CompletedTask;
    this.TookDamageThisCombat = true;
    return Task.CompletedTask;
  }

  public override bool TryModifyCardRewardOptionsLate(
    Player player,
    List<CardCreationResult> cardRewards,
    CardCreationOptions options)
  {
    if (!(this.Owner.RunState.CurrentRoom is CombatRoom) || player != this.Owner || this.TookDamageThisCombat)
      return false;
    foreach (CardCreationResult cardReward in cardRewards)
    {
      CardModel card1 = cardReward.Card;
      if (card1.IsUpgradable)
      {
        CardModel card2 = this.Owner.RunState.CloneCard(card1);
        CardCmd.Upgrade(card2);
        cardReward.ModifyCard(card2, (RelicModel) this);
      }
    }
    return true;
  }
}
