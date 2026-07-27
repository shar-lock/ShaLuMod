// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Characters.Silent
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.PotionPools;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Characters;

public sealed class Silent : CharacterModel
{
  public const string shivTrigger = "Shiv";
  public const string energyColorName = "silent";

  public override Color NameColor => StsColors.green;

  public override CharacterGender Gender => CharacterGender.Feminine;

  protected override CharacterModel? UnlocksAfterRunAs => (CharacterModel) null;

  public override int StartingHp => 70;

  public override int StartingGold => 99;

  public override CardPoolModel CardPool => (CardPoolModel) ModelDb.CardPool<SilentCardPool>();

  public override RelicPoolModel RelicPool => (RelicPoolModel) ModelDb.RelicPool<SilentRelicPool>();

  public override PotionPoolModel PotionPool
  {
    get => (PotionPoolModel) ModelDb.PotionPool<SilentPotionPool>();
  }

  public override IEnumerable<CardModel> StartingDeck
  {
    get
    {
      return (IEnumerable<CardModel>) new \u003C\u003Ez__ReadOnlyArray<CardModel>(new CardModel[12]
      {
        (CardModel) ModelDb.Card<StrikeSilent>(),
        (CardModel) ModelDb.Card<StrikeSilent>(),
        (CardModel) ModelDb.Card<StrikeSilent>(),
        (CardModel) ModelDb.Card<StrikeSilent>(),
        (CardModel) ModelDb.Card<StrikeSilent>(),
        (CardModel) ModelDb.Card<DefendSilent>(),
        (CardModel) ModelDb.Card<DefendSilent>(),
        (CardModel) ModelDb.Card<DefendSilent>(),
        (CardModel) ModelDb.Card<DefendSilent>(),
        (CardModel) ModelDb.Card<DefendSilent>(),
        (CardModel) ModelDb.Card<Neutralize>(),
        (CardModel) ModelDb.Card<Survivor>()
      });
    }
  }

  public override IReadOnlyList<RelicModel> StartingRelics
  {
    get
    {
      return (IReadOnlyList<RelicModel>) new \u003C\u003Ez__ReadOnlySingleElementList<RelicModel>((RelicModel) ModelDb.Relic<RingOfTheSnake>());
    }
  }

  public override float AttackAnimDelay => 0.15f;

  public override float CastAnimDelay => 0.25f;

  public override List<string> GetArchitectAttackVfx()
  {
    int capacity = 4;
    List<string> architectAttackVfx = new List<string>(capacity);
    CollectionsMarshal.SetCount<string>(architectAttackVfx, capacity);
    Span<string> span = CollectionsMarshal.AsSpan<string>(architectAttackVfx);
    int num1 = 0;
    span[num1] = "vfx/vfx_dagger_spray";
    int num2 = num1 + 1;
    span[num2] = "vfx/vfx_flying_slash";
    int num3 = num2 + 1;
    span[num3] = "vfx/vfx_dramatic_stab";
    int num4 = num3 + 1;
    span[num4] = "vfx/vfx_dagger_throw";
    return architectAttackVfx;
  }

  public override Color EnergyLabelOutlineColor => new Color("004f04FF");

  public override Color DialogueColor => new Color("284719");

  public override VfxColor SpeechBubbleColor => VfxColor.Swamp;

  public override Color MapDrawingColor => new Color("2F6729");

  public override Color RemoteTargetingLineColor => new Color("2EBD5EFF");

  public override Color RemoteTargetingLineOutline => new Color("004f04FF");

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState animState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("cast");
    AnimState state2 = new AnimState("attack");
    AnimState state3 = new AnimState("hurt");
    AnimState state4 = new AnimState("die");
    AnimState state5 = new AnimState("shiv");
    AnimState state6 = new AnimState("relaxed_loop", true);
    state1.NextState = animState;
    state2.NextState = animState;
    state3.NextState = animState;
    state5.NextState = animState;
    state6.AddBranch("Idle", animState);
    CreatureAnimator animator = new CreatureAnimator(animState, controller);
    animator.AddAnyState("Idle", animState);
    animator.AddAnyState("Dead", state4);
    animator.AddAnyState("Hit", state3);
    animator.AddAnyState("Attack", state2);
    animator.AddAnyState("Cast", state1);
    animator.AddAnyState("Shiv", state5);
    animator.AddAnyState("Relaxed", state6);
    animator.AddAnyState("PowerUp", state1);
    return animator;
  }
}
