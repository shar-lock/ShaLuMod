// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.AutoSlay.Handlers.Rooms.EventRoomHandler
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.AutoSlay.Helpers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Events;
using MegaCrit.Sts2.Core.Nodes.Events.Custom;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.AutoSlay.Handlers.Rooms;

public class EventRoomHandler : IRoomHandler, IHandler
{
  private const string _roomPath = "/root/Game/RootSceneContainer/Run/RoomContainer/EventRoom";
  private const int _maxIterations = 50;

  public RoomType[] HandledTypes
  {
    get => new RoomType[1]{ RoomType.Event };
  }

  public TimeSpan Timeout => TimeSpan.FromMinutes(3L);

  public async Task HandleAsync(Rng random, CancellationToken ct)
  {
    AutoSlayLog.Action("Waiting for event room");
    Node eventRoom = await this.WaitForEventRoom(ct);
    if (await this.WaitForEventOptions(eventRoom, ct))
    {
      AutoSlayLog.Action("Event room completed");
    }
    else
    {
      int iterations = 0;
      while (iterations < 50)
      {
        ct.ThrowIfCancellationRequested();
        if (!GodotObject.IsInstanceValid((GodotObject) eventRoom) || !eventRoom.IsInsideTree())
        {
          RunState state = RunManager.Instance.DebugOnlyGetState();
          if (state != null && state.CurrentRoomCount > 1)
          {
            AbstractRoom baseRoom = state.BaseRoom;
            if ((baseRoom != null ? (baseRoom.RoomType == RoomType.Event ? 1 : 0) : 0) != 0)
            {
              AutoSlayLog.Action("Event triggered combat, handling combat first");
              await this.HandleEventCombat(ct);
              AutoSlayLog.Action("Combat finished, checking if event resumes");
              Node nodeOrNull = ((Node) ((SceneTree) Engine.GetMainLoop()).Root).GetNodeOrNull(NodePath.op_Implicit("/root/Game/RootSceneContainer/Run/RoomContainer/EventRoom"));
              if (nodeOrNull == null)
              {
                AutoSlayLog.Action("Event ended after combat (no event room)");
                break;
              }
              eventRoom = nodeOrNull;
              await Task.Delay(500, ct);
              if (UiHelper.FindAll<NEventOptionButton>(eventRoom).Where<NEventOptionButton>((Func<NEventOptionButton, bool>) (o => !o.Option.IsLocked)).ToList<NEventOptionButton>().Count == 0)
              {
                AutoSlayLog.Action("Event finished after combat");
                break;
              }
              ++iterations;
              continue;
            }
          }
          AutoSlayLog.Action("Event room no longer valid, exiting");
          break;
        }
        List<NEventOptionButton> neventOptionButtonList = UiHelper.FindAll<NEventOptionButton>(eventRoom).Where<NEventOptionButton>((Func<NEventOptionButton, bool>) (o => !o.Option.IsLocked)).ToList<NEventOptionButton>();
        if (neventOptionButtonList.Count != 0)
        {
          List<NEventOptionButton> list1 = neventOptionButtonList.Where<NEventOptionButton>((Func<NEventOptionButton, bool>) (o => o.Option.WillKillPlayer == null || o.Event.Owner == null || !o.Option.WillKillPlayer(o.Event.Owner))).ToList<NEventOptionButton>();
          if (list1.Count > 0)
            neventOptionButtonList = list1;
          NEventOptionButton choice = random.NextItem<NEventOptionButton>((IEnumerable<NEventOptionButton>) neventOptionButtonList);
          AutoSlayLog.Action($"Selecting event option: {choice.Event.Id.Entry} (option: {choice.Option.Title.GetFormattedText()})");
          HashSet<NEventOptionButton> previousButtons = new HashSet<NEventOptionButton>(UiHelper.FindAll<NEventOptionButton>(eventRoom).Where<NEventOptionButton>((Func<NEventOptionButton, bool>) (o => !o.Option.IsLocked)));
          await UiHelper.Click((NClickableControl) choice);
          if (choice.Option.IsProceed)
          {
            AutoSlayLog.Action("Clicked proceed, exiting event");
            await WaitHelper.Until((Func<bool>) (() =>
            {
              if (!GodotObject.IsInstanceValid((GodotObject) eventRoom) || !eventRoom.IsInsideTree())
                return true;
              NMapScreen instance = NMapScreen.Instance;
              return instance != null && instance.IsOpen;
            }), ct, new TimeSpan?(TimeSpan.FromSeconds(5L)), "Event room did not close after clicking proceed");
            break;
          }
          await WaitHelper.Until((Func<bool>) (() =>
          {
            NOverlayStack instance1 = NOverlayStack.Instance;
            if ((instance1 != null ? (instance1.ScreenCount > 0 ? 1 : 0) : 0) != 0)
              return true;
            NMapScreen instance2 = NMapScreen.Instance;
            if ((instance2 != null ? (instance2.IsOpen ? 1 : 0) : 0) != 0 || !GodotObject.IsInstanceValid((GodotObject) eventRoom) || !eventRoom.IsInsideTree() || CombatManager.Instance.IsInProgress)
              return true;
            List<NEventOptionButton> list2 = UiHelper.FindAll<NEventOptionButton>(eventRoom).Where<NEventOptionButton>((Func<NEventOptionButton, bool>) (o => !o.Option.IsLocked)).ToList<NEventOptionButton>();
            return list2.Count == 0 || !previousButtons.SetEquals((IEnumerable<NEventOptionButton>) list2);
          }), ct, new TimeSpan?(TimeSpan.FromSeconds(5L)), "Event options did not change after choice");
          NOverlayStack instance3 = NOverlayStack.Instance;
          if ((instance3 != null ? (instance3.ScreenCount > 0 ? 1 : 0) : 0) != 0)
          {
            AutoSlayLog.Action("Overlay screen opened during event, deferring to drain loop");
            break;
          }
          if (CombatManager.Instance.IsInProgress)
          {
            AutoSlayLog.Action("Combat started during event (combat layout), handling combat");
            await this.HandleEventCombat(ct);
            if (!GodotObject.IsInstanceValid((GodotObject) eventRoom) || !eventRoom.IsInsideTree())
            {
              AutoSlayLog.Action("Event room gone after combat");
              break;
            }
            await WaitHelper.Until((Func<bool>) (() =>
            {
              if (GodotObject.IsInstanceValid((GodotObject) eventRoom) && eventRoom.IsInsideTree())
              {
                NMapScreen instance4 = NMapScreen.Instance;
                if ((instance4 != null ? (instance4.IsOpen ? 1 : 0) : 0) == 0)
                {
                  NOverlayStack instance5 = NOverlayStack.Instance;
                  if ((instance5 != null ? (instance5.ScreenCount > 0 ? 1 : 0) : 0) == 0)
                    return UiHelper.FindAll<NEventOptionButton>(eventRoom).Any<NEventOptionButton>((Func<NEventOptionButton, bool>) (o => !o.Option.IsLocked));
                }
              }
              return true;
            }), ct, new TimeSpan?(TimeSpan.FromSeconds(10L)), "Event did not resume after combat layout combat");
            if (!GodotObject.IsInstanceValid((GodotObject) eventRoom) || !eventRoom.IsInsideTree())
            {
              AutoSlayLog.Action("Event room gone after combat");
              break;
            }
          }
          ++iterations;
          choice = (NEventOptionButton) null;
        }
        else
          break;
      }
      if (iterations >= 50)
        AutoSlayLog.Warn($"Event room hit iteration limit ({50})");
      AutoSlayLog.Action("Event room completed");
    }
  }

