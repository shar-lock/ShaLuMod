// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.Utilities.NScreenShake
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;

[ScriptPath("res://src/Core/Nodes/Vfx/Utilities/NScreenShake.cs")]
public class NScreenShake : Node
{
  private Vector2 _originalTargetPosition;
  private ScreenPunchInstance? _shakeInstance;
  private ScreenRumbleInstance? _rumbleInstance;
  private ScreenTraumaRumble _traumaRumble;
  private float _multiplier;
  private readonly Dictionary<ShakeStrength, float> _strength = new Dictionary<ShakeStrength, float>();
  private readonly Dictionary<ShakeDuration, double> _duration = new Dictionary<ShakeDuration, double>();

  public Control? ShakeTarget { get; private set; }

  public override void _Ready()
  {
    this._traumaRumble = new ScreenTraumaRumble();
    this._strength.Add(ShakeStrength.VeryWeak, 2f);
    this._strength.Add(ShakeStrength.Weak, 5f);
    this._strength.Add(ShakeStrength.Medium, 20f);
    this._strength.Add(ShakeStrength.Strong, 40f);
    this._strength.Add(ShakeStrength.TooMuch, 80f);
    this._duration.Add(ShakeDuration.Short, 0.3);
    this._duration.Add(ShakeDuration.Normal, 0.8);
    this._duration.Add(ShakeDuration.Long, 1.2);
    this._duration.Add(ShakeDuration.Forever, 999999999.0);
  }

  public void SetTarget(Control targetScreen)
  {
    this.ShakeTarget = targetScreen;
    this._originalTargetPosition = targetScreen.Position;
  }

  public override void _Process(double delta)
  {
    Vector2 vector2_1 = Vector2.Zero;
    if (this._rumbleInstance != null)
    {
      vector2_1 = this._rumbleInstance.Update(delta);
      if (this._rumbleInstance.IsDone)
        this._rumbleInstance = (ScreenRumbleInstance) null;
    }
    if (this._shakeInstance != null)
    {
      vector2_1 = this._shakeInstance.Update(delta);
      if (this._shakeInstance.IsDone)
        this._shakeInstance = (ScreenPunchInstance) null;
    }
    Vector2 vector2_2 = Vector2.op_Addition(vector2_1, this._traumaRumble.Update(delta));
    if (this.ShakeTarget == null || !((Node) this.ShakeTarget).IsValid())
      return;
    this.ShakeTarget.Position = Vector2.op_Addition(this._originalTargetPosition, vector2_2);
  }

  public void Shake(ShakeStrength strength, ShakeDuration duration, float degAngle)
  {
    if (this.ShakeTarget == null)
      Log.Error("Missing screenShake target!");
    else
      this._shakeInstance = new ScreenPunchInstance(this._strength[strength] * this._multiplier, this._duration[duration], degAngle);
  }

  public void Rumble(ShakeStrength strength, ShakeDuration duration, RumbleStyle style)
  {
    if (this.ShakeTarget == null)
      Log.Error("Missing screenShake target!");
    else
      this._rumbleInstance = new ScreenRumbleInstance(this._strength[strength] * this._multiplier, this._duration[duration], 1f, style);
  }

  public void AddTrauma(ShakeStrength strength) => this._traumaRumble.AddTrauma(strength);

  public void ClearTarget()
  {
    this.ShakeTarget = (Control) null;
    this._shakeInstance = (ScreenPunchInstance) null;
    this._rumbleInstance = (ScreenRumbleInstance) null;
  }

  private void StopRumble() => this._rumbleInstance = (ScreenRumbleInstance) null;

