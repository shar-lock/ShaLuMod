// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Rewards.RewardsSet
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Odds;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Rewards;

public class RewardsSet
{
  public static Func<RewardsSet, Task>? testSelector;
  private bool _allowEmptyRewards;
  private bool _disallowSkipping;
  private bool _isGenerated;
  private readonly RewardsSetSynchronizer _synchronizer;

  public AbstractRoom? Room { get; private set; }

  public Player Player { get; }

  public List<Reward> Rewards { get; } = new List<Reward>();

  public bool ThrowInTestIfRewardsNotTaken { get; set; } = true;

  public bool DisallowSkipping => this._disallowSkipping;

  public bool AllRewardsSuccessfullySelected
  {
    get => this.Rewards.All<Reward>((Func<Reward, bool>) (r => r.SuccessfullySelected));
  }

  public int Id { get; set; } = -1;

  public RewardsSet(Player player, RewardsSetSynchronizer? synchronizer = null)
  {
    this.Player = player;
    this._synchronizer = synchronizer ?? RunManager.Instance.RewardsSetSynchronizer;
  }

  public RewardsSet EmptyForRoom(AbstractRoom room)
  {
    this.Room = room;
    this._allowEmptyRewards = true;
    return this;
  }

  public RewardsSet WithRewardsFromRoom(AbstractRoom room)
  {
    this.Room = room;
    if (room.RoomType == RoomType.Boss && this.Player.RunState.CurrentActIndex >= this.Player.RunState.Acts.Count - 1)
      return this;
    if (!this.TryGenerateTutorialRewards(this.Player, room))
      this.Rewards.AddRange((IEnumerable<Reward>) this.GenerateRewardsFor(this.Player, room));
    List<Reward> collection;
    if (this.Room is CombatRoom room1 && room1.ExtraRewards.TryGetValue(this.Player, out collection))
      this.Rewards.AddRange((IEnumerable<Reward>) collection);
    return this;
  }

  public RewardsSet WithCustomRewards(List<Reward> rewards)
  {
    this.Rewards.AddRange((IEnumerable<Reward>) rewards);
    return this;
  }

  public RewardsSet WithSkippingDisallowed()
  {
    this._disallowSkipping = true;
    return this;
  }

  public async Task GenerateWithoutOffering()
  {
    if (this._isGenerated)
      return;
    List<Reward> list = this.Rewards.ToList<Reward>();
    foreach (Reward reward in this.Rewards)
      reward.Populate();
    IEnumerable<AbstractModel> modifiers = Hook.ModifyRewards(this.Player.RunState, this.Player, this.Rewards, this.Room);
    foreach (Reward reward in this.Rewards.Except<Reward>((IEnumerable<Reward>) list))
    {
      if (!reward.IsPopulated)
        reward.Populate();
    }
    await Hook.AfterModifyingRewards(this.Player.RunState, modifiers);
    this.Rewards.Sort((Comparison<Reward>) ((x, y) => x.RewardsSetIndex.CompareTo(y.RewardsSetIndex)));
    this._isGenerated = true;
  }

  public async Task Offer()
  {
    Task task;
    if (this.Player.Creature.IsDead)
    {
      task = (Task) null;
    }
    else
    {
      await this.GenerateWithoutOffering();
      bool isTerminal = this.Room is CombatRoom;
      task = this._synchronizer.BeginRewardsSet(this);
      if (this.Rewards.Count <= 0 && !isTerminal && !this._allowEmptyRewards)
      {
        task = (Task) null;
      }
      else
      {
        if (!this.Rewards.All<Reward>((Func<Reward, bool>) (r => r.IsPopulated)) && this.Rewards.Any<Reward>((Func<Reward, bool>) (r => r.IsPopulated)))
          Log.Warn("Some rewards are populated and others are not when calling RewardsCmd.Offer! This might lead to hooks getting called twice");
        if (LocalContext.IsMe(this.Player))
        {
          if (TestMode.IsOn)
          {
            if (RewardsSet.testSelector != null)
            {
              await RewardsSet.testSelector(this);
            }
            else
            {
              foreach (Reward reward in this.Rewards)
              {
                int num = await this._synchronizer.SelectLocalReward(reward) ? 1 : 0;
              }
            }
            if (!this._synchronizer.IsRewardsSetCompleted(this) && this.ThrowInTestIfRewardsNotTaken)
              throw new InvalidOperationException("The RewardsSet is not complete after rewards were selected!");
          }
          else
            NRewardsScreen.ShowScreen(this, isTerminal, this.Player.RunState);
        }
        await task;
        task = (Task) null;
      }
    }
  }

