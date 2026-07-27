// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.TheForgotten
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

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
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class TheForgotten : MonsterModel
{
  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 111, 106);
  }

  public override int MaxInitialHp => this.MinInitialHp;

  private int DreadDamage
  {
    get
    {
      return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 15, 13) + this.Creature.GetPowerAmount<DexterityPower>();
    }
  }

  private int DebilitatingSmogDexStealAmount
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 2, 2);
  }

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Stone;

  public override async Task AfterAddedToRoom()
  {
    PossessSpeedPower possessSpeedPower = await PowerCmd.Apply<PossessSpeedPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 1M, (Creature) null, (CardModel) null);
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("MIASMA", new Func<IReadOnlyList<Creature>, Task>(this.MiasmaMove), new AbstractIntent[3]
    {
      (AbstractIntent) new DebuffIntent(),
      (AbstractIntent) new DefendIntent(),
      (AbstractIntent) new BuffIntent()
    });
    MoveState moveState = new MoveState("DREAD", new Func<IReadOnlyList<Creature>, Task>(this.DreadMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent((Func<Decimal>) (() => (Decimal) this.DreadDamage))
    });
    initialState.FollowUpState = (MonsterState) moveState;
    moveState.FollowUpState = (MonsterState) initialState;
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) moveState);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task MiasmaMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play(this.CastSfx);
    await CreatureCmd.TriggerAnim(this.Creature, "Cast", 0.5f);
    IReadOnlyList<DexterityPower> dexterityPowerList = await PowerCmd.Apply<DexterityPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) targets, (Decimal) -this.DebilitatingSmogDexStealAmount, this.Creature, (CardModel) null);
    Decimal num = await CreatureCmd.GainBlock(this.Creature, 8M, ValueProp.Move, (CardPlay) null);
    DexterityPower dexterityPower = await PowerCmd.Apply<DexterityPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, (Decimal) this.DebilitatingSmogDexStealAmount, this.Creature, (CardModel) null);
  }

  private async Task DreadMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.DreadDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.15f).OnlyPlayAnimOnce().WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("debuff");
    AnimState state2 = new AnimState("attack");
    AnimState state3 = new AnimState("hurt");
    AnimState state4 = new AnimState("die");
    state1.NextState = initialState;
    state2.NextState = initialState;
    state3.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Cast", state1);
    animator.AddAnyState("Attack", state2);
    animator.AddAnyState("Dead", state4);
    animator.AddAnyState("Hit", state3);
    return animator;
  }
}
