// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.Chomper
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class Chomper : MonsterModel
{
  public const string screechMoveId = "SCREECH_MOVE";
  private const int _screechStatusCount = 3;
  private const int _clampRepeat = 2;
  private bool _screamFirst;

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 63 /*0x3F*/, 60);
  }

  public override int MaxInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 67, 64 /*0x40*/);
  }

  private static int ClampDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 9, 8);
  }

  public bool ScreamFirst
  {
    get => this._screamFirst;
    set
    {
      this.AssertMutable();
      this._screamFirst = value;
    }
  }

  public override string TakeDamageSfx => "event:/sfx/enemy/enemy_attacks/chomper/chomper_hurt";

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    ArtifactPower artifactPower = await PowerCmd.Apply<ArtifactPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 2M, this.Creature, (CardModel) null);
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState moveState1 = new MoveState("CLAMP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ClampMove), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(Chomper.ClampDamage, 2)
    });
    MoveState moveState2 = new MoveState("SCREECH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ScreechMove), new AbstractIntent[1]
    {
      (AbstractIntent) new StatusIntent(3)
    });
    moveState1.FollowUpState = (MonsterState) moveState2;
    moveState2.FollowUpState = (MonsterState) moveState1;
    states.Add((MonsterState) moveState1);
    states.Add((MonsterState) moveState2);
    MoveState initialState = this._screamFirst ? moveState2 : moveState1;
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task ClampMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) Chomper.ClampDamage).WithHitCount(2).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.3f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  private async Task ScreechMove(IReadOnlyList<Creature> targets)
  {
    TalkCmd.Play(MonsterModel.L10NMonsterLookup("CHOMPER.moves.SCREECH.title"), this.Creature, VfxColor.Cyan);
    SfxCmd.Play(this.CastSfx);
    await CreatureCmd.TriggerAnim(this.Creature, "Cast", 1f);
    await CardPileCmd.AddToCombatAndPreview<Dazed>((IEnumerable<Creature>) targets, PileType.Discard, 3, (Player) null);
  }
}
