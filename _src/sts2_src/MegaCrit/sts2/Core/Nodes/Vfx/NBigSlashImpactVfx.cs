// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NBigSlashImpactVfx
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

[ScriptPath("res://src/Core/Nodes/Vfx/NBigSlashImpactVfx.cs")]
public class NBigSlashImpactVfx : Node2D
{
  public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_big_slash_impact");
  [Export]
  private Array<GpuParticles2D> _anticipationParticles = new Array<GpuParticles2D>();
  [Export]
  private Array<GpuParticles2D> _impactParticles = new Array<GpuParticles2D>();
  [Export]
  private Array<GpuParticles2D> _modulateParticles = new Array<GpuParticles2D>();
  [Export]
  private Node2D? _corePivot;
  private CancellationTokenSource? _cts;

  public static NBigSlashImpactVfx? Create(Creature creature)
  {
    if (TestMode.IsOn)
      return (NBigSlashImpactVfx) null;
    NCreature creatureNode = NCombatRoom.Instance?.GetCreatureNode(creature);
    return creatureNode != null ? NBigSlashImpactVfx.Create(creatureNode.VfxSpawnPosition) : (NBigSlashImpactVfx) null;
  }

  public static NBigSlashImpactVfx? Create(Vector2 targetCenterPosition)
  {
    return TestMode.IsOn ? (NBigSlashImpactVfx) null : NBigSlashImpactVfx.Create(targetCenterPosition, 60f, Color.FromHtml(string.op_Implicit("#80dbff")));
  }

  public static NBigSlashImpactVfx? Create(
    Vector2 targetCenterPosition,
    float rotationDegrees,
    Color tint)
  {
    if (TestMode.IsOn)
      return (NBigSlashImpactVfx) null;
    NBigSlashImpactVfx nbigSlashImpactVfx = PreloadManager.Cache.GetScene(NBigSlashImpactVfx.scenePath).Instantiate<NBigSlashImpactVfx>((PackedScene.GenEditState) 0L);
    nbigSlashImpactVfx.GlobalPosition = targetCenterPosition;
    nbigSlashImpactVfx.RotateCore(rotationDegrees);
    nbigSlashImpactVfx.ModulateParticles(tint);
    return nbigSlashImpactVfx;
  }

  public override void _Ready() => TaskHelper.RunSafely(this.PlaySequence());

  private void RotateCore(float rotationDegrees)
  {
    if (this._corePivot == null)
      return;
    this._corePivot.RotationDegrees = rotationDegrees;
  }

  private void ModulateParticles(Color tint)
  {
    for (int index = 0; index < this._modulateParticles.Count; ++index)
      ((CanvasItem) this._modulateParticles[index]).SelfModulate = tint;
  }

  public override void _ExitTree() => this._cts?.Cancel();

