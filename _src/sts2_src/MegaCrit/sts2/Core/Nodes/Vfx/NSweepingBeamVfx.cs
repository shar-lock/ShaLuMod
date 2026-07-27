// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NSweepingBeamVfx
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
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NSweepingBeamVfx.cs")]
public class NSweepingBeamVfx : Node2D
{
  public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_sweeping_beam");
  [Export]
  private Array<GpuParticles2D> _emittingParticles = new Array<GpuParticles2D>();
  [Export]
  private Array<GpuParticles2D> _startParticles = new Array<GpuParticles2D>();
  [Export]
  private Array<GpuParticles2D> _endParticles = new Array<GpuParticles2D>();
  [Export]
  private Array<GpuParticles2D> _sweepingParticles = new Array<GpuParticles2D>();
  [Export]
  private Curve? _sweepingIndexCurve;
  [Export]
  private float _sweepDuration = 0.65f;
  private Array<Vector2> _targetCenterPositions = new Array<Vector2>();
  private CancellationTokenSource? _cts;

  public override void _ExitTree() => this._cts?.Cancel();

  public static NSweepingBeamVfx? Create(Creature owner, List<Creature> targets)
  {
    if (TestMode.IsOn)
      return (NSweepingBeamVfx) null;
    NCreature creatureNode1 = NCombatRoom.Instance?.GetCreatureNode(owner);
    if (creatureNode1 == null)
      return (NSweepingBeamVfx) null;
    Vector2 defectEyeCenter = creatureNode1.VfxSpawnPosition;
    Player player = owner.Player;
    if (player != null && player.Character is Defect)
      defectEyeCenter = Vector2.op_Addition(defectEyeCenter, Defect.EyelineOffset);
    Array<Vector2> targetCenterPositions = new Array<Vector2>();
    foreach (Creature target in targets)
    {
      NCreature creatureNode2 = NCombatRoom.Instance?.GetCreatureNode(target);
      if (creatureNode2 != null)
        targetCenterPositions.Add(creatureNode2.VfxSpawnPosition);
    }
    return NSweepingBeamVfx.Create(defectEyeCenter, targetCenterPositions);
  }

  public static NSweepingBeamVfx? Create(
    Vector2 defectEyeCenter,
    Array<Vector2> targetCenterPositions)
  {
    if (TestMode.IsOn)
      return (NSweepingBeamVfx) null;
    NSweepingBeamVfx nsweepingBeamVfx = PreloadManager.Cache.GetScene(NSweepingBeamVfx.scenePath).Instantiate<NSweepingBeamVfx>((PackedScene.GenEditState) 0L);
    nsweepingBeamVfx.GlobalPosition = defectEyeCenter;
    nsweepingBeamVfx._targetCenterPositions = targetCenterPositions;
    return nsweepingBeamVfx;
  }

  public override void _Ready()
  {
    for (int index = 0; index < this._emittingParticles.Count; ++index)
      this._emittingParticles[index].Emitting = false;
    TaskHelper.RunSafely(this.PlaySequence());
  }