  private List<Reward> GenerateRewardsFor(Player player, AbstractRoom room)
  {
    if (RunManager.Instance == null)
      throw new InvalidOperationException("Only valid during a run.");
    List<Reward> rewardsFor = new List<Reward>();
    switch (room)
    {
      case CombatRoom combatRoom:
        switch (room.RoomType)
        {
          case RoomType.Monster:
            if ((double) combatRoom.GoldProportion > 0.0)
              rewardsFor.Add((Reward) new GoldReward((int) Math.Round((double) combatRoom.Encounter.MinGoldReward * (double) combatRoom.GoldProportion), (int) Math.Round((double) combatRoom.Encounter.MaxGoldReward * (double) combatRoom.GoldProportion), player));
            this.RollForPotionAndAddTo((ICollection<Reward>) rewardsFor, player, room.RoomType);
            rewardsFor.Add((Reward) new CardReward(CardCreationOptions.ForRoom(player, room.RoomType).WithFlags(CardCreationFlags.IsFromCombat), 3, player));
            goto label_10;
          case RoomType.Elite:
            rewardsFor.Add((Reward) new GoldReward(combatRoom.Encounter.MinGoldReward, combatRoom.Encounter.MaxGoldReward, player));
            this.RollForPotionAndAddTo((ICollection<Reward>) rewardsFor, player, room.RoomType);
            rewardsFor.Add((Reward) new CardReward(CardCreationOptions.ForRoom(player, room.RoomType).WithFlags(CardCreationFlags.IsFromCombat), 3, player));
            rewardsFor.Add((Reward) new RelicReward(player));
            goto label_10;
          case RoomType.Boss:
            rewardsFor.Add((Reward) new GoldReward(combatRoom.Encounter.MinGoldReward, combatRoom.Encounter.MaxGoldReward, player));
            this.RollForPotionAndAddTo((ICollection<Reward>) rewardsFor, player, room.RoomType);
            rewardsFor.Add((Reward) new CardReward(CardCreationOptions.ForRoom(player, room.RoomType).WithFlags(CardCreationFlags.IsFromCombat), 3, player));
            goto label_10;
          default:
            goto label_10;
        }
      case TreasureRoom _:
label_10:
        return rewardsFor;
      default:
        throw new InvalidOperationException("Tried to generate a reward for invalid room type: " + room.GetType().Name);
    }
  }

  private void RollForPotionAndAddTo(ICollection<Reward> rewards, Player player, RoomType roomType)
  {
    PotionRewardOdds potionReward = player.PlayerOdds.PotionReward;
    AscensionManager ascensionManager = RunManager.Instance.AscensionManager;
    if (!potionReward.Roll(player, roomType))
      return;
    rewards.Add((Reward) new PotionReward(player));
  }

