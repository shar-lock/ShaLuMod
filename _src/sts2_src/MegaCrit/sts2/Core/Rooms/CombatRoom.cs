// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Rooms.CombatRoom
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Rooms;

public class CombatRoom : AbstractRoom, ICombatRoomVisuals
{
  private bool _isPreFinished;
  private readonly Dictionary<Player, List<Reward>> _extraRewards = new Dictionary<Player, List<Reward>>();

  public override RoomType RoomType => this.Encounter.RoomType;

  public override ModelId ModelId => this.Encounter.Id;

  public EncounterModel Encounter => this.CombatState.Encounter;

  public CombatState CombatState { get; }

  public IEnumerable<Creature> Allies => (IEnumerable<Creature>) this.CombatState.Allies;

  public IEnumerable<Creature> Enemies => (IEnumerable<Creature>) this.CombatState.Enemies;

  public ActModel Act => this.CombatState.RunState.Act;

  public override bool IsPreFinished => this._isPreFinished;

  public float GoldProportion { get; private set; } = 1f;

  public IReadOnlyDictionary<Player, List<Reward>> ExtraRewards
  {
    get => (IReadOnlyDictionary<Player, List<Reward>>) this._extraRewards;
  }

  public bool ShouldCreateCombat { get; init; } = true;

  public bool ShouldResumeParentEventAfterCombat { get; init; } = true;

  public ModelId? ParentEventId { get; init; }

  public CombatRoom(EncounterModel encounter, IRunState? runState)
  {
    encounter.AssertMutable();
    this.CombatState = new CombatState(encounter, runState, runState?.Modifiers, runState?.BadgeModels, runState?.MultiplayerScalingModel);
  }

  public CombatRoom(CombatState combatState) => this.CombatState = combatState;

  public static CombatRoom FromSerializable(SerializableRoom serializableRoom, IRunState? runState)
  {
    if (serializableRoom.ExtraRewards.Count > 0 && runState == null)
      throw new InvalidOperationException("Cannot load extra rewards without a run state.");
    EncounterModel mutable = SaveUtil.EncounterOrDeprecated(serializableRoom.EncounterId).ToMutable();
    mutable.LoadCustomState(serializableRoom.EncounterState);
    CombatRoom combatRoom = new CombatRoom(mutable, runState)
    {
      GoldProportion = serializableRoom.GoldProportion,
      _isPreFinished = serializableRoom.IsPreFinished,
      ShouldResumeParentEventAfterCombat = serializableRoom.ShouldResumeParentEvent,
      ParentEventId = serializableRoom.ParentEventId
    };
    foreach (KeyValuePair<ulong, List<SerializableReward>> extraReward in serializableRoom.ExtraRewards)
    {
      ulong num;
      List<SerializableReward> serializableRewardList;
      extraReward.Deconstruct(ref num, ref serializableRewardList);
      ulong netId = num;
      List<SerializableReward> source = serializableRewardList;
      Player player = runState.GetPlayer(netId);
      List<Reward> list = source.Select<SerializableReward, Reward>((Func<SerializableReward, Reward>) (sr => Reward.FromSerializable(sr, player))).ToList<Reward>();
      combatRoom._extraRewards.Add(player, list);
    }
    if (serializableRoom.IsPreFinished)
      combatRoom.MarkPreFinished();
    return combatRoom;
  }

  public override async Task EnterInternal(IRunState? runState, bool isRestoringRoomStackBase)
  {
    if (isRestoringRoomStackBase)
      throw new InvalidOperationException("CombatRoom does not support room stack reconstruction.");
    if (this.CombatState.Players.Count == 0)
    {
      IRunState runState1 = runState;
      foreach (Player player in (IEnumerable<Player>) ((runState1 != null ? (object) runState1.Players : (object) null) ?? (object) Array.Empty<Player>()))
        this.CombatState.AddPlayer(player);
    }
    if (this.IsPreFinished)
      await this.StartPreFinishedCombat();
    else
      await this.StartCombat(runState);
  }

  public override Task Exit(IRunState? runState)
  {
    CombatManager.Instance.Reset(true);
    if (this.IsPreFinished)
    {
      foreach (Creature creature in this.CombatState.PlayerCreatures.ToList<Creature>())
        this.CombatState.RemoveCreature(creature, true);
    }
    return Task.CompletedTask;
  }

  public override Task Resume(AbstractRoom _, IRunState? runState)
  {
    throw new NotImplementedException();
  }

