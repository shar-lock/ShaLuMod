// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent.CrystalSphereItems.CrystalSpherePotion
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rewards;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent.CrystalSphereItems;

public class CrystalSpherePotion : CrystalSphereItem
{
  private readonly PotionRarity _rarity;

  protected override string TexturePath
  {
    get
    {
      return ImageHelper.GetImagePath($"events/crystal_sphere/crystal_sphere_{this._rarity.ToString().ToLowerInvariant()}_potion.png");
    }
  }

  public override bool IsGood => true;

  public override Vector2I Size
  {
    get => this._rarity != PotionRarity.Rare ? new Vector2I(1, 3) : new Vector2I(2, 2);
  }

  public CrystalSpherePotion(PotionRarity rarity) => this._rarity = rarity;

  public override Reward ToReward(Player owner, Rng rng)
  {
    IEnumerable<PotionModel> items = PotionFactory.GetPotionOptions(owner).Where<PotionModel>((Func<PotionModel, bool>) (p => p.Rarity == this._rarity));
    return new PotionReward(rng.NextItem<PotionModel>(items).ToMutable(), owner).SetRng(rng);
  }

  public override SerializableCrystalSphereItem ToSerializable()
  {
    return new SerializableCrystalSphereItem()
    {
      type = CrystalSphereItemType.Potion,
      potionRarity = this._rarity
    };
  }
}
