// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.RoomFullOfCheese
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class RoomFullOfCheese : EventModel
{
  public override bool IsAllowed(IRunState runState) => runState.CurrentActIndex < 2;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new DamageVar(14M, ValueProp.Unblockable | ValueProp.Unpowered));
    }
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      new EventOption((EventModel) this, new Func<Task>(this.Gorge), "ROOM_FULL_OF_CHEESE.pages.INITIAL.options.GORGE", Array.Empty<IHoverTip>()),
      new EventOption((EventModel) this, new Func<Task>(this.Search), "ROOM_FULL_OF_CHEESE.pages.INITIAL.options.SEARCH", HoverTipFactory.FromRelic<ChosenCheese>()).ThatDoesDamage(this.DynamicVars.Damage.BaseValue)
    });
  }

  private async Task Gorge()
  {
    Player owner = this.Owner;
    // ISSUE: object of a compiler-generated type is created
    CardCreationOptions options = CardCreationOptions.ForNonCombatWithUniformOdds((IEnumerable<CardPoolModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardPoolModel>(owner.Character.CardPool), (Func<CardModel, bool>) (c => c.Rarity == CardRarity.Common)).WithFlags(CardCreationFlags.NoRarityModification);
    await this.SelectCardsToAddToDeckFromGrid(CardFactory.CreateForReward(owner, 8, options).ToList<CardCreationResult>(), new CardSelectorPrefs(this.L10NLookup("ROOM_FULL_OF_CHEESE.pages.GORGE.selectionScreenPrompt"), 2));
    this.SetEventFinished(this.L10NLookup("ROOM_FULL_OF_CHEESE.pages.GORGE.description"));
  }

  private async Task Search()
  {
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Owner.Creature, this.DynamicVars.Damage, (Creature) null, (CardModel) null, (CardPlay) null);
    ChosenCheese chosenCheese = await RelicCmd.Obtain<ChosenCheese>(this.Owner);
    this.SetEventFinished(this.L10NLookup("ROOM_FULL_OF_CHEESE.pages.SEARCH.description"));
  }
}
