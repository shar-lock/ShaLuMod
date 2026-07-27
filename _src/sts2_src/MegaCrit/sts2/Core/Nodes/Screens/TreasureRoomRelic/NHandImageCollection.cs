// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.TreasureRoomRelic.NHandImageCollection
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.TreasureRelicPicking;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.TreasureRoomRelic;

[ScriptPath("res://src/Core/Nodes/Screens/TreasureRoomRelic/NHandImageCollection.cs")]
public class NHandImageCollection : Control
{
  private CancellationTokenSource _cts = new CancellationTokenSource();
  private PeerInputSynchronizer? _synchronizer;
  private readonly List<NHandImage> _hands = new List<NHandImage>();
  private IRunState _runState = (IRunState) NullRunState.Instance;
  private float _handAnimateInProgress;

  public override void _EnterTree()
  {
    ((Node) this)._EnterTree();
    this._cts = new CancellationTokenSource();
    ((GodotObject) ((Node) this).GetViewport()).Connect(Viewport.SignalName.GuiFocusChanged, Callable.From<Control>(new Action<Control>(this.ProcessGuiFocus)), 0U);
  }

  public override void _ExitTree()
  {
    this._cts.Cancel();
    if (this._synchronizer != null)
    {
      this._synchronizer.StateAdded -= new Action<ulong>(this.OnInputStateAdded);
      this._synchronizer.StateChanged -= new Action<ulong>(this.OnInputStateChanged);
      this._synchronizer.StateRemoved -= new Action<ulong>(this.OnInputStateRemoved);
    }
    NGame.Instance.CursorManager.SetCursorShown(true);
    ((GodotObject) ((Node) this).GetViewport()).Disconnect(Viewport.SignalName.GuiFocusChanged, Callable.From<Control>(new Action<Control>(this.ProcessGuiFocus)));
  }

  public void Initialize(IRunState runState)
  {
    this._runState = runState;
    if (this._runState.Players.Count <= 1)
      return;
    this._synchronizer = RunManager.Instance.InputSynchronizer;
    this._synchronizer.StateAdded += new Action<ulong>(this.OnInputStateAdded);
    this._synchronizer.StateChanged += new Action<ulong>(this.OnInputStateChanged);
    this._synchronizer.StateRemoved += new Action<ulong>(this.OnInputStateRemoved);
    foreach (Player player in (IEnumerable<Player>) this._runState.Players)
      this.AddHand(player.NetId);
    this.UpdateHandVisibility();
  }

  private void OnInputStateAdded(ulong playerId) => this.AddHand(playerId);

  private void OnInputStateRemoved(ulong playerId) => this.RemoveHand(playerId);

  private void AddHand(ulong playerId)
  {
    if (this._hands.Any<NHandImage>((Func<NHandImage, bool>) (c => (long) c.Player.NetId == (long) playerId)))
    {
      Log.Error($"Tried to add hand for player {playerId} twice!");
    }
    else
    {
      Player player = this._runState.GetPlayer(playerId);
      NHandImage child = NHandImage.Create(player, this._runState.GetPlayerSlotIndex(player));
      this._hands.Add(child);
      ((Node) this).AddChildSafely((Node) child);
    }
  }

  private void OnInputStateChanged(ulong playerId)
  {
    Vector2 spaceFocusPosition = this._synchronizer.GetControlSpaceFocusPosition(playerId, (Control) this);
    NHandImage hand = this.GetHand(playerId);
    hand.SetIsDown(this._synchronizer.GetMouseDown(playerId));
    hand.SetPointingPosition(spaceFocusPosition);
    this.UpdateHandVisibility();
  }

  public override void _Input(InputEvent inputEvent)
  {
    if (this._runState.Players.Count == 1)
      return;
    switch (inputEvent)
    {
      case InputEventMouseMotion _:
        this.GetHand(LocalContext.NetId.Value).SetPointingPosition(((CanvasItem) this).GetGlobalMousePosition());
        break;
      case InputEventMouseButton eventMouseButton:
        if (eventMouseButton.ButtonIndex != 1L)
          break;
        NHandImage hand = this.GetHand(LocalContext.NetId.Value);
        if (((InputEvent) eventMouseButton).IsPressed() && !hand.IsDown)
        {
          hand.SetIsDown(true);
          break;
        }
        if (!((InputEvent) eventMouseButton).IsReleased() || !hand.IsDown)
          break;
        hand.SetIsDown(false);
        break;
    }
  }

  private void ProcessGuiFocus(Control focusedControl)
  {
    if (!((CanvasItem) this).IsVisibleInTree() || !NControllerManager.Instance.IsUsingController || this._runState.Players.Count == 1)
      return;
    if (focusedControl is NTreasureRoomRelicHolder)
    {
      NHandImage hand = this.GetHand(LocalContext.NetId.Value);
      Vector2 down = Vector2.Down;
      Vector2 vector2 = ((Vector2) ref down).Rotated(hand.Rotation);
      Vector2 position = Vector2.op_Addition(Vector2.op_Addition(focusedControl.GlobalPosition, Vector2.op_Multiply(focusedControl.Size, 0.5f)), Vector2.op_Multiply(vector2, 100f));
      hand.SetPointingPosition(position);
    }
    else
      this.GetHand(LocalContext.NetId.Value).AnimateAway();
  }

