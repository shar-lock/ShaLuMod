// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent.CrystalSphereItems.CrystalSphereGold
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rewards;

#nullable enable
namespace MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent.CrystalSphereItems;

public class CrystalSphereGold : CrystalSphereItem
{
  private readonly bool _isBig;
  private const int _smallAmount = 10;
  private const int _largeAmount = 30;

  protected override string TexturePath
  {
    get
    {
      return !this._isBig ? ImageHelper.GetImagePath("events/crystal_sphere/crystal_sphere_gold.png") : ImageHelper.GetImagePath("events/crystal_sphere/crystal_sphere_big_gold.png");
    }
  }

  private int Amount => !this._isBig ? 10 : 30;

  public override Vector2I Size => !this._isBig ? Vector2I.One : new Vector2I(2, 1);

  public override bool IsGood => true;

  public CrystalSphereGold(bool isBig) => this._isBig = isBig;

  public override Reward ToReward(Player owner, Rng rng)
  {
    return new GoldReward(this.Amount, owner).SetRng(rng);
  }

  public override SerializableCrystalSphereItem ToSerializable()
  {
    return new SerializableCrystalSphereItem()
    {
      type = CrystalSphereItemType.Gold,
      isBigGold = this._isBig
    };
  }
}
