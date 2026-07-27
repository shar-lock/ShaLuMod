// Decompiled with JetBrains decompiler
// Type: NShaker
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
[ScriptPath("res://src/Core/Nodes/Vfx/Utilities/NShaker.cs")]
public class NShaker : Node
{
  [Export]
  private Node2D? _target;
  [Export]
  private float _maxPosOffset;
  [Export]
  private float _maxRotOffset;
  [Export]
  private float _frequency;
  [Export]
  private float _strength;
  private float _timer;
  private Vector2 _previousShakePos;
  private float _previousShakeRot;
  private Vector2 _shakePos;
  private float _shakeRot;

  public float Strength
  {
    get => this._strength;
    set => this._strength = value;
  }

  public override void _Ready()
  {
    base._Ready();
    this._timer = 0.0f;
    this._previousShakePos = Vector2.Zero;
    this._previousShakeRot = 0.0f;
    this._shakePos = Vector2.Zero;
    this._shakeRot = 0.0f;
  }

  public override void _Process(double delta)
  {
    base._Process(delta);
    if (this._target == null)
      return;
    if ((double) this._strength == 0.0)
    {
      this._target.Position = Vector2.Zero;
      this._target.RotationDegrees = 0.0f;
    }
    else if ((double) this._timer > 0.0)
    {
      this._timer = Mathf.Clamp(this._timer - (float) delta, 0.0f, this._frequency);
      float num = (float) (1.0 - (double) this._timer / (double) this._frequency);
      this.SetTargetTransform(((Vector2) ref this._previousShakePos).Lerp(this._shakePos, num), Mathf.Lerp(this._previousShakeRot, this._shakeRot, num));
    }
    else
    {
      this._timer = this._frequency;
      this._previousShakePos = this._shakePos;
      this._previousShakeRot = this._shakeRot;
      float rad = Mathf.DegToRad(GD.Randf() * 360f);
      float num = GD.Randf() * this._maxPosOffset;
      Vector2 vector2 = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
      this._shakePos = Vector2.op_Multiply(Vector2.op_Multiply(((Vector2) ref vector2).Normalized(), num), this._strength);
      this._shakeRot = (float) GD.RandRange(-(double) this._maxRotOffset, (double) this._maxRotOffset);
      this.SetTargetTransform(this._shakePos, this._shakeRot);
    }
  }

