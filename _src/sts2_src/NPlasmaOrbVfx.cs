// Decompiled with JetBrains decompiler
// Type: NPlasmaOrbVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
[ScriptPath("res://src/Core/Nodes/Orbs/NPlasmaOrbVfx.cs")]
public class NPlasmaOrbVfx : NOrbVfx
{
  [Export]
  private Vector2 _projectileOffsetRange;
  [Export]
  private float _projectileSpawnInterval = 0.05f;

  public override void OnPassiveActivated(Decimal passiveVal, Decimal evokeVal)
  {
    base.OnPassiveActivated(passiveVal, evokeVal);
    this.ShakeOrb(1f, 0.5f);
    TaskHelper.RunSafely(this.SpawnProjectile(1, this.GetPlayerVfxPosition()));
  }

  protected override void OnEvokeInternal(Vector2 targetVfxSpawnPosition)
  {
    base.OnEvokeInternal(targetVfxSpawnPosition);
    TaskHelper.RunSafely(this.SpawnProjectile(2, this.GetPlayerVfxPosition()));
  }

  private async Task SpawnProjectile(int count, Vector2 targetPosition)
  {
    for (int i = 0; i < count; ++i)
    {
      ((Node) this.VfxContainer).AddChildSafely((Node) NVfxProjectileHandler.Create("vfx/orbs/plasma/vfx_plasma_orb_projectile_handler", "vfx/orbs/plasma/vfx_plasma_orb_projectile", Vector2.op_Addition(this.GlobalPosition, this.GetRandomOffset()), targetPosition, i == count - 1 ? Callable.From((Action) (() => { })) : new Callable()));
      if (i != count - 1)
        await Cmd.Wait(this._projectileSpawnInterval);
    }
  }

  private Vector2 GetRandomOffset()
  {
    float rad = Mathf.DegToRad(GD.Randf() * 360f);
    float num = Mathf.Lerp(this._projectileOffsetRange.X, this._projectileOffsetRange.Y, GD.Randf());
    return new Vector2(Mathf.Cos(rad) * num, Mathf.Sin(rad) * num);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NPlasmaOrbVfx.MethodName.OnEvokeInternal, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("targetVfxSpawnPosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NPlasmaOrbVfx.MethodName.GetRandomOffset, new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NPlasmaOrbVfx.MethodName.OnEvokeInternal) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnEvokeInternal(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NPlasmaOrbVfx.MethodName.GetRandomOffset) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    Vector2 randomOffset = this.GetRandomOffset();
    ret = VariantUtils.CreateFrom<Vector2>(ref randomOffset);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NPlasmaOrbVfx.MethodName.OnEvokeInternal) || StringName.op_Equality(ref method, NPlasmaOrbVfx.MethodName.GetRandomOffset) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPlasmaOrbVfx.PropertyName._projectileOffsetRange))
    {
      this._projectileOffsetRange = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPlasmaOrbVfx.PropertyName._projectileSpawnInterval))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._projectileSpawnInterval = VariantUtils.ConvertTo<float>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPlasmaOrbVfx.PropertyName._projectileOffsetRange))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._projectileOffsetRange);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPlasmaOrbVfx.PropertyName._projectileSpawnInterval))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<float>(ref this._projectileSpawnInterval);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 5L, NPlasmaOrbVfx.PropertyName._projectileOffsetRange, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NPlasmaOrbVfx.PropertyName._projectileSpawnInterval, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NPlasmaOrbVfx.PropertyName._projectileOffsetRange, Variant.From<Vector2>(ref this._projectileOffsetRange));
    info.AddProperty(NPlasmaOrbVfx.PropertyName._projectileSpawnInterval, Variant.From<float>(ref this._projectileSpawnInterval));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NPlasmaOrbVfx.PropertyName._projectileOffsetRange, ref variant1))
      this._projectileOffsetRange = ((Variant) ref variant1).As<Vector2>();
    Variant variant2;
    if (!info.TryGetProperty(NPlasmaOrbVfx.PropertyName._projectileSpawnInterval, ref variant2))
      return;
    this._projectileSpawnInterval = ((Variant) ref variant2).As<float>();
  }

  public new class MethodName : NOrbVfx.MethodName
  {
    public new static readonly StringName OnEvokeInternal = StringName.op_Implicit(nameof (OnEvokeInternal));
    public static readonly StringName GetRandomOffset = StringName.op_Implicit(nameof (GetRandomOffset));
  }

  public new class PropertyName : NOrbVfx.PropertyName
  {
    public static readonly StringName _projectileOffsetRange = StringName.op_Implicit(nameof (_projectileOffsetRange));
    public static readonly StringName _projectileSpawnInterval = StringName.op_Implicit(nameof (_projectileSpawnInterval));
  }

  public new class SignalName : NOrbVfx.SignalName
  {
  }
}
