// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.ReattachPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class ReattachPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Single;

  public override bool ShouldScaleInMultiplayer => true;

  protected override object InitInternalData() => (object) new ReattachPower.Data();

  private bool IsReviving => this.GetInternalData<ReattachPower.Data>().isReviving;

  public async Task DoReattach()
  {
    if (this.AreAllOtherSegmentsDead())
      return;
    NCombatRoom.Instance?.GetCreatureNode(this.Owner)?.GetSpecialNode<NDecimillipedeSegmentVfx>("%NDecimillipedeSegmentVfx")?.Regenerate();
    this.GetInternalData<ReattachPower.Data>().isReviving = false;
    NCombatRoom.Instance?.SetCreatureIsInteractable(this.Owner, true);
    await CreatureCmd.Heal(this.Owner, (Decimal) this.Amount);
  }

  public override async Task AfterDeath(
    PlayerChoiceContext choiceContext,
    Creature creature,
    bool wasRemovalPrevented,
    float deathAnimLength)
  {
    if (wasRemovalPrevented || this.Owner != creature)
      return;
    if (!this.AreAllOtherSegmentsDead() || !this.Owner.IsDead)
    {
      this.GetInternalData<ReattachPower.Data>().isReviving = true;
      if (creature.Monster is DecimillipedeSegment monster)
        this.Owner.Monster.SetMoveImmediate(monster.DeadState);
      NCombatRoom.Instance?.SetCreatureIsInteractable(this.Owner, false);
    }
    else
    {
      await Cmd.Wait(0.25f, true);
      this.DoFadeOutOnAllSegments();
    }
  }

  public override bool ShouldAllowHitting(Creature creature)
  {
    return creature != this.Owner || !this.IsReviving;
  }

  public override bool ShouldCreatureBeRemovedFromCombatAfterDeath(Creature creature)
  {
    return creature != this.Owner;
  }

  public override bool ShouldPowerBeRemovedAfterOwnerDeath() => false;

  public override bool ShouldOwnerDeathTriggerFatal() => this.AreAllOtherSegmentsDead();

  private void DoFadeOutOnAllSegments()
  {
    List<NCreature> ncreatureList = new List<NCreature>();
    foreach (Creature enemy in (IEnumerable<Creature>) this.CombatState.Enemies)
    {
      NCreature creatureNode = NCombatRoom.Instance?.GetCreatureNode(enemy);
      if (creatureNode != null)
      {
        creatureNode.AnimHideIntent();
        ncreatureList.Add(creatureNode);
      }
    }
    NMonsterDeathVfx nmonsterDeathVfx = NMonsterDeathVfx.Create(ncreatureList);
    if (nmonsterDeathVfx == null || ncreatureList.Count <= 0)
      return;
    Node parent = ((Node) ncreatureList[0]).GetParent();
    parent.AddChildSafely((Node) nmonsterDeathVfx);
    parent.MoveChildSafely((Node) nmonsterDeathVfx, ((Node) ncreatureList[0]).GetIndex(false));
    Task task = TaskHelper.RunSafely(this.PlayVfxAndThenRemoveNodes(nmonsterDeathVfx, ncreatureList));
    foreach (NCreature node in ncreatureList)
    {
      node.DeathAnimationTask = task;
      NCombatRoom.Instance?.RemoveCreatureNode(node);
    }
  }

  private async Task PlayVfxAndThenRemoveNodes(NMonsterDeathVfx vfx, List<NCreature> creatures)
  {
    await Cmd.Wait(0.25f, true);
    await vfx.PlayVfx();
    foreach (Node creature in creatures)
      creature.QueueFreeSafely();
  }

  private IEnumerable<Creature> GetOtherSegments()
  {
    // ISSUE: object of a compiler-generated type is created
    return this.Owner.CombatState.GetTeammatesOf(this.Owner).Except<Creature>((IEnumerable<Creature>) new \u003C\u003Ez__ReadOnlySingleElementList<Creature>(this.Owner)).Where<Creature>((Func<Creature, bool>) (c => c.HasPower<ReattachPower>()));
  }

  private bool AreAllOtherSegmentsDead()
  {
    return this.GetOtherSegments().All<Creature>((Func<Creature, bool>) (s => s.IsDead));
  }

  private class Data
  {
    public bool isReviving;
  }
}
