// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NSpookyScreamVfx
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
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NSpookyScreamVfx.cs")]
public class NSpookyScreamVfx : Node2D
{
  public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_spooky_scream");
  [Export]
  private Array<GpuParticles2D> _continuousParticles = new Array<GpuParticles2D>();
  [Export]
  private Array<GpuParticles2D> _oneShotParticles = new Array<GpuParticles2D>();
  [Export]
  private float _duration = 1f;
  private CancellationTokenSource? _cts;

  public static NSpookyScreamVfx? Create(Vector2 position)
  {
    if (TestMode.IsOn)
      return (NSpookyScreamVfx) null;
    NSpookyScreamVfx nspookyScreamVfx = PreloadManager.Cache.GetScene(NSpookyScreamVfx.scenePath).Instantiate<NSpookyScreamVfx>((PackedScene.GenEditState) 0L);
    nspookyScreamVfx.GlobalPosition = position;
    return nspookyScreamVfx;
  }

  public override void _Ready() => TaskHelper.RunSafely(this.PlaySequence());

  public override void _ExitTree() => this._cts?.Cancel();

  private async Task PlaySequence()
  {
    this._cts = new CancellationTokenSource();
    for (int index = 0; index < this._oneShotParticles.Count; ++index)
    {
      this._oneShotParticles[index].Lifetime = (double) this._duration;
      this._oneShotParticles[index].Restart();
    }
    for (int index = 0; index < this._continuousParticles.Count; ++index)
    {
      this._continuousParticles[index].Restart();
      this._continuousParticles[index].Emitting = true;
    }
    await Cmd.Wait(this._duration, this._cts.Token);
    for (int index = 0; index < this._continuousParticles.Count; ++index)
      this._continuousParticles[index].Emitting = false;
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
      new MethodInfo(NSpookyScreamVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node2D"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("position"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NSpookyScreamVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSpookyScreamVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSpookyScreamVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NSpookyScreamVfx nspookyScreamVfx = NSpookyScreamVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NSpookyScreamVfx>(ref nspookyScreamVfx);
      return true;
    }
    if (StringName.op_Equality(ref method, NSpookyScreamVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NSpookyScreamVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
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
    if (StringName.op_Equality(ref method, NSpookyScreamVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NSpookyScreamVfx nspookyScreamVfx = NSpookyScreamVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NSpookyScreamVfx>(ref nspookyScreamVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSpookyScreamVfx.MethodName.Create) || StringName.op_Equality(ref method, NSpookyScreamVfx.MethodName._Ready) || StringName.op_Equality(ref method, NSpookyScreamVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSpookyScreamVfx.PropertyName._continuousParticles))
    {
      this._continuousParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpookyScreamVfx.PropertyName._oneShotParticles))
    {
      this._oneShotParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSpookyScreamVfx.PropertyName._duration))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._duration = VariantUtils.ConvertTo<float>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSpookyScreamVfx.PropertyName._continuousParticles))
    {
      value = VariantUtils.CreateFromArray<GpuParticles2D>(this._continuousParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpookyScreamVfx.PropertyName._oneShotParticles))
    {
      value = VariantUtils.CreateFromArray<GpuParticles2D>(this._oneShotParticles);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSpookyScreamVfx.PropertyName._duration))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<float>(ref this._duration);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 28L, NSpookyScreamVfx.PropertyName._continuousParticles, (PropertyHint) 23L, "24/34:GPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 28L, NSpookyScreamVfx.PropertyName._oneShotParticles, (PropertyHint) 23L, "24/34:GPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NSpookyScreamVfx.PropertyName._duration, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NSpookyScreamVfx.PropertyName._continuousParticles, Variant.CreateFrom<GpuParticles2D>(this._continuousParticles));
    info.AddProperty(NSpookyScreamVfx.PropertyName._oneShotParticles, Variant.CreateFrom<GpuParticles2D>(this._oneShotParticles));
    info.AddProperty(NSpookyScreamVfx.PropertyName._duration, Variant.From<float>(ref this._duration));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NSpookyScreamVfx.PropertyName._continuousParticles, ref variant1))
      this._continuousParticles = ((Variant) ref variant1).AsGodotArray<GpuParticles2D>();
    Variant variant2;
    if (info.TryGetProperty(NSpookyScreamVfx.PropertyName._oneShotParticles, ref variant2))
      this._oneShotParticles = ((Variant) ref variant2).AsGodotArray<GpuParticles2D>();
    Variant variant3;
    if (!info.TryGetProperty(NSpookyScreamVfx.PropertyName._duration, ref variant3))
      return;
    this._duration = ((Variant) ref variant3).As<float>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _continuousParticles = StringName.op_Implicit(nameof (_continuousParticles));
    public static readonly StringName _oneShotParticles = StringName.op_Implicit(nameof (_oneShotParticles));
    public static readonly StringName _duration = StringName.op_Implicit(nameof (_duration));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
