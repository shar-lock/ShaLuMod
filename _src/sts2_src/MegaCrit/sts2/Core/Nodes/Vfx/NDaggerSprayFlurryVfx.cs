// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NDaggerSprayFlurryVfx
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

[ScriptPath("res://src/Core/Nodes/Vfx/NDaggerSprayFlurryVfx.cs")]
public class NDaggerSprayFlurryVfx : Node2D
{
  private static readonly StringName _color = new StringName("color");
  public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_dagger_spray_flurry");
  [Export]
  private Array<GpuParticles2D> _particles = new Array<GpuParticles2D>();
  [Export]
  private Array<GpuParticles2D> _modulateParticles = new Array<GpuParticles2D>();
  private CancellationTokenSource? _cts;

  public static NDaggerSprayFlurryVfx? Create(Creature creature, Color tint, bool goingRight)
  {
    if (TestMode.IsOn)
      return (NDaggerSprayFlurryVfx) null;
    NCreature creatureNode = NCombatRoom.Instance?.GetCreatureNode(creature);
    return creatureNode != null ? NDaggerSprayFlurryVfx.Create(creatureNode.VfxSpawnPosition, tint, goingRight) : (NDaggerSprayFlurryVfx) null;
  }

  public static NDaggerSprayFlurryVfx? Create(Vector2 targetCenter, Color tint, bool goingRight)
  {
    if (TestMode.IsOn)
      return (NDaggerSprayFlurryVfx) null;
    NDaggerSprayFlurryVfx ndaggerSprayFlurryVfx = PreloadManager.Cache.GetScene(NDaggerSprayFlurryVfx.scenePath).Instantiate<NDaggerSprayFlurryVfx>((PackedScene.GenEditState) 0L);
    ndaggerSprayFlurryVfx.GlobalPosition = targetCenter;
    ndaggerSprayFlurryVfx.ApplyTint(tint);
    ndaggerSprayFlurryVfx.Scale = new Vector2(goingRight ? 1f : -1f, 1f);
    return ndaggerSprayFlurryVfx;
  }

  public override void _Ready() => TaskHelper.RunSafely(this.PlaySequence());

  public override void _ExitTree() => this._cts?.Cancel();

  public void ApplyTint(Color tint)
  {
    for (int index = 0; index < this._modulateParticles.Count; ++index)
    {
      this._modulateParticles[index].ProcessMaterial = (Material) ((Resource) this._modulateParticles[index].ProcessMaterial).Duplicate(false);
      ((GodotObject) this._modulateParticles[index].ProcessMaterial).Set(NDaggerSprayFlurryVfx._color, Variant.op_Implicit(tint));
    }
  }

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
      new MethodInfo(NDaggerSprayFlurryVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node2D"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("targetCenter"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 20L, StringName.op_Implicit("tint"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("goingRight"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NDaggerSprayFlurryVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDaggerSprayFlurryVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDaggerSprayFlurryVfx.MethodName.ApplyTint, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 20L, StringName.op_Implicit("tint"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NDaggerSprayFlurryVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      NDaggerSprayFlurryVfx ndaggerSprayFlurryVfx = NDaggerSprayFlurryVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<Color>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = VariantUtils.CreateFrom<NDaggerSprayFlurryVfx>(ref ndaggerSprayFlurryVfx);
      return true;
    }
    if (StringName.op_Equality(ref method, NDaggerSprayFlurryVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDaggerSprayFlurryVfx.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NDaggerSprayFlurryVfx.MethodName.ApplyTint) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.ApplyTint(VariantUtils.ConvertTo<Color>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NDaggerSprayFlurryVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      NDaggerSprayFlurryVfx ndaggerSprayFlurryVfx = NDaggerSprayFlurryVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<Color>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = VariantUtils.CreateFrom<NDaggerSprayFlurryVfx>(ref ndaggerSprayFlurryVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NDaggerSprayFlurryVfx.MethodName.Create) || StringName.op_Equality(ref method, NDaggerSprayFlurryVfx.MethodName._Ready) || StringName.op_Equality(ref method, NDaggerSprayFlurryVfx.MethodName._ExitTree) || StringName.op_Equality(ref method, NDaggerSprayFlurryVfx.MethodName.ApplyTint) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDaggerSprayFlurryVfx.PropertyName._particles))
    {
      this._particles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDaggerSprayFlurryVfx.PropertyName._modulateParticles))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._modulateParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDaggerSprayFlurryVfx.PropertyName._particles))
    {
      value = VariantUtils.CreateFromArray<GpuParticles2D>(this._particles);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDaggerSprayFlurryVfx.PropertyName._modulateParticles))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFromArray<GpuParticles2D>(this._modulateParticles);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 28L, NDaggerSprayFlurryVfx.PropertyName._particles, (PropertyHint) 23L, "24/34:GPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 28L, NDaggerSprayFlurryVfx.PropertyName._modulateParticles, (PropertyHint) 23L, "24/34:GPUParticles2D", (PropertyUsageFlags) 4102L, true)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NDaggerSprayFlurryVfx.PropertyName._particles, Variant.CreateFrom<GpuParticles2D>(this._particles));
    info.AddProperty(NDaggerSprayFlurryVfx.PropertyName._modulateParticles, Variant.CreateFrom<GpuParticles2D>(this._modulateParticles));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NDaggerSprayFlurryVfx.PropertyName._particles, ref variant1))
      this._particles = ((Variant) ref variant1).AsGodotArray<GpuParticles2D>();
    Variant variant2;
    if (!info.TryGetProperty(NDaggerSprayFlurryVfx.PropertyName._modulateParticles, ref variant2))
      return;
    this._modulateParticles = ((Variant) ref variant2).AsGodotArray<GpuParticles2D>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName ApplyTint = StringName.op_Implicit(nameof (ApplyTint));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _particles = StringName.op_Implicit(nameof (_particles));
    public static readonly StringName _modulateParticles = StringName.op_Implicit(nameof (_modulateParticles));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
