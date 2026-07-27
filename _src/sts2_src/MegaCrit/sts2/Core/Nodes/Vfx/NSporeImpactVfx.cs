// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NSporeImpactVfx
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

[ScriptPath("res://src/Core/Nodes/Vfx/NSporeImpactVfx.cs")]
public class NSporeImpactVfx : Node2D
{
  public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_spore_impact");
  [Export]
  private Array<GpuParticles2D> _impactParticles = new Array<GpuParticles2D>();
  [Export]
  private Node2D? _poofPivot;
  [Export]
  private Vector2 _scaleRange;
  private CancellationTokenSource? _cts;

  public static NSporeImpactVfx? Create(Creature creature, Color color)
  {
    if (TestMode.IsOn)
      return (NSporeImpactVfx) null;
    NCreature creatureNode = NCombatRoom.Instance?.GetCreatureNode(creature);
    return creatureNode != null ? NSporeImpactVfx.Create(creatureNode.GetBottomOfHitbox(), color) : (NSporeImpactVfx) null;
  }

  public static NSporeImpactVfx? Create(Vector2 targetGroundPosition, Color tint)
  {
    if (TestMode.IsOn)
      return (NSporeImpactVfx) null;
    NSporeImpactVfx nsporeImpactVfx = PreloadManager.Cache.GetScene(NSporeImpactVfx.scenePath).Instantiate<NSporeImpactVfx>((PackedScene.GenEditState) 0L);
    nsporeImpactVfx.GlobalPosition = targetGroundPosition;
    nsporeImpactVfx.Initialize();
    nsporeImpactVfx.ModulateParticles(tint);
    return nsporeImpactVfx;
  }

  private void Initialize()
  {
    this._poofPivot.Scale = Vector2.op_Multiply(Vector2.One, (float) GD.RandRange((double) this._scaleRange.X, (double) this._scaleRange.Y));
  }

  private void ModulateParticles(Color tint) => ((CanvasItem) this).Modulate = tint;

  public override void _Ready() => TaskHelper.RunSafely(this.PlaySequence());

  public override void _ExitTree() => this._cts?.Cancel();

  private async Task PlaySequence()
  {
    this._cts = new CancellationTokenSource();
    for (int index = 0; index < this._impactParticles.Count; ++index)
      this._impactParticles[index].Restart();
    await Cmd.Wait(3.5f, this._cts.Token);
    ((Node) this).QueueFreeSafely();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NSporeImpactVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node2D"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("targetGroundPosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 20L, StringName.op_Implicit("tint"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NSporeImpactVfx.MethodName.Initialize, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSporeImpactVfx.MethodName.ModulateParticles, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 20L, StringName.op_Implicit("tint"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NSporeImpactVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSporeImpactVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSporeImpactVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      NSporeImpactVfx nsporeImpactVfx = NSporeImpactVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<Color>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<NSporeImpactVfx>(ref nsporeImpactVfx);
      return true;
    }
    if (StringName.op_Equality(ref method, NSporeImpactVfx.MethodName.Initialize) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Initialize();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSporeImpactVfx.MethodName.ModulateParticles) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ModulateParticles(VariantUtils.ConvertTo<Color>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSporeImpactVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NSporeImpactVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
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
    if (StringName.op_Equality(ref method, NSporeImpactVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      NSporeImpactVfx nsporeImpactVfx = NSporeImpactVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<Color>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<NSporeImpactVfx>(ref nsporeImpactVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSporeImpactVfx.MethodName.Create) || StringName.op_Equality(ref method, NSporeImpactVfx.MethodName.Initialize) || StringName.op_Equality(ref method, NSporeImpactVfx.MethodName.ModulateParticles) || StringName.op_Equality(ref method, NSporeImpactVfx.MethodName._Ready) || StringName.op_Equality(ref method, NSporeImpactVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSporeImpactVfx.PropertyName._impactParticles))
    {
      this._impactParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSporeImpactVfx.PropertyName._poofPivot))
    {
      this._poofPivot = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSporeImpactVfx.PropertyName._scaleRange))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._scaleRange = VariantUtils.ConvertTo<Vector2>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSporeImpactVfx.PropertyName._impactParticles))
    {
      value = VariantUtils.CreateFromArray<GpuParticles2D>(this._impactParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NSporeImpactVfx.PropertyName._poofPivot))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._poofPivot);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSporeImpactVfx.PropertyName._scaleRange))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Vector2>(ref this._scaleRange);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 28L, NSporeImpactVfx.PropertyName._impactParticles, (PropertyHint) 23L, "24/34:GPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NSporeImpactVfx.PropertyName._poofPivot, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 5L, NSporeImpactVfx.PropertyName._scaleRange, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NSporeImpactVfx.PropertyName._impactParticles, Variant.CreateFrom<GpuParticles2D>(this._impactParticles));
    info.AddProperty(NSporeImpactVfx.PropertyName._poofPivot, Variant.From<Node2D>(ref this._poofPivot));
    info.AddProperty(NSporeImpactVfx.PropertyName._scaleRange, Variant.From<Vector2>(ref this._scaleRange));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NSporeImpactVfx.PropertyName._impactParticles, ref variant1))
      this._impactParticles = ((Variant) ref variant1).AsGodotArray<GpuParticles2D>();
    Variant variant2;
    if (info.TryGetProperty(NSporeImpactVfx.PropertyName._poofPivot, ref variant2))
      this._poofPivot = ((Variant) ref variant2).As<Node2D>();
    Variant variant3;
    if (!info.TryGetProperty(NSporeImpactVfx.PropertyName._scaleRange, ref variant3))
      return;
    this._scaleRange = ((Variant) ref variant3).As<Vector2>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName Initialize = StringName.op_Implicit(nameof (Initialize));
    public static readonly StringName ModulateParticles = StringName.op_Implicit(nameof (ModulateParticles));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _impactParticles = StringName.op_Implicit(nameof (_impactParticles));
    public static readonly StringName _poofPivot = StringName.op_Implicit(nameof (_poofPivot));
    public static readonly StringName _scaleRange = StringName.op_Implicit(nameof (_scaleRange));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
