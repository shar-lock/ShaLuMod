// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Orbs.NOrb
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Orbs;

[ScriptPath("res://src/Core/Nodes/Orbs/NOrb.cs")]
public class NOrb : NClickableControl
{
  private TextureRect _outline;
  private Control _visualContainer;
  private Control _labelContainer;
  private MegaLabel _passiveLabel;
  private MegaLabel _evokeLabel;
  private Control _bounds;
  private NSelectionReticle _selectionReticle;
  private NOrbVfx? _orbVfx;
  private bool _isLocal;
  private Node2D? _sprite;
  private Tween? _curTween;

  public OrbModel? Model { get; private set; }

  private static string ScenePath => SceneHelper.GetScenePath("/orbs/orb");

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NOrb.ScenePath);
    }
  }

  public static NOrb Create(bool isLocal)
  {
    NOrb norb = PreloadManager.Cache.GetScene(NOrb.ScenePath).Instantiate<NOrb>((PackedScene.GenEditState) 0L);
    norb._isLocal = isLocal;
    return norb;
  }

  public static NOrb Create(bool isLocal, OrbModel? model)
  {
    NOrb norb = NOrb.Create(isLocal);
    norb.Model = model;
    return norb;
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._outline = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Outline"));
    this._visualContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%VisualContainer"));
    this._passiveLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%PassiveAmount"));
    this._evokeLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%EvokeAmount"));
    this._bounds = ((Node) this).GetNode<Control>(NodePath.op_Implicit("Bounds"));
    this._labelContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%LabelContainer"));
    this._selectionReticle = ((Node) this).GetNode<NSelectionReticle>(NodePath.op_Implicit("%SelectionReticle"));
    if (this.Model != null)
      ((Node) this).CreateTween().TweenProperty((GodotObject) this._outline, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.25).From(Variant.op_Implicit(Vector2.Zero));
    if (this._isLocal)
      this.Scale = Vector2.op_Multiply(this.Scale, 0.85f);
    this.UpdateVisuals(false);
  }

  public void ReplaceOrb(OrbModel model)
  {
    Node2D sprite = this._sprite;
    if (sprite != null)
      ((Node) sprite).QueueFreeSafely();
    this._sprite = (Node2D) null;
    this.Model = model;
    this.UpdateVisuals(false);
  }

  public void UpdateVisuals(bool isEvoking)
  {
    if (!((Node) this).IsNodeReady() || !CombatManager.Instance.IsInProgress)
      return;
    if (this.Model == null)
    {
      Node2D sprite = this._sprite;
      if (sprite != null)
        ((Node) sprite).QueueFreeSafely();
      ((CanvasItem) this._passiveLabel).Visible = false;
      ((CanvasItem) this._evokeLabel).Visible = false;
      ((CanvasItem) this._outline).Visible = this._isLocal;
    }
    else
    {
      if (this._sprite == null)
      {
        this._sprite = this.Model.CreateSprite();
        ((Node) this._visualContainer).AddChildSafely((Node) this._sprite);
        this._sprite.Position = Vector2.Zero;
        ((Node) this).RunWhenSpineReady(new MegaSprite(Variant.op_Implicit((GodotObject) ((Node) this._sprite).GetNode(NodePath.op_Implicit("SpineSkeleton")))), (Action<MegaAnimationState>) (animState => animState.SetAnimation("idle_loop")));
        this._orbVfx = this._sprite as NOrbVfx;
        if (this._orbVfx != null)
          this._orbVfx.Initialize(this.Model);
        this._curTween?.Kill();
        this._curTween = ((Node) this).CreateTween();
        this._curTween.TweenProperty((GodotObject) this._sprite, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.5).From(Variant.op_Implicit(Vector2.Zero)).SetTrans((Tween.TransitionType) 10L).SetEase((Tween.EaseType) 1L);
      }
      ((CanvasItem) this._outline).Visible = false;
      ((CanvasItem) this._labelContainer).Visible = this._isLocal;
      if (!this._isLocal)
        ((CanvasItem) this).Modulate = this.Model.DarkenedColor;
      switch (this.Model)
      {
        case PlasmaOrb _:
          ((CanvasItem) this._passiveLabel).Visible = false;
          ((CanvasItem) this._evokeLabel).Visible = false;
          break;
        case DarkOrb _:
          ((CanvasItem) this._passiveLabel).Visible = true;
          ((CanvasItem) this._evokeLabel).Visible = true;
          this._passiveLabel.SetTextAutoSize(this.Model.PassiveVal.ToString("0"));
          this._evokeLabel.SetTextAutoSize(this.Model.EvokeVal.ToString("0"));
          break;
        case GlassOrb _:
          ((CanvasItem) this._passiveLabel).Visible = !isEvoking;
          ((CanvasItem) this._evokeLabel).Visible = isEvoking;
          ((CanvasItem) this._sprite).Modulate = this.Model.PassiveVal == 0M ? this.Model.DarkenedColor : Colors.White;
          MegaLabel passiveLabel = this._passiveLabel;
          Decimal num = this.Model.PassiveVal;
          string text1 = num.ToString("0");
          passiveLabel.SetTextAutoSize(text1);
          MegaLabel evokeLabel = this._evokeLabel;
          num = this.Model.EvokeVal;
          string text2 = num.ToString("0");
          evokeLabel.SetTextAutoSize(text2);
          break;
        default:
          ((CanvasItem) this._passiveLabel).Visible = !isEvoking;
          ((CanvasItem) this._evokeLabel).Visible = isEvoking;
          this._passiveLabel.SetTextAutoSize(this.Model.PassiveVal.ToString("0"));
          this._evokeLabel.SetTextAutoSize(this.Model.EvokeVal.ToString("0"));
          break;
      }
    }
  }

  protected override void OnFocus()
  {
    if (this.Model == null && !this._isLocal)
      return;
    IEnumerable<IHoverTip> hoverTips;
    if (this.Model != null)
      hoverTips = this.Model.HoverTips;
    else
      hoverTips = (IEnumerable<IHoverTip>) new List<IHoverTip>()
      {
        (IHoverTip) OrbModel.EmptySlotHoverTipHoverTip
      };
    NHoverTipSet.CreateAndShow(this._bounds, hoverTips, HoverTip.GetHoverTipAlignment(this._bounds))?.SetFollowOwner();
    ((CanvasItem) this._labelContainer).Visible = true;
    ((CanvasItem) this).Modulate = Colors.White;
    if (!NControllerManager.Instance.IsUsingController)
      return;
    this._selectionReticle.OnSelect();
  }

  protected override void OnUnfocus()
  {
    ((CanvasItem) this._labelContainer).Visible = this._isLocal;
    if (this.Model != null)
      ((CanvasItem) this).Modulate = this._isLocal ? Colors.White : this.Model.DarkenedColor;
    NHoverTipSet.Remove(this._bounds);
    this._selectionReticle.OnDeselect();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NOrb.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("isLocal"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NOrb.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NOrb.MethodName.UpdateVisuals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("isEvoking"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NOrb.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NOrb.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NOrb.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NOrb norb = NOrb.Create(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NOrb>(ref norb);
      return true;
    }
    if (StringName.op_Equality(ref method, NOrb.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NOrb.MethodName.UpdateVisuals) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateVisuals(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NOrb.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NOrb.MethodName.OnUnfocus) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnUnfocus();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NOrb.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NOrb norb = NOrb.Create(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NOrb>(ref norb);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NOrb.MethodName.Create) || StringName.op_Equality(ref method, NOrb.MethodName._Ready) || StringName.op_Equality(ref method, NOrb.MethodName.UpdateVisuals) || StringName.op_Equality(ref method, NOrb.MethodName.OnFocus) || StringName.op_Equality(ref method, NOrb.MethodName.OnUnfocus) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NOrb.PropertyName._outline))
    {
      this._outline = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NOrb.PropertyName._visualContainer))
    {
      this._visualContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NOrb.PropertyName._labelContainer))
    {
      this._labelContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NOrb.PropertyName._passiveLabel))
    {
      this._passiveLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NOrb.PropertyName._evokeLabel))
    {
      this._evokeLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NOrb.PropertyName._bounds))
    {
      this._bounds = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NOrb.PropertyName._selectionReticle))
    {
      this._selectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NOrb.PropertyName._orbVfx))
    {
      this._orbVfx = VariantUtils.ConvertTo<NOrbVfx>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NOrb.PropertyName._isLocal))
    {
      this._isLocal = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NOrb.PropertyName._sprite))
    {
      this._sprite = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NOrb.PropertyName._curTween))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._curTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NOrb.PropertyName._outline))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._outline);
      return true;
    }
    if (StringName.op_Equality(ref name, NOrb.PropertyName._visualContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._visualContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NOrb.PropertyName._labelContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._labelContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NOrb.PropertyName._passiveLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._passiveLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NOrb.PropertyName._evokeLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._evokeLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NOrb.PropertyName._bounds))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._bounds);
      return true;
    }
    if (StringName.op_Equality(ref name, NOrb.PropertyName._selectionReticle))
    {
      value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._selectionReticle);
      return true;
    }
    if (StringName.op_Equality(ref name, NOrb.PropertyName._orbVfx))
    {
      value = VariantUtils.CreateFrom<NOrbVfx>(ref this._orbVfx);
      return true;
    }
    if (StringName.op_Equality(ref name, NOrb.PropertyName._isLocal))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isLocal);
      return true;
    }
    if (StringName.op_Equality(ref name, NOrb.PropertyName._sprite))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._sprite);
      return true;
    }
    if (!StringName.op_Equality(ref name, NOrb.PropertyName._curTween))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Tween>(ref this._curTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NOrb.PropertyName._outline, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NOrb.PropertyName._visualContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NOrb.PropertyName._labelContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NOrb.PropertyName._passiveLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NOrb.PropertyName._evokeLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NOrb.PropertyName._bounds, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NOrb.PropertyName._selectionReticle, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NOrb.PropertyName._orbVfx, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NOrb.PropertyName._isLocal, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NOrb.PropertyName._sprite, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NOrb.PropertyName._curTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NOrb.PropertyName._outline, Variant.From<TextureRect>(ref this._outline));
    info.AddProperty(NOrb.PropertyName._visualContainer, Variant.From<Control>(ref this._visualContainer));
    info.AddProperty(NOrb.PropertyName._labelContainer, Variant.From<Control>(ref this._labelContainer));
    info.AddProperty(NOrb.PropertyName._passiveLabel, Variant.From<MegaLabel>(ref this._passiveLabel));
    info.AddProperty(NOrb.PropertyName._evokeLabel, Variant.From<MegaLabel>(ref this._evokeLabel));
    info.AddProperty(NOrb.PropertyName._bounds, Variant.From<Control>(ref this._bounds));
    info.AddProperty(NOrb.PropertyName._selectionReticle, Variant.From<NSelectionReticle>(ref this._selectionReticle));
    info.AddProperty(NOrb.PropertyName._orbVfx, Variant.From<NOrbVfx>(ref this._orbVfx));
    info.AddProperty(NOrb.PropertyName._isLocal, Variant.From<bool>(ref this._isLocal));
    info.AddProperty(NOrb.PropertyName._sprite, Variant.From<Node2D>(ref this._sprite));
    info.AddProperty(NOrb.PropertyName._curTween, Variant.From<Tween>(ref this._curTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NOrb.PropertyName._outline, ref variant1))
      this._outline = ((Variant) ref variant1).As<TextureRect>();
    Variant variant2;
    if (info.TryGetProperty(NOrb.PropertyName._visualContainer, ref variant2))
      this._visualContainer = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NOrb.PropertyName._labelContainer, ref variant3))
      this._labelContainer = ((Variant) ref variant3).As<Control>();
    Variant variant4;
    if (info.TryGetProperty(NOrb.PropertyName._passiveLabel, ref variant4))
      this._passiveLabel = ((Variant) ref variant4).As<MegaLabel>();
    Variant variant5;
    if (info.TryGetProperty(NOrb.PropertyName._evokeLabel, ref variant5))
      this._evokeLabel = ((Variant) ref variant5).As<MegaLabel>();
    Variant variant6;
    if (info.TryGetProperty(NOrb.PropertyName._bounds, ref variant6))
      this._bounds = ((Variant) ref variant6).As<Control>();
    Variant variant7;
    if (info.TryGetProperty(NOrb.PropertyName._selectionReticle, ref variant7))
      this._selectionReticle = ((Variant) ref variant7).As<NSelectionReticle>();
    Variant variant8;
    if (info.TryGetProperty(NOrb.PropertyName._orbVfx, ref variant8))
      this._orbVfx = ((Variant) ref variant8).As<NOrbVfx>();
    Variant variant9;
    if (info.TryGetProperty(NOrb.PropertyName._isLocal, ref variant9))
      this._isLocal = ((Variant) ref variant9).As<bool>();
    Variant variant10;
    if (info.TryGetProperty(NOrb.PropertyName._sprite, ref variant10))
      this._sprite = ((Variant) ref variant10).As<Node2D>();
    Variant variant11;
    if (!info.TryGetProperty(NOrb.PropertyName._curTween, ref variant11))
      return;
    this._curTween = ((Variant) ref variant11).As<Tween>();
  }

  public new class MethodName : NClickableControl.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName UpdateVisuals = StringName.op_Implicit(nameof (UpdateVisuals));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
  }

  public new class PropertyName : NClickableControl.PropertyName
  {
    public static readonly StringName _outline = StringName.op_Implicit(nameof (_outline));
    public static readonly StringName _visualContainer = StringName.op_Implicit(nameof (_visualContainer));
    public static readonly StringName _labelContainer = StringName.op_Implicit(nameof (_labelContainer));
    public static readonly StringName _passiveLabel = StringName.op_Implicit(nameof (_passiveLabel));
    public static readonly StringName _evokeLabel = StringName.op_Implicit(nameof (_evokeLabel));
    public static readonly StringName _bounds = StringName.op_Implicit(nameof (_bounds));
    public static readonly StringName _selectionReticle = StringName.op_Implicit(nameof (_selectionReticle));
    public static readonly StringName _orbVfx = StringName.op_Implicit(nameof (_orbVfx));
    public static readonly StringName _isLocal = StringName.op_Implicit(nameof (_isLocal));
    public static readonly StringName _sprite = StringName.op_Implicit(nameof (_sprite));
    public static readonly StringName _curTween = StringName.op_Implicit(nameof (_curTween));
  }

  public new class SignalName : NClickableControl.SignalName
  {
  }
}
