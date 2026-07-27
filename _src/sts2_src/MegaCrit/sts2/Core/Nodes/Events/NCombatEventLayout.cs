// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Events.NCombatEventLayout
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Events;

[ScriptPath("res://src/Core/Nodes/Events/NCombatEventLayout.cs")]
public class NCombatEventLayout : NEventLayout
{
  public const string combatScenePath = "res://scenes/events/combat_event_layout.tscn";
  private Control _combatRoomContainer;

  public NCombatRoom? EmbeddedCombatRoom { get; private set; }

  public bool HasCombatStarted { get; private set; }

  public override void _Ready()
  {
    base._Ready();
    this._combatRoomContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%CombatRoomContainer"));
  }

  public void SetCombatRoomNode(NCombatRoom? combatRoomNode)
  {
    if (combatRoomNode == null)
      return;
    this.EmbeddedCombatRoom = this.EmbeddedCombatRoom == null ? combatRoomNode : throw new InvalidOperationException("Combat room node was already set.");
    ((Node) this._combatRoomContainer).AddChildSafely((Node) combatRoomNode);
  }

  public override void SetEvent(EventModel eventModel)
  {
    IRunState runState = eventModel.Owner.RunState;
    NCombatRoom combatRoomNode = NCombatRoom.Create(eventModel.CreateCombatRoomVisuals((IEnumerable<Player>) runState.Players, runState.Act), CombatRoomMode.VisualOnly);
    this.SetCombatRoomNode(combatRoomNode);
    combatRoomNode?.SetUpBackground(runState);
    base.SetEvent(eventModel);
  }

  protected override void InitializeVisuals()
  {
  }

  public void HideEventVisuals()
  {
    if (this._description != null)
      ((CanvasItem) this._description).Visible = false;
    if (this._sharedEventLabel != null)
      ((CanvasItem) this._sharedEventLabel).Visible = false;
    ((CanvasItem) this._optionsContainer).Visible = false;
    this.HasCombatStarted = true;
    Control defaultFocusedControl = this.DefaultFocusedControl;
    if (defaultFocusedControl == null)
      return;
    defaultFocusedControl.TryGrabFocus();
  }

  public override Control? DefaultFocusedControl
  {
    get
    {
      if (!this.HasCombatStarted)
        return base.DefaultFocusedControl;
      return this.EmbeddedCombatRoom?.DefaultFocusedControl;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NCombatEventLayout.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatEventLayout.MethodName.SetCombatRoomNode, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("combatRoomNode"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCombatEventLayout.MethodName.InitializeVisuals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatEventLayout.MethodName.HideEventVisuals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCombatEventLayout.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatEventLayout.MethodName.SetCombatRoomNode) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetCombatRoomNode(VariantUtils.ConvertTo<NCombatRoom>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatEventLayout.MethodName.InitializeVisuals) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.InitializeVisuals();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCombatEventLayout.MethodName.HideEventVisuals) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.HideEventVisuals();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCombatEventLayout.MethodName._Ready) || StringName.op_Equality(ref method, NCombatEventLayout.MethodName.SetCombatRoomNode) || StringName.op_Equality(ref method, NCombatEventLayout.MethodName.InitializeVisuals) || StringName.op_Equality(ref method, NCombatEventLayout.MethodName.HideEventVisuals) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCombatEventLayout.PropertyName.EmbeddedCombatRoom))
    {
      this.EmbeddedCombatRoom = VariantUtils.ConvertTo<NCombatRoom>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatEventLayout.PropertyName.HasCombatStarted))
    {
      this.HasCombatStarted = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCombatEventLayout.PropertyName._combatRoomContainer))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._combatRoomContainer = VariantUtils.ConvertTo<Control>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCombatEventLayout.PropertyName.EmbeddedCombatRoom))
    {
      ref godot_variant local = ref value;
      NCombatRoom embeddedCombatRoom = this.EmbeddedCombatRoom;
      godot_variant from = VariantUtils.CreateFrom<NCombatRoom>(ref embeddedCombatRoom);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatEventLayout.PropertyName.HasCombatStarted))
    {
      ref godot_variant local = ref value;
      bool hasCombatStarted = this.HasCombatStarted;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref hasCombatStarted);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatEventLayout.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (!StringName.op_Equality(ref name, NCombatEventLayout.PropertyName._combatRoomContainer))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Control>(ref this._combatRoomContainer);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCombatEventLayout.PropertyName._combatRoomContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatEventLayout.PropertyName.EmbeddedCombatRoom, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCombatEventLayout.PropertyName.HasCombatStarted, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatEventLayout.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo1 = info;
    StringName embeddedCombatRoom1 = NCombatEventLayout.PropertyName.EmbeddedCombatRoom;
    NCombatRoom embeddedCombatRoom2 = this.EmbeddedCombatRoom;
    Variant variant1 = Variant.From<NCombatRoom>(ref embeddedCombatRoom2);
    serializationInfo1.AddProperty(embeddedCombatRoom1, variant1);
    GodotSerializationInfo serializationInfo2 = info;
    StringName hasCombatStarted1 = NCombatEventLayout.PropertyName.HasCombatStarted;
    bool hasCombatStarted2 = this.HasCombatStarted;
    Variant variant2 = Variant.From<bool>(ref hasCombatStarted2);
    serializationInfo2.AddProperty(hasCombatStarted1, variant2);
    info.AddProperty(NCombatEventLayout.PropertyName._combatRoomContainer, Variant.From<Control>(ref this._combatRoomContainer));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCombatEventLayout.PropertyName.EmbeddedCombatRoom, ref variant1))
      this.EmbeddedCombatRoom = ((Variant) ref variant1).As<NCombatRoom>();
    Variant variant2;
    if (info.TryGetProperty(NCombatEventLayout.PropertyName.HasCombatStarted, ref variant2))
      this.HasCombatStarted = ((Variant) ref variant2).As<bool>();
    Variant variant3;
    if (!info.TryGetProperty(NCombatEventLayout.PropertyName._combatRoomContainer, ref variant3))
      return;
    this._combatRoomContainer = ((Variant) ref variant3).As<Control>();
  }

  public new class MethodName : NEventLayout.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName SetCombatRoomNode = StringName.op_Implicit(nameof (SetCombatRoomNode));
    public new static readonly StringName InitializeVisuals = StringName.op_Implicit(nameof (InitializeVisuals));
    public static readonly StringName HideEventVisuals = StringName.op_Implicit(nameof (HideEventVisuals));
  }

  public new class PropertyName : NEventLayout.PropertyName
  {
    public static readonly StringName EmbeddedCombatRoom = StringName.op_Implicit(nameof (EmbeddedCombatRoom));
    public static readonly StringName HasCombatStarted = StringName.op_Implicit(nameof (HasCombatStarted));
    public new static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _combatRoomContainer = StringName.op_Implicit(nameof (_combatRoomContainer));
  }

  public new class SignalName : NEventLayout.SignalName
  {
  }
}
