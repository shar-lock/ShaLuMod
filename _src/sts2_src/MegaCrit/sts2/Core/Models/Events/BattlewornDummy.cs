// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.BattlewornDummy
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Models.PotionPools;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class BattlewornDummy : EventModel
{
  private const string _setting1HpKey = "Setting1Hp";
  private const string _setting2HpKey = "Setting2Hp";
  private const string _setting3HpKey = "Setting3Hp";

  public override bool IsShared => true;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[3]
      {
        new DynamicVar("Setting1Hp", (Decimal) ModelDb.Monster<BattleFriendV1>().MinInitialHp),
        new DynamicVar("Setting2Hp", (Decimal) ModelDb.Monster<BattleFriendV2>().MinInitialHp),
        new DynamicVar("Setting3Hp", (Decimal) ModelDb.Monster<BattleFriendV3>().MinInitialHp)
      });
    }
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    Player owner1 = this.Owner;
    int playerCount = owner1 != null ? owner1.RunState.Players.Count : 1;
    Player owner2 = this.Owner;
    int currentActIndex = owner2 != null ? owner2.RunState.CurrentActIndex : 0;
    this.DynamicVars["Setting1Hp"].BaseValue = Creature.ScaleHpForMultiplayer((Decimal) ModelDb.Monster<BattleFriendV1>().MinInitialHp, (EncounterModel) ModelDb.Encounter<BattlewornDummyEventV1Encounter>(), playerCount, currentActIndex);
    this.DynamicVars["Setting2Hp"].BaseValue = Creature.ScaleHpForMultiplayer((Decimal) ModelDb.Monster<BattleFriendV2>().MinInitialHp, (EncounterModel) ModelDb.Encounter<BattlewornDummyEventV1Encounter>(), playerCount, currentActIndex);
    this.DynamicVars["Setting3Hp"].BaseValue = Creature.ScaleHpForMultiplayer((Decimal) ModelDb.Monster<BattleFriendV3>().MinInitialHp, (EncounterModel) ModelDb.Encounter<BattlewornDummyEventV1Encounter>(), playerCount, currentActIndex);
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[3]
    {
      new EventOption((EventModel) this, new Func<Task>(this.Setting1), "BATTLEWORN_DUMMY.pages.INITIAL.options.SETTING_1", Array.Empty<IHoverTip>()),
      new EventOption((EventModel) this, new Func<Task>(this.Setting2), "BATTLEWORN_DUMMY.pages.INITIAL.options.SETTING_2", Array.Empty<IHoverTip>()),
      new EventOption((EventModel) this, new Func<Task>(this.Setting3), "BATTLEWORN_DUMMY.pages.INITIAL.options.SETTING_3", Array.Empty<IHoverTip>())
    });
  }

  private Task Setting1()
  {
    this.EnterCombatWithoutExitingEvent((EncounterModel) ModelDb.Encounter<BattlewornDummyEventV1Encounter>(), (IReadOnlyList<Reward>) Array.Empty<Reward>(), true);
    return Task.CompletedTask;
  }

  private Task Setting2()
  {
    this.EnterCombatWithoutExitingEvent((EncounterModel) ModelDb.Encounter<BattlewornDummyEventV2Encounter>(), (IReadOnlyList<Reward>) Array.Empty<Reward>(), true);
    return Task.CompletedTask;
  }

  private Task Setting3()
  {
    this.EnterCombatWithoutExitingEvent((EncounterModel) ModelDb.Encounter<BattlewornDummyEventV3Encounter>(), (IReadOnlyList<Reward>) Array.Empty<Reward>(), true);
    return Task.CompletedTask;
  }

  public override async Task Resume(AbstractRoom room)
  {
    CombatRoom combatRoom = (CombatRoom) room;
    BattlewornDummyEventEncounter encounter = (BattlewornDummyEventEncounter) combatRoom.Encounter;
    if (encounter.RanOutOfTime)
    {
      this.SetEventFinished(this.L10NLookup("BATTLEWORN_DUMMY.pages.DEFEAT.description"));
    }
    else
    {
      this.SetEventFinished(this.L10NLookup("BATTLEWORN_DUMMY.pages.VICTORY.description"));
      List<Reward> rewardList = new List<Reward>();
      switch (encounter)
      {
        case BattlewornDummyEventV1Encounter _:
          PotionModel potionModel = this.Owner.PlayerRng.Rewards.NextItem<PotionModel>(this.Owner.Character.PotionPool.GetUnlockedPotions(this.Owner.UnlockState).Concat<PotionModel>(ModelDb.PotionPool<SharedPotionPool>().GetUnlockedPotions(this.Owner.UnlockState)));
          if (potionModel != null)
          {
            rewardList.Add((Reward) new PotionReward(potionModel.ToMutable(), this.Owner));
            break;
          }
          break;
        case BattlewornDummyEventV2Encounter _:
          using (IEnumerator<CardModel> enumerator = PileType.Deck.GetPile(this.Owner).Cards.Where<CardModel>((Func<CardModel, bool>) (c => c != null && c.IsUpgradable)).ToList<CardModel>().StableShuffle<CardModel>(this.Rng).Take<CardModel>(2).GetEnumerator())
          {
            while (enumerator.MoveNext())
              CardCmd.Upgrade(enumerator.Current);
            break;
          }
        case BattlewornDummyEventV3Encounter _:
          RelicModel mutable = RelicFactory.PullNextRelicFromFront(this.Owner).ToMutable();
          rewardList.Add((Reward) new RelicReward(mutable, this.Owner));
          break;
      }
      if (combatRoom.ExtraRewards.ContainsKey(this.Owner))
        rewardList = rewardList.Concat<Reward>((IEnumerable<Reward>) combatRoom.ExtraRewards[this.Owner]).ToList<Reward>();
      if (rewardList.Count <= 0)
        return;
      await RewardsCmd.OfferCustom(this.Owner, rewardList);
    }
  }
}
