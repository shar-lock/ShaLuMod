// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Events.Custom.CrystalSphere.NCrystalSphereItem
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent;
using MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent.CrystalSphereItems;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Events.Custom.CrystalSphere;

[ScriptPath("res://src/Core/Nodes/Events/Custom/CrystalSphere/NCrystalSphereItem.cs")]
public class NCrystalSphereItem : Control
{
  public const string scenePath = "res://scenes/events/custom/crystal_sphere/crystal_sphere_item.tscn";
  private CrystalSphereItem _item;
  private TextureRect _icon;
  private Material _material;
  private Control _card;
  private TextureRect _cardFrame;
  private TextureRect _cardBanner;
  private Tween? _tween;

  public static NCrystalSphereItem? Create(CrystalSphereItem item)
  {
    if (TestMode.IsOn)
      return (NCrystalSphereItem) null;
    NCrystalSphereItem ncrystalSphereItem = PreloadManager.Cache.GetScene("res://scenes/events/custom/crystal_sphere/crystal_sphere_item.tscn").Instantiate<NCrystalSphereItem>((PackedScene.GenEditState) 0L);
    ncrystalSphereItem._item = item;
    return ncrystalSphereItem;
  }

  public override void _Ready()
  {
    this._icon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("Icon"));
    this._card = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Card"));
    this._cardFrame = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%CardFrame"));
    this._cardBanner = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%CardBanner"));
    this.PivotOffset = Vector2.op_Division(this.Size, 2f);
    ((Control) this._icon).PivotOffset = Vector2.op_Division(this.Size, 2f);
    this._card.PivotOffset = Vector2.op_Division(this.Size, 2f);
    this._material = ((CanvasItem) this._icon).Material;
    if (this._item is CrystalSphereCardReward sphereCardReward)
    {
      ((CanvasItem) this._card).Visible = true;
      ((CanvasItem) this._icon).Visible = false;
      ((CanvasItem) this._cardBanner).Material = sphereCardReward.BannerMaterial;
      ((CanvasItem) this._cardFrame).Material = sphereCardReward.FrameMaterial;
    }
    else
    {
      ((CanvasItem) this._card).Visible = false;
      ((CanvasItem) this._icon).Visible = true;
      this._icon.Texture = this._item.Texture;
    }
  }

  public override void _EnterTree()
  {
    this._item.Revealed += new Action<CrystalSphereItem>(this.OnRevealed);
  }

  public override void _ExitTree()
  {
    this._item.Revealed -= new Action<CrystalSphereItem>(this.OnRevealed);
    if (this._tween == null || !this._tween.IsRunning())
      return;
    this._tween.Kill();
  }

  private void OnRevealed(CrystalSphereItem item)
  {
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween();
    this._tween.TweenInterval(0.25);
    this._tween.Chain().TweenProperty((GodotObject) this._material, NodePath.op_Implicit("shader_parameter/val"), Variant.op_Implicit(1f), 0.5).From(Variant.op_Implicit(0));
    this._tween.Parallel().TweenProperty((GodotObject) this._icon, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 1.2f)), 0.15000000596046448);
    this._tween.Parallel().TweenProperty((GodotObject) this._icon, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.5).SetDelay(0.15000000596046448);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NCrystalSphereItem.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCrystalSphereItem.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCrystalSphereItem.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCrystalSphereItem.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCrystalSphereItem.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCrystalSphereItem.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCrystalSphereItem.MethodName._Ready) || StringName.op_Equality(ref method, NCrystalSphereItem.MethodName._EnterTree) || StringName.op_Equality(ref method, NCrystalSphereItem.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCrystalSphereItem.PropertyName._icon))
    {
      this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCrystalSphereItem.PropertyName._material))
    {
      this._material = VariantUtils.ConvertTo<Material>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCrystalSphereItem.PropertyName._card))
    {
      this._card = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCrystalSphereItem.PropertyName._cardFrame))
    {
      this._cardFrame = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCrystalSphereItem.PropertyName._cardBanner))
    {
      this._cardBanner = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCrystalSphereItem.PropertyName._tween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._tween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCrystalSphereItem.PropertyName._icon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
      return true;
    }
    if (StringName.op_Equality(ref name, NCrystalSphereItem.PropertyName._material))
    {
      value = VariantUtils.CreateFrom<Material>(ref this._material);
      return true;
    }
    if (StringName.op_Equality(ref name, NCrystalSphereItem.PropertyName._card))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._card);
      return true;
    }
    if (StringName.op_Equality(ref name, NCrystalSphereItem.PropertyName._cardFrame))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._cardFrame);
      return true;
    }
    if (StringName.op_Equality(ref name, NCrystalSphereItem.PropertyName._cardBanner))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._cardBanner);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCrystalSphereItem.PropertyName._tween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._tween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCrystalSphereItem.PropertyName._icon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCrystalSphereItem.PropertyName._material, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCrystalSphereItem.PropertyName._card, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCrystalSphereItem.PropertyName._cardFrame, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCrystalSphereItem.PropertyName._cardBanner, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCrystalSphereItem.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NCrystalSphereItem.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
    info.AddProperty(NCrystalSphereItem.PropertyName._material, Variant.From<Material>(ref this._material));
    info.AddProperty(NCrystalSphereItem.PropertyName._card, Variant.From<Control>(ref this._card));
    info.AddProperty(NCrystalSphereItem.PropertyName._cardFrame, Variant.From<TextureRect>(ref this._cardFrame));
    info.AddProperty(NCrystalSphereItem.PropertyName._cardBanner, Variant.From<TextureRect>(ref this._cardBanner));
    info.AddProperty(NCrystalSphereItem.PropertyName._tween, Variant.From<Tween>(ref this._tween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCrystalSphereItem.PropertyName._icon, ref variant1))
      this._icon = ((Variant) ref variant1).As<TextureRect>();
    Variant variant2;
    if (info.TryGetProperty(NCrystalSphereItem.PropertyName._material, ref variant2))
      this._material = ((Variant) ref variant2).As<Material>();
    Variant variant3;
    if (info.TryGetProperty(NCrystalSphereItem.PropertyName._card, ref variant3))
      this._card = ((Variant) ref variant3).As<Control>();
    Variant variant4;
    if (info.TryGetProperty(NCrystalSphereItem.PropertyName._cardFrame, ref variant4))
      this._cardFrame = ((Variant) ref variant4).As<TextureRect>();
    Variant variant5;
    if (info.TryGetProperty(NCrystalSphereItem.PropertyName._cardBanner, ref variant5))
      this._cardBanner = ((Variant) ref variant5).As<TextureRect>();
    Variant variant6;
    if (!info.TryGetProperty(NCrystalSphereItem.PropertyName._tween, ref variant6))
      return;
    this._tween = ((Variant) ref variant6).As<Tween>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _icon = StringName.op_Implicit(nameof (_icon));
    public static readonly StringName _material = StringName.op_Implicit(nameof (_material));
    public static readonly StringName _card = StringName.op_Implicit(nameof (_card));
    public static readonly StringName _cardFrame = StringName.op_Implicit(nameof (_cardFrame));
    public static readonly StringName _cardBanner = StringName.op_Implicit(nameof (_cardBanner));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
  }

  public class SignalName : Control.SignalName
  {
  }
}
