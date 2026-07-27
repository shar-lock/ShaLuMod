// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NMapNodeSelectVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NMapNodeSelectVfx.cs")]
public class NMapNodeSelectVfx : Control
{
  private const string _path = "res://scenes/vfx/map_node_select_vfx.tscn";
  private static readonly string[] _textures = new string[1]
  {
    "res://images/vfx/brush_particle_2.png"
  };
  private double _lifeTimer;
  private readonly CancellationTokenSource _cancelToken = new CancellationTokenSource();
  private GpuParticles2D _particles;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return ((IEnumerable<string>) NMapNodeSelectVfx._textures).Append<string>("res://scenes/vfx/map_node_select_vfx.tscn");
    }
  }

  public override void _Ready()
  {
    this._particles = ((Node) this).GetNode<GpuParticles2D>(NodePath.op_Implicit("Particles"));
    this._particles.Emitting = true;
    TaskHelper.RunSafely(this.Play());
  }

  public override void _ExitTree()
  {
    ((Node) this)._ExitTree();
    this._cancelToken.Cancel();
  }

  public static NMapNodeSelectVfx? Create(float scaleMultiplier)
  {
    if (TestMode.IsOn)
      return (NMapNodeSelectVfx) null;
    NMapNodeSelectVfx nmapNodeSelectVfx = PreloadManager.Cache.GetScene("res://scenes/vfx/map_node_select_vfx.tscn").Instantiate<NMapNodeSelectVfx>((PackedScene.GenEditState) 0L);
    nmapNodeSelectVfx.Scale = Vector2.op_Multiply(Vector2.One, scaleMultiplier);
    return nmapNodeSelectVfx;
  }

  private async Task Play()
  {
    await Task.Delay(1000, this._cancelToken.Token);
    if (this._cancelToken.IsCancellationRequested)
      return;
    ((Node) this).QueueFreeSafely();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NMapNodeSelectVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapNodeSelectVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapNodeSelectVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("scaleMultiplier"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMapNodeSelectVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapNodeSelectVfx.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMapNodeSelectVfx.MethodName.Create) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    NMapNodeSelectVfx nmapNodeSelectVfx = NMapNodeSelectVfx.Create(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = VariantUtils.CreateFrom<NMapNodeSelectVfx>(ref nmapNodeSelectVfx);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMapNodeSelectVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NMapNodeSelectVfx nmapNodeSelectVfx = NMapNodeSelectVfx.Create(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NMapNodeSelectVfx>(ref nmapNodeSelectVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMapNodeSelectVfx.MethodName._Ready) || StringName.op_Equality(ref method, NMapNodeSelectVfx.MethodName._ExitTree) || StringName.op_Equality(ref method, NMapNodeSelectVfx.MethodName.Create) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMapNodeSelectVfx.PropertyName._lifeTimer))
    {
      this._lifeTimer = VariantUtils.ConvertTo<double>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMapNodeSelectVfx.PropertyName._particles))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._particles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMapNodeSelectVfx.PropertyName._lifeTimer))
    {
      value = VariantUtils.CreateFrom<double>(ref this._lifeTimer);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMapNodeSelectVfx.PropertyName._particles))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._particles);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 3L, NMapNodeSelectVfx.PropertyName._lifeTimer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapNodeSelectVfx.PropertyName._particles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NMapNodeSelectVfx.PropertyName._lifeTimer, Variant.From<double>(ref this._lifeTimer));
    info.AddProperty(NMapNodeSelectVfx.PropertyName._particles, Variant.From<GpuParticles2D>(ref this._particles));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMapNodeSelectVfx.PropertyName._lifeTimer, ref variant1))
      this._lifeTimer = ((Variant) ref variant1).As<double>();
    Variant variant2;
    if (!info.TryGetProperty(NMapNodeSelectVfx.PropertyName._particles, ref variant2))
      return;
    this._particles = ((Variant) ref variant2).As<GpuParticles2D>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _lifeTimer = StringName.op_Implicit(nameof (_lifeTimer));
    public static readonly StringName _particles = StringName.op_Implicit(nameof (_particles));
  }

  public class SignalName : Control.SignalName
  {
  }
}
