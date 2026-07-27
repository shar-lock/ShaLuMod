// Decompiled with JetBrains decompiler
// Type: NFrostOrbVfx
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

#nullable disable
[ScriptPath("res://src/Core/Nodes/Orbs/NFrostOrbVfx.cs")]
public class NFrostOrbVfx : NOrbVfx
{
  public override void OnPassiveActivated(Decimal passiveVal, Decimal evokeVal)
  {
    base.OnPassiveActivated(passiveVal, evokeVal);
    this.ShakeOrb(1f, 0.5f);
    VfxCmd.PlayVfx(this.GetPlayerVfxPosition(), "vfx/orbs/frost/vfx_frost_orb_passive_shield", this.VfxContainer);
  }

  protected override void OnEvokeInternal(Vector2 targetVfxSpawnPosition)
  {
    base.OnEvokeInternal(targetVfxSpawnPosition);
    VfxCmd.PlayVfx(this.GetPlayerVfxPosition(), "vfx/orbs/frost/vfx_frost_orb_evoke_shield", this.VfxContainer);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NFrostOrbVfx.MethodName.OnEvokeInternal, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
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
    if (!StringName.op_Equality(ref method, NFrostOrbVfx.MethodName.OnEvokeInternal) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnEvokeInternal(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NFrostOrbVfx.MethodName.OnEvokeInternal) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
  }

  public new class MethodName : NOrbVfx.MethodName
  {
    public new static readonly StringName OnEvokeInternal = StringName.op_Implicit(nameof (OnEvokeInternal));
  }

  public new class PropertyName : NOrbVfx.PropertyName
  {
  }

  public new class SignalName : NOrbVfx.SignalName
  {
  }
}
