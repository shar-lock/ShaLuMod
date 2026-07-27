// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.ArtOfWar
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class ArtOfWar : RelicModel
{
  private bool _anyAttacksPlayedLastTurn;
  private bool _anyAttacksPlayedThisTurn;

  public override RelicRarity Rarity => RelicRarity.Rare;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new EnergyVar(1));
    }
  }

  private bool AnyAttacksPlayedLastTurn
  {
    get => this._anyAttacksPlayedLastTurn;
    set
    {
      this.AssertMutable();
      this._anyAttacksPlayedLastTurn = value;
    }
  }

  private bool AnyAttacksPlayedThisTurn
  {
    get => this._anyAttacksPlayedThisTurn;
    set
    {
      this.AssertMutable();
      this._anyAttacksPlayedThisTurn = value;
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy((RelicModel) this));
    }
  }

  public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (this.Owner != cardPlay.Card.Owner || !CombatManager.Instance.IsInProgress || cardPlay.Card.Type != CardType.Attack || this.AnyAttacksPlayedLastTurn)
      return Task.CompletedTask;
    this.Status = RelicStatus.Normal;
    this.AnyAttacksPlayedThisTurn = true;
    return Task.CompletedTask;
  }

  public override Task AfterSideTurnEnd(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    if (!participants.Contains<Creature>(this.Owner.Creature))
      return Task.CompletedTask;
    this.AnyAttacksPlayedLastTurn = this.AnyAttacksPlayedThisTurn;
    this.AnyAttacksPlayedThisTurn = false;
    return Task.CompletedTask;
  }

  public override async Task AfterEnergyReset(Player player)
  {
    if (player != this.Owner)
      return;
    this.Status = RelicStatus.Active;
    if (this.Owner.PlayerCombatState.TurnNumber <= 1)
      return;
    if (!this.AnyAttacksPlayedLastTurn)
    {
      this.Flash();
      await PlayerCmd.GainEnergy(this.DynamicVars.Energy.BaseValue, this.Owner);
    }
    this.AnyAttacksPlayedLastTurn = false;
    this.AnyAttacksPlayedThisTurn = false;
  }

  public override Task AfterCombatEnd(CombatRoom _)
  {
    this.Status = RelicStatus.Normal;
    this.AnyAttacksPlayedLastTurn = false;
    this.AnyAttacksPlayedThisTurn = false;
    return Task.CompletedTask;
  }
}
