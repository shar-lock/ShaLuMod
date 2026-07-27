// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.HexPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Afflictions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class HexPower : PowerModel
{
  public override PowerType Type => PowerType.Debuff;

  public override PowerStackType StackType => PowerStackType.Single;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get => HoverTipFactory.FromAffliction<Hexed>(this.Amount);
  }

  public override bool TryModifyKeywordsInCombat(CardModel card, ISet<CardKeyword> keywords)
  {
    return card.Owner == this.Owner.Player && card.Affliction is Hexed && keywords.Add(CardKeyword.Ethereal);
  }

  public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
  {
    foreach (CardModel allCard in this.Owner.Player.PlayerCombatState.AllCards)
      await this.Afflict(allCard);
  }

  public override async Task AfterCardEnteredCombat(CardModel card)
  {
    if (card.Owner != this.Owner.Player)
      return;
    await this.Afflict(card);
  }

  public override async Task AfterDeath(
    PlayerChoiceContext choiceContext,
    Creature creature,
    bool wasRemovalPrevented,
    float deathAnimLength)
  {
    if (wasRemovalPrevented || creature != this.Applier)
      return;
    await PowerCmd.Remove((PowerModel) this);
  }

  public override Task AfterRemoved(Creature oldOwner)
  {
    foreach (CardModel allCard in this.Owner.Player.PlayerCombatState.AllCards)
    {
      if (allCard.Affliction is Hexed)
        CardCmd.ClearAffliction(allCard);
    }
    return Task.CompletedTask;
  }

  private async Task Afflict(CardModel card)
  {
    if (card.Affliction != null)
      return;
    Hexed hexed = await CardCmd.Afflict<Hexed>(card, (Decimal) this.Amount);
  }
}
