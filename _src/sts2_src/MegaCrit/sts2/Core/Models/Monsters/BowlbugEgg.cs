// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.BowlbugEgg
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
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
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class BowlbugEgg : MonsterModel
{
  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 23, 21);
  }

  public override int MaxInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 24, 22);
  }

  private int BiteDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 8, 7);

  private int ProtectBlock
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 8, 7);
  }

  public override string DeathSfx => "event:/sfx/enemy/enemy_attacks/workbug_egg/workbug_egg_die";

  protected override string AttackSfx
  {
    get => "event:/sfx/enemy/enemy_attacks/workbug_egg/workbug_egg_attack";
  }

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Insect;

  public override void SetupSkins(MegaSprite spine, MegaSkeleton skeleton)
  {
    skeleton.SetSkin(skeleton.GetData().FindSkin("cocoon"));
    skeleton.SetSlotsToSetupPose();
    Node nodeOrNull = spine.BoundObject is Node boundObject ? boundObject.GetNodeOrNull(NodePath.op_Implicit("CocoonSlotNode/Cocoon")) : (Node) null;
    if (nodeOrNull == null)
      return;
    MegaSprite megaSprite = new MegaSprite(Variant.op_Implicit((GodotObject) nodeOrNull));
    MegaSkeleton skeleton1 = megaSprite.GetSkeleton();
    if (skeleton1 == null)
      return;
    skeleton1.SetSkin(skeleton1.GetData().FindSkin("egg1"));
    megaSprite.GetAnimationState().SetAnimation("egg_idle_loop");
    skeleton1.SetSlotsToSetupPose();
  }

  public override async Task AfterAddedToRoom() => await base.AfterAddedToRoom();

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("BITE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BiteMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.BiteDamage),
      (AbstractIntent) new DefendIntent()
    });
    initialState.FollowUpState = (MonsterState) initialState;
    states.Add((MonsterState) initialState);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task BiteMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.BiteDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.3f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
    Decimal num = await CreatureCmd.GainBlock(this.Creature, (Decimal) this.ProtectBlock, ValueProp.Move, (CardPlay) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("spit");
    AnimState state2 = new AnimState("attack");
    AnimState state3 = new AnimState("hurt");
    AnimState state4 = new AnimState("die");
    state1.NextState = initialState;
    state2.NextState = initialState;
    state3.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Dead", state4);
    animator.AddAnyState("Hit", state3);
    animator.AddAnyState("Cast", state1);
    animator.AddAnyState("Attack", state2);
    return animator;
  }
}
