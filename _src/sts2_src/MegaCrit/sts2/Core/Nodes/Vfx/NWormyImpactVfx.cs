// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NWormyImpactVfx
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

[ScriptPath("res://src/Core/Nodes/Vfx/NWormyImpactVfx.cs")]
public class NWormyImpactVfx : Node2D
{
  public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_wormy_impact");
  [Export]
  private Array<GpuParticles2D> _particles = new Array<GpuParticles2D>();
  [Export]
  private Node2D? _centerPivot;
  [Export]
  private Node2D? _groundPivot;
  private CancellationTokenSource? _cts;

  public override void _ExitTree() => this._cts?.Cancel();

  public static NWormyImpactVfx? Create(Creature creature)
  {
    if (TestMode.IsOn)
      return (NWormyImpactVfx) null;
    NCreature creatureNode = NCombatRoom.Instance?.GetCreatureNode(creature);
    return creatureNode != null ? NWormyImpactVfx.Create(creatureNode.GetBottomOfHitbox(), creatureNode.VfxSpawnPosition) : (NWormyImpactVfx) null;
  }

  public static NWormyImpactVfx? Create(Vector2 targetGroundPosition, Vector2 targetCenterPosition)
  {
    if (TestMode.IsOn)
      return (NWormyImpactVfx) null;
    NWormyImpactVfx nwormyImpactVfx = PreloadManager.Cache.GetScene(NWormyImpactVfx.scenePath).Instantiate<NWormyImpactVfx>((PackedScene.GenEditState) 0L);
    nwormyImpactVfx.GlobalPosition = targetGroundPosition;
    nwormyImpactVfx.Initialize(targetGroundPosition, targetCenterPosition);
    return nwormyImpactVfx;
  }

  private void Initialize(Vector2 targetGroundPosition, Vector2 targetCenterPosition)
  {
    this._groundPivot.GlobalPosition = targetGroundPosition;
    this._centerPivot.GlobalPosition = targetCenterPosition;
  }

  public override void _Ready() => TaskHelper.RunSafely(this.PlaySequence());

  private async Task PlaySequence()
  {
    this._cts = new CancellationTokenSource();
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
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NWormyImpactVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NWormyImpactVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node2D"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("targetGroundPosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("targetCenterPosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NWormyImpactVfx.MethodName.Initialize, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("targetGroundPosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("targetCenterPosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NWormyImpactVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NWormyImpactVfx.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NWormyImpactVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      NWormyImpactVfx nwormyImpactVfx = NWormyImpactVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<NWormyImpactVfx>(ref nwormyImpactVfx);
      return true;
    }
    if (StringName.op_Equality(ref method, NWormyImpactVfx.MethodName.Initialize) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.Initialize(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NWormyImpactVfx.MethodName._Ready) || ((NativeVariantPtrArgs) ref args).Count != 0)
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
    if (StringName.op_Equality(ref method, NWormyImpactVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      NWormyImpactVfx nwormyImpactVfx = NWormyImpactVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<NWormyImpactVfx>(ref nwormyImpactVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NWormyImpactVfx.MethodName._ExitTree) || StringName.op_Equality(ref method, NWormyImpactVfx.MethodName.Create) || StringName.op_Equality(ref method, NWormyImpactVfx.MethodName.Initialize) || StringName.op_Equality(ref method, NWormyImpactVfx.MethodName._Ready) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NWormyImpactVfx.PropertyName._particles))
    {
      this._particles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NWormyImpactVfx.PropertyName._centerPivot))
    {
      this._centerPivot = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NWormyImpactVfx.PropertyName._groundPivot))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._groundPivot = VariantUtils.ConvertTo<Node2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NWormyImpactVfx.PropertyName._particles))
    {
      value = VariantUtils.CreateFromArray<GpuParticles2D>(this._particles);
      return true;
    }
    if (StringName.op_Equality(ref name, NWormyImpactVfx.PropertyName._centerPivot))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._centerPivot);
      return true;
    }
    if (!StringName.op_Equality(ref name, NWormyImpactVfx.PropertyName._groundPivot))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Node2D>(ref this._groundPivot);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 28L, NWormyImpactVfx.PropertyName._particles, (PropertyHint) 23L, "24/34:GPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NWormyImpactVfx.PropertyName._centerPivot, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NWormyImpactVfx.PropertyName._groundPivot, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NWormyImpactVfx.PropertyName._particles, Variant.CreateFrom<GpuParticles2D>(this._particles));
    info.AddProperty(NWormyImpactVfx.PropertyName._centerPivot, Variant.From<Node2D>(ref this._centerPivot));
    info.AddProperty(NWormyImpactVfx.PropertyName._groundPivot, Variant.From<Node2D>(ref this._groundPivot));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NWormyImpactVfx.PropertyName._particles, ref variant1))
      this._particles = ((Variant) ref variant1).AsGodotArray<GpuParticles2D>();
    Variant variant2;
    if (info.TryGetProperty(NWormyImpactVfx.PropertyName._centerPivot, ref variant2))
      this._centerPivot = ((Variant) ref variant2).As<Node2D>();
    Variant variant3;
    if (!info.TryGetProperty(NWormyImpactVfx.PropertyName._groundPivot, ref variant3))
      return;
    this._groundPivot = ((Variant) ref variant3).As<Node2D>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName Initialize = StringName.op_Implicit(nameof (Initialize));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _particles = StringName.op_Implicit(nameof (_particles));
    public static readonly StringName _centerPivot = StringName.op_Implicit(nameof (_centerPivot));
    public static readonly StringName _groundPivot = StringName.op_Implicit(nameof (_groundPivot));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
