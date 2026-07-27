// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NHyperbeamVfx
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
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NHyperbeamVfx.cs")]
public class NHyperbeamVfx : Node2D
{
  private const string _hyperbeamSfxPath = "event:/sfx/characters/defect/defect_hyperbeam";
  public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_hyperbeam");
  [Export]
  private Array<GpuParticles2D> _anticipationParticles = new Array<GpuParticles2D>();
  [Export]
  private Array<GpuParticles2D> _laserParticles = new Array<GpuParticles2D>();
  [Export]
  private Array<GpuParticles2D> _laserEndParticles = new Array<GpuParticles2D>();
  [Export]
  private Line2D? _laserLine;
  [Export]
  private Node2D? _laserContainer;
  public static readonly float hyperbeamAnticipationDuration = 0.525f;
  public static readonly float hyperbeamLaserDuration = 0.5f;
  private CancellationTokenSource? _cts;

  public override void _ExitTree() => this._cts?.Cancel();

  public static NHyperbeamVfx? Create(Creature owner, Creature target)
  {
    if (TestMode.IsOn)
      return (NHyperbeamVfx) null;
    NCreature creatureNode1 = NCombatRoom.Instance?.GetCreatureNode(owner);
    NCreature creatureNode2 = NCombatRoom.Instance?.GetCreatureNode(target);
    if (creatureNode2 == null || creatureNode1 == null)
      return (NHyperbeamVfx) null;
    Vector2 defectEyePosition = creatureNode1.VfxSpawnPosition;
    Player player = owner.Player;
    if (player != null && player.Character is Defect)
      defectEyePosition = Vector2.op_Addition(defectEyePosition, Defect.EyelineOffset);
    return NHyperbeamVfx.Create(defectEyePosition, creatureNode2.VfxSpawnPosition);
  }

  public static NHyperbeamVfx? Create(Vector2 defectEyePosition, Vector2 mainTargetCenterPosition)
  {
    if (TestMode.IsOn)
      return (NHyperbeamVfx) null;
    NHyperbeamVfx nhyperbeamVfx = PreloadManager.Cache.GetScene(NHyperbeamVfx.scenePath).Instantiate<NHyperbeamVfx>((PackedScene.GenEditState) 0L);
    nhyperbeamVfx.GlobalPosition = defectEyePosition;
    nhyperbeamVfx.ApplyRotation(defectEyePosition, mainTargetCenterPosition);
    return nhyperbeamVfx;
  }

  public void ApplyRotation(Vector2 sourcePosition, Vector2 targetPosition)
  {
    Vector2 vector2 = Vector2.op_Subtraction(targetPosition, sourcePosition);
    this.RotationDegrees = Mathf.RadToDeg(Mathf.Atan2(vector2.Y, vector2.X));
  }

  public override void _Ready() => TaskHelper.RunSafely(this.PlaySequence());

  private void ShowLaser(bool showing)
  {
    for (int index = 0; index < this._laserParticles.Count; ++index)
    {
      ((CanvasItem) this._laserParticles[index]).Visible = showing;
      if (showing)
        this._laserParticles[index].Restart();
    }
    ((CanvasItem) this._laserLine).Visible = showing;
    ((CanvasItem) this._laserContainer).Visible = showing;
  }

