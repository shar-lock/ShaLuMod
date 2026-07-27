// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NPowerAppliedVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
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

[ScriptPath("res://src/Core/Nodes/Vfx/NPowerAppliedVfx.cs")]
public class NPowerAppliedVfx : Control
{
  private const string _scenePath = "res://scenes/vfx/power_applied_vfx.tscn";
  private TextureRect _icon;
  private TextureRect _iconEcho;
  private MegaLabel _powerField;
  private PowerModel _power;
  private int _amount;
  private bool _isBuff;
  private Tween? _textTween;
  private Tween? _spriteTween;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>("res://scenes/vfx/power_applied_vfx.tscn");
    }
  }

  public override void _Ready()
  {
    this._icon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("Icon"));
    this._iconEcho = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("Icon/IconEcho"));
    this._powerField = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("Label"));
    NCreature creatureNode = NCombatRoom.Instance?.GetCreatureNode(this._power.Owner);
    if (creatureNode == null)
    {
      ((Node) this).QueueFreeSafely();
    }
    else
    {
      this.GlobalPosition = creatureNode.VfxSpawnPosition;
      TaskHelper.RunSafely(this.StartVfx());
    }
  }

  public static NPowerAppliedVfx? Create(PowerModel power, int amount, bool isBuff)
  {
    if (TestMode.IsOn)
      return (NPowerAppliedVfx) null;
    if (NCombatUi.IsDebugHideTextVfx)
      return (NPowerAppliedVfx) null;
    if (!power.ShouldPlayVfx)
      return (NPowerAppliedVfx) null;
    NPowerAppliedVfx npowerAppliedVfx = PreloadManager.Cache.GetScene("res://scenes/vfx/power_applied_vfx.tscn").Instantiate<NPowerAppliedVfx>((PackedScene.GenEditState) 0L);
    npowerAppliedVfx._power = power;
    npowerAppliedVfx._amount = amount;
    npowerAppliedVfx._isBuff = isBuff;
    return npowerAppliedVfx;
  }

  public override void _ExitTree()
  {
    this._spriteTween?.Kill();
    this._textTween?.Kill();
  }

  private async Task StartVfx()
  {
    this._powerField.SetTextAutoSize(this._power.Title.GetFormattedText());
    this._icon.Texture = this._power.BigIcon;
    this._iconEcho.Texture = this._power.BigIcon;
    ((CanvasItem) this._powerField).Modulate = this._isBuff ? StsColors.green : StsColors.red;
    ((Control) this._powerField).Position = new Vector2(((Control) this._powerField).Position.X, ((Control) this._powerField).Position.Y + NCreature.PowerAppliedVfxPositionOffset.Y);
    this._spriteTween = ((Node) this).CreateTween().SetParallel(true);
    this._spriteTween.TweenProperty((GodotObject) this._icon, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 0.8f)), 1.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L).From(Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 0.4f)));
    this._spriteTween.TweenProperty((GodotObject) this._icon, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.5f), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 1L);
    this._spriteTween.TweenProperty((GodotObject) this._icon, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 1.0).SetEase((Tween.EaseType) 0L).SetTrans((Tween.TransitionType) 1L).SetDelay(0.25);
    float num = ((Control) this._powerField).Position.Y + (this._isBuff ? -100f : 100f);
    this._textTween = ((Node) this).CreateTween().SetParallel(true);
    this._textTween.TweenProperty((GodotObject) this._powerField, NodePath.op_Implicit("position:y"), Variant.op_Implicit(num), 1.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    this._textTween.TweenProperty((GodotObject) this._powerField, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._textTween.TweenProperty((GodotObject) this._powerField, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.75).SetEase((Tween.EaseType) 0L).SetTrans((Tween.TransitionType) 5L).From(Variant.op_Implicit(1f)).SetDelay(0.25);
    bool flag = await this._spriteTween.AwaitFinished((Node) this);
    ((Node) this).QueueFreeSafely();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NPowerAppliedVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPowerAppliedVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NPowerAppliedVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NPowerAppliedVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NPowerAppliedVfx.MethodName._Ready) || StringName.op_Equality(ref method, NPowerAppliedVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPowerAppliedVfx.PropertyName._icon))
    {
      this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPowerAppliedVfx.PropertyName._iconEcho))
    {
      this._iconEcho = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPowerAppliedVfx.PropertyName._powerField))
    {
      this._powerField = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPowerAppliedVfx.PropertyName._amount))
    {
      this._amount = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPowerAppliedVfx.PropertyName._isBuff))
    {
      this._isBuff = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPowerAppliedVfx.PropertyName._textTween))
    {
      this._textTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPowerAppliedVfx.PropertyName._spriteTween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._spriteTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPowerAppliedVfx.PropertyName._icon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
      return true;
    }
    if (StringName.op_Equality(ref name, NPowerAppliedVfx.PropertyName._iconEcho))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._iconEcho);
      return true;
    }
    if (StringName.op_Equality(ref name, NPowerAppliedVfx.PropertyName._powerField))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._powerField);
      return true;
    }
    if (StringName.op_Equality(ref name, NPowerAppliedVfx.PropertyName._amount))
    {
      value = VariantUtils.CreateFrom<int>(ref this._amount);
      return true;
    }
    if (StringName.op_Equality(ref name, NPowerAppliedVfx.PropertyName._isBuff))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isBuff);
      return true;
    }
    if (StringName.op_Equality(ref name, NPowerAppliedVfx.PropertyName._textTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._textTween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPowerAppliedVfx.PropertyName._spriteTween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._spriteTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NPowerAppliedVfx.PropertyName._icon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPowerAppliedVfx.PropertyName._iconEcho, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPowerAppliedVfx.PropertyName._powerField, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NPowerAppliedVfx.PropertyName._amount, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NPowerAppliedVfx.PropertyName._isBuff, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPowerAppliedVfx.PropertyName._textTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPowerAppliedVfx.PropertyName._spriteTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NPowerAppliedVfx.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
    info.AddProperty(NPowerAppliedVfx.PropertyName._iconEcho, Variant.From<TextureRect>(ref this._iconEcho));
    info.AddProperty(NPowerAppliedVfx.PropertyName._powerField, Variant.From<MegaLabel>(ref this._powerField));
    info.AddProperty(NPowerAppliedVfx.PropertyName._amount, Variant.From<int>(ref this._amount));
    info.AddProperty(NPowerAppliedVfx.PropertyName._isBuff, Variant.From<bool>(ref this._isBuff));
    info.AddProperty(NPowerAppliedVfx.PropertyName._textTween, Variant.From<Tween>(ref this._textTween));
    info.AddProperty(NPowerAppliedVfx.PropertyName._spriteTween, Variant.From<Tween>(ref this._spriteTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NPowerAppliedVfx.PropertyName._icon, ref variant1))
      this._icon = ((Variant) ref variant1).As<TextureRect>();
    Variant variant2;
    if (info.TryGetProperty(NPowerAppliedVfx.PropertyName._iconEcho, ref variant2))
      this._iconEcho = ((Variant) ref variant2).As<TextureRect>();
    Variant variant3;
    if (info.TryGetProperty(NPowerAppliedVfx.PropertyName._powerField, ref variant3))
      this._powerField = ((Variant) ref variant3).As<MegaLabel>();
    Variant variant4;
    if (info.TryGetProperty(NPowerAppliedVfx.PropertyName._amount, ref variant4))
      this._amount = ((Variant) ref variant4).As<int>();
    Variant variant5;
    if (info.TryGetProperty(NPowerAppliedVfx.PropertyName._isBuff, ref variant5))
      this._isBuff = ((Variant) ref variant5).As<bool>();
    Variant variant6;
    if (info.TryGetProperty(NPowerAppliedVfx.PropertyName._textTween, ref variant6))
      this._textTween = ((Variant) ref variant6).As<Tween>();
    Variant variant7;
    if (!info.TryGetProperty(NPowerAppliedVfx.PropertyName._spriteTween, ref variant7))
      return;
    this._spriteTween = ((Variant) ref variant7).As<Tween>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _icon = StringName.op_Implicit(nameof (_icon));
    public static readonly StringName _iconEcho = StringName.op_Implicit(nameof (_iconEcho));
    public static readonly StringName _powerField = StringName.op_Implicit(nameof (_powerField));
    public static readonly StringName _amount = StringName.op_Implicit(nameof (_amount));
    public static readonly StringName _isBuff = StringName.op_Implicit(nameof (_isBuff));
    public static readonly StringName _textTween = StringName.op_Implicit(nameof (_textTween));
    public static readonly StringName _spriteTween = StringName.op_Implicit(nameof (_spriteTween));
  }

  public class SignalName : Control.SignalName
  {
  }
}
