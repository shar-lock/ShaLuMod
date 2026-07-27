// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.Cards.NSpikeSplashVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx.Cards;

[ScriptPath("res://src/Core/Nodes/Vfx/Cards/NSpikeSplashVfx.cs")]
public class NSpikeSplashVfx : Node2D
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/spike_splash_vfx");
  private float _duration = 1f;
  private int _spikeAmount = 6;
  private Vector2 _spawnPosition;
  private VfxColor _vfxColor;
  private CancellationTokenSource? _cts;

  public override void _ExitTree() => this._cts?.Cancel();

  public static NSpikeSplashVfx? Create(Creature target, VfxColor vfxColor = VfxColor.Red)
  {
    if (TestMode.IsOn)
      return (NSpikeSplashVfx) null;
    NSpikeSplashVfx nspikeSplashVfx = PreloadManager.Cache.GetScene(NSpikeSplashVfx._scenePath).Instantiate<NSpikeSplashVfx>((PackedScene.GenEditState) 0L);
    nspikeSplashVfx._spawnPosition = NCombatRoom.Instance.GetCreatureNode(target).GetBottomOfHitbox();
    nspikeSplashVfx._vfxColor = vfxColor;
    return nspikeSplashVfx;
  }

  public override void _Ready()
  {
    for (int index = 0; index < this._spikeAmount; ++index)
    {
      ((Node) NCombatRoom.Instance.CombatVfxContainer).AddChildSafely((Node) NFgGroundSpikeVfx.Create(this._spawnPosition, true, this._vfxColor));
      ((Node) NCombatRoom.Instance.CombatVfxContainer).AddChildSafely((Node) NFgGroundSpikeVfx.Create(this._spawnPosition, false, this._vfxColor));
    }
    for (int index = 0; index < this._spikeAmount; ++index)
    {
      ((Node) NCombatRoom.Instance.BackCombatVfxContainer).AddChildSafely((Node) NBgGroundSpikeVfx.Create(this._spawnPosition, vfxColor: this._vfxColor));
      ((Node) NCombatRoom.Instance.BackCombatVfxContainer).AddChildSafely((Node) NBgGroundSpikeVfx.Create(this._spawnPosition, false, this._vfxColor));
    }
    TaskHelper.RunSafely(this.SelfDestruct());
  }

  private async Task SelfDestruct()
  {
    this._cts = new CancellationTokenSource();
    await Task.Delay(2000, this._cts.Token);
    ((Node) this).QueueFreeSafely();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NSpikeSplashVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSpikeSplashVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSpikeSplashVfx.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NSpikeSplashVfx.MethodName._Ready) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._Ready();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSpikeSplashVfx.MethodName._ExitTree) || StringName.op_Equality(ref method, NSpikeSplashVfx.MethodName._Ready) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSpikeSplashVfx.PropertyName._duration))
    {
      this._duration = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpikeSplashVfx.PropertyName._spikeAmount))
    {
      this._spikeAmount = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpikeSplashVfx.PropertyName._spawnPosition))
    {
      this._spawnPosition = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSpikeSplashVfx.PropertyName._vfxColor))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._vfxColor = VariantUtils.ConvertTo<VfxColor>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSpikeSplashVfx.PropertyName._duration))
    {
      value = VariantUtils.CreateFrom<float>(ref this._duration);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpikeSplashVfx.PropertyName._spikeAmount))
    {
      value = VariantUtils.CreateFrom<int>(ref this._spikeAmount);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpikeSplashVfx.PropertyName._spawnPosition))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._spawnPosition);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSpikeSplashVfx.PropertyName._vfxColor))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<VfxColor>(ref this._vfxColor);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 3L, NSpikeSplashVfx.PropertyName._duration, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NSpikeSplashVfx.PropertyName._spikeAmount, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NSpikeSplashVfx.PropertyName._spawnPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NSpikeSplashVfx.PropertyName._vfxColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NSpikeSplashVfx.PropertyName._duration, Variant.From<float>(ref this._duration));
    info.AddProperty(NSpikeSplashVfx.PropertyName._spikeAmount, Variant.From<int>(ref this._spikeAmount));
    info.AddProperty(NSpikeSplashVfx.PropertyName._spawnPosition, Variant.From<Vector2>(ref this._spawnPosition));
    info.AddProperty(NSpikeSplashVfx.PropertyName._vfxColor, Variant.From<VfxColor>(ref this._vfxColor));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NSpikeSplashVfx.PropertyName._duration, ref variant1))
      this._duration = ((Variant) ref variant1).As<float>();
    Variant variant2;
    if (info.TryGetProperty(NSpikeSplashVfx.PropertyName._spikeAmount, ref variant2))
      this._spikeAmount = ((Variant) ref variant2).As<int>();
    Variant variant3;
    if (info.TryGetProperty(NSpikeSplashVfx.PropertyName._spawnPosition, ref variant3))
      this._spawnPosition = ((Variant) ref variant3).As<Vector2>();
    Variant variant4;
    if (!info.TryGetProperty(NSpikeSplashVfx.PropertyName._vfxColor, ref variant4))
      return;
    this._vfxColor = ((Variant) ref variant4).As<VfxColor>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _duration = StringName.op_Implicit(nameof (_duration));
    public static readonly StringName _spikeAmount = StringName.op_Implicit(nameof (_spikeAmount));
    public static readonly StringName _spawnPosition = StringName.op_Implicit(nameof (_spawnPosition));
    public static readonly StringName _vfxColor = StringName.op_Implicit(nameof (_vfxColor));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
