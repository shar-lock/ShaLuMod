// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Combat.NTargetManager
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.RestSite;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Combat;

[ScriptPath("res://src/Core/Nodes/Combat/NTargetManager.cs")]
public class NTargetManager : Node2D
{
  private NTargetingArrow _targetingArrow;
  private TaskCompletionSource<Node?>? _completionSource;
  private Func<bool>? _exitEarlyCondition;
  private Func<Node, bool>? _nodeFilter;
  private TargetMode _targetMode;
  private TargetType _validTargetsType;
  private 
  #nullable disable
  NTargetManager.CreatureHoveredEventHandler backing_CreatureHovered;
  private NTargetManager.CreatureUnhoveredEventHandler backing_CreatureUnhovered;
  private NTargetManager.NodeHoveredEventHandler backing_NodeHovered;
  private NTargetManager.NodeUnhoveredEventHandler backing_NodeUnhovered;
  private NTargetManager.TargetingBeganEventHandler backing_TargetingBegan;
  private NTargetManager.TargetingEndedEventHandler backing_TargetingEnded;

  public static 
  #nullable enable
  NTargetManager Instance => NRun.Instance.GlobalUi.TargetManager;

  public bool IsInSelection => this._targetMode != 0;

  private Node? HoveredNode { get; set; }

  public long LastTargetingFinishedFrame { get; set; }

  public override void _Ready()
  {
    this._targetingArrow = ((Node) this).GetNode<NTargetingArrow>(NodePath.op_Implicit("TargetingArrow"));
  }

