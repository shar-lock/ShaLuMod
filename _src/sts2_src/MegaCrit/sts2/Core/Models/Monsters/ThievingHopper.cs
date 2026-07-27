// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.ThievingHopper
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class ThievingHopper : MonsterModel
{
  private static readonly Func<CardModel, bool>[] _stealPriorities = new Func<CardModel, bool>[4]
  {
    (Func<CardModel, bool>) (c => !(c.Enchantment is Imbued) && c.Rarity == CardRarity.Uncommon),
    (Func<CardModel, bool>) (c =>
    {
      bool flag1 = !(c.Enchantment is Imbued);
      if (flag1)
      {
        bool flag2;
        switch (c.Rarity)
        {
          case CardRarity.Common:
          case CardRarity.Rare:
          case CardRarity.Event:
            flag2 = true;
            break;
          default:
            flag2 = false;
            break;
        }
        flag1 = flag2;
      }
      return flag1;
    }),
    (Func<CardModel, bool>) (c =>
    {
      bool flag3 = !(c.Enchantment is Imbued);
      if (flag3)
      {
        bool flag4;
        switch (c.Rarity)
        {
          case CardRarity.Basic:
          case CardRarity.Quest:
            flag4 = true;
            break;
          default:
            flag4 = false;
            break;
        }
        flag3 = flag4;
      }
      return flag3;
    }),
    (Func<CardModel, bool>) (c => c.Rarity == CardRarity.Ancient || c.Enchantment is Imbued)
  };
  public const string stunTrigger = "StunTrigger";
  private bool _isHovering;
  private const string _fleeTrigger = "Flee";
  private const string _hoverTrigger = "Hover";
  private const string _stealTrigger = "Steal";
  private const string _escapeMoveId = "ESCAPE_MOVE";
  private const string _stealSfx = "event:/sfx/enemy/enemy_attacks/thieving_hopper/thieving_hopper_steal";
  private const string _takeOffSfx = "event:/sfx/enemy/enemy_attacks/thieving_hopper/thieving_hopper_take_off";
  public const string hoverLoop = "event:/sfx/enemy/enemy_attacks/thieving_hopper/thieving_hopper_hover_loop";

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 84, 79);
  }

  public override int MaxInitialHp => this.MinInitialHp;

  public bool IsHovering
  {
    get => this._isHovering;
    set
    {
      this.AssertMutable();
      this._isHovering = value;
    }
  }

  protected override string AttackSfx
  {
    get
    {
      return !this.IsHovering ? "event:/sfx/enemy/enemy_attacks/thieving_hopper/thieving_hopper_attack" : "event:/sfx/enemy/enemy_attacks/thieving_hopper/thieving_hopper_attack_hover";
    }
  }

  private string FleeSfx
  {
    get
    {
      return !this.IsHovering ? "event:/sfx/enemy/enemy_attacks/thieving_hopper/thieving_hopper_flee" : "event:/sfx/enemy/enemy_attacks/thieving_hopper/thieving_hopper_flee_hover";
    }
  }

  public override string DeathSfx
  {
    get => "event:/sfx/enemy/enemy_attacks/thieving_hopper/thieving_hopper_die";
  }

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Insect;

  public override string TakeDamageSfx
  {
    get
    {
      return !this.IsHovering ? base.TakeDamageSfx : "event:/sfx/enemy/enemy_attacks/thieving_hopper/thieving_hopper_hurt_hover";
    }
  }

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    EscapeArtistPower escapeArtistPower = await PowerCmd.Apply<EscapeArtistPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 5M, this.Creature, (CardModel) null);
  }

  public override void BeforeRemovedFromRoom()
  {
    SfxCmd.StopLoop(this.Creature, "event:/sfx/enemy/enemy_attacks/thieving_hopper/thieving_hopper_hover_loop");
  }

  private int TheftDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 19, 17);
  }

  private int HatTrickDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 23, 21);
  }

  private int NabDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 16 /*0x10*/, 14);
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("THIEVERY_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ThieveryMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.TheftDamage),
      (AbstractIntent) new CardDebuffIntent()
    });
    MoveState moveState1 = new MoveState("NAB_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.NabMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.NabDamage)
    });
    MoveState moveState2 = new MoveState("HAT_TRICK_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.HatTrickMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.HatTrickDamage)
    });
    MoveState moveState3 = new MoveState("FLUTTER_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.FlutterMove), new AbstractIntent[1]
    {
      (AbstractIntent) new BuffIntent()
    });
    MoveState moveState4 = new MoveState("ESCAPE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.EscapeMove), new AbstractIntent[1]
    {
      (AbstractIntent) new EscapeIntent()
    });
    initialState.FollowUpState = (MonsterState) moveState3;
    moveState3.FollowUpState = (MonsterState) moveState2;
    moveState2.FollowUpState = (MonsterState) moveState1;
    moveState1.FollowUpState = (MonsterState) moveState4;
    moveState4.FollowUpState = (MonsterState) moveState4;
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) moveState1);
    states.Add((MonsterState) moveState2);
    states.Add((MonsterState) moveState3);
    states.Add((MonsterState) moveState4);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task ThieveryMove(IReadOnlyList<Creature> targets)
  {
    NCreature creatureNode = NCombatRoom.Instance?.GetCreatureNode(this.Creature);
    if (creatureNode != null)
    {
      NCreature creatureNode1 = (LocalContext.GetMe((IEnumerable<Creature>) targets) ?? targets.First<Creature>()).GetCreatureNode();
      Node2D specialNode = creatureNode.GetSpecialNode<Node2D>("Visuals/SpineBoneNode");
      if (specialNode != null && creatureNode1 != null)
      {
        float num = 900f * creatureNode.Visuals.Scale.X;
        specialNode.GlobalPosition = new Vector2(creatureNode1.GlobalPosition.X + num, specialNode.GlobalPosition.Y);
      }
    }
    await CreatureCmd.TriggerAnim(this.Creature, "Steal", 0.25f);
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/thieving_hopper/thieving_hopper_steal");
    List<CardModel> cardsToSteal = new List<CardModel>();
    foreach (Creature target in (IEnumerable<Creature>) targets)
    {
      if (!target.IsDead)
      {
        List<CardModel> list = CardPile.GetCards(target.Player ?? target.PetOwner, PileType.Draw, PileType.Discard).Where<CardModel>((Func<CardModel, bool>) (c => c.DeckVersion != null)).ToList<CardModel>();
        IEnumerable<CardModel> cardModels = (IEnumerable<CardModel>) list;
        foreach (Func<CardModel, bool> stealPriority in ThievingHopper._stealPriorities)
        {
          IEnumerable<CardModel> source = list.Where<CardModel>(stealPriority);
          if (source.Any<CardModel>())
          {
            cardModels = source;
            break;
          }
        }
        if (cardModels.Any<CardModel>())
        {
          CardModel cardToSteal = this.RunRng.CombatCardGeneration.NextItem<CardModel>(cardModels);
          await CardPileCmd.RemoveFromCombat(cardToSteal);
          cardsToSteal.Add(cardToSteal);
          cardToSteal = (CardModel) null;
        }
      }
    }
    await Cmd.Wait(0.6f);
    foreach (CardModel card in cardsToSteal)
    {
      if (creatureNode != null && LocalContext.IsMine(card))
      {
        Marker2D specialNode = creatureNode.GetSpecialNode<Marker2D>("%StolenCardPos");
        if (specialNode != null)
        {
          NCard child = NCard.Create(card);
          ((Node) specialNode).AddChildSafely((Node) child);
          NCard ncard = child;
          ncard.Position = Vector2.op_Addition(ncard.Position, Vector2.op_Multiply(child.Size, 0.5f));
          child.UpdateVisuals(PileType.Deck, CardPreviewMode.Normal);
        }
      }
      SwipePower swipe = (SwipePower) ModelDb.Power<SwipePower>().ToMutable();
      await swipe.Steal(card);
      await PowerCmd.Apply((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (PowerModel) swipe, this.Creature, 1M, this.Creature, (CardModel) null);
      swipe = (SwipePower) null;
    }
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.TheftDamage).FromMonster((MonsterModel) this).WithNoAttackerAnim().WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
    creatureNode = (NCreature) null;
    cardsToSteal = (List<CardModel>) null;
  }

  private async Task NabMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.NabDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.3f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  private async Task HatTrickMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.HatTrickDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.3f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  private async Task FlutterMove(IReadOnlyList<Creature> targets)
  {
    this.IsHovering = true;
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/thieving_hopper/thieving_hopper_take_off");
    SfxCmd.PlayLoop(this.Creature, "event:/sfx/enemy/enemy_attacks/thieving_hopper/thieving_hopper_hover_loop");
    await CreatureCmd.TriggerAnim(this.Creature, "Hover", 0.0f);
    await Cmd.Wait(1.25f);
    FlutterPower flutterPower = await PowerCmd.Apply<FlutterPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 5M, this.Creature, (CardModel) null);
  }

  private async Task EscapeMove(IReadOnlyList<Creature> targets)
  {
    NCombatRoom.Instance?.GetCreatureNode(this.Creature)?.ToggleIsInteractable(false);
    SfxCmd.Play(this.FleeSfx);
    await CreatureCmd.TriggerAnim(this.Creature, "Flee", 0.85f);
    if (this.IsHovering)
    {
      SfxCmd.StopLoop(this.Creature, "event:/sfx/enemy/enemy_attacks/thieving_hopper/thieving_hopper_hover_loop");
      this.IsHovering = false;
    }
    await Cmd.Wait(1.5f);
    await CreatureCmd.Escape(this.Creature);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState animState1 = new AnimState("idle_loop", true)
    {
      BoundsContainer = "GroundedBounds"
    };
    AnimState state1 = new AnimState("flee");
    AnimState state2 = new AnimState("flee_hover");
    AnimState state3 = new AnimState("hurt");
    AnimState state4 = new AnimState("hurt_hover");
    AnimState state5 = new AnimState("attack");
    AnimState state6 = new AnimState("attack_hover");
    AnimState state7 = new AnimState("die");
    AnimState state8 = new AnimState("take_off");
    AnimState animState2 = new AnimState("hover_loop", true)
    {
      BoundsContainer = "FlyingBounds"
    };
    AnimState state9 = new AnimState("steal");
    state8.NextState = animState2;
    state9.NextState = animState1;
    state3.NextState = animState1;
    state4.NextState = animState2;
    state5.NextState = animState1;
    state6.NextState = animState2;
    CreatureAnimator animator = new CreatureAnimator(animState1, controller);
    animator.AddAnyState("StunTrigger", animState1);
    animator.AddAnyState("Hover", state8);
    animator.AddAnyState("Steal", state9);
    animator.AddAnyState("Dead", state7);
    animator.AddAnyState("Hit", state4, (Func<bool>) (() => this.IsHovering));
    animator.AddAnyState("Hit", state3, (Func<bool>) (() => !this.IsHovering));
    animator.AddAnyState("Attack", state6, (Func<bool>) (() => this.IsHovering));
    animator.AddAnyState("Attack", state5, (Func<bool>) (() => !this.IsHovering));
    animator.AddAnyState("Flee", state2, (Func<bool>) (() => this.IsHovering));
    animator.AddAnyState("Flee", state1, (Func<bool>) (() => !this.IsHovering));
    return animator;
  }
}
