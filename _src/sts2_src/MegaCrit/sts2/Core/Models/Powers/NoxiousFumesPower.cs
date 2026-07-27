// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.NoxiousFumesPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class NoxiousFumesPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<PoisonPower>());
    }
  }

  public override async Task AfterSideTurnStart(
    CombatSide side,
    IReadOnlyList<Creature> participants,
    ICombatState combatState)
  {
    if (!participants.Contains<Creature>(this.Owner))
      return;
    this.Flash();
    await Cmd.CustomScaledWait(0.2f, 0.4f);
    foreach (Creature hittableEnemy in (IEnumerable<Creature>) this.CombatState.HittableEnemies)
    {
      NCreature creatureNode = NCombatRoom.Instance?.GetCreatureNode(hittableEnemy);
      if (creatureNode != null)
        ((Node) NCombatRoom.Instance.CombatVfxContainer).AddChildSafely((Node) NGaseousImpactVfx.Create(creatureNode.VfxSpawnPosition, new Color("83eb85")));
    }
    IReadOnlyList<PoisonPower> poisonPowerList = await PowerCmd.Apply<PoisonPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) this.CombatState.HittableEnemies, (Decimal) this.Amount, this.Owner, (CardModel) null);
  }
}
