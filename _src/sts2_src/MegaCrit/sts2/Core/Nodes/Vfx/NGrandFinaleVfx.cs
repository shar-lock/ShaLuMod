// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NGrandFinaleVfx
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

[ScriptPath("res://src/Core/Nodes/Vfx/NGrandFinaleVfx.cs")]
public class NGrandFinaleVfx : Node2D
{
  public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_grand_finale");
  [Export]
  private Node2D? _spotlight;
  [Export]
  private NParticlesContainer? _spotlightParticles;
  [Export]
  private NParticlesContainer? _anticipationParticles;
  [Export]
  private NParticlesContainer? _slashParticles;
  [Export]
  private NParticlesContainer? _endParticles;
  private CancellationTokenSource? _cts;
  private static readonly float _spotlightDuration = 1.25f;
  private static readonly float _anticipationDuration = 0.25f;
  private static readonly float _slashDuration = 0.125f;
  private static readonly float _hitDuration = 0.0125f;
  public static readonly float totalAnticipationDuration = NGrandFinaleVfx._spotlightDuration + NGrandFinaleVfx._anticipationDuration + NGrandFinaleVfx._slashDuration;

  public static NGrandFinaleVfx? Create(Creature creature)
  {
    if (TestMode.IsOn)
      return (NGrandFinaleVfx) null;
    NCreature creatureNode = NCombatRoom.Instance?.GetCreatureNode(creature);
    return creatureNode != null ? NGrandFinaleVfx.Create(creatureNode.VfxSpawnPosition) : (NGrandFinaleVfx) null;
  }

  public static NGrandFinaleVfx? Create(Vector2 playerPosition)
  {
    if (TestMode.IsOn)
      return (NGrandFinaleVfx) null;
    NGrandFinaleVfx ngrandFinaleVfx = PreloadManager.Cache.GetScene(NGrandFinaleVfx.scenePath).Instantiate<NGrandFinaleVfx>((PackedScene.GenEditState) 0L);
    ngrandFinaleVfx.Initialize(playerPosition);
    return ngrandFinaleVfx;
  }

  private void Initialize(Vector2 playerPosition)
  {
    this._anticipationParticles.GlobalPosition = playerPosition;
    this._slashParticles.GlobalPosition = playerPosition;
    this._endParticles.GlobalPosition = playerPosition;
    ((CanvasItem) this._spotlight).Modulate = new Color(1f, 1f, 1f, 0.0f);
  }

  public override void _Ready() => TaskHelper.RunSafely(this.PlaySequence());

  public override void _ExitTree() => this._cts?.Cancel();

