// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.RelicCollection.NRelicCollectionEntry
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
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Relics;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.RelicCollection;

[ScriptPath("res://src/Core/Nodes/Screens/RelicCollection/NRelicCollectionEntry.cs")]
public class NRelicCollectionEntry : NButton
{
  public static readonly string scenePath = SceneHelper.GetScenePath("screens/relic_collection/relic_collection_entry");
  public static readonly string lockedIconPath = ImageHelper.GetImagePath("packed/common_ui/locked_model.png");
  public RelicModel relic;
  private Control _relicHolder;
  private Control _relicNode;
  private Tween? _hoverTween;

  private static LocString UnknownHoverTipTitle
  {
    get => new LocString("main_menu_ui", "COMPENDIUM_RELIC_COLLECTION.unknown.title");
  }

  private static LocString UnknownHoverTipDescription
  {
    get => new LocString("main_menu_ui", "COMPENDIUM_RELIC_COLLECTION.unknown.description");
  }

  private static HoverTip UnknownHoverTip
  {
    get
    {
      return new HoverTip(NRelicCollectionEntry.UnknownHoverTipTitle, NRelicCollectionEntry.UnknownHoverTipDescription);
    }
  }

  private static LocString LockedHoverTipTitle
  {
    get => new LocString("main_menu_ui", "COMPENDIUM_RELIC_COLLECTION.locked.title");
  }

  private static LocString LockedHoverTipDescription
  {
    get => new LocString("main_menu_ui", "COMPENDIUM_RELIC_COLLECTION.locked.description");
  }

  private static HoverTip LockedHoverTip
  {
    get
    {
      return new HoverTip(NRelicCollectionEntry.LockedHoverTipTitle, NRelicCollectionEntry.LockedHoverTipDescription);
    }
  }

  public ModelVisibility ModelVisibility { get; set; }

  public static NRelicCollectionEntry Create(RelicModel relic, ModelVisibility visibility)
  {
    NRelicCollectionEntry nrelicCollectionEntry = PreloadManager.Cache.GetScene(NRelicCollectionEntry.scenePath).Instantiate<NRelicCollectionEntry>((PackedScene.GenEditState) 0L);
    nrelicCollectionEntry.relic = relic;
    nrelicCollectionEntry.ModelVisibility = visibility;
    return nrelicCollectionEntry;
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._relicHolder = ((Node) this).GetNode<Control>(NodePath.op_Implicit("RelicHolder"));
    if (this.ModelVisibility == ModelVisibility.Locked)
    {
      TextureRect child = new TextureRect();
      child.ExpandMode = (TextureRect.ExpandModeEnum) 1L;
      child.StretchMode = (TextureRect.StretchModeEnum) 4L;
      child.Texture = PreloadManager.Cache.GetTexture2D(NRelicCollectionEntry.lockedIconPath);
      ((Control) child).Size = Vector2.op_Multiply(Vector2.One, 68f);
      ((Control) child).PivotOffset = Vector2.op_Multiply(((Control) child).Size, 0.5f);
      ((CanvasItem) child).Modulate = StsColors.gray;
      ((Node) this._relicHolder).AddChildSafely((Node) child);
      this._relicNode = (Control) child;
    }
    else
    {
      NRelic child = NRelic.Create(this.relic.ToMutable(), NRelic.IconSize.Small);
      ((Node) this._relicHolder).AddChildSafely((Node) child);
      if (this.ModelVisibility == ModelVisibility.NotSeen)
      {
        ((CanvasItem) child.Icon).SelfModulate = StsColors.ninetyPercentBlack;
        ((CanvasItem) child.Outline).SelfModulate = StsColors.halfTransparentWhite;
      }
      else
      {
        foreach (RelicPoolModel characterRelicPool in ModelDb.AllCharacterRelicPools)
        {
          if (characterRelicPool.AllRelicIds.Contains(this.relic.Id))
          {
            TextureRect outline = child.Outline;
            Color labOutlineColor = characterRelicPool.LabOutlineColor;
            labOutlineColor.A = 0.66f;
            Color color = labOutlineColor;
            ((CanvasItem) outline).SelfModulate = color;
            break;
          }
        }
      }
      this._relicNode = (Control) child;
    }
    this._relicNode.MouseFilter = (Control.MouseFilterEnum) 2L;
    this._relicNode.FocusMode = (Control.FocusModeEnum) 0L;
  }

