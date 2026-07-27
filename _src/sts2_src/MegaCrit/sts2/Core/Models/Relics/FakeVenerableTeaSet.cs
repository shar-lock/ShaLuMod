// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.FakeVenerableTeaSet
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class FakeVenerableTeaSet : RelicModel
{
  private bool _gainEnergyInNextCombat;

  public override RelicRarity Rarity => RelicRarity.Event;

  public override int MerchantCost => 50;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new EnergyVar(1));
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy((RelicModel) this));
    }
  }

  [SavedProperty]
  public bool GainEnergyInNextCombat
  {
    get => this._gainEnergyInNextCombat;
    set
    {
      this.AssertMutable();
      if (this._gainEnergyInNextCombat == value)
        return;
      this._gainEnergyInNextCombat = value;
      this.Status = this._gainEnergyInNextCombat ? RelicStatus.Active : RelicStatus.Normal;
    }
  }

  public override Task AfterRoomEntered(AbstractRoom room)
  {
    if (!(room is RestSiteRoom))
      return Task.CompletedTask;
    this.GainEnergyInNextCombat = true;
    return Task.CompletedTask;
  }

  public override async Task AfterEnergyReset(Player player)
  {
    if (this.Owner != player || !this.GainEnergyInNextCombat)
      return;
    this.Flash();
    await PlayerCmd.GainEnergy(this.DynamicVars.Energy.BaseValue, this.Owner);
    this.GainEnergyInNextCombat = false;
  }
}
