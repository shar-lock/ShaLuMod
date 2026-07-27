// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.SurroundedPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class SurroundedPower : PowerModel
{
  private const string _kaiserCrabDirectionCustomTrackName = "kaiser_crab_direction";
  private SurroundedPower.Direction _facing;

  public override PowerType Type => PowerType.Debuff;

  public override PowerStackType StackType => PowerStackType.Single;

  public SurroundedPower.Direction Facing
  {
    get => this._facing;
    private set
    {
      this.AssertMutable();
      this._facing = value;
    }
  }

  public override Decimal ModifyDamageMultiplicative(
    Creature? target,
    Decimal amount,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource,
    CardPlay? cardPlay)
  {
    if (dealer == null || target != this.Owner)
      return 1M;
    Decimal num;
    switch (this.Facing)
    {
      case SurroundedPower.Direction.Right:
        if (!dealer.HasPower<BackAttackLeftPower>())
        {
          num = 1M;
          break;
        }
        goto default;
      case SurroundedPower.Direction.Left:
        if (!dealer.HasPower<BackAttackRightPower>())
        {
          num = 1M;
          break;
        }
        goto default;
      default:
        num = 1.5M;
        break;
    }
    return num;
  }

  public override async Task BeforeCardPlayed(CardPlay cardPlay)
  {
    if (cardPlay.Target == null || cardPlay.Card.Owner != this.Owner.Player)
      return;
    await this.UpdateDirection(cardPlay.Target);
  }

  public override async Task BeforePotionUsed(PotionModel potion, Creature? target)
  {
    if (!CombatManager.Instance.IsInProgress || target == null || potion.Owner != this.Owner.Player)
      return;
    await this.UpdateDirection(target);
  }

  public override async Task AfterDeath(
    PlayerChoiceContext choiceContext,
    Creature creature,
    bool wasRemovalPrevented,
    float deathAnimLength)
  {
    if (wasRemovalPrevented || creature.Side == this.Owner.Side)
      return;
    IReadOnlyList<Creature> hittableEnemies = this.Owner.CombatState.HittableEnemies;
    if (hittableEnemies.Count == 0 || !hittableEnemies.All<Creature>((Func<Creature, bool>) (e => e.HasPower<BackAttackLeftPower>())) && !hittableEnemies.All<Creature>((Func<Creature, bool>) (e => e.HasPower<BackAttackRightPower>())))
      return;
    await this.UpdateDirection(hittableEnemies[0]);
  }

  private async Task UpdateDirection(Creature target)
  {
    switch (this.Facing)
    {
      case SurroundedPower.Direction.Right:
        if (!target.HasPower<BackAttackLeftPower>())
          break;
        await this.FaceDirection(SurroundedPower.Direction.Left);
        break;
      case SurroundedPower.Direction.Left:
        if (!target.HasPower<BackAttackRightPower>())
          break;
        await this.FaceDirection(SurroundedPower.Direction.Right);
        break;
    }
  }

  private async Task FaceDirection(SurroundedPower.Direction direction)
  {
    this.Facing = direction;
    Creature owner = this.Owner;
    IReadOnlyList<Creature> pets = this.Owner.Pets;
    int index1 = 0;
    Creature[] items = new Creature[1 + pets.Count];
    items[index1] = owner;
    int index2 = index1 + 1;
    foreach (Creature creature in (IEnumerable<Creature>) pets)
    {
      items[index2] = creature;
      ++index2;
    }
    // ISSUE: object of a compiler-generated type is created
    foreach (Node2D body in new \u003C\u003Ez__ReadOnlyArray<Creature>(items).Select<Creature, Node2D>((Func<Creature, Node2D>) (c => NCombatRoom.Instance?.GetCreatureNode(c)?.Body)))
      await this.FlipScale(body);
    if (LocalContext.GetMe(this.Owner.CombatState) != this.Owner.Player)
      return;
    NRunMusicController.Instance?.UpdateMusicParameter("kaiser_crab_direction", this.Facing == SurroundedPower.Direction.Left ? 1f : 2f);
  }

  private Task FlipScale(Node2D? body)
  {
    if (body == null)
      return Task.CompletedTask;
    float x = body.Scale.X;
    if (this.Facing == SurroundedPower.Direction.Right && (double) x < 0.0 || this.Facing == SurroundedPower.Direction.Left && (double) x > 0.0)
    {
      Node2D node2D = body;
      node2D.Scale = Vector2.op_Multiply(node2D.Scale, new Vector2(-1f, 1f));
    }
    return Task.CompletedTask;
  }

  public enum Direction
  {
    Right,
    Left,
  }
}
