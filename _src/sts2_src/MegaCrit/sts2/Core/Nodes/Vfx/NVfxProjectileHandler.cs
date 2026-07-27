// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NVfxProjectileHandler
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/Utilities/NVfxProjectileHandler.cs")]
public class NVfxProjectileHandler : Node2D
{
  [Export]
  private Curve[] _pathHeightOffsets = Array.Empty<Curve>();
  [Export]
  private Vector2 _heightOffsetRange;
  [Export]
  private Curve[] _movementCurves = Array.Empty<Curve>();
  [Export]
  private Vector2 _travelTimeRange;
  [Export]
  private string _impactParticlesScenePath = "";
  private Vector2 _sourceGlobalPosition;
  private Vector2 _destinationGlobalPosition;
  private string _projectileScenePath;
  private Callable _endAction;
  private NVfxProjectile? _loadedProjectile;

  public static NVfxProjectileHandler? Create(
    string handlerScenePath,
    string projectileScenePath,
    Vector2 sourceGlobalPosition,
    Vector2 destinationGlobalPosition,
    Callable endAction)
  {
    if (TestMode.IsOn)
      return (NVfxProjectileHandler) null;
    NVfxProjectileHandler projectileHandler = PreloadManager.Cache.GetScene(SceneHelper.GetScenePath(handlerScenePath)).Instantiate<NVfxProjectileHandler>((PackedScene.GenEditState) 0L);
    projectileHandler._sourceGlobalPosition = sourceGlobalPosition;
    projectileHandler._destinationGlobalPosition = destinationGlobalPosition;
    projectileHandler._projectileScenePath = projectileScenePath;
    projectileHandler._endAction = endAction;
    return projectileHandler;
  }

  public override void _Ready() => TaskHelper.RunSafely(this.PlaySequence());

  public override void _ExitTree()
  {
    if (this._loadedProjectile == null)
      return;
    ((Node) this._loadedProjectile).QueueFreeSafely();
  }

  private async Task PlaySequence()
  {
    this._loadedProjectile = PreloadManager.Cache.GetScene(SceneHelper.GetScenePath(this._projectileScenePath)).Instantiate<NVfxProjectile>((PackedScene.GenEditState) 0L);
    Curve chosenMovementCurve;
    Curve chosenHeightCurve;
    if (this._loadedProjectile == null)
    {
      chosenMovementCurve = (Curve) null;
      chosenHeightCurve = (Curve) null;
    }
    else
    {
      ((Node) this).AddChildSafely((Node) this._loadedProjectile);
      this._loadedProjectile.GlobalPosition = this._sourceGlobalPosition;
      this._loadedProjectile.SetEmitting(true);
      Vector2 vector2_1 = Vector2.op_Subtraction(this._destinationGlobalPosition, this._sourceGlobalPosition);
      Vector2 vector2_2 = ((Vector2) ref vector2_1).Normalized();
      Vector2 normal = new Vector2(vector2_2.Y, -vector2_2.X);
      if ((double) Vector2.op_Addition(this._sourceGlobalPosition, normal).Y < (double) this._sourceGlobalPosition.Y)
        normal = Vector2.op_Multiply(normal, -1f);
      float num1 = 0.0f;
      float projectileDuration = (float) GD.RandRange((double) this._travelTimeRange.X, (double) this._travelTimeRange.Y);
      float projectileHeightOffset = (float) GD.RandRange((double) this._heightOffsetRange.X, (double) this._heightOffsetRange.Y);
      chosenMovementCurve = this._movementCurves[Mathf.RoundToInt(GD.RandRange(0.0, (double) (this._movementCurves.Length - 1)))];
      chosenHeightCurve = this._pathHeightOffsets[Mathf.RoundToInt(GD.RandRange(0.0, (double) (this._pathHeightOffsets.Length - 1)))];
      float num;
      for (; (double) num1 < (double) projectileDuration; num1 = num + await ((Node) this).AwaitProcessFrame())
      {
        float num2 = num1 / projectileDuration;
        float num3 = chosenMovementCurve.Sample(num2);
        float num4 = chosenHeightCurve.Sample(num2);
        Vector2 vector2_3 = Vector2.op_Addition(((Vector2) ref this._sourceGlobalPosition).Lerp(this._destinationGlobalPosition, num3), Vector2.op_Multiply(Vector2.op_Multiply(normal, projectileHeightOffset), num4));
        if (this._loadedProjectile.AlignToVelocity)
        {
          Vector2 vector2_4 = Vector2.op_Subtraction(vector2_3, this._loadedProjectile.GlobalPosition);
          this._loadedProjectile.GlobalRotation = Mathf.Atan2(vector2_4.Y, vector2_4.X);
        }
        this._loadedProjectile.GlobalPosition = vector2_3;
        num = num1;
      }
      this._loadedProjectile.GlobalPosition = this._destinationGlobalPosition;
      this._loadedProjectile.SetEmitting(false);
      this.SpawnImpactVfx(this._destinationGlobalPosition);
      if ((object) ((Callable) ref this._endAction).Delegate != null)
        ((Callable) ref this._endAction).Call(Array.Empty<Variant>());
      TaskHelper.RunSafely(this.DelayedFree());
      chosenMovementCurve = (Curve) null;
      chosenHeightCurve = (Curve) null;
    }
  }

