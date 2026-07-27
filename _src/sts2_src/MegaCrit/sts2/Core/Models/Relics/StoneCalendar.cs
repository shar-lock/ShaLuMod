// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.StoneCalendar
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class StoneCalendar : RelicModel
{
  private const string _damageTurnKey = "DamageTurn";
  private bool _isActivating;

  public override RelicRarity Rarity => RelicRarity.Rare;

  public override bool ShowCounter => this.DisplayAmount > -1;

  public override int DisplayAmount
  {
    get
    {
      if (!CombatManager.Instance.IsInProgress || this.IsCanonical)
        return -1;
      int intValue = this.DynamicVars["DamageTurn"].IntValue;
      if (this.IsActivating)
        return intValue;
      int turnNumber = this.Owner.PlayerCombatState.TurnNumber;
      return turnNumber >= intValue ? -1 : turnNumber;
    }
  }

  private bool IsActivating
  {
    get => this._isActivating;
    set
    {
      this.AssertMutable();
      this._isActivating = value;
      this.InvokeDisplayAmountChanged();
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new DamageVar(52M, ValueProp.Unpowered),
        new DynamicVar("DamageTurn", 7M)
      });
    }
  }

  public override Task AfterSideTurnStart(
    CombatSide side,
    IReadOnlyList<Creature> participants,
    ICombatState combatState)
  {
    if (!participants.Contains<Creature>(this.Owner.Creature))
      return Task.CompletedTask;
    if (this.Owner.PlayerCombatState.TurnNumber == this.DynamicVars["DamageTurn"].IntValue)
      this.Status = RelicStatus.Active;
    this.InvokeDisplayAmountChanged();
    return Task.CompletedTask;
  }

  public override async Task BeforeSideTurnEnd(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    if (!participants.Contains<Creature>(this.Owner.Creature))
      return;
    int intValue = this.DynamicVars["DamageTurn"].IntValue;
    int turnNumber = this.Owner.PlayerCombatState.TurnNumber;
    this.Status = RelicStatus.Normal;
    if (turnNumber != intValue)
      return;
    TaskHelper.RunSafely(this.DoActivateVisuals());
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, (IEnumerable<Creature>) this.Owner.Creature.CombatState.HittableEnemies, this.DynamicVars.Damage, this.Owner.Creature);
    this.InvokeDisplayAmountChanged();
  }

  public override Task AfterCombatEnd(CombatRoom _)
  {
    this.Status = RelicStatus.Normal;
    this.InvokeDisplayAmountChanged();
    return Task.CompletedTask;
  }

  public override Task AfterRoomEntered(AbstractRoom room)
  {
    if (!(room is CombatRoom))
      return Task.CompletedTask;
    this.Status = RelicStatus.Normal;
    this.InvokeDisplayAmountChanged();
    return Task.CompletedTask;
  }

  private async Task DoActivateVisuals()
  {
    this.IsActivating = true;
    this.Flash();
    await Cmd.Wait(1f);
    this.IsActivating = false;
  }
}
