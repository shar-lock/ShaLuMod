// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NHyperbeamImpactVfx
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
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NHyperbeamImpactVfx.cs")]
public class NHyperbeamImpactVfx : Node2D
{
  public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_hyperbeam_impact");
  [Export]
  private Array<GpuParticles2D> _impactStartParticles = new Array<GpuParticles2D>();
  [Export]
  private Array<GpuParticles2D> _impactEndParticles = new Array<GpuParticles2D>();
  private CancellationTokenSource? _cts;

  public override void _ExitTree() => this._cts?.Cancel();

  public static NHyperbeamImpactVfx? Create(Creature owner, Creature target)
  {
    if (TestMode.IsOn)
      return (NHyperbeamImpactVfx) null;
    NCreature creatureNode1 = NCombatRoom.Instance?.GetCreatureNode(owner);
    NCreature creatureNode2 = NCombatRoom.Instance?.GetCreatureNode(target);
    if (creatureNode2 == null || creatureNode1 == null)
      return (NHyperbeamImpactVfx) null;
    Vector2 hyperbeamSourcePosition = creatureNode1.VfxSpawnPosition;
    Player player = owner.Player;
    if (player != null && player.Character is Defect)
      hyperbeamSourcePosition = Vector2.op_Addition(hyperbeamSourcePosition, Defect.EyelineOffset);
    return NHyperbeamImpactVfx.Create(hyperbeamSourcePosition, creatureNode2.VfxSpawnPosition);
  }

  public static NHyperbeamImpactVfx? Create(
    Vector2 hyperbeamSourcePosition,
    Vector2 targetCenterPosition)
  {
    if (TestMode.IsOn)
      return (NHyperbeamImpactVfx) null;
    NHyperbeamImpactVfx nhyperbeamImpactVfx = PreloadManager.Cache.GetScene(NHyperbeamImpactVfx.scenePath).Instantiate<NHyperbeamImpactVfx>((PackedScene.GenEditState) 0L);
    nhyperbeamImpactVfx.GlobalPosition = targetCenterPosition;
    nhyperbeamImpactVfx.ApplyRotation(hyperbeamSourcePosition, targetCenterPosition);
    return nhyperbeamImpactVfx;
  }

  public void ApplyRotation(Vector2 sourcePosition, Vector2 targetPosition)
  {
    Vector2 vector2 = Vector2.op_Subtraction(targetPosition, sourcePosition);
    this.RotationDegrees = Mathf.RadToDeg(Mathf.Atan2(vector2.Y, vector2.X));
  }

  public override void _Ready() => TaskHelper.RunSafely(this.PlaySequence());

  private async Task PlaySequence()
  {
    this._cts = new CancellationTokenSource();
    for (int index = 0; index < this._impactStartParticles.Count; ++index)
    {
      ((CanvasItem) this._impactStartParticles[index]).Visible = true;
      this._impactStartParticles[index].Restart();
    }
    await Cmd.Wait(NHyperbeamVfx.hyperbeamLaserDuration, this._cts.Token);
    for (int index = 0; index < this._impactStartParticles.Count; ++index)
      ((CanvasItem) this._impactStartParticles[index]).Visible = false;
    for (int index = 0; index < this._impactEndParticles.Count; ++index)
      this._impactEndParticles[index].Restart();
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
      new MethodInfo(NHyperbeamImpactVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHyperbeamImpactVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node2D"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("hyperbeamSourcePosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("targetCenterPosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NHyperbeamImpactVfx.MethodName.ApplyRotation, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("sourcePosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("targetPosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NHyperbeamImpactVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NHyperbeamImpactVfx.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHyperbeamImpactVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      NHyperbeamImpactVfx nhyperbeamImpactVfx = NHyperbeamImpactVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<NHyperbeamImpactVfx>(ref nhyperbeamImpactVfx);
      return true;
    }
    if (StringName.op_Equality(ref method, NHyperbeamImpactVfx.MethodName.ApplyRotation) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.ApplyRotation(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NHyperbeamImpactVfx.MethodName._Ready) || ((NativeVariantPtrArgs) ref args).Count != 0)
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
    if (StringName.op_Equality(ref method, NHyperbeamImpactVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      NHyperbeamImpactVfx nhyperbeamImpactVfx = NHyperbeamImpactVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<NHyperbeamImpactVfx>(ref nhyperbeamImpactVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NHyperbeamImpactVfx.MethodName._ExitTree) || StringName.op_Equality(ref method, NHyperbeamImpactVfx.MethodName.Create) || StringName.op_Equality(ref method, NHyperbeamImpactVfx.MethodName.ApplyRotation) || StringName.op_Equality(ref method, NHyperbeamImpactVfx.MethodName._Ready) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NHyperbeamImpactVfx.PropertyName._impactStartParticles))
    {
      this._impactStartParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NHyperbeamImpactVfx.PropertyName._impactEndParticles))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._impactEndParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NHyperbeamImpactVfx.PropertyName._impactStartParticles))
    {
      value = VariantUtils.CreateFromArray<GpuParticles2D>(this._impactStartParticles);
      return true;
    }
    if (!StringName.op_Equality(ref name, NHyperbeamImpactVfx.PropertyName._impactEndParticles))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFromArray<GpuParticles2D>(this._impactEndParticles);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 28L, NHyperbeamImpactVfx.PropertyName._impactStartParticles, (PropertyHint) 23L, "24/34:GPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 28L, NHyperbeamImpactVfx.PropertyName._impactEndParticles, (PropertyHint) 23L, "24/34:GPUParticles2D", (PropertyUsageFlags) 4102L, true)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NHyperbeamImpactVfx.PropertyName._impactStartParticles, Variant.CreateFrom<GpuParticles2D>(this._impactStartParticles));
    info.AddProperty(NHyperbeamImpactVfx.PropertyName._impactEndParticles, Variant.CreateFrom<GpuParticles2D>(this._impactEndParticles));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NHyperbeamImpactVfx.PropertyName._impactStartParticles, ref variant1))
      this._impactStartParticles = ((Variant) ref variant1).AsGodotArray<GpuParticles2D>();
    Variant variant2;
    if (!info.TryGetProperty(NHyperbeamImpactVfx.PropertyName._impactEndParticles, ref variant2))
      return;
    this._impactEndParticles = ((Variant) ref variant2).AsGodotArray<GpuParticles2D>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName ApplyRotation = StringName.op_Implicit(nameof (ApplyRotation));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _impactStartParticles = StringName.op_Implicit(nameof (_impactStartParticles));
    public static readonly StringName _impactEndParticles = StringName.op_Implicit(nameof (_impactEndParticles));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
