// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NStunnedVfx
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
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NStunnedVfx.cs")]
public class NStunnedVfx : Node2D
{
  private const string _scenePath = "res://scenes/vfx/stunned_vfx.tscn";
  private static LocString _stunnedLoc = new LocString("vfx", "STUNNED");
  private MegaLabel _label;
  private Creature _creature;
  private Tween? _textTween;
  private Tween? _positionTween;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>("res://scenes/vfx/stunned_vfx.tscn");
    }
  }

  public override void _Ready()
  {
    NCreature creatureNode = NCombatRoom.Instance?.GetCreatureNode(this._creature);
    if (creatureNode == null)
    {
      ((Node) this).QueueFreeSafely();
    }
    else
    {
      this._label = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%Label"));
      this.GlobalPosition = creatureNode.GetTopOfHitbox();
      TaskHelper.RunSafely(this.StartVfx());
    }
  }

  public override void _ExitTree()
  {
    this._textTween?.Kill();
    this._positionTween?.Kill();
  }

  private async Task StartVfx()
  {
    this._label.SetTextAutoSize(NStunnedVfx._stunnedLoc.GetFormattedText());
    this._textTween = ((Node) this).CreateTween();
    this._textTween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._textTween.TweenInterval(0.5);
    this._textTween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 0L);
    this._positionTween = ((Node) this).CreateTween();
    this._positionTween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("position:y"), Variant.op_Implicit(((Control) this._label).Position.Y - 100f), 2.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 3L);
    bool flag = await this._textTween.AwaitFinished((Node) this);
    ((Node) this).QueueFreeSafely();
  }

  public static NStunnedVfx? Create(Creature creature)
  {
    if (TestMode.IsOn)
      return (NStunnedVfx) null;
    if (NCombatUi.IsDebugHideTextVfx)
      return (NStunnedVfx) null;
    NStunnedVfx nstunnedVfx = PreloadManager.Cache.GetScene("res://scenes/vfx/stunned_vfx.tscn").Instantiate<NStunnedVfx>((PackedScene.GenEditState) 0L);
    nstunnedVfx._creature = creature;
    return nstunnedVfx;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NStunnedVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NStunnedVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NStunnedVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NStunnedVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NStunnedVfx.MethodName._Ready) || StringName.op_Equality(ref method, NStunnedVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NStunnedVfx.PropertyName._label))
    {
      this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NStunnedVfx.PropertyName._textTween))
    {
      this._textTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NStunnedVfx.PropertyName._positionTween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._positionTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NStunnedVfx.PropertyName._label))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
      return true;
    }
    if (StringName.op_Equality(ref name, NStunnedVfx.PropertyName._textTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._textTween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NStunnedVfx.PropertyName._positionTween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._positionTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NStunnedVfx.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NStunnedVfx.PropertyName._textTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NStunnedVfx.PropertyName._positionTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NStunnedVfx.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
    info.AddProperty(NStunnedVfx.PropertyName._textTween, Variant.From<Tween>(ref this._textTween));
    info.AddProperty(NStunnedVfx.PropertyName._positionTween, Variant.From<Tween>(ref this._positionTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NStunnedVfx.PropertyName._label, ref variant1))
      this._label = ((Variant) ref variant1).As<MegaLabel>();
    Variant variant2;
    if (info.TryGetProperty(NStunnedVfx.PropertyName._textTween, ref variant2))
      this._textTween = ((Variant) ref variant2).As<Tween>();
    Variant variant3;
    if (!info.TryGetProperty(NStunnedVfx.PropertyName._positionTween, ref variant3))
      return;
    this._positionTween = ((Variant) ref variant3).As<Tween>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _label = StringName.op_Implicit(nameof (_label));
    public static readonly StringName _textTween = StringName.op_Implicit(nameof (_textTween));
    public static readonly StringName _positionTween = StringName.op_Implicit(nameof (_positionTween));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
