// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NSweepingBeamImpactVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NSweepingBeamImpactVfx.cs")]
public class NSweepingBeamImpactVfx : Node2D
{
  public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_sweeping_beam_impact");
  [Export]
  private Array<GpuParticles2D> _impactParticles = new Array<GpuParticles2D>();
  private CancellationTokenSource? _cts;

  public override void _ExitTree() => this._cts?.Cancel();

  public static NSweepingBeamImpactVfx? Create(Creature creature)
  {
    if (TestMode.IsOn)
      return (NSweepingBeamImpactVfx) null;
    NCreature creatureNode = NCombatRoom.Instance?.GetCreatureNode(creature);
    return creatureNode != null ? NSweepingBeamImpactVfx.Create(creatureNode.VfxSpawnPosition) : (NSweepingBeamImpactVfx) null;
  }

  public static NSweepingBeamImpactVfx? Create(Vector2 targetCenter)
  {
    if (TestMode.IsOn)
      return (NSweepingBeamImpactVfx) null;
    NSweepingBeamImpactVfx nsweepingBeamImpactVfx = PreloadManager.Cache.GetScene(NSweepingBeamImpactVfx.scenePath).Instantiate<NSweepingBeamImpactVfx>((PackedScene.GenEditState) 0L);
    nsweepingBeamImpactVfx.GlobalPosition = targetCenter;
    return nsweepingBeamImpactVfx;
  }

  public override void _Ready() => TaskHelper.RunSafely(this.PlaySequence());

  private async Task PlaySequence()
  {
    this._cts = new CancellationTokenSource();
    for (int index = 0; index < this._impactParticles.Count; ++index)
      this._impactParticles[index].Restart();
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
      new MethodInfo(NSweepingBeamImpactVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSweepingBeamImpactVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node2D"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("targetCenter"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NSweepingBeamImpactVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSweepingBeamImpactVfx.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSweepingBeamImpactVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NSweepingBeamImpactVfx nsweepingBeamImpactVfx = NSweepingBeamImpactVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NSweepingBeamImpactVfx>(ref nsweepingBeamImpactVfx);
      return true;
    }
    if (!StringName.op_Equality(ref method, NSweepingBeamImpactVfx.MethodName._Ready) || ((NativeVariantPtrArgs) ref args).Count != 0)
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
    if (StringName.op_Equality(ref method, NSweepingBeamImpactVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NSweepingBeamImpactVfx nsweepingBeamImpactVfx = NSweepingBeamImpactVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NSweepingBeamImpactVfx>(ref nsweepingBeamImpactVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSweepingBeamImpactVfx.MethodName._ExitTree) || StringName.op_Equality(ref method, NSweepingBeamImpactVfx.MethodName.Create) || StringName.op_Equality(ref method, NSweepingBeamImpactVfx.MethodName._Ready) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NSweepingBeamImpactVfx.PropertyName._impactParticles))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._impactParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NSweepingBeamImpactVfx.PropertyName._impactParticles))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFromArray<GpuParticles2D>(this._impactParticles);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 28L, NSweepingBeamImpactVfx.PropertyName._impactParticles, (PropertyHint) 23L, "24/34:GPUParticles2D", (PropertyUsageFlags) 4102L, true)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NSweepingBeamImpactVfx.PropertyName._impactParticles, Variant.CreateFrom<GpuParticles2D>(this._impactParticles));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(NSweepingBeamImpactVfx.PropertyName._impactParticles, ref variant))
      return;
    this._impactParticles = ((Variant) ref variant).AsGodotArray<GpuParticles2D>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _impactParticles = StringName.op_Implicit(nameof (_impactParticles));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
