// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.CorpseSlug
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
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class CorpseSlug : MonsterModel
{
  private const string _heavyAttackTrigger = "HeavyAttackTrigger";
  private const string _doubleAttackTrigger = "DoubleAttackTrigger";
  public const string devourStartTrigger = "DevourStartTrigger";
  public const string devourEndTrigger = "DevourEndkTrigger";
  private const string _attackLightSfx = "event:/sfx/enemy/enemy_attacks/corpse_slugs/corpse_slugs_attack_light";
  public const string ravenousSfx = "event:/sfx/enemy/enemy_attacks/corpse_slugs/corpse_slugs_ravenous";
  public const string ravenousUpSfxDouble = "event:/sfx/enemy/enemy_attacks/corpse_slugs/corpse_slugs_ravenous_up_double";
  private bool _isRavenous;
  private int _starterMoveIdx;

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 27, 25);
  }

  public override int MaxInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 29, 27);
  }

  private int WhipSlapDamage => 3;

  private int WhipSlapRepeat => 2;

  private int GlompDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 9, 8);
  }

  private int GoopFrailAmt => 2;

  private int RavenousStr
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 5, 4);
  }

  protected override string AttackSfx
  {
    get => "event:/sfx/enemy/enemy_attacks/corpse_slugs/corpse_slugs_attack";
  }

  public override string DeathSfx => "event:/sfx/enemy/enemy_attacks/corpse_slugs/corpse_slugs_die";

  public bool IsRavenous
  {
    get => this._isRavenous;
    set
    {
      this.AssertMutable();
      this._isRavenous = value;
    }
  }

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Slime;

  public int StarterMoveIdx
  {
    get => this._starterMoveIdx;
    set
    {
      this.AssertMutable();
      this._starterMoveIdx = value;
    }
  }

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    RavenousPower ravenousPower = await PowerCmd.Apply<RavenousPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, (Decimal) this.RavenousStr, this.Creature, (CardModel) null);
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState moveState1 = new MoveState("WHIP_SLAP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.WhipSlapMove), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(this.WhipSlapDamage, this.WhipSlapRepeat)
    });
    MoveState moveState2 = new MoveState("GLOMP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.GlompMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.GlompDamage)
    });
    MoveState moveState3 = new MoveState("GOOP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.GoopMove), new AbstractIntent[1]
    {
      (AbstractIntent) new DebuffIntent()
    });
    moveState1.FollowUpState = (MonsterState) moveState2;
    moveState2.FollowUpState = (MonsterState) moveState3;
    moveState3.FollowUpState = (MonsterState) moveState1;
    states.Add((MonsterState) moveState1);
    states.Add((MonsterState) moveState2);
    states.Add((MonsterState) moveState3);
    MoveState moveState4;
    switch (this.StarterMoveIdx % 3)
    {
      case 0:
        moveState4 = moveState1;
        break;
      case 1:
        moveState4 = moveState2;
        break;
      default:
        moveState4 = moveState3;
        break;
    }
    MoveState initialState = moveState4;
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task WhipSlapMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.WhipSlapDamage).WithHitCount(this.WhipSlapRepeat).FromMonster((MonsterModel) this).WithAttackerAnim("DoubleAttackTrigger", 0.3f).OnlyPlayAnimOnce().WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/corpse_slugs/corpse_slugs_attack_light").WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  private async Task GlompMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.GlompDamage).FromMonster((MonsterModel) this).WithAttackerAnim("HeavyAttackTrigger", 0.3f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  private async Task GoopMove(IReadOnlyList<Creature> targets)
  {
    await CreatureCmd.TriggerAnim(this.Creature, "Attack", 0.2f);
    IReadOnlyList<FrailPower> frailPowerList = await PowerCmd.Apply<FrailPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) targets, (Decimal) this.GoopFrailAmt, this.Creature, (CardModel) null);
  }

  public static void EnsureCorpseSlugsStartWithDifferentMoves(
    IEnumerable<MonsterModel> monsters,
    Rng rng)
  {
    IEnumerable<CorpseSlug> corpseSlugs = monsters.OfType<CorpseSlug>();
    int num = rng.NextInt(3);
    foreach (CorpseSlug corpseSlug in corpseSlugs)
    {
      corpseSlug.StarterMoveIdx = num % 3;
      ++num;
    }
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("attack");
    AnimState state2 = new AnimState("attack_heavy");
    AnimState state3 = new AnimState("attack_double");
    AnimState state4 = new AnimState("hurt");
    AnimState state5 = new AnimState("die");
    AnimState animState = new AnimState("devour_loop", true);
    AnimState state6 = new AnimState("devour_start");
    AnimState state7 = new AnimState("devour_end");
    AnimState state8 = new AnimState("hurt_devouring");
    AnimState state9 = new AnimState("die_devouring");
    state2.NextState = initialState;
    state1.NextState = initialState;
    state3.NextState = initialState;
    state4.NextState = initialState;
    state6.NextState = animState;
    state7.NextState = initialState;
    state8.NextState = animState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("HeavyAttackTrigger", state2);
    animator.AddAnyState("DoubleAttackTrigger", state3);
    animator.AddAnyState("DevourStartTrigger", state6, (Func<bool>) (() => !this._isRavenous));
    animator.AddAnyState("DevourEndkTrigger", state7);
    animator.AddAnyState("Attack", state1);
    animator.AddAnyState("Dead", state5, (Func<bool>) (() => !this._isRavenous));
    animator.AddAnyState("Hit", state4, (Func<bool>) (() => !this._isRavenous));
    animator.AddAnyState("Dead", state9, (Func<bool>) (() => this._isRavenous));
    animator.AddAnyState("Hit", state8, (Func<bool>) (() => this._isRavenous));
    return animator;
  }
}
