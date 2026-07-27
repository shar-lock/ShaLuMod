// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Animation.NDecimillipedeSegmentDriver
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Animation;

[ScriptPath("res://src/Core/Nodes/Animation/NDecimillipedeSegmentDriver.cs")]
public class NDecimillipedeSegmentDriver : Node2D
{
  [Export]
  private bool _leftSegment;
  private float _speed;
  private float _magnitude;
  private Vector2 _originPos;
  private FastNoiseLite _noise = new FastNoiseLite();
  private float _time;
  private Vector2 _decimillipedeStrikeOffset = Vector2.Zero;
  private Tween? _attackTween;

  public override void _Ready()
  {
    this._originPos = this.Position;
    this._speed = this._leftSegment ? 0.1f : 0.05f;
    this._magnitude = this._leftSegment ? 250f : 300f;
    this._noise.NoiseType = (FastNoiseLite.NoiseTypeEnum) 3L;
    this._noise.Frequency = 1f;
  }

  public override void _Process(double delta)
  {
    this._time += (float) delta;
    float num = (float) ((double) this._time * (double) this._speed + (this._leftSegment ? 0.25 : 0.0));
    this.Position = Vector2.op_Addition(Vector2.op_Addition(this._originPos, Vector2.op_Multiply(new Vector2(((Noise) this._noise).GetNoise1D(num), ((Noise) this._noise).GetNoise1D(num + 0.25f)), this._magnitude)), this._decimillipedeStrikeOffset);
  }

