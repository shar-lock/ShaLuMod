// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NHeavyBluntVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NHeavyBluntVfx.cs")]
public class NHeavyBluntVfx : Node2D
{
  public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_heavy_blunt");
  [Export]
  private Array<GpuParticles2D> _anticipationParticles = new Array<GpuParticles2D>();
  [Export]
  private Array<GpuParticles2D> _impactParticles = new Array<GpuParticles2D>();
  private Vector2 _debugPosition;

  public static NHeavyBluntVfx? Create(Vector2 debugPosition)
  {
    if (TestMode.IsOn)
      return (NHeavyBluntVfx) null;
    NHeavyBluntVfx nheavyBluntVfx = PreloadManager.Cache.GetScene(NHeavyBluntVfx.scenePath).Instantiate<NHeavyBluntVfx>((PackedScene.GenEditState) 0L);
    nheavyBluntVfx._debugPosition = debugPosition;
    return nheavyBluntVfx;
  }

  public override void _Ready() => TaskHelper.RunSafely(this.PlaySequence());

  private async Task PlaySequence()
  {
    this.GlobalPosition = this._debugPosition;
    for (int index = 0; index < this._anticipationParticles.Count; ++index)
      this._anticipationParticles[index].Restart();
    await this.WaitForSeconds(0.2f);
    NGame.Instance?.ScreenShake(ShakeStrength.Strong, ShakeDuration.Short);
    for (int index = 0; index < this._impactParticles.Count; ++index)
      this._impactParticles[index].Restart();
    await this.WaitForSeconds(2f);
    ((Node) this).QueueFreeSafely();
  }

  private async Task WaitForSeconds(float duration)
  {
    double timer = 0.0;
    while (timer < (double) duration)
    {
      timer += ((Node) this).GetProcessDeltaTime();
      double num = (double) await ((Node) this).AwaitProcessFrame();
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NHeavyBluntVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node2D"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("debugPosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NHeavyBluntVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NHeavyBluntVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NHeavyBluntVfx nheavyBluntVfx = NHeavyBluntVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NHeavyBluntVfx>(ref nheavyBluntVfx);
      return true;
    }
    if (!StringName.op_Equality(ref method, NHeavyBluntVfx.MethodName._Ready) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._Ready();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NHeavyBluntVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NHeavyBluntVfx nheavyBluntVfx = NHeavyBluntVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NHeavyBluntVfx>(ref nheavyBluntVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NHeavyBluntVfx.MethodName.Create) || StringName.op_Equality(ref method, NHeavyBluntVfx.MethodName._Ready) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NHeavyBluntVfx.PropertyName._anticipationParticles))
    {
      this._anticipationParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHeavyBluntVfx.PropertyName._impactParticles))
    {
      this._impactParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NHeavyBluntVfx.PropertyName._debugPosition))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._debugPosition = VariantUtils.ConvertTo<Vector2>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NHeavyBluntVfx.PropertyName._anticipationParticles))
    {
      value = VariantUtils.CreateFromArray<GpuParticles2D>(this._anticipationParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NHeavyBluntVfx.PropertyName._impactParticles))
    {
      value = VariantUtils.CreateFromArray<GpuParticles2D>(this._impactParticles);
      return true;
    }
    if (!StringName.op_Equality(ref name, NHeavyBluntVfx.PropertyName._debugPosition))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Vector2>(ref this._debugPosition);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 28L, NHeavyBluntVfx.PropertyName._anticipationParticles, (PropertyHint) 23L, "24/34:GPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 28L, NHeavyBluntVfx.PropertyName._impactParticles, (PropertyHint) 23L, "24/34:GPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 5L, NHeavyBluntVfx.PropertyName._debugPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NHeavyBluntVfx.PropertyName._anticipationParticles, Variant.CreateFrom<GpuParticles2D>(this._anticipationParticles));
    info.AddProperty(NHeavyBluntVfx.PropertyName._impactParticles, Variant.CreateFrom<GpuParticles2D>(this._impactParticles));
    info.AddProperty(NHeavyBluntVfx.PropertyName._debugPosition, Variant.From<Vector2>(ref this._debugPosition));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NHeavyBluntVfx.PropertyName._anticipationParticles, ref variant1))
      this._anticipationParticles = ((Variant) ref variant1).AsGodotArray<GpuParticles2D>();
    Variant variant2;
    if (info.TryGetProperty(NHeavyBluntVfx.PropertyName._impactParticles, ref variant2))
      this._impactParticles = ((Variant) ref variant2).AsGodotArray<GpuParticles2D>();
    Variant variant3;
    if (!info.TryGetProperty(NHeavyBluntVfx.PropertyName._debugPosition, ref variant3))
      return;
    this._debugPosition = ((Variant) ref variant3).As<Vector2>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _anticipationParticles = StringName.op_Implicit(nameof (_anticipationParticles));
    public static readonly StringName _impactParticles = StringName.op_Implicit(nameof (_impactParticles));
    public static readonly StringName _debugPosition = StringName.op_Implicit(nameof (_debugPosition));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
