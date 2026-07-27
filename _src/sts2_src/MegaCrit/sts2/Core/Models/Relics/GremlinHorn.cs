// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.GremlinHorn
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class GremlinHorn : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Uncommon;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new EnergyVar(1),
        (DynamicVar) new CardsVar(1)
      });
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy((RelicModel) this));
    }
  }

  public override async Task AfterDeath(
    PlayerChoiceContext choiceContext,
    Creature target,
    bool wasRemovalPrevented,
    float deathAnimLength)
  {
    if (target.Side == this.Owner.Creature.Side)
      return;
    this.Flash();
    await PlayerCmd.GainEnergy(this.DynamicVars.Energy.BaseValue, this.Owner);
    IEnumerable<CardModel> cardModels = await CardPileCmd.Draw(choiceContext, this.DynamicVars.Cards.BaseValue, this.Owner);
  }
}
