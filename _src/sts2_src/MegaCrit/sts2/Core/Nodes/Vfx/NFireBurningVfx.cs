// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NFireBurningVfx
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

[ScriptPath("res://src/Core/Nodes/Vfx/NFireBurningVfx.cs")]
public class NFireBurningVfx : Node2D
{
  public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_fire_burning");
  [Export]
  private Array<GpuParticles2D> _startParticles = new Array<GpuParticles2D>();
  [Export]
  private Array<GpuParticles2D> _endParticles = new Array<GpuParticles2D>();
  private CancellationTokenSource? _cts;

  private static Color DefaultColor => Color.FromHtml(string.op_Implicit("#ff8b57"));

  public static NFireBurningVfx? Create(Creature creature, float scaleFactor, bool goingRight)
  {
    if (TestMode.IsOn)
      return (NFireBurningVfx) null;
    NCreature creatureNode = NCombatRoom.Instance?.GetCreatureNode(creature);
    return creatureNode != null ? NFireBurningVfx.Create(creatureNode.GetBottomOfHitbox(), scaleFactor, goingRight) : (NFireBurningVfx) null;
  }

  public static NFireBurningVfx? Create(
    Vector2 targetFloorPosition,
    float scaleFactor,
    bool goingRight)
  {
    return NFireBurningVfx.Create(targetFloorPosition, scaleFactor, goingRight, NFireBurningVfx.DefaultColor);
  }

  public static NFireBurningVfx? Create(
    Vector2 targetFloorPosition,
    float scaleFactor,
    bool goingRight,
    Color tint)
  {
    if (TestMode.IsOn)
      return (NFireBurningVfx) null;
    NFireBurningVfx nfireBurningVfx = PreloadManager.Cache.GetScene(NFireBurningVfx.scenePath).Instantiate<NFireBurningVfx>((PackedScene.GenEditState) 0L);
    nfireBurningVfx.GlobalPosition = targetFloorPosition;
    ((CanvasItem) nfireBurningVfx).Modulate = tint;
    Vector2 vector2 = Vector2.op_Multiply(Vector2.One, scaleFactor);
    vector2.X *= goingRight ? 1f : -1f;
    nfireBurningVfx.Scale = vector2;
    return nfireBurningVfx;
  }

  public override void _Ready() => TaskHelper.RunSafely(this.PlaySequence());

  public override void _ExitTree() => this._cts?.Cancel();

  private async Task PlaySequence()
  {
    this._cts = new CancellationTokenSource();
    for (int index = 0; index < this._startParticles.Count; ++index)
      this._startParticles[index].Restart();
    await Cmd.Wait(0.3f, this._cts.Token);
    for (int index = 0; index < this._endParticles.Count; ++index)
      this._endParticles[index].Restart();
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
      new MethodInfo(NFireBurningVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node2D"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("targetFloorPosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("scaleFactor"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("goingRight"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NFireBurningVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node2D"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("targetFloorPosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("scaleFactor"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("goingRight"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 20L, StringName.op_Implicit("tint"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NFireBurningVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NFireBurningVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NFireBurningVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      NFireBurningVfx nfireBurningVfx = NFireBurningVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = VariantUtils.CreateFrom<NFireBurningVfx>(ref nfireBurningVfx);
      return true;
    }
    if (StringName.op_Equality(ref method, NFireBurningVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 4)
    {
      NFireBurningVfx nfireBurningVfx = NFireBurningVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<Color>(ref ((NativeVariantPtrArgs) ref args)[3]));
      ret = VariantUtils.CreateFrom<NFireBurningVfx>(ref nfireBurningVfx);
      return true;
    }
    if (StringName.op_Equality(ref method, NFireBurningVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NFireBurningVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
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
    if (StringName.op_Equality(ref method, NFireBurningVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      NFireBurningVfx nfireBurningVfx = NFireBurningVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = VariantUtils.CreateFrom<NFireBurningVfx>(ref nfireBurningVfx);
      return true;
    }
    if (StringName.op_Equality(ref method, NFireBurningVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 4)
    {
      NFireBurningVfx nfireBurningVfx = NFireBurningVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<Color>(ref ((NativeVariantPtrArgs) ref args)[3]));
      ret = VariantUtils.CreateFrom<NFireBurningVfx>(ref nfireBurningVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NFireBurningVfx.MethodName.Create) || StringName.op_Equality(ref method, NFireBurningVfx.MethodName._Ready) || StringName.op_Equality(ref method, NFireBurningVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NFireBurningVfx.PropertyName._startParticles))
    {
      this._startParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NFireBurningVfx.PropertyName._endParticles))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._endParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NFireBurningVfx.PropertyName._startParticles))
    {
      value = VariantUtils.CreateFromArray<GpuParticles2D>(this._startParticles);
      return true;
    }
    if (!StringName.op_Equality(ref name, NFireBurningVfx.PropertyName._endParticles))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFromArray<GpuParticles2D>(this._endParticles);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 28L, NFireBurningVfx.PropertyName._startParticles, (PropertyHint) 23L, "24/34:GPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 28L, NFireBurningVfx.PropertyName._endParticles, (PropertyHint) 23L, "24/34:GPUParticles2D", (PropertyUsageFlags) 4102L, true)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NFireBurningVfx.PropertyName._startParticles, Variant.CreateFrom<GpuParticles2D>(this._startParticles));
    info.AddProperty(NFireBurningVfx.PropertyName._endParticles, Variant.CreateFrom<GpuParticles2D>(this._endParticles));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NFireBurningVfx.PropertyName._startParticles, ref variant1))
      this._startParticles = ((Variant) ref variant1).AsGodotArray<GpuParticles2D>();
    Variant variant2;
    if (!info.TryGetProperty(NFireBurningVfx.PropertyName._endParticles, ref variant2))
      return;
    this._endParticles = ((Variant) ref variant2).AsGodotArray<GpuParticles2D>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _startParticles = StringName.op_Implicit(nameof (_startParticles));
    public static readonly StringName _endParticles = StringName.op_Implicit(nameof (_endParticles));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
