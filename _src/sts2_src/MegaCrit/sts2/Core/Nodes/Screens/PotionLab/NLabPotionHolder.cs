// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.PotionLab.NLabPotionHolder
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Potions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.PotionLab;

[ScriptPath("res://src/Core/Nodes/Screens/PotionLab/NLabPotionHolder.cs")]
public class NLabPotionHolder : Control
{
  public static readonly string scenePath = SceneHelper.GetScenePath("screens/potion_lab/lab_potion_holder");
  public static readonly string lockedIconPath = ImageHelper.GetImagePath("packed/common_ui/locked_model.png");
  private PotionModel _model;
  private NPotion _potionNode;
  private Control _potionHolder;
  private ModelVisibility _visibility;
  private Tween? _hoverTween;

  private LocString UnknownHoverTipTitle
  {
    get => new LocString("main_menu_ui", "POTION_LAB_COLLECTION.unknown.title");
  }

  private LocString UnknownHoverTipDescription
  {
    get => new LocString("main_menu_ui", "POTION_LAB_COLLECTION.unknown.description");
  }

  private HoverTip UnknownHoverTip
  {
    get => new HoverTip(this.UnknownHoverTipTitle, this.UnknownHoverTipDescription);
  }

  private static LocString LockedHoverTipTitle
  {
    get => new LocString("main_menu_ui", "POTION_LAB_COLLECTION.locked.title");
  }

  private static LocString LockedHoverTipDescription
  {
    get => new LocString("main_menu_ui", "POTION_LAB_COLLECTION.locked.description");
  }

  private static HoverTip LockedHoverTip
  {
    get
    {
      return new HoverTip(NLabPotionHolder.LockedHoverTipTitle, NLabPotionHolder.LockedHoverTipDescription);
    }
  }

  public static NLabPotionHolder Create(PotionModel potion, ModelVisibility visibility)
  {
    NLabPotionHolder nlabPotionHolder = PreloadManager.Cache.GetScene(NLabPotionHolder.scenePath).Instantiate<NLabPotionHolder>((PackedScene.GenEditState) 0L);
    nlabPotionHolder._model = potion;
    nlabPotionHolder._visibility = visibility;
    return nlabPotionHolder;
  }

  public override void _Ready()
  {
    this._potionHolder = ((Node) this).GetNode<Control>(NodePath.op_Implicit("PotionHolder"));
    this._potionNode = NPotion.Create(this._model);
    ((Node) this._potionHolder).AddChildSafely((Node) this._potionNode);
    if (this._visibility == ModelVisibility.Locked)
    {
      this._potionNode.Image.Texture = PreloadManager.Cache.GetTexture2D(NLabPotionHolder.lockedIconPath);
      ((CanvasItem) this._potionNode.Outline).Visible = false;
      ((CanvasItem) this._potionNode).Modulate = StsColors.gray;
    }
    else if (this._visibility == ModelVisibility.NotSeen)
    {
      ((CanvasItem) this._potionNode.Image).SelfModulate = StsColors.ninetyPercentBlack;
      ((CanvasItem) this._potionNode.Outline).Modulate = StsColors.halfTransparentWhite;
    }
    else
    {
      foreach (PotionPoolModel characterPotionPool in ModelDb.AllCharacterPotionPools)
      {
        if (characterPotionPool.AllPotions.FirstOrDefault<PotionModel>((Func<PotionModel, bool>) (p => p.Id == this._model.Id)) != null)
        {
          TextureRect outline = this._potionNode.Outline;
          Color labOutlineColor = characterPotionPool.LabOutlineColor;
          labOutlineColor.A = 0.66f;
          Color color = labOutlineColor;
          ((CanvasItem) outline).Modulate = color;
          break;
        }
      }
    }
    this._potionNode.MouseFilter = (Control.MouseFilterEnum) 2L;
    this._potionNode.PivotOffset = Vector2.op_Multiply(this._potionNode.Size, 0.5f);
    this._potionNode.Position = Vector2.Zero;
    ((GodotObject) this).Connect(Control.SignalName.FocusEntered, Callable.From(new Action(this.OnFocus)), 0U);
    ((GodotObject) this).Connect(Control.SignalName.FocusExited, Callable.From(new Action(this.OnUnfocus)), 0U);
    ((GodotObject) this).Connect(Control.SignalName.MouseEntered, Callable.From(new Action(this.OnFocus)), 0U);
    ((GodotObject) this).Connect(Control.SignalName.MouseExited, Callable.From(new Action(this.OnUnfocus)), 0U);
  }

