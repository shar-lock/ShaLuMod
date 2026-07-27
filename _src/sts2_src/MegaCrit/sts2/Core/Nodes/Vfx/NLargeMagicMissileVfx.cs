// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NLargeMagicMissileVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NLargeMagicMissileVfx.cs")]
public class NLargeMagicMissileVfx : Node2D
{
  public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_large_magic_missile");
  [Export]
  private Array<GpuParticles2D> _anticipationParticles = new Array<GpuParticles2D>();
  [Export]
  private Array<GpuParticles2D> _projectileStartParticles = new Array<GpuParticles2D>();
  [Export]
  private Array<GpuParticles2D> _projectileParticles = new Array<GpuParticles2D>();
  [Export]
  private Array<GpuParticles2D> _impactParticles = new Array<GpuParticles2D>();
  [Export]
  private Array<GpuParticles2D> _modulateParticles = new Array<GpuParticles2D>();
  [Export]
  private Node2D? _anticipationContainer;
  [Export]
  private float _anticipationDuration = 0.2f;
  [Export]
  private Node2D? _projectileContainer;
  [Export]
  private Node2D? _projectileStartPoint;
  [Export]
  private Node2D? _projectileEndPoint;
  [Export]
  private float _projectileOffset = 100f;
  private CancellationTokenSource? _cts;

  [field: Export]
  public float WaitTime { get; private set; } = 0.2f;

  public static NLargeMagicMissileVfx? Create(Vector2 targetFloorPosition, Color tint)
  {
    if (TestMode.IsOn)
      return (NLargeMagicMissileVfx) null;
    NLargeMagicMissileVfx nlargeMagicMissileVfx = PreloadManager.Cache.GetScene(NLargeMagicMissileVfx.scenePath).Instantiate<NLargeMagicMissileVfx>((PackedScene.GenEditState) 0L);
    nlargeMagicMissileVfx.GlobalPosition = targetFloorPosition;
    nlargeMagicMissileVfx.Initialize();
    nlargeMagicMissileVfx.ModulateParticles(tint);
    return nlargeMagicMissileVfx;
  }

  private Vector2 GetProjectileDirection()
  {
    Vector3 vector3 = Quaternion.op_Multiply(Quaternion.FromEuler(new Vector3(0.0f, 0.0f, Mathf.DegToRad(-30f))), Vector3.Up);
    Vector2 vector2 = new Vector2(vector3.X, vector3.Y);
    return ((Vector2) ref vector2).Normalized();
  }

  private Vector2 GetTopPosition(Vector2 projectileDirection)
  {
    return Variant.op_Explicit(Geometry2D.LineIntersectsLine(this.GlobalPosition, projectileDirection, new Vector2(0.0f, 80f), Vector2.Right));
  }

  private void Initialize()
  {
    Vector2 projectileDirection = this.GetProjectileDirection();
    Vector2 topPosition = this.GetTopPosition(projectileDirection);
    this._anticipationContainer.GlobalPosition = topPosition;
    this._projectileStartPoint.GlobalPosition = Vector2.op_Addition(topPosition, Vector2.op_Multiply(projectileDirection, this._projectileOffset));
    this._projectileEndPoint.GlobalPosition = Vector2.op_Addition(this.GlobalPosition, Vector2.op_Multiply(projectileDirection, this._projectileOffset));
    ((CanvasItem) this._projectileContainer).Visible = false;
  }

  private void ModulateParticles(Color tint)
  {
    for (int index = 0; index < this._modulateParticles.Count; ++index)
      ((CanvasItem) this._modulateParticles[index]).SelfModulate = tint;
  }

  public override void _Ready() => TaskHelper.RunSafely(this.PlaySequence());

  public override void _ExitTree() => this._cts?.Cancel();