  private async Task HandleEventCombat(CancellationToken ct)
  {
    await WaitHelper.Until((Func<bool>) (() => CombatManager.Instance.IsInProgress), ct, new TimeSpan?(AutoSlayConfig.nodeWaitTimeout), "Event combat not started");
    AutoSlayLog.Action("Event combat started, applying buffs and killing enemies");
    Creature player = LocalContext.GetMe((IPlayerCollection) RunManager.Instance.DebugOnlyGetState()).Creature;
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), player, 100M, player, (CardModel) null);
    PlatingPower platingPower = await PowerCmd.Apply<PlatingPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), player, 100M, player, (CardModel) null);
    RegenPower regenPower = await PowerCmd.Apply<RegenPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), player, 100M, player, (CardModel) null);
    int killAttempts = 0;
    int noEnemyWaitLoops = 0;
    while (CombatManager.Instance.IsInProgress && killAttempts < 20)
    {
      ct.ThrowIfCancellationRequested();
      CombatState combatState = CombatManager.Instance.DebugOnlyGetState();
      if (combatState == null)
      {
        AutoSlayLog.Info("Combat state became null, exiting kill loop");
        break;
      }
      foreach (Creature creature in (IEnumerable<Creature>) combatState.Enemies)
      {
        foreach (PowerModel power in creature.Powers.Where<PowerModel>((Func<PowerModel, bool>) (p => p.ShouldStopCombatFromEnding())).ToList<PowerModel>())
        {
          AutoSlayLog.Info($"Removing blocking power {power.GetType().Name} from {creature}");
          await PowerCmd.Remove(power);
        }
      }
      List<Creature> list = combatState.Enemies.Where<Creature>((Func<Creature, bool>) (e => e.IsAlive)).ToList<Creature>();
      if (list.Count > 0)
      {
        AutoSlayLog.Action($"Killing {list.Count} event combat enemies (attempt {killAttempts + 1})");
        await CreatureCmd.Kill((IReadOnlyCollection<Creature>) list);
        ++killAttempts;
        noEnemyWaitLoops = 0;
        await Task.Delay(1000, ct);
        int num = await CombatManager.Instance.CheckWinCondition() ? 1 : 0;
      }
      else
      {
        ++noEnemyWaitLoops;
        if (noEnemyWaitLoops > 100)
        {
          AutoSlayLog.Info("Event combat still in progress but no enemies for 10s, breaking loop");
          break;
        }
        await Task.Delay(100, ct);
      }
      combatState = (CombatState) null;
    }
    if (CombatManager.Instance.IsInProgress)
    {
      int num1 = await CombatManager.Instance.CheckWinCondition() ? 1 : 0;
    }
    await WaitHelper.Until((Func<bool>) (() => !CombatManager.Instance.IsInProgress), ct, new TimeSpan?(TimeSpan.FromSeconds(30L)), "Event combat did not end");
    AutoSlayLog.Action("Event combat finished");
    player = (Creature) null;
  }

  private async Task<Node> WaitForEventRoom(CancellationToken ct)
  {
    return await WaitHelper.ForNode<Node>((Node) ((SceneTree) Engine.GetMainLoop()).Root, "/root/Game/RootSceneContainer/Run/RoomContainer/EventRoom", ct);
  }

  private async Task<bool> WaitForEventOptions(Node eventRoom, CancellationToken ct)
  {
    NAncientEventLayout first1 = UiHelper.FindFirst<NAncientEventLayout>(eventRoom);
    if (first1 != null)
    {
      await this.HandleAncientEventDialogue(first1, ct);
      return false;
    }
    NFakeMerchant first2 = UiHelper.FindFirst<NFakeMerchant>(eventRoom);
    if (first2 != null)
    {
      AutoSlayLog.Info("Detected custom event: FakeMerchant");
      await this.HandleFakeMerchantEvent(first2, ct);
      return true;
    }
    int waitCycles = 0;
    await WaitHelper.Until((Func<bool>) (() =>
    {
      ++waitCycles;
      if (waitCycles % 50 == 0)
      {
        AutoSlayLog.Info($"Waiting for event options: {eventRoom.GetChildCount(false)} children in event room");
        AutoSlayer.CurrentWatchdog?.Reset("Waiting for event options to load");
      }
      return UiHelper.FindAll<NEventOptionButton>(eventRoom).Count > 0;
    }), ct, new TimeSpan?(TimeSpan.FromSeconds(30L)), "Event options not loaded");
    return false;
  }

  private async Task HandleFakeMerchantEvent(NFakeMerchant fakeMerchant, CancellationToken ct)
  {
    AutoSlayLog.Action("Handling FakeMerchant event");
    NProceedButton proceedButton = (NProceedButton) null;
    await WaitHelper.Until((Func<bool>) (() =>
    {
      proceedButton = UiHelper.FindFirst<NProceedButton>((Node) fakeMerchant);
      return proceedButton != null && proceedButton.IsEnabled && ((CanvasItem) proceedButton).Visible;
    }), ct, new TimeSpan?(TimeSpan.FromSeconds(10L)), "FakeMerchant proceed button not available");
    AutoSlayLog.Action("Clicking FakeMerchant proceed button");
    await UiHelper.Click((NClickableControl) proceedButton);
  }

  private async Task HandleAncientEventDialogue(
    NAncientEventLayout ancientLayout,
    CancellationToken ct)
  {
    AutoSlayLog.Info("Detected Ancient event, clicking through dialogue");
    int clicks = 0;
    while (clicks < 50)
    {
      ct.ThrowIfCancellationRequested();
      if (GodotObject.IsInstanceValid((GodotObject) ancientLayout))
      {
        List<NEventOptionButton> list = UiHelper.FindAll<NEventOptionButton>((Node) ancientLayout).Where<NEventOptionButton>((Func<NEventOptionButton, bool>) (b => b.IsEnabled && !b.Option.IsLocked)).ToList<NEventOptionButton>();
        if (list.Count > 0)
        {
          AutoSlayLog.Info($"Ancient dialogue finished, {list.Count} options available");
          break;
        }
        NButton nodeOrNull = ((Node) ancientLayout).GetNodeOrNull<NButton>(NodePath.op_Implicit("%DialogueHitbox"));
        if (nodeOrNull != null && ((CanvasItem) nodeOrNull).Visible && nodeOrNull.IsEnabled)
        {
          AutoSlayLog.Info($"Clicking Ancient dialogue (click {clicks + 1})");
          AutoSlayer.CurrentWatchdog?.Reset("Clicking Ancient event dialogue");
          ((GodotObject) nodeOrNull).EmitSignal(NClickableControl.SignalName.Released, new Variant[1]
          {
            Variant.op_Implicit((GodotObject) nodeOrNull)
          });
          ++clicks;
          await Task.Delay(500, ct);
        }
        else
          await Task.Delay(100, ct);
      }
      else
        break;
    }
    await WaitHelper.Until((Func<bool>) (() => UiHelper.FindAll<NEventOptionButton>((Node) ancientLayout).Any<NEventOptionButton>((Func<NEventOptionButton, bool>) (b => b.IsEnabled && !b.Option.IsLocked))), ct, new TimeSpan?(TimeSpan.FromSeconds(10L)), "Ancient event options did not become available after dialogue");
  }
}
