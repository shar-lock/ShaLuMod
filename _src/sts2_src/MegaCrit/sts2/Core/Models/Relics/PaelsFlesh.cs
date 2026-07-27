// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.PaelsFlesh
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class PaelsFlesh : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Ancient;

  public override int DisplayAmount
  {
    get
    {
      PlayerCombatState playerCombatState = this.Owner.PlayerCombatState;
      return playerCombatState == null ? 1 : playerCombatState.TurnNumber;
    }
  }

  public override bool ShowCounter
  {
    get => CombatManager.Instance.IsInProgress && this.Status == RelicStatus.Normal;
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new EnergyVar(1));
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy((RelicModel) this));
    }
  }

  public override Decimal ModifyMaxEnergy(Player player, Decimal amount)
  {
    return player != this.Owner || player.PlayerCombatState.TurnNumber < 3 ? amount : amount + this.DynamicVars.Energy.BaseValue;
  }

  public override Task BeforeCombatStart()
  {
    this.InvokeDisplayAmountChanged();
    return Task.CompletedTask;
  }

  public override Task BeforeSideTurnStart(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IReadOnlyList<Creature> participants,
    ICombatState combatState)
  {
    if (!participants.Contains<Creature>(this.Owner.Creature))
      return Task.CompletedTask;
    this.InvokeDisplayAmountChanged();
    return Task.CompletedTask;
  }

  public override Task AfterSideTurnStart(
    CombatSide side,
    IReadOnlyList<Creature> participants,
    ICombatState combatState)
  {
    if (!participants.Contains<Creature>(this.Owner.Creature) || this.Owner.PlayerCombatState.TurnNumber < 3 || this.Status == RelicStatus.Active)
      return Task.CompletedTask;
    this.Status = RelicStatus.Active;
    this.InvokeDisplayAmountChanged();
    this.Flash();
    return Task.CompletedTask;
  }

  public override Task AfterCombatEnd(CombatRoom room)
  {
    this.Status = RelicStatus.Normal;
    this.InvokeDisplayAmountChanged();
    return Task.CompletedTask;
  }
}
