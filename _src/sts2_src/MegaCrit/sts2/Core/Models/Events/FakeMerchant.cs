// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.FakeMerchant
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Events.Custom;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class FakeMerchant : EventModel
{
  public const int relicCost = 50;
  private static readonly RelicModel[] _inventoryRelics = new RelicModel[9]
  {
    (RelicModel) ModelDb.Relic<FakeAnchor>(),
    (RelicModel) ModelDb.Relic<FakeBloodVial>(),
    (RelicModel) ModelDb.Relic<FakeHappyFlower>(),
    (RelicModel) ModelDb.Relic<FakeLeesWaffle>(),
    (RelicModel) ModelDb.Relic<FakeMango>(),
    (RelicModel) ModelDb.Relic<FakeOrichalcum>(),
    (RelicModel) ModelDb.Relic<FakeSneckoEye>(),
    (RelicModel) ModelDb.Relic<FakeStrikeDummy>(),
    (RelicModel) ModelDb.Relic<FakeVenerableTeaSet>()
  };
  private static MerchantDialogueSet? _dialogue;
  private MerchantInventory? _inventory;
  private bool _startedFight;

  public static MerchantDialogueSet Dialogue
  {
    get
    {
      if (FakeMerchant._dialogue != null)
        return FakeMerchant._dialogue;
      FakeMerchant._dialogue = MerchantDialogueSet.CreateFromLocStrings((IEnumerable<LocString>) LocManager.Instance.GetTable("events").GetLocStringsWithPrefix(StringHelper.Slugify(nameof (FakeMerchant)) + ".talk."));
      return FakeMerchant._dialogue;
    }
  }

  public override EventLayoutType LayoutType => EventLayoutType.Custom;

  public override bool IsShared => true;

  public MerchantInventory Inventory
  {
    get => this._inventory;
    private set
    {
      this.AssertMutable();
      this._inventory = value;
    }
  }

  public bool StartedFight
  {
    get => this._startedFight;
    private set
    {
      this.AssertMutable();
      this._startedFight = value;
    }
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    return (IReadOnlyList<EventOption>) Array.Empty<EventOption>();
  }

  public override IEnumerable<LocString> GameInfoOptions
  {
    get => (IEnumerable<LocString>) Array.Empty<LocString>();
  }

  public override bool IsAllowed(IRunState runState)
  {
    return runState.CurrentActIndex >= 1 && runState.Players.Count <= 1 && runState.Players.All<Player>((Func<Player, bool>) (player => player.Gold >= 100 || player.Potions.Any<PotionModel>((Func<PotionModel, bool>) (potion => potion is FoulPotion))));
  }

  protected override Task BeforeEventStarted(bool isPreFinished)
  {
    this.Inventory = new MerchantInventory(this.Owner);
    foreach (RelicModel relicModel in ((IEnumerable<RelicModel>) FakeMerchant._inventoryRelics).ToList<RelicModel>().UnstableShuffle<RelicModel>(this.Rng).Take<RelicModel>(6).ToList<RelicModel>())
      this.Inventory.AddRelicEntry(new MerchantRelicEntry(relicModel.ToMutable(), this.Owner));
    return Task.CompletedTask;
  }

  protected override void OnEventFinished()
  {
    if (this.StartedFight)
      return;
    PlayerMapPointHistoryEntry entry = this.Owner.RunState.CurrentMapPointHistoryEntry?.GetEntry(this.Owner.NetId);
    if (entry == null)
      return;
    foreach (MerchantRelicEntry relicEntry in (IEnumerable<MerchantRelicEntry>) this.Inventory.RelicEntries)
    {
      if (relicEntry.IsStocked)
        entry.RelicChoices.Add(new ModelChoiceHistoryEntry(relicEntry.Model.Id, false));
    }
  }

  public async Task FoulPotionThrown(FoulPotion potion)
  {
    if (LocalContext.IsMine((EventModel) this) && this.Node is NFakeMerchant node)
      await node.FoulPotionThrown();
    this.StartedFight = true;
    List<Reward> extraRewards = new List<Reward>();
    extraRewards.Add((Reward) new RelicReward(ModelDb.Relic<FakeMerchantsRug>().ToMutable(), this.Owner));
    foreach (MerchantRelicEntry relicEntry in (IEnumerable<MerchantRelicEntry>) ((FakeMerchant) RunManager.Instance.EventSynchronizer.GetEventForPlayer(this.Owner)).Inventory.RelicEntries)
    {
      if (relicEntry.IsStocked || this.Owner.RunState.Players.Count > 1)
        extraRewards.Add((Reward) new RelicReward(relicEntry.Model, this.Owner));
    }
    this.EnterCombatWithoutExitingEvent<FakeMerchantEventEncounter>((IReadOnlyList<Reward>) extraRewards, false);
  }
}