  protected override void OnRelease()
  {
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween();
    this._hoverTween.TweenProperty((GodotObject) this._relicNode, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    NHoverTipSet.Remove((Control) this);
  }

  protected override void OnFocus()
  {
    this._hoverTween?.Kill();
    this._relicNode.Scale = Vector2.op_Multiply(Vector2.One, 1.25f);
    ModelVisibility modelVisibility = this.ModelVisibility;
    IEnumerable<IHoverTip> hoverTips;
    switch (modelVisibility)
    {
      case ModelVisibility.None:
        throw new ArgumentOutOfRangeException();
      case ModelVisibility.Visible:
        hoverTips = this.relic.HoverTips;
        break;
      case ModelVisibility.NotSeen:
        // ISSUE: object of a compiler-generated type is created
        hoverTips = (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>((IHoverTip) NRelicCollectionEntry.UnknownHoverTip);
        break;
      case ModelVisibility.Locked:
        // ISSUE: object of a compiler-generated type is created
        hoverTips = (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>((IHoverTip) NRelicCollectionEntry.LockedHoverTip);
        break;
      default:
        // ISSUE: reference to a compiler-generated method
        \u003CPrivateImplementationDetails\u003E.ThrowSwitchExpressionException((object) modelVisibility);
        break;
    }
    NHoverTipSet.CreateAndShow((Control) this, hoverTips, HoverTip.GetHoverTipAlignment((Control) this))?.SetFollowOwner();
  }

  protected override void OnUnfocus()
  {
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween();
    this._hoverTween.TweenProperty((GodotObject) this._relicNode, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    NHoverTipSet.Remove((Control) this);
  }

  protected override void OnPress()
  {
    base.OnPress();
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween();
    this._hoverTween.TweenProperty((GodotObject) this._relicNode, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NRelicCollectionEntry.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRelicCollectionEntry.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRelicCollectionEntry.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRelicCollectionEntry.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRelicCollectionEntry.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NRelicCollectionEntry.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRelicCollectionEntry.MethodName.OnRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRelease();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRelicCollectionEntry.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRelicCollectionEntry.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NRelicCollectionEntry.MethodName.OnPress) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnPress();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NRelicCollectionEntry.MethodName._Ready) || StringName.op_Equality(ref method, NRelicCollectionEntry.MethodName.OnRelease) || StringName.op_Equality(ref method, NRelicCollectionEntry.MethodName.OnFocus) || StringName.op_Equality(ref method, NRelicCollectionEntry.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NRelicCollectionEntry.MethodName.OnPress) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRelicCollectionEntry.PropertyName.ModelVisibility))
    {
      this.ModelVisibility = VariantUtils.ConvertTo<ModelVisibility>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicCollectionEntry.PropertyName._relicHolder))
    {
      this._relicHolder = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicCollectionEntry.PropertyName._relicNode))
    {
      this._relicNode = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRelicCollectionEntry.PropertyName._hoverTween))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRelicCollectionEntry.PropertyName.ModelVisibility))
    {
      ref godot_variant local = ref value;
      ModelVisibility modelVisibility = this.ModelVisibility;
      godot_variant from = VariantUtils.CreateFrom<ModelVisibility>(ref modelVisibility);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicCollectionEntry.PropertyName._relicHolder))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._relicHolder);
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicCollectionEntry.PropertyName._relicNode))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._relicNode);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRelicCollectionEntry.PropertyName._hoverTween))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 2L, NRelicCollectionEntry.PropertyName.ModelVisibility, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRelicCollectionEntry.PropertyName._relicHolder, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRelicCollectionEntry.PropertyName._relicNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRelicCollectionEntry.PropertyName._hoverTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName modelVisibility1 = NRelicCollectionEntry.PropertyName.ModelVisibility;
    ModelVisibility modelVisibility2 = this.ModelVisibility;
    Variant variant = Variant.From<ModelVisibility>(ref modelVisibility2);
    serializationInfo.AddProperty(modelVisibility1, variant);
    info.AddProperty(NRelicCollectionEntry.PropertyName._relicHolder, Variant.From<Control>(ref this._relicHolder));
    info.AddProperty(NRelicCollectionEntry.PropertyName._relicNode, Variant.From<Control>(ref this._relicNode));
    info.AddProperty(NRelicCollectionEntry.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NRelicCollectionEntry.PropertyName.ModelVisibility, ref variant1))
      this.ModelVisibility = ((Variant) ref variant1).As<ModelVisibility>();
    Variant variant2;
    if (info.TryGetProperty(NRelicCollectionEntry.PropertyName._relicHolder, ref variant2))
      this._relicHolder = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NRelicCollectionEntry.PropertyName._relicNode, ref variant3))
      this._relicNode = ((Variant) ref variant3).As<Control>();
    Variant variant4;
    if (!info.TryGetProperty(NRelicCollectionEntry.PropertyName._hoverTween, ref variant4))
      return;
    this._hoverTween = ((Variant) ref variant4).As<Tween>();
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public new static readonly StringName OnPress = StringName.op_Implicit(nameof (OnPress));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public static readonly StringName ModelVisibility = StringName.op_Implicit(nameof (ModelVisibility));
    public static readonly StringName _relicHolder = StringName.op_Implicit(nameof (_relicHolder));
    public static readonly StringName _relicNode = StringName.op_Implicit(nameof (_relicNode));
    public static readonly StringName _hoverTween = StringName.op_Implicit(nameof (_hoverTween));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
