// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.TreasureRoomRelic.NTreasureRoomRelicCollection
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.TreasureRelicPicking;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.TreasureRoomRelic;

[ScriptPath("res://src/Core/Nodes/Screens/TreasureRoomRelic/NTreasureRoomRelicCollection.cs")]
public class NTreasureRoomRelicCollection : Control, IScreenContext
{
  private const ulong _noSelectionTimeMsec = 200;
  private Control _relicContainer;
  private Control _fightBackstop;
  private MegaLabel _fightLabel;
  private Tween? _emptyVfxTween;
  private NHandImageCollection _hands;
  private CancellationTokenSource _cts = new CancellationTokenSource();
  private readonly List<NTreasureRoomRelicHolder> _multiplayerHolders = new List<NTreasureRoomRelicHolder>();
  private List<NTreasureRoomRelicHolder> _holdersInUse = new List<NTreasureRoomRelicHolder>();
  private readonly TaskCompletionSource _relicPickingBeganTaskCompletionSource = new TaskCompletionSource();
  private readonly TaskCompletionSource _relicPickingCompleteTaskCompletionSource = new TaskCompletionSource();
  private ulong _openedTicks;
  private IRunState _runState;
  private bool _isEmptyChest;

  public NTreasureRoomRelicHolder SingleplayerRelicHolder { get; private set; }