  private async Task PlaySequence()
  {
    this._cts = new CancellationTokenSource();
    for (int index = 0; index < this._anticipationParticles.Count; ++index)
      this._anticipationParticles[index].Restart();
    await Cmd.Wait(this._anticipationDuration, this._cts.Token);
    for (int index = 0; index < this._projectileStartParticles.Count; ++index)
      this._projectileStartParticles[index].Restart();
    this._projectileContainer.GlobalPosition = this._projectileStartPoint.GlobalPosition;
    ((CanvasItem) this._projectileContainer).Visible = true;
    for (int index = 0; index < this._projectileParticles.Count; ++index)
      this._projectileParticles[index].Restart();
    double timer = 0.0;
    while (timer < (double) this.WaitTime && !this._cts.IsCancellationRequested)
    {
      float num1 = (float) timer / this.WaitTime;
      Node2D projectileContainer = this._projectileContainer;
      Vector2 globalPosition = this._projectileStartPoint.GlobalPosition;
      Vector2 vector2 = ((Vector2) ref globalPosition).Lerp(this._projectileEndPoint.GlobalPosition, num1);
      projectileContainer.GlobalPosition = vector2;
      timer += ((Node) this).GetProcessDeltaTime();
      double num2 = (double) await ((Node) this).AwaitProcessFrame();
    }
    if (this._cts.IsCancellationRequested)
      return;
    ((CanvasItem) this._projectileContainer).Visible = false;
    for (int index = 0; index < this._impactParticles.Count; ++index)
      this._impactParticles[index].Restart();
    NGame.Instance?.ScreenShake(ShakeStrength.Strong, ShakeDuration.Normal);
    await Cmd.Wait(2f, this._cts.Token);
    ((Node) this).QueueFreeSafely();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(7)
    {
      new MethodInfo(NLargeMagicMissileVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node2D"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("targetFloorPosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 20L, StringName.op_Implicit("tint"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NLargeMagicMissileVfx.MethodName.GetProjectileDirection, new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLargeMagicMissileVfx.MethodName.GetTopPosition, new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("projectileDirection"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NLargeMagicMissileVfx.MethodName.Initialize, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLargeMagicMissileVfx.MethodName.ModulateParticles, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 20L, StringName.op_Implicit("tint"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NLargeMagicMissileVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLargeMagicMissileVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NLargeMagicMissileVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      NLargeMagicMissileVfx nlargeMagicMissileVfx = NLargeMagicMissileVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<Color>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<NLargeMagicMissileVfx>(ref nlargeMagicMissileVfx);
      return true;
    }
    if (StringName.op_Equality(ref method, NLargeMagicMissileVfx.MethodName.GetProjectileDirection) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      Vector2 projectileDirection = this.GetProjectileDirection();
      ret = VariantUtils.CreateFrom<Vector2>(ref projectileDirection);
      return true;
    }
    if (StringName.op_Equality(ref method, NLargeMagicMissileVfx.MethodName.GetTopPosition) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      Vector2 topPosition = this.GetTopPosition(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<Vector2>(ref topPosition);
      return true;
    }
    if (StringName.op_Equality(ref method, NLargeMagicMissileVfx.MethodName.Initialize) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Initialize();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NLargeMagicMissileVfx.MethodName.ModulateParticles) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ModulateParticles(VariantUtils.ConvertTo<Color>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NLargeMagicMissileVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NLargeMagicMissileVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
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
    if (StringName.op_Equality(ref method, NLargeMagicMissileVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      NLargeMagicMissileVfx nlargeMagicMissileVfx = NLargeMagicMissileVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<Color>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<NLargeMagicMissileVfx>(ref nlargeMagicMissileVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NLargeMagicMissileVfx.MethodName.Create) || StringName.op_Equality(ref method, NLargeMagicMissileVfx.MethodName.GetProjectileDirection) || StringName.op_Equality(ref method, NLargeMagicMissileVfx.MethodName.GetTopPosition) || StringName.op_Equality(ref method, NLargeMagicMissileVfx.MethodName.Initialize) || StringName.op_Equality(ref method, NLargeMagicMissileVfx.MethodName.ModulateParticles) || StringName.op_Equality(ref method, NLargeMagicMissileVfx.MethodName._Ready) || StringName.op_Equality(ref method, NLargeMagicMissileVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NLargeMagicMissileVfx.PropertyName.WaitTime))
    {
      this.WaitTime = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NLargeMagicMissileVfx.PropertyName._anticipationParticles))
    {
      this._anticipationParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NLargeMagicMissileVfx.PropertyName._projectileStartParticles))
    {
      this._projectileStartParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NLargeMagicMissileVfx.PropertyName._projectileParticles))
    {
      this._projectileParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NLargeMagicMissileVfx.PropertyName._impactParticles))
    {
      this._impactParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NLargeMagicMissileVfx.PropertyName._modulateParticles))
    {
      this._modulateParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NLargeMagicMissileVfx.PropertyName._anticipationContainer))
    {
      this._anticipationContainer = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NLargeMagicMissileVfx.PropertyName._anticipationDuration))
    {
      this._anticipationDuration = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NLargeMagicMissileVfx.PropertyName._projectileContainer))
    {
      this._projectileContainer = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NLargeMagicMissileVfx.PropertyName._projectileStartPoint))
    {
      this._projectileStartPoint = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NLargeMagicMissileVfx.PropertyName._projectileEndPoint))
    {
      this._projectileEndPoint = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NLargeMagicMissileVfx.PropertyName._projectileOffset))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._projectileOffset = VariantUtils.ConvertTo<float>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NLargeMagicMissileVfx.PropertyName.WaitTime))
    {
      ref godot_variant local = ref value;
      float waitTime = this.WaitTime;
      godot_variant from = VariantUtils.CreateFrom<float>(ref waitTime);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NLargeMagicMissileVfx.PropertyName._anticipationParticles))
    {
      value = VariantUtils.CreateFromArray<GpuParticles2D>(this._anticipationParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NLargeMagicMissileVfx.PropertyName._projectileStartParticles))
    {
      value = VariantUtils.CreateFromArray<GpuParticles2D>(this._projectileStartParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NLargeMagicMissileVfx.PropertyName._projectileParticles))
    {
      value = VariantUtils.CreateFromArray<GpuParticles2D>(this._projectileParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NLargeMagicMissileVfx.PropertyName._impactParticles))
    {
      value = VariantUtils.CreateFromArray<GpuParticles2D>(this._impactParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NLargeMagicMissileVfx.PropertyName._modulateParticles))
    {
      value = VariantUtils.CreateFromArray<GpuParticles2D>(this._modulateParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NLargeMagicMissileVfx.PropertyName._anticipationContainer))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._anticipationContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NLargeMagicMissileVfx.PropertyName._anticipationDuration))
    {
      value = VariantUtils.CreateFrom<float>(ref this._anticipationDuration);
      return true;
    }
    if (StringName.op_Equality(ref name, NLargeMagicMissileVfx.PropertyName._projectileContainer))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._projectileContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NLargeMagicMissileVfx.PropertyName._projectileStartPoint))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._projectileStartPoint);
      return true;
    }
    if (StringName.op_Equality(ref name, NLargeMagicMissileVfx.PropertyName._projectileEndPoint))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._projectileEndPoint);
      return true;
    }
    if (!StringName.op_Equality(ref name, NLargeMagicMissileVfx.PropertyName._projectileOffset))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<float>(ref this._projectileOffset);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 3L, NLargeMagicMissileVfx.PropertyName.WaitTime, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 28L, NLargeMagicMissileVfx.PropertyName._anticipationParticles, (PropertyHint) 23L, "24/34:GPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 28L, NLargeMagicMissileVfx.PropertyName._projectileStartParticles, (PropertyHint) 23L, "24/34:GPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 28L, NLargeMagicMissileVfx.PropertyName._projectileParticles, (PropertyHint) 23L, "24/34:GPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 28L, NLargeMagicMissileVfx.PropertyName._impactParticles, (PropertyHint) 23L, "24/34:GPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 28L, NLargeMagicMissileVfx.PropertyName._modulateParticles, (PropertyHint) 23L, "24/34:GPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NLargeMagicMissileVfx.PropertyName._anticipationContainer, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NLargeMagicMissileVfx.PropertyName._anticipationDuration, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NLargeMagicMissileVfx.PropertyName._projectileContainer, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NLargeMagicMissileVfx.PropertyName._projectileStartPoint, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NLargeMagicMissileVfx.PropertyName._projectileEndPoint, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NLargeMagicMissileVfx.PropertyName._projectileOffset, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName waitTime1 = NLargeMagicMissileVfx.PropertyName.WaitTime;
    float waitTime2 = this.WaitTime;
    Variant variant = Variant.From<float>(ref waitTime2);
    serializationInfo.AddProperty(waitTime1, variant);
    info.AddProperty(NLargeMagicMissileVfx.PropertyName._anticipationParticles, Variant.CreateFrom<GpuParticles2D>(this._anticipationParticles));
    info.AddProperty(NLargeMagicMissileVfx.PropertyName._projectileStartParticles, Variant.CreateFrom<GpuParticles2D>(this._projectileStartParticles));
    info.AddProperty(NLargeMagicMissileVfx.PropertyName._projectileParticles, Variant.CreateFrom<GpuParticles2D>(this._projectileParticles));
    info.AddProperty(NLargeMagicMissileVfx.PropertyName._impactParticles, Variant.CreateFrom<GpuParticles2D>(this._impactParticles));
    info.AddProperty(NLargeMagicMissileVfx.PropertyName._modulateParticles, Variant.CreateFrom<GpuParticles2D>(this._modulateParticles));
    info.AddProperty(NLargeMagicMissileVfx.PropertyName._anticipationContainer, Variant.From<Node2D>(ref this._anticipationContainer));
    info.AddProperty(NLargeMagicMissileVfx.PropertyName._anticipationDuration, Variant.From<float>(ref this._anticipationDuration));
    info.AddProperty(NLargeMagicMissileVfx.PropertyName._projectileContainer, Variant.From<Node2D>(ref this._projectileContainer));
    info.AddProperty(NLargeMagicMissileVfx.PropertyName._projectileStartPoint, Variant.From<Node2D>(ref this._projectileStartPoint));
    info.AddProperty(NLargeMagicMissileVfx.PropertyName._projectileEndPoint, Variant.From<Node2D>(ref this._projectileEndPoint));
    info.AddProperty(NLargeMagicMissileVfx.PropertyName._projectileOffset, Variant.From<float>(ref this._projectileOffset));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NLargeMagicMissileVfx.PropertyName.WaitTime, ref variant1))
      this.WaitTime = ((Variant) ref variant1).As<float>();
    Variant variant2;
    if (info.TryGetProperty(NLargeMagicMissileVfx.PropertyName._anticipationParticles, ref variant2))
      this._anticipationParticles = ((Variant) ref variant2).AsGodotArray<GpuParticles2D>();
    Variant variant3;
    if (info.TryGetProperty(NLargeMagicMissileVfx.PropertyName._projectileStartParticles, ref variant3))
      this._projectileStartParticles = ((Variant) ref variant3).AsGodotArray<GpuParticles2D>();
    Variant variant4;
    if (info.TryGetProperty(NLargeMagicMissileVfx.PropertyName._projectileParticles, ref variant4))
      this._projectileParticles = ((Variant) ref variant4).AsGodotArray<GpuParticles2D>();
    Variant variant5;
    if (info.TryGetProperty(NLargeMagicMissileVfx.PropertyName._impactParticles, ref variant5))
      this._impactParticles = ((Variant) ref variant5).AsGodotArray<GpuParticles2D>();
    Variant variant6;
    if (info.TryGetProperty(NLargeMagicMissileVfx.PropertyName._modulateParticles, ref variant6))
      this._modulateParticles = ((Variant) ref variant6).AsGodotArray<GpuParticles2D>();
    Variant variant7;
    if (info.TryGetProperty(NLargeMagicMissileVfx.PropertyName._anticipationContainer, ref variant7))
      this._anticipationContainer = ((Variant) ref variant7).As<Node2D>();
    Variant variant8;
    if (info.TryGetProperty(NLargeMagicMissileVfx.PropertyName._anticipationDuration, ref variant8))
      this._anticipationDuration = ((Variant) ref variant8).As<float>();
    Variant variant9;
    if (info.TryGetProperty(NLargeMagicMissileVfx.PropertyName._projectileContainer, ref variant9))
      this._projectileContainer = ((Variant) ref variant9).As<Node2D>();
    Variant variant10;
    if (info.TryGetProperty(NLargeMagicMissileVfx.PropertyName._projectileStartPoint, ref variant10))
      this._projectileStartPoint = ((Variant) ref variant10).As<Node2D>();
    Variant variant11;
    if (info.TryGetProperty(NLargeMagicMissileVfx.PropertyName._projectileEndPoint, ref variant11))
      this._projectileEndPoint = ((Variant) ref variant11).As<Node2D>();
    Variant variant12;
    if (!info.TryGetProperty(NLargeMagicMissileVfx.PropertyName._projectileOffset, ref variant12))
      return;
    this._projectileOffset = ((Variant) ref variant12).As<float>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName GetProjectileDirection = StringName.op_Implicit(nameof (GetProjectileDirection));
    public static readonly StringName GetTopPosition = StringName.op_Implicit(nameof (GetTopPosition));
    public static readonly StringName Initialize = StringName.op_Implicit(nameof (Initialize));
    public static readonly StringName ModulateParticles = StringName.op_Implicit(nameof (ModulateParticles));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName WaitTime = StringName.op_Implicit(nameof (WaitTime));
    public static readonly StringName _anticipationParticles = StringName.op_Implicit(nameof (_anticipationParticles));
    public static readonly StringName _projectileStartParticles = StringName.op_Implicit(nameof (_projectileStartParticles));
    public static readonly StringName _projectileParticles = StringName.op_Implicit(nameof (_projectileParticles));
    public static readonly StringName _impactParticles = StringName.op_Implicit(nameof (_impactParticles));
    public static readonly StringName _modulateParticles = StringName.op_Implicit(nameof (_modulateParticles));
    public static readonly StringName _anticipationContainer = StringName.op_Implicit(nameof (_anticipationContainer));
    public static readonly StringName _anticipationDuration = StringName.op_Implicit(nameof (_anticipationDuration));
    public static readonly StringName _projectileContainer = StringName.op_Implicit(nameof (_projectileContainer));
    public static readonly StringName _projectileStartPoint = StringName.op_Implicit(nameof (_projectileStartPoint));
    public static readonly StringName _projectileEndPoint = StringName.op_Implicit(nameof (_projectileEndPoint));
    public static readonly StringName _projectileOffset = StringName.op_Implicit(nameof (_projectileOffset));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
