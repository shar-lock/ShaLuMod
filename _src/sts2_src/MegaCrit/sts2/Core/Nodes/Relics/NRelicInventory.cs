// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Relics.NRelicInventory
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Relics;

[ScriptPath("res://src/Core/Nodes/Relics/NRelicInventory.cs")]
public class NRelicInventory : FlowContainer
{
  private Player? _player;
  private readonly List<NRelicInventoryHolder> _relicNodes = new List<NRelicInventoryHolder>();
  private Vector2 _originalPos;
  private Tween? _curTween;
  private Tween? _debugHideTween;
  private bool _isDebugHidden;
  private 
  #nullable disable
  NRelicInventory.RelicsChangedEventHandler backing_RelicsChanged;

  public 
  #nullable enable
  IReadOnlyList<NRelicInventoryHolder> RelicNodes
  {
    get => (IReadOnlyList<NRelicInventoryHolder>) this._relicNodes;
  }

  public override void _Ready()
  {
    this._originalPos = ((Control) this).Position;
    ((GodotObject) this).Connect(Control.SignalName.FocusEntered, Callable.From((Action) (() => this._relicNodes[0].TryGrabFocus())), 0U);
  }

  public override void _EnterTree()
  {
    ((Node) this)._EnterTree();
    this.ConnectPlayerEvents();
  }

  public override void _ExitTree()
  {
    ((Node) this)._ExitTree();
    this.DisconnectPlayerEvents();
  }

  public void Initialize(RunState runState)
  {
    this.DisconnectPlayerEvents();
    this._player = LocalContext.GetMe((IPlayerCollection) runState);
    this.ConnectPlayerEvents();
    foreach (RelicModel relic in (IEnumerable<RelicModel>) this._player.Relics)
      this.Add(relic, true);
    this.UpdateNavigation();
  }

  private void ConnectPlayerEvents()
  {
    if (this._player == null)
      return;
    this._player.RelicObtained += new Action<RelicModel>(this.OnRelicObtained);
    this._player.RelicRemoved += new Action<RelicModel>(this.OnRelicRemoved);
  }

  private void DisconnectPlayerEvents()
  {
    if (this._player == null)
      return;
    this._player.RelicObtained -= new Action<RelicModel>(this.OnRelicObtained);
    this._player.RelicRemoved -= new Action<RelicModel>(this.OnRelicRemoved);
  }

