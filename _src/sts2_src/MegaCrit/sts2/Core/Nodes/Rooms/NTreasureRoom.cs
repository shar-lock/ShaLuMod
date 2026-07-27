// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Rooms.NTreasureRoom
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Ftue;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Nodes.Screens.TreasureRoomRelic;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Rooms;

[ScriptPath("res://src/Core/Nodes/Rooms/NTreasureRoom.cs")]
public class NTreasureRoom : Control, IScreenContext, IRoomWithProceedButton
{
  private TreasureRoom _room;
  private IRunState _runState;
  private NCommonBanner _banner;
  private NButton _chestButton;
  private Node2D _chestNode;
  private MegaSprite _chestAnimController;
  private NProceedButton _proceedButton;
  private MegaSkin? _regularChestSkin;
  private MegaSkin? _outlineChestSkin;
  private GpuParticles2D _goldParticles;
  private NTreasureRoomRelicCollection _relicCollection;
  private NMultiplayerVoteContainer _skipVoteContainer;
  private static readonly string _scenePath = SceneHelper.GetScenePath("rooms/treasure_room");
  private CancellationTokenSource _cts = new CancellationTokenSource();
  private bool _isRelicCollectionOpen;
  private bool _hasChestBeenOpened;

  public NProceedButton ProceedButton => this._proceedButton;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NTreasureRoom._scenePath);
    }
  }

  public static NTreasureRoom? Create(TreasureRoom room, IRunState runState)
  {
    if (TestMode.IsOn)
      return (NTreasureRoom) null;
    NTreasureRoom ntreasureRoom = PreloadManager.Cache.GetScene(NTreasureRoom._scenePath).Instantiate<NTreasureRoom>((PackedScene.GenEditState) 0L);
    ntreasureRoom._room = room;
    ntreasureRoom._runState = runState;
    return ntreasureRoom;
  }

  public override void _Ready()
  {
    this._banner = ((Node) this).GetNode<NCommonBanner>(NodePath.op_Implicit("%Banner"));
    if (this._runState.Players.Count == 1)
      this._banner.label.SetTextAutoSize(new LocString("gameplay_ui", "TREASURE_BANNER").GetRawText());
    else
      this._banner.label.SetTextAutoSize(new LocString("gameplay_ui", "CHOOSE_SHARED_RELIC_HEADER").GetRawText());
    this._proceedButton = ((Node) this).GetNode<NProceedButton>(NodePath.op_Implicit("%ProceedButton"));
    this._chestNode = ((Node) this).GetNode<Node2D>(NodePath.op_Implicit("%ChestVisual"));
    this._chestAnimController = new MegaSprite(Variant.op_Implicit((GodotObject) this._chestNode));
    this._goldParticles = ((Node) this).GetNode<GpuParticles2D>(NodePath.op_Implicit("%GoldExplosion"));
    this._relicCollection = ((Node) this).GetNode<NTreasureRoomRelicCollection>(NodePath.op_Implicit("%RelicCollection"));
    this._skipVoteContainer = ((Node) this).GetNode<NMultiplayerVoteContainer>(NodePath.op_Implicit("%SkipMultiplayerVoteContainer"));
    this._relicCollection.Initialize(this._runState);
    ((CanvasItem) this._relicCollection).Visible = false;
    this._chestAnimController.SetSkeletonDataRes(this._runState.Act.ChestSpineResource);
    MegaSkeleton skeleton = this._chestAnimController.GetSkeleton();
    if (skeleton != null)
    {
      MegaSkeletonDataResource data = skeleton.GetData();
      this._regularChestSkin = data.FindSkin(this._runState.Act.ChestSpineSkinNameNormal);
      this._outlineChestSkin = data.FindSkin(this._runState.Act.ChestSpineSkinNameStroke);
      skeleton.SetSlotsToSetupPose();
      this._chestAnimController.GetAnimationState().Apply(skeleton);
      MegaAnimationState animationState = this._chestAnimController.GetAnimationState();
      animationState.SetAnimation("animation", false);
      this._chestAnimController.GetAnimationState().AddAnimation("shine_fade", loop: false);
      animationState.SetTimeScale(0.0f);
      this.UpdateChestSkin(false);
    }
    ((GodotObject) this._proceedButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnProceedButtonPressed)), 0U);
    this._proceedButton.UpdateText(NProceedButton.ProceedLoc);
    this._skipVoteContainer.Initialize(new NMultiplayerVoteContainer.PlayerVotedDelegate(this.IsPlayerVotingForSkip), this._runState.Players);
    this._chestButton = ((Node) this).GetNode<NButton>(NodePath.op_Implicit("%Chest"));
    ((GodotObject) this._chestButton).Connect(Control.SignalName.MouseEntered, Callable.From(new Action(this.OnMouseEntered)), 0U);
    ((GodotObject) this._chestButton).Connect(Control.SignalName.MouseExited, Callable.From(new Action(this.OnMouseExited)), 0U);
    ((GodotObject) this._chestButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnChestButtonReleased)), 0U);
    NGame.Instance.SetScreenShakeTarget((Control) this);
  }

  public override void _EnterTree()
  {
    this._cts = new CancellationTokenSource();
    ActiveScreenContext.Instance.Updated += new Action(this.OnActiveScreenChanged);
    RunManager.Instance.TreasureRoomRelicSynchronizer.VotesChanged += new Action(this.RefreshVotes);
  }

  public override void _ExitTree()
  {
    this._cts.Cancel();
    ActiveScreenContext.Instance.Updated -= new Action(this.OnActiveScreenChanged);
    RunManager.Instance.TreasureRoomRelicSynchronizer.VotesChanged -= new Action(this.RefreshVotes);
    NGame.Instance.ClearScreenShakeTarget();
  }

  private void OnProceedButtonPressed(NButton _)
  {
    if (this._proceedButton.IsSkip)
    {
      RunManager.Instance.TreasureRoomRelicSynchronizer.SkipRelicLocally();
      if (this._runState.Players.Count != 1)
        return;
      NMapScreen.Instance.SetTravelEnabled(true);
      TaskHelper.RunSafely(RunManager.Instance.ProceedFromTerminalRewardsScreen());
    }
    else
      TaskHelper.RunSafely(RunManager.Instance.ProceedFromTerminalRewardsScreen());
  }

  private void RefreshVotes() => this._skipVoteContainer.RefreshPlayerVotes();

  private void OnProceedButtonReleased(NButton _) => NMapScreen.Instance.Open();

  private void OnChestButtonReleased(NButton _)
  {
    TaskHelper.RunSafely(this.OpenChest());
    this._chestButton.Disable();
  }

  private void OnMouseEntered() => this.UpdateChestSkin(true);

  private void OnMouseExited() => this.UpdateChestSkin(false);

  private async Task OpenChest()
  {
    this._banner.AnimateIn();
    this._proceedButton.Disable();
    this.UpdateChestSkin(false);
    SfxCmd.Play(this._runState.Act.ChestOpenSfx);
    this._chestAnimController.GetAnimationState().SetTimeScale(1f);
    this._chestButton.MouseFilter = (Control.MouseFilterEnum) 2L;
    int num = await this._room.DoNormalRewards();
    if (num > 0)
    {
      this._goldParticles.Amount = num;
      this._goldParticles.Emitting = true;
    }
    await this._room.DoExtraRewardsIfNeeded();
    this._relicCollection.InitializeRelics();
    this._relicCollection.AnimIn((Node) this._chestNode);
    this._isRelicCollectionOpen = true;
    Control defaultFocusedControl = this.DefaultFocusedControl;
    if (defaultFocusedControl != null)
      defaultFocusedControl.TryGrabFocus();
    TaskHelper.RunSafely(this.RelicFtueCheck());
    CancellationTokenSource cancelSource = new CancellationTokenSource();
    if (this._runState.Players.Count == 1)
    {
      this._proceedButton.UpdateText(NProceedButton.SkipLoc);
      TaskHelper.RunSafely(this.EnableSkipAfterDelay(2.5f, cancelSource.Token));
    }
    this._hasChestBeenOpened = true;
    await this._relicCollection.RelicPickingBegan();
    await cancelSource.CancelAsync();
    this._proceedButton.Disable();
    await this._relicCollection.RelicPickingFinished();
    this._isRelicCollectionOpen = false;
    this._skipVoteContainer.RefreshPlayerVotes();
    this._proceedButton.UpdateText(NProceedButton.ProceedLoc);
    this._proceedButton.Enable();
    this._banner.AnimateOut();
    NMapScreen.Instance.SetTravelEnabled(true);
    this._relicCollection.AnimOut((Node) this._chestNode);
    cancelSource = (CancellationTokenSource) null;
  }

  private async Task EnableSkipAfterDelay(float delay, CancellationToken token)
  {
    await Cmd.Wait(delay, token);
    if (token.IsCancellationRequested)
      return;
    this._proceedButton.Enable();
  }

  private async Task RelicFtueCheck()
  {
    if (SaveManager.Instance.SeenFtue("obtain_relic_ftue"))
      return;
    this._relicCollection.SetSelectionEnabled(false);
    await Cmd.Wait(1f, this._cts.Token);
    Control relicReward = !((CanvasItem) this._relicCollection.SingleplayerRelicHolder).Visible ? (Control) this._relicCollection : (Control) this._relicCollection.SingleplayerRelicHolder;
    this._relicCollection.SetSelectionEnabled(true);
    NModalContainer.Instance.Add((Node) NRelicRewardFtue.Create(relicReward));
    SaveManager.Instance.MarkFtueAsComplete("obtain_relic_ftue");
  }

  private void UpdateChestSkin(bool showOutline)
  {
    MegaSkeleton skeleton = this._chestAnimController.GetSkeleton();
    if (skeleton == null)
      return;
    skeleton.SetSkin(showOutline ? this._outlineChestSkin : this._regularChestSkin);
    skeleton.SetSlotsToSetupPose();
    this._chestAnimController.GetAnimationState().Apply(skeleton);
  }

  private void OnActiveScreenChanged()
  {
    this.UpdateControllerNavEnabled<NTreasureRoom>();
    if (ActiveScreenContext.Instance.IsCurrent((IScreenContext) this) && this._hasChestBeenOpened)
      this._proceedButton.Enable();
    else
      this._proceedButton.Disable();
  }

  private bool IsPlayerVotingForSkip(Player player)
  {
    if (RunManager.Instance.TreasureRoomRelicSynchronizer.CurrentRelics == null)
      return false;
    TreasureRoomRelicSynchronizer.PlayerVote playerVote = RunManager.Instance.TreasureRoomRelicSynchronizer.GetPlayerVote(player);
    return playerVote != null && playerVote.voteReceived && !playerVote.index.HasValue;
  }

  public Control? DefaultFocusedControl
  {
    get
    {
      return !this._isRelicCollectionOpen ? (Control) null : this._relicCollection.DefaultFocusedControl;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(11)
    {
      new MethodInfo(NTreasureRoom.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTreasureRoom.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTreasureRoom.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTreasureRoom.MethodName.OnProceedButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NTreasureRoom.MethodName.RefreshVotes, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTreasureRoom.MethodName.OnProceedButtonReleased, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NTreasureRoom.MethodName.OnChestButtonReleased, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NTreasureRoom.MethodName.OnMouseEntered, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTreasureRoom.MethodName.OnMouseExited, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTreasureRoom.MethodName.UpdateChestSkin, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("showOutline"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NTreasureRoom.MethodName.OnActiveScreenChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NTreasureRoom.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTreasureRoom.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTreasureRoom.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTreasureRoom.MethodName.OnProceedButtonPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnProceedButtonPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTreasureRoom.MethodName.RefreshVotes) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshVotes();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTreasureRoom.MethodName.OnProceedButtonReleased) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnProceedButtonReleased(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTreasureRoom.MethodName.OnChestButtonReleased) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnChestButtonReleased(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTreasureRoom.MethodName.OnMouseEntered) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnMouseEntered();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTreasureRoom.MethodName.OnMouseExited) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnMouseExited();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTreasureRoom.MethodName.UpdateChestSkin) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateChestSkin(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NTreasureRoom.MethodName.OnActiveScreenChanged) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnActiveScreenChanged();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NTreasureRoom.MethodName._Ready) || StringName.op_Equality(ref method, NTreasureRoom.MethodName._EnterTree) || StringName.op_Equality(ref method, NTreasureRoom.MethodName._ExitTree) || StringName.op_Equality(ref method, NTreasureRoom.MethodName.OnProceedButtonPressed) || StringName.op_Equality(ref method, NTreasureRoom.MethodName.RefreshVotes) || StringName.op_Equality(ref method, NTreasureRoom.MethodName.OnProceedButtonReleased) || StringName.op_Equality(ref method, NTreasureRoom.MethodName.OnChestButtonReleased) || StringName.op_Equality(ref method, NTreasureRoom.MethodName.OnMouseEntered) || StringName.op_Equality(ref method, NTreasureRoom.MethodName.OnMouseExited) || StringName.op_Equality(ref method, NTreasureRoom.MethodName.UpdateChestSkin) || StringName.op_Equality(ref method, NTreasureRoom.MethodName.OnActiveScreenChanged) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NTreasureRoom.PropertyName._banner))
    {
      this._banner = VariantUtils.ConvertTo<NCommonBanner>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTreasureRoom.PropertyName._chestButton))
    {
      this._chestButton = VariantUtils.ConvertTo<NButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTreasureRoom.PropertyName._chestNode))
    {
      this._chestNode = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTreasureRoom.PropertyName._proceedButton))
    {
      this._proceedButton = VariantUtils.ConvertTo<NProceedButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTreasureRoom.PropertyName._goldParticles))
    {
      this._goldParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTreasureRoom.PropertyName._relicCollection))
    {
      this._relicCollection = VariantUtils.ConvertTo<NTreasureRoomRelicCollection>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTreasureRoom.PropertyName._skipVoteContainer))
    {
      this._skipVoteContainer = VariantUtils.ConvertTo<NMultiplayerVoteContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTreasureRoom.PropertyName._isRelicCollectionOpen))
    {
      this._isRelicCollectionOpen = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NTreasureRoom.PropertyName._hasChestBeenOpened))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._hasChestBeenOpened = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NTreasureRoom.PropertyName.ProceedButton))
    {
      ref godot_variant local = ref value;
      NProceedButton proceedButton = this.ProceedButton;
      godot_variant from = VariantUtils.CreateFrom<NProceedButton>(ref proceedButton);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NTreasureRoom.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NTreasureRoom.PropertyName._banner))
    {
      value = VariantUtils.CreateFrom<NCommonBanner>(ref this._banner);
      return true;
    }
    if (StringName.op_Equality(ref name, NTreasureRoom.PropertyName._chestButton))
    {
      value = VariantUtils.CreateFrom<NButton>(ref this._chestButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NTreasureRoom.PropertyName._chestNode))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._chestNode);
      return true;
    }
    if (StringName.op_Equality(ref name, NTreasureRoom.PropertyName._proceedButton))
    {
      value = VariantUtils.CreateFrom<NProceedButton>(ref this._proceedButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NTreasureRoom.PropertyName._goldParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._goldParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NTreasureRoom.PropertyName._relicCollection))
    {
      value = VariantUtils.CreateFrom<NTreasureRoomRelicCollection>(ref this._relicCollection);
      return true;
    }
    if (StringName.op_Equality(ref name, NTreasureRoom.PropertyName._skipVoteContainer))
    {
      value = VariantUtils.CreateFrom<NMultiplayerVoteContainer>(ref this._skipVoteContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NTreasureRoom.PropertyName._isRelicCollectionOpen))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isRelicCollectionOpen);
      return true;
    }
    if (!StringName.op_Equality(ref name, NTreasureRoom.PropertyName._hasChestBeenOpened))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._hasChestBeenOpened);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NTreasureRoom.PropertyName._banner, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTreasureRoom.PropertyName._chestButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTreasureRoom.PropertyName._chestNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTreasureRoom.PropertyName._proceedButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTreasureRoom.PropertyName.ProceedButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTreasureRoom.PropertyName._goldParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTreasureRoom.PropertyName._relicCollection, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTreasureRoom.PropertyName._skipVoteContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NTreasureRoom.PropertyName._isRelicCollectionOpen, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NTreasureRoom.PropertyName._hasChestBeenOpened, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTreasureRoom.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NTreasureRoom.PropertyName._banner, Variant.From<NCommonBanner>(ref this._banner));
    info.AddProperty(NTreasureRoom.PropertyName._chestButton, Variant.From<NButton>(ref this._chestButton));
    info.AddProperty(NTreasureRoom.PropertyName._chestNode, Variant.From<Node2D>(ref this._chestNode));
    info.AddProperty(NTreasureRoom.PropertyName._proceedButton, Variant.From<NProceedButton>(ref this._proceedButton));
    info.AddProperty(NTreasureRoom.PropertyName._goldParticles, Variant.From<GpuParticles2D>(ref this._goldParticles));
    info.AddProperty(NTreasureRoom.PropertyName._relicCollection, Variant.From<NTreasureRoomRelicCollection>(ref this._relicCollection));
    info.AddProperty(NTreasureRoom.PropertyName._skipVoteContainer, Variant.From<NMultiplayerVoteContainer>(ref this._skipVoteContainer));
    info.AddProperty(NTreasureRoom.PropertyName._isRelicCollectionOpen, Variant.From<bool>(ref this._isRelicCollectionOpen));
    info.AddProperty(NTreasureRoom.PropertyName._hasChestBeenOpened, Variant.From<bool>(ref this._hasChestBeenOpened));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NTreasureRoom.PropertyName._banner, ref variant1))
      this._banner = ((Variant) ref variant1).As<NCommonBanner>();
    Variant variant2;
    if (info.TryGetProperty(NTreasureRoom.PropertyName._chestButton, ref variant2))
      this._chestButton = ((Variant) ref variant2).As<NButton>();
    Variant variant3;
    if (info.TryGetProperty(NTreasureRoom.PropertyName._chestNode, ref variant3))
      this._chestNode = ((Variant) ref variant3).As<Node2D>();
    Variant variant4;
    if (info.TryGetProperty(NTreasureRoom.PropertyName._proceedButton, ref variant4))
      this._proceedButton = ((Variant) ref variant4).As<NProceedButton>();
    Variant variant5;
    if (info.TryGetProperty(NTreasureRoom.PropertyName._goldParticles, ref variant5))
      this._goldParticles = ((Variant) ref variant5).As<GpuParticles2D>();
    Variant variant6;
    if (info.TryGetProperty(NTreasureRoom.PropertyName._relicCollection, ref variant6))
      this._relicCollection = ((Variant) ref variant6).As<NTreasureRoomRelicCollection>();
    Variant variant7;
    if (info.TryGetProperty(NTreasureRoom.PropertyName._skipVoteContainer, ref variant7))
      this._skipVoteContainer = ((Variant) ref variant7).As<NMultiplayerVoteContainer>();
    Variant variant8;
    if (info.TryGetProperty(NTreasureRoom.PropertyName._isRelicCollectionOpen, ref variant8))
      this._isRelicCollectionOpen = ((Variant) ref variant8).As<bool>();
    Variant variant9;
    if (!info.TryGetProperty(NTreasureRoom.PropertyName._hasChestBeenOpened, ref variant9))
      return;
    this._hasChestBeenOpened = ((Variant) ref variant9).As<bool>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName OnProceedButtonPressed = StringName.op_Implicit(nameof (OnProceedButtonPressed));
    public static readonly StringName RefreshVotes = StringName.op_Implicit(nameof (RefreshVotes));
    public static readonly StringName OnProceedButtonReleased = StringName.op_Implicit(nameof (OnProceedButtonReleased));
    public static readonly StringName OnChestButtonReleased = StringName.op_Implicit(nameof (OnChestButtonReleased));
    public static readonly StringName OnMouseEntered = StringName.op_Implicit(nameof (OnMouseEntered));
    public static readonly StringName OnMouseExited = StringName.op_Implicit(nameof (OnMouseExited));
    public static readonly StringName UpdateChestSkin = StringName.op_Implicit(nameof (UpdateChestSkin));
    public static readonly StringName OnActiveScreenChanged = StringName.op_Implicit(nameof (OnActiveScreenChanged));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName ProceedButton = StringName.op_Implicit(nameof (ProceedButton));
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _banner = StringName.op_Implicit(nameof (_banner));
    public static readonly StringName _chestButton = StringName.op_Implicit(nameof (_chestButton));
    public static readonly StringName _chestNode = StringName.op_Implicit(nameof (_chestNode));
    public static readonly StringName _proceedButton = StringName.op_Implicit(nameof (_proceedButton));
    public static readonly StringName _goldParticles = StringName.op_Implicit(nameof (_goldParticles));
    public static readonly StringName _relicCollection = StringName.op_Implicit(nameof (_relicCollection));
    public static readonly StringName _skipVoteContainer = StringName.op_Implicit(nameof (_skipVoteContainer));
    public static readonly StringName _isRelicCollectionOpen = StringName.op_Implicit(nameof (_isRelicCollectionOpen));
    public static readonly StringName _hasChestBeenOpened = StringName.op_Implicit(nameof (_hasChestBeenOpened));
  }

  public class SignalName : Control.SignalName
  {
  }
}
