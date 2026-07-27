// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.TwigSlimeM
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
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
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class TwigSlimeM : MonsterModel
{
  private const int _stickyAmount = 1;

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 27, 26);
  }

  public override int MaxInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 29, 28);
  }

  protected override bool HasPhobiaSpineSkin => true;

  private int ClumpDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 12, 11);
  }

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Slime;

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState state = new MoveState("POKEY_POUNCE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ClumpShotMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.ClumpDamage)
    });
    MoveState moveState = new MoveState("STICKY_SHOT_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.StickyShotMove), new AbstractIntent[1]
    {
      (AbstractIntent) new StatusIntent(1)
    });
    RandomBranchState randomBranchState = new RandomBranchState("RAND");
    state.FollowUpState = (MonsterState) randomBranchState;
    moveState.FollowUpState = (MonsterState) randomBranchState;
    randomBranchState.AddBranch((MonsterState) state, 2);
    randomBranchState.AddBranch((MonsterState) moveState, MoveRepeatType.CannotRepeat);
    states.Add((MonsterState) state);
    states.Add((MonsterState) moveState);
    states.Add((MonsterState) randomBranchState);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) moveState);
  }

  private async Task ClumpShotMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.ClumpDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.15f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_slime_impact").Execute((PlayerChoiceContext) null);
  }

  private async Task StickyShotMove(IReadOnlyList<Creature> targets)
  {
    if (TestMode.IsOff)
    {
      NCreature ncreature = (NCreature) null;
      foreach (Creature target in (IEnumerable<Creature>) targets)
      {
        NCreature creatureNode = target.GetCreatureNode();
        if (creatureNode != null && (ncreature == null || (double) ncreature.GlobalPosition.X > (double) creatureNode.GlobalPosition.X))
          ncreature = creatureNode;
      }
      NCreature creatureNode1 = this.Creature.GetCreatureNode();
      Node2D specialNode = creatureNode1?.GetSpecialNode<Node2D>("Visuals/SpitTarget");
      if (creatureNode1 != null && specialNode != null && ncreature != null)
        specialNode.GlobalPosition = new Vector2(ncreature.GlobalPosition.X, specialNode.GlobalPosition.Y);
    }
    SfxCmd.Play(this.CastSfx);
    await CreatureCmd.TriggerAnim(this.Creature, "Cast", 0.75f);
    VfxCmd.PlayOnCreatureCenters((IEnumerable<Creature>) targets, "vfx/vfx_slime_impact");
    await CardPileCmd.AddToCombatAndPreview<Slimed>((IEnumerable<Creature>) targets, PileType.Discard, 1, (Player) null);
  }
}
