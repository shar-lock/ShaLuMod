// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.SpectralKnight
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class SpectralKnight : MonsterModel
{
  private const int _soulFlameRepeat = 3;
  private const string _attackFlameTrigger = "AttackFlame";
  private const string _attackSwordTrigger = "AttackSword";
  private const string _hexSfx = "event:/sfx/enemy/enemy_attacks/spectral_knight/spectral_knight_hex";
  private const string _soulFlameSfx = "event:/sfx/enemy/enemy_attacks/spectral_knight/spectral_knight_soul_flame";
  private const string _soulSlashSfx = "event:/sfx/enemy/enemy_attacks/spectral_knight/spectral_knight_soul_slash";

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 97, 93);
  }

  public override int MaxInitialHp => this.MinInitialHp;

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Armor;

  private int SoulSlashDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 17, 15);
  }

  private int SoulFlameDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("HEX", new Func<IReadOnlyList<Creature>, Task>(this.HexMove), new AbstractIntent[1]
    {
      (AbstractIntent) new DebuffIntent()
    });
    MoveState state1 = new MoveState("SOUL_SLASH", new Func<IReadOnlyList<Creature>, Task>(this.SoulSlashMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.SoulSlashDamage)
    });
    MoveState state2 = new MoveState("SOUL_FLAME", new Func<IReadOnlyList<Creature>, Task>(this.SoulFlameMove), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(this.SoulFlameDamage, 3)
    });
    RandomBranchState randomBranchState = new RandomBranchState("RAND");
    initialState.FollowUpState = (MonsterState) state1;
    state1.FollowUpState = (MonsterState) randomBranchState;
    state2.FollowUpState = (MonsterState) randomBranchState;
    randomBranchState.AddBranch((MonsterState) state1, 2);
    randomBranchState.AddBranch((MonsterState) state2, MoveRepeatType.CannotRepeat);
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) state1);
    states.Add((MonsterState) state2);
    states.Add((MonsterState) randomBranchState);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task HexMove(IReadOnlyList<Creature> targets)
  {
    await CreatureCmd.TriggerAnim(this.Creature, "Cast", 0.3f);
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/spectral_knight/spectral_knight_hex");
    foreach (Creature target in (IEnumerable<Creature>) targets)
    {
      HexPower hexPower = await PowerCmd.Apply<HexPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), target, 2M, this.Creature, (CardModel) null);
    }
  }

  private async Task SoulSlashMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.SoulSlashDamage).FromMonster((MonsterModel) this).WithAttackerAnim("AttackSword", 0.25f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/spectral_knight/spectral_knight_soul_slash").WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  private async Task SoulFlameMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.SoulFlameDamage).WithHitCount(3).FromMonster((MonsterModel) this).OnlyPlayAnimOnce().WithAttackerAnim("AttackFlame", 0.25f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/spectral_knight/spectral_knight_soul_flame").Execute((PlayerChoiceContext) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("debuff");
    AnimState state2 = new AnimState("attack_sword");
    AnimState state3 = new AnimState("attack_flame");
    AnimState state4 = new AnimState("hurt");
    AnimState state5 = new AnimState("die");
    state1.NextState = initialState;
    state2.NextState = initialState;
    state3.NextState = initialState;
    state4.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Cast", state1);
    animator.AddAnyState("AttackSword", state2);
    animator.AddAnyState("AttackFlame", state3);
    animator.AddAnyState("Dead", state5);
    animator.AddAnyState("Hit", state4);
    return animator;
  }
}
