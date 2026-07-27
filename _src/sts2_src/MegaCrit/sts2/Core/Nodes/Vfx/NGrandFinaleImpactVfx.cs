// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NGrandFinaleImpactVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
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

[ScriptPath("res://src/Core/Nodes/Vfx/NGrandFinaleImpactVfx.cs")]
public class NGrandFinaleImpactVfx : Node2D
{
  public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_grand_finale_impact");
  [Export]
  private NParticlesContainer? _centerParticles;
  [Export]
  private NParticlesContainer? _groundParticles;
  private CancellationTokenSource? _cts;

  public static NGrandFinaleImpactVfx? Create(Creature creature)
  {
    if (TestMode.IsOn)
      return (NGrandFinaleImpactVfx) null;
    NCreature creatureNode = NCombatRoom.Instance?.GetCreatureNode(creature);
    return creatureNode != null ? NGrandFinaleImpactVfx.Create(creatureNode.VfxSpawnPosition, creatureNode.GetBottomOfHitbox()) : (NGrandFinaleImpactVfx) null;
  }

  public static NGrandFinaleImpactVfx? Create(
    Vector2 targetCenterPosition,
    Vector2 targetGroundPosition)
  {
    if (TestMode.IsOn)
      return (NGrandFinaleImpactVfx) null;
    NGrandFinaleImpactVfx ngrandFinaleImpactVfx = PreloadManager.Cache.GetScene(NGrandFinaleImpactVfx.scenePath).Instantiate<NGrandFinaleImpactVfx>((PackedScene.GenEditState) 0L);
    ngrandFinaleImpactVfx.InitializePositions(targetCenterPosition, targetGroundPosition);
    return ngrandFinaleImpactVfx;
  }

  private void InitializePositions(Vector2 targetCenterPosition, Vector2 targetGroundPosition)
  {
    this._centerParticles.GlobalPosition = targetCenterPosition;
    this._groundParticles.GlobalPosition = targetGroundPosition;
  }

  public override void _Ready() => TaskHelper.RunSafely(this.PlaySequence());

  public override void _ExitTree() => this._cts?.Cancel();

  private async Task PlaySequence()
  {
    this._cts = new CancellationTokenSource();
    this._centerParticles.Restart();
    this._groundParticles.Restart();
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
      new MethodInfo(NGrandFinaleImpactVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node2D"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("targetCenterPosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("targetGroundPosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NGrandFinaleImpactVfx.MethodName.InitializePositions, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("targetCenterPosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("targetGroundPosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NGrandFinaleImpactVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGrandFinaleImpactVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NGrandFinaleImpactVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      NGrandFinaleImpactVfx ngrandFinaleImpactVfx = NGrandFinaleImpactVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<NGrandFinaleImpactVfx>(ref ngrandFinaleImpactVfx);
      return true;
    }
    if (StringName.op_Equality(ref method, NGrandFinaleImpactVfx.MethodName.InitializePositions) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.InitializePositions(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGrandFinaleImpactVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NGrandFinaleImpactVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
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
    if (StringName.op_Equality(ref method, NGrandFinaleImpactVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      NGrandFinaleImpactVfx ngrandFinaleImpactVfx = NGrandFinaleImpactVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<NGrandFinaleImpactVfx>(ref ngrandFinaleImpactVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NGrandFinaleImpactVfx.MethodName.Create) || StringName.op_Equality(ref method, NGrandFinaleImpactVfx.MethodName.InitializePositions) || StringName.op_Equality(ref method, NGrandFinaleImpactVfx.MethodName._Ready) || StringName.op_Equality(ref method, NGrandFinaleImpactVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NGrandFinaleImpactVfx.PropertyName._centerParticles))
    {
      this._centerParticles = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NGrandFinaleImpactVfx.PropertyName._groundParticles))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._groundParticles = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NGrandFinaleImpactVfx.PropertyName._centerParticles))
    {
      value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._centerParticles);
      return true;
    }
    if (!StringName.op_Equality(ref name, NGrandFinaleImpactVfx.PropertyName._groundParticles))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._groundParticles);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NGrandFinaleImpactVfx.PropertyName._centerParticles, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NGrandFinaleImpactVfx.PropertyName._groundParticles, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NGrandFinaleImpactVfx.PropertyName._centerParticles, Variant.From<NParticlesContainer>(ref this._centerParticles));
    info.AddProperty(NGrandFinaleImpactVfx.PropertyName._groundParticles, Variant.From<NParticlesContainer>(ref this._groundParticles));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NGrandFinaleImpactVfx.PropertyName._centerParticles, ref variant1))
      this._centerParticles = ((Variant) ref variant1).As<NParticlesContainer>();
    Variant variant2;
    if (!info.TryGetProperty(NGrandFinaleImpactVfx.PropertyName._groundParticles, ref variant2))
      return;
    this._groundParticles = ((Variant) ref variant2).As<NParticlesContainer>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName InitializePositions = StringName.op_Implicit(nameof (InitializePositions));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _centerParticles = StringName.op_Implicit(nameof (_centerParticles));
    public static readonly StringName _groundParticles = StringName.op_Implicit(nameof (_groundParticles));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
