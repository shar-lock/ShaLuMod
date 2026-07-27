// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.KinFollower
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
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class KinFollower : MonsterModel
{
  private static readonly string[] _hairOptions = new string[3]
  {
    "hair_1",
    "hair_2",
    "hair_3"
  };
  private const string _kinPoofPath = "vfx/vfx_kin_poof";
  private const string _slashTrigger = "SlashTrigger";
  private const string _boomerangTrigger = "BoomerangTrigger";
  private const string _quickSlashSfx = "event:/sfx/enemy/enemy_attacks/the_kin_minion/the_kin_minion_quick_slash";
  private const string _boomerangSfx = "event:/sfx/enemy/enemy_attacks/the_kin_minion/the_kin_minion_boomerang_slash";
  private const string _buffSfx = "event:/sfx/enemy/enemy_attacks/the_kin_minion/the_kin_minion_buff";
  private const int _boomerangRepeat = 2;
  private bool _startsWithDance;

  public override IEnumerable<string> AssetPaths
  {
    get
    {
      int capacity = 1;
      List<string> first = new List<string>(capacity);
      CollectionsMarshal.SetCount<string>(first, capacity);
      CollectionsMarshal.AsSpan<string>(first)[0] = SceneHelper.GetScenePath("vfx/vfx_kin_poof");
      return first.Concat<string>(base.AssetPaths);
    }
  }

  public override string DeathSfx
  {
    get => "event:/sfx/enemy/enemy_attacks/the_kin_minion/the_kin_minion_die";
  }

  public override string HurtSfx
  {
    get => "event:/sfx/enemy/enemy_attacks/the_kin_priest/the_kin_priest_hurt";
  }

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 62, 58);
  }

  public override int MaxInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 63 /*0x3F*/, 59);
  }

  private int QuickSlashDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 5, 5);
  }

  private int BoomerangDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 2, 2);
  }

  private int DanceStrength
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 2);
  }

  public bool StartsWithDance
  {
    get => this._startsWithDance;
    set
    {
      this.AssertMutable();
      this._startsWithDance = value;
    }
  }

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Fur;

  public override void SetupSkins(MegaSprite spine, MegaSkeleton skeleton)
  {
    MegaSkin skin1 = spine.NewSkin("custom-skin");
    MegaSkin skin2 = skeleton.GetData().FindSkin(Rng.Chaotic.NextItem<string>((IEnumerable<string>) KinFollower._hairOptions));
    skin1.AddSkin(skin2);
    skeleton.SetSkin(skin1);
    skeleton.SetSlotsToSetupPose();
  }

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    MinionPower minionPower = await PowerCmd.Apply<MinionPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 1M, this.Creature, (CardModel) null);
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState moveState1 = new MoveState("QUICK_SLASH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.QuickSlashMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.QuickSlashDamage)
    });
    MoveState moveState2 = new MoveState("BOOMERANG_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BoomerangMove), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(this.BoomerangDamage, 2)
    });
    MoveState moveState3 = new MoveState("POWER_DANCE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.PowerDanceMove), new AbstractIntent[1]
    {
      (AbstractIntent) new BuffIntent()
    });
    moveState1.FollowUpState = (MonsterState) moveState2;
    moveState2.FollowUpState = (MonsterState) moveState3;
    moveState3.FollowUpState = (MonsterState) moveState1;
    states.Add((MonsterState) moveState1);
    states.Add((MonsterState) moveState2);
    states.Add((MonsterState) moveState3);
    MoveState initialState = this.StartsWithDance ? moveState3 : moveState1;
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task QuickSlashMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.QuickSlashDamage).FromMonster((MonsterModel) this).WithAttackerAnim("SlashTrigger", 0.2f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/the_kin_minion/the_kin_minion_quick_slash").WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  private async Task PowerDanceMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/the_kin_minion/the_kin_minion_buff");
    await CreatureCmd.TriggerAnim(this.Creature, "Cast", 0.9f);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, (Decimal) this.DanceStrength, this.Creature, (CardModel) null);
  }

  private async Task BoomerangMove(IReadOnlyList<Creature> targets)
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
      Node2D specialNode = creatureNode1?.GetSpecialNode<Node2D>("Visuals/AttackDistanceControl");
      if (creatureNode1 != null && specialNode != null && ncreature != null)
      {
        float num = 400f * creatureNode1.Visuals.Scale.X;
        specialNode.GlobalPosition = new Vector2(ncreature.GlobalPosition.X + num, specialNode.GlobalPosition.Y);
      }
    }
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.BoomerangDamage).WithHitCount(2).FromMonster((MonsterModel) this).WithAttackerAnim("BoomerangTrigger", 0.2f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/the_kin_minion/the_kin_minion_boomerang_slash").WithHitFx("vfx/vfx_attack_slash").OnlyPlayAnimOnce().Execute((PlayerChoiceContext) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("attack_slash");
    AnimState state2 = new AnimState("attack_boomerang");
    AnimState state3 = new AnimState("buff");
    AnimState state4 = new AnimState("hurt");
    AnimState state5 = new AnimState("die");
    state1.NextState = initialState;
    state2.NextState = initialState;
    state4.NextState = initialState;
    state3.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("BoomerangTrigger", state2);
    animator.AddAnyState("SlashTrigger", state1);
    animator.AddAnyState("Cast", state3);
    animator.AddAnyState("Dead", state5);
    animator.AddAnyState("Hit", state4);
    return animator;
  }
}
