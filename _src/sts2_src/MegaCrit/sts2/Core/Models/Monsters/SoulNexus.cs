// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.SoulNexus
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

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
using MegaCrit.Sts2.Core.Nodes.Rooms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class SoulNexus : MonsterModel
{
  private const string _maelstromMove = "MAELSTROM_MOVE";

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 254, 234);
  }

  public override int MaxInitialHp => this.MinInitialHp;

  private int SoulBurnDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 31 /*0x1F*/, 29);
  }

  private int MaelstromDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 7, 6);
  }

  private int MaelstromRepeat
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 4);
  }

  private int DrainLifeDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 19, 18);
  }

  public override bool ShouldFadeAfterDeath => false;

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Magic;

  public override void SetupSkins(MegaSprite spine, MegaSkeleton skeleton)
  {
    spine.GetAnimationState().SetAnimation("tracks/writhe", trackId: 1);
  }

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    this.Creature.Died += new Action<Creature>(this.AfterDeath);
  }

  private void AfterDeath(Creature _)
  {
    this.Creature.Died -= new Action<Creature>(this.AfterDeath);
    NCombatRoom.Instance.GetCreatureNode(this.Creature)?.SpineAnimation.SetAnimation("tracks/empty", track: 1);
  }

  public override void BeforeRemovedFromRoom()
  {
    if (this.CombatState.RunState.IsGameOver)
      return;
    NCombatRoom.Instance?.GetCreatureNode(this.Creature)?.SpineAnimation.SetAnimation("tracks/empty", track: 1);
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState moveState = new MoveState("SOUL_BURN_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SoulBurnMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.SoulBurnDamage)
    });
    MoveState state1 = new MoveState("MAELSTROM_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.MaelstromMove), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(this.MaelstromDamage, this.MaelstromRepeat)
    });
    MoveState state2 = new MoveState("DRAIN_LIFE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.DrainLifeMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.DrainLifeDamage),
      (AbstractIntent) new DebuffIntent(true)
    });
    RandomBranchState randomBranchState = new RandomBranchState("RAND");
    moveState.FollowUpState = (MonsterState) randomBranchState;
    state1.FollowUpState = (MonsterState) randomBranchState;
    state2.FollowUpState = (MonsterState) randomBranchState;
    randomBranchState.AddBranch((MonsterState) moveState, MoveRepeatType.CannotRepeat, 1f);
    randomBranchState.AddBranch((MonsterState) state1, MoveRepeatType.CannotRepeat, 1f);
    randomBranchState.AddBranch((MonsterState) state2, MoveRepeatType.CannotRepeat, 1f);
    states.Add((MonsterState) moveState);
    states.Add((MonsterState) state1);
    states.Add((MonsterState) state2);
    states.Add((MonsterState) randomBranchState);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) moveState);
  }

  private async Task SoulBurnMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.SoulBurnDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.6f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  private async Task MaelstromMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.MaelstromDamage).WithHitCount(this.MaelstromRepeat).FromMonster((MonsterModel) this).OnlyPlayAnimOnce().WithAttackerAnim("Attack", 0.2f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  private async Task DrainLifeMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.DrainLifeDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Cast", 1f).WithAttackerFx(sfx: this.CastSfx).Execute((PlayerChoiceContext) null);
    IReadOnlyList<VulnerablePower> vulnerablePowerList = await PowerCmd.Apply<VulnerablePower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) targets, 2M, this.Creature, (CardModel) null);
    IReadOnlyList<WeakPower> weakPowerList = await PowerCmd.Apply<WeakPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) targets, 2M, this.Creature, (CardModel) null);
  }

  protected override bool ShouldShowMoveInBestiary(string moveStateId)
  {
    return moveStateId != "MAELSTROM_MOVE";
  }
}
