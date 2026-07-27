// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.LouseProgenitor
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

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
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class LouseProgenitor : MonsterModel
{
  public const string curlTrigger = "Curl";
  private const string _uncurlTrigger = "Uncurl";
  private const string _webTrigger = "Web";
  private const string _webSfx = "event:/sfx/enemy/enemy_attacks/giant_louse/giant_louse_attack_web";
  public const string curlSfx = "event:/sfx/enemy/enemy_attacks/giant_louse/giant_louse_curl";
  private const string _uncurlSfx = "event:/sfx/enemy/enemy_attacks/giant_louse/giant_louse_uncurl";
  private bool _curled;
  private const int _webFrail = 2;
  private const int _growStrength = 5;

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 138, 134);
  }

  public override int MaxInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 141, 136);
  }

  protected override string AttackSfx
  {
    get => "event:/sfx/enemy/enemy_attacks/giant_louse/giant_louse_attack";
  }

  public override string DeathSfx => "event:/sfx/enemy/enemy_attacks/giant_louse/giant_louse_die";

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Insect;

  public bool Curled
  {
    get => this._curled;
    set
    {
      this.AssertMutable();
      this._curled = value;
    }
  }

  private int WebDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 10, 9);

  private int PounceDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 16 /*0x10*/, 14);
  }

  private int CurlBlock => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 18, 14);

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    CurlUpPower curlUpPower = await PowerCmd.Apply<CurlUpPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, (Decimal) this.CurlBlock, this.Creature, (CardModel) null);
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("WEB_CANNON_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.WebMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.WebDamage),
      (AbstractIntent) new DebuffIntent()
    });
    MoveState moveState1 = new MoveState("POUNCE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.PounceMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.PounceDamage)
    });
    MoveState moveState2 = new MoveState("CURL_AND_GROW_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.CurlAndGrowMove), new AbstractIntent[2]
    {
      (AbstractIntent) new DefendIntent(),
      (AbstractIntent) new BuffIntent()
    });
    initialState.FollowUpState = (MonsterState) moveState2;
    moveState2.FollowUpState = (MonsterState) moveState1;
    moveState1.FollowUpState = (MonsterState) initialState;
    states.Add((MonsterState) moveState2);
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) moveState1);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task WebMove(IReadOnlyList<Creature> targets)
  {
    if (this.Curled)
    {
      SfxCmd.Play("event:/sfx/enemy/enemy_attacks/giant_louse/giant_louse_uncurl");
      await CreatureCmd.TriggerAnim(this.Creature, "Uncurl", 0.9f);
      this.Curled = false;
    }
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.WebDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Web", 0.2f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/giant_louse/giant_louse_attack_web").WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
    IReadOnlyList<FrailPower> frailPowerList = await PowerCmd.Apply<FrailPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) targets, 2M, this.Creature, (CardModel) null);
  }

  private async Task CurlAndGrowMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/giant_louse/giant_louse_curl");
    await CreatureCmd.TriggerAnim(this.Creature, "Curl", 0.25f);
    Decimal num = await CreatureCmd.GainBlock(this.Creature, (Decimal) this.CurlBlock, ValueProp.Move, (CardPlay) null);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 5M, this.Creature, (CardModel) null);
    this.Curled = true;
  }

  private async Task PounceMove(IReadOnlyList<Creature> targets)
  {
    if (this.Curled)
    {
      SfxCmd.Play("event:/sfx/enemy/enemy_attacks/giant_louse/giant_louse_uncurl");
      await CreatureCmd.TriggerAnim(this.Creature, "Uncurl", 0.9f);
      this.Curled = false;
    }
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.PounceDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.2f).WithAttackerFx(sfx: this.AttackSfx).Execute((PlayerChoiceContext) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("curl");
    AnimState state2 = new AnimState("uncurl");
    AnimState animState = new AnimState("curled_loop", true);
    AnimState state3 = new AnimState("attack");
    AnimState state4 = new AnimState("attack_web");
    AnimState state5 = new AnimState("hurt");
    AnimState state6 = new AnimState("die");
    AnimState state7 = new AnimState("die_curled");
    initialState.AddBranch("Curl", state1);
    initialState.AddBranch("Attack", state3);
    initialState.AddBranch("Web", state4);
    initialState.AddBranch("Hit", state5);
    initialState.AddBranch("Dead", state6);
    state1.NextState = animState;
    animState.AddBranch("Uncurl", state2);
    animState.AddBranch("Dead", state7);
    state3.NextState = initialState;
    state3.AddBranch("Hit", state5);
    state3.AddBranch("Dead", state6);
    state4.NextState = initialState;
    state4.AddBranch("Hit", state5);
    state4.AddBranch("Dead", state6);
    state5.NextState = initialState;
    state5.AddBranch("Hit", state5);
    state5.AddBranch("Dead", state6);
    state5.AddBranch("Curl", state1);
    state2.NextState = initialState;
    return new CreatureAnimator(initialState, controller);
  }
}