  public void SetMultiplier(float multiplier)
  {
    this._multiplier = multiplier;
    this._traumaRumble.SetMultiplier(multiplier);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(9)
    {
      new MethodInfo(NScreenShake.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NScreenShake.MethodName.SetTarget, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("targetScreen"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NScreenShake.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NScreenShake.MethodName.Shake, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("strength"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("duration"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("degAngle"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NScreenShake.MethodName.Rumble, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("strength"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("duration"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("style"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NScreenShake.MethodName.AddTrauma, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("strength"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NScreenShake.MethodName.ClearTarget, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NScreenShake.MethodName.StopRumble, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NScreenShake.MethodName.SetMultiplier, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("multiplier"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NScreenShake.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NScreenShake.MethodName.SetTarget) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetTarget(VariantUtils.ConvertTo<Control>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NScreenShake.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      base._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NScreenShake.MethodName.Shake) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      this.Shake(VariantUtils.ConvertTo<ShakeStrength>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<ShakeDuration>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NScreenShake.MethodName.Rumble) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      this.Rumble(VariantUtils.ConvertTo<ShakeStrength>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<ShakeDuration>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<RumbleStyle>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NScreenShake.MethodName.AddTrauma) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.AddTrauma(VariantUtils.ConvertTo<ShakeStrength>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NScreenShake.MethodName.ClearTarget) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ClearTarget();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NScreenShake.MethodName.StopRumble) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StopRumble();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NScreenShake.MethodName.SetMultiplier) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.SetMultiplier(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NScreenShake.MethodName._Ready) || StringName.op_Equality(ref method, NScreenShake.MethodName.SetTarget) || StringName.op_Equality(ref method, NScreenShake.MethodName._Process) || StringName.op_Equality(ref method, NScreenShake.MethodName.Shake) || StringName.op_Equality(ref method, NScreenShake.MethodName.Rumble) || StringName.op_Equality(ref method, NScreenShake.MethodName.AddTrauma) || StringName.op_Equality(ref method, NScreenShake.MethodName.ClearTarget) || StringName.op_Equality(ref method, NScreenShake.MethodName.StopRumble) || StringName.op_Equality(ref method, NScreenShake.MethodName.SetMultiplier) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NScreenShake.PropertyName.ShakeTarget))
    {
      this.ShakeTarget = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NScreenShake.PropertyName._originalTargetPosition))
    {
      this._originalTargetPosition = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NScreenShake.PropertyName._multiplier))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._multiplier = VariantUtils.ConvertTo<float>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NScreenShake.PropertyName.ShakeTarget))
    {
      ref godot_variant local = ref value;
      Control shakeTarget = this.ShakeTarget;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref shakeTarget);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NScreenShake.PropertyName._originalTargetPosition))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._originalTargetPosition);
      return true;
    }
    if (!StringName.op_Equality(ref name, NScreenShake.PropertyName._multiplier))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<float>(ref this._multiplier);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 5L, NScreenShake.PropertyName._originalTargetPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NScreenShake.PropertyName._multiplier, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NScreenShake.PropertyName.ShakeTarget, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName shakeTarget1 = NScreenShake.PropertyName.ShakeTarget;
    Control shakeTarget2 = this.ShakeTarget;
    Variant variant = Variant.From<Control>(ref shakeTarget2);
    serializationInfo.AddProperty(shakeTarget1, variant);
    info.AddProperty(NScreenShake.PropertyName._originalTargetPosition, Variant.From<Vector2>(ref this._originalTargetPosition));
    info.AddProperty(NScreenShake.PropertyName._multiplier, Variant.From<float>(ref this._multiplier));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NScreenShake.PropertyName.ShakeTarget, ref variant1))
      this.ShakeTarget = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NScreenShake.PropertyName._originalTargetPosition, ref variant2))
      this._originalTargetPosition = ((Variant) ref variant2).As<Vector2>();
    Variant variant3;
    if (!info.TryGetProperty(NScreenShake.PropertyName._multiplier, ref variant3))
      return;
    this._multiplier = ((Variant) ref variant3).As<float>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName SetTarget = StringName.op_Implicit(nameof (SetTarget));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName Shake = StringName.op_Implicit(nameof (Shake));
    public static readonly StringName Rumble = StringName.op_Implicit(nameof (Rumble));
    public static readonly StringName AddTrauma = StringName.op_Implicit(nameof (AddTrauma));
    public static readonly StringName ClearTarget = StringName.op_Implicit(nameof (ClearTarget));
    public static readonly StringName StopRumble = StringName.op_Implicit(nameof (StopRumble));
    public static readonly StringName SetMultiplier = StringName.op_Implicit(nameof (SetMultiplier));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName ShakeTarget = StringName.op_Implicit(nameof (ShakeTarget));
    public static readonly StringName _originalTargetPosition = StringName.op_Implicit(nameof (_originalTargetPosition));
    public static readonly StringName _multiplier = StringName.op_Implicit(nameof (_multiplier));
  }

  public class SignalName : Node.SignalName
  {
  }
}
