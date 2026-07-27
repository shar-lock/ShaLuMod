// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent.CrystalSphereItems.CrystalSphereCardReward
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent.CrystalSphereItems;

public class CrystalSphereCardReward : CrystalSphereItem
{
  private readonly Player _owner;
  private readonly CardRarity _rarity;

  private string BannerMaterialPath
  {
    get
    {
      string bannerMaterialPath;
      switch (this._rarity)
      {
        case CardRarity.Uncommon:
          bannerMaterialPath = "res://materials/cards/banners/card_banner_uncommon_mat.tres";
          break;
        case CardRarity.Rare:
          bannerMaterialPath = "res://materials/cards/banners/card_banner_rare_mat.tres";
          break;
        case CardRarity.Ancient:
          bannerMaterialPath = "res://materials/cards/banners/card_banner_ancient_mat.tres";
          break;
        case CardRarity.Event:
          bannerMaterialPath = "res://materials/cards/banners/card_banner_event_mat.tres";
          break;
        case CardRarity.Status:
          bannerMaterialPath = "res://materials/cards/banners/card_banner_status_mat.tres";
          break;
        case CardRarity.Curse:
          bannerMaterialPath = "res://materials/cards/banners/card_banner_curse_mat.tres";
          break;
        case CardRarity.Quest:
          bannerMaterialPath = "res://materials/cards/banners/card_banner_quest_mat.tres";
          break;
        default:
          bannerMaterialPath = "res://materials/cards/banners/card_banner_common_mat.tres";
          break;
      }
      return bannerMaterialPath;
    }
  }

  public Material BannerMaterial => PreloadManager.Cache.GetMaterial(this.BannerMaterialPath);

  public Material FrameMaterial => this._owner.Character.CardPool.FrameMaterial;

  public override Vector2I Size => new Vector2I(2, 2);

  protected override string TexturePath
  {
    get
    {
      return ImageHelper.GetImagePath($"events/crystal_sphere/crystal_sphere_{this._rarity.ToString().ToLowerInvariant()}_card_reward.png");
    }
  }

  public override bool IsGood => true;

  public CrystalSphereCardReward(CardRarity rarity, Player owner)
  {
    this._rarity = rarity;
    this._owner = owner;
  }

  public override Reward ToReward(Player owner, Rng rng)
  {
    // ISSUE: object of a compiler-generated type is created
    return (Reward) new CardReward(new CardCreationOptions((IEnumerable<CardPoolModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardPoolModel>(owner.Character.CardPool), CardCreationSource.Other, CardRarityOddsType.Uniform, (Func<CardModel, bool>) (c => c.Rarity == this._rarity)).WithRngOverride(rng), 3, owner);
  }

  public override SerializableCrystalSphereItem ToSerializable()
  {
    return new SerializableCrystalSphereItem()
    {
      type = CrystalSphereItemType.CardReward,
      cardRarity = this._rarity
    };
  }
}
