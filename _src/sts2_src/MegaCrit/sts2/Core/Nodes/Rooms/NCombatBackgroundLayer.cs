// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Rooms.NCombatBackgroundLayer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Rooms;

[ScriptPath("res://src/Core/Nodes/Rooms/NCombatBackgroundLayer.cs")]
public class NCombatBackgroundLayer : Control
{
  private Control _visual;
  private Control _phobiaModeVisual;

  public override void _Ready()
  {
    this._visual = ((Node) this).GetNode<Control>(NodePath.op_Implicit("Visual"));
    this._phobiaModeVisual = ((Node) this).GetNode<Control>(NodePath.op_Implicit("PhobiaModeVisual"));
    this.UpdatePhobiaMode();
  }

  public override void _EnterTree()
  {
    ((GodotObject) NGame.Instance)?.Connect(NGame.SignalName.PhobiaModeToggled, Callable.From(new Action(this.UpdatePhobiaMode)), 0U);
  }

  public override void _ExitTree()
  {
    ((GodotObject) NGame.Instance)?.Disconnect(NGame.SignalName.PhobiaModeToggled, Callable.From(new Action(this.UpdatePhobiaMode)));
  }

  private void UpdatePhobiaMode()
  {
    ((CanvasItem) this._phobiaModeVisual).Visible = SaveManager.Instance.PrefsSave.PhobiaMode;
    ((CanvasItem) this._visual).Visible = !((CanvasItem) this._phobiaModeVisual).Visible;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NCombatBackgroundLayer.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatBackgroundLayer.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatBackgroundLayer.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatBackgroundLayer.MethodName.UpdatePhobiaMode, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCombatBackgroundLayer.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatBackgroundLayer.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatBackgroundLayer.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCombatBackgroundLayer.MethodName.UpdatePhobiaMode) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.UpdatePhobiaMode();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCombatBackgroundLayer.MethodName._Ready) || StringName.op_Equality(ref method, NCombatBackgroundLayer.MethodName._EnterTree) || StringName.op_Equality(ref method, NCombatBackgroundLayer.MethodName._ExitTree) || StringName.op_Equality(ref method, NCombatBackgroundLayer.MethodName.UpdatePhobiaMode) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCombatBackgroundLayer.PropertyName._visual))
    {
      this._visual = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCombatBackgroundLayer.PropertyName._phobiaModeVisual))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._phobiaModeVisual = VariantUtils.ConvertTo<Control>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCombatBackgroundLayer.PropertyName._visual))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._visual);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCombatBackgroundLayer.PropertyName._phobiaModeVisual))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Control>(ref this._phobiaModeVisual);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCombatBackgroundLayer.PropertyName._visual, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatBackgroundLayer.PropertyName._phobiaModeVisual, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NCombatBackgroundLayer.PropertyName._visual, Variant.From<Control>(ref this._visual));
    info.AddProperty(NCombatBackgroundLayer.PropertyName._phobiaModeVisual, Variant.From<Control>(ref this._phobiaModeVisual));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCombatBackgroundLayer.PropertyName._visual, ref variant1))
      this._visual = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (!info.TryGetProperty(NCombatBackgroundLayer.PropertyName._phobiaModeVisual, ref variant2))
      return;
    this._phobiaModeVisual = ((Variant) ref variant2).As<Control>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName UpdatePhobiaMode = StringName.op_Implicit(nameof (UpdatePhobiaMode));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _visual = StringName.op_Implicit(nameof (_visual));
    public static readonly StringName _phobiaModeVisual = StringName.op_Implicit(nameof (_phobiaModeVisual));
  }

  public class SignalName : Control.SignalName
  {
  }
}