  private void Add(RelicModel relic, bool startsShown, int index = -1)
  {
    NRelicInventoryHolder child = NRelicInventoryHolder.Create(relic);
    child.Inventory = this;
    if (index < 0)
      this._relicNodes.Add(child);
    else
      this._relicNodes.Insert(index, child);
    ((Node) this).AddChildSafely((Node) child);
    ((Node) this).MoveChildSafely((Node) child, index);
    if (!startsShown)
    {
      TextureRect icon = child.Relic.Icon;
      Color modulate = ((CanvasItem) child.Relic.Icon).Modulate;
      modulate.A = 0.0f;
      Color color = modulate;
      ((CanvasItem) icon).Modulate = color;
      this.UpdateNavigation();
    }
    ((GodotObject) child).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>((Action<NButton>) (_ => this.OnRelicClicked(relic))), 0U);
    ((GodotObject) child).Connect(NClickableControl.SignalName.Focused, Callable.From<NClickableControl>((Action<NClickableControl>) (_ => this.OnRelicFocused(relic))), 0U);
    ((GodotObject) child).Connect(NClickableControl.SignalName.Unfocused, Callable.From<NClickableControl>((Action<NClickableControl>) (_ => NRelicInventory.OnRelicUnfocused())), 0U);
    ((GodotObject) this).EmitSignal(NRelicInventory.SignalName.RelicsChanged, Array.Empty<Variant>());
  }

  private void Remove(RelicModel relic)
  {
    if (!LocalContext.IsMine(relic))
      return;
    NRelicInventoryHolder child = this._relicNodes.First<NRelicInventoryHolder>((Func<NRelicInventoryHolder, bool>) (n => n.Relic.Model == relic));
    this._relicNodes.Remove(child);
    ((Node) this).RemoveChildSafely((Node) child);
    this.EmitSignalRelicsChanged();
    this.UpdateNavigation();
  }

  private void OnRelicClicked(RelicModel model)
  {
    List<RelicModel> relics = new List<RelicModel>();
    foreach (NRelicInventoryHolder relicNode in this._relicNodes)
      relics.Add(relicNode.Relic.Model);
    NGame.Instance.GetInspectRelicScreen().Open((IReadOnlyList<RelicModel>) relics, model);
  }

  private void OnRelicFocused(RelicModel model)
  {
    RunManager.Instance.HoveredModelTracker.OnLocalRelicHovered(model);
  }

  private static void OnRelicUnfocused()
  {
    RunManager.Instance.HoveredModelTracker.OnLocalRelicUnhovered();
  }

  public void AnimateRelic(RelicModel relic, Vector2? startPosition = null, Vector2? startScale = null)
  {
    if (!LocalContext.IsMine(relic))
      return;
    TaskHelper.RunSafely(this._relicNodes.First<NRelicInventoryHolder>((Func<NRelicInventoryHolder, bool>) (n => n.Relic.Model == relic)).PlayNewlyAcquiredAnimation(startPosition, startScale));
  }

  private void OnRelicObtained(RelicModel relic)
  {
    this.Add(relic, false, this._player.Relics.IndexOf<RelicModel>(relic));
  }

  private void OnRelicRemoved(RelicModel relic) => this.Remove(relic);

  public void AnimShow()
  {
    ((Control) this).FocusBehaviorRecursive = (Control.FocusBehaviorRecursiveEnum) 2L;
    this._curTween?.Kill();
    this._curTween = ((Node) this).CreateTween();
    this._curTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("global_position:y"), Variant.op_Implicit(this._originalPos.Y), 0.25).SetTrans((Tween.TransitionType) 7L).SetEase((Tween.EaseType) 1L);
  }

  public void AnimHide()
  {
    ((Control) this).FocusBehaviorRecursive = (Control.FocusBehaviorRecursiveEnum) 1L;
    this._curTween?.Kill();
    this._curTween = ((Node) this).CreateTween();
    this._curTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("global_position:y"), Variant.op_Implicit((float) ((double) this._originalPos.Y - 68.0 * (double) this.GetLineCount() - 90.0)), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
  }

  public void ShowImmediately()
  {
    this._curTween?.Kill();
    Vector2 position = ((Control) this).Position;
    position.Y = this._originalPos.Y;
    ((Control) this).Position = position;
  }

  public void HideImmediately()
  {
    this._curTween?.Kill();
    Vector2 position = ((Control) this).Position;
    position.Y = (float) ((double) this._originalPos.Y - 68.0 * (double) this.GetLineCount() - 90.0);
    ((Control) this).Position = position;
  }

  public Vector2 GetDefaultPosition() => this._originalPos;

  public override void _Input(InputEvent inputEvent)
  {
    if (!inputEvent.IsActionReleased(DebugHotkey.hideTopBar, false))
      return;
    this.DebugHideTopBar();
  }

  private void DebugHideTopBar()
  {
    if (this._isDebugHidden)
      this.AnimShow();
    else
      this.AnimHide();
    this._isDebugHidden = !this._isDebugHidden;
  }

  private void UpdateNavigation()
  {
    for (int index = 0; index < this.RelicNodes.Count; ++index)
    {
      NRelicInventoryHolder relicNode = this.RelicNodes[index];
      NRelicInventoryHolder nrelicInventoryHolder = relicNode;
      NodePath path;
      if (index <= 0)
      {
        IReadOnlyList<NRelicInventoryHolder> relicNodes = this.RelicNodes;
        path = ((Node) relicNodes[relicNodes.Count - 1]).GetPath();
      }
      else
        path = ((Node) this.RelicNodes[index - 1]).GetPath();
      nrelicInventoryHolder.FocusNeighborLeft = path;
      relicNode.FocusNeighborRight = index < this.RelicNodes.Count - 1 ? ((Node) this.RelicNodes[index + 1]).GetPath() : ((Node) this.RelicNodes[0]).GetPath();
      Control firstPotionControl = NRun.Instance.GlobalUi.TopBar.PotionContainer.FirstPotionControl;
      relicNode.FocusNeighborTop = firstPotionControl == null || !GodotObject.IsInstanceValid((GodotObject) firstPotionControl) ? ((Node) relicNode).GetPath() : ((Node) firstPotionControl).GetPath();
      NMultiplayerPlayerStateContainer multiplayerPlayerContainer = NRun.Instance.GlobalUi.MultiplayerPlayerContainer;
      if (((Node) multiplayerPlayerContainer).GetChildCount(false) > 0)
      {
        Control hitbox = (Control) multiplayerPlayerContainer.FirstPlayerState?.Hitbox;
        relicNode.FocusNeighborBottom = hitbox == null || !GodotObject.IsInstanceValid((GodotObject) hitbox) ? ((Node) relicNode).GetPath() : ((Node) hitbox).GetPath();
      }
      else
        relicNode.FocusNeighborBottom = ((Node) NRun.Instance.GlobalUi.TopBar.ActiveScreenProxy).GetPath();
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(14)
    {
      new MethodInfo(NRelicInventory.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRelicInventory.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRelicInventory.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRelicInventory.MethodName.ConnectPlayerEvents, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRelicInventory.MethodName.DisconnectPlayerEvents, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRelicInventory.MethodName.OnRelicUnfocused, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRelicInventory.MethodName.AnimShow, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRelicInventory.MethodName.AnimHide, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRelicInventory.MethodName.ShowImmediately, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRelicInventory.MethodName.HideImmediately, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRelicInventory.MethodName.GetDefaultPosition, new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRelicInventory.MethodName._Input, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NRelicInventory.MethodName.DebugHideTopBar, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRelicInventory.MethodName.UpdateNavigation, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NRelicInventory.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRelicInventory.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRelicInventory.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRelicInventory.MethodName.ConnectPlayerEvents) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ConnectPlayerEvents();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRelicInventory.MethodName.DisconnectPlayerEvents) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DisconnectPlayerEvents();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRelicInventory.MethodName.OnRelicUnfocused) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NRelicInventory.OnRelicUnfocused();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRelicInventory.MethodName.AnimShow) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimShow();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRelicInventory.MethodName.AnimHide) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimHide();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRelicInventory.MethodName.ShowImmediately) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ShowImmediately();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRelicInventory.MethodName.HideImmediately) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.HideImmediately();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRelicInventory.MethodName.GetDefaultPosition) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      Vector2 defaultPosition = this.GetDefaultPosition();
      ret = VariantUtils.CreateFrom<Vector2>(ref defaultPosition);
      return true;
    }
    if (StringName.op_Equality(ref method, NRelicInventory.MethodName._Input) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Input(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRelicInventory.MethodName.DebugHideTopBar) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DebugHideTopBar();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NRelicInventory.MethodName.UpdateNavigation) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.UpdateNavigation();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NRelicInventory.MethodName.OnRelicUnfocused) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NRelicInventory.OnRelicUnfocused();
      ret = new godot_variant();
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NRelicInventory.MethodName._Ready) || StringName.op_Equality(ref method, NRelicInventory.MethodName._EnterTree) || StringName.op_Equality(ref method, NRelicInventory.MethodName._ExitTree) || StringName.op_Equality(ref method, NRelicInventory.MethodName.ConnectPlayerEvents) || StringName.op_Equality(ref method, NRelicInventory.MethodName.DisconnectPlayerEvents) || StringName.op_Equality(ref method, NRelicInventory.MethodName.OnRelicUnfocused) || StringName.op_Equality(ref method, NRelicInventory.MethodName.AnimShow) || StringName.op_Equality(ref method, NRelicInventory.MethodName.AnimHide) || StringName.op_Equality(ref method, NRelicInventory.MethodName.ShowImmediately) || StringName.op_Equality(ref method, NRelicInventory.MethodName.HideImmediately) || StringName.op_Equality(ref method, NRelicInventory.MethodName.GetDefaultPosition) || StringName.op_Equality(ref method, NRelicInventory.MethodName._Input) || StringName.op_Equality(ref method, NRelicInventory.MethodName.DebugHideTopBar) || StringName.op_Equality(ref method, NRelicInventory.MethodName.UpdateNavigation) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRelicInventory.PropertyName._originalPos))
    {
      this._originalPos = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicInventory.PropertyName._curTween))
    {
      this._curTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicInventory.PropertyName._debugHideTween))
    {
      this._debugHideTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRelicInventory.PropertyName._isDebugHidden))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._isDebugHidden = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRelicInventory.PropertyName._originalPos))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._originalPos);
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicInventory.PropertyName._curTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._curTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicInventory.PropertyName._debugHideTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._debugHideTween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRelicInventory.PropertyName._isDebugHidden))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._isDebugHidden);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 5L, NRelicInventory.PropertyName._originalPos, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRelicInventory.PropertyName._curTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRelicInventory.PropertyName._debugHideTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NRelicInventory.PropertyName._isDebugHidden, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NRelicInventory.PropertyName._originalPos, Variant.From<Vector2>(ref this._originalPos));
    info.AddProperty(NRelicInventory.PropertyName._curTween, Variant.From<Tween>(ref this._curTween));
    info.AddProperty(NRelicInventory.PropertyName._debugHideTween, Variant.From<Tween>(ref this._debugHideTween));
    info.AddProperty(NRelicInventory.PropertyName._isDebugHidden, Variant.From<bool>(ref this._isDebugHidden));
    info.AddSignalEventDelegate(NRelicInventory.SignalName.RelicsChanged, (Delegate) this.backing_RelicsChanged);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NRelicInventory.PropertyName._originalPos, ref variant1))
      this._originalPos = ((Variant) ref variant1).As<Vector2>();
    Variant variant2;
    if (info.TryGetProperty(NRelicInventory.PropertyName._curTween, ref variant2))
      this._curTween = ((Variant) ref variant2).As<Tween>();
    Variant variant3;
    if (info.TryGetProperty(NRelicInventory.PropertyName._debugHideTween, ref variant3))
      this._debugHideTween = ((Variant) ref variant3).As<Tween>();
    Variant variant4;
    if (info.TryGetProperty(NRelicInventory.PropertyName._isDebugHidden, ref variant4))
      this._isDebugHidden = ((Variant) ref variant4).As<bool>();
    NRelicInventory.RelicsChangedEventHandler changedEventHandler;
    if (!info.TryGetSignalEventDelegate<NRelicInventory.RelicsChangedEventHandler>(NRelicInventory.SignalName.RelicsChanged, ref changedEventHandler))
      return;
    this.backing_RelicsChanged = changedEventHandler;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<MethodInfo> GetGodotSignalList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NRelicInventory.SignalName.RelicsChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  public event NRelicInventory.RelicsChangedEventHandler RelicsChanged
  {
    add => this.backing_RelicsChanged += value;
    remove => this.backing_RelicsChanged -= value;
  }

  protected void EmitSignalRelicsChanged()
  {
    ((GodotObject) this).EmitSignal(NRelicInventory.SignalName.RelicsChanged, Array.Empty<Variant>());
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RaiseGodotClassSignalCallbacks(
    in godot_string_name signal,
    NativeVariantPtrArgs args)
  {
    if (StringName.op_Equality(ref signal, NRelicInventory.SignalName.RelicsChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NRelicInventory.RelicsChangedEventHandler backingRelicsChanged = this.backing_RelicsChanged;
      if (backingRelicsChanged == null)
        return;
      backingRelicsChanged();
    }
    else
      ((GodotObject) this).RaiseGodotClassSignalCallbacks(ref signal, args);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassSignal(in godot_string_name signal)
  {
    return StringName.op_Equality(ref signal, NRelicInventory.SignalName.RelicsChanged) || base.HasGodotClassSignal(ref signal);
  }

  [Signal]
  public delegate void RelicsChangedEventHandler();

  public class MethodName : FlowContainer.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName ConnectPlayerEvents = StringName.op_Implicit(nameof (ConnectPlayerEvents));
    public static readonly StringName DisconnectPlayerEvents = StringName.op_Implicit(nameof (DisconnectPlayerEvents));
    public static readonly StringName OnRelicUnfocused = StringName.op_Implicit(nameof (OnRelicUnfocused));
    public static readonly StringName AnimShow = StringName.op_Implicit(nameof (AnimShow));
    public static readonly StringName AnimHide = StringName.op_Implicit(nameof (AnimHide));
    public static readonly StringName ShowImmediately = StringName.op_Implicit(nameof (ShowImmediately));
    public static readonly StringName HideImmediately = StringName.op_Implicit(nameof (HideImmediately));
    public static readonly StringName GetDefaultPosition = StringName.op_Implicit(nameof (GetDefaultPosition));
    public static readonly StringName _Input = StringName.op_Implicit(nameof (_Input));
    public static readonly StringName DebugHideTopBar = StringName.op_Implicit(nameof (DebugHideTopBar));
    public static readonly StringName UpdateNavigation = StringName.op_Implicit(nameof (UpdateNavigation));
  }

  public class PropertyName : FlowContainer.PropertyName
  {
    public static readonly StringName _originalPos = StringName.op_Implicit(nameof (_originalPos));
    public static readonly StringName _curTween = StringName.op_Implicit(nameof (_curTween));
    public static readonly StringName _debugHideTween = StringName.op_Implicit(nameof (_debugHideTween));
    public static readonly StringName _isDebugHidden = StringName.op_Implicit(nameof (_isDebugHidden));
  }

  public class SignalName : FlowContainer.SignalName
  {
    public static readonly StringName RelicsChanged = StringName.op_Implicit(nameof (RelicsChanged));
  }
}
