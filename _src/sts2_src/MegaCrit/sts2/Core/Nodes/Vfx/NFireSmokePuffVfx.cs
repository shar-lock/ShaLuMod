// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NFireSmokePuffVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NFireSmokePuffVfx.cs")]
public class NFireSmokePuffVfx : Node2D
{
  private GpuParticles2D _clouds;
  private GpuParticles2D _ember;
  private CancellationTokenSource? _cts;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NFireSmokePuffVfx.ScenePath);
    }
  }

  public override void _ExitTree() => this._cts?.Cancel();

  private static string ScenePath => SceneHelper.GetScenePath("vfx/vfx_fire_smoke_puff");

  public static NFireSmokePuffVfx? Create(Creature target)
  {
    if (TestMode.IsOn)
      return (NFireSmokePuffVfx) null;
    NFireSmokePuffVfx nfireSmokePuffVfx = PreloadManager.Cache.GetScene(NFireSmokePuffVfx.ScenePath).Instantiate<NFireSmokePuffVfx>((PackedScene.GenEditState) 0L);
    nfireSmokePuffVfx.GlobalPosition = NCombatRoom.Instance.GetCreatureNode(target).VfxSpawnPosition;
    return nfireSmokePuffVfx;
  }

  public override void _Ready()
  {
    this._ember = ((Node) this).GetNode<GpuParticles2D>(NodePath.op_Implicit("Ember"));
    this._clouds = ((Node) this).GetNode<GpuParticles2D>(NodePath.op_Implicit("Clouds"));
    this._ember.Emitting = true;
    this._clouds.Emitting = true;
    TaskHelper.RunSafely(this.DeleteAfterComplete());
  }

  private async Task DeleteAfterComplete()
  {
    this._cts = new CancellationTokenSource();
    await Task.Delay(2500, this._cts.Token);
    ((Node) this).QueueFreeSafely();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NFireSmokePuffVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NFireSmokePuffVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NFireSmokePuffVfx.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NFireSmokePuffVfx.MethodName._Ready) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._Ready();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NFireSmokePuffVfx.MethodName._ExitTree) || StringName.op_Equality(ref method, NFireSmokePuffVfx.MethodName._Ready) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NFireSmokePuffVfx.PropertyName._clouds))
    {
      this._clouds = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NFireSmokePuffVfx.PropertyName._ember))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._ember = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NFireSmokePuffVfx.PropertyName._clouds))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._clouds);
      return true;
    }
    if (!StringName.op_Equality(ref name, NFireSmokePuffVfx.PropertyName._ember))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._ember);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NFireSmokePuffVfx.PropertyName._clouds, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NFireSmokePuffVfx.PropertyName._ember, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NFireSmokePuffVfx.PropertyName._clouds, Variant.From<GpuParticles2D>(ref this._clouds));
    info.AddProperty(NFireSmokePuffVfx.PropertyName._ember, Variant.From<GpuParticles2D>(ref this._ember));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NFireSmokePuffVfx.PropertyName._clouds, ref variant1))
      this._clouds = ((Variant) ref variant1).As<GpuParticles2D>();
    Variant variant2;
    if (!info.TryGetProperty(NFireSmokePuffVfx.PropertyName._ember, ref variant2))
      return;
    this._ember = ((Variant) ref variant2).As<GpuParticles2D>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _clouds = StringName.op_Implicit(nameof (_clouds));
    public static readonly StringName _ember = StringName.op_Implicit(nameof (_ember));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