  private async Task PlaySequence()
  {
    this._cts = new CancellationTokenSource();
    NParticlesContainer spotlightParticles = this._spotlightParticles;
    Rect2 viewportRect = ((CanvasItem) this).GetViewportRect();
    Vector2 vector2 = new Vector2(((Rect2) ref viewportRect).Size.X / 2f, 0.0f);
    spotlightParticles.GlobalPosition = vector2;
    ((Node) this).GetTree().CreateTween().TweenProperty((GodotObject) this._spotlight, NodePath.op_Implicit("modulate"), Variant.op_Implicit(new Color(1f, 1f, 1f, 1f)), 1.0);
    this._spotlightParticles.Restart();
    await Cmd.Wait(NGrandFinaleVfx._spotlightDuration, this._cts.Token);
    this._anticipationParticles.Restart();
    await Cmd.Wait(NGrandFinaleVfx._anticipationDuration, this._cts.Token);
    this._slashParticles.Restart();
    await Cmd.Wait(NGrandFinaleVfx._slashDuration, this._cts.Token);
    NGame.Instance?.ScreenShake(ShakeStrength.Strong, ShakeDuration.Normal);
    await Cmd.Wait(NGrandFinaleVfx._hitDuration, this._cts.Token);
    this._endParticles.Restart();
    ((Node) this).GetTree().CreateTween().TweenProperty((GodotObject) this._spotlight, NodePath.op_Implicit("modulate"), Variant.op_Implicit(new Color(1f, 1f, 1f, 0.0f)), 0.5);
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
      new MethodInfo(NGrandFinaleVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node2D"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("playerPosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NGrandFinaleVfx.MethodName.Initialize, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("playerPosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NGrandFinaleVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGrandFinaleVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NGrandFinaleVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NGrandFinaleVfx ngrandFinaleVfx = NGrandFinaleVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NGrandFinaleVfx>(ref ngrandFinaleVfx);
      return true;
    }
    if (StringName.op_Equality(ref method, NGrandFinaleVfx.MethodName.Initialize) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.Initialize(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGrandFinaleVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NGrandFinaleVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
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
    if (StringName.op_Equality(ref method, NGrandFinaleVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NGrandFinaleVfx ngrandFinaleVfx = NGrandFinaleVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NGrandFinaleVfx>(ref ngrandFinaleVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NGrandFinaleVfx.MethodName.Create) || StringName.op_Equality(ref method, NGrandFinaleVfx.MethodName.Initialize) || StringName.op_Equality(ref method, NGrandFinaleVfx.MethodName._Ready) || StringName.op_Equality(ref method, NGrandFinaleVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NGrandFinaleVfx.PropertyName._spotlight))
    {
      this._spotlight = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGrandFinaleVfx.PropertyName._spotlightParticles))
    {
      this._spotlightParticles = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGrandFinaleVfx.PropertyName._anticipationParticles))
    {
      this._anticipationParticles = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGrandFinaleVfx.PropertyName._slashParticles))
    {
      this._slashParticles = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NGrandFinaleVfx.PropertyName._endParticles))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._endParticles = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NGrandFinaleVfx.PropertyName._spotlight))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._spotlight);
      return true;
    }
    if (StringName.op_Equality(ref name, NGrandFinaleVfx.PropertyName._spotlightParticles))
    {
      value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._spotlightParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NGrandFinaleVfx.PropertyName._anticipationParticles))
    {
      value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._anticipationParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NGrandFinaleVfx.PropertyName._slashParticles))
    {
      value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._slashParticles);
      return true;
    }
    if (!StringName.op_Equality(ref name, NGrandFinaleVfx.PropertyName._endParticles))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._endParticles);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NGrandFinaleVfx.PropertyName._spotlight, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NGrandFinaleVfx.PropertyName._spotlightParticles, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NGrandFinaleVfx.PropertyName._anticipationParticles, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NGrandFinaleVfx.PropertyName._slashParticles, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NGrandFinaleVfx.PropertyName._endParticles, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NGrandFinaleVfx.PropertyName._spotlight, Variant.From<Node2D>(ref this._spotlight));
    info.AddProperty(NGrandFinaleVfx.PropertyName._spotlightParticles, Variant.From<NParticlesContainer>(ref this._spotlightParticles));
    info.AddProperty(NGrandFinaleVfx.PropertyName._anticipationParticles, Variant.From<NParticlesContainer>(ref this._anticipationParticles));
    info.AddProperty(NGrandFinaleVfx.PropertyName._slashParticles, Variant.From<NParticlesContainer>(ref this._slashParticles));
    info.AddProperty(NGrandFinaleVfx.PropertyName._endParticles, Variant.From<NParticlesContainer>(ref this._endParticles));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NGrandFinaleVfx.PropertyName._spotlight, ref variant1))
      this._spotlight = ((Variant) ref variant1).As<Node2D>();
    Variant variant2;
    if (info.TryGetProperty(NGrandFinaleVfx.PropertyName._spotlightParticles, ref variant2))
      this._spotlightParticles = ((Variant) ref variant2).As<NParticlesContainer>();
    Variant variant3;
    if (info.TryGetProperty(NGrandFinaleVfx.PropertyName._anticipationParticles, ref variant3))
      this._anticipationParticles = ((Variant) ref variant3).As<NParticlesContainer>();
    Variant variant4;
    if (info.TryGetProperty(NGrandFinaleVfx.PropertyName._slashParticles, ref variant4))
      this._slashParticles = ((Variant) ref variant4).As<NParticlesContainer>();
    Variant variant5;
    if (!info.TryGetProperty(NGrandFinaleVfx.PropertyName._endParticles, ref variant5))
      return;
    this._endParticles = ((Variant) ref variant5).As<NParticlesContainer>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName Initialize = StringName.op_Implicit(nameof (Initialize));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _spotlight = StringName.op_Implicit(nameof (_spotlight));
    public static readonly StringName _spotlightParticles = StringName.op_Implicit(nameof (_spotlightParticles));
    public static readonly StringName _anticipationParticles = StringName.op_Implicit(nameof (_anticipationParticles));
    public static readonly StringName _slashParticles = StringName.op_Implicit(nameof (_slashParticles));
    public static readonly StringName _endParticles = StringName.op_Implicit(nameof (_endParticles));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
