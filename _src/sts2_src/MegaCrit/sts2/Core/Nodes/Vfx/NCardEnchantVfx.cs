// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NCardEnchantVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NCardEnchantVfx.cs")]
public class NCardEnchantVfx : Node2D
{
  private static readonly StringName _progress = new StringName("progress");
  private Tween? _tween;
  private CancellationTokenSource? _cts;
  private CardModel _cardModel;
  private NCard _cardNode;
  private GpuParticles2D _enchantmentSparkles;
  private TextureRect _enchantmentIcon;
  private MegaLabel _enchantmentLabel;

  private static string ScenePath => SceneHelper.GetScenePath("vfx/vfx_card_enchant");

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NCardEnchantVfx.ScenePath);
    }
  }

  [Export]
  public Curve? EmbossCurve { get; set; }

  public static NCardEnchantVfx? Create(CardModel card)
  {
    if (TestMode.IsOn)
      return (NCardEnchantVfx) null;
    if (!LocalContext.IsMine(card))
      return (NCardEnchantVfx) null;
    NCardEnchantVfx ncardEnchantVfx = PreloadManager.Cache.GetScene(NCardEnchantVfx.ScenePath).Instantiate<NCardEnchantVfx>((PackedScene.GenEditState) 0L);
    ncardEnchantVfx._cardModel = card;
    return ncardEnchantVfx;
  }

  public override void _Ready()
  {
    this._enchantmentSparkles = ((Node) this).GetNode<GpuParticles2D>(NodePath.op_Implicit("%EnchantmentAppearSparkles"));
    this._enchantmentIcon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%EnchantmentInViewport/Icon"));
    this._enchantmentLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%EnchantmentInViewport/Label"));
    this._enchantmentIcon.Texture = (Texture2D) this._cardModel.Enchantment.Icon;
    this._enchantmentLabel.SetTextAutoSize(this._cardModel.Enchantment.DisplayAmount.ToString());
    ((CanvasItem) this._enchantmentLabel).Visible = this._cardModel.Enchantment.ShowAmount;
    this._cardNode = NCard.Create(this._cardModel);
    ((Node) this).AddChildSafely((Node) this._cardNode);
    ((Node) this).MoveChildSafely((Node) this._cardNode, 0);
    this._cardNode.UpdateVisuals(PileType.None, CardPreviewMode.Normal);
    ((CanvasItem) this._cardNode.EnchantmentTab).Visible = false;
    ((CanvasItem) this._cardNode.EnchantmentVfxOverride).Visible = true;
    this._cardNode.EnchantmentVfxOverride.Texture = (Texture2D) ((Node) this).GetNode<Viewport>(NodePath.op_Implicit("%EnchantmentViewport")).GetTexture();
    TaskHelper.RunSafely(this.PlayAnimation());
  }

  public override void _ExitTree()
  {
    this._tween?.Kill();
    this._cts?.Cancel();
    if (!((Node) this._cardNode).IsValid() || !((Node) this).IsAncestorOf((Node) this._cardNode))
      return;
    ((Node) this._cardNode).QueueFreeSafely();
  }

  private async Task PlayAnimation()
  {
    this._cts = new CancellationTokenSource();
    ((ShaderMaterial) ((CanvasItem) this._cardNode.EnchantmentVfxOverride).Material).SetShaderParameter(NCardEnchantVfx._progress, Variant.op_Implicit(0.0f));
    this._tween = ((Node) this).CreateTween();
    SfxCmd.Play("event:/sfx/ui/enchant_shimmer");
    this._tween.TweenProperty((GodotObject) this._cardNode.EnchantmentVfxOverride, NodePath.op_Implicit("material:shader_parameter/progress"), Variant.op_Implicit(1f), 1.0).SetEase((Tween.EaseType) 2L).SetTrans((Tween.TransitionType) 4L);
    this._tween.Parallel().TweenCallback(Callable.From<bool>((Func<bool>) (() => this._enchantmentSparkles.Emitting = true))).SetDelay(0.20000000298023224);
    this._tween.Parallel().TweenProperty((GodotObject) this._enchantmentSparkles, NodePath.op_Implicit("position:x"), Variant.op_Implicit(((Node2D) this._enchantmentSparkles).Position.X + 72f), 0.40000000596046448).SetDelay(0.20000000298023224);
    if (!await this._tween.AwaitFinished((Node) this))
      return;
    await Cmd.Wait(1f, this._cts.Token);
    CardModel model = this._cardNode.Model;
    if (((Node) this._cardNode).IsInsideTree() && model.Pile == null)
    {
      this._tween = ((Node) this).CreateTween();
      this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.Zero), 0.15000000596046448);
      if (!await this._tween.AwaitFinished((Node) this))
        return;
    }
    else if (((Node) this._cardNode).IsInsideTree())
    {
      NCardFlyVfx child = NCardFlyVfx.Create(this._cardNode, model.Pile.Type, false, model.Owner.Character.TrailPath);
      NRun instance = NRun.Instance;
      if (instance != null)
        instance.GlobalUi.TopBar.TrailContainer.AddChildSafely((Node) child);
      if (child.SwooshAwayCompletion != null)
        await child.SwooshAwayCompletion.Task;
    }
    ((Node) this).QueueFreeSafely();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NCardEnchantVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardEnchantVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCardEnchantVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCardEnchantVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCardEnchantVfx.MethodName._Ready) || StringName.op_Equality(ref method, NCardEnchantVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardEnchantVfx.PropertyName.EmbossCurve))
    {
      this.EmbossCurve = VariantUtils.ConvertTo<Curve>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardEnchantVfx.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardEnchantVfx.PropertyName._cardNode))
    {
      this._cardNode = VariantUtils.ConvertTo<NCard>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardEnchantVfx.PropertyName._enchantmentSparkles))
    {
      this._enchantmentSparkles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardEnchantVfx.PropertyName._enchantmentIcon))
    {
      this._enchantmentIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardEnchantVfx.PropertyName._enchantmentLabel))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._enchantmentLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardEnchantVfx.PropertyName.EmbossCurve))
    {
      ref godot_variant local = ref value;
      Curve embossCurve = this.EmbossCurve;
      godot_variant from = VariantUtils.CreateFrom<Curve>(ref embossCurve);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardEnchantVfx.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardEnchantVfx.PropertyName._cardNode))
    {
      value = VariantUtils.CreateFrom<NCard>(ref this._cardNode);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardEnchantVfx.PropertyName._enchantmentSparkles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._enchantmentSparkles);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardEnchantVfx.PropertyName._enchantmentIcon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._enchantmentIcon);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardEnchantVfx.PropertyName._enchantmentLabel))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<MegaLabel>(ref this._enchantmentLabel);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCardEnchantVfx.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardEnchantVfx.PropertyName.EmbossCurve, (PropertyHint) 17L, "Curve", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NCardEnchantVfx.PropertyName._cardNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardEnchantVfx.PropertyName._enchantmentSparkles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardEnchantVfx.PropertyName._enchantmentIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardEnchantVfx.PropertyName._enchantmentLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName embossCurve1 = NCardEnchantVfx.PropertyName.EmbossCurve;
    Curve embossCurve2 = this.EmbossCurve;
    Variant variant = Variant.From<Curve>(ref embossCurve2);
    serializationInfo.AddProperty(embossCurve1, variant);
    info.AddProperty(NCardEnchantVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NCardEnchantVfx.PropertyName._cardNode, Variant.From<NCard>(ref this._cardNode));
    info.AddProperty(NCardEnchantVfx.PropertyName._enchantmentSparkles, Variant.From<GpuParticles2D>(ref this._enchantmentSparkles));
    info.AddProperty(NCardEnchantVfx.PropertyName._enchantmentIcon, Variant.From<TextureRect>(ref this._enchantmentIcon));
    info.AddProperty(NCardEnchantVfx.PropertyName._enchantmentLabel, Variant.From<MegaLabel>(ref this._enchantmentLabel));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCardEnchantVfx.PropertyName.EmbossCurve, ref variant1))
      this.EmbossCurve = ((Variant) ref variant1).As<Curve>();
    Variant variant2;
    if (info.TryGetProperty(NCardEnchantVfx.PropertyName._tween, ref variant2))
      this._tween = ((Variant) ref variant2).As<Tween>();
    Variant variant3;
    if (info.TryGetProperty(NCardEnchantVfx.PropertyName._cardNode, ref variant3))
      this._cardNode = ((Variant) ref variant3).As<NCard>();
    Variant variant4;
    if (info.TryGetProperty(NCardEnchantVfx.PropertyName._enchantmentSparkles, ref variant4))
      this._enchantmentSparkles = ((Variant) ref variant4).As<GpuParticles2D>();
    Variant variant5;
    if (info.TryGetProperty(NCardEnchantVfx.PropertyName._enchantmentIcon, ref variant5))
      this._enchantmentIcon = ((Variant) ref variant5).As<TextureRect>();
    Variant variant6;
    if (!info.TryGetProperty(NCardEnchantVfx.PropertyName._enchantmentLabel, ref variant6))
      return;
    this._enchantmentLabel = ((Variant) ref variant6).As<MegaLabel>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName EmbossCurve = StringName.op_Implicit(nameof (EmbossCurve));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _cardNode = StringName.op_Implicit(nameof (_cardNode));
    public static readonly StringName _enchantmentSparkles = StringName.op_Implicit(nameof (_enchantmentSparkles));
    public static readonly StringName _enchantmentIcon = StringName.op_Implicit(nameof (_enchantmentIcon));
    public static readonly StringName _enchantmentLabel = StringName.op_Implicit(nameof (_enchantmentLabel));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
