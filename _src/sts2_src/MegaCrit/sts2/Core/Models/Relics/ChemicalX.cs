// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.ChemicalX
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class ChemicalX : RelicModel
{
  private const string _increaseKey = "Increase";

  public override RelicRarity Rarity => RelicRarity.Shop;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Increase", 2M));
    }
  }

  public override Task BeforeCardPlayed(CardPlay cardPlay)
  {
    if (cardPlay.Card.Owner != this.Owner || !cardPlay.Card.EnergyCost.CostsX && !cardPlay.Card.HasStarCostX)
      return Task.CompletedTask;
    this.Flash();
    return Task.CompletedTask;
  }

  public override int ModifyXValue(CardModel card, int originalValue)
  {
    return this.Owner != card.Owner ? originalValue : originalValue + this.DynamicVars["Increase"].IntValue;
  }
}
