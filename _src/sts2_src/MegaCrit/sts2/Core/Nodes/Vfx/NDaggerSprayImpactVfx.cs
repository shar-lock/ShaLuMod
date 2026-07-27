// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NDaggerSprayImpactVfx
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
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NDaggerSprayImpactVfx.cs")]
public class NDaggerSprayImpactVfx : Node2D
{
  public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_dagger_spray_impact");
  [Export]
  private Array<GpuParticles2D> _particles = new Array<GpuParticles2D>();
  [Export]
  private Array<GpuParticles2D> _modulateParticles = new Array<GpuParticles2D>();
  [Export]
  private float _impactDelay = 0.1f;
  private CancellationTokenSource? _cts;

  public static NDaggerSprayImpactVfx? Create(Creature creature, Color tint, bool goingRight)
  {
    if (TestMode.IsOn)
      return (NDaggerSprayImpactVfx) null;
    NCreature creatureNode = NCombatRoom.Instance?.GetCreatureNode(creature);
    return creatureNode != null ? NDaggerSprayImpactVfx.Create(creatureNode.VfxSpawnPosition, tint, goingRight) : (NDaggerSprayImpactVfx) null;
  }

  public static NDaggerSprayImpactVfx? Create(Vector2 targetCenter, Color tint, bool goingRight)
  {
    if (TestMode.IsOn)
      return (NDaggerSprayImpactVfx) null;
    NDaggerSprayImpactVfx ndaggerSprayImpactVfx = PreloadManager.Cache.GetScene(NDaggerSprayImpactVfx.scenePath).Instantiate<NDaggerSprayImpactVfx>((PackedScene.GenEditState) 0L);
    ndaggerSprayImpactVfx.GlobalPosition = targetCenter;
    ndaggerSprayImpactVfx.ApplyTint(tint);
    ndaggerSprayImpactVfx.Scale = new Vector2(goingRight ? 1f : -1f, 1f);
    return ndaggerSprayImpactVfx;
  }

  public override void _Ready() => TaskHelper.RunSafely(this.PlaySequence());

  public override void _ExitTree() => this._cts?.Cancel();

  public void ApplyTint(Color tint)
  {
    for (int index = 0; index < this._modulateParticles.Count; ++index)
      ((CanvasItem) this._modulateParticles[index]).SelfModulate = tint;
  }

  private async Task PlaySequence()
  {
    this._cts = new CancellationTokenSource();
    await Cmd.Wait(this._impactDelay, this._cts.Token);
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
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NDaggerSprayImpactVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node2D"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("targetCenter"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 20L, StringName.op_Implicit("tint"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("goingRight"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NDaggerSprayImpactVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDaggerSprayImpactVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDaggerSprayImpactVfx.MethodName.ApplyTint, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NDaggerSprayImpactVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      NDaggerSprayImpactVfx ndaggerSprayImpactVfx = NDaggerSprayImpactVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<Color>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = VariantUtils.CreateFrom<NDaggerSprayImpactVfx>(ref ndaggerSprayImpactVfx);
      return true;
    }
    if (StringName.op_Equality(ref method, NDaggerSprayImpactVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDaggerSprayImpactVfx.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NDaggerSprayImpactVfx.MethodName.ApplyTint) || ((NativeVariantPtrArgs) ref args).Count != 1)
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
    if (StringName.op_Equality(ref method, NDaggerSprayImpactVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      NDaggerSprayImpactVfx ndaggerSprayImpactVfx = NDaggerSprayImpactVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<Color>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = VariantUtils.CreateFrom<NDaggerSprayImpactVfx>(ref ndaggerSprayImpactVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NDaggerSprayImpactVfx.MethodName.Create) || StringName.op_Equality(ref method, NDaggerSprayImpactVfx.MethodName._Ready) || StringName.op_Equality(ref method, NDaggerSprayImpactVfx.MethodName._ExitTree) || StringName.op_Equality(ref method, NDaggerSprayImpactVfx.MethodName.ApplyTint) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDaggerSprayImpactVfx.PropertyName._particles))
    {
      this._particles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDaggerSprayImpactVfx.PropertyName._modulateParticles))
    {
      this._modulateParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDaggerSprayImpactVfx.PropertyName._impactDelay))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._impactDelay = VariantUtils.ConvertTo<float>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDaggerSprayImpactVfx.PropertyName._particles))
    {
      value = VariantUtils.CreateFromArray<GpuParticles2D>(this._particles);
      return true;
    }
    if (StringName.op_Equality(ref name, NDaggerSprayImpactVfx.PropertyName._modulateParticles))
    {
      value = VariantUtils.CreateFromArray<GpuParticles2D>(this._modulateParticles);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDaggerSprayImpactVfx.PropertyName._impactDelay))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<float>(ref this._impactDelay);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 28L, NDaggerSprayImpactVfx.PropertyName._particles, (PropertyHint) 23L, "24/34:GPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 28L, NDaggerSprayImpactVfx.PropertyName._modulateParticles, (PropertyHint) 23L, "24/34:GPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NDaggerSprayImpactVfx.PropertyName._impactDelay, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NDaggerSprayImpactVfx.PropertyName._particles, Variant.CreateFrom<GpuParticles2D>(this._particles));
    info.AddProperty(NDaggerSprayImpactVfx.PropertyName._modulateParticles, Variant.CreateFrom<GpuParticles2D>(this._modulateParticles));
    info.AddProperty(NDaggerSprayImpactVfx.PropertyName._impactDelay, Variant.From<float>(ref this._impactDelay));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NDaggerSprayImpactVfx.PropertyName._particles, ref variant1))
      this._particles = ((Variant) ref variant1).AsGodotArray<GpuParticles2D>();
    Variant variant2;
    if (info.TryGetProperty(NDaggerSprayImpactVfx.PropertyName._modulateParticles, ref variant2))
      this._modulateParticles = ((Variant) ref variant2).AsGodotArray<GpuParticles2D>();
    Variant variant3;
    if (!info.TryGetProperty(NDaggerSprayImpactVfx.PropertyName._impactDelay, ref variant3))
      return;
    this._impactDelay = ((Variant) ref variant3).As<float>();
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
    public static readonly StringName _impactDelay = StringName.op_Implicit(nameof (_impactDelay));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
