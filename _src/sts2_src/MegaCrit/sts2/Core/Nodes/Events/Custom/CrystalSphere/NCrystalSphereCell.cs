// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Events.Custom.CrystalSphere.NCrystalSphereCell
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Events.Custom.CrystalSphere;

[ScriptPath("res://src/Core/Nodes/Events/Custom/CrystalSphere/NCrystalSphereCell.cs")]
public class NCrystalSphereCell : NClickableControl
{
  private const string _scenePath = "res://scenes/events/custom/crystal_sphere/crystal_sphere_cell.tscn";
  private NCrystalSphereMask _mask;
  private Control _hoveredFg;
  private Tween? _fadeTween;

  public CrystalSphereCell Entity { get; private set; }

  public static NCrystalSphereCell? Create(CrystalSphereCell cell, NCrystalSphereMask mask)
  {
    if (TestMode.IsOn)
      return (NCrystalSphereCell) null;
    NCrystalSphereCell ncrystalSphereCell = PreloadManager.Cache.GetScene("res://scenes/events/custom/crystal_sphere/crystal_sphere_cell.tscn").Instantiate<NCrystalSphereCell>((PackedScene.GenEditState) 0L);
    ncrystalSphereCell.Entity = cell;
    ncrystalSphereCell._mask = mask;
    return ncrystalSphereCell;
  }

  public override void _Ready()
  {
    this._hoveredFg = ((Node) this).GetNode<Control>(NodePath.op_Implicit("HoveredFg"));
    ((CanvasItem) this).Modulate = Colors.Transparent;
    this.MouseFilter = this.Entity.IsHidden ? (Control.MouseFilterEnum) 0L : (Control.MouseFilterEnum) 2L;
    this.FocusMode = this.Entity.IsHidden ? (Control.FocusModeEnum) 2L : (Control.FocusModeEnum) 0L;
    ((CanvasItem) this._hoveredFg).Visible = false;
  }

  public override void _EnterTree()
  {
    this.Entity.HighlightUpdated += new Action(this.OnEntityHighlightUpdated);
    this.Entity.FogUpdated += new Action(this.EntityClicked);
  }

  public override void _ExitTree()
  {
    this.Entity.HighlightUpdated -= new Action(this.OnEntityHighlightUpdated);
    this.Entity.FogUpdated -= new Action(this.EntityClicked);
  }

  private void OnEntityHighlightUpdated()
  {
    this._fadeTween?.Kill();
    this._fadeTween = ((Node) this).CreateTween();
    Tween fadeTween = this._fadeTween;
    NodePath nodePath = NodePath.op_Implicit("modulate");
    CrystalSphereCell entity = this.Entity;
    Variant variant = Variant.op_Implicit(entity == null || !entity.IsHighlighted || !entity.IsHidden ? Colors.Transparent : Colors.White);
    fadeTween.TweenProperty((GodotObject) this, nodePath, variant, 0.15000000596046448);
    ((CanvasItem) this._hoveredFg).Visible = this.Entity.IsHovered;
  }

  private void EntityClicked()
  {
    this.MouseFilter = this.Entity.IsHidden ? (Control.MouseFilterEnum) 0L : (Control.MouseFilterEnum) 2L;
    this.FocusMode = this.Entity.IsHidden ? (Control.FocusModeEnum) 2L : (Control.FocusModeEnum) 0L;
    this._mask.UpdateMat(this.Entity);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NCrystalSphereCell.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCrystalSphereCell.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCrystalSphereCell.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCrystalSphereCell.MethodName.OnEntityHighlightUpdated, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCrystalSphereCell.MethodName.EntityClicked, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCrystalSphereCell.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCrystalSphereCell.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCrystalSphereCell.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCrystalSphereCell.MethodName.OnEntityHighlightUpdated) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnEntityHighlightUpdated();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCrystalSphereCell.MethodName.EntityClicked) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.EntityClicked();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCrystalSphereCell.MethodName._Ready) || StringName.op_Equality(ref method, NCrystalSphereCell.MethodName._EnterTree) || StringName.op_Equality(ref method, NCrystalSphereCell.MethodName._ExitTree) || StringName.op_Equality(ref method, NCrystalSphereCell.MethodName.OnEntityHighlightUpdated) || StringName.op_Equality(ref method, NCrystalSphereCell.MethodName.EntityClicked) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCrystalSphereCell.PropertyName._mask))
    {
      this._mask = VariantUtils.ConvertTo<NCrystalSphereMask>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCrystalSphereCell.PropertyName._hoveredFg))
    {
      this._hoveredFg = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCrystalSphereCell.PropertyName._fadeTween))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._fadeTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCrystalSphereCell.PropertyName._mask))
    {
      value = VariantUtils.CreateFrom<NCrystalSphereMask>(ref this._mask);
      return true;
    }
    if (StringName.op_Equality(ref name, NCrystalSphereCell.PropertyName._hoveredFg))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._hoveredFg);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCrystalSphereCell.PropertyName._fadeTween))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Tween>(ref this._fadeTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCrystalSphereCell.PropertyName._mask, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCrystalSphereCell.PropertyName._hoveredFg, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCrystalSphereCell.PropertyName._fadeTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NCrystalSphereCell.PropertyName._mask, Variant.From<NCrystalSphereMask>(ref this._mask));
    info.AddProperty(NCrystalSphereCell.PropertyName._hoveredFg, Variant.From<Control>(ref this._hoveredFg));
    info.AddProperty(NCrystalSphereCell.PropertyName._fadeTween, Variant.From<Tween>(ref this._fadeTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCrystalSphereCell.PropertyName._mask, ref variant1))
      this._mask = ((Variant) ref variant1).As<NCrystalSphereMask>();
    Variant variant2;
    if (info.TryGetProperty(NCrystalSphereCell.PropertyName._hoveredFg, ref variant2))
      this._hoveredFg = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (!info.TryGetProperty(NCrystalSphereCell.PropertyName._fadeTween, ref variant3))
      return;
    this._fadeTween = ((Variant) ref variant3).As<Tween>();
  }

  public new class MethodName : NClickableControl.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName OnEntityHighlightUpdated = StringName.op_Implicit(nameof (OnEntityHighlightUpdated));
    public static readonly StringName EntityClicked = StringName.op_Implicit(nameof (EntityClicked));
  }

  public new class PropertyName : NClickableControl.PropertyName
  {
    public static readonly StringName _mask = StringName.op_Implicit(nameof (_mask));
    public static readonly StringName _hoveredFg = StringName.op_Implicit(nameof (_hoveredFg));
    public static readonly StringName _fadeTween = StringName.op_Implicit(nameof (_fadeTween));
  }

  public new class SignalName : NClickableControl.SignalName
  {
  }
}
