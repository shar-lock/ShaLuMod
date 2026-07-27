// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.NeowsBones
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Rewards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class NeowsBones : RelicModel
{
  private const string _relicCountKey = "Relics";
  private const string _cursesCountKey = "Curses";

  public override RelicRarity Rarity => RelicRarity.Ancient;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        new DynamicVar("Relics", 2M),
        new DynamicVar("Curses", 1M)
      });
    }
  }

  private static IEnumerable<RelicModel> GetValidRelics(Player owner)
  {
    return ModelDb.Event<Neow>().AllPossibleOptions.Where<EventOption>((Func<EventOption, bool>) (o => o.Relic != null && o.Relic.IsAllowedAtNeow(owner) && !(o.Relic is NeowsBones))).Select<EventOption, RelicModel>((Func<EventOption, RelicModel>) (o => o.Relic)).OfType<RelicModel>();
  }

  public override async Task AfterObtained()
  {
    List<RelicModel> list = NeowsBones.GetValidRelics(this.Owner).ToList<RelicModel>();
    this.Owner.PlayerRng.Rewards.Shuffle<RelicModel>((IList<RelicModel>) list);
    await new RewardsSet(this.Owner).WithCustomRewards(list.Take<RelicModel>(this.DynamicVars["Relics"].IntValue).Select<RelicModel, Reward>((Func<RelicModel, Reward>) (r => (Reward) new RelicReward(r, this.Owner))).ToList<Reward>()).WithSkippingDisallowed().Offer();
    List<CardModel> availableCurses = ModelDb.CardPool<CurseCardPool>().GetUnlockedCards(this.Owner.UnlockState, this.Owner.RunState.CardMultiplayerConstraint).Where<CardModel>((Func<CardModel, bool>) (c => c.CanBeGeneratedByModifiers)).OrderBy<CardModel, ModelId>((Func<CardModel, ModelId>) (c => c.Id)).ToList<CardModel>();
    List<CardPileAddResult> curseResults = new List<CardPileAddResult>();
    for (int i = 0; i < this.DynamicVars["Curses"].IntValue; ++i)
    {
      CardModel canonicalCard = this.Owner.RunState.Rng.Niche.NextItem<CardModel>((IEnumerable<CardModel>) availableCurses);
      availableCurses.Remove(canonicalCard);
      curseResults.Add(await CardPileCmd.Add(this.Owner.RunState.CreateCard(canonicalCard, this.Owner), PileType.Deck));
    }
    CardCmd.PreviewCardPileAdd((IReadOnlyList<CardPileAddResult>) curseResults, 2f);
    await Cmd.Wait(0.75f);
    availableCurses = (List<CardModel>) null;
    curseResults = (List<CardPileAddResult>) null;
  }
}