  private async Task PlaySequence()
  {
    this._cts = new CancellationTokenSource();
    for (int index = 0; index < this._anticipationParticles.Count; ++index)
      this._anticipationParticles[index].Restart();
    await Cmd.Wait(0.1f, this._cts.Token);
    NGame.Instance?.ScreenShake(ShakeStrength.Strong, ShakeDuration.Short);
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
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NBigSlashImpactVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node2D"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("targetCenterPosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NBigSlashImpactVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node2D"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("targetCenterPosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("rotationDegrees"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 20L, StringName.op_Implicit("tint"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NBigSlashImpactVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBigSlashImpactVfx.MethodName.RotateCore, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("rotationDegrees"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NBigSlashImpactVfx.MethodName.ModulateParticles, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 20L, StringName.op_Implicit("tint"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NBigSlashImpactVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NBigSlashImpactVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NBigSlashImpactVfx nbigSlashImpactVfx = NBigSlashImpactVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NBigSlashImpactVfx>(ref nbigSlashImpactVfx);
      return true;
    }
    if (StringName.op_Equality(ref method, NBigSlashImpactVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      NBigSlashImpactVfx nbigSlashImpactVfx = NBigSlashImpactVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<Color>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = VariantUtils.CreateFrom<NBigSlashImpactVfx>(ref nbigSlashImpactVfx);
      return true;
    }
    if (StringName.op_Equality(ref method, NBigSlashImpactVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBigSlashImpactVfx.MethodName.RotateCore) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.RotateCore(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBigSlashImpactVfx.MethodName.ModulateParticles) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ModulateParticles(VariantUtils.ConvertTo<Color>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NBigSlashImpactVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
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
    if (StringName.op_Equality(ref method, NBigSlashImpactVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NBigSlashImpactVfx nbigSlashImpactVfx = NBigSlashImpactVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NBigSlashImpactVfx>(ref nbigSlashImpactVfx);
      return true;
    }
    if (StringName.op_Equality(ref method, NBigSlashImpactVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      NBigSlashImpactVfx nbigSlashImpactVfx = NBigSlashImpactVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<Color>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = VariantUtils.CreateFrom<NBigSlashImpactVfx>(ref nbigSlashImpactVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NBigSlashImpactVfx.MethodName.Create) || StringName.op_Equality(ref method, NBigSlashImpactVfx.MethodName._Ready) || StringName.op_Equality(ref method, NBigSlashImpactVfx.MethodName.RotateCore) || StringName.op_Equality(ref method, NBigSlashImpactVfx.MethodName.ModulateParticles) || StringName.op_Equality(ref method, NBigSlashImpactVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NBigSlashImpactVfx.PropertyName._anticipationParticles))
    {
      this._anticipationParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBigSlashImpactVfx.PropertyName._impactParticles))
    {
      this._impactParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBigSlashImpactVfx.PropertyName._modulateParticles))
    {
      this._modulateParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NBigSlashImpactVfx.PropertyName._corePivot))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._corePivot = VariantUtils.ConvertTo<Node2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NBigSlashImpactVfx.PropertyName._anticipationParticles))
    {
      value = VariantUtils.CreateFromArray<GpuParticles2D>(this._anticipationParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NBigSlashImpactVfx.PropertyName._impactParticles))
    {
      value = VariantUtils.CreateFromArray<GpuParticles2D>(this._impactParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NBigSlashImpactVfx.PropertyName._modulateParticles))
    {
      value = VariantUtils.CreateFromArray<GpuParticles2D>(this._modulateParticles);
      return true;
    }
    if (!StringName.op_Equality(ref name, NBigSlashImpactVfx.PropertyName._corePivot))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Node2D>(ref this._corePivot);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 28L, NBigSlashImpactVfx.PropertyName._anticipationParticles, (PropertyHint) 23L, "24/34:GPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 28L, NBigSlashImpactVfx.PropertyName._impactParticles, (PropertyHint) 23L, "24/34:GPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 28L, NBigSlashImpactVfx.PropertyName._modulateParticles, (PropertyHint) 23L, "24/34:GPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NBigSlashImpactVfx.PropertyName._corePivot, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NBigSlashImpactVfx.PropertyName._anticipationParticles, Variant.CreateFrom<GpuParticles2D>(this._anticipationParticles));
    info.AddProperty(NBigSlashImpactVfx.PropertyName._impactParticles, Variant.CreateFrom<GpuParticles2D>(this._impactParticles));
    info.AddProperty(NBigSlashImpactVfx.PropertyName._modulateParticles, Variant.CreateFrom<GpuParticles2D>(this._modulateParticles));
    info.AddProperty(NBigSlashImpactVfx.PropertyName._corePivot, Variant.From<Node2D>(ref this._corePivot));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NBigSlashImpactVfx.PropertyName._anticipationParticles, ref variant1))
      this._anticipationParticles = ((Variant) ref variant1).AsGodotArray<GpuParticles2D>();
    Variant variant2;
    if (info.TryGetProperty(NBigSlashImpactVfx.PropertyName._impactParticles, ref variant2))
      this._impactParticles = ((Variant) ref variant2).AsGodotArray<GpuParticles2D>();
    Variant variant3;
    if (info.TryGetProperty(NBigSlashImpactVfx.PropertyName._modulateParticles, ref variant3))
      this._modulateParticles = ((Variant) ref variant3).AsGodotArray<GpuParticles2D>();
    Variant variant4;
    if (!info.TryGetProperty(NBigSlashImpactVfx.PropertyName._corePivot, ref variant4))
      return;
    this._corePivot = ((Variant) ref variant4).As<Node2D>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName RotateCore = StringName.op_Implicit(nameof (RotateCore));
    public static readonly StringName ModulateParticles = StringName.op_Implicit(nameof (ModulateParticles));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _anticipationParticles = StringName.op_Implicit(nameof (_anticipationParticles));
    public static readonly StringName _impactParticles = StringName.op_Implicit(nameof (_impactParticles));
    public static readonly StringName _modulateParticles = StringName.op_Implicit(nameof (_modulateParticles));
    public static readonly StringName _corePivot = StringName.op_Implicit(nameof (_corePivot));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
