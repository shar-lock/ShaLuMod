// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NFgGroundSpikeVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NFgGroundSpikeVfx.cs")]
public class NFgGroundSpikeVfx : NBgGroundSpikeVfx
{
  private const string _scenePath = "res://scenes/vfx/fg_ground_spike_vfx.tscn";

  public static NFgGroundSpikeVfx? Create(Vector2 position, bool movingRight = true, VfxColor vfxColor = VfxColor.Red)
  {
    if (TestMode.IsOn)
      return (NFgGroundSpikeVfx) null;
    NFgGroundSpikeVfx nfgGroundSpikeVfx = PreloadManager.Cache.GetScene("res://scenes/vfx/fg_ground_spike_vfx.tscn").Instantiate<NFgGroundSpikeVfx>((PackedScene.GenEditState) 0L);
    nfgGroundSpikeVfx._startPosition = position;
    nfgGroundSpikeVfx._movingRight = movingRight;
    nfgGroundSpikeVfx._vfxColor = vfxColor;
    return nfgGroundSpikeVfx;
  }

  protected override void AdjustStartPosition()
  {
    this._startPosition = Vector2.op_Addition(this._startPosition, new Vector2(this._movingRight ? Rng.Chaotic.NextFloat(40f, 160f) : Rng.Chaotic.NextFloat(-160f, -40f), Rng.Chaotic.NextFloat(10f, 32f)));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NFgGroundSpikeVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Sprite2D"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("position"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("movingRight"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("vfxColor"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NFgGroundSpikeVfx.MethodName.AdjustStartPosition, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NFgGroundSpikeVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      NFgGroundSpikeVfx nfgGroundSpikeVfx = NFgGroundSpikeVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<VfxColor>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = VariantUtils.CreateFrom<NFgGroundSpikeVfx>(ref nfgGroundSpikeVfx);
      return true;
    }
    if (!StringName.op_Equality(ref method, NFgGroundSpikeVfx.MethodName.AdjustStartPosition) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.AdjustStartPosition();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NFgGroundSpikeVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      NFgGroundSpikeVfx nfgGroundSpikeVfx = NFgGroundSpikeVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<VfxColor>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = VariantUtils.CreateFrom<NFgGroundSpikeVfx>(ref nfgGroundSpikeVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NFgGroundSpikeVfx.MethodName.Create) || StringName.op_Equality(ref method, NFgGroundSpikeVfx.MethodName.AdjustStartPosition) || base.HasGodotClassMethod(in method);
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

  public new class MethodName : NBgGroundSpikeVfx.MethodName
  {
    public new static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public new static readonly StringName AdjustStartPosition = StringName.op_Implicit(nameof (AdjustStartPosition));
  }

  public new class PropertyName : NBgGroundSpikeVfx.PropertyName
  {
  }

  public new class SignalName : NBgGroundSpikeVfx.SignalName
  {
  }
}
