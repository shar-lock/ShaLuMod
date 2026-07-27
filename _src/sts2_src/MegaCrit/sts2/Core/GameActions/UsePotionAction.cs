// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.GameActions.UsePotionAction
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.GameActions;

public class UsePotionAction : GameAction
{
  public override ulong OwnerId => this.Player.NetId;

  public override GameActionType ActionType
  {
    get
    {
      return !this.WasEnqueuedInCombat ? GameActionType.NonCombat : GameActionType.CombatPlayPhaseOnly;
    }
  }

  public Player Player { get; }

  public uint PotionIndex { get; }

  public uint? TargetId { get; }

  public bool WasEnqueuedInCombat { get; }

  private ulong? TargetPlayerId { get; }

  public PlayerChoiceContext? PlayerChoiceContext { get; private set; }

  public UsePotionAction(PotionModel potion, Creature? target, bool isCombatInProgress)
  {
    int num = potion.Owner != null ? potion.Owner.GetPotionSlotIndex(potion) : throw new InvalidOperationException($"Cannot enqueue UsePotionAction for potion {potion} without an owner!");
    if (num < 0)
      throw new InvalidOperationException($"Potion {potion} has owner {this.Player}, but the owner's potion list does not contain it!");
    this.Player = potion.Owner;
    this.PotionIndex = (uint) num;
    this.WasEnqueuedInCombat = isCombatInProgress;
    if (target == null)
      return;
    if (!target.CombatId.HasValue)
    {
      if (CombatManager.Instance.IsInProgress)
        throw new InvalidOperationException($"Trying to target potion {potion} at target {target} that has no combat ID assigned during combat!");
      if (!target.IsPlayer)
        throw new InvalidOperationException($"Trying to target potion {potion} at target {target} outside of combat that is not a player!");
    }
    this.TargetId = target.CombatId;
    this.TargetPlayerId = target.Player?.NetId;
  }

  public UsePotionAction(
    Player player,
    uint potionIndex,
    uint? targetId,
    ulong? targetPlayerId,
    bool isCombatInProgress)
  {
    this.Player = player;
    this.PotionIndex = potionIndex;
    this.TargetId = targetId;
    this.TargetPlayerId = targetPlayerId;
    this.WasEnqueuedInCombat = isCombatInProgress;
  }

  protected override async Task ExecuteAction()
  {
    PotionModel potion = this.Player.GetPotionAtSlotIndex((int) this.PotionIndex);
    if (potion == null)
    {
      Log.Warn($"{nameof (UsePotionAction)}: potion at index {this.PotionIndex} is null for player {this.Player.NetId}, canceling");
      this.Cancel();
      potion = (PotionModel) null;
    }
    else
    {
      Creature target = (Creature) null;
      if (CombatManager.Instance.IsInProgress)
      {
        if (!this.TargetId.HasValue && potion.TargetType.IsSingleTarget())
          target = this.Player.Creature;
        else
          target = await this.Player.Creature.CombatState.GetCreatureAsync(this.TargetId, 10.0);
      }
      else if (!this.TargetPlayerId.HasValue && potion.TargetType != TargetType.TargetedNoCreature)
        target = this.Player.Creature;
      else if (this.TargetPlayerId.HasValue)
        target = this.Player.RunState.GetPlayer(this.TargetPlayerId.Value).Creature;
      if (!potion.IsValidTarget(target))
      {
        this.Cancel();
        potion = (PotionModel) null;
      }
      else
      {
        string str1;
        if (target == null)
        {
          str1 = "no target";
        }
        else
        {
          DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 2);
          interpolatedStringHandler.AppendLiteral("targeting ");
          interpolatedStringHandler.AppendFormatted(target.LogName);
          interpolatedStringHandler.AppendLiteral(" (index ");
          ref DefaultInterpolatedStringHandler local = ref interpolatedStringHandler;
          ICombatState combatState = this.Player.Creature.CombatState;
          int? nullable = combatState != null ? new int?(combatState.Creatures.IndexOf<Creature>(target)) : new int?();
          local.AppendFormatted<int?>(nullable);
          interpolatedStringHandler.AppendLiteral(")");
          str1 = interpolatedStringHandler.ToStringAndClear();
        }
        string str2 = str1;
        Log.Info($"Player {potion.Owner.NetId} using potion {potion.Id.Entry} ({str2})");
        this.PlayerChoiceContext = (PlayerChoiceContext) new GameActionPlayerChoiceContext((GameAction) this);
        await potion.OnUseWrapper(this.PlayerChoiceContext, target);
        potion = (PotionModel) null;
      }
    }
  }

  protected override void CancelAction()
  {
    PotionModel potionAtSlotIndex = this.Player.GetPotionAtSlotIndex((int) this.PotionIndex);
    if (TestMode.IsOff && NRun.Instance != null && LocalContext.IsMe(this.Player) && potionAtSlotIndex != null)
      NRun.Instance.GlobalUi.TopBar.PotionContainer.OnPotionUseOrDiscardCanceled(potionAtSlotIndex);
    potionAtSlotIndex?.AfterUsageCanceled();
  }

  public override INetAction ToNetAction()
  {
    return (INetAction) new NetUsePotionAction()
    {
      potionIndex = this.PotionIndex,
      targetId = this.TargetId,
      targetPlayerId = this.TargetPlayerId,
      enqueuedInCombat = this.WasEnqueuedInCombat
    };
  }

  public override string ToString()
  {
    return $"{nameof (UsePotionAction)} {this.Player.NetId} {this.Player.GetPotionAtSlotIndex((int) this.PotionIndex)} index: {this.PotionIndex} target: {this.TargetId} ({this.Player.Creature.CombatState?.GetCreature(this.TargetId)}) combat: {this.WasEnqueuedInCombat}";
  }
}
