// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.MagiKnight
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class MagiKnight : MonsterModel
{
  private static readonly LocString _dampenDialogue = new LocString("powers", "DAMPEN_POWER.banter");
  private const string _prepMove = "PREP_MOVE";
  private const string _bombTrigger = "BombCast";
  private const string _ramAttackTrigger = "RamAttack";
  private const string _shieldTrigger = "ShieldAttack";
  private const string _ramSfx = "event:/sfx/enemy/enemy_attacks/magi_knight/magi_knight_attack_ram";
  private const string _bombSfx = "event:/sfx/enemy/enemy_attacks/magi_knight/magi_knight_attack_bomb";
  private const string _castShieldSfx = "event:/sfx/enemy/enemy_attacks/magi_knight/magi_knight_cast_shield";

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 89, 82);
  }

  public override int MaxInitialHp => this.MinInitialHp;

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Armor;

  private int PowerShieldDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 7, 6);
  }

  private int PowerShieldBlock
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 9, 5);
  }

  private int SpearDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 11, 10);
  }

  private int BombDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 40, 35);
  }

  public override string HurtSfx => "event:/sfx/enemy/enemy_attacks/magi_knight/magi_knight_hurt";

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("POWER_SHIELD_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.PowerShieldMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.PowerShieldDamage),
      (AbstractIntent) new DefendIntent()
    });
    MoveState moveState1 = new MoveState("DAMPEN_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.DampenMove), new AbstractIntent[1]
    {
      (AbstractIntent) new DebuffIntent()
    });
    MoveState moveState2 = new MoveState("PREP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.PrepMove), new AbstractIntent[1]
    {
      (AbstractIntent) new DefendIntent()
    });
    MoveState moveState3 = new MoveState("MAGIC_BOMB", new Func<IReadOnlyList<Creature>, Task>(this.MagicBombMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.BombDamage)
    });
    MoveState moveState4 = new MoveState("RAM_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SpearMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.SpearDamage)
    });
    initialState.FollowUpState = (MonsterState) moveState1;
    moveState1.FollowUpState = (MonsterState) moveState4;
    moveState4.FollowUpState = (MonsterState) moveState2;
    moveState2.FollowUpState = (MonsterState) moveState3;
    moveState3.FollowUpState = (MonsterState) moveState4;
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) moveState1);
    states.Add((MonsterState) moveState4);
    states.Add((MonsterState) moveState2);
    states.Add((MonsterState) moveState3);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task DampenMove(IReadOnlyList<Creature> targets)
  {
    foreach (Creature target in (IEnumerable<Creature>) targets)
    {
      DampenPower power = target.GetPower<DampenPower>();
      bool flag = power == null;
      if (flag)
        power = (DampenPower) ModelDb.Power<DampenPower>().ToMutable();
      power.AddCaster(this.Creature);
      if (flag)
        await PowerCmd.Apply((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (PowerModel) power, target, 1M, this.Creature, (CardModel) null);
    }
    TalkCmd.Play(MagiKnight._dampenDialogue, this.Creature, VfxColor.Orange, VfxDuration.Long);
    await Cmd.Wait(0.5f);
  }

  private async Task PowerShieldMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.PowerShieldDamage).FromMonster((MonsterModel) this).WithAttackerAnim("ShieldAttack", 0.6f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/magi_knight/magi_knight_cast_shield").WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
    Decimal num = await CreatureCmd.GainBlock(this.Creature, (Decimal) this.PowerShieldBlock, ValueProp.Move, (CardPlay) null);
  }

  private async Task PrepMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/magi_knight/magi_knight_cast_shield");
    await CreatureCmd.TriggerAnim(this.Creature, "ShieldAttack", 0.6f);
    Decimal num = await CreatureCmd.GainBlock(this.Creature, (Decimal) this.PowerShieldBlock, ValueProp.Move, (CardPlay) null);
  }

  private async Task MagicBombMove(IReadOnlyList<Creature> targets)
  {
    if (TestMode.IsOff && this.CombatState.IsLiveCombat())
    {
      NCreature ncreature = (NCreature) null;
      foreach (Creature target in (IEnumerable<Creature>) targets)
      {
        NCreature creatureNode = target.GetCreatureNode();
        if (creatureNode != null && (ncreature == null || (double) ncreature.GlobalPosition.X > (double) creatureNode.GlobalPosition.X))
          ncreature = creatureNode;
      }
      NCreature creatureNode1 = this.Creature.GetCreatureNode();
      Node2D specialNode = creatureNode1?.GetSpecialNode<Node2D>("Visuals/AttackDistanceControl");
      if (creatureNode1 != null && specialNode != null && ncreature != null)
      {
        float num = 600f * creatureNode1.Visuals.Scale.X;
        specialNode.GlobalPosition = new Vector2(ncreature.GlobalPosition.X + num, specialNode.GlobalPosition.Y);
      }
    }
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.BombDamage).FromMonster((MonsterModel) this).WithAttackerAnim("BombCast", 1.2f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/magi_knight/magi_knight_attack_bomb").WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
  }

  private async Task SpearMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.SpearDamage).FromMonster((MonsterModel) this).WithAttackerAnim("RamAttack", 1.2f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/magi_knight/magi_knight_attack_ram").WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("attack_bomb");
    AnimState state2 = new AnimState("attack_ram");
    AnimState state3 = new AnimState("cast_shield");
    AnimState state4 = new AnimState("hurt");
    AnimState state5 = new AnimState("die");
    state1.NextState = initialState;
    state3.NextState = initialState;
    state2.NextState = initialState;
    state4.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Dead", state5);
    animator.AddAnyState("Hit", state4);
    animator.AddAnyState("Cast", state1);
    animator.AddAnyState("BombCast", state1);
    animator.AddAnyState("RamAttack", state2);
    animator.AddAnyState("ShieldAttack", state3);
    return animator;
  }

  protected override bool ShouldShowMoveInBestiary(string moveStateId)
  {
    return moveStateId != "PREP_MOVE";
  }
}
