// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Multiplayer.NRemoteMouseCursorContainer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Multiplayer;

[ScriptPath("res://src/Core/Nodes/Multiplayer/NRemoteMouseCursorContainer.cs")]
public class NRemoteMouseCursorContainer : Control
{
  private static bool _isDebugUiVisible = true;
  private PeerInputSynchronizer? _synchronizer;
  private readonly List<NRemoteMouseCursor> _cursors = new List<NRemoteMouseCursor>();

  public void Initialize(PeerInputSynchronizer synchronizer, IEnumerable<ulong> connectedPlayerIds)
  {
    if (this._synchronizer != null)
      this.Deinitialize();
    this._synchronizer = synchronizer;
    this._synchronizer.StateAdded += new Action<ulong>(this.OnInputStateAdded);
    this._synchronizer.StateChanged += new Action<ulong>(this.OnInputStateChanged);
    this._synchronizer.StateRemoved += new Action<ulong>(this.OnInputStateRemoved);
    this._synchronizer.NetService.Disconnected += new Action<NetErrorInfo>(this.NetServiceDisconnected);
  }

  private void NetServiceDisconnected(NetErrorInfo _) => this.Deinitialize();

  public void Deinitialize()
  {
    if (this._synchronizer != null)
    {
      this._synchronizer.StateAdded -= new Action<ulong>(this.OnInputStateAdded);
      this._synchronizer.StateChanged -= new Action<ulong>(this.OnInputStateChanged);
      this._synchronizer.StateRemoved -= new Action<ulong>(this.OnInputStateRemoved);
      this._synchronizer.NetService.Disconnected -= new Action<NetErrorInfo>(this.NetServiceDisconnected);
      this._synchronizer.Dispose();
      this._synchronizer = (PeerInputSynchronizer) null;
    }
    foreach (Node cursor in this._cursors)
      cursor.QueueFreeSafely();
    this._cursors.Clear();
  }

  public override void _Ready()
  {
    ((GodotObject) ((Node) this).GetViewport()).Connect(Viewport.SignalName.GuiFocusChanged, Callable.From<Control>(new Action<Control>(this.OnGuiFocusChanged)), 0U);
  }

  public override void _ExitTree() => this.Deinitialize();

  public void ForceUpdateAllCursors()
  {
    foreach (NRemoteMouseCursor cursor in this._cursors)
      this.OnInputStateChanged(cursor.PlayerId);
  }

  public Vector2 GetCursorPosition(ulong playerId) => this.GetCursor(playerId).Position;

  private void OnInputStateAdded(ulong playerId) => this.AddCursor(playerId);

  private void OnInputStateRemoved(ulong playerId) => this.RemoveCursor(playerId);

  private void AddCursor(ulong playerId)
  {
    long num = (long) playerId;
    ulong? netId = this._synchronizer?.NetService.NetId;
    long valueOrDefault = (long) netId.GetValueOrDefault();
    if (num == valueOrDefault & netId.HasValue)
      return;
    if (this._cursors.Any<NRemoteMouseCursor>((Func<NRemoteMouseCursor, bool>) (c => (long) c.PlayerId == (long) playerId)))
    {
      Log.Error($"Tried to add cursor for player {playerId} twice!");
    }
    else
    {
      NRemoteMouseCursor child = NRemoteMouseCursor.Create(playerId);
      this._cursors.Add(child);
      ((Node) this).AddChildSafely((Node) child);
    }
  }

  private void OnInputStateChanged(ulong playerId)
  {
    long num = (long) playerId;
    ulong? netId = this._synchronizer?.NetService.NetId;
    long valueOrDefault = (long) netId.GetValueOrDefault();
    if (num == valueOrDefault & netId.HasValue)
    {
      this.UpdateCursorVisibility();
    }
    else
    {
      Vector2 spaceFocusPosition = this._synchronizer.GetControlSpaceFocusPosition(playerId, (Control) this);
      NRemoteMouseCursor cursor = this.GetCursor(playerId);
      cursor.SetNextPosition(spaceFocusPosition);
      cursor.UpdateImage(this._synchronizer.GetMouseDown(playerId), NRemoteMouseCursorContainer.GetDrawingMode(playerId));
      this.UpdateCursorVisibility();
    }
  }

