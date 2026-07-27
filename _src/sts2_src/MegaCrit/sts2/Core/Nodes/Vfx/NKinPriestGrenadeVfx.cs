// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NKinPriestGrenadeVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Audio.Debug;
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

[ScriptPath("res://src/Core/Nodes/Vfx/NKinPriestGrenadeVfx.cs")]
public class NKinPriestGrenadeVfx : Node2D
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/monsters/kin_priest_grenade_vfx");
  private GpuParticles2D _cryptoParticles;
  private GpuParticles2D _noiseParticles;
  private GpuParticles2D _explosionBase;
  private readonly CancellationTokenSource _cancelToken = new CancellationTokenSource();

  public static NKinPriestGrenadeVfx? Create(Creature target)
  {
    if (TestMode.IsOn)
      return (NKinPriestGrenadeVfx) null;
    NCreature creatureNode = NCombatRoom.Instance.GetCreatureNode(target);
    if (creatureNode == null)
      return (NKinPriestGrenadeVfx) null;
    NKinPriestGrenadeVfx priestGrenadeVfx = PreloadManager.Cache.GetScene(NKinPriestGrenadeVfx._scenePath).Instantiate<NKinPriestGrenadeVfx>((PackedScene.GenEditState) 0L);
    priestGrenadeVfx.GlobalPosition = creatureNode.GetBottomOfHitbox();
    return priestGrenadeVfx;
  }

  public override void _Ready()
  {
    this._cryptoParticles = ((Node) this).GetNode<GpuParticles2D>(NodePath.op_Implicit("CryptoParticles"));
    this._noiseParticles = ((Node) this).GetNode<GpuParticles2D>(NodePath.op_Implicit("NoiseParticles"));
    this._explosionBase = ((Node) this).GetNode<GpuParticles2D>(NodePath.op_Implicit("ExplosionBaseParticle"));
    this._cryptoParticles.Emitting = false;
    this._cryptoParticles.OneShot = true;
    this._noiseParticles.Emitting = false;
    this._noiseParticles.OneShot = true;
    this._explosionBase.Emitting = false;
    this._explosionBase.OneShot = true;
    TaskHelper.RunSafely(this.Play());
  }

  private async Task Play()
  {
    NDebugAudioManager.Instance?.Play("blunt_attack.mp3");
    this._noiseParticles.SetEmitting(true);
    this._explosionBase.SetEmitting(true);
    await Task.Delay(100, this._cancelToken.Token);
    this._cryptoParticles.SetEmitting(true);
    await Task.Delay(5000, this._cancelToken.Token);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NKinPriestGrenadeVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (!StringName.op_Equality(ref method, NKinPriestGrenadeVfx.MethodName._Ready) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._Ready();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NKinPriestGrenadeVfx.MethodName._Ready) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NKinPriestGrenadeVfx.PropertyName._cryptoParticles))
    {
      this._cryptoParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NKinPriestGrenadeVfx.PropertyName._noiseParticles))
    {
      this._noiseParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NKinPriestGrenadeVfx.PropertyName._explosionBase))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._explosionBase = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NKinPriestGrenadeVfx.PropertyName._cryptoParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._cryptoParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NKinPriestGrenadeVfx.PropertyName._noiseParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._noiseParticles);
      return true;
    }
    if (!StringName.op_Equality(ref name, NKinPriestGrenadeVfx.PropertyName._explosionBase))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._explosionBase);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NKinPriestGrenadeVfx.PropertyName._cryptoParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NKinPriestGrenadeVfx.PropertyName._noiseParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NKinPriestGrenadeVfx.PropertyName._explosionBase, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NKinPriestGrenadeVfx.PropertyName._cryptoParticles, Variant.From<GpuParticles2D>(ref this._cryptoParticles));
    info.AddProperty(NKinPriestGrenadeVfx.PropertyName._noiseParticles, Variant.From<GpuParticles2D>(ref this._noiseParticles));
    info.AddProperty(NKinPriestGrenadeVfx.PropertyName._explosionBase, Variant.From<GpuParticles2D>(ref this._explosionBase));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NKinPriestGrenadeVfx.PropertyName._cryptoParticles, ref variant1))
      this._cryptoParticles = ((Variant) ref variant1).As<GpuParticles2D>();
    Variant variant2;
    if (info.TryGetProperty(NKinPriestGrenadeVfx.PropertyName._noiseParticles, ref variant2))
      this._noiseParticles = ((Variant) ref variant2).As<GpuParticles2D>();
    Variant variant3;
    if (!info.TryGetProperty(NKinPriestGrenadeVfx.PropertyName._explosionBase, ref variant3))
      return;
    this._explosionBase = ((Variant) ref variant3).As<GpuParticles2D>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _cryptoParticles = StringName.op_Implicit(nameof (_cryptoParticles));
    public static readonly StringName _noiseParticles = StringName.op_Implicit(nameof (_noiseParticles));
    public static readonly StringName _explosionBase = StringName.op_Implicit(nameof (_explosionBase));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
