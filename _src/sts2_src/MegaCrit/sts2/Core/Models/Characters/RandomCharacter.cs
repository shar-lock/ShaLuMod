// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Characters.RandomCharacter
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.PotionPools;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Models.Relics;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Characters;

public sealed class RandomCharacter : CharacterModel
{
  public override bool IsPlayable => false;

  public override CharacterGender Gender => CharacterGender.Neutral;

  protected override CharacterModel? UnlocksAfterRunAs => (CharacterModel) null;

  public override Color NameColor => StsColors.gold;

  public override int StartingHp => 1;

  public override int StartingGold => 1;

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
        (CardModel) ModelDb.Card<StrikeSilent>(),
        (CardModel) ModelDb.Card<StrikeRegent>(),
        (CardModel) ModelDb.Card<StrikeNecrobinder>(),
        (CardModel) ModelDb.Card<StrikeDefect>(),
        (CardModel) ModelDb.Card<DefendIronclad>(),
        (CardModel) ModelDb.Card<DefendSilent>(),
        (CardModel) ModelDb.Card<DefendRegent>(),
        (CardModel) ModelDb.Card<DefendNecrobinder>(),
        (CardModel) ModelDb.Card<DefendDefect>()
      });
    }
  }

  public override IReadOnlyList<RelicModel> StartingRelics
  {
    get
    {
      return (IReadOnlyList<RelicModel>) new \u003C\u003Ez__ReadOnlySingleElementList<RelicModel>((RelicModel) ModelDb.Relic<Circlet>());
    }
  }

  protected override string CharacterSelectIconPath
  {
    get => ImageHelper.GetImagePath("packed/character_select/char_select_random.png");
  }

  protected override string CharacterSelectLockedIconPath
  {
    get => ImageHelper.GetImagePath("packed/character_select/char_select_random_locked.png");
  }

  public override float AttackAnimDelay => 0.0f;

  public override float CastAnimDelay => 0.0f;

  public override List<string> GetArchitectAttackVfx() => new List<string>();

  public override Color EnergyLabelOutlineColor => Colors.Magenta;

  public override Color DialogueColor => Colors.Magenta;

  public override Color MapDrawingColor => Colors.Magenta;

  public override Color RemoteTargetingLineColor => Colors.Magenta;

  public override Color RemoteTargetingLineOutline => Colors.Magenta;
}
