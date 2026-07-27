// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.SandpitPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class SandpitPower : PowerModel
{
  private const float _paddingDistanceFromMonster = 450f;
  private const float _paddingDistanceFromOriginal = 50f;
  private const float _tweenTime = 0.25f;
  private int _initialAmount;
  private float _initialTargetPosition;

  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

  public override Task AfterApplied(Creature? applier, CardModel? cardSource)
  {
    if (TestMode.IsOn)
      return Task.CompletedTask;
    NCreature creatureNode = NCombatRoom.Instance.GetCreatureNode(this.Target);
    this._initialAmount = this.Amount;
    this._initialTargetPosition = creatureNode.GlobalPosition.X;
    return Task.CompletedTask;
  }

  public override async Task AfterSideTurnStartLate(
    CombatSide side,
    IReadOnlyList<Creature> participants,
    ICombatState combatState)
  {
    if (side != CombatSide.Enemy)
      return;
    await PowerCmd.Decrement((PowerModel) this);
  }

  public override async Task AfterPowerAmountChanged(
    PlayerChoiceContext choiceContext,
    PowerModel power,
    Decimal _,
    Creature? __,
    CardModel? cardSource)
  {
    if (TestMode.IsOn || power != this)
      return;
    await this.UpdateCreaturePositions();
    if (!LocalContext.IsMe(this.Target))
      return;
    NRunMusicController.Instance?.UpdateMusicParameter(TheInsatiable.TheInsatiableTrackName, (float) Mathf.Clamp(6 - this.Amount, 0, 5));
  }

  public override async Task AfterRemoved(Creature oldOwner)
  {
    if (oldOwner.IsDead || this.Target.IsDead)
      return;
    if (TestMode.IsOff)
    {
      NCreature creatureNode1 = NCombatRoom.Instance.GetCreatureNode(this.Owner);
      Tween tween = ((Node) NCombatRoom.Instance).CreateTween();
      float num = creatureNode1.GlobalPosition.X - 450f;
      foreach (Creature affectedCreature in (IEnumerable<Creature>) this.AllAffectedCreatures)
      {
        NCreature creatureNode2 = NCombatRoom.Instance.GetCreatureNode(affectedCreature);
        tween.Parallel().TweenProperty((GodotObject) creatureNode2, NodePath.op_Implicit("global_position:x"), Variant.op_Implicit(num), 0.699999988079071).SetEase((Tween.EaseType) 2L).SetTrans((Tween.TransitionType) 1L);
      }
    }
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/the_insatiable/the_insatiable_finisher");
    await CreatureCmd.TriggerAnim(this.Owner, "EatPlayerTrigger", 0.0f);
    await Cmd.Wait(0.5f);
    foreach (Creature affectedCreature in (IEnumerable<Creature>) this.AllAffectedCreatures)
    {
      if (TestMode.IsOff)
        ((CanvasItem) NCombatRoom.Instance.GetCreatureNode(affectedCreature).Visuals).Visible = false;
      if (affectedCreature.IsPlayer || affectedCreature.Monster is Osty)
        await CreatureCmd.Kill(affectedCreature, true);
    }
  }

  public override async Task AfterCreatureAddedToCombat(Creature creature)
  {
    if (creature.Side == this.Owner.Side)
      return;
    await this.UpdateCreaturePositions();
  }

  public override async Task AfterOstyRevived(Creature osty)
  {
    await this.UpdateCreaturePositions();
  }

  public override async Task BeforeSideTurnEnd(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    if (side != CombatSide.Enemy)
      return;
    await this.UpdateCreaturePositions();
  }

  private async Task UpdateCreaturePositions()
  {
    if (TestMode.IsOn)
      return;
    NCreature creatureNode1 = NCombatRoom.Instance.GetCreatureNode(this.Owner);
    NCreature creatureNode2 = NCombatRoom.Instance.GetCreatureNode(this.Target);
    float num1 = creatureNode1.GlobalPosition.X - 450f;
    float num2 = this._initialTargetPosition + 50f;
    float num3 = (num2 - num1) / (float) this._initialAmount;
    int num4 = Mathf.Min(this.Amount, this._initialAmount);
    int num5 = Mathf.Max(this.Amount - this._initialAmount, 0);
    NCreature ncreature = creatureNode2;
    float num6 = 0.0f;
    Player player = this.Target?.Player;
    if (player != null && player.IsOstyAlive && LocalContext.IsMe(this.Target))
    {
      NCreature creatureNode3 = NCombatRoom.Instance.GetCreatureNode(this.Target.Player.Osty);
      ncreature = creatureNode3;
      float x = NCreature.GetOstyOffsetFromPlayer(creatureNode3.Entity).X;
      num3 = (num2 + x - num1) / (float) this._initialAmount;
      float num7 = (float) (100.0 * (1.0 - (double) num4 / (double) this._initialAmount));
      num6 = x - num7;
    }
    float num8 = (float) ((double) creatureNode1.GlobalPosition.X - 400.0 + (double) num3 * (double) num4 + (double) num3 * ((double) num5 / ((double) num5 + 2.0)));
    Tween tween = (Tween) null;
    foreach (Creature affectedCreature in (IEnumerable<Creature>) this.AllAffectedCreatures)
    {
      float num9 = num8 - ncreature.GlobalPosition.X;
      if (affectedCreature != ncreature.Entity)
        num9 = num8 - num6 - creatureNode2.GlobalPosition.X;
      if ((double) Math.Abs(num9) > 5.0 && !affectedCreature.IsDead)
      {
        NCreature creatureNode4 = NCombatRoom.Instance.GetCreatureNode(affectedCreature);
        if (creatureNode4 != null)
        {
          if (tween == null)
            tween = ((Node) NCombatRoom.Instance).CreateTween().SetParallel(true).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
          tween.TweenProperty((GodotObject) creatureNode4, NodePath.op_Implicit("global_position:x"), Variant.op_Implicit(creatureNode4.GlobalPosition.X + num9), 0.25);
        }
      }
    }
    if (tween == null)
      return;
    bool flag = await tween.AwaitFinished((Node) NCombatRoom.Instance);
  }

  private IReadOnlyList<Creature> AllAffectedCreatures
  {
    get
    {
      Creature creature1 = this.Target.Player.Creature;
      IReadOnlyList<Creature> pets = this.Target.Pets;
      int index1 = 0;
      Creature[] items = new Creature[1 + pets.Count];
      items[index1] = creature1;
      int index2 = index1 + 1;
      foreach (Creature creature2 in (IEnumerable<Creature>) pets)
      {
        items[index2] = creature2;
        ++index2;
      }
      return (IReadOnlyList<Creature>) new \u003C\u003Ez__ReadOnlyArray<Creature>(items);
    }
  }
}
