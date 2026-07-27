// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.PotionPoolModel
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Unlocks;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Models;

public abstract class PotionPoolModel : AbstractModel, IPoolModel
{
  private IEnumerable<PotionModel>? _allPotions;
  private HashSet<ModelId>? _allPotionIds;

  public abstract string EnergyColorName { get; }

  public override bool ShouldReceiveCombatHooks => false;

  public virtual Color LabOutlineColor => StsColors.halfTransparentBlack;

  public IEnumerable<PotionModel> AllPotions
  {
    get
    {
      if (this._allPotions == null)
      {
        this._allPotions = this.GenerateAllPotions();
        this._allPotions = ModHelper.ConcatModelsFromMods<PotionModel>((IPoolModel) this, this._allPotions);
      }
      return this._allPotions;
    }
  }

  public IEnumerable<ModelId> AllPotionIds
  {
    get
    {
      return (IEnumerable<ModelId>) this._allPotionIds ?? (IEnumerable<ModelId>) (this._allPotionIds = this.AllPotions.Select<PotionModel, ModelId>((Func<PotionModel, ModelId>) (p => p.Id)).ToHashSet<ModelId>());
    }
  }

  protected abstract IEnumerable<PotionModel> GenerateAllPotions();

  public virtual IEnumerable<PotionModel> GetUnlockedPotions(UnlockState unlockState)
  {
    return this.AllPotions;
  }
}