  private async Task PlaySequence()
  {
    this._cts = new CancellationTokenSource();
    SfxCmd.Play("event:/sfx/characters/defect/defect_hyperbeam");
    this.ShowLaser(false);
    for (int index = 0; index < this._anticipationParticles.Count; ++index)
      this._anticipationParticles[index].Restart();
    await Cmd.Wait(NHyperbeamVfx.hyperbeamAnticipationDuration, this._cts.Token);
    this.ShowLaser(true);
    NGame.Instance?.ScreenShake(ShakeStrength.Medium, ShakeDuration.Normal);
    await Cmd.Wait(NHyperbeamVfx.hyperbeamLaserDuration, this._cts.Token);
    this.ShowLaser(false);
    for (int index = 0; index < this._laserEndParticles.Count; ++index)
      this._laserEndParticles[index].Restart();
    NGame.Instance?.ScreenShake(ShakeStrength.Strong, ShakeDuration.Short);
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
      new MethodInfo(NHyperbeamVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHyperbeamVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node2D"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("defectEyePosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("mainTargetCenterPosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NHyperbeamVfx.MethodName.ApplyRotation, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("sourcePosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("targetPosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NHyperbeamVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHyperbeamVfx.MethodName.ShowLaser, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("showing"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NHyperbeamVfx.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHyperbeamVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      NHyperbeamVfx nhyperbeamVfx = NHyperbeamVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<NHyperbeamVfx>(ref nhyperbeamVfx);
      return true;
    }
    if (StringName.op_Equality(ref method, NHyperbeamVfx.MethodName.ApplyRotation) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.ApplyRotation(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHyperbeamVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NHyperbeamVfx.MethodName.ShowLaser) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.ShowLaser(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NHyperbeamVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      NHyperbeamVfx nhyperbeamVfx = NHyperbeamVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<NHyperbeamVfx>(ref nhyperbeamVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NHyperbeamVfx.MethodName._ExitTree) || StringName.op_Equality(ref method, NHyperbeamVfx.MethodName.Create) || StringName.op_Equality(ref method, NHyperbeamVfx.MethodName.ApplyRotation) || StringName.op_Equality(ref method, NHyperbeamVfx.MethodName._Ready) || StringName.op_Equality(ref method, NHyperbeamVfx.MethodName.ShowLaser) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NHyperbeamVfx.PropertyName._anticipationParticles))
    {
      this._anticipationParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHyperbeamVfx.PropertyName._laserParticles))
    {
      this._laserParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHyperbeamVfx.PropertyName._laserEndParticles))
    {
      this._laserEndParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHyperbeamVfx.PropertyName._laserLine))
    {
      this._laserLine = VariantUtils.ConvertTo<Line2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NHyperbeamVfx.PropertyName._laserContainer))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._laserContainer = VariantUtils.ConvertTo<Node2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NHyperbeamVfx.PropertyName._anticipationParticles))
    {
      value = VariantUtils.CreateFromArray<GpuParticles2D>(this._anticipationParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NHyperbeamVfx.PropertyName._laserParticles))
    {
      value = VariantUtils.CreateFromArray<GpuParticles2D>(this._laserParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NHyperbeamVfx.PropertyName._laserEndParticles))
    {
      value = VariantUtils.CreateFromArray<GpuParticles2D>(this._laserEndParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NHyperbeamVfx.PropertyName._laserLine))
    {
      value = VariantUtils.CreateFrom<Line2D>(ref this._laserLine);
      return true;
    }
    if (!StringName.op_Equality(ref name, NHyperbeamVfx.PropertyName._laserContainer))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Node2D>(ref this._laserContainer);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 28L, NHyperbeamVfx.PropertyName._anticipationParticles, (PropertyHint) 23L, "24/34:GPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 28L, NHyperbeamVfx.PropertyName._laserParticles, (PropertyHint) 23L, "24/34:GPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 28L, NHyperbeamVfx.PropertyName._laserEndParticles, (PropertyHint) 23L, "24/34:GPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NHyperbeamVfx.PropertyName._laserLine, (PropertyHint) 34L, "Line2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NHyperbeamVfx.PropertyName._laserContainer, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NHyperbeamVfx.PropertyName._anticipationParticles, Variant.CreateFrom<GpuParticles2D>(this._anticipationParticles));
    info.AddProperty(NHyperbeamVfx.PropertyName._laserParticles, Variant.CreateFrom<GpuParticles2D>(this._laserParticles));
    info.AddProperty(NHyperbeamVfx.PropertyName._laserEndParticles, Variant.CreateFrom<GpuParticles2D>(this._laserEndParticles));
    info.AddProperty(NHyperbeamVfx.PropertyName._laserLine, Variant.From<Line2D>(ref this._laserLine));
    info.AddProperty(NHyperbeamVfx.PropertyName._laserContainer, Variant.From<Node2D>(ref this._laserContainer));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NHyperbeamVfx.PropertyName._anticipationParticles, ref variant1))
      this._anticipationParticles = ((Variant) ref variant1).AsGodotArray<GpuParticles2D>();
    Variant variant2;
    if (info.TryGetProperty(NHyperbeamVfx.PropertyName._laserParticles, ref variant2))
      this._laserParticles = ((Variant) ref variant2).AsGodotArray<GpuParticles2D>();
    Variant variant3;
    if (info.TryGetProperty(NHyperbeamVfx.PropertyName._laserEndParticles, ref variant3))
      this._laserEndParticles = ((Variant) ref variant3).AsGodotArray<GpuParticles2D>();
    Variant variant4;
    if (info.TryGetProperty(NHyperbeamVfx.PropertyName._laserLine, ref variant4))
      this._laserLine = ((Variant) ref variant4).As<Line2D>();
    Variant variant5;
    if (!info.TryGetProperty(NHyperbeamVfx.PropertyName._laserContainer, ref variant5))
      return;
    this._laserContainer = ((Variant) ref variant5).As<Node2D>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName ApplyRotation = StringName.op_Implicit(nameof (ApplyRotation));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName ShowLaser = StringName.op_Implicit(nameof (ShowLaser));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _anticipationParticles = StringName.op_Implicit(nameof (_anticipationParticles));
    public static readonly StringName _laserParticles = StringName.op_Implicit(nameof (_laserParticles));
    public static readonly StringName _laserEndParticles = StringName.op_Implicit(nameof (_laserEndParticles));
    public static readonly StringName _laserLine = StringName.op_Implicit(nameof (_laserLine));
    public static readonly StringName _laserContainer = StringName.op_Implicit(nameof (_laserContainer));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