  public void DrawingCursorStateChanged(ulong playerId)
  {
    this.GetCursor(playerId)?.UpdateImage(this._synchronizer.GetMouseDown(playerId), NRemoteMouseCursorContainer.GetDrawingMode(playerId));
  }

  private static DrawingMode GetDrawingMode(ulong playerId)
  {
    return NRun.Instance != null ? NRun.Instance.GlobalUi.MapScreen.Drawings.GetDrawingMode(playerId) : DrawingMode.None;
  }

  private NRemoteMouseCursor? GetCursor(ulong playerId)
  {
    return this._cursors.FirstOrDefault<NRemoteMouseCursor>((Func<NRemoteMouseCursor, bool>) (c => (long) c.PlayerId == (long) playerId));
  }

  private void RemoveCursor(ulong playerId)
  {
    NRemoteMouseCursor cursor = this.GetCursor(playerId);
    if (cursor == null)
      return;
    ((Node) cursor).QueueFreeSafely();
    this._cursors.Remove(cursor);
  }

  private void UpdateCursorVisibility()
  {
    NetScreenType screenType1 = this._synchronizer.GetScreenType(this._synchronizer.NetService.NetId);
    foreach (NRemoteMouseCursor cursor in this._cursors)
    {
      NetScreenType screenType2 = this._synchronizer.GetScreenType(cursor.PlayerId);
      bool flag1 = screenType1 == screenType2;
      bool flag2;
      switch (screenType2)
      {
        case NetScreenType.DeckView:
        case NetScreenType.CardPile:
        case NetScreenType.SimpleCardsView:
        case NetScreenType.CardSelection:
        case NetScreenType.RemotePlayerExpandedState:
          flag2 = true;
          break;
        default:
          flag2 = false;
          break;
      }
      bool flag3 = flag2;
      bool flag4 = screenType2 == NetScreenType.SharedRelicPicking;
      ((CanvasItem) cursor).Visible = flag1 && !flag3 && !flag4;
    }
  }

  public override void _Input(InputEvent inputEvent)
  {
    if (this._synchronizer == null)
      return;
    if (inputEvent.IsActionReleased(DebugHotkey.hideMpCursors, false))
    {
      NRemoteMouseCursorContainer._isDebugUiVisible = !NRemoteMouseCursorContainer._isDebugUiVisible;
      this.ApplyDebugUiVisibility();
      ((Node) NGame.Instance).AddChildSafely((Node) NFullscreenTextVfx.Create(NRemoteMouseCursorContainer._isDebugUiVisible ? "Show MP Cursors" : "Hide MP Cursors"));
    }
    switch (inputEvent)
    {
      case InputEventMouseMotion eventMouseMotion:
        this._synchronizer.SyncLocalIsUsingController(false);
        if (((CanvasItem) NGame.Instance.ReactionWheel).Visible)
          break;
        this._synchronizer.SyncLocalMousePos(((InputEventMouse) eventMouseMotion).Position, (Control) this);
        break;
      case InputEventMouseButton eventMouseButton:
        this._synchronizer.SyncLocalIsUsingController(false);
        if (eventMouseButton.ButtonIndex != 1L)
          break;
        this._synchronizer.SyncLocalMouseDown(eventMouseButton.Pressed);
        break;
    }
  }