  private void SpawnImpactVfx(Vector2 spawnPosition)
  {
    if (string.IsNullOrEmpty(this._impactParticlesScenePath))
      return;
    Control vfxContainer = ((Node) this).GetParent<Control>();
    if (NCombatRoom.Instance != null)
      vfxContainer = NCombatRoom.Instance.CombatVfxContainer;
    VfxCmd.PlayVfx(spawnPosition, this._impactParticlesScenePath, vfxContainer);
  }

  private async Task DelayedFree()
  {
    await Cmd.Wait(2f);
    ((Node) this).QueueFreeSafely();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NVfxProjectileHandler.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node2D"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("handlerScenePath"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("projectileScenePath"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("sourceGlobalPosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("destinationGlobalPosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 25L, StringName.op_Implicit("endAction"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NVfxProjectileHandler.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NVfxProjectileHandler.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NVfxProjectileHandler.MethodName.SpawnImpactVfx, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("spawnPosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NVfxProjectileHandler.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 5)
    {
      NVfxProjectileHandler projectileHandler = NVfxProjectileHandler.Create(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[3]), VariantUtils.ConvertTo<Callable>(ref ((NativeVariantPtrArgs) ref args)[4]));
      ret = VariantUtils.CreateFrom<NVfxProjectileHandler>(ref projectileHandler);
      return true;
    }
    if (StringName.op_Equality(ref method, NVfxProjectileHandler.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NVfxProjectileHandler.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NVfxProjectileHandler.MethodName.SpawnImpactVfx) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.SpawnImpactVfx(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NVfxProjectileHandler.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 5)
    {
      NVfxProjectileHandler projectileHandler = NVfxProjectileHandler.Create(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[3]), VariantUtils.ConvertTo<Callable>(ref ((NativeVariantPtrArgs) ref args)[4]));
      ret = VariantUtils.CreateFrom<NVfxProjectileHandler>(ref projectileHandler);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NVfxProjectileHandler.MethodName.Create) || StringName.op_Equality(ref method, NVfxProjectileHandler.MethodName._Ready) || StringName.op_Equality(ref method, NVfxProjectileHandler.MethodName._ExitTree) || StringName.op_Equality(ref method, NVfxProjectileHandler.MethodName.SpawnImpactVfx) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NVfxProjectileHandler.PropertyName._pathHeightOffsets))
    {
      this._pathHeightOffsets = VariantUtils.ConvertToSystemArrayOfGodotObject<Curve>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NVfxProjectileHandler.PropertyName._heightOffsetRange))
    {
      this._heightOffsetRange = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NVfxProjectileHandler.PropertyName._movementCurves))
    {
      this._movementCurves = VariantUtils.ConvertToSystemArrayOfGodotObject<Curve>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NVfxProjectileHandler.PropertyName._travelTimeRange))
    {
      this._travelTimeRange = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NVfxProjectileHandler.PropertyName._impactParticlesScenePath))
    {
      this._impactParticlesScenePath = VariantUtils.ConvertTo<string>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NVfxProjectileHandler.PropertyName._sourceGlobalPosition))
    {
      this._sourceGlobalPosition = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NVfxProjectileHandler.PropertyName._destinationGlobalPosition))
    {
      this._destinationGlobalPosition = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NVfxProjectileHandler.PropertyName._projectileScenePath))
    {
      this._projectileScenePath = VariantUtils.ConvertTo<string>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NVfxProjectileHandler.PropertyName._endAction))
    {
      this._endAction = VariantUtils.ConvertTo<Callable>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NVfxProjectileHandler.PropertyName._loadedProjectile))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._loadedProjectile = VariantUtils.ConvertTo<NVfxProjectile>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NVfxProjectileHandler.PropertyName._pathHeightOffsets))
    {
      value = VariantUtils.CreateFromSystemArrayOfGodotObject((GodotObject[]) this._pathHeightOffsets);
      return true;
    }
    if (StringName.op_Equality(ref name, NVfxProjectileHandler.PropertyName._heightOffsetRange))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._heightOffsetRange);
      return true;
    }
    if (StringName.op_Equality(ref name, NVfxProjectileHandler.PropertyName._movementCurves))
    {
      value = VariantUtils.CreateFromSystemArrayOfGodotObject((GodotObject[]) this._movementCurves);
      return true;
    }
    if (StringName.op_Equality(ref name, NVfxProjectileHandler.PropertyName._travelTimeRange))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._travelTimeRange);
      return true;
    }
    if (StringName.op_Equality(ref name, NVfxProjectileHandler.PropertyName._impactParticlesScenePath))
    {
      value = VariantUtils.CreateFrom<string>(ref this._impactParticlesScenePath);
      return true;
    }
    if (StringName.op_Equality(ref name, NVfxProjectileHandler.PropertyName._sourceGlobalPosition))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._sourceGlobalPosition);
      return true;
    }
    if (StringName.op_Equality(ref name, NVfxProjectileHandler.PropertyName._destinationGlobalPosition))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._destinationGlobalPosition);
      return true;
    }
    if (StringName.op_Equality(ref name, NVfxProjectileHandler.PropertyName._projectileScenePath))
    {
      value = VariantUtils.CreateFrom<string>(ref this._projectileScenePath);
      return true;
    }
    if (StringName.op_Equality(ref name, NVfxProjectileHandler.PropertyName._endAction))
    {
      value = VariantUtils.CreateFrom<Callable>(ref this._endAction);
      return true;
    }
    if (!StringName.op_Equality(ref name, NVfxProjectileHandler.PropertyName._loadedProjectile))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<NVfxProjectile>(ref this._loadedProjectile);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 28L, NVfxProjectileHandler.PropertyName._pathHeightOffsets, (PropertyHint) 23L, "24/17:Curve", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 5L, NVfxProjectileHandler.PropertyName._heightOffsetRange, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 28L, NVfxProjectileHandler.PropertyName._movementCurves, (PropertyHint) 23L, "24/17:Curve", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 5L, NVfxProjectileHandler.PropertyName._travelTimeRange, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 4L, NVfxProjectileHandler.PropertyName._impactParticlesScenePath, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 5L, NVfxProjectileHandler.PropertyName._sourceGlobalPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NVfxProjectileHandler.PropertyName._destinationGlobalPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, NVfxProjectileHandler.PropertyName._projectileScenePath, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 25L, NVfxProjectileHandler.PropertyName._endAction, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NVfxProjectileHandler.PropertyName._loadedProjectile, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NVfxProjectileHandler.PropertyName._pathHeightOffsets, Variant.CreateFrom((GodotObject[]) this._pathHeightOffsets));
    info.AddProperty(NVfxProjectileHandler.PropertyName._heightOffsetRange, Variant.From<Vector2>(ref this._heightOffsetRange));
    info.AddProperty(NVfxProjectileHandler.PropertyName._movementCurves, Variant.CreateFrom((GodotObject[]) this._movementCurves));
    info.AddProperty(NVfxProjectileHandler.PropertyName._travelTimeRange, Variant.From<Vector2>(ref this._travelTimeRange));
    info.AddProperty(NVfxProjectileHandler.PropertyName._impactParticlesScenePath, Variant.From<string>(ref this._impactParticlesScenePath));
    info.AddProperty(NVfxProjectileHandler.PropertyName._sourceGlobalPosition, Variant.From<Vector2>(ref this._sourceGlobalPosition));
    info.AddProperty(NVfxProjectileHandler.PropertyName._destinationGlobalPosition, Variant.From<Vector2>(ref this._destinationGlobalPosition));
    info.AddProperty(NVfxProjectileHandler.PropertyName._projectileScenePath, Variant.From<string>(ref this._projectileScenePath));
    info.AddProperty(NVfxProjectileHandler.PropertyName._endAction, Variant.From<Callable>(ref this._endAction));
    info.AddProperty(NVfxProjectileHandler.PropertyName._loadedProjectile, Variant.From<NVfxProjectile>(ref this._loadedProjectile));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NVfxProjectileHandler.PropertyName._pathHeightOffsets, ref variant1))
      this._pathHeightOffsets = ((Variant) ref variant1).AsGodotObjectArray<Curve>();
    Variant variant2;
    if (info.TryGetProperty(NVfxProjectileHandler.PropertyName._heightOffsetRange, ref variant2))
      this._heightOffsetRange = ((Variant) ref variant2).As<Vector2>();
    Variant variant3;
    if (info.TryGetProperty(NVfxProjectileHandler.PropertyName._movementCurves, ref variant3))
      this._movementCurves = ((Variant) ref variant3).AsGodotObjectArray<Curve>();
    Variant variant4;
    if (info.TryGetProperty(NVfxProjectileHandler.PropertyName._travelTimeRange, ref variant4))
      this._travelTimeRange = ((Variant) ref variant4).As<Vector2>();
    Variant variant5;
    if (info.TryGetProperty(NVfxProjectileHandler.PropertyName._impactParticlesScenePath, ref variant5))
      this._impactParticlesScenePath = ((Variant) ref variant5).As<string>();
    Variant variant6;
    if (info.TryGetProperty(NVfxProjectileHandler.PropertyName._sourceGlobalPosition, ref variant6))
      this._sourceGlobalPosition = ((Variant) ref variant6).As<Vector2>();
    Variant variant7;
    if (info.TryGetProperty(NVfxProjectileHandler.PropertyName._destinationGlobalPosition, ref variant7))
      this._destinationGlobalPosition = ((Variant) ref variant7).As<Vector2>();
    Variant variant8;
    if (info.TryGetProperty(NVfxProjectileHandler.PropertyName._projectileScenePath, ref variant8))
      this._projectileScenePath = ((Variant) ref variant8).As<string>();
    Variant variant9;
    if (info.TryGetProperty(NVfxProjectileHandler.PropertyName._endAction, ref variant9))
      this._endAction = ((Variant) ref variant9).As<Callable>();
    Variant variant10;
    if (!info.TryGetProperty(NVfxProjectileHandler.PropertyName._loadedProjectile, ref variant10))
      return;
    this._loadedProjectile = ((Variant) ref variant10).As<NVfxProjectile>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName SpawnImpactVfx = StringName.op_Implicit(nameof (SpawnImpactVfx));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _pathHeightOffsets = StringName.op_Implicit(nameof (_pathHeightOffsets));
    public static readonly StringName _heightOffsetRange = StringName.op_Implicit(nameof (_heightOffsetRange));
    public static readonly StringName _movementCurves = StringName.op_Implicit(nameof (_movementCurves));
    public static readonly StringName _travelTimeRange = StringName.op_Implicit(nameof (_travelTimeRange));
    public static readonly StringName _impactParticlesScenePath = StringName.op_Implicit(nameof (_impactParticlesScenePath));
    public static readonly StringName _sourceGlobalPosition = StringName.op_Implicit(nameof (_sourceGlobalPosition));
    public static readonly StringName _destinationGlobalPosition = StringName.op_Implicit(nameof (_destinationGlobalPosition));
    public static readonly StringName _projectileScenePath = StringName.op_Implicit(nameof (_projectileScenePath));
    public static readonly StringName _endAction = StringName.op_Implicit(nameof (_endAction));
    public static readonly StringName _loadedProjectile = StringName.op_Implicit(nameof (_loadedProjectile));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
