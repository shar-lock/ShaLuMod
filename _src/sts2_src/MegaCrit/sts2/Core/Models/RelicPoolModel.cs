// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.RelicPoolModel
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

public abstract class RelicPoolModel : AbstractModel, IPoolModel
{
  private IEnumerable<RelicModel>? _relics;
  private HashSet<ModelId>? _allRelicIds;

  public abstract string EnergyColorName { get; }

  public virtual Color LabOutlineColor => StsColors.halfTransparentBlack;

  public IEnumerable<RelicModel> AllRelics
  {
    get
    {
      if (this._relics == null)
      {
        this._relics = this.GenerateAllRelics();
        this._relics = ModHelper.ConcatModelsFromMods<RelicModel>((IPoolModel) this, this._relics);
      }
      return this._relics;
    }
  }

  public HashSet<ModelId> AllRelicIds
  {
    get
    {
      return this._allRelicIds ?? (this._allRelicIds = this.AllRelics.Select<RelicModel, ModelId>((Func<RelicModel, ModelId>) (c => c.Id)).ToHashSet<ModelId>());
    }
  }

  protected abstract IEnumerable<RelicModel> GenerateAllRelics();

  public virtual IEnumerable<RelicModel> GetUnlockedRelics(UnlockState unlockState)
  {
    return this.AllRelics;
  }

  public override bool ShouldReceiveCombatHooks => false;
}
