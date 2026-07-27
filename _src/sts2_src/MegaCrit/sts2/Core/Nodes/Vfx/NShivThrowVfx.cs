// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NShivThrowVfx
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

[ScriptPath("res://src/Core/Nodes/Vfx/NShivThrowVfx.cs")]
public class NShivThrowVfx : Node2D
{
  private static readonly StringName _color = new StringName("color");
  public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_shiv_throw");
  [Export]
  private Array<GpuParticles2D> _throwParticles = new Array<GpuParticles2D>();
  [Export]
  private Array<GpuParticles2D> _impactParticles = new Array<GpuParticles2D>();
  [Export]
  private Array<GpuParticles2D> _modulateParticles = new Array<GpuParticles2D>();
  private CancellationTokenSource? _cts;

  public static NShivThrowVfx? Create(Creature owner, Creature? target, Color tint)
  {
    if (TestMode.IsOn)
      return (NShivThrowVfx) null;
    NCreature creatureNode1 = NCombatRoom.Instance?.GetCreatureNode(owner);
    if (creatureNode1 == null)
      return (NShivThrowVfx) null;
    NCreature creatureNode2 = NCombatRoom.Instance?.GetCreatureNode(target);
    return creatureNode2 == null ? (NShivThrowVfx) null : NShivThrowVfx.Create(creatureNode1.VfxSpawnPosition, creatureNode2.VfxSpawnPosition, tint);
  }

  public static NShivThrowVfx? Create(
    Vector2 throwerCenterPosition,
    Vector2 targetCenterPosition,
    Color tint)
  {
    if (TestMode.IsOn)
      return (NShivThrowVfx) null;
    NShivThrowVfx nshivThrowVfx = PreloadManager.Cache.GetScene(NShivThrowVfx.scenePath).Instantiate<NShivThrowVfx>((PackedScene.GenEditState) 0L);
    nshivThrowVfx.GlobalPosition = targetCenterPosition;
    nshivThrowVfx.ApplyRotation(throwerCenterPosition, targetCenterPosition);
    nshivThrowVfx.ApplyTint(tint);
    return nshivThrowVfx;
  }

  public void ApplyTint(Color tint)
  {
    for (int index = 0; index < this._modulateParticles.Count; ++index)
    {
      this._modulateParticles[index].ProcessMaterial = (Material) ((Resource) this._modulateParticles[index].ProcessMaterial).Duplicate(false);
      ((GodotObject) this._modulateParticles[index].ProcessMaterial).Set(NShivThrowVfx._color, Variant.op_Implicit(tint));
    }
  }

  public void ApplyRotation(Vector2 throwerPosition, Vector2 targetPosition)
  {
    Vector2 vector2 = Vector2.op_Subtraction(targetPosition, throwerPosition);
    this.RotationDegrees = Mathf.RadToDeg(Mathf.Atan2(vector2.Y, vector2.X));
  }

  public override void _Ready() => TaskHelper.RunSafely(this.PlaySequence());

  public override void _ExitTree() => this._cts?.Cancel();

