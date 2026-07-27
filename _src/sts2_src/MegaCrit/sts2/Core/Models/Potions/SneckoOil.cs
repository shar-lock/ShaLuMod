// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Potions.SneckoOil
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Potions;

public sealed class SneckoOil : PotionModel
{
  private int _testEnergyCostOverride = -1;

  public override PotionRarity Rarity => PotionRarity.Rare;

  public override PotionUsage Usage => PotionUsage.CombatOnly;

  public override TargetType TargetType => TargetType.AnyPlayer;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new CardsVar(7));
    }
  }

  public int TestEnergyCostOverride
  {
    get => this._testEnergyCostOverride;
    set
    {
      TestMode.AssertOn();
      this.AssertMutable();
      this._testEnergyCostOverride = value;
    }
  }

  protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
  {
    PotionModel.AssertValidForTargetedPotion(target);
    NCombatRoom.Instance?.PlaySplashVfx(target, new Color("6ec46f"));
    IEnumerable<CardModel> cardModels = await CardPileCmd.Draw(choiceContext, this.DynamicVars.Cards.BaseValue, target.Player);
    foreach (CardModel card in PileType.Hand.GetPile(target.Player).Cards.Where<CardModel>((Func<CardModel, bool>) (c => !c.EnergyCost.CostsX)))
    {
      if (card.EnergyCost.GetWithModifiers(CostModifiers.None) >= 0)
      {
        card.EnergyCost.SetThisTurnOrUntilPlayed(this.NextEnergyCost());
        NCard.FindOnTable(card)?.PlayRandomizeCostAnim();
      }
    }
  }

  private int NextEnergyCost()
  {
    return this.TestEnergyCostOverride >= 0 ? this.TestEnergyCostOverride : this.Owner.RunState.Rng.CombatEnergyCosts.NextInt(4);
  }
}
