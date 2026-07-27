// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.GameActions.DiscardPotionGameAction
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.GameActions;

public class DiscardPotionGameAction : GameAction
{
  private readonly Player _player;
  private readonly uint _potionSlotIndex;

  public override ulong OwnerId => this._player.NetId;

  public override GameActionType ActionType
  {
    get
    {
      return !this.WasEnqueuedInCombat ? GameActionType.NonCombat : GameActionType.CombatPlayPhaseOnly;
    }
  }

  public bool WasEnqueuedInCombat { get; }

  public DiscardPotionGameAction(Player player, uint potionSlotIndex, bool isCombatInProgress)
  {
    this._player = player;
    this._potionSlotIndex = potionSlotIndex;
    this.WasEnqueuedInCombat = isCombatInProgress;
  }

  protected override async Task ExecuteAction()
  {
    if ((long) this._potionSlotIndex >= (long) this._player.PotionSlots.Count)
      throw new IndexOutOfRangeException($"Tried to discard potion at slot index {this._potionSlotIndex}, but player {this._player.NetId} only has {this._player.PotionSlots.Count} potion slots!");
    PotionModel potionSlot = this._player.PotionSlots[(int) this._potionSlotIndex];
    if (potionSlot == null)
    {
      Log.Warn($"{nameof (DiscardPotionGameAction)}: potion at slot index {this._potionSlotIndex} is null for player {this._player.NetId}, canceling");
      this.Cancel();
    }
    else
    {
      Log.Info($"Player {potionSlot.Owner.NetId} discarding potion {potionSlot.Id.Entry}");
      await PotionCmd.Discard(potionSlot);
    }
  }

  protected override void CancelAction()
  {
    PotionModel potionAtSlotIndex = this._player.GetPotionAtSlotIndex((int) this._potionSlotIndex);
    if (TestMode.IsOff && NRun.Instance != null && LocalContext.IsMe(this._player) && potionAtSlotIndex != null)
      NRun.Instance.GlobalUi.TopBar.PotionContainer.OnPotionUseOrDiscardCanceled(potionAtSlotIndex);
    potionAtSlotIndex?.AfterUsageCanceled();
  }

  public override INetAction ToNetAction()
  {
    return (INetAction) new NetDiscardPotionGameAction()
    {
      potionSlotIndex = this._potionSlotIndex,
      wasEnqueuedInCombat = this.WasEnqueuedInCombat
    };
  }

  public override string ToString()
  {
    return $"{"NetDiscardPotionGameAction"} for player {this._player.NetId} potion slot: {this._potionSlotIndex} in combat: {this.WasEnqueuedInCombat}";
  }
}