  public override void _EnterTree()
  {
    ((Node) this)._EnterTree();
    if (NControllerManager.Instance != null)
    {
      ((GodotObject) NControllerManager.Instance).Connect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.CancelTargeting)), 0U);
      ((GodotObject) NControllerManager.Instance).Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.CancelTargeting)), 0U);
    }
    CombatManager.Instance.CombatEnded += new Action<CombatRoom>(this.OnCombatEnded);
  }

  public override void _ExitTree()
  {
    ((Node) this)._ExitTree();
    if (NControllerManager.Instance != null)
    {
      ((GodotObject) NControllerManager.Instance).Disconnect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.CancelTargeting)));
      ((GodotObject) NControllerManager.Instance).Disconnect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.CancelTargeting)));
    }
    CombatManager.Instance.CombatEnded -= new Action<CombatRoom>(this.OnCombatEnded);
  }

  public override void _Input(InputEvent inputEvent)
  {
    if (!this.IsInSelection)
      return;
    bool flag = false;
    bool cancel = false;
    if (inputEvent is InputEventMouseButton eventMouseButton)
    {
      MouseButton buttonIndex = eventMouseButton.ButtonIndex;
      if (buttonIndex != 1L)
      {
        if (buttonIndex == 2L)
        {
          flag = ((InputEvent) eventMouseButton).IsPressed();
          cancel = true;
        }
      }
      else if (((InputEvent) eventMouseButton).IsReleased())
      {
        switch (this._targetMode)
        {
          case TargetMode.ReleaseMouseToTarget:
            if (this.HoveredNode != null)
            {
              flag = true;
              break;
            }
            this._targetMode = TargetMode.ClickMouseToTarget;
            break;
          case TargetMode.ClickMouseToTarget:
            flag = true;
            break;
        }
      }
    }
    else if (inputEvent.IsActionPressed(MegaInput.select, false, false) && this.HoveredNode != null)
    {
      flag = true;
      cancel = false;
      ((Node) this).GetViewport().SetInputAsHandled();
    }
    else if (inputEvent.IsActionPressed(MegaInput.cancel, false, false) || inputEvent.IsActionPressed(MegaInput.topPanel, false, false))
    {
      flag = true;
      cancel = true;
      if (inputEvent.IsActionPressed(MegaInput.cancel, false, false))
        ((Node) this).GetViewport().SetInputAsHandled();
    }
    if (this._exitEarlyCondition != null && this._exitEarlyCondition())
    {
      flag = true;
      cancel = true;
    }
    if (!flag)
      return;
    this.FinishTargeting(cancel);
  }

  public override void _Process(double delta)
  {
    if (this._exitEarlyCondition != null && this._exitEarlyCondition())
      this.FinishTargeting(true);
    if (!(this.HoveredNode is NCreature hoveredNode))
      return;
    Creature entity = hoveredNode.Entity;
    if (entity == null || entity.IsHittable)
      return;
    this.FinishTargeting(true);
  }

  private void OnCombatEnded(CombatRoom _)
  {
    if (this._exitEarlyCondition == null)
      return;
    this.FinishTargeting(true);
  }

  public void CancelTargeting()
  {
    if (this._targetMode == TargetMode.None)
      return;
    this.FinishTargeting(true);
  }

  private void FinishTargeting(bool cancel)
  {
    NHoverTipSet.shouldBlockHoverTips = false;
    this._exitEarlyCondition = (Func<bool>) null;
    this._completionSource.SetResult(cancel ? (Node) null : this.HoveredNode);
    this.LastTargetingFinishedFrame = ((Node) this).GetTree().GetFrame();
    ((GodotObject) this).EmitSignal(NTargetManager.SignalName.TargetingEnded, Array.Empty<Variant>());
    this._targetMode = TargetMode.None;
    this._targetingArrow.StopDrawing();
    if (this.HoveredNode is NCreature hoveredNode2)
      hoveredNode2.HideMultiselectReticle();
    else if (this.HoveredNode is NRestSiteCharacter hoveredNode1)
      hoveredNode1.Deselect();
    this.HoveredNode = (Node) null;
    RunManager.Instance.InputSynchronizer.SyncLocalIsTargeting(false);
  }

  public async Task<Node?> SelectionFinished() => await this._completionSource.Task;

  public void StartTargeting(
    TargetType validTargetsType,
    Vector2 startPosition,
    TargetMode startingMode,
    Func<bool>? exitEarlyCondition,
    Func<Node, bool>? nodeFilter)
  {
    this._validTargetsType = validTargetsType.IsSingleTarget() ? validTargetsType : throw new InvalidOperationException($"Tried to begin targeting with invalid ActionTarget {validTargetsType}!");
    this._targetingArrow.StartDrawingFrom(startPosition, startingMode == TargetMode.Controller);
    this._completionSource = new TaskCompletionSource<Node>();
    this._exitEarlyCondition = exitEarlyCondition;
    this._nodeFilter = nodeFilter;
    this._targetMode = startingMode;
    NHoverTipSet.shouldBlockHoverTips = true;
    ((GodotObject) this).EmitSignal(NTargetManager.SignalName.TargetingBegan, Array.Empty<Variant>());
    RunManager.Instance.InputSynchronizer.SyncLocalIsTargeting(true);
    NCombatRoom instance = NCombatRoom.Instance;
    foreach (NCreature ncreature in (IEnumerable<NCreature>) ((instance != null ? (object) instance.CreatureNodes : (object) null) ?? (object) Array.Empty<NCreature>()))
      ncreature.OnTargetingStarted();
  }

  public void StartTargeting(
    TargetType validTargetsType,
    Control control,
    TargetMode startingMode,
    Func<bool>? exitEarlyCondition,
    Func<Node, bool>? nodeFilter)
  {
    this._validTargetsType = validTargetsType.IsSingleTarget() ? validTargetsType : throw new InvalidOperationException($"Tried to begin targeting with invalid ActionTarget {validTargetsType}!");
    this._targetingArrow.StartDrawingFrom(control, startingMode == TargetMode.Controller);
    this._completionSource = new TaskCompletionSource<Node>();
    this._exitEarlyCondition = exitEarlyCondition;
    this._nodeFilter = nodeFilter;
    this._targetMode = startingMode;
    NHoverTipSet.shouldBlockHoverTips = true;
    ((GodotObject) this).EmitSignal(NTargetManager.SignalName.TargetingBegan, Array.Empty<Variant>());
    RunManager.Instance.InputSynchronizer.SyncLocalIsTargeting(true);
    NCombatRoom instance = NCombatRoom.Instance;
    foreach (NCreature ncreature in (IEnumerable<NCreature>) ((instance != null ? (object) instance.CreatureNodes : (object) null) ?? (object) Array.Empty<NCreature>()))
      ncreature.OnTargetingStarted();
  }

  public bool AllowedToTargetNode(Node node)
  {
    if (this._nodeFilter != null && !this._nodeFilter(node))
      return false;
    switch (node)
    {
      case NCreature ncreature:
        return this.AllowedToTargetCreature(ncreature.Entity);
      case NMultiplayerPlayerState nmultiplayerPlayerState:
        return this.AllowedToTargetCreature(nmultiplayerPlayerState.Player.Creature);
      default:
        return true;
    }
  }

  private bool AllowedToTargetCreature(Creature creature)
  {
    switch (this._validTargetsType)
    {
      case TargetType.AnyEnemy:
        if (creature.Side != CombatSide.Enemy || creature.IsDead)
          return false;
        break;
      case TargetType.AnyPlayer:
        if (!creature.IsPlayer || creature.IsDead)
          return false;
        break;
      case TargetType.AnyAlly:
        if (!creature.IsPlayer || creature.IsDead || LocalContext.IsMe(creature.Player))
          return false;
        break;
      default:
        throw new ArgumentOutOfRangeException("_validTargetsType", (object) this._validTargetsType, (string) null);
    }
    return true;
  }

  public void OnNodeHovered(Node node)
  {
    if (!this.IsInSelection || !this.AllowedToTargetNode(node))
      return;
    if (node is NCreature creature)
    {
      this.OnCreatureHovered(creature);
    }
    else
    {
      this.HoveredNode = node;
      this._targetingArrow.SetHighlightingOn(false);
      if (this._targetMode == TargetMode.Controller)
      {
        switch (node)
        {
          case NMultiplayerPlayerState nmultiplayerPlayerState:
            this._targetingArrow.UpdateDrawingTo(Vector2.op_Addition(Vector2.op_Addition(nmultiplayerPlayerState.GlobalPosition, Vector2.op_Multiply(Vector2.Right, nmultiplayerPlayerState.Hitbox.Size.X)), Vector2.op_Division(Vector2.op_Multiply(Vector2.Down, nmultiplayerPlayerState.Hitbox.Size.Y), 2f)));
            break;
          case Control control:
            this._targetingArrow.UpdateDrawingTo(Vector2.op_Addition(control.GlobalPosition, control.PivotOffset));
            break;
          case Node2D node2D:
            this._targetingArrow.UpdateDrawingTo(node2D.GlobalPosition);
            break;
        }
      }
      ((GodotObject) this).EmitSignal(NTargetManager.SignalName.NodeHovered, new Variant[1]
      {
        Variant.op_Implicit((GodotObject) node)
      });
    }
  }

  public void OnNodeUnhovered(Node node)
  {
    if (!this.IsInSelection || !this.AllowedToTargetNode(node))
      return;
    if (node is NCreature creature)
    {
      this.OnCreatureUnhovered(creature);
    }
    else
    {
      this.HoveredNode = (Node) null;
      this._targetingArrow.SetHighlightingOff();
      ((GodotObject) this).EmitSignal(NTargetManager.SignalName.NodeUnhovered, new Variant[1]
      {
        Variant.op_Implicit((GodotObject) node)
      });
    }
  }

  private void OnCreatureHovered(NCreature creature)
  {
    AbstractModel preventer;
    if (Hook.ShouldAllowTargeting(creature.Entity.CombatState, creature.Entity, out preventer))
    {
      this.HoveredNode = (Node) creature;
      this._targetingArrow.SetHighlightingOn(creature.Entity.IsEnemy);
      creature.ShowSingleSelectReticle();
      ((GodotObject) this).EmitSignal(NTargetManager.SignalName.CreatureHovered, new Variant[1]
      {
        Variant.op_Implicit((GodotObject) creature)
      });
      if (this._targetMode != TargetMode.Controller)
        return;
      this._targetingArrow.UpdateDrawingTo(creature.VfxSpawnPosition);
    }
    else
      TaskHelper.RunSafely(preventer.AfterTargetingBlockedVfx(creature.Entity));
  }

  private void OnCreatureUnhovered(NCreature creature)
  {
    ((GodotObject) this).EmitSignal(NTargetManager.SignalName.CreatureUnhovered, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) creature)
    });
    if (this.HoveredNode == creature)
      this.HoveredNode = (Node) null;
    this._targetingArrow.SetHighlightingOff();
    if (this._targetMode == TargetMode.None)
      return;
    creature.HideSingleSelectReticle();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(12)
    {
      new MethodInfo(NTargetManager.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTargetManager.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTargetManager.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTargetManager.MethodName._Input, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NTargetManager.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NTargetManager.MethodName.CancelTargeting, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTargetManager.MethodName.FinishTargeting, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("cancel"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NTargetManager.MethodName.AllowedToTargetNode, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("node"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node"), false)
      }, (List<Variant>) null),
      new MethodInfo(NTargetManager.MethodName.OnNodeHovered, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("node"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node"), false)
      }, (List<Variant>) null),
      new MethodInfo(NTargetManager.MethodName.OnNodeUnhovered, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("node"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node"), false)
      }, (List<Variant>) null),
      new MethodInfo(NTargetManager.MethodName.OnCreatureHovered, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("creature"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NTargetManager.MethodName.OnCreatureUnhovered, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("creature"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NTargetManager.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTargetManager.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTargetManager.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTargetManager.MethodName._Input) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Input(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTargetManager.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTargetManager.MethodName.CancelTargeting) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.CancelTargeting();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTargetManager.MethodName.FinishTargeting) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.FinishTargeting(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTargetManager.MethodName.AllowedToTargetNode) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      bool targetNode = this.AllowedToTargetNode(VariantUtils.ConvertTo<Node>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<bool>(ref targetNode);
      return true;
    }
    if (StringName.op_Equality(ref method, NTargetManager.MethodName.OnNodeHovered) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnNodeHovered(VariantUtils.ConvertTo<Node>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTargetManager.MethodName.OnNodeUnhovered) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnNodeUnhovered(VariantUtils.ConvertTo<Node>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTargetManager.MethodName.OnCreatureHovered) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnCreatureHovered(VariantUtils.ConvertTo<NCreature>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NTargetManager.MethodName.OnCreatureUnhovered) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnCreatureUnhovered(VariantUtils.ConvertTo<NCreature>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NTargetManager.MethodName._Ready) || StringName.op_Equality(ref method, NTargetManager.MethodName._EnterTree) || StringName.op_Equality(ref method, NTargetManager.MethodName._ExitTree) || StringName.op_Equality(ref method, NTargetManager.MethodName._Input) || StringName.op_Equality(ref method, NTargetManager.MethodName._Process) || StringName.op_Equality(ref method, NTargetManager.MethodName.CancelTargeting) || StringName.op_Equality(ref method, NTargetManager.MethodName.FinishTargeting) || StringName.op_Equality(ref method, NTargetManager.MethodName.AllowedToTargetNode) || StringName.op_Equality(ref method, NTargetManager.MethodName.OnNodeHovered) || StringName.op_Equality(ref method, NTargetManager.MethodName.OnNodeUnhovered) || StringName.op_Equality(ref method, NTargetManager.MethodName.OnCreatureHovered) || StringName.op_Equality(ref method, NTargetManager.MethodName.OnCreatureUnhovered) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NTargetManager.PropertyName.HoveredNode))
    {
      this.HoveredNode = VariantUtils.ConvertTo<Node>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTargetManager.PropertyName.LastTargetingFinishedFrame))
    {
      this.LastTargetingFinishedFrame = VariantUtils.ConvertTo<long>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTargetManager.PropertyName._targetingArrow))
    {
      this._targetingArrow = VariantUtils.ConvertTo<NTargetingArrow>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTargetManager.PropertyName._targetMode))
    {
      this._targetMode = VariantUtils.ConvertTo<TargetMode>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NTargetManager.PropertyName._validTargetsType))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._validTargetsType = VariantUtils.ConvertTo<TargetType>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NTargetManager.PropertyName.IsInSelection))
    {
      ref godot_variant local = ref value;
      bool isInSelection = this.IsInSelection;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isInSelection);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NTargetManager.PropertyName.HoveredNode))
    {
      ref godot_variant local = ref value;
      Node hoveredNode = this.HoveredNode;
      godot_variant from = VariantUtils.CreateFrom<Node>(ref hoveredNode);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NTargetManager.PropertyName.LastTargetingFinishedFrame))
    {
      ref godot_variant local = ref value;
      long targetingFinishedFrame = this.LastTargetingFinishedFrame;
      godot_variant from = VariantUtils.CreateFrom<long>(ref targetingFinishedFrame);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NTargetManager.PropertyName._targetingArrow))
    {
      value = VariantUtils.CreateFrom<NTargetingArrow>(ref this._targetingArrow);
      return true;
    }
    if (StringName.op_Equality(ref name, NTargetManager.PropertyName._targetMode))
    {
      value = VariantUtils.CreateFrom<TargetMode>(ref this._targetMode);
      return true;
    }
    if (!StringName.op_Equality(ref name, NTargetManager.PropertyName._validTargetsType))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<TargetType>(ref this._validTargetsType);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NTargetManager.PropertyName._targetingArrow, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NTargetManager.PropertyName.IsInSelection, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NTargetManager.PropertyName._targetMode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NTargetManager.PropertyName._validTargetsType, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTargetManager.PropertyName.HoveredNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NTargetManager.PropertyName.LastTargetingFinishedFrame, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo1 = info;
    StringName hoveredNode1 = NTargetManager.PropertyName.HoveredNode;
    Node hoveredNode2 = this.HoveredNode;
    Variant variant1 = Variant.From<Node>(ref hoveredNode2);
    serializationInfo1.AddProperty(hoveredNode1, variant1);
    GodotSerializationInfo serializationInfo2 = info;
    StringName targetingFinishedFrame1 = NTargetManager.PropertyName.LastTargetingFinishedFrame;
    long targetingFinishedFrame2 = this.LastTargetingFinishedFrame;
    Variant variant2 = Variant.From<long>(ref targetingFinishedFrame2);
    serializationInfo2.AddProperty(targetingFinishedFrame1, variant2);
    info.AddProperty(NTargetManager.PropertyName._targetingArrow, Variant.From<NTargetingArrow>(ref this._targetingArrow));
    info.AddProperty(NTargetManager.PropertyName._targetMode, Variant.From<TargetMode>(ref this._targetMode));
    info.AddProperty(NTargetManager.PropertyName._validTargetsType, Variant.From<TargetType>(ref this._validTargetsType));
    info.AddSignalEventDelegate(NTargetManager.SignalName.CreatureHovered, (Delegate) this.backing_CreatureHovered);
    info.AddSignalEventDelegate(NTargetManager.SignalName.CreatureUnhovered, (Delegate) this.backing_CreatureUnhovered);
    info.AddSignalEventDelegate(NTargetManager.SignalName.NodeHovered, (Delegate) this.backing_NodeHovered);
    info.AddSignalEventDelegate(NTargetManager.SignalName.NodeUnhovered, (Delegate) this.backing_NodeUnhovered);
    info.AddSignalEventDelegate(NTargetManager.SignalName.TargetingBegan, (Delegate) this.backing_TargetingBegan);
    info.AddSignalEventDelegate(NTargetManager.SignalName.TargetingEnded, (Delegate) this.backing_TargetingEnded);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NTargetManager.PropertyName.HoveredNode, ref variant1))
      this.HoveredNode = ((Variant) ref variant1).As<Node>();
    Variant variant2;
    if (info.TryGetProperty(NTargetManager.PropertyName.LastTargetingFinishedFrame, ref variant2))
      this.LastTargetingFinishedFrame = ((Variant) ref variant2).As<long>();
    Variant variant3;
    if (info.TryGetProperty(NTargetManager.PropertyName._targetingArrow, ref variant3))
      this._targetingArrow = ((Variant) ref variant3).As<NTargetingArrow>();
    Variant variant4;
    if (info.TryGetProperty(NTargetManager.PropertyName._targetMode, ref variant4))
      this._targetMode = ((Variant) ref variant4).As<TargetMode>();
    Variant variant5;
    if (info.TryGetProperty(NTargetManager.PropertyName._validTargetsType, ref variant5))
      this._validTargetsType = ((Variant) ref variant5).As<TargetType>();
    NTargetManager.CreatureHoveredEventHandler hoveredEventHandler1;
    if (info.TryGetSignalEventDelegate<NTargetManager.CreatureHoveredEventHandler>(NTargetManager.SignalName.CreatureHovered, ref hoveredEventHandler1))
      this.backing_CreatureHovered = hoveredEventHandler1;
    NTargetManager.CreatureUnhoveredEventHandler unhoveredEventHandler1;
    if (info.TryGetSignalEventDelegate<NTargetManager.CreatureUnhoveredEventHandler>(NTargetManager.SignalName.CreatureUnhovered, ref unhoveredEventHandler1))
      this.backing_CreatureUnhovered = unhoveredEventHandler1;
    NTargetManager.NodeHoveredEventHandler hoveredEventHandler2;
    if (info.TryGetSignalEventDelegate<NTargetManager.NodeHoveredEventHandler>(NTargetManager.SignalName.NodeHovered, ref hoveredEventHandler2))
      this.backing_NodeHovered = hoveredEventHandler2;
    NTargetManager.NodeUnhoveredEventHandler unhoveredEventHandler2;
    if (info.TryGetSignalEventDelegate<NTargetManager.NodeUnhoveredEventHandler>(NTargetManager.SignalName.NodeUnhovered, ref unhoveredEventHandler2))
      this.backing_NodeUnhovered = unhoveredEventHandler2;
    NTargetManager.TargetingBeganEventHandler beganEventHandler;
    if (info.TryGetSignalEventDelegate<NTargetManager.TargetingBeganEventHandler>(NTargetManager.SignalName.TargetingBegan, ref beganEventHandler))
      this.backing_TargetingBegan = beganEventHandler;
    NTargetManager.TargetingEndedEventHandler endedEventHandler;
    if (!info.TryGetSignalEventDelegate<NTargetManager.TargetingEndedEventHandler>(NTargetManager.SignalName.TargetingEnded, ref endedEventHandler))
      return;
    this.backing_TargetingEnded = endedEventHandler;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<MethodInfo> GetGodotSignalList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NTargetManager.SignalName.CreatureHovered, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("creature"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NTargetManager.SignalName.CreatureUnhovered, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("creature"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NTargetManager.SignalName.NodeHovered, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("node"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node"), false)
      }, (List<Variant>) null),
      new MethodInfo(NTargetManager.SignalName.NodeUnhovered, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("node"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node"), false)
      }, (List<Variant>) null),
      new MethodInfo(NTargetManager.SignalName.TargetingBegan, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTargetManager.SignalName.TargetingEnded, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  public event NTargetManager.CreatureHoveredEventHandler CreatureHovered
  {
    add => this.backing_CreatureHovered += value;
    remove => this.backing_CreatureHovered -= value;
  }

  protected void EmitSignalCreatureHovered(NCreature creature)
  {
    ((GodotObject) this).EmitSignal(NTargetManager.SignalName.CreatureHovered, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) creature)
    });
  }

  public event NTargetManager.CreatureUnhoveredEventHandler CreatureUnhovered
  {
    add => this.backing_CreatureUnhovered += value;
    remove => this.backing_CreatureUnhovered -= value;
  }

  protected void EmitSignalCreatureUnhovered(NCreature creature)
  {
    ((GodotObject) this).EmitSignal(NTargetManager.SignalName.CreatureUnhovered, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) creature)
    });
  }

  public event NTargetManager.NodeHoveredEventHandler NodeHovered
  {
    add => this.backing_NodeHovered += value;
    remove => this.backing_NodeHovered -= value;
  }

  protected void EmitSignalNodeHovered(Node node)
  {
    ((GodotObject) this).EmitSignal(NTargetManager.SignalName.NodeHovered, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) node)
    });
  }

  public event NTargetManager.NodeUnhoveredEventHandler NodeUnhovered
  {
    add => this.backing_NodeUnhovered += value;
    remove => this.backing_NodeUnhovered -= value;
  }

  protected void EmitSignalNodeUnhovered(Node node)
  {
    ((GodotObject) this).EmitSignal(NTargetManager.SignalName.NodeUnhovered, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) node)
    });
  }

  public event NTargetManager.TargetingBeganEventHandler TargetingBegan
  {
    add => this.backing_TargetingBegan += value;
    remove => this.backing_TargetingBegan -= value;
  }

  protected void EmitSignalTargetingBegan()
  {
    ((GodotObject) this).EmitSignal(NTargetManager.SignalName.TargetingBegan, Array.Empty<Variant>());
  }

  public event NTargetManager.TargetingEndedEventHandler TargetingEnded
  {
    add => this.backing_TargetingEnded += value;
    remove => this.backing_TargetingEnded -= value;
  }

  protected void EmitSignalTargetingEnded()
  {
    ((GodotObject) this).EmitSignal(NTargetManager.SignalName.TargetingEnded, Array.Empty<Variant>());
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RaiseGodotClassSignalCallbacks(
    in godot_string_name signal,
    NativeVariantPtrArgs args)
  {
    if (StringName.op_Equality(ref signal, NTargetManager.SignalName.CreatureHovered) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NTargetManager.CreatureHoveredEventHandler backingCreatureHovered = this.backing_CreatureHovered;
      if (backingCreatureHovered == null)
        return;
      backingCreatureHovered(VariantUtils.ConvertTo<NCreature>(ref ((NativeVariantPtrArgs) ref args)[0]));
    }
    else if (StringName.op_Equality(ref signal, NTargetManager.SignalName.CreatureUnhovered) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NTargetManager.CreatureUnhoveredEventHandler creatureUnhovered = this.backing_CreatureUnhovered;
      if (creatureUnhovered == null)
        return;
      creatureUnhovered(VariantUtils.ConvertTo<NCreature>(ref ((NativeVariantPtrArgs) ref args)[0]));
    }
    else if (StringName.op_Equality(ref signal, NTargetManager.SignalName.NodeHovered) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NTargetManager.NodeHoveredEventHandler backingNodeHovered = this.backing_NodeHovered;
      if (backingNodeHovered == null)
        return;
      backingNodeHovered(VariantUtils.ConvertTo<Node>(ref ((NativeVariantPtrArgs) ref args)[0]));
    }
    else if (StringName.op_Equality(ref signal, NTargetManager.SignalName.NodeUnhovered) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NTargetManager.NodeUnhoveredEventHandler backingNodeUnhovered = this.backing_NodeUnhovered;
      if (backingNodeUnhovered == null)
        return;
      backingNodeUnhovered(VariantUtils.ConvertTo<Node>(ref ((NativeVariantPtrArgs) ref args)[0]));
    }
    else if (StringName.op_Equality(ref signal, NTargetManager.SignalName.TargetingBegan) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NTargetManager.TargetingBeganEventHandler backingTargetingBegan = this.backing_TargetingBegan;
      if (backingTargetingBegan == null)
        return;
      backingTargetingBegan();
    }
    else if (StringName.op_Equality(ref signal, NTargetManager.SignalName.TargetingEnded) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NTargetManager.TargetingEndedEventHandler backingTargetingEnded = this.backing_TargetingEnded;
      if (backingTargetingEnded == null)
        return;
      backingTargetingEnded();
    }
    else
      ((GodotObject) this).RaiseGodotClassSignalCallbacks(ref signal, args);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassSignal(in godot_string_name signal)
  {
    return StringName.op_Equality(ref signal, NTargetManager.SignalName.CreatureHovered) || StringName.op_Equality(ref signal, NTargetManager.SignalName.CreatureUnhovered) || StringName.op_Equality(ref signal, NTargetManager.SignalName.NodeHovered) || StringName.op_Equality(ref signal, NTargetManager.SignalName.NodeUnhovered) || StringName.op_Equality(ref signal, NTargetManager.SignalName.TargetingBegan) || StringName.op_Equality(ref signal, NTargetManager.SignalName.TargetingEnded) || base.HasGodotClassSignal(ref signal);
  }

  [Signal]
  public delegate void CreatureHoveredEventHandler(
  #nullable enable
  NCreature creature);

  [Signal]
  public delegate void CreatureUnhoveredEventHandler(NCreature creature);

  [Signal]
  public delegate void NodeHoveredEventHandler(Node node);

  [Signal]
  public delegate void NodeUnhoveredEventHandler(Node node);

  [Signal]
  public delegate void TargetingBeganEventHandler();

  [Signal]
  public delegate void TargetingEndedEventHandler();

  public class MethodName : Node2D.MethodName
  {
    public static readonly 
    #nullable disable
    StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName _Input = StringName.op_Implicit(nameof (_Input));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName CancelTargeting = StringName.op_Implicit(nameof (CancelTargeting));
    public static readonly StringName FinishTargeting = StringName.op_Implicit(nameof (FinishTargeting));
    public static readonly StringName AllowedToTargetNode = StringName.op_Implicit(nameof (AllowedToTargetNode));
    public static readonly StringName OnNodeHovered = StringName.op_Implicit(nameof (OnNodeHovered));
    public static readonly StringName OnNodeUnhovered = StringName.op_Implicit(nameof (OnNodeUnhovered));
    public static readonly StringName OnCreatureHovered = StringName.op_Implicit(nameof (OnCreatureHovered));
    public static readonly StringName OnCreatureUnhovered = StringName.op_Implicit(nameof (OnCreatureUnhovered));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName IsInSelection = StringName.op_Implicit(nameof (IsInSelection));
    public static readonly StringName HoveredNode = StringName.op_Implicit(nameof (HoveredNode));
    public static readonly StringName LastTargetingFinishedFrame = StringName.op_Implicit(nameof (LastTargetingFinishedFrame));
    public static readonly StringName _targetingArrow = StringName.op_Implicit(nameof (_targetingArrow));
    public static readonly StringName _targetMode = StringName.op_Implicit(nameof (_targetMode));
    public static readonly StringName _validTargetsType = StringName.op_Implicit(nameof (_validTargetsType));
  }

  public class SignalName : Node2D.SignalName
  {
    public static readonly StringName CreatureHovered = StringName.op_Implicit(nameof (CreatureHovered));
    public static readonly StringName CreatureUnhovered = StringName.op_Implicit(nameof (CreatureUnhovered));
    public static readonly StringName NodeHovered = StringName.op_Implicit(nameof (NodeHovered));
    public static readonly StringName NodeUnhovered = StringName.op_Implicit(nameof (NodeUnhovered));
    public static readonly StringName TargetingBegan = StringName.op_Implicit(nameof (TargetingBegan));
    public static readonly StringName TargetingEnded = StringName.op_Implicit(nameof (TargetingEnded));
  }
}
