// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Characters.Ironclad
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

public sealed class Ironclad : CharacterModel
{
  public const string heavyAttackTrigger = "heavyAttack";
  public const string energyColorName = "ironclad";

  public override CharacterGender Gender => CharacterGender.Masculine;

  protected override CharacterModel? UnlocksAfterRunAs => (CharacterModel) null;

  public override Color NameColor => StsColors.red;

  public override int StartingHp => 80 /*0x50*/;

  public override int StartingGold => 99;

  public override CardPoolModel CardPool => (CardPoolModel) ModelDb.CardPool<IroncladCardPool>();

  public override PotionPoolModel PotionPool
  {
    get => (PotionPoolModel) ModelDb.PotionPool<IroncladPotionPool>();
  }

  public override RelicPoolModel RelicPool
  {
    get => (RelicPoolModel) ModelDb.RelicPool<IroncladRelicPool>();
  }

  public override IEnumerable<CardModel> StartingDeck
  {
    get
    {
      return (IEnumerable<CardModel>) new \u003C\u003Ez__ReadOnlyArray<CardModel>(new CardModel[10]
      {
        (CardModel) ModelDb.Card<StrikeIronclad>(),
        (CardModel) ModelDb.Card<StrikeIronclad>(),
        (CardModel) ModelDb.Card<StrikeIronclad>(),
        (CardModel) ModelDb.Card<StrikeIronclad>(),
        (CardModel) ModelDb.Card<StrikeIronclad>(),
        (CardModel) ModelDb.Card<DefendIronclad>(),
        (CardModel) ModelDb.Card<DefendIronclad>(),
        (CardModel) ModelDb.Card<DefendIronclad>(),
        (CardModel) ModelDb.Card<DefendIronclad>(),
        (CardModel) ModelDb.Card<Bash>()
      });
    }
  }

  public override IReadOnlyList<RelicModel> StartingRelics
  {
    get
    {
      return (IReadOnlyList<RelicModel>) new \u003C\u003Ez__ReadOnlySingleElementList<RelicModel>((RelicModel) ModelDb.Relic<BurningBlood>());
    }
  }

  public override float AttackAnimDelay => 0.15f;

  public override float CastAnimDelay => 0.25f;

  public override List<string> GetArchitectAttackVfx()
  {
    int capacity = 5;
    List<string> architectAttackVfx = new List<string>(capacity);
    CollectionsMarshal.SetCount<string>(architectAttackVfx, capacity);
    Span<string> span = CollectionsMarshal.AsSpan<string>(architectAttackVfx);
    int num1 = 0;
    span[num1] = "vfx/vfx_attack_blunt";
    int num2 = num1 + 1;
    span[num2] = "vfx/vfx_heavy_blunt";
    int num3 = num2 + 1;
    span[num3] = "vfx/vfx_attack_slash";
    int num4 = num3 + 1;
    span[num4] = "vfx/vfx_bloody_impact";
    int num5 = num4 + 1;
    span[num5] = "vfx/vfx_rock_shatter";
    return architectAttackVfx;
  }

  public override Color EnergyLabelOutlineColor => new Color("801212FF");

  public override Color DialogueColor => new Color("590700");

  public override VfxColor SpeechBubbleColor => VfxColor.Red;

  public override Color MapDrawingColor => new Color("CB282B");

  public override Color RemoteTargetingLineColor => new Color("E15847FF");

  public override Color RemoteTargetingLineOutline => new Color("801212FF");

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState animState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("cast");
    AnimState state2 = new AnimState("attack");
    AnimState state3 = new AnimState("hurt");
    AnimState state4 = new AnimState("die");
    AnimState state5 = new AnimState("attack_heavy");
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
    animator.AddAnyState("heavyAttack", state5);
    animator.AddAnyState("PowerUp", state1);
    animator.AddAnyState("Relaxed", state6);
    return animator;
  }

  public static string GetHeavyAnimIfApplicable(CharacterModel character)
  {
    return !(character is Ironclad) ? "Attack" : "heavyAttack";
  }

  public static float GetHeavyAttackDelayIfApplicable(CharacterModel character)
  {
    return !(character is Ironclad) ? character.AttackAnimDelay : 0.2f;
  }
}
