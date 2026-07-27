// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NEpochSlotParticle
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Random;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NEpochSlotParticle.cs")]
public class NEpochSlotParticle : Sprite2D
{
  public static readonly string scenePath = SceneHelper.GetScenePath("vfx/epoch_slot_particle_vfx");
  private const double _checkRate = 0.25;
  private double _timer;
  private Control _target;
  private float _speed;
  private const float _deleteDistance = 1500f;

  public static NEpochSlotParticle Create(Control target)
  {
    NEpochSlotParticle nepochSlotParticle = PreloadManager.Cache.GetScene(NEpochSlotParticle.scenePath).Instantiate<NEpochSlotParticle>((PackedScene.GenEditState) 0L);
    nepochSlotParticle._target = target;
    return nepochSlotParticle;
  }

  public override void _Ready()
  {
    ((Node2D) this).GlobalPosition = Vector2.op_Addition(this._target.GlobalPosition, new Vector2(Rng.Chaotic.NextFloat(40f, 350f) * (Rng.Chaotic.NextBool() ? -1f : 1f), Rng.Chaotic.NextFloat(40f, 300f) * (Rng.Chaotic.NextBool() ? -1f : 1f)));
    float num = Rng.Chaotic.NextFloat(0.5f, 1.5f);
    ((Node2D) this).Scale = Vector2.op_Multiply(new Vector2(1f, 1f), num);
    ((CanvasItem) this).Modulate = new Color(Rng.Chaotic.NextFloat(0.75f, 0.9f), Rng.Chaotic.NextFloat(0.7f, 0.8f), Rng.Chaotic.NextFloat(0.0f, 0.4f), 0.0f);
    this._speed = Rng.Chaotic.NextFloat(3f, 4f) / num;
    Vector2 vector2 = Vector2.op_Subtraction(this._target.GlobalPosition, ((Node2D) this).GlobalPosition);
    ((Node2D) this).Rotation = Mathf.Atan2(vector2.Y, vector2.X) + 1.57079637f;
    ((Node) this).CreateTween().TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(Rng.Chaotic.NextFloat(0.75f, 1f)), 0.25);
  }

  public override void _Process(double delta)
  {
    this._timer -= delta;
    Vector2 globalPosition;
    if (this._timer < 0.0)
    {
      this._timer += 0.25;
      globalPosition = ((Node2D) this).GlobalPosition;
      if ((double) ((Vector2) ref globalPosition).DistanceSquaredTo(this._target.GlobalPosition) < 1500.0)
        ((Node) this).QueueFreeSafely();
    }
    globalPosition = ((Node2D) this).GlobalPosition;
    ((Node2D) this).GlobalPosition = ((Vector2) ref globalPosition).Lerp(this._target.GlobalPosition, this._speed * (float) delta);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NEpochSlotParticle.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Sprite2D"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("target"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NEpochSlotParticle.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEpochSlotParticle.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NEpochSlotParticle.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NEpochSlotParticle nepochSlotParticle = NEpochSlotParticle.Create(VariantUtils.ConvertTo<Control>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NEpochSlotParticle>(ref nepochSlotParticle);
      return true;
    }
    if (StringName.op_Equality(ref method, NEpochSlotParticle.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NEpochSlotParticle.MethodName._Process) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NEpochSlotParticle.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NEpochSlotParticle nepochSlotParticle = NEpochSlotParticle.Create(VariantUtils.ConvertTo<Control>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NEpochSlotParticle>(ref nepochSlotParticle);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NEpochSlotParticle.MethodName.Create) || StringName.op_Equality(ref method, NEpochSlotParticle.MethodName._Ready) || StringName.op_Equality(ref method, NEpochSlotParticle.MethodName._Process) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NEpochSlotParticle.PropertyName._timer))
    {
      this._timer = VariantUtils.ConvertTo<double>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlotParticle.PropertyName._target))
    {
      this._target = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NEpochSlotParticle.PropertyName._speed))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._speed = VariantUtils.ConvertTo<float>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NEpochSlotParticle.PropertyName._timer))
    {
      value = VariantUtils.CreateFrom<double>(ref this._timer);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochSlotParticle.PropertyName._target))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._target);
      return true;
    }
    if (!StringName.op_Equality(ref name, NEpochSlotParticle.PropertyName._speed))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<float>(ref this._speed);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 3L, NEpochSlotParticle.PropertyName._timer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochSlotParticle.PropertyName._target, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NEpochSlotParticle.PropertyName._speed, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NEpochSlotParticle.PropertyName._timer, Variant.From<double>(ref this._timer));
    info.AddProperty(NEpochSlotParticle.PropertyName._target, Variant.From<Control>(ref this._target));
    info.AddProperty(NEpochSlotParticle.PropertyName._speed, Variant.From<float>(ref this._speed));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NEpochSlotParticle.PropertyName._timer, ref variant1))
      this._timer = ((Variant) ref variant1).As<double>();
    Variant variant2;
    if (info.TryGetProperty(NEpochSlotParticle.PropertyName._target, ref variant2))
      this._target = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (!info.TryGetProperty(NEpochSlotParticle.PropertyName._speed, ref variant3))
      return;
    this._speed = ((Variant) ref variant3).As<float>();
  }

  public class MethodName : Sprite2D.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
  }

  public class PropertyName : Sprite2D.PropertyName
  {
    public static readonly StringName _timer = StringName.op_Implicit(nameof (_timer));
    public static readonly StringName _target = StringName.op_Implicit(nameof (_target));
    public static readonly StringName _speed = StringName.op_Implicit(nameof (_speed));
  }

  public class SignalName : Sprite2D.SignalName
  {
  }
}
