// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.PunchOff
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class PunchOff : EventModel
{
  private CancellationTokenSource? _punchCts;

  public override EventLayoutType LayoutType => EventLayoutType.Combat;

  public override EncounterModel CanonicalEncounter
  {
    get => (EncounterModel) ModelDb.Encounter<PunchOffEventEncounter>();
  }

  public override bool IsShared => true;

  public override bool IsAllowed(IRunState runState) => runState.TotalFloor >= 6;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new GoldVar(0));
    }
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      new EventOption((EventModel) this, new Func<Task>(this.Nab), "PUNCH_OFF.pages.INITIAL.options.NAB", HoverTipFactory.FromCardWithCardHoverTips<Injury>()),
      new EventOption((EventModel) this, new Func<Task>(this.TakeThem), "PUNCH_OFF.pages.INITIAL.options.I_CAN_TAKE_THEM", Array.Empty<IHoverTip>())
    });
  }

  public override Task AfterEventStarted()
  {
    RunManager.Instance.RoomExited += new Action(this.OnRoomExited);
    this._punchCts = new CancellationTokenSource();
    TaskHelper.RunSafely(this.PunchEachOther());
    return Task.CompletedTask;
  }

  private async Task PunchEachOther()
  {
    Creature leftEnemy = this._combatSynchronizer.CombatStateForLayout.Enemies[0];
    Creature rightEnemy = this._combatSynchronizer.CombatStateForLayout.Enemies[1];
    NCreature leftEnemyNode = NCombatRoom.Instance?.GetCreatureNode(leftEnemy);
    Control vfxContainer;
    if (leftEnemyNode == null)
    {
      leftEnemy = (Creature) null;
      rightEnemy = (Creature) null;
      leftEnemyNode = (NCreature) null;
      vfxContainer = (Control) null;
    }
    else
    {
      Vector2 originalScale = leftEnemyNode.Scale;
      leftEnemyNode.Scale = new Vector2(-originalScale.X, originalScale.Y);
      vfxContainer = NCombatRoom.Instance?.CombatVfxContainer;
      while (!this._punchCts.IsCancellationRequested)
      {
        await CreatureCmd.TriggerAnim(leftEnemy, "Attack", 0.0f);
        await Cmd.Wait(0.1f);
        VfxCmd.PlayOnCreatureCenter(rightEnemy, "vfx/vfx_attack_blunt");
        Control parent1 = vfxContainer;
        if (parent1 != null)
          ((Godot.Node) parent1).AddChildSafely((Godot.Node) NHitSparkVfx.Create(rightEnemy, false));
        await CreatureCmd.TriggerAnim(rightEnemy, "Hit", 0.0f);
        await Cmd.Wait(1.2f);
        if (!this._punchCts.IsCancellationRequested)
        {
          await CreatureCmd.TriggerAnim(rightEnemy, "Attack", 0.0f);
          await Cmd.Wait(0.1f);
          VfxCmd.PlayOnCreatureCenter(leftEnemy, "vfx/vfx_attack_blunt");
          Control parent2 = vfxContainer;
          if (parent2 != null)
            ((Godot.Node) parent2).AddChildSafely((Godot.Node) NHitSparkVfx.Create(leftEnemy, false));
          await CreatureCmd.TriggerAnim(leftEnemy, "Hit", 0.0f);
          await Cmd.Wait(1.2f);
        }
        else
          break;
      }
      this._punchCts = (CancellationTokenSource) null;
      if (!((Godot.Node) leftEnemyNode).IsValid())
      {
        leftEnemy = (Creature) null;
        rightEnemy = (Creature) null;
        leftEnemyNode = (NCreature) null;
        vfxContainer = (Control) null;
      }
      else
      {
        leftEnemyNode.Scale = originalScale;
        leftEnemy = (Creature) null;
        rightEnemy = (Creature) null;
        leftEnemyNode = (NCreature) null;
        vfxContainer = (Control) null;
      }
    }
  }

  public override void CalculateVars()
  {
    this.DynamicVars.Gold.BaseValue = (Decimal) this.Rng.NextInt(91, 99);
  }

  private async Task Nab()
  {
    CardModel deck = await CardPileCmd.AddCurseToDeck<Injury>(this.Owner);
    NGame.Instance.ScreenShakeTrauma(ShakeStrength.Strong);
    NDebugAudioManager.Instance?.Play("blunt_attack.mp3");
    await Cmd.CustomScaledWait(0.25f, 0.5f);
    await RewardsCmd.OfferCustom(this.Owner, new List<Reward>(1)
    {
      (Reward) new RelicReward(this.Owner)
    });
    this.SetEventFinished(this.L10NLookup("PUNCH_OFF.pages.NAB.description"));
  }

  private Task TakeThem()
  {
    this._punchCts?.Cancel();
    // ISSUE: object of a compiler-generated type is created
    this.SetEventState(this.L10NLookup("PUNCH_OFF.pages.I_CAN_TAKE_THEM.description"), (IEnumerable<EventOption>) new \u003C\u003Ez__ReadOnlySingleElementList<EventOption>(new EventOption((EventModel) this, new Func<Task>(this.Fight), "PUNCH_OFF.pages.I_CAN_TAKE_THEM.options.FIGHT", Array.Empty<IHoverTip>())));
    return Task.CompletedTask;
  }

  private Task Fight()
  {
    int capacity = 2;
    List<Reward> extraRewards = new List<Reward>(capacity);
    CollectionsMarshal.SetCount<Reward>(extraRewards, capacity);
    Span<Reward> span = CollectionsMarshal.AsSpan<Reward>(extraRewards);
    int num1 = 0;
    span[num1] = (Reward) new RelicReward(this.Owner);
    int num2 = num1 + 1;
    span[num2] = (Reward) new PotionReward(this.Owner);
    this.EnterCombatWithoutExitingEvent<PunchOffEventEncounter>((IReadOnlyList<Reward>) extraRewards, false);
    return Task.CompletedTask;
  }

  private void OnRoomExited()
  {
    RunManager.Instance.RoomExited -= new Action(this.OnRoomExited);
    this._punchCts?.Cancel();
  }
}