  public override SerializableRoom ToSerializable()
  {
    if (this.ParentEventId != (ModelId) null && !this.IsPreFinished)
      throw new InvalidOperationException("Cannot serialize a CombatRoom with a ParentEventId that is not pre-finished.");
    SerializableRoom serializable = base.ToSerializable();
    serializable.EncounterId = this.Encounter.Id;
    serializable.IsPreFinished = this.IsPreFinished;
    serializable.GoldProportion = this.GoldProportion;
    serializable.ParentEventId = this.ParentEventId;
    serializable.ShouldResumeParentEvent = this.ShouldResumeParentEventAfterCombat;
    serializable.EncounterState = this.Encounter.SaveCustomState();
    foreach (KeyValuePair<Player, List<Reward>> extraReward in (IEnumerable<KeyValuePair<Player, List<Reward>>>) this.ExtraRewards)
    {
      Player player1;
      List<Reward> rewardList;
      extraReward.Deconstruct(ref player1, ref rewardList);
      Player player2 = player1;
      List<Reward> source = rewardList;
      serializable.ExtraRewards[player2.NetId] = source.Select<Reward, SerializableReward>((Func<Reward, SerializableReward>) (r => r.ToSerializable())).ToList<SerializableReward>();
    }
    return serializable;
  }

  public void MarkPreFinished() => this._isPreFinished = true;

  public void AddExtraReward(Player player, Reward reward)
  {
    if (!this.ExtraRewards.ContainsKey(player))
      this._extraRewards.Add(player, new List<Reward>());
    this.ExtraRewards[player].Add(reward);
  }

  private async Task StartCombat(IRunState? runState)
  {
    if (!this.Encounter.HaveMonstersBeenGenerated)
      this.Encounter.GenerateMonstersWithSlots(this.CombatState.RunState);
    if (this.ShouldCreateCombat)
      await PreloadManager.LoadRoomCombatAssets(this.Encounter, runState ?? (IRunState) NullRunState.Instance);
    foreach ((MonsterModel monster, string slot) in (IEnumerable<(MonsterModel, string)>) this.Encounter.MonstersWithSlots)
    {
      monster.AssertMutable();
      if (this.ShouldCreateCombat)
        this.CombatState.AddCreature(this.CombatState.CreateCreature(monster, CombatSide.Enemy, slot));
      MapPointHistoryEntry pointHistoryEntry = this.CombatState.RunState.CurrentMapPointHistoryEntry;
      if (pointHistoryEntry != null)
        pointHistoryEntry.Rooms.Last<MapPointRoomHistoryEntry>().MonsterIds.Add(monster.Id);
    }
    if (this.ShouldCreateCombat)
      NRun.Instance?.SetCurrentRoom((Control) NCombatRoom.Create((ICombatRoomVisuals) this, CombatRoomMode.ActiveCombat));
    else
      NCombatRoom.Instance?.TransitionToActiveCombat(this);
    CombatManager.Instance.SetUpCombat(this.CombatState);
    if (runState != null)
      await Hook.AfterRoomEntered(runState, (AbstractRoom) this);
    CombatManager.Instance.AfterCombatRoomLoaded();
  }

  public void OnCombatEnded()
  {
    this.GoldProportion = this.Encounter.CalculateGoldProportion(this.CombatState);
  }

  private async Task StartPreFinishedCombat()
  {
    this.Encounter.GenerateMonstersWithSlots(this.CombatState.RunState);
    await PreloadManager.LoadRoomCombatAssets(this.Encounter, this.CombatState.RunState);
    NCombatRoom node = NCombatRoom.Create((ICombatRoomVisuals) this, CombatRoomMode.FinishedCombat);
    NRun.Instance?.SetCurrentRoom((Control) node);
    node?.SetUpBackground(this.CombatState.RunState);
    NMapScreen.Instance?.SetTravelEnabled(true);
    foreach (Player player in (IEnumerable<Player>) this.CombatState.RunState.Players)
      player.ResetCombatState();
    RunManager.Instance.ActionExecutor.Unpause();
    if (this.Encounter.ShouldGiveRewards)
    {
      await this.OfferRoomEndRewards();
    }
    else
    {
      if (node == null)
        return;
      await node.Ui.ProceedWithoutRewards();
    }
  }

  public async Task OfferRoomEndRewards()
  {
    List<RewardsSet> rewards = new List<RewardsSet>();
    foreach (Player player in (IEnumerable<Player>) this.CombatState.Players)
    {
      List<RewardsSet> rewardsSetList = rewards;
      rewardsSetList.Add(await RewardsCmd.GenerateForRoomEnd(player, (AbstractRoom) this));
      rewardsSetList = (List<RewardsSet>) null;
    }
    foreach (RewardsSet reward in rewards)
    {
      await Hook.BeforeCombatRewardOffered(reward, this.CombatState.RunState, this);
      TaskHelper.RunSafely(reward.Offer());
    }
    rewards = (List<RewardsSet>) null;
  }
}