  private bool TryGenerateTutorialRewards(Player player, AbstractRoom room)
  {
    CardCreationOptions rerollOptions = CardCreationOptions.ForRoom(player, room.RoomType);
    if (player.UnlockState.NumberOfRuns == 0 && player.UnlockState.EpochUnlockCount() == 0 && player.Character is Ironclad && room is CombatRoom combatRoom)
    {
      int num = player.RunState.MapPointHistory.SelectMany<IReadOnlyList<MapPointHistoryEntry>, MapPointHistoryEntry>((Func<IReadOnlyList<MapPointHistoryEntry>, IEnumerable<MapPointHistoryEntry>>) (p => (IEnumerable<MapPointHistoryEntry>) p)).Count<MapPointHistoryEntry>((Func<MapPointHistoryEntry, bool>) (e => e.Rooms.FindIndex((Predicate<MapPointRoomHistoryEntry>) (r => r.RoomType == RoomType.Monster)) >= 0));
      if (room.RoomType == RoomType.Monster && num <= 7)
      {
        (CardModel[] Cards, PotionModel Potion)? tutorialMonsterRewards = RewardsSet.GetTutorialMonsterRewards(player, num - 1);
        if (!tutorialMonsterRewards.HasValue)
          return false;
        (CardModel[] Cards, PotionModel Potion) valueOrDefault = tutorialMonsterRewards.GetValueOrDefault();
        this.Rewards.Add((Reward) new GoldReward(10, 20, player));
        if (valueOrDefault.Potion != null)
          this.Rewards.Add((Reward) new PotionReward(valueOrDefault.Potion, player));
        this.Rewards.Add((Reward) new CardReward((IEnumerable<CardModel>) valueOrDefault.Cards, CardCreationSource.Encounter, player, rerollOptions));
        return true;
      }
      if (room.RoomType == RoomType.Elite)
      {
        switch (player.RunState.MapPointHistory.SelectMany<IReadOnlyList<MapPointHistoryEntry>, MapPointHistoryEntry>((Func<IReadOnlyList<MapPointHistoryEntry>, IEnumerable<MapPointHistoryEntry>>) (l => (IEnumerable<MapPointHistoryEntry>) l)).Count<MapPointHistoryEntry>((Func<MapPointHistoryEntry, bool>) (e => e.MapPointType == MapPointType.Elite)))
        {
          case 1:
            CardModel[] cardsToOffer1 = new CardModel[3]
            {
              (CardModel) player.RunState.CreateCard<Bludgeon>(player),
              (CardModel) player.RunState.CreateCard<Pyre>(player),
              (CardModel) player.RunState.CreateCard<EvilEye>(player)
            };
            this.Rewards.Add((Reward) new GoldReward(combatRoom.Encounter.MinGoldReward, combatRoom.Encounter.MaxGoldReward, player));
            this.Rewards.Add((Reward) new PotionReward(ModelDb.Potion<BlockPotion>().ToMutable(), player));
            this.Rewards.Add((Reward) RewardsSet.GetTutorialRelicReward<Vajra>(player));
            this.Rewards.Add((Reward) new CardReward((IEnumerable<CardModel>) cardsToOffer1, CardCreationSource.Encounter, player, rerollOptions));
            return true;
          case 2:
            CardModel[] cardsToOffer2 = new CardModel[3]
            {
              (CardModel) player.RunState.CreateCard<Pillage>(player),
              (CardModel) player.RunState.CreateCard<Rampage>(player),
              (CardModel) player.RunState.CreateCard<FlameBarrier>(player)
            };
            this.Rewards.Add((Reward) new GoldReward(combatRoom.Encounter.MinGoldReward, combatRoom.Encounter.MaxGoldReward, player));
            this.Rewards.Add((Reward) RewardsSet.GetTutorialRelicReward<OrnamentalFan>(player));
            this.Rewards.Add((Reward) new CardReward((IEnumerable<CardModel>) cardsToOffer2, CardCreationSource.Encounter, player, rerollOptions));
            return true;
        }
      }
      else if (room.RoomType == RoomType.Boss && player.RunState.MapPointHistory.SelectMany<IReadOnlyList<MapPointHistoryEntry>, MapPointHistoryEntry>((Func<IReadOnlyList<MapPointHistoryEntry>, IEnumerable<MapPointHistoryEntry>>) (l => (IEnumerable<MapPointHistoryEntry>) l)).Count<MapPointHistoryEntry>((Func<MapPointHistoryEntry, bool>) (e => e.MapPointType == MapPointType.Boss)) == 1)
      {
        CardModel[] cardsToOffer3 = new CardModel[3]
        {
          (CardModel) player.RunState.CreateCard<PrimalForce>(player),
          (CardModel) player.RunState.CreateCard<DemonForm>(player),
          (CardModel) player.RunState.CreateCard<Thrash>(player)
        };
        this.Rewards.Add((Reward) new GoldReward(combatRoom.Encounter.MinGoldReward, combatRoom.Encounter.MaxGoldReward, player));
        this.Rewards.Add((Reward) new CardReward((IEnumerable<CardModel>) cardsToOffer3, CardCreationSource.Encounter, player, rerollOptions));
        return true;
      }
    }
    return false;
  }

