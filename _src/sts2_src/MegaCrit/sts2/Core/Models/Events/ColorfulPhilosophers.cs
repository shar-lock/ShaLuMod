// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.ColorfulPhilosophers
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class ColorfulPhilosophers : EventModel
{
  private static IEnumerable<CardPoolModel> CardPoolColorOrder
  {
    get
    {
      return (IEnumerable<CardPoolModel>) new \u003C\u003Ez__ReadOnlyArray<CardPoolModel>(new CardPoolModel[5]
      {
        (CardPoolModel) ModelDb.CardPool<NecrobinderCardPool>(),
        (CardPoolModel) ModelDb.CardPool<IroncladCardPool>(),
        (CardPoolModel) ModelDb.CardPool<RegentCardPool>(),
        (CardPoolModel) ModelDb.CardPool<SilentCardPool>(),
        (CardPoolModel) ModelDb.CardPool<DefectCardPool>()
      });
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new CardsVar(3));
    }
  }

  public override bool IsAllowed(IRunState runState)
  {
    return runState.Players.All<Player>((Func<Player, bool>) (p => p.UnlockState.CharacterCardPools.Count<CardPoolModel>() > 1));
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    List<EventOption> initialOptions = new List<EventOption>();
    CharacterModel character = this.Owner.Character;
    List<CardPoolModel> list = this.Owner.UnlockState.CharacterCardPools.ToList<CardPoolModel>();
    foreach (CardPoolModel cardPoolModel in ColorfulPhilosophers.CardPoolColorOrder)
    {
      CardPoolModel cardPool = cardPoolModel;
      if (character.CardPool != cardPool && list.Contains(cardPool))
        initialOptions.Add(new EventOption((EventModel) this, (Func<Task>) (() => this.OfferRewards(cardPool)), "COLORFUL_PHILOSOPHERS.pages.INITIAL.options." + cardPool.EnergyColorName.ToUpperInvariant(), Array.Empty<IHoverTip>()));
    }
    int num = Mathf.Min(3, initialOptions.Count);
    while (initialOptions.Count > num)
      initialOptions.RemoveAt(this.Rng.NextInt(initialOptions.Count));
    return (IReadOnlyList<EventOption>) initialOptions;
  }

  private async Task OfferRewards(CardPoolModel pool)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: object of a compiler-generated type is created
    await RewardsCmd.OfferCustom(this.Owner, new List<Reward>(3)
    {
      (Reward) new CardReward(new CardCreationOptions((IEnumerable<CardPoolModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardPoolModel>(pool), CardCreationSource.Other, CardRarityOddsType.Uniform, (Func<CardModel, bool>) (c => c.Rarity == CardRarity.Common)).WithFlags(CardCreationFlags.NoRarityModification | CardCreationFlags.NoCardPoolModifications), this.DynamicVars.Cards.IntValue, this.Owner),
      (Reward) new CardReward(new CardCreationOptions((IEnumerable<CardPoolModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardPoolModel>(pool), CardCreationSource.Other, CardRarityOddsType.Uniform, (Func<CardModel, bool>) (c => c.Rarity == CardRarity.Uncommon)).WithFlags(CardCreationFlags.NoRarityModification | CardCreationFlags.NoCardPoolModifications), this.DynamicVars.Cards.IntValue, this.Owner),
      (Reward) new CardReward(new CardCreationOptions((IEnumerable<CardPoolModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardPoolModel>(pool), CardCreationSource.Other, CardRarityOddsType.Uniform, (Func<CardModel, bool>) (c => c.Rarity == CardRarity.Rare)).WithFlags(CardCreationFlags.NoRarityModification | CardCreationFlags.NoCardPoolModifications), this.DynamicVars.Cards.IntValue, this.Owner)
    });
    this.SetEventFinished(this.L10NLookup("COLORFUL_PHILOSOPHERS.pages.DONE.description"));
  }
}
