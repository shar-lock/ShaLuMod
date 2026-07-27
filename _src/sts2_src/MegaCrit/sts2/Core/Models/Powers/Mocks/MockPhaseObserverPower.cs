// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.Mocks.MockPhaseObserverPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers.Mocks;

public sealed class MockPhaseObserverPower : PowerModel
{
  public override bool IsMock => true;

  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public static List<(string Hook, PlayerTurnPhase Phase)> Observations { get; } = new List<(string, PlayerTurnPhase)>();

  public static Action<string, Player>? OnRecordCallback { get; set; }

  public static void ResetObservations()
  {
    MockPhaseObserverPower.Observations.Clear();
    MockPhaseObserverPower.OnRecordCallback = (Action<string, Player>) null;
  }

  public override Task BeforeHandDraw(
    Player player,
    PlayerChoiceContext choiceContext,
    ICombatState combatState)
  {
    this.Record(nameof (BeforeHandDraw), player);
    return Task.CompletedTask;
  }

  public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
  {
    this.Record(nameof (AfterPlayerTurnStart), player);
    return Task.CompletedTask;
  }

  public override Task AfterAutoPrePlayPhaseEntered(
    PlayerChoiceContext choiceContext,
    Player player)
  {
    this.Record(nameof (AfterAutoPrePlayPhaseEntered), player);
    return Task.CompletedTask;
  }

  public override Task AfterAutoPostPlayPhaseEntered(
    PlayerChoiceContext choiceContext,
    Player player)
  {
    this.Record(nameof (AfterAutoPostPlayPhaseEntered), player);
    return Task.CompletedTask;
  }

  public override Task BeforeSideTurnEnd(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    if (!participants.Contains<Creature>(this.Owner))
      return Task.CompletedTask;
    this.Record(nameof (BeforeSideTurnEnd), this.Owner.Player);
    return Task.CompletedTask;
  }

  public override Task BeforeFlush(PlayerChoiceContext choiceContext, Player player)
  {
    this.Record(nameof (BeforeFlush), player);
    return Task.CompletedTask;
  }

  public override Task AfterSideTurnEnd(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    if (!participants.Contains<Creature>(this.Owner))
      return Task.CompletedTask;
    this.Record(nameof (AfterSideTurnEnd), this.Owner.Player);
    return Task.CompletedTask;
  }

  private void Record(string hook, Player player)
  {
    MockPhaseObserverPower.Observations.Add((hook, player.PlayerCombatState.Phase));
    Action<string, Player> onRecordCallback = MockPhaseObserverPower.OnRecordCallback;
    if (onRecordCallback == null)
      return;
    onRecordCallback(hook, player);
  }
}