  public NHandImage? GetHand(ulong playerId)
  {
    return this._hands.FirstOrDefault<NHandImage>((Func<NHandImage, bool>) (c => (long) c.Player.NetId == (long) playerId));
  }

  private void RemoveHand(ulong playerId)
  {
    NHandImage hand = this.GetHand(playerId);
    if (hand == null)
      return;
    ((Node) hand).QueueFreeSafely();
    this._hands.Remove(hand);
  }

  private void UpdateHandVisibility()
  {
    NetScreenType screenType = this._synchronizer.GetScreenType(LocalContext.NetId.Value);
    foreach (NHandImage hand in this._hands)
    {
      bool flag = (!RunManager.Instance.IsSingleplayerOrFakeMultiplayer ? (int) this._synchronizer.GetScreenType(hand.Player.NetId) : 13) == 13 && screenType == NetScreenType.SharedRelicPicking;
      if (!hand.IsShown & flag)
        hand.AnimateIn();
      else if (hand.IsShown && !flag)
        hand.AnimateAway();
    }
    NGame.Instance.CursorManager.SetCursorShown(screenType != NetScreenType.SharedRelicPicking);
  }

  public void BeforeRelicsAwarded()
  {
    foreach (NHandImage hand in this._hands)
      hand.SetFrozenForRelicAwards(true);
  }

  public void BeforeFightStarted(List<Player> playersInvolved)
  {
    foreach (Player player in playersInvolved)
      this.GetHand(player.NetId).SetIsInFight(true);
  }

  public void AnimateHandsIn()
  {
    foreach (NHandImage hand in this._hands)
    {
      if (((CanvasItem) hand).Visible)
        hand.AnimateIn();
    }
  }

  public async Task DoFight(RelicPickingResult result, NTreasureRoomRelicHolder holder)
  {
    RelicPickingFight fight = result.fight;
    List<Tween> tweens = new List<Tween>();
    List<Task> tasks = new List<Task>();
    for (int i = 0; i < fight.rounds.Count; ++i)
    {
      float durationMultiplier = (float) (1.5 / ((double) i + 1.5));
      RelicPickingFightRound round = fight.rounds[i];
      tweens.Clear();
      for (int index = 0; index < fight.playersInvolved.Count; ++index)
      {
        RelicPickingFightMove? move = round.moves[index];
        if (move.HasValue)
        {
          Player player = fight.playersInvolved[index];
          tweens.Add(this.GetHand(player.NetId).DoFightMove(move.Value, 1.5f * durationMultiplier));
        }
      }
      await Task.WhenAll(tweens.Select<Tween, Task>((Func<Tween, Task>) (t => t.AwaitFinished(this._cts.Token))));
      List<Player> playerList = new List<Player>();
      for (int index = 0; index < fight.playersInvolved.Count; ++index)
      {
        Player player = fight.playersInvolved[index];
        RelicPickingFightMove? move;
        if (i < fight.rounds.Count - 1)
        {
          move = round.moves[index];
          if (move.HasValue)
          {
            move = fight.rounds[i + 1].moves[index];
            if (!move.HasValue)
              playerList.Add(player);
          }
        }
        else
        {
          move = round.moves[index];
          if (move.HasValue && result.player != player)
            playerList.Add(player);
        }
      }
      tasks.Clear();
      foreach (Player player in playerList)
        tasks.Add(this.DoLoseShake(player, Mathf.Max(1f * durationMultiplier, 0.5f)));
      if (i == fight.rounds.Count - 1)
      {
        await Cmd.Wait(0.5f, this._cts.Token);
        tasks.Add(this.GetHand(result.player.NetId).GrabRelic(holder));
      }
      if (tasks.Count == 0)
        await Cmd.Wait(1f * durationMultiplier, this._cts.Token);
      else
        await Task.WhenAll((IEnumerable<Task>) tasks);
      round = (RelicPickingFightRound) null;
    }
    foreach (Player player in fight.playersInvolved)
      this.GetHand(player.NetId).SetIsInFight(false);
    fight = (RelicPickingFight) null;
    tweens = (List<Tween>) null;
    tasks = (List<Task>) null;
  }

