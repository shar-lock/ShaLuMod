// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Helpers.EnergyIconHelper
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

#nullable enable
namespace MegaCrit.Sts2.Core.Helpers;

public static class EnergyIconHelper
{
  public static string GetPrefix(AbstractModel model)
  {
    return EnergyIconHelper.GetPool(model).EnergyColorName;
  }

  public static string GetPath(AbstractModel model)
  {
    return EnergyIconHelper.GetPath(EnergyIconHelper.GetPrefix(model));
  }

  public static string GetPath(string prefix)
  {
    return ImageHelper.GetImagePath($"atlases/ui_atlas.sprites/card/energy_{prefix.ToLowerInvariant()}.tres");
  }

  private static IPoolModel GetPool(AbstractModel model)
  {
    if (model is IPoolModel pool1)
      return pool1;
    Player player = (Player) null;
    IPoolModel pool2 = (IPoolModel) null;
    if (!(model is CardModel cardModel))
    {
      if (!(model is EnchantmentModel enchantmentModel))
      {
        if (!(model is PotionModel potionModel))
        {
          if (!(model is RelicModel relicModel))
          {
            if (model is PowerModel powerModel)
            {
              if (powerModel != null && powerModel.IsMutable)
              {
                Creature owner = powerModel.Owner;
                if (owner != null && owner.IsPlayer)
                {
                  player = powerModel.Owner.Player;
                  goto label_23;
                }
              }
              pool2 = (IPoolModel) ModelDb.CardPool<ColorlessCardPool>();
            }
          }
          else
          {
            if (relicModel.IsMutable)
              player = relicModel.Owner;
            pool2 = (IPoolModel) relicModel.Pool;
          }
        }
        else
        {
          if (potionModel.IsMutable)
            player = potionModel.Owner;
          pool2 = (IPoolModel) potionModel.Pool;
        }
      }
      else if (enchantmentModel.HasCard)
      {
        player = enchantmentModel.Card.Owner;
        pool2 = (IPoolModel) enchantmentModel.Card.Pool;
      }
      else
        pool2 = (IPoolModel) ModelDb.CardPool<ColorlessCardPool>();
    }
    else
    {
      if (cardModel.IsMutable)
        player = cardModel.Owner;
      pool2 = (IPoolModel) cardModel.Pool;
    }
label_23:
    if (player != null)
      return (IPoolModel) player.Character.CardPool;
    if (pool2 != null)
      return pool2;
    Log.Error($"Model {model.Id} is not in any pool! It was probably deprecated without being removed.");
    return (IPoolModel) ModelDb.CardPool<IroncladCardPool>();
  }
}
