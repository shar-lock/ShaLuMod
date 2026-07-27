// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent.CrystalSphereItem
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent.CrystalSphereItems;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rewards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent;

public abstract class CrystalSphereItem
{
  public abstract Vector2I Size { get; }

  public Vector2I Position { get; private set; }

  protected virtual string TexturePath
  {
    get
    {
      return ImageHelper.GetImagePath($"events/crystal_sphere/{StringExtensions.ToSnakeCase(this.GetType().Name)}.png");
    }
  }

  public Texture2D Texture => PreloadManager.Cache.GetTexture2D(this.TexturePath);

  public abstract bool IsGood { get; }

  public event Action<CrystalSphereItem>? Revealed;

  public bool PlaceItem(CrystalSphereMinigame game)
  {
    List<Vector2I> vector2IList = new List<Vector2I>();
    for (int x = 0; x < game.GridSize.X; ++x)
    {
      for (int y = 0; y < game.GridSize.Y; ++y)
      {
        if (this.CanPlaceHere(game.cells, x, y))
          vector2IList.Add(new Vector2I(x, y));
      }
    }
    if (!vector2IList.Any<Vector2I>())
      return false;
    this.Position = game.Rng.NextItem<Vector2I>((IEnumerable<Vector2I>) vector2IList);
    for (int index1 = 0; index1 < this.Size.X; ++index1)
    {
      for (int index2 = 0; index2 < this.Size.Y; ++index2)
      {
        int index3 = this.Position.X + index1;
        int index4 = this.Position.Y + index2;
        game.cells[index3, index4].SetItem(this);
      }
    }
    return true;
  }

  private bool CanPlaceHere(CrystalSphereCell[,] grid, int x, int y)
  {
    for (int index1 = 0; index1 < this.Size.X; ++index1)
    {
      for (int index2 = 0; index2 < this.Size.Y; ++index2)
      {
        int index3 = x + index1;
        int index4 = y + index2;
        if (index3 < 0 || index3 >= grid.GetLength(0) || index4 < 0 || index4 >= grid.GetLength(1) || !grid[index3, index4].IsHidden || grid[index3, index4].Item != null)
          return false;
      }
    }
    return true;
  }

  public virtual Task RevealItem(Player _)
  {
    Action<CrystalSphereItem> revealed = this.Revealed;
    if (revealed != null)
      revealed(this);
    return Task.CompletedTask;
  }

  public virtual Reward? ToReward(Player owner, Rng rng) => (Reward) null;

  public abstract SerializableCrystalSphereItem ToSerializable();

  public static CrystalSphereItem FromSerializable(
    SerializableCrystalSphereItem serializable,
    Player owner)
  {
    switch (serializable.type)
    {
      case CrystalSphereItemType.CardReward:
        return (CrystalSphereItem) new CrystalSphereCardReward(serializable.cardRarity, owner);
      case CrystalSphereItemType.Curse:
        return (CrystalSphereItem) new CrystalSphereCurse();
      case CrystalSphereItemType.Gold:
        return (CrystalSphereItem) new CrystalSphereGold(serializable.isBigGold);
      case CrystalSphereItemType.Potion:
        return (CrystalSphereItem) new CrystalSpherePotion(serializable.potionRarity);
      case CrystalSphereItemType.Relic:
        return (CrystalSphereItem) new CrystalSphereRelic();
      default:
        throw new ArgumentOutOfRangeException();
    }
  }
}
