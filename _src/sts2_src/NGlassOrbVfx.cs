// Decompiled with JetBrains decompiler
// Type: NGlassOrbVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
[ScriptPath("res://src/Core/Nodes/Orbs/NGlassOrbVfx.cs")]
public class NGlassOrbVfx : NOrbVfx
{
  [Export]
  private GpuParticles2D _passiveChromaticAberration;
  [Export]
  private float _basePassiveChromaticAberrationStength = 0.01f;
  private Decimal _basePassiveVal = 4M;
  private static readonly StringName _aberrationStrengthString = new StringName("instance_shader_parameters/base_intensity");

  public override void OnPassiveActivated(Decimal passiveVal, Decimal evokeVal)
  {
    if (passiveVal <= 0M)
      return;
    base.OnPassiveActivated(passiveVal, evokeVal);
    float initialStrength = (float) (passiveVal / this._basePassiveVal);
    ((GodotObject) this._passiveChromaticAberration).Set(NGlassOrbVfx._aberrationStrengthString, Variant.op_Implicit(Mathf.Lerp(0.0f, this._basePassiveChromaticAberrationStength, initialStrength)));
    this.ShakeOrb(initialStrength, 0.5f);
  }

  public override void AfterPassiveActivated(Decimal passiveVal, Decimal evokeVal)
  {
    base.AfterPassiveActivated(passiveVal, evokeVal);
    this.UpdateFocusPowerState();
  }

  protected override bool HasFocusPower()
  {
    return (this._orbModel == null || !(this._orbModel.PassiveVal <= 0M)) && base.HasFocusPower();
  }

  public void ShowPassiveImpact(Vector2[] targetVfxSpawnPositions)
  {
    for (int index = 0; index < targetVfxSpawnPositions.Length; ++index)
      this.ShowPassiveImpact(targetVfxSpawnPositions[index]);
  }

  private void ShowPassiveImpact(Vector2 targetVfxSpawnPosition)
  {
    VfxCmd.PlayVfx(targetVfxSpawnPosition, "vfx/orbs/glass/vfx_glass_orb_passive_impact", this.VfxContainer);
  }

  protected override void OnEvokeInternal(Vector2 targetVfxSpawnPosition)
  {
    base.OnEvokeInternal(targetVfxSpawnPosition);
    VfxCmd.PlayVfx(targetVfxSpawnPosition, "vfx/orbs/glass/vfx_glass_orb_evoke_impact", this.VfxContainer);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NGlassOrbVfx.MethodName.HasFocusPower, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGlassOrbVfx.MethodName.ShowPassiveImpact, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 35L, StringName.op_Implicit("targetVfxSpawnPositions"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NGlassOrbVfx.MethodName.OnEvokeInternal, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("targetVfxSpawnPosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NGlassOrbVfx.MethodName.HasFocusPower) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      bool flag = this.HasFocusPower();
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    if (StringName.op_Equality(ref method, NGlassOrbVfx.MethodName.ShowPassiveImpact) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ShowPassiveImpact(VariantUtils.ConvertTo<Vector2[]>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NGlassOrbVfx.MethodName.OnEvokeInternal) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnEvokeInternal(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NGlassOrbVfx.MethodName.HasFocusPower) || StringName.op_Equality(ref method, NGlassOrbVfx.MethodName.ShowPassiveImpact) || StringName.op_Equality(ref method, NGlassOrbVfx.MethodName.OnEvokeInternal) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NGlassOrbVfx.PropertyName._passiveChromaticAberration))
    {
      this._passiveChromaticAberration = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NGlassOrbVfx.PropertyName._basePassiveChromaticAberrationStength))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._basePassiveChromaticAberrationStength = VariantUtils.ConvertTo<float>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NGlassOrbVfx.PropertyName._passiveChromaticAberration))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._passiveChromaticAberration);
      return true;
    }
    if (!StringName.op_Equality(ref name, NGlassOrbVfx.PropertyName._basePassiveChromaticAberrationStength))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<float>(ref this._basePassiveChromaticAberrationStength);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NGlassOrbVfx.PropertyName._passiveChromaticAberration, (PropertyHint) 34L, "GPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NGlassOrbVfx.PropertyName._basePassiveChromaticAberrationStength, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NGlassOrbVfx.PropertyName._passiveChromaticAberration, Variant.From<GpuParticles2D>(ref this._passiveChromaticAberration));
    info.AddProperty(NGlassOrbVfx.PropertyName._basePassiveChromaticAberrationStength, Variant.From<float>(ref this._basePassiveChromaticAberrationStength));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NGlassOrbVfx.PropertyName._passiveChromaticAberration, ref variant1))
      this._passiveChromaticAberration = ((Variant) ref variant1).As<GpuParticles2D>();
    Variant variant2;
    if (!info.TryGetProperty(NGlassOrbVfx.PropertyName._basePassiveChromaticAberrationStength, ref variant2))
      return;
    this._basePassiveChromaticAberrationStength = ((Variant) ref variant2).As<float>();
  }

  public new class MethodName : NOrbVfx.MethodName
  {
    public new static readonly StringName HasFocusPower = StringName.op_Implicit(nameof (HasFocusPower));
    public static readonly StringName ShowPassiveImpact = StringName.op_Implicit(nameof (ShowPassiveImpact));
    public new static readonly StringName OnEvokeInternal = StringName.op_Implicit(nameof (OnEvokeInternal));
  }

  public new class PropertyName : NOrbVfx.PropertyName
  {
    public static readonly StringName _passiveChromaticAberration = StringName.op_Implicit(nameof (_passiveChromaticAberration));
    public static readonly StringName _basePassiveChromaticAberrationStength = StringName.op_Implicit(nameof (_basePassiveChromaticAberrationStength));
  }

  public new class SignalName : NOrbVfx.SignalName
  {
  }
}
