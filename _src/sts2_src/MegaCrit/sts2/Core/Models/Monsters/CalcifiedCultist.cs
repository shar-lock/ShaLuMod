// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.CalcifiedCultist
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

public sealed class CalcifiedCultist : MonsterModel
{
  private static readonly LocString _cawCawDialogue = new LocString("monsters", "CALCIFIED_CULTIST.moves.INCANTATION.banter");
  private const string _buffSfx = "event:/sfx/enemy/enemy_attacks/cultists/cultists_buff_calcified";
  private float _attackSfxStrength;

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 39, 38);
  }

  public override int MaxInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 42, 41);
  }

  private int DarkStrikeDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 11, 9);
  }

  private int IncantationAmount => 2;

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Fur;

  public override Vector2 ExtraDeathVfxPadding => new Vector2(1.5f, 1.2f);

  public override string DeathSfx
  {
    get => "event:/sfx/enemy/enemy_attacks/cultists/cultists_die_calcified";
  }

  protected override string AttackSfx => "event:/sfx/enemy/enemy_attacks/cultists/cultists_attack";

  private float AttackSfxStrength
  {
    get => this._attackSfxStrength;
    set
    {
      this.AssertMutable();
      this._attackSfxStrength = value;
    }
  }

  public override void SetupSkins(MegaSprite spine, MegaSkeleton skeleton)
  {
    MegaSkin skin = spine.NewSkin("custom-skin");
    MegaSkeletonDataResource data = skeleton.GetData();
    skin.AddSkin(data.FindSkin("coral"));
    skeleton.SetSkin(skin);
    skeleton.SetSlotsToSetupPose();
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("INCANTATION_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.IncantationMove), new AbstractIntent[1]
    {
      (AbstractIntent) new BuffIntent()
    });
    MoveState moveState = new MoveState("DARK_STRIKE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.DarkStrikeMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.DarkStrikeDamage)
    });
    initialState.FollowUpState = (MonsterState) moveState;
    moveState.FollowUpState = (MonsterState) moveState;
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) moveState);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task IncantationMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/cultists/cultists_buff_calcified");
    await CreatureCmd.TriggerAnim(this.Creature, "Cast", 0.5f);
    TalkCmd.Play(CalcifiedCultist._cawCawDialogue, this.Creature, VfxColor.Purple, VfxDuration.Standard);
    await Cmd.CustomScaledWait(0.25f, 0.5f);
    RitualPower ritualPower = await PowerCmd.Apply<RitualPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, (Decimal) this.IncantationAmount, this.Creature, (CardModel) null);
  }

  private async Task DarkStrikeMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.DarkStrikeDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.2f).BeforeDamage(new Func<Task>(this.PlayAttackSfx)).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("buff");
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

  private Task PlayAttackSfx()
  {
    SfxCmd.Play(this.AttackSfx, "enemy_strength", this.AttackSfxStrength);
    this.AttackSfxStrength += 0.2f;
    return Task.CompletedTask;
  }
}
