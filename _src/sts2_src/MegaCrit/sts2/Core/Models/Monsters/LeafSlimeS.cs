// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.LeafSlimeS
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.MonsterMoves;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class LeafSlimeS : MonsterModel
{
  private const int _goopAmount = 1;

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 12, 11);
  }

  public override int MaxInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 16 /*0x10*/, 15);
  }

  private int TackleDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);
  }

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Slime;

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState state1 = new MoveState("TACKLE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.TackleMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.TackleDamage)
    });
    MoveState state2 = new MoveState("GOOP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.GoopMove), new AbstractIntent[1]
    {
      (AbstractIntent) new StatusIntent(1)
    });
    RandomBranchState initialState = new RandomBranchState("RAND");
    state1.FollowUpState = (MonsterState) initialState;
    state2.FollowUpState = (MonsterState) initialState;
    initialState.AddBranch((MonsterState) state1, MoveRepeatType.CannotRepeat);
    initialState.AddBranch((MonsterState) state2, MoveRepeatType.CannotRepeat);
    states.Add((MonsterState) state1);
    states.Add((MonsterState) state2);
    states.Add((MonsterState) initialState);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task TackleMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.TackleDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.15f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_slime_impact").Execute((PlayerChoiceContext) null);
  }

  private async Task GoopMove(IReadOnlyList<Creature> targets)
  {
    await CreatureCmd.TriggerAnim(this.Creature, "Cast", 0.5f);
    SfxCmd.Play(this.AttackSfx);
    VfxCmd.PlayOnCreatureCenters((IEnumerable<Creature>) targets, "vfx/vfx_slime_impact");
    await CardPileCmd.AddToCombatAndPreview<Slimed>((IEnumerable<Creature>) targets, PileType.Discard, 1, (Player) null);
  }
}