  private async Task PlaySequence()
  {
    this._cts = new CancellationTokenSource();
    for (int index = 0; index < this._throwParticles.Count; ++index)
      this._throwParticles[index].Restart();
    await Cmd.Wait(0.15f, this._cts.Token);
    for (int index = 0; index < this._impactParticles.Count; ++index)
      this._impactParticles[index].Restart();
    NGame.Instance?.ScreenShake(ShakeStrength.Weak, ShakeDuration.Short);
    await Cmd.Wait(2f, this._cts.Token);
    ((Node) this).QueueFreeSafely();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NShivThrowVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node2D"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("throwerCenterPosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("targetCenterPosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 20L, StringName.op_Implicit("tint"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NShivThrowVfx.MethodName.ApplyTint, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 20L, StringName.op_Implicit("tint"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NShivThrowVfx.MethodName.ApplyRotation, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("throwerPosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("targetPosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NShivThrowVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NShivThrowVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NShivThrowVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      NShivThrowVfx nshivThrowVfx = NShivThrowVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<Color>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = VariantUtils.CreateFrom<NShivThrowVfx>(ref nshivThrowVfx);
      return true;
    }
    if (StringName.op_Equality(ref method, NShivThrowVfx.MethodName.ApplyTint) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ApplyTint(VariantUtils.ConvertTo<Color>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NShivThrowVfx.MethodName.ApplyRotation) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.ApplyRotation(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NShivThrowVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NShivThrowVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
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
    if (StringName.op_Equality(ref method, NShivThrowVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      NShivThrowVfx nshivThrowVfx = NShivThrowVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<Color>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = VariantUtils.CreateFrom<NShivThrowVfx>(ref nshivThrowVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NShivThrowVfx.MethodName.Create) || StringName.op_Equality(ref method, NShivThrowVfx.MethodName.ApplyTint) || StringName.op_Equality(ref method, NShivThrowVfx.MethodName.ApplyRotation) || StringName.op_Equality(ref method, NShivThrowVfx.MethodName._Ready) || StringName.op_Equality(ref method, NShivThrowVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NShivThrowVfx.PropertyName._throwParticles))
    {
      this._throwParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NShivThrowVfx.PropertyName._impactParticles))
    {
      this._impactParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NShivThrowVfx.PropertyName._modulateParticles))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._modulateParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NShivThrowVfx.PropertyName._throwParticles))
    {
      value = VariantUtils.CreateFromArray<GpuParticles2D>(this._throwParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NShivThrowVfx.PropertyName._impactParticles))
    {
      value = VariantUtils.CreateFromArray<GpuParticles2D>(this._impactParticles);
      return true;
    }
    if (!StringName.op_Equality(ref name, NShivThrowVfx.PropertyName._modulateParticles))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFromArray<GpuParticles2D>(this._modulateParticles);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 28L, NShivThrowVfx.PropertyName._throwParticles, (PropertyHint) 23L, "24/34:GPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 28L, NShivThrowVfx.PropertyName._impactParticles, (PropertyHint) 23L, "24/34:GPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 28L, NShivThrowVfx.PropertyName._modulateParticles, (PropertyHint) 23L, "24/34:GPUParticles2D", (PropertyUsageFlags) 4102L, true)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NShivThrowVfx.PropertyName._throwParticles, Variant.CreateFrom<GpuParticles2D>(this._throwParticles));
    info.AddProperty(NShivThrowVfx.PropertyName._impactParticles, Variant.CreateFrom<GpuParticles2D>(this._impactParticles));
    info.AddProperty(NShivThrowVfx.PropertyName._modulateParticles, Variant.CreateFrom<GpuParticles2D>(this._modulateParticles));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NShivThrowVfx.PropertyName._throwParticles, ref variant1))
      this._throwParticles = ((Variant) ref variant1).AsGodotArray<GpuParticles2D>();
    Variant variant2;
    if (info.TryGetProperty(NShivThrowVfx.PropertyName._impactParticles, ref variant2))
      this._impactParticles = ((Variant) ref variant2).AsGodotArray<GpuParticles2D>();
    Variant variant3;
    if (!info.TryGetProperty(NShivThrowVfx.PropertyName._modulateParticles, ref variant3))
      return;
    this._modulateParticles = ((Variant) ref variant3).AsGodotArray<GpuParticles2D>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName ApplyTint = StringName.op_Implicit(nameof (ApplyTint));
    public static readonly StringName ApplyRotation = StringName.op_Implicit(nameof (ApplyRotation));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _throwParticles = StringName.op_Implicit(nameof (_throwParticles));
    public static readonly StringName _impactParticles = StringName.op_Implicit(nameof (_impactParticles));
    public static readonly StringName _modulateParticles = StringName.op_Implicit(nameof (_modulateParticles));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
