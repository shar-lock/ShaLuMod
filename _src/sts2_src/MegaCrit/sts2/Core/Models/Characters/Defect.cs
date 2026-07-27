// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Characters.Defect
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

public sealed class Defect : CharacterModel
{
  public const string energyColorName = "defect";

  public override Color NameColor => StsColors.blue;

  public override CharacterGender Gender => CharacterGender.Neutral;

  protected override CharacterModel UnlocksAfterRunAs
  {
    get => (CharacterModel) ModelDb.Character<Necrobinder>();
  }

  public override int StartingHp => 75;

  public override int StartingGold => 99;

  public override CardPoolModel CardPool => (CardPoolModel) ModelDb.CardPool<DefectCardPool>();

  public override RelicPoolModel RelicPool => (RelicPoolModel) ModelDb.RelicPool<DefectRelicPool>();

  public override PotionPoolModel PotionPool
  {
    get => (PotionPoolModel) ModelDb.PotionPool<DefectPotionPool>();
  }

  public static Vector2 EyelineOffset => new Vector2(34f, -30f);

  public override IEnumerable<CardModel> StartingDeck
  {
    get
    {
      return (IEnumerable<CardModel>) new \u003C\u003Ez__ReadOnlyArray<CardModel>(new CardModel[10]
      {
        (CardModel) ModelDb.Card<StrikeDefect>(),
        (CardModel) ModelDb.Card<StrikeDefect>(),
        (CardModel) ModelDb.Card<StrikeDefect>(),
        (CardModel) ModelDb.Card<StrikeDefect>(),
        (CardModel) ModelDb.Card<DefendDefect>(),
        (CardModel) ModelDb.Card<DefendDefect>(),
        (CardModel) ModelDb.Card<DefendDefect>(),
        (CardModel) ModelDb.Card<DefendDefect>(),
        (CardModel) ModelDb.Card<Zap>(),
        (CardModel) ModelDb.Card<Dualcast>()
      });
    }
  }

  public override IReadOnlyList<RelicModel> StartingRelics
  {
    get
    {
      return (IReadOnlyList<RelicModel>) new \u003C\u003Ez__ReadOnlySingleElementList<RelicModel>((RelicModel) ModelDb.Relic<CrackedCore>());
    }
  }

  public override float AttackAnimDelay => 0.15f;

  public override float CastAnimDelay => 0.25f;

  public override float PowerUpAnimDelay => 0.5f;

  public override List<string> GetArchitectAttackVfx()
  {
    int capacity = 5;
    List<string> architectAttackVfx = new List<string>(capacity);
    CollectionsMarshal.SetCount<string>(architectAttackVfx, capacity);
    Span<string> span = CollectionsMarshal.AsSpan<string>(architectAttackVfx);
    int num1 = 0;
    span[num1] = "vfx/vfx_attack_lightning";
    int num2 = num1 + 1;
    span[num2] = "vfx/vfx_attack_blunt";
    int num3 = num2 + 1;
    span[num3] = "vfx/vfx_scratch";
    int num4 = num3 + 1;
    span[num4] = "vfx/vfx_attack_slash";
    int num5 = num4 + 1;
    span[num5] = "vfx/vfx_heavy_blunt";
    return architectAttackVfx;
  }

  public override Color EnergyLabelOutlineColor => new Color("163E64FF");

  public override int BaseOrbSlotCount => 3;

  public override Color DialogueColor => new Color("13446B");

  public override VfxColor SpeechBubbleColor => VfxColor.Blue;

  public override Color MapDrawingColor => new Color("0D638C");

  public override Color RemoteTargetingLineColor => new Color("70B6EDFF");

  public override Color RemoteTargetingLineOutline => new Color("163E64FF");

  public override string CharacterTransitionSfx => "event:/sfx/ui/wipe_ironclad";

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState animState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("cast");
    AnimState state2 = new AnimState("attack");
    AnimState state3 = new AnimState("hurt");
    AnimState state4 = new AnimState("die");
    AnimState state5 = new AnimState("process");
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
    animator.AddAnyState("PowerUp", state5);
    animator.AddAnyState("Relaxed", state6);
    return animator;
  }
}
