// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.Toadpole
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
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class Toadpole : MonsterModel
{
  private static readonly string[] _eyeOptions = new string[2]
  {
    "eye1",
    "eye2"
  };
  private static readonly string[] _patternOptions = new string[2]
  {
    "pattern1",
    "pattern2"
  };
  private bool _isFront;
  private const string _attackSingleTrigger = "AttackSingle";
  private const string _attackTripleTrigger = "AttackTriple";
  private const string _spinAttackSfx = "event:/sfx/enemy/enemy_attacks/toadpole/toadpole_attack_spin";

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 22, 21);
  }

  public override int MaxInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 26, 25);
  }

  public bool IsFront
  {
    get => this._isFront;
    set
    {
      this.AssertMutable();
      this._isFront = value;
    }
  }

  private int SpikeSpitDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);
  }

  private int SpikeSpitRepeat => 3;

  private int WhirlDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 8, 7);
  }

  private int SpikenAmount => 2;

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Slime;

  public override void SetupSkins(MegaSprite spine, MegaSkeleton skeleton)
  {
    MegaSkin skin = spine.NewSkin("custom-skin");
    MegaSkeletonDataResource data = skeleton.GetData();
    skin.AddSkin(data.FindSkin(Rng.Chaotic.NextItem<string>((IEnumerable<string>) Toadpole._eyeOptions)));
    skin.AddSkin(data.FindSkin(Rng.Chaotic.NextItem<string>((IEnumerable<string>) Toadpole._patternOptions)));
    skeleton.SetSkin(skin);
    skeleton.SetSlotsToSetupPose();
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState moveState = new MoveState("SPIKE_SPIT_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SpikeSpitMove), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(this.SpikeSpitDamage, this.SpikeSpitRepeat)
    });
    MoveState move1 = new MoveState("WHIRL_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.WhirlMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.WhirlDamage)
    });
    MoveState move2 = new MoveState("SPIKEN_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SpikenMove), new AbstractIntent[1]
    {
      (AbstractIntent) new BuffIntent()
    });
    ConditionalBranchState initialState = new ConditionalBranchState("INIT_MOVE");
    move1.FollowUpState = (MonsterState) move2;
    move2.FollowUpState = (MonsterState) moveState;
    moveState.FollowUpState = (MonsterState) move1;
    initialState.AddState((MonsterState) move1, (Func<bool>) (() => !((Toadpole) this.Creature.Monster).IsFront));
    initialState.AddState((MonsterState) move2, (Func<bool>) (() => ((Toadpole) this.Creature.Monster).IsFront));
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) move2);
    states.Add((MonsterState) moveState);
    states.Add((MonsterState) move1);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task SpikeSpitMove(IReadOnlyList<Creature> targets)
  {
    ThornsPower thornsPower = await PowerCmd.Apply<ThornsPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, (Decimal) -this.SpikenAmount, this.Creature, (CardModel) null);
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.SpikeSpitDamage).WithHitCount(this.SpikeSpitRepeat).FromMonster((MonsterModel) this).WithAttackerAnim("AttackTriple", 0.3f).OnlyPlayAnimOnce().WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/toadpole/toadpole_attack_spin").WithHitFx("vfx/vfx_attack_blunt", this.AttackSfx).Execute((PlayerChoiceContext) null);
  }

  private async Task WhirlMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.WhirlDamage).FromMonster((MonsterModel) this).WithAttackerAnim("AttackSingle", 0.15f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_blunt", this.AttackSfx).Execute((PlayerChoiceContext) null);
  }

  private async Task SpikenMove(IReadOnlyList<Creature> targets)
  {
    await CreatureCmd.TriggerAnim(this.Creature, "Cast", 0.2f);
    ThornsPower thornsPower = await PowerCmd.Apply<ThornsPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, (Decimal) this.SpikenAmount, this.Creature, (CardModel) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("buff");
    AnimState state2 = new AnimState("attack_single");
    AnimState state3 = new AnimState("attack_triple");
    AnimState state4 = new AnimState("hurt");
    AnimState state5 = new AnimState("die");
    AnimState animState = new AnimState("idle_loop_buffed", true);
    AnimState state6 = new AnimState("attack_single_buffed");
    AnimState state7 = new AnimState("attack_triple");
    AnimState state8 = new AnimState("hurt_buffed");
    AnimState state9 = new AnimState("die_buffed");
    state1.NextState = animState;
    state2.NextState = initialState;
    state3.NextState = initialState;
    state4.NextState = initialState;
    state6.NextState = animState;
    state7.NextState = initialState;
    state8.NextState = animState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Cast", state1);
    animator.AddAnyState("AttackSingle", state2, (Func<bool>) (() => !this.Creature.HasPower<ThornsPower>()));
    animator.AddAnyState("AttackTriple", state3, (Func<bool>) (() => !this.Creature.HasPower<ThornsPower>()));
    animator.AddAnyState("Dead", state5, (Func<bool>) (() => !this.Creature.HasPower<ThornsPower>()));
    animator.AddAnyState("Hit", state4, (Func<bool>) (() => !this.Creature.HasPower<ThornsPower>()));
    animator.AddAnyState("AttackSingle", state6, (Func<bool>) (() => this.Creature.HasPower<ThornsPower>()));
    animator.AddAnyState("AttackTriple", state7, (Func<bool>) (() => this.Creature.HasPower<ThornsPower>()));
    animator.AddAnyState("Dead", state9, (Func<bool>) (() => this.Creature.HasPower<ThornsPower>()));
    animator.AddAnyState("Hit", state8, (Func<bool>) (() => this.Creature.HasPower<ThornsPower>()));
    return animator;
  }
}
