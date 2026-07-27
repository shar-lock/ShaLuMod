// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.ChainsOfBindingPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Afflictions;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class ChainsOfBindingPower : PowerModel
{
  public override PowerType Type => PowerType.Debuff;

  public override PowerStackType StackType => PowerStackType.Counter;

  protected override object InitInternalData() => (object) new ChainsOfBindingPower.Data();

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get => HoverTipFactory.FromAffliction<Bound>();
  }

  public override async Task AfterCardDrawn(
    PlayerChoiceContext choiceContext,
    CardModel card,
    bool fromHandDraw)
  {
    if (card.Owner != this.Owner.Player || this.CombatState.CurrentSide != this.Owner.Side || !ModelDb.Affliction<Bound>().CanAfflict(card) || CombatManager.Instance.History.Entries.OfType<CardAfflictedEntry>().Count<CardAfflictedEntry>((Func<CardAfflictedEntry, bool>) (e => e.HappenedThisTurn(this.CombatState) && e.Actor == this.Owner && e.Affliction is Bound)) >= this.Amount)
      return;
    // ISSUE: object of a compiler-generated type is created
    IEnumerable<Bound> bounds = await CardCmd.AfflictAndPreview<Bound>((IEnumerable<CardModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardModel>(card), (Decimal) this.Amount, CardPreviewStyle.None);
  }

  public override Task BeforeCardPlayed(CardPlay cardPlay)
  {
    CardModel card = cardPlay.Card;
    if (card.IsDupe || card.Owner.Creature != this.Owner || !(card.Affliction is Bound))
      return Task.CompletedTask;
    this.GetInternalData<ChainsOfBindingPower.Data>().boundCardPlayed = true;
    return Task.CompletedTask;
  }

  public override bool ShouldPlay(CardModel card, AutoPlayType autoPlayType)
  {
    return card.Owner.Creature != this.Owner || !(card.Affliction is Bound) || !this.GetInternalData<ChainsOfBindingPower.Data>().boundCardPlayed;
  }

  public override Task BeforeSideTurnEnd(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    if (!participants.Contains<Creature>(this.Owner))
      return Task.CompletedTask;
    this.GetInternalData<ChainsOfBindingPower.Data>().boundCardPlayed = false;
    Player player = this.Owner.Player;
    object obj;
    if (player == null)
    {
      obj = (object) null;
    }
    else
    {
      PlayerCombatState playerCombatState = player.PlayerCombatState;
      obj = playerCombatState != null ? (object) playerCombatState.AllCards : (object) null;
    }
    if (obj == null)
      obj = (object) Array.Empty<CardModel>();
    foreach (CardModel card in (IEnumerable<CardModel>) obj)
    {
      if (card.Affliction is Bound)
        CardCmd.ClearAffliction(card);
    }
    return Task.CompletedTask;
  }

  private class Data
  {
    public bool boundCardPlayed;
  }
}
