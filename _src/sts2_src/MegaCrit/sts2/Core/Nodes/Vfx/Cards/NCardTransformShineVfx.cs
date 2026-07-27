// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.Cards.NCardTransformShineVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx.Cards;

[ScriptPath("res://src/Core/Nodes/Vfx/Cards/NCardTransformShineVfx.cs")]
public class NCardTransformShineVfx : Control
{
  public static readonly string scenePath = SceneHelper.GetScenePath("vfx/ui/card/vfx_card_transform");
  [Export]
  private Control _overlay;
  [Export]
  private Control _borderGlow;
  [Export]
  private NParticlesContainer _revealParticles;
  [Export]
  private NParticlesContainer _shineParticles;
  [Export]
  private NParticlesContainer _endParticles;
  [Export]
  private CurveXyzTexture _anticipationScaleCurve;
  [Export]
  private CurveXyzTexture _revealScaleCurve;
  [Export]
  private float _overlayShowDuration = 0.75f;
  [Export]
  private float _overlayShowShortDuration = 0.75f;
  [Export]
  private float _overlayIdleDuration = 0.125f;
  [Export]
  private float _overlayIdleShortDuration = 0.75f;
  [Export]
  private float _overlayHideDuration = 0.25f;
  [Export]
  private float _glowFadeDuration = 0.25f;
  [Export]
  private float _glowTopScale = 1.5f;
  [Export]
  private float _shineDelay = 0.125f;
  [Export]
  private float _endParticlesDelay = 0.4f;
  private NCard _cardNode;
  private CardModel _endCard;
  private IEnumerable<RelicModel>? _relicsToFlash;
  private Tween? _tween;
  private Color _whiteOpaque = new Color(1f, 1f, 1f, 1f);
  private Color _whiteClear = new Color(1f, 1f, 1f, 0.0f);
  private static Vector2 _originalCardScale = new Vector2(1f, 1f);

  public static NCardTransformShineVfx? Create(
    NCard cardNode,
    CardModel endCard,
    IEnumerable<RelicModel>? relicsToFlash)
  {
    if (TestMode.IsOn)
      return (NCardTransformShineVfx) null;
    NCardTransformShineVfx child = PreloadManager.Cache.GetScene(NCardTransformShineVfx.scenePath).Instantiate<NCardTransformShineVfx>((PackedScene.GenEditState) 0L);
    child._cardNode = cardNode;
    child._endCard = endCard;
    child._relicsToFlash = relicsToFlash;
    cardNode.CardVfxContainer.AddChildSafely((Node) child);
    return child;
  }

  public override void _ExitTree() => this._tween?.Kill();

  private async Task<bool> WaitAndInterruptIfNecessary(float seconds, NCard cardNode)
  {
    float num;
    for (float num1 = 0.0f; (double) num1 <= (double) seconds; num1 = num + await ((Node) this).AwaitProcessFrame())
    {
      if (!((Node) cardNode).IsInsideTree() || this._endCard.Pile == null)
        return false;
      num = num1;
    }
    return true;
  }

  private static void UpdateCard(NCard cardNode, CardModel endCard)
  {
    if (endCard.Pile == null)
      return;
    NPlayerHand.Instance?.TryCancelCardPlay(cardNode.Model);
    cardNode.Model = endCard;
    cardNode.UpdateVisuals(endCard.Pile.Type, CardPreviewMode.Normal);
    if (!(NCombatRoom.Instance?.Ui.Hand.GetCardHolder(endCard) is NHandCardHolder cardHolder))
      return;
    cardHolder.UpdateCard();
  }

  public async Task PlayUntilCardUpdate(bool shortVersion = false)
  {
    ((CanvasItem) this._overlay).SelfModulate = this._whiteClear;
    ((CanvasItem) this._borderGlow).SelfModulate = this._whiteClear;
    float num1 = shortVersion ? this._overlayShowShortDuration : this._overlayShowDuration;
    float num2 = shortVersion ? this._overlayIdleShortDuration : this._overlayIdleDuration;
    TaskHelper.RunSafely(this.AnimatingCardScale(this._anticipationScaleCurve, num1 + num2));
    this._tween = ((Node) this).CreateTween();
    this._tween.TweenProperty((GodotObject) this._overlay, NodePath.op_Implicit("self_modulate"), Variant.op_Implicit(this._whiteOpaque), (double) num1).SetEase((Tween.EaseType) 2L).SetTrans((Tween.TransitionType) 3L);
    if (!await this.WaitAndInterruptIfNecessary(num1 + num2, this._cardNode))
    {
      this._cardNode.Scale = NCardTransformShineVfx._originalCardScale;
      ((Node) this).QueueFreeSafely();
    }
    else
      NCardTransformShineVfx.UpdateCard(this._cardNode, this._endCard);
  }

