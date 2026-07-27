// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NThinSliceVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NThinSliceVfx.cs")]
public class NThinSliceVfx : Node2D
{
  private const string _scenePath = "res://scenes/vfx/thin_slice_vfx.tscn";
  private CancellationTokenSource? _cts;
  private GpuParticles2D _slash;
  private GpuParticles2D _sparkle;
  private Vector2 _creatureCenter;
  private VfxColor _vfxColor;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>("res://scenes/vfx/thin_slice_vfx.tscn");
    }
  }

  public static NThinSliceVfx? Create(Creature? target, VfxColor vfxColor = VfxColor.Cyan)
  {
    if (TestMode.IsOn)
      return (NThinSliceVfx) null;
    NCreature creatureNode = NCombatRoom.Instance?.GetCreatureNode(target);
    if (creatureNode == null)
      return (NThinSliceVfx) null;
    Vector2 vfxSpawnPosition = creatureNode.VfxSpawnPosition;
    NThinSliceVfx nthinSliceVfx = PreloadManager.Cache.GetScene("res://scenes/vfx/thin_slice_vfx.tscn").Instantiate<NThinSliceVfx>((PackedScene.GenEditState) 0L);
    nthinSliceVfx._vfxColor = vfxColor;
    Vector2 vector2;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2).\u002Ector(Rng.Chaotic.NextFloat(-50f, 50f), Rng.Chaotic.NextFloat(-50f, 50f));
    nthinSliceVfx._creatureCenter = Vector2.op_Addition(vfxSpawnPosition, vector2);
    return nthinSliceVfx;
  }

  public override void _Ready()
  {
    this._slash = ((Node) this).GetNode<GpuParticles2D>(NodePath.op_Implicit("Slash"));
    ((Node2D) this._slash).GlobalPosition = this.GenerateSpawnPosition();
    ((Node2D) this._slash).Rotation = this.GetAngle();
    this._slash.Emitting = true;
    this._sparkle = ((Node) this._slash).GetNode<GpuParticles2D>(NodePath.op_Implicit("Sparkle"));
    ((Node2D) this._sparkle).GlobalPosition = this._creatureCenter;
    this._sparkle.Emitting = true;
    this.SetColor();
    TaskHelper.RunSafely(this.SelfDestruct());
  }

  public override void _ExitTree() => this._cts?.Cancel();

  private void SetColor()
  {
    ParticleProcessMaterial processMaterial = (ParticleProcessMaterial) this._slash.GetProcessMaterial();
    switch (this._vfxColor)
    {
      case VfxColor.Red:
        processMaterial.Color = new Color("FF9900");
        break;
      case VfxColor.Green:
        break;
      case VfxColor.Blue:
        break;
      case VfxColor.Purple:
        break;
      case VfxColor.Black:
        break;
      case VfxColor.White:
        processMaterial.Color = Colors.White;
        break;
      case VfxColor.Cyan:
        processMaterial.Color = new Color("C4FFE6");
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  private Vector2 GenerateSpawnPosition()
  {
    float num1 = Rng.Chaotic.NextFloat(0.0f, 6.28318548f);
    float num2 = Rng.Chaotic.NextFloat(400f, 500f);
    return new Vector2(this._creatureCenter.X + num2 * Mathf.Cos(num1), this._creatureCenter.Y + num2 * Mathf.Sin(num1));
  }

  private float GetAngle()
  {
    Vector2 vector2 = Vector2.op_Subtraction(this._creatureCenter, ((Node2D) this._slash).GlobalPosition);
    return Mathf.Atan2(vector2.Y, vector2.X);
  }

  private async Task SelfDestruct()
  {
    this._cts?.Cancel();
    this._cts = new CancellationTokenSource();
    await Task.Delay(1000, this._cts.Token);
    if (this._cts.IsCancellationRequested)
      return;
    ((Node) this).QueueFreeSafely();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NThinSliceVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NThinSliceVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NThinSliceVfx.MethodName.SetColor, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NThinSliceVfx.MethodName.GenerateSpawnPosition, new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NThinSliceVfx.MethodName.GetAngle, new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NThinSliceVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NThinSliceVfx.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NThinSliceVfx.MethodName.SetColor) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SetColor();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NThinSliceVfx.MethodName.GenerateSpawnPosition) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      Vector2 spawnPosition = this.GenerateSpawnPosition();
      ret = VariantUtils.CreateFrom<Vector2>(ref spawnPosition);
      return true;
    }
    if (!StringName.op_Equality(ref method, NThinSliceVfx.MethodName.GetAngle) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    float angle = this.GetAngle();
    ret = VariantUtils.CreateFrom<float>(ref angle);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NThinSliceVfx.MethodName._Ready) || StringName.op_Equality(ref method, NThinSliceVfx.MethodName._ExitTree) || StringName.op_Equality(ref method, NThinSliceVfx.MethodName.SetColor) || StringName.op_Equality(ref method, NThinSliceVfx.MethodName.GenerateSpawnPosition) || StringName.op_Equality(ref method, NThinSliceVfx.MethodName.GetAngle) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NThinSliceVfx.PropertyName._slash))
    {
      this._slash = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NThinSliceVfx.PropertyName._sparkle))
    {
      this._sparkle = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NThinSliceVfx.PropertyName._creatureCenter))
    {
      this._creatureCenter = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NThinSliceVfx.PropertyName._vfxColor))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._vfxColor = VariantUtils.ConvertTo<VfxColor>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NThinSliceVfx.PropertyName._slash))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._slash);
      return true;
    }
    if (StringName.op_Equality(ref name, NThinSliceVfx.PropertyName._sparkle))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._sparkle);
      return true;
    }
    if (StringName.op_Equality(ref name, NThinSliceVfx.PropertyName._creatureCenter))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._creatureCenter);
      return true;
    }
    if (!StringName.op_Equality(ref name, NThinSliceVfx.PropertyName._vfxColor))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<VfxColor>(ref this._vfxColor);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NThinSliceVfx.PropertyName._slash, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NThinSliceVfx.PropertyName._sparkle, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NThinSliceVfx.PropertyName._creatureCenter, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NThinSliceVfx.PropertyName._vfxColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NThinSliceVfx.PropertyName._slash, Variant.From<GpuParticles2D>(ref this._slash));
    info.AddProperty(NThinSliceVfx.PropertyName._sparkle, Variant.From<GpuParticles2D>(ref this._sparkle));
    info.AddProperty(NThinSliceVfx.PropertyName._creatureCenter, Variant.From<Vector2>(ref this._creatureCenter));
    info.AddProperty(NThinSliceVfx.PropertyName._vfxColor, Variant.From<VfxColor>(ref this._vfxColor));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NThinSliceVfx.PropertyName._slash, ref variant1))
      this._slash = ((Variant) ref variant1).As<GpuParticles2D>();
    Variant variant2;
    if (info.TryGetProperty(NThinSliceVfx.PropertyName._sparkle, ref variant2))
      this._sparkle = ((Variant) ref variant2).As<GpuParticles2D>();
    Variant variant3;
    if (info.TryGetProperty(NThinSliceVfx.PropertyName._creatureCenter, ref variant3))
      this._creatureCenter = ((Variant) ref variant3).As<Vector2>();
    Variant variant4;
    if (!info.TryGetProperty(NThinSliceVfx.PropertyName._vfxColor, ref variant4))
      return;
    this._vfxColor = ((Variant) ref variant4).As<VfxColor>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName SetColor = StringName.op_Implicit(nameof (SetColor));
    public static readonly StringName GenerateSpawnPosition = StringName.op_Implicit(nameof (GenerateSpawnPosition));
    public static readonly StringName GetAngle = StringName.op_Implicit(nameof (GetAngle));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _slash = StringName.op_Implicit(nameof (_slash));
    public static readonly StringName _sparkle = StringName.op_Implicit(nameof (_sparkle));
    public static readonly StringName _creatureCenter = StringName.op_Implicit(nameof (_creatureCenter));
    public static readonly StringName _vfxColor = StringName.op_Implicit(nameof (_vfxColor));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
