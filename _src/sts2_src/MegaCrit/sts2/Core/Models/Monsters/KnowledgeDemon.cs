// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.KnowledgeDemon
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class KnowledgeDemon : MonsterModel
{
  private const string _knowledgeDemonCustomTrackName = "knowledge_demon_progress";
  private const string _clapSfx = "event:/sfx/enemy/enemy_attacks/knowledge_demon/knowledge_demon_clap";
  private const string _flameSfx = "event:/sfx/enemy/enemy_attacks/knowledge_demon/knowledge_demon_flame";
  private const string _slapSfx = "event:/sfx/enemy/enemy_attacks/knowledge_demon/knowledge_demon_slap";
  private static readonly LocString _curseOfKnowledgeStartLine = MonsterModel.L10NMonsterLookup("KNOWLEDGE_DEMON.moves.CURSE_OF_KNOWLEDGE.startLine");
  private static readonly LocString _curseOfKnowledgeDoneLine = MonsterModel.L10NMonsterLookup("KNOWLEDGE_DEMON.moves.CURSE_OF_KNOWLEDGE.doneLine");
  private static readonly int[] _disintegrationDamageValues = new int[3]
  {
    6,
    7,
    8
  };
  private static readonly IReadOnlyList<IReadOnlyList<KnowledgeDemon.IChoosable>> _curseOfKnowledgeSets = (IReadOnlyList<IReadOnlyList<KnowledgeDemon.IChoosable>>) new \u003C\u003Ez__ReadOnlyArray<IReadOnlyList<KnowledgeDemon.IChoosable>>(new IReadOnlyList<KnowledgeDemon.IChoosable>[3]
  {
    (IReadOnlyList<KnowledgeDemon.IChoosable>) new \u003C\u003Ez__ReadOnlyArray<KnowledgeDemon.IChoosable>(new KnowledgeDemon.IChoosable[2]
    {
      (KnowledgeDemon.IChoosable) ModelDb.Card<Disintegration>(),
      (KnowledgeDemon.IChoosable) ModelDb.Card<MindRot>()
    }),
    (IReadOnlyList<KnowledgeDemon.IChoosable>) new \u003C\u003Ez__ReadOnlyArray<KnowledgeDemon.IChoosable>(new KnowledgeDemon.IChoosable[2]
    {
      (KnowledgeDemon.IChoosable) ModelDb.Card<Disintegration>(),
      (KnowledgeDemon.IChoosable) ModelDb.Card<Sloth>()
    }),
    (IReadOnlyList<KnowledgeDemon.IChoosable>) new \u003C\u003Ez__ReadOnlyArray<KnowledgeDemon.IChoosable>(new KnowledgeDemon.IChoosable[2]
    {
      (KnowledgeDemon.IChoosable) ModelDb.Card<Disintegration>(),
      (KnowledgeDemon.IChoosable) ModelDb.Card<WasteAway>()
    })
  });
  private int _curseOfKnowledgeCounter;
  private const int _knowledgeOverwhelmingRepeat = 3;
  private const int _ponderHeal = 30;
  private bool _isBurnt;
  private const string _mindRotTrigger = "MindRotTrigger";
  private const string _lightAttackTrigger = "LightAttackTrigger";
  private const string _mediumAttackTrigger = "MediumAttackTrigger";
  private const string _heavyAttackTrigger = "HeavyAttackTrigger";
  private const string _healTrigger = "HealTrigger";

  private int CurseOfKnowledgeCounter
  {
    get => this._curseOfKnowledgeCounter;
    set
    {
      this.AssertMutable();
      this._curseOfKnowledgeCounter = value;
    }
  }

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 399, 379);
  }

  public override int MaxInitialHp => this.MinInitialHp;

  private int SlapDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 18, 17);
  }

  private int PonderDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 13, 11);
  }

  private int KnowledgeOverwhelmingDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 9, 8);
  }

  private int PonderStrength
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 2);
  }

  public bool IsBurnt
  {
    get => this._isBurnt;
    set
    {
      this.AssertMutable();
      this._isBurnt = value;
    }
  }

  public override void BeforeRemovedFromRoom()
  {
    NRunMusicController.Instance?.UpdateMusicParameter("knowledge_demon_progress", 5f);
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState moveState1 = new MoveState("CURSE_OF_KNOWLEDGE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.CurseOfKnowledgeMove), new AbstractIntent[1]
    {
      (AbstractIntent) new DebuffIntent()
    });
    MoveState move = new MoveState("SLAP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SlapMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.SlapDamage)
    });
    MoveState moveState2 = new MoveState("KNOWLEDGE_OVERWHELMING_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.KnowledgeOverwhelmingMove), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(this.KnowledgeOverwhelmingDamage, 3)
    });
    MoveState moveState3 = new MoveState("PONDER_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.PonderMove), new AbstractIntent[3]
    {
      (AbstractIntent) new SingleAttackIntent(this.PonderDamage),
      (AbstractIntent) new HealIntent(),
      (AbstractIntent) new BuffIntent()
    });
    ConditionalBranchState conditionalBranchState = new ConditionalBranchState("CurseOfKnowledgeBranch");
    moveState1.FollowUpState = (MonsterState) move;
    move.FollowUpState = (MonsterState) moveState2;
    moveState2.FollowUpState = (MonsterState) moveState3;
    moveState3.FollowUpState = (MonsterState) conditionalBranchState;
    conditionalBranchState.AddState((MonsterState) moveState1, (Func<bool>) (() => this._curseOfKnowledgeCounter < 3));
    conditionalBranchState.AddState((MonsterState) move, (Func<bool>) (() => this._curseOfKnowledgeCounter >= 3));
    states.Add((MonsterState) conditionalBranchState);
    states.Add((MonsterState) moveState1);
    states.Add((MonsterState) move);
    states.Add((MonsterState) moveState3);
    states.Add((MonsterState) moveState2);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) moveState1);
  }

  private async Task CurseOfKnowledgeMove(IReadOnlyList<Creature> targets)
  {
    if (this.CurseOfKnowledgeCounter >= KnowledgeDemon._curseOfKnowledgeSets.Count)
      throw new InvalidOperationException($"There are no valid sets at this index {this.CurseOfKnowledgeCounter}");
    TalkCmd.Play(KnowledgeDemon._curseOfKnowledgeStartLine, this.Creature, VfxColor.Gold, VfxDuration.Standard);
    await CreatureCmd.TriggerAnim(this.Creature, "MindRotTrigger", 1f);
    List<Task> taskList = new List<Task>();
    foreach (Creature target in (IEnumerable<Creature>) targets)
      taskList.Add(this.ChooseCurse(target));
    await Task.WhenAll((IEnumerable<Task>) taskList);
    TalkCmd.Play(KnowledgeDemon._curseOfKnowledgeDoneLine, this.Creature, VfxColor.Gold, VfxDuration.Standard);
    if (!this.CombatState.IsLiveCombat())
      return;
    this.CurseOfKnowledgeCounter++;
  }

  private async Task ChooseCurse(Creature target)
  {
    if (target.IsDead)
      return;
    int disintegrationDamage = KnowledgeDemon._disintegrationDamageValues[this.CurseOfKnowledgeCounter];
    CardModel cardModel = await CardSelectCmd.FromChooseACardScreen((PlayerChoiceContext) new BlockingPlayerChoiceContext(), (IReadOnlyList<CardModel>) KnowledgeDemon._curseOfKnowledgeSets[this.CurseOfKnowledgeCounter].Select<KnowledgeDemon.IChoosable, CardModel>((Func<KnowledgeDemon.IChoosable, CardModel>) (c =>
    {
      CardModel card = this.CombatState.CreateCard((CardModel) c, target.Player);
      if (card is Disintegration)
        card.DynamicVars["DisintegrationPower"].BaseValue = (Decimal) disintegrationDamage;
      return card;
    })).ToList<CardModel>(), target.Player);
    if (cardModel == null)
      return;
    await ((KnowledgeDemon.IChoosable) cardModel).OnChosen();
  }

  private async Task SlapMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.SlapDamage).FromMonster((MonsterModel) this).WithAttackerAnim("MediumAttackTrigger", 0.5f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/knowledge_demon/knowledge_demon_slap").WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3").Execute((PlayerChoiceContext) null);
  }

  private async Task KnowledgeOverwhelmingMove(IReadOnlyList<Creature> targets)
  {
    this.IsBurnt = true;
    NRunMusicController.Instance?.UpdateMusicParameter("knowledge_demon_progress", 1f);
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.KnowledgeOverwhelmingDamage).WithHitCount(3).FromMonster((MonsterModel) this).WithAttackerAnim("HeavyAttackTrigger", 0.85f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/knowledge_demon/knowledge_demon_clap").OnlyPlayAnimOnce().WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3").Execute((PlayerChoiceContext) null);
  }

  private async Task PonderMove(IReadOnlyList<Creature> targets)
  {
    await CreatureCmd.TriggerAnim(this.Creature, "HealTrigger", 1.8f);
    NRunMusicController.Instance?.UpdateMusicParameter("knowledge_demon_progress", 2f);
    this.IsBurnt = false;
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.PonderDamage).FromMonster((MonsterModel) this).WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3").Execute((PlayerChoiceContext) null);
    await CreatureCmd.Heal(this.Creature, (Decimal) (30 * this.CombatState.Players.Count));
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, (Decimal) this.PonderStrength, this.Creature, (CardModel) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("attack_light");
    AnimState state2 = new AnimState("attack_medium");
    AnimState state3 = new AnimState("attack_heavy");
    AnimState state4 = new AnimState("brain_rot");
    AnimState state5 = new AnimState("heal");
    AnimState animState = new AnimState("burnt_loop", true);
    AnimState state6 = new AnimState("hurt");
    AnimState state7 = new AnimState("die");
    AnimState state8 = new AnimState("hurt_burnt");
    AnimState state9 = new AnimState("die_burnt");
    state1.NextState = initialState;
    state2.NextState = initialState;
    state3.NextState = animState;
    state4.NextState = initialState;
    state5.NextState = initialState;
    state6.NextState = initialState;
    state8.NextState = animState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("LightAttackTrigger", state1);
    animator.AddAnyState("MediumAttackTrigger", state2);
    animator.AddAnyState("HeavyAttackTrigger", state3);
    animator.AddAnyState("MindRotTrigger", state4);
    animator.AddAnyState("HealTrigger", state5);
    animator.AddAnyState("Dead", state7, (Func<bool>) (() => !this._isBurnt));
    animator.AddAnyState("Hit", state6, (Func<bool>) (() => !this._isBurnt));
    animator.AddAnyState("Dead", state9, (Func<bool>) (() => this._isBurnt));
    animator.AddAnyState("Hit", state8, (Func<bool>) (() => this._isBurnt));
    return animator;
  }

  public interface IChoosable
  {
    Task OnChosen();
  }
}
