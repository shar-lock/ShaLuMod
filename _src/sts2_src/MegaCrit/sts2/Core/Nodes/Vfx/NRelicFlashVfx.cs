// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NRelicFlashVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NRelicFlashVfx.cs")]
public class NRelicFlashVfx : Control
{
  public const float activationDuration = 1f;
  private const string _scenePath = "res://scenes/vfx/relic_flash_vfx.tscn";
  private TextureRect _sprite;
  private TextureRect _sprite2;
  private TextureRect _sprite3;
  private static readonly Vector2 _targetScale = Vector2.op_Multiply(Vector2.One, 1.25f);
  private RelicModel? _relic;
  private Creature? _target;
  private Tween? _tween;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>("res://scenes/vfx/relic_flash_vfx.tscn");
    }
  }

  public static NRelicFlashVfx? Create(RelicModel relic)
  {
    if (TestMode.IsOn)
      return (NRelicFlashVfx) null;
    NRelicFlashVfx nrelicFlashVfx = PreloadManager.Cache.GetScene("res://scenes/vfx/relic_flash_vfx.tscn").Instantiate<NRelicFlashVfx>((PackedScene.GenEditState) 0L);
    nrelicFlashVfx._relic = relic;
    return nrelicFlashVfx;
  }

  public static NRelicFlashVfx? Create(RelicModel relic, Creature target)
  {
    NRelicFlashVfx nrelicFlashVfx = NRelicFlashVfx.Create(relic);
    if (nrelicFlashVfx == null)
      return (NRelicFlashVfx) null;
    nrelicFlashVfx._target = target;
    return nrelicFlashVfx;
  }

  public override void _Ready()
  {
    this._sprite = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("Image1"));
    this._sprite2 = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("Image2"));
    this._sprite3 = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("Image3"));
    if (this._target != null)
      this.GlobalPosition = NCombatRoom.Instance.GetCreatureNode(this._target).GetTopOfHitbox();
    TaskHelper.RunSafely(this.StartVfx());
  }

  public override void _ExitTree() => this._tween?.Kill();

  private async Task StartVfx()
  {
    this._sprite.Texture = this._relic.Icon;
    this._sprite2.Texture = this._relic.Icon;
    this._sprite3.Texture = this._relic.Icon;
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    if (this._target != null)
    {
      this.Position = Vector2.op_Addition(this.Position, new Vector2(0.0f, 64f));
      this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position:y"), Variant.op_Implicit(this.Position.Y - 64f), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    }
    this._tween.TweenProperty((GodotObject) this._sprite, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.01);
    this._tween.TweenProperty((GodotObject) this._sprite, NodePath.op_Implicit("scale"), Variant.op_Implicit(NRelicFlashVfx._targetScale), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    this._tween.TweenProperty((GodotObject) this._sprite, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 1.5).SetDelay(0.01);
    this._tween.TweenProperty((GodotObject) this._sprite2, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.01).SetDelay(0.2);
    this._tween.TweenProperty((GodotObject) this._sprite2, NodePath.op_Implicit("scale"), Variant.op_Implicit(NRelicFlashVfx._targetScale), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L).SetDelay(0.2);
    this._tween.TweenProperty((GodotObject) this._sprite2, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 1.5).SetDelay(0.21);
    this._tween.TweenProperty((GodotObject) this._sprite3, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.01).SetDelay(0.4);
    this._tween.TweenProperty((GodotObject) this._sprite3, NodePath.op_Implicit("scale"), Variant.op_Implicit(NRelicFlashVfx._targetScale), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L).SetDelay(0.4);
    this._tween.TweenProperty((GodotObject) this._sprite3, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 1.5).SetDelay(0.41);
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
      new MethodInfo(NRelicFlashVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRelicFlashVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NRelicFlashVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NRelicFlashVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NRelicFlashVfx.MethodName._Ready) || StringName.op_Equality(ref method, NRelicFlashVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRelicFlashVfx.PropertyName._sprite))
    {
      this._sprite = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicFlashVfx.PropertyName._sprite2))
    {
      this._sprite2 = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicFlashVfx.PropertyName._sprite3))
    {
      this._sprite3 = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRelicFlashVfx.PropertyName._tween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._tween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRelicFlashVfx.PropertyName._sprite))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._sprite);
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicFlashVfx.PropertyName._sprite2))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._sprite2);
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicFlashVfx.PropertyName._sprite3))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._sprite3);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRelicFlashVfx.PropertyName._tween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._tween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NRelicFlashVfx.PropertyName._sprite, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRelicFlashVfx.PropertyName._sprite2, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRelicFlashVfx.PropertyName._sprite3, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRelicFlashVfx.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NRelicFlashVfx.PropertyName._sprite, Variant.From<TextureRect>(ref this._sprite));
    info.AddProperty(NRelicFlashVfx.PropertyName._sprite2, Variant.From<TextureRect>(ref this._sprite2));
    info.AddProperty(NRelicFlashVfx.PropertyName._sprite3, Variant.From<TextureRect>(ref this._sprite3));
    info.AddProperty(NRelicFlashVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NRelicFlashVfx.PropertyName._sprite, ref variant1))
      this._sprite = ((Variant) ref variant1).As<TextureRect>();
    Variant variant2;
    if (info.TryGetProperty(NRelicFlashVfx.PropertyName._sprite2, ref variant2))
      this._sprite2 = ((Variant) ref variant2).As<TextureRect>();
    Variant variant3;
    if (info.TryGetProperty(NRelicFlashVfx.PropertyName._sprite3, ref variant3))
      this._sprite3 = ((Variant) ref variant3).As<TextureRect>();
    Variant variant4;
    if (!info.TryGetProperty(NRelicFlashVfx.PropertyName._tween, ref variant4))
      return;
    this._tween = ((Variant) ref variant4).As<Tween>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _sprite = StringName.op_Implicit(nameof (_sprite));
    public static readonly StringName _sprite2 = StringName.op_Implicit(nameof (_sprite2));
    public static readonly StringName _sprite3 = StringName.op_Implicit(nameof (_sprite3));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
  }

  public class SignalName : Control.SignalName
  {
  }
}
