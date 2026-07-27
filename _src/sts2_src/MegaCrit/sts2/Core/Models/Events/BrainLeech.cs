// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.BrainLeech
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
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class BrainLeech : EventModel
{
  private const string _ripHpLossKey = "RipHpLoss";
  private const string _rewardCountKey = "RewardCount";
  private const string _cardChoiceCountKey = "CardChoiceCount";
  private const string _fromCardChoiceCountKey = "FromCardChoiceCount";

  public override bool IsAllowed(IRunState runState) => runState.CurrentActIndex < 2;

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      new EventOption((EventModel) this, new Func<Task>(this.ShareKnowledge), "BRAIN_LEECH.pages.INITIAL.options.SHARE_KNOWLEDGE", Array.Empty<IHoverTip>()),
      new EventOption((EventModel) this, new Func<Task>(this.Rip), "BRAIN_LEECH.pages.INITIAL.options.RIP", Array.Empty<IHoverTip>()).ThatDoesDamage(this.DynamicVars["RipHpLoss"].BaseValue)
    });
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[4]
      {
        (DynamicVar) new DamageVar("RipHpLoss", 5M, ValueProp.Unblockable | ValueProp.Unpowered),
        (DynamicVar) new IntVar("RewardCount", 1M),
        (DynamicVar) new IntVar("CardChoiceCount", 1M),
        (DynamicVar) new IntVar("FromCardChoiceCount", 5M)
      });
    }
  }

  private async Task Rip()
  {
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Owner.Creature, (DamageVar) this.DynamicVars["RipHpLoss"], (Creature) null, (CardModel) null, (CardPlay) null);
    for (int i = 0; i < this.DynamicVars["RewardCount"].IntValue; ++i)
    {
      // ISSUE: object of a compiler-generated type is created
      await RewardsCmd.OfferCustom(this.Owner, new List<Reward>(1)
      {
        (Reward) new CardReward(CardCreationOptions.ForNonCombatWithDefaultOdds((IEnumerable<CardPoolModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardPoolModel>((CardPoolModel) ModelDb.CardPool<ColorlessCardPool>())).WithFlags(CardCreationFlags.NoRarityModification | CardCreationFlags.NoCardPoolModifications), 3, this.Owner)
      });
    }
    this.SetEventFinished(this.L10NLookup("BRAIN_LEECH.pages.RIP.description"));
  }

  private async Task ShareKnowledge()
  {
    Player owner = this.Owner;
    // ISSUE: object of a compiler-generated type is created
    await this.SelectCardsToAddToDeckFromGrid(CardFactory.CreateForReward(owner, this.DynamicVars["FromCardChoiceCount"].IntValue, CardCreationOptions.ForNonCombatWithDefaultOdds((IEnumerable<CardPoolModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardPoolModel>(owner.Character.CardPool))).ToList<CardCreationResult>(), new CardSelectorPrefs(this.L10NLookup("BRAIN_LEECH.pages.SHARE_KNOWLEDGE.selectionScreenPrompt"), 1)
    {
      Cancelable = false
    });
    this.SetEventFinished(this.L10NLookup("BRAIN_LEECH.pages.SHARE_KNOWLEDGE.description"));
  }
}
