// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.TwoTailedRat
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Animation;
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
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class TwoTailedRat : MonsterModel
{
  private const string _attackHandsSfx = "event:/sfx/enemy/enemy_attacks/two_tail_rats/two_tail_rats_attack_hands";
  private const string _summonSfx = "event:/sfx/enemy/enemy_attacks/two_tail_rats/two_tail_rats_summon";
  private const string _attackBite = "event:/sfx/enemy/enemy_attacks/two_tail_rats/two_tail_rats_attack_bite";
  private static readonly string[] _barnacleOptions = new string[3]
  {
    "barnacle1",
    "barnacle1",
    "barnacle3"
  };
  private static readonly string[] _headOptions = new string[3]
  {
    "head1",
    "head2",
    "head3"
  };
  private const string _callForBackupMoveId = "CALL_FOR_BACKUP_MOVE";
  private const float _callForBackupChance = 0.75f;
  private const int _callForBackupLimit = 3;
  private int _starterMoveIndex = -1;
  private int _turnsUntilSummonable = 2;
  private int _callForBackupCount;
  private const string _summonTrigger = "Summon";

  public override string DeathSfx
  {
    get => "event:/sfx/enemy/enemy_attacks/two_tail_rats/two_tail_rats_die";
  }

  public override string HurtSfx
  {
    get => "event:/sfx/enemy/enemy_attacks/two_tail_rats/two_tail_rats_hurt";
  }

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 18, 17);
  }

  public override int MaxInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 22, 21);
  }

  private int ScratchDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 9, 8);
  }

  private int DiseaseBiteDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 7, 6);
  }

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Slime;

  public int StarterMoveIndex
  {
    get => this._starterMoveIndex;
    set
    {
      this.AssertMutable();
      this._starterMoveIndex = value;
    }
  }

  private int TurnsUntilSummonable
  {
    get => this._turnsUntilSummonable;
    set
    {
      this.AssertMutable();
      this._turnsUntilSummonable = value;
    }
  }

  public int CallForBackupCount
  {
    get => this._callForBackupCount;
    set
    {
      this.AssertMutable();
      this._callForBackupCount = value;
    }
  }

  public override void SetupSkins(MegaSprite spine, MegaSkeleton skeleton)
  {
    MegaSkin skin = spine.NewSkin("custom-skin");
    MegaSkeletonDataResource data = skeleton.GetData();
    skin.AddSkin(data.FindSkin(Rng.Chaotic.NextItem<string>((IEnumerable<string>) TwoTailedRat._barnacleOptions)));
    skin.AddSkin(data.FindSkin(Rng.Chaotic.NextItem<string>((IEnumerable<string>) TwoTailedRat._headOptions)));
    skeleton.SetSkin(skin);
    skeleton.SetSlotsToSetupPose();
  }

  public override async Task AfterAddedToRoom() => await base.AfterAddedToRoom();

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState state1 = new MoveState("SCRATCH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ScratchMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.ScratchDamage)
    });
    MoveState state2 = new MoveState("DISEASE_BITE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.DiseaseBiteMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.DiseaseBiteDamage)
    });
    MoveState state3 = new MoveState("SCREECH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ScreechMove), new AbstractIntent[1]
    {
      (AbstractIntent) new DebuffIntent()
    });
    MoveState state4 = new MoveState("CALL_FOR_BACKUP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.CallForBackup), new AbstractIntent[1]
    {
      (AbstractIntent) new SummonIntent()
    });
    RandomBranchState initialState1 = new RandomBranchState("RAND");
    state1.FollowUpState = (MonsterState) initialState1;
    state2.FollowUpState = (MonsterState) initialState1;
    state3.FollowUpState = (MonsterState) initialState1;
    state4.FollowUpState = (MonsterState) initialState1;
    initialState1.AddBranch((MonsterState) state1, MoveRepeatType.CannotRepeat, (Func<float>) (() => !this.CanSummon() ? 1f : 0.0833333358f));
    initialState1.AddBranch((MonsterState) state2, MoveRepeatType.CannotRepeat, (Func<float>) (() => !this.CanSummon() ? 1f : 0.0833333358f));
    initialState1.AddBranch((MonsterState) state3, 3, MoveRepeatType.CannotRepeat, (Func<float>) (() => !this.CanSummon() ? 1f : 0.0833333358f));
    initialState1.AddBranch((MonsterState) state4, MoveRepeatType.UseOnlyOnce, (Func<float>) (() => !this.CanSummon() ? 0.0f : 0.75f));
    states.Add((MonsterState) initialState1);
    states.Add((MonsterState) state1);
    states.Add((MonsterState) state2);
    states.Add((MonsterState) state3);
    states.Add((MonsterState) state4);
    if (this.StarterMoveIndex == -1)
      return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState1);
    MoveState moveState;
    switch (this.StarterMoveIndex % 3)
    {
      case 0:
        moveState = state1;
        break;
      case 1:
        moveState = state2;
        break;
      default:
        moveState = state3;
        break;
    }
    MoveState initialState2 = moveState;
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState2);
  }

  private async Task ScratchMove(IReadOnlyList<Creature> targets)
  {
    this.TurnsUntilSummonable--;
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.ScratchDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.25f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/two_tail_rats/two_tail_rats_attack_hands").WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  private async Task DiseaseBiteMove(IReadOnlyList<Creature> targets)
  {
    this.TurnsUntilSummonable--;
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.DiseaseBiteDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.25f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/two_tail_rats/two_tail_rats_attack_hands").WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  private async Task ScreechMove(IReadOnlyList<Creature> targets)
  {
    this.TurnsUntilSummonable--;
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/two_tail_rats/two_tail_rats_attack_bite");
    await CreatureCmd.TriggerAnim(this.Creature, "Cast", 0.3f);
    IReadOnlyList<FrailPower> frailPowerList = await PowerCmd.Apply<FrailPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) targets, 1M, this.Creature, (CardModel) null);
  }

  private async Task CallForBackup(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/two_tail_rats/two_tail_rats_summon");
    await CreatureCmd.TriggerAnim(this.Creature, "Summon", 0.3f);
    string nextSlot;
    if (!this.CombatState.IsLiveCombat())
    {
      nextSlot = (string) null;
    }
    else
    {
      nextSlot = Enumerable.LastOrDefault<string>((IEnumerable<string>) this.CombatState.Encounter.Slots, (Func<string, bool>) (s => this.CombatState.Enemies.All<Creature>((Func<Creature, bool>) (c => c.SlotName != s))), string.Empty);
      if (!string.IsNullOrEmpty(nextSlot))
      {
        await Cmd.Wait(0.5f);
        Creature creature = await CreatureCmd.Add<TwoTailedRat>(this.CombatState, nextSlot);
      }
      List<TwoTailedRat> list = this.CombatState.Enemies.Select<Creature, MonsterModel>((Func<Creature, MonsterModel>) (c => c.Monster)).OfType<TwoTailedRat>().ToList<TwoTailedRat>();
      int maxCallForBackupCount = list.Max<TwoTailedRat>((Func<TwoTailedRat, int>) (c => c.CallForBackupCount + 1));
      list.ForEach((Action<TwoTailedRat>) (r => r.CallForBackupCount = maxCallForBackupCount));
      nextSlot = (string) null;
    }
  }

  private bool CanSummon()
  {
    if (this.TurnsUntilSummonable > 0 || this.CallForBackupCount >= 3 || string.IsNullOrEmpty(this.CombatState.Encounter?.GetNextSlot(this.CombatState)))
      return false;
    foreach (Creature creature in this.CombatState.GetTeammatesOf(this.Creature).Where<Creature>((Func<Creature, bool>) (c => c != this.Creature)).ToList<Creature>())
    {
      if (creature.Monster.NextMove.Id.Equals("CALL_FOR_BACKUP_MOVE"))
        return false;
    }
    return true;
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("debuff");
    AnimState state2 = new AnimState("summon");
    AnimState state3 = new AnimState("attack");
    AnimState state4 = new AnimState("hurt");
    AnimState state5 = new AnimState("die");
    state2.NextState = initialState;
    state1.NextState = initialState;
    state3.NextState = initialState;
    state4.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Cast", state1);
    animator.AddAnyState("Summon", state2);
    animator.AddAnyState("Attack", state3);
    animator.AddAnyState("Dead", state5);
    animator.AddAnyState("Hit", state4);
    return animator;
  }
}
