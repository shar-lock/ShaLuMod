// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.Utilities.NTrail2D
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;

[ScriptPath("res://src/Core/Nodes/Vfx/Utilities/NTrail2D.cs")]
public class NTrail2D : Line2D
{
  private int _maxSegments = 20;
  private Node2D _parent;
  private readonly List<Vector2> _pointQueue = new List<Vector2>();
  private bool _isActive;

  public override void _Ready()
  {
    this._parent = ((Node) this).GetParent<Node2D>();
    ((GodotObject) this).Connect(CanvasItem.SignalName.VisibilityChanged, Callable.From(new Action(this.OnToggleVisibility)), 0U);
    this.OnToggleVisibility();
  }

  private void OnToggleVisibility()
  {
    this._isActive = ((CanvasItem) this).Visible;
    if (((CanvasItem) this).Visible)
      return;
    this._pointQueue.Clear();
    this.Points = this._pointQueue.ToArray();
  }

  public override void _Process(double delta)
  {
    if (!this._isActive)
      return;
    this._pointQueue.Insert(0, this._parent.GlobalPosition);
    if (this._pointQueue.Count >= this._maxSegments)
      this._pointQueue.RemoveAt(this._pointQueue.Count - 1);
    this.Points = this._pointQueue.Select<Vector2, Vector2>((Func<Vector2, Vector2>) (point => ((Node) this).GetParent<Node2D>().ToLocal(point))).ToArray<Vector2>();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NTrail2D.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTrail2D.MethodName.OnToggleVisibility, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTrail2D.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NTrail2D.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTrail2D.MethodName.OnToggleVisibility) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnToggleVisibility();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NTrail2D.MethodName._Process) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NTrail2D.MethodName._Ready) || StringName.op_Equality(ref method, NTrail2D.MethodName.OnToggleVisibility) || StringName.op_Equality(ref method, NTrail2D.MethodName._Process) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NTrail2D.PropertyName._maxSegments))
    {
      this._maxSegments = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTrail2D.PropertyName._parent))
    {
      this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NTrail2D.PropertyName._isActive))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._isActive = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NTrail2D.PropertyName._maxSegments))
    {
      value = VariantUtils.CreateFrom<int>(ref this._maxSegments);
      return true;
    }
    if (StringName.op_Equality(ref name, NTrail2D.PropertyName._parent))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
      return true;
    }
    if (!StringName.op_Equality(ref name, NTrail2D.PropertyName._isActive))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._isActive);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 2L, NTrail2D.PropertyName._maxSegments, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTrail2D.PropertyName._parent, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NTrail2D.PropertyName._isActive, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NTrail2D.PropertyName._maxSegments, Variant.From<int>(ref this._maxSegments));
    info.AddProperty(NTrail2D.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
    info.AddProperty(NTrail2D.PropertyName._isActive, Variant.From<bool>(ref this._isActive));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NTrail2D.PropertyName._maxSegments, ref variant1))
      this._maxSegments = ((Variant) ref variant1).As<int>();
    Variant variant2;
    if (info.TryGetProperty(NTrail2D.PropertyName._parent, ref variant2))
      this._parent = ((Variant) ref variant2).As<Node2D>();
    Variant variant3;
    if (!info.TryGetProperty(NTrail2D.PropertyName._isActive, ref variant3))
      return;
    this._isActive = ((Variant) ref variant3).As<bool>();
  }

  public class MethodName : Line2D.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnToggleVisibility = StringName.op_Implicit(nameof (OnToggleVisibility));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
  }

  public class PropertyName : Line2D.PropertyName
  {
    public static readonly StringName _maxSegments = StringName.op_Implicit(nameof (_maxSegments));
    public static readonly StringName _parent = StringName.op_Implicit(nameof (_parent));
    public static readonly StringName _isActive = StringName.op_Implicit(nameof (_isActive));
  }

  public class SignalName : Line2D.SignalName
  {
  }
}
