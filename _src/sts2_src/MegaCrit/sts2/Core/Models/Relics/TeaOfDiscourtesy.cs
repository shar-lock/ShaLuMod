// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.TeaOfDiscourtesy
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class TeaOfDiscourtesy : RelicModel
{
  private const string _combatsKey = "Combats";
  private const string _dazedCountKey = "DazedCount";
  private int _combatsLeft = 1;

  public override RelicRarity Rarity => RelicRarity.Event;

  public override bool IsUsedUp => this.CombatsLeft <= 0;

  public override bool ShowCounter => false;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[3]
      {
        (DynamicVar) new HealVar(1M),
        new DynamicVar("Combats", (Decimal) this.CombatsLeft),
        new DynamicVar("DazedCount", 2M)
      });
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get => HoverTipFactory.FromCardWithCardHoverTips<Dazed>();
  }

  [SavedProperty]
  private int CombatsLeft
  {
    get => this._combatsLeft;
    set
    {
      this.AssertMutable();
      this._combatsLeft = value;
      this.DynamicVars["Combats"].BaseValue = (Decimal) this._combatsLeft;
      this.InvokeDisplayAmountChanged();
      if (!this.IsUsedUp)
        return;
      this.Status = RelicStatus.Disabled;
    }
  }

  public override async Task BeforeCombatStart()
  {
    if (this.CombatsLeft <= 0)
      return;
    await CardPileCmd.AddToCombatAndPreview<Dazed>(this.Owner.Creature, PileType.Draw, this.DynamicVars["DazedCount"].IntValue, this.Owner, CardPilePosition.Random);
    this.CombatsLeft--;
    this.Flash();
  }
}
