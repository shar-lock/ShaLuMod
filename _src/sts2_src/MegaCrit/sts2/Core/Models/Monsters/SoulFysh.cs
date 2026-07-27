// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.SoulFysh
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Context;
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
using MegaCrit.Sts2.Core.Nodes.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class SoulFysh : MonsterModel
{
  private const string _soulFyshCustomTrackName = "soulfysh_progress";
  private const string _beckonCustomTrackName = "beckon";
  private const string _intangibleSfx = "event:/sfx/enemy/enemy_attacks/soul_fysh/soul_fysh_intangible";
  private const string _beckonSfx = "event:/sfx/enemy/enemy_attacks/soul_fysh/soul_fysh_beckon";
  private const string _waveSfx = "event:/sfx/enemy/enemy_attacks/soul_fysh/soul_fysh_wave";
  private const string _reappearSfx = "event:/sfx/enemy/enemy_attacks/soul_fysh/soul_fysh_reappear";
  private const string _attackBeckonTrigger = "AttackBeckon";
  private const string _intangibleStartTrigger = "IntangibleStart";
  private const string _attackDebuffTrigger = "AttackDebuffTrigger";
  private const string _beckonTrigger = "Beckon";
  private bool _isInvisible;

  public override string HurtSfx => "event:/sfx/enemy/enemy_attacks/soul_fysh/soul_fysh_hurt";

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 221, 211);
  }

  public override int MaxInitialHp => this.MinInitialHp;

  private int DeGasDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 17, 16 /*0x10*/);
  }

  private int ScreamDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 15, 13);
  }

  private int GazeDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 8, 7);

  private int BeckonMoveAmount => 2;

  private int GazeMoveAmount => 1;

  private int ScreamMoveAmount => 3;

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Magic;

  public bool IsInvisible
  {
    get => this._isInvisible;
    set
    {
      this.AssertMutable();
      this._isInvisible = value;
    }
  }

  public override Task AfterDeath(
    PlayerChoiceContext choiceContext,
    Creature creature,
    bool wasRemovalPrevented,
    float deathAnimLength)
  {
    if (creature != this.Creature)
      return Task.CompletedTask;
    NRunMusicController.Instance?.UpdateMusicParameter("soulfysh_progress", 5f);
    return Task.CompletedTask;
  }

  public override Task AfterCardChangedPilesLate(
    CardModel card,
    PileType oldPileType,
    AbstractModel? clonedBy)
  {
    if (LocalContext.GetMe(this.CombatState) != card.Owner || !(card is Beckon) || CombatManager.Instance.IsOverOrEnding)
      return Task.CompletedTask;
    NRunMusicController.Instance?.UpdateMusicParameter("beckon", (float) (card.Owner.PlayerCombatState.Hand.Cards.Any<CardModel>((Func<CardModel, bool>) (c => c is Beckon)) ? 1 : 0));
    return Task.CompletedTask;
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("BECKON_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BeckonMove), new AbstractIntent[1]
    {
      (AbstractIntent) new StatusIntent(this.BeckonMoveAmount)
    });
    MoveState moveState1 = new MoveState("DE_GAS_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.DeGasMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.DeGasDamage)
    });
    MoveState moveState2 = new MoveState("GAZE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.GazeMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.GazeDamage),
      (AbstractIntent) new StatusIntent(this.GazeMoveAmount)
    });
    MoveState moveState3 = new MoveState("FADE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.FadeMove), new AbstractIntent[1]
    {
      (AbstractIntent) new BuffIntent()
    });
    MoveState moveState4 = new MoveState("SCREAM_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ScreamMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.ScreamDamage),
      (AbstractIntent) new DebuffIntent()
    });
    initialState.FollowUpState = (MonsterState) moveState1;
    moveState1.FollowUpState = (MonsterState) moveState2;
    moveState2.FollowUpState = (MonsterState) moveState3;
    moveState3.FollowUpState = (MonsterState) moveState4;
    moveState4.FollowUpState = (MonsterState) initialState;
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) moveState1);
    states.Add((MonsterState) moveState2);
    states.Add((MonsterState) moveState4);
    states.Add((MonsterState) moveState3);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task BeckonMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/soul_fysh/soul_fysh_beckon");
    await CreatureCmd.TriggerAnim(this.Creature, "Beckon", 0.0f);
    await Cmd.Wait(0.3f);
    VfxCmd.PlayOnCreatureCenter(this.Creature, "vfx/vfx_spooky_scream");
    await Cmd.CustomScaledWait(0.0f, 0.3f);
    foreach (Creature target in (IEnumerable<Creature>) targets)
    {
      Player player = target.Player ?? target.PetOwner;
      CardPileAddResult[] statusCards = new CardPileAddResult[this.BeckonMoveAmount];
      CardModel card1 = (CardModel) this.CombatState.CreateCard<Beckon>(player);
      CardPileAddResult[] cardPileAddResultArray = statusCards;
      cardPileAddResultArray[0] = await CardPileCmd.AddGeneratedCardToCombat(card1, PileType.Draw, (Player) null, CardPilePosition.Random);
      cardPileAddResultArray = (CardPileAddResult[]) null;
      CardModel card2 = (CardModel) this.CombatState.CreateCard<Beckon>(player);
      cardPileAddResultArray = statusCards;
      cardPileAddResultArray[1] = await CardPileCmd.AddGeneratedCardToCombat(card2, PileType.Discard, (Player) null);
      cardPileAddResultArray = (CardPileAddResult[]) null;
      if (LocalContext.IsMe(player))
      {
        CardCmd.PreviewCardPileAdd((IReadOnlyList<CardPileAddResult>) statusCards);
        await Cmd.Wait(1f);
      }
      player = (Player) null;
      statusCards = (CardPileAddResult[]) null;
    }
  }

  private async Task GazeMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.GazeDamage).FromMonster((MonsterModel) this).WithAttackerAnim("AttackBeckon", 0.6f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/soul_fysh/soul_fysh_beckon").WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
    foreach (Creature target in (IEnumerable<Creature>) targets)
    {
      Player player = target.Player ?? target.PetOwner;
      CardPileAddResult[] statusCards = new CardPileAddResult[1];
      CardModel card = (CardModel) this.CombatState.CreateCard<Beckon>(player);
      CardPileAddResult[] cardPileAddResultArray = statusCards;
      cardPileAddResultArray[0] = await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Discard, (Player) null);
      cardPileAddResultArray = (CardPileAddResult[]) null;
      if (LocalContext.IsMe(player))
      {
        CardCmd.PreviewCardPileAdd((IReadOnlyList<CardPileAddResult>) statusCards);
        await Cmd.Wait(1f);
      }
      player = (Player) null;
      statusCards = (CardPileAddResult[]) null;
    }
  }

  private async Task DeGasMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.DeGasDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.45f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  private async Task ScreamMove(IReadOnlyList<Creature> targets)
  {
    this.IsInvisible = false;
    NRunMusicController.Instance?.UpdateMusicParameter("soulfysh_progress", 2f);
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.ScreamDamage).FromMonster((MonsterModel) this).WithAttackerAnim("AttackDebuffTrigger", 0.65f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/soul_fysh/soul_fysh_wave").WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
    IReadOnlyList<VulnerablePower> vulnerablePowerList = await PowerCmd.Apply<VulnerablePower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) targets, (Decimal) this.ScreamMoveAmount, this.Creature, (CardModel) null);
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/soul_fysh/soul_fysh_reappear");
  }

  private async Task FadeMove(IReadOnlyList<Creature> targets)
  {
    this.IsInvisible = true;
    NRunMusicController.Instance?.UpdateMusicParameter("soulfysh_progress", 1f);
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/soul_fysh/soul_fysh_intangible");
    await CreatureCmd.TriggerAnim(this.Creature, "IntangibleStart", 0.8f);
    IntangiblePower intangiblePower = await PowerCmd.Apply<IntangiblePower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 2M, this.Creature, (CardModel) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("cast");
    AnimState state2 = new AnimState("attack_heavy");
    AnimState state3 = new AnimState("attack_beckon");
    AnimState state4 = new AnimState("hurt");
    AnimState state5 = new AnimState("die");
    AnimState state6 = new AnimState("beckon");
    AnimState animState1 = new AnimState("intangible_loop", true);
    AnimState state7 = new AnimState("intangible_start");
    AnimState animState2 = new AnimState("intangible_end");
    AnimState state8 = new AnimState("hurt_intangible");
    AnimState state9 = new AnimState("die_intangible");
    AnimState state10 = new AnimState("attack_debuff");
    state1.NextState = initialState;
    state2.NextState = initialState;
    state3.NextState = initialState;
    state4.NextState = initialState;
    state6.NextState = initialState;
    state7.NextState = animState1;
    state8.NextState = animState1;
    state10.NextState = animState2;
    animState2.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Cast", state1);
    animator.AddAnyState("Attack", state2);
    animator.AddAnyState("AttackBeckon", state3);
    animator.AddAnyState("Beckon", state6);
    animator.AddAnyState("IntangibleStart", state7);
    animator.AddAnyState("AttackDebuffTrigger", state10);
    animator.AddAnyState("Dead", state5, (Func<bool>) (() => !this.IsInvisible));
    animator.AddAnyState("Hit", state4, (Func<bool>) (() => !this.IsInvisible));
    animator.AddAnyState("Dead", state9, (Func<bool>) (() => this.IsInvisible));
    animator.AddAnyState("Hit", state8, (Func<bool>) (() => this.IsInvisible));
    return animator;
  }
}