  private void SetTargetTransform(Vector2 position, float rotation)
  {
    if (this._target == null)
      return;
    this._target.Position = position;
    this._target.RotationDegrees = rotation;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NShaker.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NShaker.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NShaker.MethodName.SetTargetTransform, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("position"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("rotation"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NShaker.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NShaker.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      base._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NShaker.MethodName.SetTargetTransform) || ((NativeVariantPtrArgs) ref args).Count != 2)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.SetTargetTransform(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[1]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NShaker.MethodName._Ready) || StringName.op_Equality(ref method, NShaker.MethodName._Process) || StringName.op_Equality(ref method, NShaker.MethodName.SetTargetTransform) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NShaker.PropertyName.Strength))
    {
      this.Strength = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NShaker.PropertyName._target))
    {
      this._target = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NShaker.PropertyName._maxPosOffset))
    {
      this._maxPosOffset = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NShaker.PropertyName._maxRotOffset))
    {
      this._maxRotOffset = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NShaker.PropertyName._frequency))
    {
      this._frequency = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NShaker.PropertyName._strength))
    {
      this._strength = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NShaker.PropertyName._timer))
    {
      this._timer = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NShaker.PropertyName._previousShakePos))
    {
      this._previousShakePos = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NShaker.PropertyName._previousShakeRot))
    {
      this._previousShakeRot = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NShaker.PropertyName._shakePos))
    {
      this._shakePos = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NShaker.PropertyName._shakeRot))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._shakeRot = VariantUtils.ConvertTo<float>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NShaker.PropertyName.Strength))
    {
      ref godot_variant local = ref value;
      float strength = this.Strength;
      godot_variant from = VariantUtils.CreateFrom<float>(ref strength);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NShaker.PropertyName._target))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._target);
      return true;
    }
    if (StringName.op_Equality(ref name, NShaker.PropertyName._maxPosOffset))
    {
      value = VariantUtils.CreateFrom<float>(ref this._maxPosOffset);
      return true;
    }
    if (StringName.op_Equality(ref name, NShaker.PropertyName._maxRotOffset))
    {
      value = VariantUtils.CreateFrom<float>(ref this._maxRotOffset);
      return true;
    }
    if (StringName.op_Equality(ref name, NShaker.PropertyName._frequency))
    {
      value = VariantUtils.CreateFrom<float>(ref this._frequency);
      return true;
    }
    if (StringName.op_Equality(ref name, NShaker.PropertyName._strength))
    {
      value = VariantUtils.CreateFrom<float>(ref this._strength);
      return true;
    }
    if (StringName.op_Equality(ref name, NShaker.PropertyName._timer))
    {
      value = VariantUtils.CreateFrom<float>(ref this._timer);
      return true;
    }
    if (StringName.op_Equality(ref name, NShaker.PropertyName._previousShakePos))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._previousShakePos);
      return true;
    }
    if (StringName.op_Equality(ref name, NShaker.PropertyName._previousShakeRot))
    {
      value = VariantUtils.CreateFrom<float>(ref this._previousShakeRot);
      return true;
    }
    if (StringName.op_Equality(ref name, NShaker.PropertyName._shakePos))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._shakePos);
      return true;
    }
    if (!StringName.op_Equality(ref name, NShaker.PropertyName._shakeRot))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<float>(ref this._shakeRot);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NShaker.PropertyName._target, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NShaker.PropertyName._maxPosOffset, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NShaker.PropertyName._maxRotOffset, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NShaker.PropertyName._frequency, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NShaker.PropertyName._strength, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NShaker.PropertyName._timer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NShaker.PropertyName._previousShakePos, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NShaker.PropertyName._previousShakeRot, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NShaker.PropertyName._shakePos, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NShaker.PropertyName._shakeRot, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NShaker.PropertyName.Strength, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName strength1 = NShaker.PropertyName.Strength;
    float strength2 = this.Strength;
    Variant variant = Variant.From<float>(ref strength2);
    serializationInfo.AddProperty(strength1, variant);
    info.AddProperty(NShaker.PropertyName._target, Variant.From<Node2D>(ref this._target));
    info.AddProperty(NShaker.PropertyName._maxPosOffset, Variant.From<float>(ref this._maxPosOffset));
    info.AddProperty(NShaker.PropertyName._maxRotOffset, Variant.From<float>(ref this._maxRotOffset));
    info.AddProperty(NShaker.PropertyName._frequency, Variant.From<float>(ref this._frequency));
    info.AddProperty(NShaker.PropertyName._strength, Variant.From<float>(ref this._strength));
    info.AddProperty(NShaker.PropertyName._timer, Variant.From<float>(ref this._timer));
    info.AddProperty(NShaker.PropertyName._previousShakePos, Variant.From<Vector2>(ref this._previousShakePos));
    info.AddProperty(NShaker.PropertyName._previousShakeRot, Variant.From<float>(ref this._previousShakeRot));
    info.AddProperty(NShaker.PropertyName._shakePos, Variant.From<Vector2>(ref this._shakePos));
    info.AddProperty(NShaker.PropertyName._shakeRot, Variant.From<float>(ref this._shakeRot));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NShaker.PropertyName.Strength, ref variant1))
      this.Strength = ((Variant) ref variant1).As<float>();
    Variant variant2;
    if (info.TryGetProperty(NShaker.PropertyName._target, ref variant2))
      this._target = ((Variant) ref variant2).As<Node2D>();
    Variant variant3;
    if (info.TryGetProperty(NShaker.PropertyName._maxPosOffset, ref variant3))
      this._maxPosOffset = ((Variant) ref variant3).As<float>();
    Variant variant4;
    if (info.TryGetProperty(NShaker.PropertyName._maxRotOffset, ref variant4))
      this._maxRotOffset = ((Variant) ref variant4).As<float>();
    Variant variant5;
    if (info.TryGetProperty(NShaker.PropertyName._frequency, ref variant5))
      this._frequency = ((Variant) ref variant5).As<float>();
    Variant variant6;
    if (info.TryGetProperty(NShaker.PropertyName._strength, ref variant6))
      this._strength = ((Variant) ref variant6).As<float>();
    Variant variant7;
    if (info.TryGetProperty(NShaker.PropertyName._timer, ref variant7))
      this._timer = ((Variant) ref variant7).As<float>();
    Variant variant8;
    if (info.TryGetProperty(NShaker.PropertyName._previousShakePos, ref variant8))
      this._previousShakePos = ((Variant) ref variant8).As<Vector2>();
    Variant variant9;
    if (info.TryGetProperty(NShaker.PropertyName._previousShakeRot, ref variant9))
      this._previousShakeRot = ((Variant) ref variant9).As<float>();
    Variant variant10;
    if (info.TryGetProperty(NShaker.PropertyName._shakePos, ref variant10))
      this._shakePos = ((Variant) ref variant10).As<Vector2>();
    Variant variant11;
    if (!info.TryGetProperty(NShaker.PropertyName._shakeRot, ref variant11))
      return;
    this._shakeRot = ((Variant) ref variant11).As<float>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName SetTargetTransform = StringName.op_Implicit(nameof (SetTargetTransform));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName Strength = StringName.op_Implicit(nameof (Strength));
    public static readonly StringName _target = StringName.op_Implicit(nameof (_target));
    public static readonly StringName _maxPosOffset = StringName.op_Implicit(nameof (_maxPosOffset));
    public static readonly StringName _maxRotOffset = StringName.op_Implicit(nameof (_maxRotOffset));
    public static readonly StringName _frequency = StringName.op_Implicit(nameof (_frequency));
    public static readonly StringName _strength = StringName.op_Implicit(nameof (_strength));
    public static readonly StringName _timer = StringName.op_Implicit(nameof (_timer));
    public static readonly StringName _previousShakePos = StringName.op_Implicit(nameof (_previousShakePos));
    public static readonly StringName _previousShakeRot = StringName.op_Implicit(nameof (_previousShakeRot));
    public static readonly StringName _shakePos = StringName.op_Implicit(nameof (_shakePos));
    public static readonly StringName _shakeRot = StringName.op_Implicit(nameof (_shakeRot));
  }

  public class SignalName : Node.SignalName
  {
  }
}
