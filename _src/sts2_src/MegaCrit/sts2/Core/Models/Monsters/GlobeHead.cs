// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.GlobeHead
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

public sealed class GlobeHead : MonsterModel
{
  private const string _galvanicBurstMove = "GALVANIC_BURST";
  private const string _chargeSfx = "event:/sfx/enemy/enemy_attacks/globe_head/globe_head_charge";
  private const string _slapSfx = "event:/sfx/enemy/enemy_attacks/globe_head/globe_head_slap";
  private const int _thunderStrikeRepeat = 3;
  private const int _shockingSlapFrail = 2;
  private const int _galvanicBurstStr = 2;

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 158, 148);
  }

  public override int MaxInitialHp => this.MinInitialHp;

  private int ThunderStrikeDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 7, 6);
  }

  private int ShockingSlapDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 14, 13);
  }

  private int GalvanicBurstDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 17, 16 /*0x10*/);
  }

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Armor;

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    GalvanicPower galvanicPower = await PowerCmd.Apply<GalvanicPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 6M, this.Creature, (CardModel) null);
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState moveState1 = new MoveState("THUNDER_STRIKE", new Func<IReadOnlyList<Creature>, Task>(this.ThunderStrike), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(this.ThunderStrikeDamage, 3)
    });
    MoveState initialState = new MoveState("SHOCKING_SLAP", new Func<IReadOnlyList<Creature>, Task>(this.ShockingSlap), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.ShockingSlapDamage),
      (AbstractIntent) new DebuffIntent()
    });
    MoveState moveState2 = new MoveState("GALVANIC_BURST", new Func<IReadOnlyList<Creature>, Task>(this.GalvanicBurstMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.GalvanicBurstDamage),
      (AbstractIntent) new BuffIntent()
    });
    initialState.FollowUpState = (MonsterState) moveState1;
    moveState1.FollowUpState = (MonsterState) moveState2;
    moveState2.FollowUpState = (MonsterState) initialState;
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) moveState1);
    states.Add((MonsterState) moveState2);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task ThunderStrike(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.ThunderStrikeDamage).WithHitCount(3).FromMonster((MonsterModel) this).WithAttackerAnim("Cast", 0.5f).OnlyPlayAnimOnce().WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/globe_head/globe_head_charge").WithHitFx("vfx/vfx_attack_lightning").Execute((PlayerChoiceContext) null);
  }

  private async Task ShockingSlap(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.ShockingSlapDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.5f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/globe_head/globe_head_slap").WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
    IReadOnlyList<FrailPower> frailPowerList = await PowerCmd.Apply<FrailPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) targets, 2M, this.Creature, (CardModel) null);
  }

  private async Task GalvanicBurstMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.GalvanicBurstDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.5f).WithHitFx("vfx/vfx_attack_lightning", tmpSfx: "blunt_attack.mp3").Execute((PlayerChoiceContext) null);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 2M, this.Creature, (CardModel) null);
  }

  protected override bool ShouldShowMoveInBestiary(string moveStateId)
  {
    return moveStateId != "GALVANIC_BURST";
  }
}
