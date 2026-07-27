// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NPowerFlashVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
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

[ScriptPath("res://src/Core/Nodes/Vfx/NPowerFlashVfx.cs")]
public class NPowerFlashVfx : Node2D
{
  private const string _scenePath = "res://scenes/vfx/power_flash_vfx.tscn";
  private Sprite2D _sprite;
  private PowerModel _power;
  private Tween? _spriteTween;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>("res://scenes/vfx/power_flash_vfx.tscn");
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
      this._sprite = ((Node) this).GetNode<Sprite2D>(NodePath.op_Implicit("Sprite2D"));
      this.GlobalPosition = creatureNode.VfxSpawnPosition;
      TaskHelper.RunSafely(this.StartVfx());
    }
  }

  public override void _ExitTree() => this._spriteTween?.Kill();

  private async Task StartVfx()
  {
    this._sprite.Texture = this._power.BigIcon;
    ((CanvasItem) this._sprite).Modulate = Colors.White;
    this._spriteTween = ((Node) this).CreateTween();
    this._spriteTween.SetParallel(true);
    this._spriteTween.TweenProperty((GodotObject) this._sprite, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 0.4f)), 0.4);
    this._spriteTween.TweenProperty((GodotObject) this._sprite, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 1L);
    this._spriteTween.SetParallel(false);
    this._spriteTween.TweenProperty((GodotObject) this._sprite, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 0.45f)), 0.4);
    this._spriteTween.TweenProperty((GodotObject) this._sprite, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0), 0.25).SetEase((Tween.EaseType) 0L).SetTrans((Tween.TransitionType) 1L);
    bool flag = await this._spriteTween.AwaitFinished((Node) this);
    ((Node) this).QueueFreeSafely();
  }

  public static NPowerFlashVfx? Create(PowerModel power)
  {
    if (TestMode.IsOn)
      return (NPowerFlashVfx) null;
    if (!power.ShouldPlayVfx)
      return (NPowerFlashVfx) null;
    NPowerFlashVfx npowerFlashVfx = (NPowerFlashVfx) PreloadManager.Cache.GetScene("res://scenes/vfx/power_flash_vfx.tscn").Instantiate((PackedScene.GenEditState) 0L);
    npowerFlashVfx._power = power;
    return npowerFlashVfx;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NPowerFlashVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPowerFlashVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NPowerFlashVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NPowerFlashVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NPowerFlashVfx.MethodName._Ready) || StringName.op_Equality(ref method, NPowerFlashVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPowerFlashVfx.PropertyName._sprite))
    {
      this._sprite = VariantUtils.ConvertTo<Sprite2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPowerFlashVfx.PropertyName._spriteTween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._spriteTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPowerFlashVfx.PropertyName._sprite))
    {
      value = VariantUtils.CreateFrom<Sprite2D>(ref this._sprite);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPowerFlashVfx.PropertyName._spriteTween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._spriteTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NPowerFlashVfx.PropertyName._sprite, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPowerFlashVfx.PropertyName._spriteTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NPowerFlashVfx.PropertyName._sprite, Variant.From<Sprite2D>(ref this._sprite));
    info.AddProperty(NPowerFlashVfx.PropertyName._spriteTween, Variant.From<Tween>(ref this._spriteTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NPowerFlashVfx.PropertyName._sprite, ref variant1))
      this._sprite = ((Variant) ref variant1).As<Sprite2D>();
    Variant variant2;
    if (!info.TryGetProperty(NPowerFlashVfx.PropertyName._spriteTween, ref variant2))
      return;
    this._spriteTween = ((Variant) ref variant2).As<Tween>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _sprite = StringName.op_Implicit(nameof (_sprite));
    public static readonly StringName _spriteTween = StringName.op_Implicit(nameof (_spriteTween));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
