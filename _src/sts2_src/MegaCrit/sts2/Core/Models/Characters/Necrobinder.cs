// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Characters.Necrobinder
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
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Characters;

public sealed class Necrobinder : CharacterModel
{
  private const string _summonTrigger = "summonTrigger";
  public const string energyColorName = "necrobinder";
  public const string healOstyPath = "vfx/vfx_heal_osty";
  private const string _ostyVisualsPath = "creature_visuals/osty";

  public override Color NameColor => StsColors.purple;

  public override CharacterGender Gender => CharacterGender.Feminine;

  protected override CharacterModel UnlocksAfterRunAs
  {
    get => (CharacterModel) ModelDb.Character<Regent>();
  }

  public override int StartingHp => 66;

  public override int StartingGold => 99;

  public override CardPoolModel CardPool => (CardPoolModel) ModelDb.CardPool<NecrobinderCardPool>();

  public override RelicPoolModel RelicPool
  {
    get => (RelicPoolModel) ModelDb.RelicPool<NecrobinderRelicPool>();
  }

  public override PotionPoolModel PotionPool
  {
    get => (PotionPoolModel) ModelDb.PotionPool<NecrobinderPotionPool>();
  }

  protected override IEnumerable<string> ExtraAssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[2]
      {
        SceneHelper.GetScenePath("vfx/vfx_heal_osty"),
        SceneHelper.GetScenePath("creature_visuals/osty")
      });
    }
  }

  public override IEnumerable<CardModel> StartingDeck
  {
    get
    {
      return (IEnumerable<CardModel>) new \u003C\u003Ez__ReadOnlyArray<CardModel>(new CardModel[10]
      {
        (CardModel) ModelDb.Card<StrikeNecrobinder>(),
        (CardModel) ModelDb.Card<StrikeNecrobinder>(),
        (CardModel) ModelDb.Card<StrikeNecrobinder>(),
        (CardModel) ModelDb.Card<StrikeNecrobinder>(),
        (CardModel) ModelDb.Card<DefendNecrobinder>(),
        (CardModel) ModelDb.Card<DefendNecrobinder>(),
        (CardModel) ModelDb.Card<DefendNecrobinder>(),
        (CardModel) ModelDb.Card<DefendNecrobinder>(),
        (CardModel) ModelDb.Card<Bodyguard>(),
        (CardModel) ModelDb.Card<Unleash>()
      });
    }
  }

  public override IReadOnlyList<RelicModel> StartingRelics
  {
    get
    {
      return (IReadOnlyList<RelicModel>) new \u003C\u003Ez__ReadOnlySingleElementList<RelicModel>((RelicModel) ModelDb.Relic<BoundPhylactery>());
    }
  }

  public override float AttackAnimDelay => 0.15f;

  public override float CastAnimDelay => 0.4f;

  public override List<string> GetArchitectAttackVfx()
  {
    int capacity = 4;
    List<string> architectAttackVfx = new List<string>(capacity);
    CollectionsMarshal.SetCount<string>(architectAttackVfx, capacity);
    Span<string> span = CollectionsMarshal.AsSpan<string>(architectAttackVfx);
    int num1 = 0;
    span[num1] = "vfx/vfx_thrash";
    int num2 = num1 + 1;
    span[num2] = "vfx/vfx_heavy_blunt";
    int num3 = num2 + 1;
    span[num3] = "vfx/vfx_attack_slash";
    int num4 = num3 + 1;
    span[num4] = "vfx/vfx_bloody_impact";
    return architectAttackVfx;
  }

  public override Color EnergyLabelOutlineColor => new Color("702D6FFF");

  public override Color DialogueColor => new Color("6B4658");

  public override Color MapDrawingColor => new Color("AC0486");

  public override Color RemoteTargetingLineColor => new Color("FD98C9FF");

  public override Color RemoteTargetingLineOutline => new Color("702D6FFF");

  public override string CharacterTransitionSfx => "event:/sfx/ui/wipe_ironclad";

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState animState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("cast");
    AnimState state2 = new AnimState("attack");
    AnimState state3 = new AnimState("hurt");
    AnimState state4 = new AnimState("die");
    AnimState state5 = new AnimState("cast_mighty");
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
    animator.AddAnyState("summonTrigger", state1);
    animator.AddAnyState("Cast", state5);
    animator.AddAnyState("Relaxed", state6);
    animator.AddAnyState("PowerUp", state5);
    return animator;
  }

  public static string GetSummonAnimIfApplicable(CharacterModel character)
  {
    return !(character is Necrobinder) ? "Cast" : "summonTrigger";
  }

  public static float GetSummonDelayIfApplicable(CharacterModel character)
  {
    return !(character is Necrobinder) ? character.CastAnimDelay : 0.25f;
  }
}
