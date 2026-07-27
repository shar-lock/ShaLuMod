// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.Cards.NHellraiserVfx
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
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx.Cards;

[ScriptPath("res://src/Core/Nodes/Vfx/Cards/NHellraiserVfx.cs")]
public class NHellraiserVfx : Control
{
  private float _duration = 1f;
  private int _swordAmount = 10;
  private Vector2 _spawnPosition;
  private static readonly Vector2 _vfxOffset = new Vector2(-100f, -200f);
  private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/cards/vfx_hellraiser/hellraiser_vfx");
  private const string _hellraiserSfxPath = "event:/sfx/characters/ironclad/ironclad_hellraiser";
  private CancellationTokenSource? _cts;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NHellraiserVfx._scenePath);
    }
  }

  public override void _ExitTree() => this._cts?.Cancel();

  public static NHellraiserVfx? Create(Creature target)
  {
    if (TestMode.IsOn)
      return (NHellraiserVfx) null;
    NHellraiserVfx nhellraiserVfx = PreloadManager.Cache.GetScene(NHellraiserVfx._scenePath).Instantiate<NHellraiserVfx>((PackedScene.GenEditState) 0L);
    nhellraiserVfx._spawnPosition = Vector2.op_Addition(NCombatRoom.Instance.GetCreatureNode(target).GetBottomOfHitbox(), NHellraiserVfx._vfxOffset);
    return nhellraiserVfx;
  }

  public override void _Ready()
  {
    List<float> floatList1 = new List<float>();
    for (int index = 0; index < this._swordAmount; ++index)
      floatList1.Add(Rng.Chaotic.NextFloat(10f, 50f));
    floatList1.Sort();
    foreach (float num1 in floatList1)
    {
      NHellraiserSwordVfx child = NHellraiserSwordVfx.Create();
      child.GlobalPosition = this._spawnPosition;
      child.posY = num1;
      float num2 = MathHelper.Remap(num1, 10f, 50f, 0.8f, 1f);
      child.targetColor = new Color(num2, num2, num2, 1f);
      ((Node) NCombatRoom.Instance.CombatVfxContainer).AddChildSafely((Node) child);
    }
    List<float> floatList2 = new List<float>();
    for (int index = 0; index < this._swordAmount; ++index)
      floatList2.Add(Rng.Chaotic.NextFloat(-50f, -10f));
    SfxCmd.Play("event:/sfx/characters/ironclad/ironclad_hellraiser");
    floatList2.Sort();
    foreach (float num3 in floatList2)
    {
      NHellraiserSwordVfx child = NHellraiserSwordVfx.Create();
      child.GlobalPosition = this._spawnPosition;
      child.posY = num3;
      float num4 = MathHelper.Remap(num3, -10f, -50f, 0.7f, 0.4f);
      child.targetColor = new Color(num4, num4, num4, 1f);
      ((Node) NCombatRoom.Instance.BackCombatVfxContainer).AddChildSafely((Node) child);
    }
    ((Node) NCombatRoom.Instance.CombatVfxContainer).AddChildSafely((Node) NAdditiveOverlayVfx.Create());
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
      new MethodInfo(NHellraiserVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHellraiserVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NHellraiserVfx.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NHellraiserVfx.MethodName._Ready) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._Ready();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NHellraiserVfx.MethodName._ExitTree) || StringName.op_Equality(ref method, NHellraiserVfx.MethodName._Ready) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NHellraiserVfx.PropertyName._duration))
    {
      this._duration = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHellraiserVfx.PropertyName._swordAmount))
    {
      this._swordAmount = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NHellraiserVfx.PropertyName._spawnPosition))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._spawnPosition = VariantUtils.ConvertTo<Vector2>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NHellraiserVfx.PropertyName._duration))
    {
      value = VariantUtils.CreateFrom<float>(ref this._duration);
      return true;
    }
    if (StringName.op_Equality(ref name, NHellraiserVfx.PropertyName._swordAmount))
    {
      value = VariantUtils.CreateFrom<int>(ref this._swordAmount);
      return true;
    }
    if (!StringName.op_Equality(ref name, NHellraiserVfx.PropertyName._spawnPosition))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Vector2>(ref this._spawnPosition);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 3L, NHellraiserVfx.PropertyName._duration, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NHellraiserVfx.PropertyName._swordAmount, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NHellraiserVfx.PropertyName._spawnPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NHellraiserVfx.PropertyName._duration, Variant.From<float>(ref this._duration));
    info.AddProperty(NHellraiserVfx.PropertyName._swordAmount, Variant.From<int>(ref this._swordAmount));
    info.AddProperty(NHellraiserVfx.PropertyName._spawnPosition, Variant.From<Vector2>(ref this._spawnPosition));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NHellraiserVfx.PropertyName._duration, ref variant1))
      this._duration = ((Variant) ref variant1).As<float>();
    Variant variant2;
    if (info.TryGetProperty(NHellraiserVfx.PropertyName._swordAmount, ref variant2))
      this._swordAmount = ((Variant) ref variant2).As<int>();
    Variant variant3;
    if (!info.TryGetProperty(NHellraiserVfx.PropertyName._spawnPosition, ref variant3))
      return;
    this._spawnPosition = ((Variant) ref variant3).As<Vector2>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _duration = StringName.op_Implicit(nameof (_duration));
    public static readonly StringName _swordAmount = StringName.op_Implicit(nameof (_swordAmount));
    public static readonly StringName _spawnPosition = StringName.op_Implicit(nameof (_spawnPosition));
  }

  public class SignalName : Control.SignalName
  {
  }
}