  private async Task PlaySequence()
  {
    this._cts = new CancellationTokenSource();
    double timer = 0.0;
    bool playedImpactParticles = false;
    for (int index = 0; index < this._startParticles.Count; ++index)
      this._startParticles[index].Restart();
    for (int index = 0; index < this._emittingParticles.Count; ++index)
    {
      this._emittingParticles[index].Restart();
      this._emittingParticles[index].Emitting = true;
    }
    int previousSweepIndex = -1;
    while (timer < (double) this._sweepDuration)
    {
      double processDeltaTime = ((Node) this).GetProcessDeltaTime();
      float num1 = (float) timer / this._sweepDuration;
      int num2 = Mathf.FloorToInt(this._sweepingIndexCurve.Sample(num1));
      if (previousSweepIndex != num2 && num2 >= 0 && num2 < this._sweepingParticles.Count)
      {
        this._sweepingParticles[num2].Restart();
        previousSweepIndex = num2;
      }
      if ((double) num1 >= 0.5 && !playedImpactParticles)
      {
        playedImpactParticles = true;
        NGame.Instance?.ScreenShake(ShakeStrength.Medium, ShakeDuration.Normal);
        for (int index = 0; index < this._targetCenterPositions.Count; ++index)
        {
          NSweepingBeamImpactVfx child = NSweepingBeamImpactVfx.Create(this._targetCenterPositions[index]);
          ((Node) ((Node) this).GetTree().Root).AddChildSafely((Node) child);
        }
      }
      timer += processDeltaTime;
      double num3 = (double) await ((Node) this).AwaitProcessFrame();
    }
    for (int index = 0; index < this._endParticles.Count; ++index)
      this._endParticles[index].Restart();
    for (int index = 0; index < this._emittingParticles.Count; ++index)
      this._emittingParticles[index].Emitting = false;
    await Cmd.Wait(2f, this._cts.Token);
    ((Node) this).QueueFreeSafely();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NSweepingBeamVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSweepingBeamVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node2D"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("defectEyeCenter"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 28L, StringName.op_Implicit("targetCenterPositions"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NSweepingBeamVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSweepingBeamVfx.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSweepingBeamVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      NSweepingBeamVfx nsweepingBeamVfx = NSweepingBeamVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertToArray<Vector2>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<NSweepingBeamVfx>(ref nsweepingBeamVfx);
      return true;
    }
    if (!StringName.op_Equality(ref method, NSweepingBeamVfx.MethodName._Ready) || ((NativeVariantPtrArgs) ref args).Count != 0)
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
    if (StringName.op_Equality(ref method, NSweepingBeamVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      NSweepingBeamVfx nsweepingBeamVfx = NSweepingBeamVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertToArray<Vector2>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<NSweepingBeamVfx>(ref nsweepingBeamVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSweepingBeamVfx.MethodName._ExitTree) || StringName.op_Equality(ref method, NSweepingBeamVfx.MethodName.Create) || StringName.op_Equality(ref method, NSweepingBeamVfx.MethodName._Ready) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSweepingBeamVfx.PropertyName._emittingParticles))
    {
      this._emittingParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSweepingBeamVfx.PropertyName._startParticles))
    {
      this._startParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSweepingBeamVfx.PropertyName._endParticles))
    {
      this._endParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSweepingBeamVfx.PropertyName._sweepingParticles))
    {
      this._sweepingParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSweepingBeamVfx.PropertyName._sweepingIndexCurve))
    {
      this._sweepingIndexCurve = VariantUtils.ConvertTo<Curve>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSweepingBeamVfx.PropertyName._sweepDuration))
    {
      this._sweepDuration = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSweepingBeamVfx.PropertyName._targetCenterPositions))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._targetCenterPositions = VariantUtils.ConvertToArray<Vector2>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSweepingBeamVfx.PropertyName._emittingParticles))
    {
      value = VariantUtils.CreateFromArray<GpuParticles2D>(this._emittingParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NSweepingBeamVfx.PropertyName._startParticles))
    {
      value = VariantUtils.CreateFromArray<GpuParticles2D>(this._startParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NSweepingBeamVfx.PropertyName._endParticles))
    {
      value = VariantUtils.CreateFromArray<GpuParticles2D>(this._endParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NSweepingBeamVfx.PropertyName._sweepingParticles))
    {
      value = VariantUtils.CreateFromArray<GpuParticles2D>(this._sweepingParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NSweepingBeamVfx.PropertyName._sweepingIndexCurve))
    {
      value = VariantUtils.CreateFrom<Curve>(ref this._sweepingIndexCurve);
      return true;
    }
    if (StringName.op_Equality(ref name, NSweepingBeamVfx.PropertyName._sweepDuration))
    {
      value = VariantUtils.CreateFrom<float>(ref this._sweepDuration);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSweepingBeamVfx.PropertyName._targetCenterPositions))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFromArray<Vector2>(this._targetCenterPositions);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 28L, NSweepingBeamVfx.PropertyName._emittingParticles, (PropertyHint) 23L, "24/34:GPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 28L, NSweepingBeamVfx.PropertyName._startParticles, (PropertyHint) 23L, "24/34:GPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 28L, NSweepingBeamVfx.PropertyName._endParticles, (PropertyHint) 23L, "24/34:GPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 28L, NSweepingBeamVfx.PropertyName._sweepingParticles, (PropertyHint) 23L, "24/34:GPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NSweepingBeamVfx.PropertyName._sweepingIndexCurve, (PropertyHint) 17L, "Curve", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NSweepingBeamVfx.PropertyName._sweepDuration, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 28L, NSweepingBeamVfx.PropertyName._targetCenterPositions, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NSweepingBeamVfx.PropertyName._emittingParticles, Variant.CreateFrom<GpuParticles2D>(this._emittingParticles));
    info.AddProperty(NSweepingBeamVfx.PropertyName._startParticles, Variant.CreateFrom<GpuParticles2D>(this._startParticles));
    info.AddProperty(NSweepingBeamVfx.PropertyName._endParticles, Variant.CreateFrom<GpuParticles2D>(this._endParticles));
    info.AddProperty(NSweepingBeamVfx.PropertyName._sweepingParticles, Variant.CreateFrom<GpuParticles2D>(this._sweepingParticles));
    info.AddProperty(NSweepingBeamVfx.PropertyName._sweepingIndexCurve, Variant.From<Curve>(ref this._sweepingIndexCurve));
    info.AddProperty(NSweepingBeamVfx.PropertyName._sweepDuration, Variant.From<float>(ref this._sweepDuration));
    info.AddProperty(NSweepingBeamVfx.PropertyName._targetCenterPositions, Variant.CreateFrom<Vector2>(this._targetCenterPositions));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NSweepingBeamVfx.PropertyName._emittingParticles, ref variant1))
      this._emittingParticles = ((Variant) ref variant1).AsGodotArray<GpuParticles2D>();
    Variant variant2;
    if (info.TryGetProperty(NSweepingBeamVfx.PropertyName._startParticles, ref variant2))
      this._startParticles = ((Variant) ref variant2).AsGodotArray<GpuParticles2D>();
    Variant variant3;
    if (info.TryGetProperty(NSweepingBeamVfx.PropertyName._endParticles, ref variant3))
      this._endParticles = ((Variant) ref variant3).AsGodotArray<GpuParticles2D>();
    Variant variant4;
    if (info.TryGetProperty(NSweepingBeamVfx.PropertyName._sweepingParticles, ref variant4))
      this._sweepingParticles = ((Variant) ref variant4).AsGodotArray<GpuParticles2D>();
    Variant variant5;
    if (info.TryGetProperty(NSweepingBeamVfx.PropertyName._sweepingIndexCurve, ref variant5))
      this._sweepingIndexCurve = ((Variant) ref variant5).As<Curve>();
    Variant variant6;
    if (info.TryGetProperty(NSweepingBeamVfx.PropertyName._sweepDuration, ref variant6))
      this._sweepDuration = ((Variant) ref variant6).As<float>();
    Variant variant7;
    if (!info.TryGetProperty(NSweepingBeamVfx.PropertyName._targetCenterPositions, ref variant7))
      return;
    this._targetCenterPositions = ((Variant) ref variant7).AsGodotArray<Vector2>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _emittingParticles = StringName.op_Implicit(nameof (_emittingParticles));
    public static readonly StringName _startParticles = StringName.op_Implicit(nameof (_startParticles));
    public static readonly StringName _endParticles = StringName.op_Implicit(nameof (_endParticles));
    public static readonly StringName _sweepingParticles = StringName.op_Implicit(nameof (_sweepingParticles));
    public static readonly StringName _sweepingIndexCurve = StringName.op_Implicit(nameof (_sweepingIndexCurve));
    public static readonly StringName _sweepDuration = StringName.op_Implicit(nameof (_sweepDuration));
    public static readonly StringName _targetCenterPositions = StringName.op_Implicit(nameof (_targetCenterPositions));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