  public override void _Ready()
  {
    this._fightBackstop = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%FightBackstop"));
    this._hands = ((Node) this).GetNode<NHandImageCollection>(NodePath.op_Implicit("%HandsContainer"));
    this._relicContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("Container"));
    this.SingleplayerRelicHolder = ((Node) this._relicContainer).GetNode<NTreasureRoomRelicHolder>(NodePath.op_Implicit("%SingleplayerRelicHolder"));
    foreach (NTreasureRoomRelicHolder ntreasureRoomRelicHolder in ((IEnumerable) ((Node) this._relicContainer).GetChildren(false)).OfType<NTreasureRoomRelicHolder>())
    {
      if (ntreasureRoomRelicHolder != this.SingleplayerRelicHolder)
        this._multiplayerHolders.Add(ntreasureRoomRelicHolder);
    }
    Control fightBackstop = this._fightBackstop;
    Color modulate = ((CanvasItem) this._fightBackstop).Modulate;
    modulate.A = 0.0f;
    Color color = modulate;
    ((CanvasItem) fightBackstop).Modulate = color;
    ((CanvasItem) this._fightBackstop).Visible = false;
    this._fightLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%FightLabel"));
    this._fightLabel.Text = new LocString("gameplay_ui", "TREASURE_FIGHT_TEXT").GetFormattedText();
    RunManager.Instance.TreasureRoomRelicSynchronizer.VotesChanged += new Action(this.RefreshVotes);
    RunManager.Instance.TreasureRoomRelicSynchronizer.RelicsAwarded += new Action<List<RelicPickingResult>>(this.OnRelicsAwarded);
  }

  public override void _EnterTree() => this._cts = new CancellationTokenSource();

  public override void _ExitTree()
  {
    this._cts.Cancel();
    RunManager.Instance.TreasureRoomRelicSynchronizer.VotesChanged -= new Action(this.RefreshVotes);
    RunManager.Instance.TreasureRoomRelicSynchronizer.RelicsAwarded -= new Action<List<RelicPickingResult>>(this.OnRelicsAwarded);
  }

  public void Initialize(IRunState runState)
  {
    this._runState = runState;
    this._hands.Initialize(runState);
  }

  public void InitializeRelics()
  {
    IReadOnlyList<RelicModel> currentRelics = RunManager.Instance.TreasureRoomRelicSynchronizer.CurrentRelics;
    if (currentRelics == null || currentRelics.Count == 0)
    {
      this._isEmptyChest = true;
      ((CanvasItem) this.SingleplayerRelicHolder).Visible = false;
      foreach (CanvasItem multiplayerHolder in this._multiplayerHolders)
        multiplayerHolder.Visible = false;
      this.SpawnEmptyChestVfx();
    }
    else if (currentRelics.Count == 1)
    {
      this.SingleplayerRelicHolder.Initialize(currentRelics[0], this._runState);
      ((CanvasItem) this.SingleplayerRelicHolder).Visible = true;
      this.SingleplayerRelicHolder.Index = 0;
      ((GodotObject) this.SingleplayerRelicHolder).Connect(NClickableControl.SignalName.Released, Callable.From<NTreasureRoomRelicHolder>((Action<NTreasureRoomRelicHolder>) (_ => this.PickRelic(this.SingleplayerRelicHolder))), 0U);
      int capacity = 1;
      List<NTreasureRoomRelicHolder> ntreasureRoomRelicHolderList = new List<NTreasureRoomRelicHolder>(capacity);
      CollectionsMarshal.SetCount<NTreasureRoomRelicHolder>(ntreasureRoomRelicHolderList, capacity);
      CollectionsMarshal.AsSpan<NTreasureRoomRelicHolder>(ntreasureRoomRelicHolderList)[0] = this.SingleplayerRelicHolder;
      this._holdersInUse = ntreasureRoomRelicHolderList;
      foreach (CanvasItem multiplayerHolder in this._multiplayerHolders)
        multiplayerHolder.Visible = false;
    }
    else
    {
      ((CanvasItem) this.SingleplayerRelicHolder).Visible = false;
      for (int index = 0; index < this._multiplayerHolders.Count; ++index)
      {
        NTreasureRoomRelicHolder holder = this._multiplayerHolders[index];
        if (index < currentRelics.Count)
        {
          ((CanvasItem) holder).Visible = true;
          holder.Relic.Model = currentRelics[index];
          holder.Initialize(currentRelics[index], this._runState);
        }
        else
          ((CanvasItem) holder).Visible = false;
        holder.Index = index;
        ((GodotObject) holder).Connect(NClickableControl.SignalName.Released, Callable.From<NTreasureRoomRelicHolder>((Action<NTreasureRoomRelicHolder>) (_ => this.PickRelic(holder))), 0U);
        this._holdersInUse.Add(holder);
        holder.VoteContainer.RefreshPlayerVotes();
      }
      for (int index = 0; index < this._holdersInUse.Count; ++index)
      {
        this._holdersInUse[index].SetFocusMode((Control.FocusModeEnum) 2L);
        this._holdersInUse[index].FocusNeighborTop = ((Node) this._holdersInUse[index]).GetPath();
        this._holdersInUse[index].FocusNeighborBottom = ((Node) this._holdersInUse[index]).GetPath();
        NTreasureRoomRelicHolder ntreasureRoomRelicHolder = this._holdersInUse[index];
        NodePath path;
        if (index <= 0)
        {
          List<NTreasureRoomRelicHolder> holdersInUse = this._holdersInUse;
          path = ((Node) holdersInUse[holdersInUse.Count - 1]).GetPath();
        }
        else
          path = ((Node) this._holdersInUse[index - 1]).GetPath();
        ntreasureRoomRelicHolder.FocusNeighborLeft = path;
        this._holdersInUse[index].FocusNeighborRight = index < this._holdersInUse.Count - 1 ? ((Node) this._holdersInUse[index + 1]).GetPath() : ((Node) this._holdersInUse[0]).GetPath();
      }
      if (currentRelics.Count != 2)
        return;
      this._multiplayerHolders[1].Position = this._multiplayerHolders[3].Position;
    }
  }

  private void SpawnEmptyChestVfx()
  {
    MegaLabel node = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%EmptyLabel"));
    node.Text = new LocString("gameplay_ui", "TREASURE_EMPTY").GetFormattedText();
    ((CanvasItem) node).Visible = true;
    this._emptyVfxTween = ((Node) this).CreateTween().SetParallel(true);
    this._emptyVfxTween.TweenProperty((GodotObject) node, NodePath.op_Implicit("self_modulate:a"), Variant.op_Implicit(1f), 0.25);
    this._emptyVfxTween.TweenProperty((GodotObject) node, NodePath.op_Implicit("position:y"), Variant.op_Implicit(((Control) node).Position.Y), 1.0).From(Variant.op_Implicit(((Control) node).Position.Y + 64f)).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 11L);
    this._emptyVfxTween.Chain().TweenInterval(1.0);
    this._emptyVfxTween.Chain();
    this._emptyVfxTween.TweenProperty((GodotObject) node, NodePath.op_Implicit("self_modulate:a"), Variant.op_Implicit(0.0f), 1.0);
    ((Node) this).GetNode<GpuParticles2D>(NodePath.op_Implicit("%SmokePuffVfx")).Emitting = true;
  }

  public void SetSelectionEnabled(bool isEnabled)
  {
    if (isEnabled)
    {
      this.SingleplayerRelicHolder.Enable();
      foreach (NClickableControl multiplayerHolder in this._multiplayerHolders)
        multiplayerHolder.Enable();
    }
    else
    {
      this.SingleplayerRelicHolder.Disable();
      foreach (NClickableControl multiplayerHolder in this._multiplayerHolders)
        multiplayerHolder.Disable();
    }
  }

  public Task RelicPickingBegan() => this._relicPickingBeganTaskCompletionSource.Task;

  public Task RelicPickingFinished() => this._relicPickingCompleteTaskCompletionSource.Task;

  public void AnimIn(Node chestVisual)
  {
    ((CanvasItem) this).Visible = true;
    ((CanvasItem) this).Modulate = Colors.Transparent;
    Tween tween1 = ((Node) this).CreateTween().SetParallel(true);
    tween1.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.4);
    tween1.TweenProperty((GodotObject) chestVisual, NodePath.op_Implicit("modulate"), Variant.op_Implicit(StsColors.halfTransparentWhite), 0.4);
    if (this._isEmptyChest)
    {
      Player me = LocalContext.GetMe((IPlayerCollection) this._runState);
      if (me != null)
        me.Relics.OfType<SilverCrucible>().FirstOrDefault<SilverCrucible>()?.Flash();
      tween1.TweenCallback(Callable.From((Action) (() => RunManager.Instance.TreasureRoomRelicSynchronizer.CompleteWithNoRelics()))).SetDelay(1.0);
    }
    else
    {
      foreach (NTreasureRoomRelicHolder ntreasureRoomRelicHolder1 in this._holdersInUse)
      {
        NTreasureRoomRelicHolder holder = ntreasureRoomRelicHolder1;
        holder.MouseFilter = (Control.MouseFilterEnum) 2L;
        float num1 = this._holdersInUse.Count == 1 ? 150f : 50f;
        float num2 = (float) (0.20000000298023224 + 0.20000000298023224 * (double) Rng.Chaotic.NextFloat());
        ((CanvasItem) holder).Modulate = Colors.Black;
        NTreasureRoomRelicHolder ntreasureRoomRelicHolder2 = holder;
        Vector2 position = holder.Position;
        position.Y = holder.Position.Y + num1;
        Vector2 vector2 = position;
        ntreasureRoomRelicHolder2.Position = vector2;
        Tween tween2 = ((Node) this).CreateTween().SetParallel(true);
        tween2.TweenProperty((GodotObject) holder, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.2).SetDelay((double) num2);
        tween2.TweenProperty((GodotObject) holder, NodePath.op_Implicit("position:y"), Variant.op_Implicit(holder.Position.Y - num1), 0.6).SetDelay((double) num2).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L);
        tween2.TweenCallback(Callable.From<Control.MouseFilterEnum>((Func<Control.MouseFilterEnum>) (() => holder.MouseFilter = (Control.MouseFilterEnum) 0L))).SetDelay((double) num2 + 0.6);
      }
      NRun.Instance.ScreenStateTracker.SetIsInSharedRelicPickingScreen(true);
      this._hands.AnimateHandsIn();
    }
  }

  public void AnimOut(Node chestVisual)
  {
    ((CanvasItem) this).Modulate = Colors.White;
    Tween tween = ((Node) this).CreateTween().Parallel();
    tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate"), Variant.op_Implicit(StsColors.transparentWhite), 0.3);
    tween.TweenProperty((GodotObject) chestVisual, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.3);
    tween.TweenCallback(Callable.From<bool>((Func<bool>) (() => ((CanvasItem) this).Visible = false)));
    NRun.Instance.ScreenStateTracker.SetIsInSharedRelicPickingScreen(false);
  }

  private void PickRelic(NTreasureRoomRelicHolder holder)
  {
    if (Time.GetTicksMsec() - this._openedTicks <= 200UL)
      return;
    RunManager.Instance.TreasureRoomRelicSynchronizer.PickRelicLocally(new int?(holder.Index));
  }

  private void OnRelicsAwarded(List<RelicPickingResult> results)
  {
    TaskHelper.RunSafely(this.AnimateRelicAwards(results));
  }

  private async Task AnimateRelicAwards(List<RelicPickingResult> results)
  {
    foreach (Control control in this._holdersInUse)
      control.SetFocusMode((Control.FocusModeEnum) 0L);
    this._relicPickingBeganTaskCompletionSource.SetResult();
    foreach (Player player in (IEnumerable<Player>) this._runState.Players)
    {
      TreasureRoomRelicSynchronizer.PlayerVote playerVote = RunManager.Instance.TreasureRoomRelicSynchronizer.GetPlayerVote(player);
      if (playerVote.voteReceived && !playerVote.index.HasValue)
        this._hands.GetHand(player.NetId)?.SetSkipped();
    }
    this._hands.BeforeRelicsAwarded();
    List<Task> tasksToWait = new List<Task>();
    RelicPickingResultType? nullable1 = new RelicPickingResultType?();
    results.Sort((Comparison<RelicPickingResult>) ((r1, r2) => r1.type.CompareTo((object) r2.type)));
    foreach (RelicPickingResult result1 in results)
    {
      RelicPickingResult result = result1;
      NTreasureRoomRelicHolder holder = this._holdersInUse.First<NTreasureRoomRelicHolder>((Func<NTreasureRoomRelicHolder, bool>) (h => h.Relic.Model == result.relic));
      holder.AnimateAwayVotes();
      if (nullable1.HasValue)
      {
        int type = (int) result.type;
        RelicPickingResultType? nullable2 = nullable1;
        int valueOrDefault = (int) nullable2.GetValueOrDefault();
        if (!(type == valueOrDefault & nullable2.HasValue))
          await Cmd.Wait(0.5f, this._cts.Token);
      }
      if (result.type == RelicPickingResultType.FoughtOver)
      {
        Node parent = ((Node) this._fightBackstop).GetParent();
        ((Node) holder).Reparent(parent, true);
        parent.MoveChildSafely((Node) holder, ((Node) this._fightBackstop).GetIndex(false) + 1);
        ((CanvasItem) this._fightBackstop).Visible = true;
        Tween tween1 = ((Node) this).CreateTween();
        tween1.TweenProperty((GodotObject) holder, NodePath.op_Implicit("global_position"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.op_Subtraction(this._fightBackstop.Size, holder.Size), 0.5f)), 0.25).SetTrans((Tween.TransitionType) 10L).SetEase((Tween.EaseType) 0L);
        tween1.TweenProperty((GodotObject) this._fightBackstop, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.25);
        this._hands.BeforeFightStarted(result.fight.playersInvolved);
        await tween1.AwaitFinished(this._cts.Token);
        await Cmd.Wait(1f, this._cts.Token);
        await this._hands.DoFight(result, holder);
        Tween tween2 = ((Node) this).CreateTween();
        tween2.TweenProperty((GodotObject) this._fightBackstop, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.25);
        await tween2.AwaitFinished(this._cts.Token);
        ((CanvasItem) this._fightBackstop).Visible = false;
      }
      else if (result.type != RelicPickingResultType.Skipped)
      {
        NHandImage hand = this._hands.GetHand(result.player.NetId);
        if (hand != null)
        {
          tasksToWait.Add(TaskHelper.RunSafely(hand.GrabRelic(holder)));
          await Cmd.Wait(0.25f, this._cts.Token);
        }
      }
      nullable1 = new RelicPickingResultType?(result.type);
      holder = (NTreasureRoomRelicHolder) null;
    }
    await Task.WhenAll((IEnumerable<Task>) tasksToWait);
    if (tasksToWait.Count == 0 && this._runState.Players.Count > 1)
      await Cmd.Wait(0.7f, this._cts.Token);
    foreach (RelicPickingResult result2 in results)
    {
      RelicPickingResult result = result2;
      NTreasureRoomRelicHolder ntreasureRoomRelicHolder = this._holdersInUse.First<NTreasureRoomRelicHolder>((Func<NTreasureRoomRelicHolder, bool>) (h => h.Relic.Model == result.relic));
      RelicModel mutable = result.relic.ToMutable();
      ntreasureRoomRelicHolder.Disable();
      if (result.type != RelicPickingResultType.Skipped)
      {
        TaskHelper.RunSafely((Task) RelicCmd.Obtain(mutable, result.player));
        if (LocalContext.IsMe(result.player))
          NRun.Instance.GlobalUi.RelicInventory.AnimateRelic(mutable, new Vector2?(ntreasureRoomRelicHolder.GlobalPosition), new Vector2?(ntreasureRoomRelicHolder.Scale));
        if (this._runState.Players.Count == 1)
          ((CanvasItem) ntreasureRoomRelicHolder).Visible = false;
      }
      foreach (Player player in (IEnumerable<Player>) this._runState.Players)
      {
        if (player != result.player)
          player.RelicGrabBag.MoveToFallback(result.relic);
      }
    }
    this._relicPickingCompleteTaskCompletionSource.SetResult();
    tasksToWait = (List<Task>) null;
  }

  private void RefreshVotes()
  {
    foreach (NTreasureRoomRelicHolder ntreasureRoomRelicHolder in this._holdersInUse)
      ntreasureRoomRelicHolder.VoteContainer.RefreshPlayerVotes();
  }

  public Control? DefaultFocusedControl
  {
    get
    {
      return this._holdersInUse.Count <= 0 ? (Control) null : (Control) this._holdersInUse[this._runState.GetPlayerSlotIndex(LocalContext.GetMe((IEnumerable<Player>) this._runState.Players))];
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(10)
    {
      new MethodInfo(NTreasureRoomRelicCollection.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTreasureRoomRelicCollection.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTreasureRoomRelicCollection.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTreasureRoomRelicCollection.MethodName.InitializeRelics, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTreasureRoomRelicCollection.MethodName.SpawnEmptyChestVfx, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTreasureRoomRelicCollection.MethodName.SetSelectionEnabled, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("isEnabled"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NTreasureRoomRelicCollection.MethodName.AnimIn, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("chestVisual"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node"), false)
      }, (List<Variant>) null),
      new MethodInfo(NTreasureRoomRelicCollection.MethodName.AnimOut, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("chestVisual"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node"), false)
      }, (List<Variant>) null),
      new MethodInfo(NTreasureRoomRelicCollection.MethodName.PickRelic, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("holder"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NTreasureRoomRelicCollection.MethodName.RefreshVotes, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NTreasureRoomRelicCollection.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTreasureRoomRelicCollection.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTreasureRoomRelicCollection.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTreasureRoomRelicCollection.MethodName.InitializeRelics) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.InitializeRelics();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTreasureRoomRelicCollection.MethodName.SpawnEmptyChestVfx) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SpawnEmptyChestVfx();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTreasureRoomRelicCollection.MethodName.SetSelectionEnabled) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetSelectionEnabled(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTreasureRoomRelicCollection.MethodName.AnimIn) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.AnimIn(VariantUtils.ConvertTo<Node>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTreasureRoomRelicCollection.MethodName.AnimOut) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.AnimOut(VariantUtils.ConvertTo<Node>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTreasureRoomRelicCollection.MethodName.PickRelic) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.PickRelic(VariantUtils.ConvertTo<NTreasureRoomRelicHolder>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NTreasureRoomRelicCollection.MethodName.RefreshVotes) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.RefreshVotes();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NTreasureRoomRelicCollection.MethodName._Ready) || StringName.op_Equality(ref method, NTreasureRoomRelicCollection.MethodName._EnterTree) || StringName.op_Equality(ref method, NTreasureRoomRelicCollection.MethodName._ExitTree) || StringName.op_Equality(ref method, NTreasureRoomRelicCollection.MethodName.InitializeRelics) || StringName.op_Equality(ref method, NTreasureRoomRelicCollection.MethodName.SpawnEmptyChestVfx) || StringName.op_Equality(ref method, NTreasureRoomRelicCollection.MethodName.SetSelectionEnabled) || StringName.op_Equality(ref method, NTreasureRoomRelicCollection.MethodName.AnimIn) || StringName.op_Equality(ref method, NTreasureRoomRelicCollection.MethodName.AnimOut) || StringName.op_Equality(ref method, NTreasureRoomRelicCollection.MethodName.PickRelic) || StringName.op_Equality(ref method, NTreasureRoomRelicCollection.MethodName.RefreshVotes) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NTreasureRoomRelicCollection.PropertyName.SingleplayerRelicHolder))
    {
      this.SingleplayerRelicHolder = VariantUtils.ConvertTo<NTreasureRoomRelicHolder>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTreasureRoomRelicCollection.PropertyName._relicContainer))
    {
      this._relicContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTreasureRoomRelicCollection.PropertyName._fightBackstop))
    {
      this._fightBackstop = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTreasureRoomRelicCollection.PropertyName._fightLabel))
    {
      this._fightLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTreasureRoomRelicCollection.PropertyName._emptyVfxTween))
    {
      this._emptyVfxTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTreasureRoomRelicCollection.PropertyName._hands))
    {
      this._hands = VariantUtils.ConvertTo<NHandImageCollection>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTreasureRoomRelicCollection.PropertyName._openedTicks))
    {
      this._openedTicks = VariantUtils.ConvertTo<ulong>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NTreasureRoomRelicCollection.PropertyName._isEmptyChest))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._isEmptyChest = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NTreasureRoomRelicCollection.PropertyName.SingleplayerRelicHolder))
    {
      ref godot_variant local = ref value;
      NTreasureRoomRelicHolder singleplayerRelicHolder = this.SingleplayerRelicHolder;
      godot_variant from = VariantUtils.CreateFrom<NTreasureRoomRelicHolder>(ref singleplayerRelicHolder);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NTreasureRoomRelicCollection.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NTreasureRoomRelicCollection.PropertyName._relicContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._relicContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NTreasureRoomRelicCollection.PropertyName._fightBackstop))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._fightBackstop);
      return true;
    }
    if (StringName.op_Equality(ref name, NTreasureRoomRelicCollection.PropertyName._fightLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._fightLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NTreasureRoomRelicCollection.PropertyName._emptyVfxTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._emptyVfxTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NTreasureRoomRelicCollection.PropertyName._hands))
    {
      value = VariantUtils.CreateFrom<NHandImageCollection>(ref this._hands);
      return true;
    }
    if (StringName.op_Equality(ref name, NTreasureRoomRelicCollection.PropertyName._openedTicks))
    {
      value = VariantUtils.CreateFrom<ulong>(ref this._openedTicks);
      return true;
    }
    if (!StringName.op_Equality(ref name, NTreasureRoomRelicCollection.PropertyName._isEmptyChest))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._isEmptyChest);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NTreasureRoomRelicCollection.PropertyName._relicContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTreasureRoomRelicCollection.PropertyName._fightBackstop, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTreasureRoomRelicCollection.PropertyName._fightLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTreasureRoomRelicCollection.PropertyName._emptyVfxTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTreasureRoomRelicCollection.PropertyName._hands, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NTreasureRoomRelicCollection.PropertyName._openedTicks, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NTreasureRoomRelicCollection.PropertyName._isEmptyChest, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTreasureRoomRelicCollection.PropertyName.SingleplayerRelicHolder, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTreasureRoomRelicCollection.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName singleplayerRelicHolder1 = NTreasureRoomRelicCollection.PropertyName.SingleplayerRelicHolder;
    NTreasureRoomRelicHolder singleplayerRelicHolder2 = this.SingleplayerRelicHolder;
    Variant variant = Variant.From<NTreasureRoomRelicHolder>(ref singleplayerRelicHolder2);
    serializationInfo.AddProperty(singleplayerRelicHolder1, variant);
    info.AddProperty(NTreasureRoomRelicCollection.PropertyName._relicContainer, Variant.From<Control>(ref this._relicContainer));
    info.AddProperty(NTreasureRoomRelicCollection.PropertyName._fightBackstop, Variant.From<Control>(ref this._fightBackstop));
    info.AddProperty(NTreasureRoomRelicCollection.PropertyName._fightLabel, Variant.From<MegaLabel>(ref this._fightLabel));
    info.AddProperty(NTreasureRoomRelicCollection.PropertyName._emptyVfxTween, Variant.From<Tween>(ref this._emptyVfxTween));
    info.AddProperty(NTreasureRoomRelicCollection.PropertyName._hands, Variant.From<NHandImageCollection>(ref this._hands));
    info.AddProperty(NTreasureRoomRelicCollection.PropertyName._openedTicks, Variant.From<ulong>(ref this._openedTicks));
    info.AddProperty(NTreasureRoomRelicCollection.PropertyName._isEmptyChest, Variant.From<bool>(ref this._isEmptyChest));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NTreasureRoomRelicCollection.PropertyName.SingleplayerRelicHolder, ref variant1))
      this.SingleplayerRelicHolder = ((Variant) ref variant1).As<NTreasureRoomRelicHolder>();
    Variant variant2;
    if (info.TryGetProperty(NTreasureRoomRelicCollection.PropertyName._relicContainer, ref variant2))
      this._relicContainer = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NTreasureRoomRelicCollection.PropertyName._fightBackstop, ref variant3))
      this._fightBackstop = ((Variant) ref variant3).As<Control>();
    Variant variant4;
    if (info.TryGetProperty(NTreasureRoomRelicCollection.PropertyName._fightLabel, ref variant4))
      this._fightLabel = ((Variant) ref variant4).As<MegaLabel>();
    Variant variant5;
    if (info.TryGetProperty(NTreasureRoomRelicCollection.PropertyName._emptyVfxTween, ref variant5))
      this._emptyVfxTween = ((Variant) ref variant5).As<Tween>();
    Variant variant6;
    if (info.TryGetProperty(NTreasureRoomRelicCollection.PropertyName._hands, ref variant6))
      this._hands = ((Variant) ref variant6).As<NHandImageCollection>();
    Variant variant7;
    if (info.TryGetProperty(NTreasureRoomRelicCollection.PropertyName._openedTicks, ref variant7))
      this._openedTicks = ((Variant) ref variant7).As<ulong>();
    Variant variant8;
    if (!info.TryGetProperty(NTreasureRoomRelicCollection.PropertyName._isEmptyChest, ref variant8))
      return;
    this._isEmptyChest = ((Variant) ref variant8).As<bool>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName InitializeRelics = StringName.op_Implicit(nameof (InitializeRelics));
    public static readonly StringName SpawnEmptyChestVfx = StringName.op_Implicit(nameof (SpawnEmptyChestVfx));
    public static readonly StringName SetSelectionEnabled = StringName.op_Implicit(nameof (SetSelectionEnabled));
    public static readonly StringName AnimIn = StringName.op_Implicit(nameof (AnimIn));
    public static readonly StringName AnimOut = StringName.op_Implicit(nameof (AnimOut));
    public static readonly StringName PickRelic = StringName.op_Implicit(nameof (PickRelic));
    public static readonly StringName RefreshVotes = StringName.op_Implicit(nameof (RefreshVotes));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName SingleplayerRelicHolder = StringName.op_Implicit(nameof (SingleplayerRelicHolder));
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _relicContainer = StringName.op_Implicit(nameof (_relicContainer));
    public static readonly StringName _fightBackstop = StringName.op_Implicit(nameof (_fightBackstop));
    public static readonly StringName _fightLabel = StringName.op_Implicit(nameof (_fightLabel));
    public static readonly StringName _emptyVfxTween = StringName.op_Implicit(nameof (_emptyVfxTween));
    public static readonly StringName _hands = StringName.op_Implicit(nameof (_hands));
    public static readonly StringName _openedTicks = StringName.op_Implicit(nameof (_openedTicks));
    public static readonly StringName _isEmptyChest = StringName.op_Implicit(nameof (_isEmptyChest));
  }

  public class SignalName : Control.SignalName
  {
  }
}
