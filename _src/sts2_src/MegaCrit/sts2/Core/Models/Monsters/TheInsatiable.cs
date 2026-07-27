// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.TheInsatiable
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.Combat;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class TheInsatiable : MonsterModel
{
  private const int _liquifyStatusDrawCount = 3;
  private const int _liquifyStatusDiscardCount = 3;
  private const int _thrashRepeat = 2;
  private const string _liquifySandTrigger = "LiquifySand";
  private const string _salivateTrigger = "Salivate";
  private const string _biteTrigger = "Bite";
  private const string _thrashTrigger = "Thrash";
  public const string eatPlayerTrigger = "EatPlayerTrigger";
  public const string finisherSfx = "event:/sfx/enemy/enemy_attacks/the_insatiable/the_insatiable_finisher";
  private const string _liquifyGroundSfx = "event:/sfx/enemy/enemy_attacks/the_insatiable/the_insatiable_liquify_ground";
  private const string _lungingBiteSfx = "event:/sfx/enemy/enemy_attacks/the_insatiable/the_insatiable_lunging_bite";
  private const string _salivateSfx = "event:/sfx/enemy/enemy_attacks/the_insatiable/the_insatiable_salivate";
  private const string _thrashSfx = "event:/sfx/enemy/enemy_attacks/the_insatiable/the_insatiable_thrash";
  private bool _hasLiquified;

  public static string TheInsatiableTrackName => "insatiable_progress";

  public static string EatPlayerAnim => "eat_player";

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 341, 321);
  }

  public override int MaxInitialHp => this.MinInitialHp;

  private int ThrashDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 9, 8);
  }

  private int BiteDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 31 /*0x1F*/, 28);
  }

  private int SalivateStrength
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 2);
  }

  private bool HasLiquified
  {
    get => this._hasLiquified;
    set
    {
      this.AssertMutable();
      this._hasLiquified = value;
    }
  }

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Insect;

  public override Task AfterDeath(
    PlayerChoiceContext choiceContext,
    Creature creature,
    bool wasRemovalPrevented,
    float deathAnimLength)
  {
    if (creature != this.Creature)
      return Task.CompletedTask;
    NRunMusicController.Instance?.UpdateMusicParameter(TheInsatiable.TheInsatiableTrackName, 10f);
    return Task.CompletedTask;
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("LIQUIFY_GROUND_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.LiquifyMove), new AbstractIntent[2]
    {
      (AbstractIntent) new BuffIntent(),
      (AbstractIntent) new StatusIntent(6)
    });
    MoveState moveState1 = new MoveState("THRASH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ThrashMove), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(this.ThrashDamage, 2)
    });
    MoveState moveState2 = new MoveState("THRASH_MOVE_2", new Func<IReadOnlyList<Creature>, Task>(this.ThrashMove), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(this.ThrashDamage, 2)
    });
    MoveState moveState3 = new MoveState("LUNGING_BITE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BiteMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.BiteDamage)
    });
    MoveState moveState4 = new MoveState("SALIVATE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SalivateMove), new AbstractIntent[1]
    {
      (AbstractIntent) new BuffIntent()
    });
    initialState.FollowUpState = (MonsterState) moveState1;
    moveState1.FollowUpState = (MonsterState) moveState3;
    moveState3.FollowUpState = (MonsterState) moveState4;
    moveState4.FollowUpState = (MonsterState) moveState2;
    moveState2.FollowUpState = (MonsterState) moveState1;
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) moveState3);
    states.Add((MonsterState) moveState1);
    states.Add((MonsterState) moveState2);
    states.Add((MonsterState) moveState4);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task LiquifyMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/the_insatiable/the_insatiable_liquify_ground");
    await CreatureCmd.TriggerAnim(this.Creature, "LiquifySand", 0.0f);
    await Cmd.Wait(0.5f);
    VfxCmd.PlayOnCreatureCenter(this.Creature, "vfx/vfx_scream");
    await Cmd.Wait(0.75f);
    foreach (Creature target in (IEnumerable<Creature>) targets)
    {
      SandpitPower mutable = (SandpitPower) ModelDb.Power<SandpitPower>().ToMutable();
      mutable.Target = target;
      await PowerCmd.Apply((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (PowerModel) mutable, this.Creature, 4M, this.Creature, (CardModel) null);
    }
    foreach (Creature target in (IEnumerable<Creature>) targets)
    {
      Player player = target.Player ?? target.PetOwner;
      List<CardPileAddResult> statusCards = new List<CardPileAddResult>();
      for (int i = 0; i < 6; ++i)
      {
        CardModel card = (CardModel) this.CombatState.CreateCard<FranticEscape>(player);
        PileType newPileType = i < 3 ? PileType.Draw : PileType.Discard;
        List<CardPileAddResult> cardPileAddResultList = statusCards;
        cardPileAddResultList.Add(await CardPileCmd.AddGeneratedCardToCombat(card, newPileType, (Player) null, CardPilePosition.Random));
        cardPileAddResultList = (List<CardPileAddResult>) null;
      }
      if (LocalContext.IsMe(player))
      {
        CardCmd.PreviewCardPileAdd((IReadOnlyList<CardPileAddResult>) statusCards);
        await Cmd.Wait(1f);
      }
      player = (Player) null;
      statusCards = (List<CardPileAddResult>) null;
    }
    this.HasLiquified = true;
  }

  private async Task ThrashMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.ThrashDamage).WithHitCount(2).FromMonster((MonsterModel) this).WithHitFx("vfx/vfx_scratch").WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/the_insatiable/the_insatiable_thrash").WithAttackerAnim("Thrash", 0.3f).OnlyPlayAnimOnce().Execute((PlayerChoiceContext) null);
  }

  private async Task BiteMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.BiteDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Bite", 0.25f).OnlyPlayAnimOnce().WithHitFx("vfx/vfx_bite").WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/the_insatiable/the_insatiable_lunging_bite").Execute((PlayerChoiceContext) null);
  }

  private async Task SalivateMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/the_insatiable/the_insatiable_salivate");
    await CreatureCmd.TriggerAnim(this.Creature, "Salivate", 0.5f);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, (Decimal) this.SalivateStrength, this.Creature, (CardModel) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("intro_loop", true);
    AnimState state1 = new AnimState("liquify_sand");
    AnimState animState = new AnimState("idle_loop", true);
    AnimState state2 = new AnimState("salivate");
    AnimState state3 = new AnimState("attack_thrash");
    AnimState state4 = new AnimState("attack_bite");
    AnimState state5 = new AnimState(TheInsatiable.EatPlayerAnim);
    AnimState state6 = new AnimState("intro_hurt");
    AnimState state7 = new AnimState("hurt");
    AnimState state8 = new AnimState("die");
    state1.NextState = animState;
    state2.NextState = animState;
    state3.NextState = animState;
    state4.NextState = animState;
    state7.NextState = animState;
    state5.NextState = animState;
    state6.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Cast", state2);
    animator.AddAnyState("Attack", state3);
    animator.AddAnyState("Dead", state8);
    animator.AddAnyState("EatPlayerTrigger", state5);
    animator.AddAnyState("LiquifySand", state1);
    animator.AddAnyState("Salivate", state2);
    animator.AddAnyState("Thrash", state3);
    animator.AddAnyState("Bite", state4);
    animator.AddAnyState("Hit", state6, (Func<bool>) (() => !this.HasLiquified));
    animator.AddAnyState("Hit", state7, (Func<bool>) (() => this.HasLiquified));
    return animator;
  }

  public override List<BestiaryMonsterMove> GenerateBestiaryMoveList(
    NCreatureVisuals? creatureVisuals)
  {
    List<BestiaryMonsterMove> bestiaryMoveList = base.GenerateBestiaryMoveList(creatureVisuals);
    bestiaryMoveList.Insert(4, BestiaryMonsterMove.FromAnim(new LocString("monsters", "THE_INSATIABLE.moves.DEVOUR.title"), TheInsatiable.EatPlayerAnim, "event:/sfx/enemy/enemy_attacks/the_insatiable/the_insatiable_finisher"));
    return bestiaryMoveList;
  }
}