  private void OnFocus()
  {
    this._hoverTween?.Kill();
    NHoverTipSet.Remove((Control) this);
    this._hoverTween = ((Node) this).CreateTween();
    this._hoverTween.TweenProperty((GodotObject) this._potionNode, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 1.2f)), 0.05);
    ModelVisibility visibility = this._visibility;
    IEnumerable<IHoverTip> hoverTips;
    switch (visibility)
    {
      case ModelVisibility.None:
        throw new ArgumentOutOfRangeException();
      case ModelVisibility.Visible:
        hoverTips = this._potionNode.Model.HoverTips;
        break;
      case ModelVisibility.NotSeen:
        // ISSUE: object of a compiler-generated type is created
        hoverTips = (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>((IHoverTip) this.UnknownHoverTip);
        break;
      case ModelVisibility.Locked:
        // ISSUE: object of a compiler-generated type is created
        hoverTips = (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>((IHoverTip) NLabPotionHolder.LockedHoverTip);
        break;
      default:
        // ISSUE: reference to a compiler-generated method
        \u003CPrivateImplementationDetails\u003E.ThrowSwitchExpressionException((object) visibility);
        break;
    }
    NHoverTipSet andShow = NHoverTipSet.CreateAndShow((Control) this, hoverTips, HoverTip.GetHoverTipAlignment((Control) this));
    andShow?.SetFollowOwner();
    andShow?.SetExtraFollowOffset(new Vector2(32f, 0.0f));
  }

  private void OnUnfocus()
  {
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween();
    this._hoverTween.TweenProperty((GodotObject) this._potionNode, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    NHoverTipSet.Remove((Control) this);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NLabPotionHolder.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLabPotionHolder.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLabPotionHolder.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NLabPotionHolder.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NLabPotionHolder.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NLabPotionHolder.MethodName.OnUnfocus) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnUnfocus();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NLabPotionHolder.MethodName._Ready) || StringName.op_Equality(ref method, NLabPotionHolder.MethodName.OnFocus) || StringName.op_Equality(ref method, NLabPotionHolder.MethodName.OnUnfocus) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NLabPotionHolder.PropertyName._potionNode))
    {
      this._potionNode = VariantUtils.ConvertTo<NPotion>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NLabPotionHolder.PropertyName._potionHolder))
    {
      this._potionHolder = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NLabPotionHolder.PropertyName._visibility))
    {
      this._visibility = VariantUtils.ConvertTo<ModelVisibility>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NLabPotionHolder.PropertyName._hoverTween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NLabPotionHolder.PropertyName._potionNode))
    {
      value = VariantUtils.CreateFrom<NPotion>(ref this._potionNode);
      return true;
    }
    if (StringName.op_Equality(ref name, NLabPotionHolder.PropertyName._potionHolder))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._potionHolder);
      return true;
    }
    if (StringName.op_Equality(ref name, NLabPotionHolder.PropertyName._visibility))
    {
      value = VariantUtils.CreateFrom<ModelVisibility>(ref this._visibility);
      return true;
    }
    if (!StringName.op_Equality(ref name, NLabPotionHolder.PropertyName._hoverTween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NLabPotionHolder.PropertyName._potionNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NLabPotionHolder.PropertyName._potionHolder, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NLabPotionHolder.PropertyName._visibility, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NLabPotionHolder.PropertyName._hoverTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NLabPotionHolder.PropertyName._potionNode, Variant.From<NPotion>(ref this._potionNode));
    info.AddProperty(NLabPotionHolder.PropertyName._potionHolder, Variant.From<Control>(ref this._potionHolder));
    info.AddProperty(NLabPotionHolder.PropertyName._visibility, Variant.From<ModelVisibility>(ref this._visibility));
    info.AddProperty(NLabPotionHolder.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NLabPotionHolder.PropertyName._potionNode, ref variant1))
      this._potionNode = ((Variant) ref variant1).As<NPotion>();
    Variant variant2;
    if (info.TryGetProperty(NLabPotionHolder.PropertyName._potionHolder, ref variant2))
      this._potionHolder = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NLabPotionHolder.PropertyName._visibility, ref variant3))
      this._visibility = ((Variant) ref variant3).As<ModelVisibility>();
    Variant variant4;
    if (!info.TryGetProperty(NLabPotionHolder.PropertyName._hoverTween, ref variant4))
      return;
    this._hoverTween = ((Variant) ref variant4).As<Tween>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _potionNode = StringName.op_Implicit(nameof (_potionNode));
    public static readonly StringName _potionHolder = StringName.op_Implicit(nameof (_potionHolder));
    public static readonly StringName _visibility = StringName.op_Implicit(nameof (_visibility));
    public static readonly StringName _hoverTween = StringName.op_Implicit(nameof (_hoverTween));
  }

  public class SignalName : Control.SignalName
  {
  }
}