  private async Task DoLoseShake(Player player, float duration)
  {
    NHandImage hand = this.GetHand(player.NetId);
    await hand.DoLoseShake(duration);
    hand.SetIsInFight(false);
    hand = (NHandImage) null;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(13)
    {
      new MethodInfo(NHandImageCollection.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHandImageCollection.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHandImageCollection.MethodName.OnInputStateAdded, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("playerId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NHandImageCollection.MethodName.OnInputStateRemoved, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("playerId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NHandImageCollection.MethodName.AddHand, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("playerId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NHandImageCollection.MethodName.OnInputStateChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("playerId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NHandImageCollection.MethodName._Input, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NHandImageCollection.MethodName.ProcessGuiFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("focusedControl"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NHandImageCollection.MethodName.GetHand, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("playerId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NHandImageCollection.MethodName.RemoveHand, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("playerId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NHandImageCollection.MethodName.UpdateHandVisibility, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHandImageCollection.MethodName.BeforeRelicsAwarded, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHandImageCollection.MethodName.AnimateHandsIn, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NHandImageCollection.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHandImageCollection.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHandImageCollection.MethodName.OnInputStateAdded) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnInputStateAdded(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHandImageCollection.MethodName.OnInputStateRemoved) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnInputStateRemoved(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHandImageCollection.MethodName.AddHand) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.AddHand(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHandImageCollection.MethodName.OnInputStateChanged) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnInputStateChanged(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHandImageCollection.MethodName._Input) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Input(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHandImageCollection.MethodName.ProcessGuiFocus) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ProcessGuiFocus(VariantUtils.ConvertTo<Control>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHandImageCollection.MethodName.GetHand) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NHandImage hand = this.GetHand(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NHandImage>(ref hand);
      return true;
    }
    if (StringName.op_Equality(ref method, NHandImageCollection.MethodName.RemoveHand) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.RemoveHand(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHandImageCollection.MethodName.UpdateHandVisibility) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateHandVisibility();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHandImageCollection.MethodName.BeforeRelicsAwarded) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.BeforeRelicsAwarded();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NHandImageCollection.MethodName.AnimateHandsIn) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.AnimateHandsIn();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NHandImageCollection.MethodName._EnterTree) || StringName.op_Equality(ref method, NHandImageCollection.MethodName._ExitTree) || StringName.op_Equality(ref method, NHandImageCollection.MethodName.OnInputStateAdded) || StringName.op_Equality(ref method, NHandImageCollection.MethodName.OnInputStateRemoved) || StringName.op_Equality(ref method, NHandImageCollection.MethodName.AddHand) || StringName.op_Equality(ref method, NHandImageCollection.MethodName.OnInputStateChanged) || StringName.op_Equality(ref method, NHandImageCollection.MethodName._Input) || StringName.op_Equality(ref method, NHandImageCollection.MethodName.ProcessGuiFocus) || StringName.op_Equality(ref method, NHandImageCollection.MethodName.GetHand) || StringName.op_Equality(ref method, NHandImageCollection.MethodName.RemoveHand) || StringName.op_Equality(ref method, NHandImageCollection.MethodName.UpdateHandVisibility) || StringName.op_Equality(ref method, NHandImageCollection.MethodName.BeforeRelicsAwarded) || StringName.op_Equality(ref method, NHandImageCollection.MethodName.AnimateHandsIn) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NHandImageCollection.PropertyName._handAnimateInProgress))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._handAnimateInProgress = VariantUtils.ConvertTo<float>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NHandImageCollection.PropertyName._handAnimateInProgress))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<float>(ref this._handAnimateInProgress);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 3L, NHandImageCollection.PropertyName._handAnimateInProgress, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NHandImageCollection.PropertyName._handAnimateInProgress, Variant.From<float>(ref this._handAnimateInProgress));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(NHandImageCollection.PropertyName._handAnimateInProgress, ref variant))
      return;
    this._handAnimateInProgress = ((Variant) ref variant).As<float>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName OnInputStateAdded = StringName.op_Implicit(nameof (OnInputStateAdded));
    public static readonly StringName OnInputStateRemoved = StringName.op_Implicit(nameof (OnInputStateRemoved));
    public static readonly StringName AddHand = StringName.op_Implicit(nameof (AddHand));
    public static readonly StringName OnInputStateChanged = StringName.op_Implicit(nameof (OnInputStateChanged));
    public static readonly StringName _Input = StringName.op_Implicit(nameof (_Input));
    public static readonly StringName ProcessGuiFocus = StringName.op_Implicit(nameof (ProcessGuiFocus));
    public static readonly StringName GetHand = StringName.op_Implicit(nameof (GetHand));
    public static readonly StringName RemoveHand = StringName.op_Implicit(nameof (RemoveHand));
    public static readonly StringName UpdateHandVisibility = StringName.op_Implicit(nameof (UpdateHandVisibility));
    public static readonly StringName BeforeRelicsAwarded = StringName.op_Implicit(nameof (BeforeRelicsAwarded));
    public static readonly StringName AnimateHandsIn = StringName.op_Implicit(nameof (AnimateHandsIn));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _handAnimateInProgress = StringName.op_Implicit(nameof (_handAnimateInProgress));
  }

  public class SignalName : Control.SignalName
  {
  }
}
