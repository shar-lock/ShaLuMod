// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Potions.Mocks.MockDiscardAndAddShivsPotion
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Potions.Mocks;

public sealed class MockDiscardAndAddShivsPotion : PotionModel
{
  private const string _shivKey = "Shivs";

  public override bool IsMock => true;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new CardsVar(2),
        new DynamicVar("Shivs", 2M)
      });
    }
  }

  public override PotionRarity Rarity => PotionRarity.None;

  public override PotionUsage Usage => PotionUsage.CombatOnly;

  public override TargetType TargetType => TargetType.Self;

  protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
  {
    CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt, this.DynamicVars.Cards.IntValue);
    await CardCmd.Discard(choiceContext, await CardSelectCmd.FromHandForDiscard(choiceContext, this.Owner, prefs, (Func<CardModel, bool>) null, (AbstractModel) this));
    IEnumerable<CardModel> inHand = await Shiv.CreateInHand(this.Owner, this.DynamicVars["Shivs"].IntValue, this.Owner.Creature.CombatState);
  }
}
