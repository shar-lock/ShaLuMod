// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.DoomPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class DoomPower : PowerModel
{
  public override PowerType Type => PowerType.Debuff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override Color AmountLabelColor => PowerModel._normalAmountLabelColor;

  public static async Task DoomKill(IReadOnlyList<Creature> creatures)
  {
    ICombatState combatState;
    if (creatures.Count == 0)
    {
      combatState = (ICombatState) null;
    }
    else
    {
      combatState = creatures.First<Creature>().CombatState;
      foreach (Creature creature in (IEnumerable<Creature>) creatures)
      {
        await DoomPower.PlayVfx(creature);
        await CreatureCmd.Kill(creature);
      }
      await Hook.AfterDiedToDoom(combatState, creatures);
      combatState = (ICombatState) null;
    }
  }

  public static IReadOnlyList<Creature> GetDoomedCreatures(IReadOnlyList<Creature> creatures)
  {
    return (IReadOnlyList<Creature>) creatures.Where<Creature>((Func<Creature, bool>) (c =>
    {
      DoomPower power = c.GetPower<DoomPower>();
      return power != null && power.IsOwnerDoomed();
    })).ToList<Creature>();
  }

  public override async Task BeforeSideTurnEnd(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    if (side == CombatSide.Player || !this.ShouldDoomTrigger(participants))
      return;
    await DoomPower.DoomKill(this.GetDoomedCreatures(side));
  }

  public override async Task AfterSideTurnEnd(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    if (side == CombatSide.Enemy || !this.ShouldDoomTrigger(participants))
      return;
    await DoomPower.DoomKill(this.GetDoomedCreatures(side));
  }

  private bool ShouldDoomTrigger(IEnumerable<Creature> participants)
  {
    return !CombatManager.Instance.IsOverOrEnding && participants.Contains<Creature>(this.Owner) && !this.Owner.IsDead && this.IsOwnerDoomed() && this.GetDoomedCreatures(this.Owner.Side).First<Creature>() == this.Owner;
  }

  private static async Task PlayVfx(Creature creature)
  {
    NCreature creatureNode = NCombatRoom.Instance?.GetCreatureNode(creature);
    if (creatureNode == null)
      return;
    bool shouldDie = false;
    if (creature.IsMonster)
      shouldDie = Hook.ShouldDie(creature.Player?.RunState ?? creature.CombatState.RunState, creature.CombatState, creature, out AbstractModel _) && creature.Monster.ShouldDisappearFromDoom;
    DoomPower.StartDoomAnim(creatureNode, shouldDie);
    NDoomOverlayVfx child = NDoomOverlayVfx.GetOrCreate();
    if (child != null && !((Node) child).IsInsideTree())
      ((Node) NCombatRoom.Instance.CombatVfxContainer).AddChildSafely((Node) child);
    List<Creature> list = creature.CombatState.GetTeammatesOf(creature).Where<Creature>((Func<Creature, bool>) (c => c.IsAlive)).ToList<Creature>();
    if (!shouldDie)
      return;
    if (list.Count<Creature>() == 1 && list.First<Creature>() == creature)
      await Cmd.Wait(1.5f);
    else
      await Cmd.Wait(0.25f);
  }

  private IReadOnlyList<Creature> GetDoomedCreatures(CombatSide side)
  {
    return DoomPower.GetDoomedCreatures(this.CombatState.GetCreaturesOnSide(side));
  }

  private bool IsOwnerDoomed() => this.Owner.CurrentHp <= this.Amount;

  private static void StartDoomAnim(NCreature creature, bool shouldDie)
  {
    Task task1 = (Task) null;
    if (shouldDie)
    {
      creature.Entity.Monster?.OnDieToDoom();
      creature.DisableInteractionForDeath();
      Tween t = creature.AnimDisableUi();
      t.TweenCallback(Callable.From(new Action(((GodotTreeExtensions) creature).QueueFreeSafely)));
      task1 = DoomPower.WaitForTween(t, (Node) creature);
      if (creature.SpineAnimation.IsValid)
      {
        creature.SetAnimationTrigger("Hit");
        using (MegaTrackEntry currentTrack = creature.SpineAnimation.GetCurrentTrack())
        {
          if (currentTrack?.GetAnimationName() == "hurt")
          {
            MonsterModel monster = creature.Entity.Monster;
            float time = monster != null ? monster.HurtAnimationTrackOffsetForDoom : 0.1f;
            currentTrack.SetTrackTime(time);
            currentTrack.SetTimeScale(0.0f);
          }
        }
      }
      NCombatRoom.Instance?.RemoveCreatureNode(creature);
    }
    NDoomVfx child = NDoomVfx.Create(creature.Visuals, creature.Hitbox.GlobalPosition, creature.Hitbox.Size, shouldDie);
    NCombatRoom instance = NCombatRoom.Instance;
    if (instance != null)
      ((Node) instance.CombatVfxContainer).AddChildSafely((Node) child);
    if (!shouldDie)
      return;
    NCreature ncreature = creature;
    \u003C\u003Ey__InlineArray2<Task> buffer = new \u003C\u003Ey__InlineArray2<Task>();
    // ISSUE: reference to a compiler-generated method
    \u003CPrivateImplementationDetails\u003E.InlineArrayElementRef<\u003C\u003Ey__InlineArray2<Task>, Task>(ref buffer, 0) = task1;
    // ISSUE: reference to a compiler-generated method
    \u003CPrivateImplementationDetails\u003E.InlineArrayElementRef<\u003C\u003Ey__InlineArray2<Task>, Task>(ref buffer, 1) = child.VfxTask;
    // ISSUE: reference to a compiler-generated method
    Task task2 = Task.WhenAll(\u003CPrivateImplementationDetails\u003E.InlineArrayAsReadOnlySpan<\u003C\u003Ey__InlineArray2<Task>, Task>(in buffer, 2));
    ncreature.DeathAnimationTask = task2;
  }

  private static async Task WaitForTween(Tween t, Node owner)
  {
    bool flag = await t.AwaitFinished(owner);
  }
}