  public async Task PlayShineAndReveal()
  {
    TaskHelper.RunSafely(this.AnimatingCardScale(this._revealScaleCurve, this._glowFadeDuration));
    ((CanvasItem) this._borderGlow).SelfModulate = this._whiteOpaque;
    this._borderGlow.Scale = Vector2.One;
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._overlay, NodePath.op_Implicit("self_modulate"), Variant.op_Implicit(this._whiteClear), (double) this._overlayHideDuration).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 3L);
    this._tween.TweenProperty((GodotObject) this._borderGlow, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, this._glowTopScale)), (double) this._glowFadeDuration).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 2L);
    this._tween.TweenProperty((GodotObject) this._borderGlow, NodePath.op_Implicit("self_modulate"), Variant.op_Implicit(this._whiteClear), (double) this._glowFadeDuration).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 2L);
    if (this._relicsToFlash != null)
    {
      foreach (RelicModel relic in this._relicsToFlash)
      {
        relic.Flash();
        this._cardNode.FlashRelicOnCard(relic);
      }
    }
    this._revealParticles.Restart();
    if (!await this.WaitAndInterruptIfNecessary(this._shineDelay, this._cardNode))
    {
      this._cardNode.Scale = NCardTransformShineVfx._originalCardScale;
      ((Node) this).QueueFreeSafely();
    }
    else
    {
      this._shineParticles.Restart();
      if (!await this.WaitAndInterruptIfNecessary(this._endParticlesDelay, this._cardNode))
      {
        this._cardNode.Scale = NCardTransformShineVfx._originalCardScale;
        ((Node) this).QueueFreeSafely();
      }
      else
      {
        this._endParticles.Restart();
        TaskHelper.RunSafely(this.DelayedFree());
      }
    }
  }

  public async Task PlayAnimation(bool shortVersion = false)
  {
    await this.PlayUntilCardUpdate(shortVersion);
    await this.PlayShineAndReveal();
  }

  public async Task PlayAnimationWithoutWaitingForEnd(bool shortVersion = false)
  {
    await this.PlayUntilCardUpdate(shortVersion);
    TaskHelper.RunSafely(this.PlayShineAndReveal());
  }

  private async Task DelayedFree()
  {
    await Cmd.Wait(2f);
    ((Node) this).QueueFreeSafely();
  }

  private async Task AnimatingCardScale(CurveXyzTexture curve, float duration)
  {
    float num1 = 0.0f;
    Vector2 one = Vector2.One;
    float num;
    for (; (double) num1 < (double) duration; num1 = num + await ((Node) this).AwaitProcessFrame())
    {
      float num2 = num1 / duration;
      float num3 = curve.CurveX.Sample(num2);
      float num4 = curve.CurveY.Sample(num2);
      one.X = num3;
      one.Y = num4;
      this._cardNode.Scale = Vector2.op_Multiply(NCardTransformShineVfx._originalCardScale, one);
      num = num1;
    }
    one.X = curve.CurveX.Sample(1f);
    one.Y = curve.CurveY.Sample(1f);
    this._cardNode.Scale = Vector2.op_Multiply(NCardTransformShineVfx._originalCardScale, one);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NCardTransformShineVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (!StringName.op_Equality(ref method, NCardTransformShineVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCardTransformShineVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardTransformShineVfx.PropertyName._overlay))
    {
      this._overlay = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTransformShineVfx.PropertyName._borderGlow))
    {
      this._borderGlow = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTransformShineVfx.PropertyName._revealParticles))
    {
      this._revealParticles = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTransformShineVfx.PropertyName._shineParticles))
    {
      this._shineParticles = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTransformShineVfx.PropertyName._endParticles))
    {
      this._endParticles = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTransformShineVfx.PropertyName._anticipationScaleCurve))
    {
      this._anticipationScaleCurve = VariantUtils.ConvertTo<CurveXyzTexture>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTransformShineVfx.PropertyName._revealScaleCurve))
    {
      this._revealScaleCurve = VariantUtils.ConvertTo<CurveXyzTexture>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTransformShineVfx.PropertyName._overlayShowDuration))
    {
      this._overlayShowDuration = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTransformShineVfx.PropertyName._overlayShowShortDuration))
    {
      this._overlayShowShortDuration = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTransformShineVfx.PropertyName._overlayIdleDuration))
    {
      this._overlayIdleDuration = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTransformShineVfx.PropertyName._overlayIdleShortDuration))
    {
      this._overlayIdleShortDuration = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTransformShineVfx.PropertyName._overlayHideDuration))
    {
      this._overlayHideDuration = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTransformShineVfx.PropertyName._glowFadeDuration))
    {
      this._glowFadeDuration = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTransformShineVfx.PropertyName._glowTopScale))
    {
      this._glowTopScale = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTransformShineVfx.PropertyName._shineDelay))
    {
      this._shineDelay = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTransformShineVfx.PropertyName._endParticlesDelay))
    {
      this._endParticlesDelay = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTransformShineVfx.PropertyName._cardNode))
    {
      this._cardNode = VariantUtils.ConvertTo<NCard>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTransformShineVfx.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTransformShineVfx.PropertyName._whiteOpaque))
    {
      this._whiteOpaque = VariantUtils.ConvertTo<Color>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardTransformShineVfx.PropertyName._whiteClear))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._whiteClear = VariantUtils.ConvertTo<Color>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardTransformShineVfx.PropertyName._overlay))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._overlay);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTransformShineVfx.PropertyName._borderGlow))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._borderGlow);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTransformShineVfx.PropertyName._revealParticles))
    {
      value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._revealParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTransformShineVfx.PropertyName._shineParticles))
    {
      value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._shineParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTransformShineVfx.PropertyName._endParticles))
    {
      value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._endParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTransformShineVfx.PropertyName._anticipationScaleCurve))
    {
      value = VariantUtils.CreateFrom<CurveXyzTexture>(ref this._anticipationScaleCurve);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTransformShineVfx.PropertyName._revealScaleCurve))
    {
      value = VariantUtils.CreateFrom<CurveXyzTexture>(ref this._revealScaleCurve);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTransformShineVfx.PropertyName._overlayShowDuration))
    {
      value = VariantUtils.CreateFrom<float>(ref this._overlayShowDuration);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTransformShineVfx.PropertyName._overlayShowShortDuration))
    {
      value = VariantUtils.CreateFrom<float>(ref this._overlayShowShortDuration);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTransformShineVfx.PropertyName._overlayIdleDuration))
    {
      value = VariantUtils.CreateFrom<float>(ref this._overlayIdleDuration);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTransformShineVfx.PropertyName._overlayIdleShortDuration))
    {
      value = VariantUtils.CreateFrom<float>(ref this._overlayIdleShortDuration);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTransformShineVfx.PropertyName._overlayHideDuration))
    {
      value = VariantUtils.CreateFrom<float>(ref this._overlayHideDuration);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTransformShineVfx.PropertyName._glowFadeDuration))
    {
      value = VariantUtils.CreateFrom<float>(ref this._glowFadeDuration);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTransformShineVfx.PropertyName._glowTopScale))
    {
      value = VariantUtils.CreateFrom<float>(ref this._glowTopScale);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTransformShineVfx.PropertyName._shineDelay))
    {
      value = VariantUtils.CreateFrom<float>(ref this._shineDelay);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTransformShineVfx.PropertyName._endParticlesDelay))
    {
      value = VariantUtils.CreateFrom<float>(ref this._endParticlesDelay);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTransformShineVfx.PropertyName._cardNode))
    {
      value = VariantUtils.CreateFrom<NCard>(ref this._cardNode);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTransformShineVfx.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTransformShineVfx.PropertyName._whiteOpaque))
    {
      value = VariantUtils.CreateFrom<Color>(ref this._whiteOpaque);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardTransformShineVfx.PropertyName._whiteClear))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Color>(ref this._whiteClear);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCardTransformShineVfx.PropertyName._overlay, (PropertyHint) 34L, "Control", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NCardTransformShineVfx.PropertyName._borderGlow, (PropertyHint) 34L, "Control", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NCardTransformShineVfx.PropertyName._revealParticles, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NCardTransformShineVfx.PropertyName._shineParticles, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NCardTransformShineVfx.PropertyName._endParticles, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NCardTransformShineVfx.PropertyName._anticipationScaleCurve, (PropertyHint) 17L, "CurveXYZTexture", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NCardTransformShineVfx.PropertyName._revealScaleCurve, (PropertyHint) 17L, "CurveXYZTexture", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NCardTransformShineVfx.PropertyName._overlayShowDuration, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NCardTransformShineVfx.PropertyName._overlayShowShortDuration, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NCardTransformShineVfx.PropertyName._overlayIdleDuration, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NCardTransformShineVfx.PropertyName._overlayIdleShortDuration, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NCardTransformShineVfx.PropertyName._overlayHideDuration, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NCardTransformShineVfx.PropertyName._glowFadeDuration, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NCardTransformShineVfx.PropertyName._glowTopScale, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NCardTransformShineVfx.PropertyName._shineDelay, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NCardTransformShineVfx.PropertyName._endParticlesDelay, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NCardTransformShineVfx.PropertyName._cardNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardTransformShineVfx.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NCardTransformShineVfx.PropertyName._whiteOpaque, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NCardTransformShineVfx.PropertyName._whiteClear, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NCardTransformShineVfx.PropertyName._overlay, Variant.From<Control>(ref this._overlay));
    info.AddProperty(NCardTransformShineVfx.PropertyName._borderGlow, Variant.From<Control>(ref this._borderGlow));
    info.AddProperty(NCardTransformShineVfx.PropertyName._revealParticles, Variant.From<NParticlesContainer>(ref this._revealParticles));
    info.AddProperty(NCardTransformShineVfx.PropertyName._shineParticles, Variant.From<NParticlesContainer>(ref this._shineParticles));
    info.AddProperty(NCardTransformShineVfx.PropertyName._endParticles, Variant.From<NParticlesContainer>(ref this._endParticles));
    info.AddProperty(NCardTransformShineVfx.PropertyName._anticipationScaleCurve, Variant.From<CurveXyzTexture>(ref this._anticipationScaleCurve));
    info.AddProperty(NCardTransformShineVfx.PropertyName._revealScaleCurve, Variant.From<CurveXyzTexture>(ref this._revealScaleCurve));
    info.AddProperty(NCardTransformShineVfx.PropertyName._overlayShowDuration, Variant.From<float>(ref this._overlayShowDuration));
    info.AddProperty(NCardTransformShineVfx.PropertyName._overlayShowShortDuration, Variant.From<float>(ref this._overlayShowShortDuration));
    info.AddProperty(NCardTransformShineVfx.PropertyName._overlayIdleDuration, Variant.From<float>(ref this._overlayIdleDuration));
    info.AddProperty(NCardTransformShineVfx.PropertyName._overlayIdleShortDuration, Variant.From<float>(ref this._overlayIdleShortDuration));
    info.AddProperty(NCardTransformShineVfx.PropertyName._overlayHideDuration, Variant.From<float>(ref this._overlayHideDuration));
    info.AddProperty(NCardTransformShineVfx.PropertyName._glowFadeDuration, Variant.From<float>(ref this._glowFadeDuration));
    info.AddProperty(NCardTransformShineVfx.PropertyName._glowTopScale, Variant.From<float>(ref this._glowTopScale));
    info.AddProperty(NCardTransformShineVfx.PropertyName._shineDelay, Variant.From<float>(ref this._shineDelay));
    info.AddProperty(NCardTransformShineVfx.PropertyName._endParticlesDelay, Variant.From<float>(ref this._endParticlesDelay));
    info.AddProperty(NCardTransformShineVfx.PropertyName._cardNode, Variant.From<NCard>(ref this._cardNode));
    info.AddProperty(NCardTransformShineVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NCardTransformShineVfx.PropertyName._whiteOpaque, Variant.From<Color>(ref this._whiteOpaque));
    info.AddProperty(NCardTransformShineVfx.PropertyName._whiteClear, Variant.From<Color>(ref this._whiteClear));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCardTransformShineVfx.PropertyName._overlay, ref variant1))
      this._overlay = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NCardTransformShineVfx.PropertyName._borderGlow, ref variant2))
      this._borderGlow = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NCardTransformShineVfx.PropertyName._revealParticles, ref variant3))
      this._revealParticles = ((Variant) ref variant3).As<NParticlesContainer>();
    Variant variant4;
    if (info.TryGetProperty(NCardTransformShineVfx.PropertyName._shineParticles, ref variant4))
      this._shineParticles = ((Variant) ref variant4).As<NParticlesContainer>();
    Variant variant5;
    if (info.TryGetProperty(NCardTransformShineVfx.PropertyName._endParticles, ref variant5))
      this._endParticles = ((Variant) ref variant5).As<NParticlesContainer>();
    Variant variant6;
    if (info.TryGetProperty(NCardTransformShineVfx.PropertyName._anticipationScaleCurve, ref variant6))
      this._anticipationScaleCurve = ((Variant) ref variant6).As<CurveXyzTexture>();
    Variant variant7;
    if (info.TryGetProperty(NCardTransformShineVfx.PropertyName._revealScaleCurve, ref variant7))
      this._revealScaleCurve = ((Variant) ref variant7).As<CurveXyzTexture>();
    Variant variant8;
    if (info.TryGetProperty(NCardTransformShineVfx.PropertyName._overlayShowDuration, ref variant8))
      this._overlayShowDuration = ((Variant) ref variant8).As<float>();
    Variant variant9;
    if (info.TryGetProperty(NCardTransformShineVfx.PropertyName._overlayShowShortDuration, ref variant9))
      this._overlayShowShortDuration = ((Variant) ref variant9).As<float>();
    Variant variant10;
    if (info.TryGetProperty(NCardTransformShineVfx.PropertyName._overlayIdleDuration, ref variant10))
      this._overlayIdleDuration = ((Variant) ref variant10).As<float>();
    Variant variant11;
    if (info.TryGetProperty(NCardTransformShineVfx.PropertyName._overlayIdleShortDuration, ref variant11))
      this._overlayIdleShortDuration = ((Variant) ref variant11).As<float>();
    Variant variant12;
    if (info.TryGetProperty(NCardTransformShineVfx.PropertyName._overlayHideDuration, ref variant12))
      this._overlayHideDuration = ((Variant) ref variant12).As<float>();
    Variant variant13;
    if (info.TryGetProperty(NCardTransformShineVfx.PropertyName._glowFadeDuration, ref variant13))
      this._glowFadeDuration = ((Variant) ref variant13).As<float>();
    Variant variant14;
    if (info.TryGetProperty(NCardTransformShineVfx.PropertyName._glowTopScale, ref variant14))
      this._glowTopScale = ((Variant) ref variant14).As<float>();
    Variant variant15;
    if (info.TryGetProperty(NCardTransformShineVfx.PropertyName._shineDelay, ref variant15))
      this._shineDelay = ((Variant) ref variant15).As<float>();
    Variant variant16;
    if (info.TryGetProperty(NCardTransformShineVfx.PropertyName._endParticlesDelay, ref variant16))
      this._endParticlesDelay = ((Variant) ref variant16).As<float>();
    Variant variant17;
    if (info.TryGetProperty(NCardTransformShineVfx.PropertyName._cardNode, ref variant17))
      this._cardNode = ((Variant) ref variant17).As<NCard>();
    Variant variant18;
    if (info.TryGetProperty(NCardTransformShineVfx.PropertyName._tween, ref variant18))
      this._tween = ((Variant) ref variant18).As<Tween>();
    Variant variant19;
    if (info.TryGetProperty(NCardTransformShineVfx.PropertyName._whiteOpaque, ref variant19))
      this._whiteOpaque = ((Variant) ref variant19).As<Color>();
    Variant variant20;
    if (!info.TryGetProperty(NCardTransformShineVfx.PropertyName._whiteClear, ref variant20))
      return;
    this._whiteClear = ((Variant) ref variant20).As<Color>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _overlay = StringName.op_Implicit(nameof (_overlay));
    public static readonly StringName _borderGlow = StringName.op_Implicit(nameof (_borderGlow));
    public static readonly StringName _revealParticles = StringName.op_Implicit(nameof (_revealParticles));
    public static readonly StringName _shineParticles = StringName.op_Implicit(nameof (_shineParticles));
    public static readonly StringName _endParticles = StringName.op_Implicit(nameof (_endParticles));
    public static readonly StringName _anticipationScaleCurve = StringName.op_Implicit(nameof (_anticipationScaleCurve));
    public static readonly StringName _revealScaleCurve = StringName.op_Implicit(nameof (_revealScaleCurve));
    public static readonly StringName _overlayShowDuration = StringName.op_Implicit(nameof (_overlayShowDuration));
    public static readonly StringName _overlayShowShortDuration = StringName.op_Implicit(nameof (_overlayShowShortDuration));
    public static readonly StringName _overlayIdleDuration = StringName.op_Implicit(nameof (_overlayIdleDuration));
    public static readonly StringName _overlayIdleShortDuration = StringName.op_Implicit(nameof (_overlayIdleShortDuration));
    public static readonly StringName _overlayHideDuration = StringName.op_Implicit(nameof (_overlayHideDuration));
    public static readonly StringName _glowFadeDuration = StringName.op_Implicit(nameof (_glowFadeDuration));
    public static readonly StringName _glowTopScale = StringName.op_Implicit(nameof (_glowTopScale));
    public static readonly StringName _shineDelay = StringName.op_Implicit(nameof (_shineDelay));
    public static readonly StringName _endParticlesDelay = StringName.op_Implicit(nameof (_endParticlesDelay));
    public static readonly StringName _cardNode = StringName.op_Implicit(nameof (_cardNode));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _whiteOpaque = StringName.op_Implicit(nameof (_whiteOpaque));
    public static readonly StringName _whiteClear = StringName.op_Implicit(nameof (_whiteClear));
  }

  public class SignalName : Control.SignalName
  {
  }
}
