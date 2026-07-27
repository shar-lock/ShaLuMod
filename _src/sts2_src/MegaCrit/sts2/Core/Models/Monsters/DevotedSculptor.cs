// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.DevotedSculptor
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class DevotedSculptor : MonsterModel
{
  private static readonly LocString _forbiddenIncantationDialogue = new LocString("monsters", "DEVOTED_SCULPTOR.moves.FORBIDDEN_INCANTATION.banter");
  private readonly int _ritualGain = 9;

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 172, 162);
  }

  public override int MaxInitialHp => this.MinInitialHp;

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Fur;

  private int SavageDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 15, 12);
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("FORBIDDEN_INCANTATION_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ForbiddenIncantationMove), new AbstractIntent[1]
    {
      (AbstractIntent) new BuffIntent()
    });
    MoveState moveState = new MoveState("SAVAGE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SavageMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.SavageDamage)
    });
    initialState.FollowUpState = (MonsterState) moveState;
    moveState.FollowUpState = (MonsterState) moveState;
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) moveState);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task ForbiddenIncantationMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play(this.CastSfx);
    await CreatureCmd.TriggerAnim(this.Creature, "Cast", 0.0f);
    await Cmd.Wait(0.3f);
    VfxCmd.PlayOnCreatureCenter(this.Creature, "vfx/vfx_scream");
    TalkCmd.Play(DevotedSculptor._forbiddenIncantationDialogue, this.Creature, VfxColor.Blue);
    await Cmd.CustomScaledWait(0.75f, 1f);
    RitualPower ritualPower = await PowerCmd.Apply<RitualPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, (Decimal) this._ritualGain, (Creature) null, (CardModel) null);
  }

  private async Task SavageMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.SavageDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.3f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
  }
}
