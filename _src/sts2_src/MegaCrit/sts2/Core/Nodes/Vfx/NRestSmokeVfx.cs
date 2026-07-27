// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NRestSmokeVfx
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
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NRestSmokeVfx.cs")]
public class NRestSmokeVfx : Node2D
{
  private GpuParticles2D _clouds;
  private CancellationTokenSource? _cts;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NRestSmokeVfx.ScenePath);
    }
  }

  public override void _ExitTree() => this._cts?.Cancel();

  private static string ScenePath => SceneHelper.GetScenePath("vfx/rest_smoke_vfx");

  public static NRestSmokeVfx? Create()
  {
    return TestMode.IsOn ? (NRestSmokeVfx) null : PreloadManager.Cache.GetScene(NRestSmokeVfx.ScenePath).Instantiate<NRestSmokeVfx>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready()
  {
    this._clouds = ((Node) this).GetNode<GpuParticles2D>(NodePath.op_Implicit("Clouds"));
    this._clouds.Emitting = true;
    Rect2 visibleRect = ((Node) this).GetViewport().GetVisibleRect();
    this.GlobalPosition = ((Rect2) ref visibleRect).GetCenter();
    TaskHelper.RunSafely(this.DeleteWhenFinished());
  }

  private async Task DeleteWhenFinished()
  {
    this._cts = new CancellationTokenSource();
    await Task.Delay(4000, this._cts.Token);
    if (!GodotObject.IsInstanceValid((GodotObject) this))
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
      new MethodInfo(NRestSmokeVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRestSmokeVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node2D"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRestSmokeVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NRestSmokeVfx.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRestSmokeVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NRestSmokeVfx nrestSmokeVfx = NRestSmokeVfx.Create();
      ret = VariantUtils.CreateFrom<NRestSmokeVfx>(ref nrestSmokeVfx);
      return true;
    }
    if (!StringName.op_Equality(ref method, NRestSmokeVfx.MethodName._Ready) || ((NativeVariantPtrArgs) ref args).Count != 0)
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
    if (StringName.op_Equality(ref method, NRestSmokeVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NRestSmokeVfx nrestSmokeVfx = NRestSmokeVfx.Create();
      ret = VariantUtils.CreateFrom<NRestSmokeVfx>(ref nrestSmokeVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NRestSmokeVfx.MethodName._ExitTree) || StringName.op_Equality(ref method, NRestSmokeVfx.MethodName.Create) || StringName.op_Equality(ref method, NRestSmokeVfx.MethodName._Ready) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NRestSmokeVfx.PropertyName._clouds))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._clouds = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NRestSmokeVfx.PropertyName._clouds))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._clouds);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NRestSmokeVfx.PropertyName._clouds, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NRestSmokeVfx.PropertyName._clouds, Variant.From<GpuParticles2D>(ref this._clouds));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(NRestSmokeVfx.PropertyName._clouds, ref variant))
      return;
    this._clouds = ((Variant) ref variant).As<GpuParticles2D>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _clouds = StringName.op_Implicit(nameof (_clouds));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