  public void AttackShake()
  {
    this._attackTween?.Kill();
    this._attackTween = ((Node) this).CreateTween();
    this._attackTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("_decimillipedeStrikeOffset"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.Left, 100f)), 0.4).SetEase((Tween.EaseType) 2L).SetTrans((Tween.TransitionType) 1L);
    this._attackTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("_decimillipedeStrikeOffset"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.Right, 100f)), 0.10000000149011612).SetEase((Tween.EaseType) 2L).SetTrans((Tween.TransitionType) 1L);
    this._attackTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("_decimillipedeStrikeOffset"), Variant.op_Implicit(Vector2.Zero), 0.75).SetEase((Tween.EaseType) 2L).SetTrans((Tween.TransitionType) 1L);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NDecimillipedeSegmentDriver.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDecimillipedeSegmentDriver.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NDecimillipedeSegmentDriver.MethodName.AttackShake, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NDecimillipedeSegmentDriver.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDecimillipedeSegmentDriver.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NDecimillipedeSegmentDriver.MethodName.AttackShake) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.AttackShake();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NDecimillipedeSegmentDriver.MethodName._Ready) || StringName.op_Equality(ref method, NDecimillipedeSegmentDriver.MethodName._Process) || StringName.op_Equality(ref method, NDecimillipedeSegmentDriver.MethodName.AttackShake) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDecimillipedeSegmentDriver.PropertyName._leftSegment))
    {
      this._leftSegment = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDecimillipedeSegmentDriver.PropertyName._speed))
    {
      this._speed = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDecimillipedeSegmentDriver.PropertyName._magnitude))
    {
      this._magnitude = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDecimillipedeSegmentDriver.PropertyName._originPos))
    {
      this._originPos = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDecimillipedeSegmentDriver.PropertyName._noise))
    {
      this._noise = VariantUtils.ConvertTo<FastNoiseLite>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDecimillipedeSegmentDriver.PropertyName._time))
    {
      this._time = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDecimillipedeSegmentDriver.PropertyName._decimillipedeStrikeOffset))
    {
      this._decimillipedeStrikeOffset = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDecimillipedeSegmentDriver.PropertyName._attackTween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._attackTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDecimillipedeSegmentDriver.PropertyName._leftSegment))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._leftSegment);
      return true;
    }
    if (StringName.op_Equality(ref name, NDecimillipedeSegmentDriver.PropertyName._speed))
    {
      value = VariantUtils.CreateFrom<float>(ref this._speed);
      return true;
    }
    if (StringName.op_Equality(ref name, NDecimillipedeSegmentDriver.PropertyName._magnitude))
    {
      value = VariantUtils.CreateFrom<float>(ref this._magnitude);
      return true;
    }
    if (StringName.op_Equality(ref name, NDecimillipedeSegmentDriver.PropertyName._originPos))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._originPos);
      return true;
    }
    if (StringName.op_Equality(ref name, NDecimillipedeSegmentDriver.PropertyName._noise))
    {
      value = VariantUtils.CreateFrom<FastNoiseLite>(ref this._noise);
      return true;
    }
    if (StringName.op_Equality(ref name, NDecimillipedeSegmentDriver.PropertyName._time))
    {
      value = VariantUtils.CreateFrom<float>(ref this._time);
      return true;
    }
    if (StringName.op_Equality(ref name, NDecimillipedeSegmentDriver.PropertyName._decimillipedeStrikeOffset))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._decimillipedeStrikeOffset);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDecimillipedeSegmentDriver.PropertyName._attackTween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._attackTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 1L, NDecimillipedeSegmentDriver.PropertyName._leftSegment, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NDecimillipedeSegmentDriver.PropertyName._speed, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NDecimillipedeSegmentDriver.PropertyName._magnitude, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NDecimillipedeSegmentDriver.PropertyName._originPos, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDecimillipedeSegmentDriver.PropertyName._noise, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NDecimillipedeSegmentDriver.PropertyName._time, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NDecimillipedeSegmentDriver.PropertyName._decimillipedeStrikeOffset, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDecimillipedeSegmentDriver.PropertyName._attackTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NDecimillipedeSegmentDriver.PropertyName._leftSegment, Variant.From<bool>(ref this._leftSegment));
    info.AddProperty(NDecimillipedeSegmentDriver.PropertyName._speed, Variant.From<float>(ref this._speed));
    info.AddProperty(NDecimillipedeSegmentDriver.PropertyName._magnitude, Variant.From<float>(ref this._magnitude));
    info.AddProperty(NDecimillipedeSegmentDriver.PropertyName._originPos, Variant.From<Vector2>(ref this._originPos));
    info.AddProperty(NDecimillipedeSegmentDriver.PropertyName._noise, Variant.From<FastNoiseLite>(ref this._noise));
    info.AddProperty(NDecimillipedeSegmentDriver.PropertyName._time, Variant.From<float>(ref this._time));
    info.AddProperty(NDecimillipedeSegmentDriver.PropertyName._decimillipedeStrikeOffset, Variant.From<Vector2>(ref this._decimillipedeStrikeOffset));
    info.AddProperty(NDecimillipedeSegmentDriver.PropertyName._attackTween, Variant.From<Tween>(ref this._attackTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NDecimillipedeSegmentDriver.PropertyName._leftSegment, ref variant1))
      this._leftSegment = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (info.TryGetProperty(NDecimillipedeSegmentDriver.PropertyName._speed, ref variant2))
      this._speed = ((Variant) ref variant2).As<float>();
    Variant variant3;
    if (info.TryGetProperty(NDecimillipedeSegmentDriver.PropertyName._magnitude, ref variant3))
      this._magnitude = ((Variant) ref variant3).As<float>();
    Variant variant4;
    if (info.TryGetProperty(NDecimillipedeSegmentDriver.PropertyName._originPos, ref variant4))
      this._originPos = ((Variant) ref variant4).As<Vector2>();
    Variant variant5;
    if (info.TryGetProperty(NDecimillipedeSegmentDriver.PropertyName._noise, ref variant5))
      this._noise = ((Variant) ref variant5).As<FastNoiseLite>();
    Variant variant6;
    if (info.TryGetProperty(NDecimillipedeSegmentDriver.PropertyName._time, ref variant6))
      this._time = ((Variant) ref variant6).As<float>();
    Variant variant7;
    if (info.TryGetProperty(NDecimillipedeSegmentDriver.PropertyName._decimillipedeStrikeOffset, ref variant7))
      this._decimillipedeStrikeOffset = ((Variant) ref variant7).As<Vector2>();
    Variant variant8;
    if (!info.TryGetProperty(NDecimillipedeSegmentDriver.PropertyName._attackTween, ref variant8))
      return;
    this._attackTween = ((Variant) ref variant8).As<Tween>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName AttackShake = StringName.op_Implicit(nameof (AttackShake));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _leftSegment = StringName.op_Implicit(nameof (_leftSegment));
    public static readonly StringName _speed = StringName.op_Implicit(nameof (_speed));
    public static readonly StringName _magnitude = StringName.op_Implicit(nameof (_magnitude));
    public static readonly StringName _originPos = StringName.op_Implicit(nameof (_originPos));
    public static readonly StringName _noise = StringName.op_Implicit(nameof (_noise));
    public static readonly StringName _time = StringName.op_Implicit(nameof (_time));
    public static readonly StringName _decimillipedeStrikeOffset = StringName.op_Implicit(nameof (_decimillipedeStrikeOffset));
    public static readonly StringName _attackTween = StringName.op_Implicit(nameof (_attackTween));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
