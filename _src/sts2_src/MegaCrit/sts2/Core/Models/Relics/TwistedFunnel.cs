// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.TwistedFunnel
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class TwistedFunnel : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Uncommon;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new PowerVar<PoisonPower>(4M));
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<PoisonPower>());
    }
  }

  public override async Task BeforeSideTurnStart(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IReadOnlyList<Creature> participants,
    ICombatState combatState)
  {
    if (!participants.Contains<Creature>(this.Owner.Creature) || this.Owner.PlayerCombatState.TurnNumber > 1)
      return;
    this.Flash();
    foreach (Creature hittableEnemy in (IEnumerable<Creature>) this.Owner.Creature.CombatState.HittableEnemies)
    {
      NCombatRoom instance = NCombatRoom.Instance;
      if (instance != null)
        ((Node) instance.CombatVfxContainer).AddChildSafely((Node) NSmokePuffVfx.Create(hittableEnemy, NSmokePuffVfx.SmokePuffColor.Green));
    }
    await Cmd.CustomScaledWait(0.2f, 0.4f);
    foreach (Creature hittableEnemy in (IEnumerable<Creature>) this.Owner.Creature.CombatState.HittableEnemies)
    {
      PoisonPower poisonPower = await PowerCmd.Apply<PoisonPower>(choiceContext, hittableEnemy, (Decimal) this.DynamicVars["PoisonPower"].IntValue, this.Owner.Creature, (CardModel) null);
    }
  }
}
