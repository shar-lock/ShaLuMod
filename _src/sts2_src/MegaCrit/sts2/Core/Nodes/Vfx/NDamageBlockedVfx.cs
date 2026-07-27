// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NDamageBlockedVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NDamageBlockedVfx.cs")]
public class NDamageBlockedVfx : Node2D
{
  private static readonly LocString _blockedLoc = new LocString("vfx", "BLOCKED");
  private MegaLabel _label;
  private Tween? _tween;

  private static string ScenePath => SceneHelper.GetScenePath("vfx/vfx_blocked_text");

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NDamageBlockedVfx.ScenePath);
    }
  }

  public static NDamageBlockedVfx? Create(Creature target)
  {
    if (TestMode.IsOn)
      return (NDamageBlockedVfx) null;
    NCreature creatureNode = NCombatRoom.Instance.GetCreatureNode(target);
    if (!creatureNode.IsInteractable)
      return (NDamageBlockedVfx) null;
    NDamageBlockedVfx ndamageBlockedVfx = PreloadManager.Cache.GetScene(NDamageBlockedVfx.ScenePath).Instantiate<NDamageBlockedVfx>((PackedScene.GenEditState) 0L);
    ndamageBlockedVfx.GlobalPosition = Vector2.op_Addition(creatureNode.VfxSpawnPosition, new Vector2(Rng.Chaotic.NextFloat(-20f, 20f), Rng.Chaotic.NextFloat(-60f, -40f)));
    ndamageBlockedVfx.RotationDegrees = Rng.Chaotic.NextFloat(-2f, 2f);
    return ndamageBlockedVfx;
  }

  public override void _Ready()
  {
    this._label = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("Label"));
    this._label.SetTextAutoSize(NDamageBlockedVfx._blockedLoc.GetRawText());
    TaskHelper.RunSafely(this.BlockAnim());
  }

  public override void _ExitTree() => this._tween?.Kill();

  private async Task BlockAnim()
  {
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 0.6f)), 2.0).From(Variant.op_Implicit(Vector2.One)).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 4L);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position:y"), Variant.op_Implicit(this.Position.Y - 250f), 2.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 4L);
    this._tween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 2.0).From(Variant.op_Implicit(new Color("21C0FF"))).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 1.5).SetEase((Tween.EaseType) 0L).SetTrans((Tween.TransitionType) 1L).SetDelay(0.5);
    bool flag = await this._tween.AwaitFinished((Node) this);
    ((Node) this).QueueFreeSafely();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NDamageBlockedVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDamageBlockedVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NDamageBlockedVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NDamageBlockedVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NDamageBlockedVfx.MethodName._Ready) || StringName.op_Equality(ref method, NDamageBlockedVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDamageBlockedVfx.PropertyName._label))
    {
      this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDamageBlockedVfx.PropertyName._tween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._tween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDamageBlockedVfx.PropertyName._label))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDamageBlockedVfx.PropertyName._tween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._tween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NDamageBlockedVfx.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDamageBlockedVfx.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NDamageBlockedVfx.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
    info.AddProperty(NDamageBlockedVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NDamageBlockedVfx.PropertyName._label, ref variant1))
      this._label = ((Variant) ref variant1).As<MegaLabel>();
    Variant variant2;
    if (!info.TryGetProperty(NDamageBlockedVfx.PropertyName._tween, ref variant2))
      return;
    this._tween = ((Variant) ref variant2).As<Tween>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _label = StringName.op_Implicit(nameof (_label));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
