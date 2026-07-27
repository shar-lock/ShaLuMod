// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NPowerRemovedVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NPowerRemovedVfx.cs")]
public class NPowerRemovedVfx : Node2D
{
  private const string _scenePath = "res://scenes/vfx/power_removed_vfx.tscn";
  private static LocString _wearsOffLoc = new LocString("vfx", "POWER_WEARS_OFF");
  private TextureRect _sprite;
  private MegaLabel _powerField;
  private Control _vfxContainer;
  private PowerModel _power;
  private Tween? _textTween;
  private Tween? _positionTween;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>("res://scenes/vfx/power_removed_vfx.tscn");
    }
  }

  public override void _Ready()
  {
    NCreature creatureNode = NCombatRoom.Instance?.GetCreatureNode(this._power.Owner);
    if (creatureNode == null)
    {
      ((Node) this).QueueFreeSafely();
    }
    else
    {
      this._sprite = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%TextureRect"));
      this._powerField = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%PowerField"));
      this._vfxContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Container"));
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
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%WearsOff")).SetTextAutoSize(NPowerRemovedVfx._wearsOffLoc.GetRawText());
    this._powerField.SetTextAutoSize(this._power.Title.GetFormattedText());
    this._sprite.Texture = this._power.BigIcon;
    Control vfxContainer = this._vfxContainer;
    vfxContainer.Position = Vector2.op_Subtraction(vfxContainer.Position, Vector2.op_Multiply(this._vfxContainer.Size, 0.5f));
    this._textTween = ((Node) this).CreateTween();
    this._textTween.TweenProperty((GodotObject) this._vfxContainer, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._textTween.TweenInterval(0.5);
    this._textTween.TweenProperty((GodotObject) this._vfxContainer, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 0L);
    this._positionTween = ((Node) this).CreateTween();
    this._positionTween.TweenProperty((GodotObject) this._vfxContainer, NodePath.op_Implicit("position:y"), Variant.op_Implicit(((Control) this._powerField).Position.Y - 160f), 2.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 3L);
    bool flag = await this._textTween.AwaitFinished((Node) this);
    ((Node) this).QueueFreeSafely();
  }

  public static NPowerRemovedVfx? Create(PowerModel power)
  {
    if (TestMode.IsOn)
      return (NPowerRemovedVfx) null;
    if (NCombatUi.IsDebugHideTextVfx)
      return (NPowerRemovedVfx) null;
    if (!power.ShouldPlayVfx)
      return (NPowerRemovedVfx) null;
    NPowerRemovedVfx npowerRemovedVfx = PreloadManager.Cache.GetScene("res://scenes/vfx/power_removed_vfx.tscn").Instantiate<NPowerRemovedVfx>((PackedScene.GenEditState) 0L);
    npowerRemovedVfx._power = power;
    return npowerRemovedVfx;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NPowerRemovedVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPowerRemovedVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NPowerRemovedVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NPowerRemovedVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NPowerRemovedVfx.MethodName._Ready) || StringName.op_Equality(ref method, NPowerRemovedVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPowerRemovedVfx.PropertyName._sprite))
    {
      this._sprite = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPowerRemovedVfx.PropertyName._powerField))
    {
      this._powerField = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPowerRemovedVfx.PropertyName._vfxContainer))
    {
      this._vfxContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPowerRemovedVfx.PropertyName._textTween))
    {
      this._textTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPowerRemovedVfx.PropertyName._positionTween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._positionTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPowerRemovedVfx.PropertyName._sprite))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._sprite);
      return true;
    }
    if (StringName.op_Equality(ref name, NPowerRemovedVfx.PropertyName._powerField))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._powerField);
      return true;
    }
    if (StringName.op_Equality(ref name, NPowerRemovedVfx.PropertyName._vfxContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._vfxContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NPowerRemovedVfx.PropertyName._textTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._textTween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPowerRemovedVfx.PropertyName._positionTween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._positionTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NPowerRemovedVfx.PropertyName._sprite, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPowerRemovedVfx.PropertyName._powerField, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPowerRemovedVfx.PropertyName._vfxContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPowerRemovedVfx.PropertyName._textTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPowerRemovedVfx.PropertyName._positionTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NPowerRemovedVfx.PropertyName._sprite, Variant.From<TextureRect>(ref this._sprite));
    info.AddProperty(NPowerRemovedVfx.PropertyName._powerField, Variant.From<MegaLabel>(ref this._powerField));
    info.AddProperty(NPowerRemovedVfx.PropertyName._vfxContainer, Variant.From<Control>(ref this._vfxContainer));
    info.AddProperty(NPowerRemovedVfx.PropertyName._textTween, Variant.From<Tween>(ref this._textTween));
    info.AddProperty(NPowerRemovedVfx.PropertyName._positionTween, Variant.From<Tween>(ref this._positionTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NPowerRemovedVfx.PropertyName._sprite, ref variant1))
      this._sprite = ((Variant) ref variant1).As<TextureRect>();
    Variant variant2;
    if (info.TryGetProperty(NPowerRemovedVfx.PropertyName._powerField, ref variant2))
      this._powerField = ((Variant) ref variant2).As<MegaLabel>();
    Variant variant3;
    if (info.TryGetProperty(NPowerRemovedVfx.PropertyName._vfxContainer, ref variant3))
      this._vfxContainer = ((Variant) ref variant3).As<Control>();
    Variant variant4;
    if (info.TryGetProperty(NPowerRemovedVfx.PropertyName._textTween, ref variant4))
      this._textTween = ((Variant) ref variant4).As<Tween>();
    Variant variant5;
    if (!info.TryGetProperty(NPowerRemovedVfx.PropertyName._positionTween, ref variant5))
      return;
    this._positionTween = ((Variant) ref variant5).As<Tween>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _sprite = StringName.op_Implicit(nameof (_sprite));
    public static readonly StringName _powerField = StringName.op_Implicit(nameof (_powerField));
    public static readonly StringName _vfxContainer = StringName.op_Implicit(nameof (_vfxContainer));
    public static readonly StringName _textTween = StringName.op_Implicit(nameof (_textTween));
    public static readonly StringName _positionTween = StringName.op_Implicit(nameof (_positionTween));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
