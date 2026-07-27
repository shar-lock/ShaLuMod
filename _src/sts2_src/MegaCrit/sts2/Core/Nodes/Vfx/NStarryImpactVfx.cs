// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NStarryImpactVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NStarryImpactVfx.cs")]
public class NStarryImpactVfx : Node2D
{
  public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_starry_impact");
  [Export]
  private Array<GpuParticles2D> _particles = new Array<GpuParticles2D>();
  private CancellationTokenSource? _cts;

  public static NStarryImpactVfx? Create(Vector2 position)
  {
    if (TestMode.IsOn)
      return (NStarryImpactVfx) null;
    NStarryImpactVfx nstarryImpactVfx = PreloadManager.Cache.GetScene(NStarryImpactVfx.scenePath).Instantiate<NStarryImpactVfx>((PackedScene.GenEditState) 0L);
    nstarryImpactVfx.GlobalPosition = position;
    return nstarryImpactVfx;
  }

  public override void _Ready() => TaskHelper.RunSafely(this.PlaySequence());

  public override void _ExitTree() => this._cts?.Cancel();

  private async Task PlaySequence()
  {
    this._cts = new CancellationTokenSource();
    NGame.Instance?.ScreenShake(ShakeStrength.Weak, ShakeDuration.Short);
    for (int index = 0; index < this._particles.Count; ++index)
      this._particles[index].Restart();
    await Cmd.Wait(2f, this._cts.Token);
    ((Node) this).QueueFreeSafely();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NStarryImpactVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node2D"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("position"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NStarryImpactVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NStarryImpactVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NStarryImpactVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NStarryImpactVfx nstarryImpactVfx = NStarryImpactVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NStarryImpactVfx>(ref nstarryImpactVfx);
      return true;
    }
    if (StringName.op_Equality(ref method, NStarryImpactVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NStarryImpactVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NStarryImpactVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NStarryImpactVfx nstarryImpactVfx = NStarryImpactVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NStarryImpactVfx>(ref nstarryImpactVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NStarryImpactVfx.MethodName.Create) || StringName.op_Equality(ref method, NStarryImpactVfx.MethodName._Ready) || StringName.op_Equality(ref method, NStarryImpactVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NStarryImpactVfx.PropertyName._particles))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._particles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NStarryImpactVfx.PropertyName._particles))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFromArray<GpuParticles2D>(this._particles);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 28L, NStarryImpactVfx.PropertyName._particles, (PropertyHint) 23L, "24/34:GPUParticles2D", (PropertyUsageFlags) 4102L, true)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NStarryImpactVfx.PropertyName._particles, Variant.CreateFrom<GpuParticles2D>(this._particles));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(NStarryImpactVfx.PropertyName._particles, ref variant))
      return;
    this._particles = ((Variant) ref variant).AsGodotArray<GpuParticles2D>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _particles = StringName.op_Implicit(nameof (_particles));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
