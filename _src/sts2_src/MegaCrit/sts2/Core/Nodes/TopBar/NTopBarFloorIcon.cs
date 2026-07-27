// Decompiled with JetBrains decompiler
// Type: MegaCrit.sts2.Core.Nodes.TopBar.NTopBarFloorIcon
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.sts2.Core.Nodes.TopBar;

[ScriptPath("res://src/Core/Nodes/TopBar/NTopBarFloorIcon.cs")]
public class NTopBarFloorIcon : NClickableControl
{
  private MegaLabel _floorNumLabel;
  private IRunState _runState;

  public override void _Ready()
  {
    this._floorNumLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("FloorNumLabel"));
    this.ConnectSignals();
  }

  public void Initialize(IRunState runState)
  {
    this._runState = runState;
    this.UpdateIcon();
  }

  public override void _EnterTree()
  {
    RunManager.Instance.RoomEntered += new Action(this.UpdateIcon);
  }

  public override void _ExitTree()
  {
    RunManager.Instance.RoomEntered -= new Action(this.UpdateIcon);
  }

  private void UpdateIcon()
  {
    if (this._runState.CurrentRoom == null)
      return;
    this._floorNumLabel.SetTextAutoSize(this._runState.TotalFloor.ToString());
  }

  protected override void OnFocus()
  {
    NHoverTipSet.CreateAndShow((Control) this, (IHoverTip) new HoverTip(new LocString("static_hover_tips", "FLOOR.title"), new LocString("static_hover_tips", "FLOOR.description")))?.SetGlobalPosition(Vector2.op_Addition(this.GlobalPosition, new Vector2(0.0f, this.Size.Y + 20f)), false);
  }

  protected override void OnUnfocus() => NHoverTipSet.Remove((Control) this);

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NTopBarFloorIcon.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarFloorIcon.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarFloorIcon.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarFloorIcon.MethodName.UpdateIcon, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarFloorIcon.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarFloorIcon.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NTopBarFloorIcon.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarFloorIcon.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarFloorIcon.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarFloorIcon.MethodName.UpdateIcon) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateIcon();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarFloorIcon.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NTopBarFloorIcon.MethodName.OnUnfocus) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnUnfocus();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NTopBarFloorIcon.MethodName._Ready) || StringName.op_Equality(ref method, NTopBarFloorIcon.MethodName._EnterTree) || StringName.op_Equality(ref method, NTopBarFloorIcon.MethodName._ExitTree) || StringName.op_Equality(ref method, NTopBarFloorIcon.MethodName.UpdateIcon) || StringName.op_Equality(ref method, NTopBarFloorIcon.MethodName.OnFocus) || StringName.op_Equality(ref method, NTopBarFloorIcon.MethodName.OnUnfocus) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NTopBarFloorIcon.PropertyName._floorNumLabel))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._floorNumLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NTopBarFloorIcon.PropertyName._floorNumLabel))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<MegaLabel>(ref this._floorNumLabel);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NTopBarFloorIcon.PropertyName._floorNumLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NTopBarFloorIcon.PropertyName._floorNumLabel, Variant.From<MegaLabel>(ref this._floorNumLabel));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(NTopBarFloorIcon.PropertyName._floorNumLabel, ref variant))
      return;
    this._floorNumLabel = ((Variant) ref variant).As<MegaLabel>();
  }

  public new class MethodName : NClickableControl.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName UpdateIcon = StringName.op_Implicit(nameof (UpdateIcon));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
  }

  public new class PropertyName : NClickableControl.PropertyName
  {
    public static readonly StringName _floorNumLabel = StringName.op_Implicit(nameof (_floorNumLabel));
  }

  public new class SignalName : NClickableControl.SignalName
  {
  }
}
