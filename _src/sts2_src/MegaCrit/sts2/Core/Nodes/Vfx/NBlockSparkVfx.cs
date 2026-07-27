// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NBlockSparkVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NBlockSparkVfx.cs")]
public class NBlockSparkVfx : Node2D
{
  [Export]
  private Array<GpuParticles2D> _particles = new Array<GpuParticles2D>();
  [Export]
  private GpuParticles2D _specks;
  private NCreature _creatureNode;

  private static string ScenePath => SceneHelper.GetScenePath("vfx/block_spark_vfx");

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NBlockSparkVfx.ScenePath);
    }
  }

  public static NBlockSparkVfx? Create(Creature target)
  {
    NCreature creatureNode = NCombatRoom.Instance?.GetCreatureNode(target);
    if (creatureNode == null || !creatureNode.IsInteractable)
      return (NBlockSparkVfx) null;
    NBlockSparkVfx nblockSparkVfx = PreloadManager.Cache.GetScene(NBlockSparkVfx.ScenePath).Instantiate<NBlockSparkVfx>((PackedScene.GenEditState) 0L);
    nblockSparkVfx._creatureNode = creatureNode;
    return nblockSparkVfx;
  }

  public override void _Ready()
  {
    this.GlobalPosition = this._creatureNode.VfxSpawnPosition;
    for (int index = 0; index < this._particles.Count; ++index)
      this._particles[index].Restart();
    TaskHelper.RunSafely(this.FlashAndFree());
  }

  private async Task FlashAndFree()
  {
    await ((GodotObject) this._specks).AwaitSignal(GpuParticles2D.SignalName.Finished, (Node) this);
    ((Node) this).QueueFreeSafely();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NBlockSparkVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (!StringName.op_Equality(ref method, NBlockSparkVfx.MethodName._Ready) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._Ready();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NBlockSparkVfx.MethodName._Ready) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NBlockSparkVfx.PropertyName._particles))
    {
      this._particles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBlockSparkVfx.PropertyName._specks))
    {
      this._specks = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NBlockSparkVfx.PropertyName._creatureNode))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._creatureNode = VariantUtils.ConvertTo<NCreature>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NBlockSparkVfx.PropertyName._particles))
    {
      value = VariantUtils.CreateFromArray<GpuParticles2D>(this._particles);
      return true;
    }
    if (StringName.op_Equality(ref name, NBlockSparkVfx.PropertyName._specks))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._specks);
      return true;
    }
    if (!StringName.op_Equality(ref name, NBlockSparkVfx.PropertyName._creatureNode))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<NCreature>(ref this._creatureNode);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 28L, NBlockSparkVfx.PropertyName._particles, (PropertyHint) 23L, "24/34:GPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NBlockSparkVfx.PropertyName._specks, (PropertyHint) 34L, "GPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NBlockSparkVfx.PropertyName._creatureNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NBlockSparkVfx.PropertyName._particles, Variant.CreateFrom<GpuParticles2D>(this._particles));
    info.AddProperty(NBlockSparkVfx.PropertyName._specks, Variant.From<GpuParticles2D>(ref this._specks));
    info.AddProperty(NBlockSparkVfx.PropertyName._creatureNode, Variant.From<NCreature>(ref this._creatureNode));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NBlockSparkVfx.PropertyName._particles, ref variant1))
      this._particles = ((Variant) ref variant1).AsGodotArray<GpuParticles2D>();
    Variant variant2;
    if (info.TryGetProperty(NBlockSparkVfx.PropertyName._specks, ref variant2))
      this._specks = ((Variant) ref variant2).As<GpuParticles2D>();
    Variant variant3;
    if (!info.TryGetProperty(NBlockSparkVfx.PropertyName._creatureNode, ref variant3))
      return;
    this._creatureNode = ((Variant) ref variant3).As<NCreature>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _particles = StringName.op_Implicit(nameof (_particles));
    public static readonly StringName _specks = StringName.op_Implicit(nameof (_specks));
    public static readonly StringName _creatureNode = StringName.op_Implicit(nameof (_creatureNode));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