  private static RelicReward GetTutorialRelicReward<T>(Player player) where T : RelicModel
  {
    return player.RelicGrabBag.Contains((RelicModel) ModelDb.Relic<T>()) ? new RelicReward(ModelDb.Relic<T>().ToMutable(), player) : new RelicReward(player);
  }

  public static (CardModel[] Cards, PotionModel? Potion)? GetTutorialMonsterRewards(
    Player player,
    int index)
  {
    (CardModel[], PotionModel)? tutorialMonsterRewards;
    switch (index)
    {
      case 0:
        tutorialMonsterRewards = new (CardModel[], PotionModel)?((new CardModel[3]
        {
          (CardModel) player.RunState.CreateCard<SetupStrike>(player),
          (CardModel) player.RunState.CreateCard<Tremble>(player),
          (CardModel) player.RunState.CreateCard<BloodWall>(player)
        }, (PotionModel) null));
        break;
      case 1:
        tutorialMonsterRewards = new (CardModel[], PotionModel)?((new CardModel[3]
        {
          (CardModel) player.RunState.CreateCard<Breakthrough>(player),
          (CardModel) player.RunState.CreateCard<Inflame>(player),
          (CardModel) player.RunState.CreateCard<Anger>(player)
        }, (PotionModel) null));
        break;
      case 2:
        tutorialMonsterRewards = new (CardModel[], PotionModel)?((new CardModel[3]
        {
          (CardModel) player.RunState.CreateCard<IronWave>(player),
          (CardModel) player.RunState.CreateCard<Dismantle>(player),
          (CardModel) player.RunState.CreateCard<Cinder>(player)
        }, ModelDb.Potion<FirePotion>().ToMutable()));
        break;
      case 3:
        tutorialMonsterRewards = new (CardModel[], PotionModel)?((new CardModel[3]
        {
          (CardModel) player.RunState.CreateCard<Stomp>(player),
          (CardModel) player.RunState.CreateCard<ShrugItOff>(player),
          (CardModel) player.RunState.CreateCard<Armaments>(player)
        }, (PotionModel) null));
        break;
      case 4:
        tutorialMonsterRewards = new (CardModel[], PotionModel)?((new CardModel[3]
        {
          (CardModel) player.RunState.CreateCard<Thunderclap>(player),
          (CardModel) player.RunState.CreateCard<SetupStrike>(player),
          (CardModel) player.RunState.CreateCard<Rage>(player)
        }, ModelDb.Potion<StrengthPotion>().ToMutable()));
        break;
      case 5:
        tutorialMonsterRewards = new (CardModel[], PotionModel)?((new CardModel[3]
        {
          (CardModel) player.RunState.CreateCard<BattleTrance>(player),
          (CardModel) player.RunState.CreateCard<TrueGrit>(player),
          (CardModel) player.RunState.CreateCard<Uppercut>(player)
        }, (PotionModel) null));
        break;
      case 6:
        tutorialMonsterRewards = new (CardModel[], PotionModel)?((new CardModel[3]
        {
          (CardModel) player.RunState.CreateCard<Bloodletting>(player),
          (CardModel) player.RunState.CreateCard<Whirlwind>(player),
          (CardModel) player.RunState.CreateCard<Tremble>(player)
        }, ModelDb.Potion<EnergyPotion>().ToMutable()));
        break;
      default:
        tutorialMonsterRewards = new (CardModel[], PotionModel)?();
        break;
    }
    return tutorialMonsterRewards;
  }

  public override string ToString()
  {
    return $"Id: {this.Id} Owner: {this.Player.NetId} Rewards: {string.Join<Reward>(",", (IEnumerable<Reward>) this.Rewards)}";
  }
}
