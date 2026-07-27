// Decompiled with JetBrains decompiler
// Type: NDarkOrbVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
[ScriptPath("res://src/Core/Nodes/Orbs/NDarkOrbVfx.cs")]
public class NDarkOrbVfx : NOrbVfx
{
  [Export]
  private NParticlesContainer? _superchargedParticles;
  [Export]
  private Node2D _darkBg;
  [Export]
  private float _superchargeThreshold = 24f;
  private float _darkBgNormalScale = 1f;
  private float _darkBgSuperchargedScale = 1.2f;

  public override void OnPassiveActivated(Decimal passiveVal, Decimal evokeVal)
  {
    base.OnPassiveActivated(passiveVal, evokeVal);
    bool flag = (double) (float) evokeVal >= (double) this._superchargeThreshold;
    if (this._superchargedParticles != null)
      this._superchargedParticles.SetEmitting(flag);
    this.UpdateDarkBgSize(flag);
    this.ShakeOrb(this.HasFocusPower() ? 1f : 0.65f, 0.55f);
  }

  private void UpdateDarkBgSize(bool isSupercharged)
  {
    float num = isSupercharged ? this._darkBgSuperchargedScale : this._darkBgNormalScale;
    if (Mathf.IsEqualApprox(this._darkBg.Scale.X, num))
      return;
    ((Node) this).GetTree().CreateTween().TweenProperty((GodotObject) this._darkBg, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, num)), 0.25);
  }

  protected override void OnEvokeInternal(Vector2 targetVfxSpawnPosition)
  {
    base.OnEvokeInternal(targetVfxSpawnPosition);
    ((Node) this.VfxContainer).AddChildSafely((Node) NVfxProjectileHandler.Create("vfx/orbs/dark/vfx_dark_orb_evoke_projectile_handler", "vfx/orbs/dark/vfx_dark_orb_evoke_projectile", this.GlobalPosition, targetVfxSpawnPosition, new Callable()));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NDarkOrbVfx.MethodName.UpdateDarkBgSize, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("isSupercharged"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NDarkOrbVfx.MethodName.OnEvokeInternal, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NDarkOrbVfx.MethodName.UpdateDarkBgSize) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateDarkBgSize(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NDarkOrbVfx.MethodName.OnEvokeInternal) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnEvokeInternal(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NDarkOrbVfx.MethodName.UpdateDarkBgSize) || StringName.op_Equality(ref method, NDarkOrbVfx.MethodName.OnEvokeInternal) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDarkOrbVfx.PropertyName._superchargedParticles))
    {
      this._superchargedParticles = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDarkOrbVfx.PropertyName._darkBg))
    {
      this._darkBg = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDarkOrbVfx.PropertyName._superchargeThreshold))
    {
      this._superchargeThreshold = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDarkOrbVfx.PropertyName._darkBgNormalScale))
    {
      this._darkBgNormalScale = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDarkOrbVfx.PropertyName._darkBgSuperchargedScale))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._darkBgSuperchargedScale = VariantUtils.ConvertTo<float>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDarkOrbVfx.PropertyName._superchargedParticles))
    {
      value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._superchargedParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NDarkOrbVfx.PropertyName._darkBg))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._darkBg);
      return true;
    }
    if (StringName.op_Equality(ref name, NDarkOrbVfx.PropertyName._superchargeThreshold))
    {
      value = VariantUtils.CreateFrom<float>(ref this._superchargeThreshold);
      return true;
    }
    if (StringName.op_Equality(ref name, NDarkOrbVfx.PropertyName._darkBgNormalScale))
    {
      value = VariantUtils.CreateFrom<float>(ref this._darkBgNormalScale);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDarkOrbVfx.PropertyName._darkBgSuperchargedScale))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<float>(ref this._darkBgSuperchargedScale);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NDarkOrbVfx.PropertyName._superchargedParticles, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NDarkOrbVfx.PropertyName._darkBg, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NDarkOrbVfx.PropertyName._superchargeThreshold, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NDarkOrbVfx.PropertyName._darkBgNormalScale, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NDarkOrbVfx.PropertyName._darkBgSuperchargedScale, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NDarkOrbVfx.PropertyName._superchargedParticles, Variant.From<NParticlesContainer>(ref this._superchargedParticles));
    info.AddProperty(NDarkOrbVfx.PropertyName._darkBg, Variant.From<Node2D>(ref this._darkBg));
    info.AddProperty(NDarkOrbVfx.PropertyName._superchargeThreshold, Variant.From<float>(ref this._superchargeThreshold));
    info.AddProperty(NDarkOrbVfx.PropertyName._darkBgNormalScale, Variant.From<float>(ref this._darkBgNormalScale));
    info.AddProperty(NDarkOrbVfx.PropertyName._darkBgSuperchargedScale, Variant.From<float>(ref this._darkBgSuperchargedScale));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NDarkOrbVfx.PropertyName._superchargedParticles, ref variant1))
      this._superchargedParticles = ((Variant) ref variant1).As<NParticlesContainer>();
    Variant variant2;
    if (info.TryGetProperty(NDarkOrbVfx.PropertyName._darkBg, ref variant2))
      this._darkBg = ((Variant) ref variant2).As<Node2D>();
    Variant variant3;
    if (info.TryGetProperty(NDarkOrbVfx.PropertyName._superchargeThreshold, ref variant3))
      this._superchargeThreshold = ((Variant) ref variant3).As<float>();
    Variant variant4;
    if (info.TryGetProperty(NDarkOrbVfx.PropertyName._darkBgNormalScale, ref variant4))
      this._darkBgNormalScale = ((Variant) ref variant4).As<float>();
    Variant variant5;
    if (!info.TryGetProperty(NDarkOrbVfx.PropertyName._darkBgSuperchargedScale, ref variant5))
      return;
    this._darkBgSuperchargedScale = ((Variant) ref variant5).As<float>();
  }

  public new class MethodName : NOrbVfx.MethodName
  {
    public static readonly StringName UpdateDarkBgSize = StringName.op_Implicit(nameof (UpdateDarkBgSize));
    public new static readonly StringName OnEvokeInternal = StringName.op_Implicit(nameof (OnEvokeInternal));
  }

  public new class PropertyName : NOrbVfx.PropertyName
  {
    public static readonly StringName _superchargedParticles = StringName.op_Implicit(nameof (_superchargedParticles));
    public static readonly StringName _darkBg = StringName.op_Implicit(nameof (_darkBg));
    public static readonly StringName _superchargeThreshold = StringName.op_Implicit(nameof (_superchargeThreshold));
    public static readonly StringName _darkBgNormalScale = StringName.op_Implicit(nameof (_darkBgNormalScale));
    public static readonly StringName _darkBgSuperchargedScale = StringName.op_Implicit(nameof (_darkBgSuperchargedScale));
  }

  public new class SignalName : NOrbVfx.SignalName
  {
  }
}
