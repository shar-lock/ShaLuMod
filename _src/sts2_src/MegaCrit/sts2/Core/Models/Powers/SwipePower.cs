// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.SwipePower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class SwipePower : PowerModel
{
  private CardModel? _stolenCard;

  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Single;

  public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

  public CardModel? StolenCard
  {
    get => this._stolenCard;
    set
    {
      this.AssertMutable();
      this._stolenCard = value;
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return this.StolenCard == null ? (IEnumerable<IHoverTip>) Array.Empty<IHoverTip>() : (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard(this.StolenCard));
    }
  }

  public override Task BeforeDeath(Creature target)
  {
    if (this.Owner != target || this.StolenCard?.DeckVersion == null)
      return Task.CompletedTask;
    IRunState runState = this.CombatState.RunState;
    runState.AddCard(this.StolenCard.DeckVersion, this.Target.Player);
    SpecialCardReward specialCardReward = new SpecialCardReward(this.StolenCard.DeckVersion, this.Target.Player);
    specialCardReward.SetCustomDescriptionEncounterSource(ModelDb.Encounter<ThievingHopperWeak>().Id);
    ((CombatRoom) runState.CurrentRoom).AddExtraReward(this.Target.Player, (Reward) specialCardReward);
    runState.CurrentMapPointHistoryEntry?.GetEntry(this.Target.Player.NetId).MarkLootReturned();
    return Task.CompletedTask;
  }

  public async Task Steal(CardModel card)
  {
    this.Target = card.Owner.Creature;
    this.StolenCard = card;
    if (card.DeckVersion == null)
      return;
    await CardPileCmd.RemoveFromDeck(card.DeckVersion, false);
    card.Owner.RunState.CurrentMapPointHistoryEntry?.GetEntry(card.Owner.NetId).MarkLootStolen();
  }
}
