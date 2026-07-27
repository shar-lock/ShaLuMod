// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.AssassinRubyRaider
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
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class AssassinRubyRaider : MonsterModel
{
  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 19, 18);
  }

  public override int MaxInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 24, 23);
  }

  private static int KillshotDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 11, 10);
  }

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Armor;

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("KILLSHOT_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.KillshotMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(AssassinRubyRaider.KillshotDamage)
    });
    initialState.FollowUpState = (MonsterState) initialState;
    states.Add((MonsterState) initialState);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task KillshotMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) AssassinRubyRaider.KillshotDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.5f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }
}
