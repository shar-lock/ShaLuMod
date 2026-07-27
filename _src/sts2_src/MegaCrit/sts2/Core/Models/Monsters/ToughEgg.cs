// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.ToughEgg
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Combat;
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
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class ToughEgg : MonsterModel
{
  private bool _hatched;
  private const string _hatchTrigger = "Hatch";
  private const string _hatchSfx = "event:/sfx/enemy/enemy_attacks/tough_egg/tough_egg_hatch";
  private static readonly string[] _eggOptions = new string[2]
  {
    "egg1",
    "egg2"
  };
  private MonsterState? _afterHatchedState;
  private bool _isHatched;
  private Vector2? _hatchPos;

  public override LocString Title
  {
    get
    {
      return !this._hatched ? MonsterModel.L10NMonsterLookup(this.Id.Entry + ".name") : MonsterModel.L10NMonsterLookup("HATCHLING.name");
    }
  }

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 15, 14);
  }

  public override int MaxInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 19, 18);
  }

  public int HatchlingMinHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 20, 19);
  }

  public int HatchlingMaxHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 23, 22);
  }

  private static int NibbleDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 5, 4);
  }

  public override string DeathSfx
  {
    get
    {
      return !this._hatched ? "event:/sfx/enemy/enemy_attacks/tough_egg/tough_egg_die" : "event:/sfx/enemy/enemy_attacks/tough_egg/hatchling_die";
    }
  }

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Slime;

  public MonsterState? AfterHatchedState
  {
    get => this._afterHatchedState;
    set
    {
      this.AssertMutable();
      this._afterHatchedState = value;
    }
  }

  public bool IsHatched
  {
    get => this._isHatched;
    set
    {
      this.AssertMutable();
      this._isHatched = value;
    }
  }

  public Vector2? HatchPos
  {
    get => this._hatchPos;
    set
    {
      this.AssertMutable();
      this._hatchPos = value;
    }
  }

  public override void SetupSkins(MegaSprite spine, MegaSkeleton skeleton)
  {
    MegaSkin skin = spine.NewSkin("custom-skin");
    skin.AddSkin(skeleton.GetData().FindSkin(this.Rng.NextItem<string>((IEnumerable<string>) ToughEgg._eggOptions)));
    skeleton.SetSkin(skin);
    skeleton.SetSlotsToSetupPose();
  }

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    if (TestMode.IsOff && this.HatchPos.HasValue)
      NCombatRoom.Instance.GetCreatureNode(this.Creature).GlobalPosition = this.HatchPos.Value;
    if (!this.IsHatched)
    {
      HatchPower hatchPower = await PowerCmd.Apply<HatchPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, (Decimal) (this.CombatState.CurrentSide == CombatSide.Enemy ? 2 : 1), this.Creature, (CardModel) null);
    }
    else
    {
      await this.Hatch();
      this.MoveStateMachine?.ForceCurrentState(this.AfterHatchedState);
    }
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("HATCH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.HatchMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SummonIntent()
    });
    MoveState moveState = new MoveState("NIBBLE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.NibbleMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(ToughEgg.NibbleDamage)
    });
    initialState.FollowUpState = (MonsterState) moveState;
    moveState.FollowUpState = (MonsterState) moveState;
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) moveState);
    this.AfterHatchedState = (MonsterState) moveState;
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task HatchMove(IReadOnlyList<Creature> targets)
  {
    this.IsHatched = true;
    await PowerCmd.Remove<HatchPower>(this.Creature);
    this._hatched = true;
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/tough_egg/tough_egg_hatch");
    foreach (PowerModel power in this.Creature.Powers.Where<PowerModel>((Func<PowerModel, bool>) (p => !(p is MinionPower))).ToList<PowerModel>())
      await PowerCmd.Remove(power);
    await this.Hatch();
  }

  private async Task Hatch()
  {
    await CreatureCmd.TriggerAnim(this.Creature, nameof (Hatch), 0.5f);
    await CreatureCmd.SetMaxAndCurrentHp(this.Creature, Creature.ScaleHpForMultiplayer((Decimal) this.RunRng.Niche.NextInt(this.HatchlingMinHp, this.HatchlingMaxHp), this.CombatState.Encounter, this.CombatState.Players.Count, this.CombatState.RunState.CurrentActIndex));
  }

  private async Task NibbleMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) ToughEgg.NibbleDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.2f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_bite").Execute((PlayerChoiceContext) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState animState1 = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("hurt");
    AnimState state2 = new AnimState("attack");
    AnimState state3 = new AnimState("die");
    AnimState initialState = new AnimState("egg_spawn");
    AnimState animState2 = new AnimState("egg_idle_loop", true);
    AnimState state4 = new AnimState("egg_hurt");
    AnimState state5 = new AnimState("egg_die");
    AnimState state6 = new AnimState("egg_hatch");
    state1.NextState = animState1;
    state2.NextState = animState1;
    initialState.NextState = animState2;
    state6.NextState = animState1;
    state4.NextState = animState2;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Hatch", state6);
    animator.AddAnyState("Hit", state4, (Func<bool>) (() => !this.IsHatched));
    animator.AddAnyState("Dead", state5, (Func<bool>) (() => !this.IsHatched));
    animator.AddAnyState("Hit", state1, (Func<bool>) (() => this.IsHatched));
    animator.AddAnyState("Dead", state3, (Func<bool>) (() => this.IsHatched));
    animator.AddAnyState("Attack", state2);
    return animator;
  }
}
