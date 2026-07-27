// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.TrackerRubyRaider
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
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class TrackerRubyRaider : MonsterModel
{
  private const string _trackMove = "TRACK_MOVE";

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 22, 21);
  }

  public override int MaxInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 26, 25);
  }

  private int HoundsDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 1, 1);
  }

  private int HoundsRepeat
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 9, 8);
  }

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Armor;

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("TRACK_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.TrackMove), new AbstractIntent[1]
    {
      (AbstractIntent) new DebuffIntent()
    });
    MoveState moveState = new MoveState("HOUNDS_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.HoundsMove), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(this.HoundsDamage, this.HoundsRepeat)
    });
    initialState.FollowUpState = (MonsterState) moveState;
    moveState.FollowUpState = (MonsterState) moveState;
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) moveState);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task TrackMove(IReadOnlyList<Creature> targets)
  {
    await CreatureCmd.TriggerAnim(this.Creature, "Attack", 0.8f);
    VfxCmd.PlayOnCreatureCenters((IEnumerable<Creature>) targets, "vfx/vfx_attack_slash");
    IReadOnlyList<FrailPower> frailPowerList = await PowerCmd.Apply<FrailPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) targets, 2M, this.Creature, (CardModel) null);
  }

  private async Task HoundsMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.HoundsDamage).WithHitCount(this.HoundsRepeat).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.5f).OnlyPlayAnimOnce().WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  protected override bool ShouldShowMoveInBestiary(string moveStateId)
  {
    return moveStateId != "TRACK_MOVE";
  }
}
