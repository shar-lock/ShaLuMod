// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NCardTrail
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NCardTrail.cs")]
public class NCardTrail : Line2D
{
  private Node2D _parent;
  private float _pointDuration = 0.8f;
  private readonly List<float> _pointAge = new List<float>();
  private const float _minSpawnDist = 12f;
  private const float _maxSpawnDist = 48f;
  private Vector2? _lastPointPosition;

  public override void _Ready()
  {
    this._parent = ((Node) this).GetParent<Node2D>();
    ((GodotObject) this).Connect(CanvasItem.SignalName.VisibilityChanged, Callable.From(new Action(this.OnToggleVisibility)), 0U);
  }

  public override void _Process(double delta)
  {
    ((Node2D) this).GlobalPosition = Vector2.Zero;
    ((Node2D) this).GlobalRotation = 0.0f;
    float num = (float) delta;
    for (int index = 0; index < this.GetPointCount(); ++index)
    {
      if ((double) this._pointAge[index] > (double) this._pointDuration)
      {
        this.RemovePoint(0);
        this._pointAge.RemoveAt(0);
      }
      else
        this._pointAge[index] += num;
    }
    this.CreatePoint(this._parent.GlobalPosition, delta);
  }

  private void OnToggleVisibility()
  {
    ((Node) this).ProcessMode = ((CanvasItem) this).Visible ? (Node.ProcessModeEnum) 0L : (Node.ProcessModeEnum) 4L;
    this.ClearPoints();
  }

  private void CreatePoint(Vector2 pointPos, double delta)
  {
    if (this._lastPointPosition.HasValue)
    {
      float num1 = ((Vector2) ref pointPos).DistanceTo(this._lastPointPosition.Value);
      if ((double) num1 < 12.0)
        return;
      int pointCount = this.GetPointCount();
      if (pointCount > 2 && (double) num1 > 48.0)
      {
        Vector2 pointPosition1 = this.GetPointPosition(pointCount - 2);
        Vector2 pointPosition2 = this.GetPointPosition(pointCount - 1);
        Vector2 vector2_1 = pointPos;
        for (float num2 = 48f; (double) num2 < (double) num1 - 12.0; num2 += 48f)
        {
          float num3 = (float) (0.5 + (double) num2 / (double) num1 * 0.5);
          Vector2 vector2_2 = ((Vector2) ref pointPosition1).Lerp(pointPosition2, num3);
          Vector2 vector2_3 = ((Vector2) ref pointPosition2).Lerp(vector2_1, num3);
          Vector2 vector2_4 = ((Vector2) ref vector2_2).Lerp(vector2_3, num3);
          this._pointAge.Add((float) delta * num3);
          this.AddPoint(vector2_4, -1);
        }
      }
    }
    this._pointAge.Add(0.0f);
    this.AddPoint(pointPos, -1);
    this._lastPointPosition = new Vector2?(pointPos);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NCardTrail.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardTrail.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCardTrail.MethodName.OnToggleVisibility, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardTrail.MethodName.CreatePoint, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("pointPos"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
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
    if (StringName.op_Equality(ref method, NCardTrail.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardTrail.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardTrail.MethodName.OnToggleVisibility) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnToggleVisibility();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCardTrail.MethodName.CreatePoint) || ((NativeVariantPtrArgs) ref args).Count != 2)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.CreatePoint(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[1]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCardTrail.MethodName._Ready) || StringName.op_Equality(ref method, NCardTrail.MethodName._Process) || StringName.op_Equality(ref method, NCardTrail.MethodName.OnToggleVisibility) || StringName.op_Equality(ref method, NCardTrail.MethodName.CreatePoint) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardTrail.PropertyName._parent))
    {
      this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardTrail.PropertyName._pointDuration))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._pointDuration = VariantUtils.ConvertTo<float>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardTrail.PropertyName._parent))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardTrail.PropertyName._pointDuration))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<float>(ref this._pointDuration);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCardTrail.PropertyName._parent, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NCardTrail.PropertyName._pointDuration, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NCardTrail.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
    info.AddProperty(NCardTrail.PropertyName._pointDuration, Variant.From<float>(ref this._pointDuration));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCardTrail.PropertyName._parent, ref variant1))
      this._parent = ((Variant) ref variant1).As<Node2D>();
    Variant variant2;
    if (!info.TryGetProperty(NCardTrail.PropertyName._pointDuration, ref variant2))
      return;
    this._pointDuration = ((Variant) ref variant2).As<float>();
  }

  public class MethodName : Line2D.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName OnToggleVisibility = StringName.op_Implicit(nameof (OnToggleVisibility));
    public static readonly StringName CreatePoint = StringName.op_Implicit(nameof (CreatePoint));
  }

  public class PropertyName : Line2D.PropertyName
  {
    public static readonly StringName _parent = StringName.op_Implicit(nameof (_parent));
    public static readonly StringName _pointDuration = StringName.op_Implicit(nameof (_pointDuration));
  }

  public class SignalName : Line2D.SignalName
  {
  }
}
