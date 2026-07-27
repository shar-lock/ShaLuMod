// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.InfernoPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class InfernoPower : PowerModel
{
  private const string _selfDamageKey = "SelfDamage";

  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new DamageVar("SelfDamage", 0M, ValueProp.Unblockable | ValueProp.Unpowered));
    }
  }

  public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
  {
    if (player != this.Owner.Player)
      return;
    NCombatRoom instance = NCombatRoom.Instance;
    if (instance != null)
      ((Node) instance.CombatVfxContainer).AddChildSafely((Node) NFireSmokePuffVfx.Create(this.Owner));
    await Cmd.CustomScaledWait(0.2f, 0.4f);
    DamageVar dynamicVar = (DamageVar) this.DynamicVars["SelfDamage"];
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, this.Owner, dynamicVar.BaseValue, dynamicVar.Props, this.Owner);
  }

  public override async Task AfterDamageReceived(
    PlayerChoiceContext choiceContext,
    Creature target,
    DamageResult result,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource)
  {
    if (target != this.Owner || result.UnblockedDamage <= 0 || this.Owner.CombatState.CurrentSide != this.Owner.Side)
      return;
    foreach (Creature hittableEnemy in (IEnumerable<Creature>) this.CombatState.HittableEnemies)
    {
      NFireBurstVfx child = NFireBurstVfx.Create(hittableEnemy, 0.75f);
      NCombatRoom instance = NCombatRoom.Instance;
      if (instance != null)
        ((Node) instance.CombatVfxContainer).AddChildSafely((Node) child);
    }
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, (IEnumerable<Creature>) this.CombatState.HittableEnemies, (Decimal) this.Amount, ValueProp.Unpowered, this.Owner);
  }

  public void IncrementSelfDamage()
  {
    this.AssertMutable();
    ++this.DynamicVars["SelfDamage"].BaseValue;
  }
}
