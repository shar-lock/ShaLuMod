// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent.CrystalSphereItems.CrystalSphereRelic
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

public class CrystalSphereRelic : CrystalSphereItem
{
  public override Vector2I Size => new Vector2I(4, 4);

  public override bool IsGood => true;

  protected override string TexturePath
  {
    get => ImageHelper.GetImagePath("events/crystal_sphere/crystal_sphere_relic.png");
  }

  public override Reward ToReward(Player owner, Rng rng) => new RelicReward(owner).SetRng(rng);

  public override SerializableCrystalSphereItem ToSerializable()
  {
    return new SerializableCrystalSphereItem()
    {
      type = CrystalSphereItemType.Relic
    };
  }
}