  private void OnGuiFocusChanged(Control focused)
  {
    if (this._synchronizer == null)
      return;
    NControllerManager instance = NControllerManager.Instance;
    if ((instance != null ? (instance.IsUsingController ? 1 : 0) : 0) == 0)
      return;
    this._synchronizer.SyncLocalIsUsingController(true);
    this._synchronizer.SyncLocalControllerFocus(Vector2.op_Addition(focused.GlobalPosition, Vector2.op_Multiply(focused.Size, 0.5f)), (Control) this);
  }

  private void ApplyDebugUiVisibility()
  {
    ((CanvasItem) this).Visible = NRemoteMouseCursorContainer._isDebugUiVisible;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(17)
    {
      new MethodInfo(NRemoteMouseCursorContainer.MethodName.Deinitialize, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRemoteMouseCursorContainer.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRemoteMouseCursorContainer.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRemoteMouseCursorContainer.MethodName.ForceUpdateAllCursors, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRemoteMouseCursorContainer.MethodName.GetCursorPosition, new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("playerId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NRemoteMouseCursorContainer.MethodName.OnInputStateAdded, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("playerId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NRemoteMouseCursorContainer.MethodName.OnInputStateRemoved, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("playerId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NRemoteMouseCursorContainer.MethodName.AddCursor, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("playerId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NRemoteMouseCursorContainer.MethodName.OnInputStateChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("playerId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NRemoteMouseCursorContainer.MethodName.DrawingCursorStateChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("playerId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NRemoteMouseCursorContainer.MethodName.GetDrawingMode, new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("playerId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NRemoteMouseCursorContainer.MethodName.GetCursor, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("playerId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NRemoteMouseCursorContainer.MethodName.RemoveCursor, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("playerId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NRemoteMouseCursorContainer.MethodName.UpdateCursorVisibility, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRemoteMouseCursorContainer.MethodName._Input, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NRemoteMouseCursorContainer.MethodName.OnGuiFocusChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("focused"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NRemoteMouseCursorContainer.MethodName.ApplyDebugUiVisibility, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NRemoteMouseCursorContainer.MethodName.Deinitialize) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Deinitialize();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRemoteMouseCursorContainer.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRemoteMouseCursorContainer.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRemoteMouseCursorContainer.MethodName.ForceUpdateAllCursors) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ForceUpdateAllCursors();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRemoteMouseCursorContainer.MethodName.GetCursorPosition) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      Vector2 cursorPosition = this.GetCursorPosition(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<Vector2>(ref cursorPosition);
      return true;
    }
    if (StringName.op_Equality(ref method, NRemoteMouseCursorContainer.MethodName.OnInputStateAdded) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnInputStateAdded(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRemoteMouseCursorContainer.MethodName.OnInputStateRemoved) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnInputStateRemoved(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRemoteMouseCursorContainer.MethodName.AddCursor) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.AddCursor(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRemoteMouseCursorContainer.MethodName.OnInputStateChanged) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnInputStateChanged(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRemoteMouseCursorContainer.MethodName.DrawingCursorStateChanged) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.DrawingCursorStateChanged(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRemoteMouseCursorContainer.MethodName.GetDrawingMode) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      DrawingMode drawingMode = NRemoteMouseCursorContainer.GetDrawingMode(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<DrawingMode>(ref drawingMode);
      return true;
    }
    if (StringName.op_Equality(ref method, NRemoteMouseCursorContainer.MethodName.GetCursor) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NRemoteMouseCursor cursor = this.GetCursor(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NRemoteMouseCursor>(ref cursor);
      return true;
    }
    if (StringName.op_Equality(ref method, NRemoteMouseCursorContainer.MethodName.RemoveCursor) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.RemoveCursor(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRemoteMouseCursorContainer.MethodName.UpdateCursorVisibility) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateCursorVisibility();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRemoteMouseCursorContainer.MethodName._Input) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Input(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRemoteMouseCursorContainer.MethodName.OnGuiFocusChanged) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnGuiFocusChanged(VariantUtils.ConvertTo<Control>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NRemoteMouseCursorContainer.MethodName.ApplyDebugUiVisibility) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.ApplyDebugUiVisibility();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NRemoteMouseCursorContainer.MethodName.GetDrawingMode) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      DrawingMode drawingMode = NRemoteMouseCursorContainer.GetDrawingMode(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<DrawingMode>(ref drawingMode);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NRemoteMouseCursorContainer.MethodName.Deinitialize) || StringName.op_Equality(ref method, NRemoteMouseCursorContainer.MethodName._Ready) || StringName.op_Equality(ref method, NRemoteMouseCursorContainer.MethodName._ExitTree) || StringName.op_Equality(ref method, NRemoteMouseCursorContainer.MethodName.ForceUpdateAllCursors) || StringName.op_Equality(ref method, NRemoteMouseCursorContainer.MethodName.GetCursorPosition) || StringName.op_Equality(ref method, NRemoteMouseCursorContainer.MethodName.OnInputStateAdded) || StringName.op_Equality(ref method, NRemoteMouseCursorContainer.MethodName.OnInputStateRemoved) || StringName.op_Equality(ref method, NRemoteMouseCursorContainer.MethodName.AddCursor) || StringName.op_Equality(ref method, NRemoteMouseCursorContainer.MethodName.OnInputStateChanged) || StringName.op_Equality(ref method, NRemoteMouseCursorContainer.MethodName.DrawingCursorStateChanged) || StringName.op_Equality(ref method, NRemoteMouseCursorContainer.MethodName.GetDrawingMode) || StringName.op_Equality(ref method, NRemoteMouseCursorContainer.MethodName.GetCursor) || StringName.op_Equality(ref method, NRemoteMouseCursorContainer.MethodName.RemoveCursor) || StringName.op_Equality(ref method, NRemoteMouseCursorContainer.MethodName.UpdateCursorVisibility) || StringName.op_Equality(ref method, NRemoteMouseCursorContainer.MethodName._Input) || StringName.op_Equality(ref method, NRemoteMouseCursorContainer.MethodName.OnGuiFocusChanged) || StringName.op_Equality(ref method, NRemoteMouseCursorContainer.MethodName.ApplyDebugUiVisibility) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName Deinitialize = StringName.op_Implicit(nameof (Deinitialize));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName ForceUpdateAllCursors = StringName.op_Implicit(nameof (ForceUpdateAllCursors));
    public static readonly StringName GetCursorPosition = StringName.op_Implicit(nameof (GetCursorPosition));
    public static readonly StringName OnInputStateAdded = StringName.op_Implicit(nameof (OnInputStateAdded));
    public static readonly StringName OnInputStateRemoved = StringName.op_Implicit(nameof (OnInputStateRemoved));
    public static readonly StringName AddCursor = StringName.op_Implicit(nameof (AddCursor));
    public static readonly StringName OnInputStateChanged = StringName.op_Implicit(nameof (OnInputStateChanged));
    public static readonly StringName DrawingCursorStateChanged = StringName.op_Implicit(nameof (DrawingCursorStateChanged));
    public static readonly StringName GetDrawingMode = StringName.op_Implicit(nameof (GetDrawingMode));
    public static readonly StringName GetCursor = StringName.op_Implicit(nameof (GetCursor));
    public static readonly StringName RemoveCursor = StringName.op_Implicit(nameof (RemoveCursor));
    public static readonly StringName UpdateCursorVisibility = StringName.op_Implicit(nameof (UpdateCursorVisibility));
    public static readonly StringName _Input = StringName.op_Implicit(nameof (_Input));
    public static readonly StringName OnGuiFocusChanged = StringName.op_Implicit(nameof (OnGuiFocusChanged));
    public static readonly StringName ApplyDebugUiVisibility = StringName.op_Implicit(nameof (ApplyDebugUiVisibility));
  }

  public class PropertyName : Control.PropertyName
  {
  }

  public class SignalName : Control.SignalName
  {
  }
}
