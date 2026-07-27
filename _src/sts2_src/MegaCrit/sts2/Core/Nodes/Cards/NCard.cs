// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Cards.NCard
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Enchantments;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Helpers.Models;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Pooling;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Cards;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Cards;

[ScriptPath("res://src/Core/Nodes/Cards/NCard.cs")]
public class NCard : Control, IPoolable
{
  private const string _scenePath = "res://scenes/cards/card.tscn";
  private const string _portraitBlurMaterialPath = "res://scenes/cards/card_portrait_blur_material.tres";
  private const string _canvasGroupMaskMaterialPath = "res://scenes/cards/card_canvas_group_mask_material.tres";
  private const string _canvasGroupBlurMaterialPath = "res://scenes/cards/card_canvas_group_blur_material.tres";
  private const string _canvasGroupMaskBlurMaterialPath = "res://scenes/cards/card_canvas_group_mask_blur_material.tres";
  private const float _typePlaqueXMargin = 17f;
  private const float _typePlaqueMinXSize = 61f;
  private static readonly StringName _v = new StringName("v");
  private static readonly StringName _s = new StringName("s");
  private static readonly StringName _h = new StringName("h");
  public static readonly Vector2 defaultSize = new Vector2(300f, 422f);
  private CancellationTokenSource _cts = new CancellationTokenSource();
  private CardModel? _model;
  private MegaLabel _titleLabel;
  private MegaRichTextLabel _descriptionLabel;
  private TextureRect _ancientPortrait;
  private TextureRect _portrait;
  private TextureRect _frame;
  private TextureRect _ancientBorderGlassOverlay;
  private TextureRect _ancientBorder;
  private Control _ancientBanner;
  private TextureRect _ancientTextBg;
  private TextureRect _portraitBorder;
  private TextureRect _banner;
  private TextureRect _lock;
  private NinePatchRect _typePlaque;
  private MegaLabel _typeLabel;
  private CanvasGroup _portraitCanvasGroup;
  private NCardRareGlow? _rareGlow;
  private NCardUncommonGlow? _uncommonGlow;
  private GpuParticles2D _sparkles;
  private readonly List<NRelicFlashVfx> _flashVfx = new List<NRelicFlashVfx>();
  private TextureRect _energyIcon;
  private MegaLabel _energyLabel;
  private TextureRect _unplayableEnergyIcon;
  private TextureRect _starIcon;
  private MegaLabel _starLabel;
  private TextureRect _unplayableStarIcon;
  private Node _overlayContainer;
  private Control? _cardOverlay;
  private Node _cardVfxContainer;
  private Creature? _previewTarget;
  private Control _enchantmentTab;
  private TextureRect _enchantmentVfxOverride;
  private TextureRect _enchantmentIcon;
  private MegaLabel _enchantmentLabel;
  private Vector2 _defaultEnchantmentPosition;
  private const int _enchantmentTabStarLabelOffset = 45;
  private EnchantmentModel? _subscribedEnchantment;
  private bool _pretendCardCanBePlayed;
  private bool _forceUnpoweredPreview;
  private Material? _portraitBlurMaterial;
  private Material? _canvasGroupMaskBlurMaterial;
  private Material? _canvasGroupBlurMaterial;
  private Material? _canvasGroupMaskMaterial;
  private ModelVisibility _visibility = ModelVisibility.Visible;
  private readonly LocString _unknownDescription = new LocString("card_library", "UNKNOWN.description");
  private readonly LocString _unknownTitle = new LocString("card_library", "UNKNOWN.title");
  private readonly LocString _lockedDescription = new LocString("card_library", "LOCKED.description");
  private readonly LocString _lockedTitle = new LocString("card_library", "LOCKED.title");

  public NCardHighlight CardHighlight { get; private set; }

  public Control Body { get; private set; }

  public ModelVisibility Visibility
  {
    get => this._visibility;
    set
    {
      if (this._visibility == value)
        return;
      this._visibility = value;
      this.Reload();
    }
  }

  public Tween? PlayPileTween { get; set; }

  private Tween? RandomizeCostTween { get; set; }

  public PileType DisplayingPile { get; private set; }

  public Control EnchantmentTab => this._enchantmentTab;

  public TextureRect EnchantmentVfxOverride => this._enchantmentVfxOverride;

  public Node OverlayContainer => this._overlayContainer;

  public Node CardVfxContainer => this._cardVfxContainer;

  public event Action<CardModel?>? ModelChanged;

  public CardModel? Model
  {
    get => this._model;
    set
    {
      if (this._model == value)
        return;
      CardModel model = this._model;
      this.UnsubscribeFromModel(model);
      this._model = value;
      this.Reload();
      this.SubscribeToModel(this._model);
      Action<CardModel> modelChanged = this.ModelChanged;
      if (modelChanged != null)
        modelChanged(model);
      if (this._model == null || this._model.RunState == null && this._model.CombatState == null || !LocalContext.IsMine(this._model))
        return;
      SaveManager.Instance.MarkCardAsSeen(this._model);
    }
  }

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[5]
      {
        "res://scenes/cards/card.tscn",
        "res://scenes/cards/card_portrait_blur_material.tres",
        "res://scenes/cards/card_canvas_group_blur_material.tres",
        "res://scenes/cards/card_canvas_group_mask_blur_material.tres",
        "res://scenes/cards/card_canvas_group_mask_material.tres"
      });
    }
  }

  public void OnInstantiated()
  {
  }

  public override void _Ready()
  {
    this._titleLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%TitleLabel"));
    this._descriptionLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%DescriptionLabel"));
    this._frame = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Frame"));
    this._ancientBorderGlassOverlay = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%AncientBorderGlassOverlay"));
    this._ancientBorder = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%AncientBorder"));
    this._ancientTextBg = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%AncientTextBg"));
    this._ancientBanner = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%AncientBanner"));
    this._portrait = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Portrait"));
    this._ancientPortrait = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%AncientPortrait"));
    this._typeLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%TypeLabel"));
    this._portraitBorder = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%PortraitBorder"));
    this._portraitCanvasGroup = ((Node) this).GetNode<CanvasGroup>(NodePath.op_Implicit("%PortraitCanvasGroup"));
    this._energyLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%EnergyLabel"));
    this._energyIcon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%EnergyIcon"));
    this._starLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%StarLabel"));
    this._starIcon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%StarIcon"));
    this._banner = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%TitleBanner"));
    this._lock = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Lock"));
    this._typePlaque = ((Node) this).GetNode<NinePatchRect>(NodePath.op_Implicit("%TypePlaque"));
    this._unplayableEnergyIcon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%UnplayableEnergyIcon"));
    this._unplayableStarIcon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%UnplayableStarIcon"));
    this._enchantmentIcon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Enchantment/Icon"));
    this._enchantmentLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%Enchantment/Label"));
    this._sparkles = ((Node) this).GetNode<GpuParticles2D>(NodePath.op_Implicit("CardContainer/CardSparkles"));
    this._enchantmentTab = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Enchantment"));
    ((CanvasItem) this._enchantmentTab).Visible = false;
    this._enchantmentVfxOverride = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%EnchantmentVfxOverride"));
    this.CardHighlight = ((Node) this).GetNode<NCardHighlight>(NodePath.op_Implicit("%Highlight"));
    this.Body = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%CardContainer"));
    this._overlayContainer = ((Node) this).GetNode(NodePath.op_Implicit("%OverlayContainer"));
    this._cardVfxContainer = ((Node) this).GetNode(NodePath.op_Implicit("%CardVfxContainer"));
    this._defaultEnchantmentPosition = this._enchantmentTab.Position;
    this.Reload();
  }

  public override void _EnterTree()
  {
    this._cts = new CancellationTokenSource();
    this.SubscribeToModel(this.Model);
  }

  public override void _ExitTree()
  {
    this._cts.Cancel();
    this.RandomizeCostTween?.Kill();
    this.RandomizeCostTween = (Tween) null;
    this.UnsubscribeFromModel(this.Model);
  }

  private void SubscribeToModel(CardModel? model)
  {
    if (model == null || !((Node) this).IsInsideTree())
      return;
    model.AfflictionChanged += new Action(this.OnAfflictionChanged);
    model.EnchantmentChanged += new Action(this.OnEnchantmentChanged);
    this.SubscribeToEnchantment(model.Enchantment);
  }

  private void UnsubscribeFromModel(CardModel? model)
  {
    if (model == null)
      return;
    model.AfflictionChanged -= new Action(this.OnAfflictionChanged);
    model.EnchantmentChanged -= new Action(this.OnEnchantmentChanged);
    this.UnsubscribeFromEnchantment(model.Enchantment);
  }

  private void SubscribeToEnchantment(EnchantmentModel? model)
  {
    if (model == null || !((Node) this).IsInsideTree())
      return;
    this._subscribedEnchantment = this._subscribedEnchantment == null ? model : throw new InvalidOperationException($"Attempted to subscribe to enchantment {model}, but {this} is already subscribed to {this._subscribedEnchantment}!");
    this._subscribedEnchantment.StatusChanged += new Action(this.OnEnchantmentStatusChanged);
  }

  private void UnsubscribeFromEnchantment(EnchantmentModel? model)
  {
    if (model == null || model != this._subscribedEnchantment)
      return;
    this._subscribedEnchantment.StatusChanged -= new Action(this.OnEnchantmentStatusChanged);
    this._subscribedEnchantment = (EnchantmentModel) null;
  }

  public static void InitPool() => NodePool.Init<NCard>("res://scenes/cards/card.tscn", 30);

  public static NCard? Create(CardModel card, ModelVisibility visibility = ModelVisibility.Visible)
  {
    if (TestMode.IsOn)
      return (NCard) null;
    NCard ncard = NodePool.Get<NCard>();
    ncard.Model = card;
    ncard.Visibility = visibility;
    return ncard;
  }

  public static NCard? FindOnTable(CardModel card, PileType? overridePile = null)
  {
    if (TestMode.IsOn)
      return (NCard) null;
    if (!CombatManager.Instance.IsInProgress)
      return (NCard) null;
    NCombatUi ui = NCombatRoom.Instance?.Ui;
    if (ui == null)
      return (NCard) null;
    CardPile pile = card.Pile;
    PileType? nullable = pile != null ? new PileType?(pile.Type) : overridePile;
    if (!nullable.HasValue)
      return (NCard) null;
    switch (nullable.GetValueOrDefault())
    {
      case PileType.None:
        return (NCard) null;
      case PileType.Draw:
        return (NCard) null;
      case PileType.Hand:
        return ui.Hand.GetCard(card) ?? ui.PlayQueue.GetCardNode(card) ?? ui.GetCardFromPlayContainer(card);
      case PileType.Discard:
        return (NCard) null;
      case PileType.Exhaust:
        return (NCard) null;
      case PileType.Play:
        return ui.GetCardFromPlayContainer(card);
      case PileType.Deck:
        return (NCard) null;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  public Vector2 GetCurrentSize() => Vector2.op_Multiply(NCard.defaultSize, this.Scale);

  public void SetPreviewTarget(Creature? creature)
  {
    if (this._previewTarget == creature)
      return;
    this._previewTarget = creature;
    this.UpdateVisuals(this.DisplayingPile, CardPreviewMode.Normal);
  }

  public void UpdateVisuals(PileType pileType, CardPreviewMode previewMode)
  {
    if (!((Node) this).IsNodeReady())
      return;
    if (this.Model == null)
      throw new InvalidOperationException("Cannot update text with no model.");
    this.DisplayingPile = pileType;
    Creature target = this._previewTarget ?? this.Model.CurrentTarget;
    this.UpdatePortrait();
    this.UpdateTitleLabel();
    this.UpdateEnergyCostVisuals(pileType);
    this.UpdateStarCostVisuals(pileType);
    this.UpdateEnchantmentVisuals();
    this.Model.DynamicVars.ClearPreview();
    if (!this._forceUnpoweredPreview)
    {
      this.Model.UpdateDynamicVarPreview(previewMode, target, this.Model.DynamicVars);
      if (this.Model.Enchantment != null)
      {
        this.Model.Enchantment.DynamicVars.ClearPreview();
        this.Model.UpdateDynamicVarPreview(previewMode, target, this.Model.Enchantment.DynamicVars);
      }
    }
    string str = previewMode != CardPreviewMode.Upgrade ? this.Model.GetDescriptionForPile(pileType, target) : this.Model.GetDescriptionForUpgradePreview();
    switch (this.Visibility)
    {
      case ModelVisibility.Visible:
        this._descriptionLabel.SetTextAutoSize($"[center]{str}[/center]");
        break;
      case ModelVisibility.NotSeen:
        this._descriptionLabel.SetTextAutoSize($"[center][font_size=40]{this._unknownDescription.GetFormattedText()}[/font_size][/center]");
        break;
      case ModelVisibility.Locked:
        this._descriptionLabel.SetTextAutoSize($"[center][font_size=40]{this._lockedDescription.GetFormattedText()}[/font_size][/center]");
        break;
      default:
        throw new InvalidOperationException();
    }
  }

  public void ShowUpgradePreview()
  {
    this.UpdateVisuals(this.DisplayingPile, CardPreviewMode.Upgrade);
  }

  private void UpdateEnchantmentVisuals()
  {
    EnchantmentModel enchantmentModel = this.Model != null ? this.Model.Enchantment : throw new InvalidOperationException("Cannot show enchantment with no model.");
    if (enchantmentModel != null)
    {
      ((CanvasItem) this._enchantmentTab).Visible = true;
      this._enchantmentIcon.Texture = (Texture2D) enchantmentModel.Icon;
      this._enchantmentLabel.SetTextAutoSize(enchantmentModel.DisplayAmount.ToString());
      ((CanvasItem) this._enchantmentLabel).Visible = enchantmentModel.ShowAmount;
      this.SetEnchantmentStatus(enchantmentModel.Status);
    }
    else
      ((CanvasItem) this._enchantmentTab).Visible = false;
    if (this.Model.HasStarCostX || this.Model.CurrentStarCost >= 0)
      this._enchantmentTab.Position = this._defaultEnchantmentPosition;
    else
      this._enchantmentTab.Position = Vector2.op_Addition(this._defaultEnchantmentPosition, Vector2.op_Multiply(Vector2.Up, 45f));
  }

  private void OnEnchantmentStatusChanged()
  {
    this.SetEnchantmentStatus(this.Model?.Enchantment?.Status ?? EnchantmentStatus.Disabled);
  }

  private void SetEnchantmentStatus(EnchantmentStatus status)
  {
    if (status == EnchantmentStatus.Disabled)
    {
      ((CanvasItem) this._enchantmentTab).Modulate = new Color(1f, 1f, 1f, 0.9f);
      ShaderMaterial material = (ShaderMaterial) ((CanvasItem) this._enchantmentTab).Material;
      material.SetShaderParameter(NCard._h, Variant.op_Implicit(0.25));
      material.SetShaderParameter(NCard._s, Variant.op_Implicit(0.1));
      material.SetShaderParameter(NCard._v, Variant.op_Implicit(0.6));
      ((CanvasItem) this._enchantmentIcon).UseParentMaterial = true;
      ((CanvasItem) this._enchantmentLabel).SelfModulate = StsColors.gray;
    }
    else
    {
      ((CanvasItem) this._enchantmentTab).Modulate = Colors.White;
      ShaderMaterial material = (ShaderMaterial) ((CanvasItem) this._enchantmentTab).Material;
      material.SetShaderParameter(NCard._h, Variant.op_Implicit(0.25));
      material.SetShaderParameter(NCard._s, Variant.op_Implicit(0.4));
      material.SetShaderParameter(NCard._v, Variant.op_Implicit(0.6));
      ((CanvasItem) this._enchantmentIcon).UseParentMaterial = false;
      ((CanvasItem) this._enchantmentLabel).SelfModulate = Colors.White;
    }
  }

  private void UpdateEnergyCostVisuals(PileType pileType)
  {
    if (this.Visibility != ModelVisibility.Visible)
    {
      this._energyLabel.SetTextAutoSize("?");
      ((CanvasItem) this._energyIcon).Visible = true;
      ((Control) this._energyLabel).AddThemeColorOverride(ThemeConstants.Label.FontColor, StsColors.cream);
      ((Control) this._energyLabel).AddThemeColorOverride(ThemeConstants.Label.FontOutlineColor, this.Model.Pool.EnergyOutlineColor);
    }
    else
    {
      if (this.Model.EnergyCost.CostsX)
      {
        this._energyLabel.SetTextAutoSize("X");
        ((CanvasItem) this._energyIcon).Visible = true;
      }
      else
      {
        int withModifiers = this.Model.EnergyCost.GetWithModifiers(CostModifiers.All);
        this._energyLabel.SetTextAutoSize(withModifiers.ToString());
        ((CanvasItem) this._energyIcon).Visible = withModifiers >= 0;
      }
      this.UpdateEnergyCostColor(pileType);
      UnplayableReason reason;
      if (pileType == PileType.Hand && !this.Model.CanPlay(out reason, out AbstractModel _))
        ((CanvasItem) this._unplayableEnergyIcon).Visible = !reason.HasResourceCostReason();
      else
        ((CanvasItem) this._unplayableEnergyIcon).Visible = false;
    }
  }

  public void SetPretendCardCanBePlayed(bool pretendCardCanBePlayed)
  {
    this._pretendCardCanBePlayed = pretendCardCanBePlayed;
    this.UpdateEnergyCostVisuals(this.DisplayingPile);
    this.UpdateStarCostVisuals(this.DisplayingPile);
  }

  public void SetForceUnpoweredPreview(bool forceUnpoweredPreview)
  {
    this._forceUnpoweredPreview = forceUnpoweredPreview;
  }

  private void UpdateEnergyCostColor(PileType pileType)
  {
    Color defaultColor1 = StsColors.cream;
    Color defaultColor2 = this.Model.Pool.EnergyOutlineColor;
    CardEnergyCost energyCost = this.Model.EnergyCost;
    if (energyCost != null && !energyCost.CostsX && energyCost.WasJustUpgraded)
    {
      defaultColor1 = StsColors.green;
      defaultColor2 = StsColors.energyGreenOutline;
    }
    else if (pileType == PileType.Hand)
    {
      CardCostColor energyCostColor = CardCostHelper.GetEnergyCostColor(this.Model, this.Model.CombatState);
      defaultColor1 = NCard.GetCostTextColorInHand(energyCostColor, this._pretendCardCanBePlayed, defaultColor1);
      defaultColor2 = NCard.GetCostOutlineColorInHand(energyCostColor, this._pretendCardCanBePlayed, defaultColor2);
    }
    ((Control) this._energyLabel).AddThemeColorOverride(ThemeConstants.Label.FontColor, defaultColor1);
    ((Control) this._energyLabel).AddThemeColorOverride(ThemeConstants.Label.FontOutlineColor, defaultColor2);
  }

  private void UpdateStarCostVisuals(PileType pileType)
  {
    if (this.Visibility != ModelVisibility.Visible)
    {
      this._starLabel.SetTextAutoSize(string.Empty);
      ((CanvasItem) this._starIcon).Visible = false;
      ((Control) this._starLabel).AddThemeColorOverride(ThemeConstants.Label.FontColor, StsColors.cream);
      ((Control) this._starLabel).AddThemeColorOverride(ThemeConstants.Label.FontOutlineColor, this.Model.Pool.EnergyOutlineColor);
    }
    else
    {
      if (this.Model.HasStarCostX)
      {
        this._starLabel.SetTextAutoSize("X");
        ((CanvasItem) this._starIcon).Visible = true;
      }
      else
      {
        this._starLabel.SetTextAutoSize(this.Model.GetStarCostWithModifiers().ToString());
        ((CanvasItem) this._starIcon).Visible = this.Model.GetStarCostWithModifiers() >= 0;
      }
      this.UpdateStarCostColor(pileType);
      UnplayableReason reason;
      if (pileType == PileType.Hand && !this.Model.CanPlay(out reason, out AbstractModel _))
        ((CanvasItem) this._unplayableStarIcon).Visible = !reason.HasResourceCostReason();
      else
        ((CanvasItem) this._unplayableStarIcon).Visible = false;
    }
  }

  private void UpdateStarCostText(int cost)
  {
    if (this.Model.HasStarCostX)
    {
      this._starLabel.SetTextAutoSize("X");
      ((CanvasItem) this._starIcon).Visible = true;
    }
    else if (cost >= 0)
    {
      this._starLabel.SetTextAutoSize(cost.ToString());
      ((CanvasItem) this._starIcon).Visible = true;
    }
    else
      ((CanvasItem) this._starIcon).Visible = false;
  }

  private void UpdateStarCostColor(PileType pileType)
  {
    Color defaultColor1 = StsColors.cream;
    Color defaultColor2 = StsColors.defaultStarCostOutline;
    if (!this.Model.HasStarCostX && this.Model.WasStarCostJustUpgraded)
    {
      defaultColor1 = StsColors.green;
      defaultColor2 = StsColors.energyGreenOutline;
    }
    else if (pileType == PileType.Hand)
    {
      CardCostColor starCostColor = CardCostHelper.GetStarCostColor(this.Model, this.Model.CombatState);
      defaultColor1 = NCard.GetCostTextColorInHand(starCostColor, this._pretendCardCanBePlayed, defaultColor1);
      defaultColor2 = NCard.GetCostOutlineColorInHand(starCostColor, this._pretendCardCanBePlayed, defaultColor2);
    }
    ((Control) this._starLabel).AddThemeColorOverride(ThemeConstants.Label.FontColor, defaultColor1);
    ((Control) this._starLabel).AddThemeColorOverride(ThemeConstants.Label.FontOutlineColor, defaultColor2);
  }

  private static Color GetCostTextColorInHand(
    CardCostColor costColor,
    bool pretendCardCanBePlayed,
    Color defaultColor)
  {
    switch (costColor)
    {
      case CardCostColor.Unmodified:
        return defaultColor;
      case CardCostColor.Increased:
        return StsColors.energyBlue;
      case CardCostColor.Decreased:
        return StsColors.green;
      case CardCostColor.InsufficientResources:
        return pretendCardCanBePlayed ? defaultColor : StsColors.red;
      default:
        throw new ArgumentOutOfRangeException(nameof (costColor), (object) costColor, (string) null);
    }
  }

  private static Color GetCostOutlineColorInHand(
    CardCostColor costColor,
    bool pretendCardCanBePlayed,
    Color defaultColor)
  {
    switch (costColor)
    {
      case CardCostColor.Unmodified:
        return defaultColor;
      case CardCostColor.Increased:
        return StsColors.energyBlueOutline;
      case CardCostColor.Decreased:
        return StsColors.energyGreenOutline;
      case CardCostColor.InsufficientResources:
        return pretendCardCanBePlayed ? defaultColor : StsColors.unplayableEnergyCostOutline;
      default:
        throw new ArgumentOutOfRangeException(nameof (costColor), (object) costColor, (string) null);
    }
  }

  public void PlayRandomizeCostAnim()
  {
    this.RandomizeCostTween?.Kill();
    this.RandomizeCostTween = ((Node) this).CreateTween();
    float offset = Rng.Chaotic.NextFloat(10f);
    this.RandomizeCostTween.TweenMethod(Callable.From<float>((Action<float>) (t =>
    {
      if ((int) ((double) offset + (double) t) % 8 > 3)
        this._energyLabel.SetTextAutoSize("?");
      else
        this._energyLabel.SetTextAutoSize((t % 4f).ToString());
    })), Variant.op_Implicit(0), Variant.op_Implicit(50), (double) Rng.Chaotic.NextFloat(0.4f, 0.6f)).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 1L);
    ((GodotObject) this.RandomizeCostTween).Connect(Tween.SignalName.Finished, Callable.From((Action) (() =>
    {
      if (this.Model == null)
        return;
      this.UpdateEnergyCostVisuals(this.DisplayingPile);
    })), 4U);
  }

  private void Reload()
  {
    if (!((Node) this).IsNodeReady() || this.Model == null)
      return;
    if (OS.HasFeature("editor"))
      ((Node) this).Name = StringName.op_Implicit($"{typeof (NCard)}-{this.Model.Id}");
    this._energyIcon.Texture = this.Model.EnergyIcon;
    this.UpdateTypePlaque();
    bool flag = this.Model.Rarity == CardRarity.Ancient;
    ((CanvasItem) this._portraitBorder).Visible = !flag;
    ((CanvasItem) this._portrait).Visible = !flag;
    ((CanvasItem) this._frame).Visible = !flag;
    ((CanvasItem) this._ancientPortrait).Visible = flag;
    ((CanvasItem) this._ancientBorderGlassOverlay).Visible = flag;
    ((CanvasItem) this._ancientBorder).Visible = flag;
    ((CanvasItem) this._ancientTextBg).Visible = flag;
    ((CanvasItem) this._ancientBanner).Visible = flag;
    ((CanvasItem) this._banner).Visible = !flag;
    ((CanvasItem) this._lock).Visible = this.Visibility == ModelVisibility.Locked;
    if (this.Visibility != ModelVisibility.Visible)
    {
      if (this._portraitBlurMaterial == null)
        this._portraitBlurMaterial = PreloadManager.Cache.GetMaterial("res://scenes/cards/card_portrait_blur_material.tres");
      if (flag)
      {
        if (this._canvasGroupMaskBlurMaterial == null)
          this._canvasGroupMaskBlurMaterial = PreloadManager.Cache.GetMaterial("res://scenes/cards/card_canvas_group_mask_blur_material.tres");
        ((CanvasItem) this._portraitCanvasGroup).Material = this._canvasGroupMaskBlurMaterial;
      }
      else
      {
        if (this._canvasGroupBlurMaterial == null)
          this._canvasGroupBlurMaterial = PreloadManager.Cache.GetMaterial("res://scenes/cards/card_canvas_group_blur_material.tres");
        ((CanvasItem) this._portraitCanvasGroup).Material = this._canvasGroupBlurMaterial;
      }
      ((CanvasItem) this._portrait).Material = this._portraitBlurMaterial;
      ((CanvasItem) this._ancientPortrait).Material = this._portraitBlurMaterial;
    }
    else
    {
      if (flag)
      {
        if (this._canvasGroupMaskMaterial == null)
          this._canvasGroupMaskMaterial = PreloadManager.Cache.GetMaterial("res://scenes/cards/card_canvas_group_mask_material.tres");
        ((CanvasItem) this._portraitCanvasGroup).Material = this._canvasGroupMaskMaterial;
      }
      else
        ((CanvasItem) this._portraitCanvasGroup).Material = (Material) null;
      ((CanvasItem) this._portrait).Material = (Material) null;
      ((CanvasItem) this._ancientPortrait).Material = (Material) null;
    }
    this.UpdatePortrait();
    ((CanvasItem) this._frame).Material = this.Model.FrameMaterial;
    this.ReloadOverlay();
  }

  private void UpdatePortrait()
  {
    if (this.Model == null)
      return;
    Texture2D portrait = this.Model.Portrait;
    if (this.Model.Rarity != CardRarity.Ancient)
    {
      this._portrait.Texture = portrait;
      this._portraitBorder.Texture = this.Model.PortraitBorder;
      ((CanvasItem) this._portraitBorder).Material = this.Model.BannerMaterial;
      this._frame.Texture = this.Model.Frame;
      ((CanvasItem) this._banner).Material = this.Model.BannerMaterial;
      this._banner.Texture = this.Model.BannerTexture;
    }
    else
    {
      this._ancientBorder.Texture = this.Model.AncientBorder;
      this._ancientTextBg.Texture = this.Model.AncientTextBg;
      this._ancientPortrait.Texture = portrait;
      ((CanvasItem) this._banner).Material = (Material) null;
    }
  }

  private void UpdateTypePlaque()
  {
    this._typeLabel.SetTextAutoSize(this.Model.Type.ToLocString().GetFormattedText());
    if (((CanvasItem) this._typePlaque).Material != this.Model.BannerMaterial)
      ((CanvasItem) this._typePlaque).Material = this.Model.BannerMaterial;
    Callable callable = Callable.From(new Action(this.UpdateTypePlaqueSizeAndPosition));
    ((Callable) ref callable).CallDeferred(Array.Empty<Variant>());
  }

  private void UpdateTypePlaqueSizeAndPosition()
  {
    float num = ((Control) this._typePlaque).Position.X + ((Control) this._typePlaque).Size.X * 0.5f;
    NinePatchRect typePlaque1 = this._typePlaque;
    Vector2 size = ((Control) this._typePlaque).Size;
    size.X = Mathf.Max(((Control) this._typeLabel).Size.X + 17f, 61f);
    Vector2 vector2_1 = size;
    ((Control) typePlaque1).Size = vector2_1;
    NinePatchRect typePlaque2 = this._typePlaque;
    Vector2 position = ((Control) this._typePlaque).Position;
    position.X = num - ((Control) this._typePlaque).Size.X * 0.5f;
    Vector2 vector2_2 = position;
    ((Control) typePlaque2).Position = vector2_2;
  }

  private void UpdateTitleLabel()
  {
    string text;
    Color color1;
    Color color2;
    if (this.Visibility == ModelVisibility.NotSeen)
    {
      text = this._unknownTitle.GetFormattedText();
      color1 = StsColors.cream;
      color2 = this.GetTitleLabelOutlineColor();
    }
    else if (this.Visibility == ModelVisibility.Locked)
    {
      text = this._lockedTitle.GetFormattedText();
      color1 = StsColors.cream;
      color2 = this.GetTitleLabelOutlineColor();
    }
    else if (this.Model.CurrentUpgradeLevel == 0)
    {
      text = this.Model.Title;
      color1 = StsColors.cream;
      color2 = this.GetTitleLabelOutlineColor();
    }
    else
    {
      text = this.Model.Title;
      color1 = StsColors.green;
      color2 = StsColors.cardTitleOutlineSpecial;
    }
    this._titleLabel.SetTextAutoSize(text);
    ((Control) this._titleLabel).AddThemeColorOverride(ThemeConstants.Label.FontColor, color1);
    ((Control) this._titleLabel).AddThemeColorOverride(ThemeConstants.Label.FontOutlineColor, color2);
  }

  private Color GetTitleLabelOutlineColor()
  {
    switch (this._model.Rarity)
    {
      case CardRarity.None:
      case CardRarity.Basic:
      case CardRarity.Common:
      case CardRarity.Token:
        return StsColors.cardTitleOutlineCommon;
      case CardRarity.Uncommon:
        return StsColors.cardTitleOutlineUncommon;
      case CardRarity.Rare:
        return StsColors.cardTitleOutlineRare;
      case CardRarity.Ancient:
        return StsColors.cardTitleOutlineCommon;
      case CardRarity.Event:
        return StsColors.cardTitleOutlineSpecial;
      case CardRarity.Status:
        return StsColors.cardTitleOutlineStatus;
      case CardRarity.Curse:
        return StsColors.cardTitleOutlineCurse;
      case CardRarity.Quest:
        return StsColors.cardTitleOutlineQuest;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  private void ReloadOverlay()
  {
    if (this._cardOverlay != null)
    {
      this.OverlayContainer.RemoveChildSafely((Node) this._cardOverlay);
      ((Node) this._cardOverlay).QueueFreeSafely();
      this._cardOverlay = (Control) null;
    }
    if (this.Model == null)
      return;
    if (this.Model.Rarity == CardRarity.Ancient)
    {
      ((CanvasItem) this._frame).Visible = false;
      ((CanvasItem) this._ancientBorder).Visible = true;
    }
    AfflictionModel affliction = this.Model.Affliction;
    if (affliction != null && affliction.HasOverlay)
      this._cardOverlay = this.Model.Affliction.CreateOverlay();
    else if (this.Model.HasBuiltInOverlay)
      this._cardOverlay = this.Model.CreateOverlay();
    if (this._cardOverlay == null)
      return;
    this.OverlayContainer.AddChildSafely((Node) this._cardOverlay);
  }

  private void OnAfflictionChanged() => this.ReloadOverlay();

  private void OnEnchantmentChanged()
  {
    this.UnsubscribeFromEnchantment(this._subscribedEnchantment);
    this.SubscribeToEnchantment(this.Model?.Enchantment);
    this.UpdateEnchantmentVisuals();
  }

  private string GetTitleText() => this._titleLabel.Text;

  public void ActivateRewardScreenGlow()
  {
    if (this._model.Rarity == CardRarity.Rare)
    {
      ((CanvasItem) this._sparkles).Visible = true;
      this._rareGlow = NCardRareGlow.Create();
      if (this._rareGlow != null)
      {
        ((Node) this.Body).AddChildSafely((Node) this._rareGlow);
        ((Node) this.Body).MoveChildSafely((Node) this._rareGlow, 1);
      }
      ((CanvasItem) this.CardHighlight).Modulate = NCardHighlight.gold;
    }
    else
    {
      if (this._model.Rarity != CardRarity.Uncommon)
        return;
      this._uncommonGlow = NCardUncommonGlow.Create();
      if (this._uncommonGlow != null)
      {
        ((Node) this.Body).AddChildSafely((Node) this._uncommonGlow);
        ((Node) this.Body).MoveChildSafely((Node) this._uncommonGlow, 1);
      }
      ((CanvasItem) this.CardHighlight).Modulate = NCardHighlight.playableColor;
    }
  }

  public void FlashRelicOnCard(RelicModel relic)
  {
    NRelicFlashVfx child = NRelicFlashVfx.Create(relic);
    ((Node) this.Body).AddChildSafely((Node) child);
    child.Scale = Vector2.op_Multiply(Vector2.One, 2f);
    child.Position = Vector2.op_Multiply(this.Size, 0.5f);
    this._flashVfx.Add(child);
  }

  public void KillRarityGlow()
  {
    this._rareGlow?.Kill();
    this._uncommonGlow?.Kill();
  }

  public async Task AnimMultiCardPlay()
  {
    if (!GodotObject.IsInstanceValid((GodotObject) this))
      return;
    Vector2 scale = this.Scale;
    float y = this.Position.Y;
    Tween playPileTween = this.PlayPileTween;
    if (playPileTween != null)
      playPileTween.FastForwardToCompletion();
    this.PlayPileTween = ((Node) this).CreateTween().SetParallel(true);
    this.PlayPileTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate"), Variant.op_Implicit(StsColors.transparentBlack), 0.2);
    this.PlayPileTween.Chain();
    this.PlayPileTween.TweenInterval(SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast ? 0.1 : 0.2);
    this.PlayPileTween.TweenCallback(Callable.From((Action) (() => this.UpdateVisuals(PileType.Play, CardPreviewMode.Normal))));
    this.PlayPileTween.Chain();
    this.PlayPileTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.2).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    this.PlayPileTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(scale), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L).From(Variant.op_Implicit(Vector2.op_Multiply(scale, 0.5f)));
    this.PlayPileTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position:y"), Variant.op_Implicit(y), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L).From(Variant.op_Implicit(y + 250f));
    await Cmd.CustomScaledWait(0.4f, 0.5f, cancellationToken: this._cts.Token);
  }

  public void AnimCardToPlayPile()
  {
    Vector2 targetPosition = PileType.Play.GetTargetPosition(this);
    Tween playPileTween = this.PlayPileTween;
    if (playPileTween != null)
      playPileTween.FastForwardToCompletion();
    this.PlayPileTween = ((Node) this).CreateTween().SetParallel(true);
    this.PlayPileTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position"), Variant.op_Implicit(targetPosition), 0.2).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    this.PlayPileTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 0.8f)), 0.2).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
  }

  public void OnReturnedFromPool()
  {
    if (!((Node) this).IsNodeReady())
      return;
    this.Position = Vector2.Zero;
    this.Rotation = 0.0f;
    this.Scale = Vector2.One;
    ((CanvasItem) this).Modulate = Colors.White;
    ((CanvasItem) this).Visible = true;
    ((CanvasItem) this.Body).Visible = true;
    ((CanvasItem) this.Body).Modulate = Colors.White;
    this.Body.Scale = Vector2.One;
    this._visibility = ModelVisibility.Visible;
    ((CanvasItem) this.CardHighlight).Modulate = NCardHighlight.playableColor;
    this.CardHighlight.AnimHideInstantly();
    ((CanvasItem) this._sparkles).Visible = false;
    ((CanvasItem) this._enchantmentTab).Visible = false;
    ((CanvasItem) this._enchantmentVfxOverride).Visible = false;
    this._model = (CardModel) null;
    this._previewTarget = (Creature) null;
    this._pretendCardCanBePlayed = false;
    this._forceUnpoweredPreview = false;
    ((CanvasItem) this._portrait).Material = (Material) null;
    ((CanvasItem) this._ancientPortrait).Material = (Material) null;
    ((CanvasItem) this._portraitCanvasGroup).Material = (Material) null;
    this.DisplayingPile = PileType.None;
    this.ModelChanged = (Action<CardModel>) null;
  }

  public void OnFreedToPool()
  {
    NCardRareGlow rareGlow = this._rareGlow;
    if (rareGlow != null)
      ((Node) rareGlow).QueueFreeSafely();
    this._rareGlow = (NCardRareGlow) null;
    NCardUncommonGlow uncommonGlow = this._uncommonGlow;
    if (uncommonGlow != null)
      ((Node) uncommonGlow).QueueFreeSafely();
    this._uncommonGlow = (NCardUncommonGlow) null;
    Control cardOverlay = this._cardOverlay;
    if (cardOverlay != null)
      ((Node) cardOverlay).QueueFreeSafely();
    this._cardOverlay = (Control) null;
    this._portrait.Texture = (Texture2D) null;
    this._enchantmentVfxOverride.Texture = (Texture2D) null;
    foreach (NRelicFlashVfx nrelicFlashVfx in this._flashVfx)
    {
      if (((Node) nrelicFlashVfx).IsValid())
        ((Node) nrelicFlashVfx).QueueFreeSafely();
    }
    this._flashVfx.Clear();
    this.PlayPileTween?.Kill();
    this.PlayPileTween = (Tween) null;
    this.RandomizeCostTween?.Kill();
    this.RandomizeCostTween = (Tween) null;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(36)
    {
      new MethodInfo(NCard.MethodName.OnInstantiated, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCard.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCard.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCard.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCard.MethodName.InitPool, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCard.MethodName.GetCurrentSize, new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCard.MethodName.UpdateVisuals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("pileType"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("previewMode"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCard.MethodName.ShowUpgradePreview, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCard.MethodName.UpdateEnchantmentVisuals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCard.MethodName.OnEnchantmentStatusChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCard.MethodName.SetEnchantmentStatus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("status"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCard.MethodName.UpdateEnergyCostVisuals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("pileType"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCard.MethodName.SetPretendCardCanBePlayed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("pretendCardCanBePlayed"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCard.MethodName.SetForceUnpoweredPreview, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("forceUnpoweredPreview"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCard.MethodName.UpdateEnergyCostColor, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("pileType"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCard.MethodName.UpdateStarCostVisuals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("pileType"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCard.MethodName.UpdateStarCostText, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("cost"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCard.MethodName.UpdateStarCostColor, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("pileType"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCard.MethodName.GetCostTextColorInHand, new PropertyInfo((Variant.Type) 20L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("costColor"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("pretendCardCanBePlayed"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 20L, StringName.op_Implicit("defaultColor"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCard.MethodName.GetCostOutlineColorInHand, new PropertyInfo((Variant.Type) 20L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("costColor"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("pretendCardCanBePlayed"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 20L, StringName.op_Implicit("defaultColor"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCard.MethodName.PlayRandomizeCostAnim, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCard.MethodName.Reload, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCard.MethodName.UpdatePortrait, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCard.MethodName.UpdateTypePlaque, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCard.MethodName.UpdateTypePlaqueSizeAndPosition, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCard.MethodName.UpdateTitleLabel, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCard.MethodName.GetTitleLabelOutlineColor, new PropertyInfo((Variant.Type) 20L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCard.MethodName.ReloadOverlay, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCard.MethodName.OnAfflictionChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCard.MethodName.OnEnchantmentChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCard.MethodName.GetTitleText, new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCard.MethodName.ActivateRewardScreenGlow, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCard.MethodName.KillRarityGlow, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCard.MethodName.AnimCardToPlayPile, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCard.MethodName.OnReturnedFromPool, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCard.MethodName.OnFreedToPool, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCard.MethodName.OnInstantiated) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnInstantiated();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCard.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCard.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCard.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCard.MethodName.InitPool) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NCard.InitPool();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCard.MethodName.GetCurrentSize) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      Vector2 currentSize = this.GetCurrentSize();
      ret = VariantUtils.CreateFrom<Vector2>(ref currentSize);
      return true;
    }
    if (StringName.op_Equality(ref method, NCard.MethodName.UpdateVisuals) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.UpdateVisuals(VariantUtils.ConvertTo<PileType>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<CardPreviewMode>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCard.MethodName.ShowUpgradePreview) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ShowUpgradePreview();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCard.MethodName.UpdateEnchantmentVisuals) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateEnchantmentVisuals();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCard.MethodName.OnEnchantmentStatusChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnEnchantmentStatusChanged();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCard.MethodName.SetEnchantmentStatus) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetEnchantmentStatus(VariantUtils.ConvertTo<EnchantmentStatus>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCard.MethodName.UpdateEnergyCostVisuals) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateEnergyCostVisuals(VariantUtils.ConvertTo<PileType>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCard.MethodName.SetPretendCardCanBePlayed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetPretendCardCanBePlayed(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCard.MethodName.SetForceUnpoweredPreview) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetForceUnpoweredPreview(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCard.MethodName.UpdateEnergyCostColor) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateEnergyCostColor(VariantUtils.ConvertTo<PileType>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCard.MethodName.UpdateStarCostVisuals) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateStarCostVisuals(VariantUtils.ConvertTo<PileType>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCard.MethodName.UpdateStarCostText) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateStarCostText(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCard.MethodName.UpdateStarCostColor) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateStarCostColor(VariantUtils.ConvertTo<PileType>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCard.MethodName.GetCostTextColorInHand) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      Color costTextColorInHand = NCard.GetCostTextColorInHand(VariantUtils.ConvertTo<CardCostColor>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<Color>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = VariantUtils.CreateFrom<Color>(ref costTextColorInHand);
      return true;
    }
    if (StringName.op_Equality(ref method, NCard.MethodName.GetCostOutlineColorInHand) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      Color outlineColorInHand = NCard.GetCostOutlineColorInHand(VariantUtils.ConvertTo<CardCostColor>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<Color>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = VariantUtils.CreateFrom<Color>(ref outlineColorInHand);
      return true;
    }
    if (StringName.op_Equality(ref method, NCard.MethodName.PlayRandomizeCostAnim) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.PlayRandomizeCostAnim();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCard.MethodName.Reload) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Reload();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCard.MethodName.UpdatePortrait) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdatePortrait();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCard.MethodName.UpdateTypePlaque) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateTypePlaque();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCard.MethodName.UpdateTypePlaqueSizeAndPosition) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateTypePlaqueSizeAndPosition();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCard.MethodName.UpdateTitleLabel) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateTitleLabel();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCard.MethodName.GetTitleLabelOutlineColor) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      Color labelOutlineColor = this.GetTitleLabelOutlineColor();
      ret = VariantUtils.CreateFrom<Color>(ref labelOutlineColor);
      return true;
    }
    if (StringName.op_Equality(ref method, NCard.MethodName.ReloadOverlay) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ReloadOverlay();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCard.MethodName.OnAfflictionChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnAfflictionChanged();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCard.MethodName.OnEnchantmentChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnEnchantmentChanged();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCard.MethodName.GetTitleText) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      string titleText = this.GetTitleText();
      ret = VariantUtils.CreateFrom<string>(ref titleText);
      return true;
    }
    if (StringName.op_Equality(ref method, NCard.MethodName.ActivateRewardScreenGlow) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ActivateRewardScreenGlow();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCard.MethodName.KillRarityGlow) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.KillRarityGlow();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCard.MethodName.AnimCardToPlayPile) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimCardToPlayPile();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCard.MethodName.OnReturnedFromPool) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnReturnedFromPool();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCard.MethodName.OnFreedToPool) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnFreedToPool();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCard.MethodName.InitPool) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NCard.InitPool();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCard.MethodName.GetCostTextColorInHand) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      Color costTextColorInHand = NCard.GetCostTextColorInHand(VariantUtils.ConvertTo<CardCostColor>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<Color>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = VariantUtils.CreateFrom<Color>(ref costTextColorInHand);
      return true;
    }
    if (StringName.op_Equality(ref method, NCard.MethodName.GetCostOutlineColorInHand) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      Color outlineColorInHand = NCard.GetCostOutlineColorInHand(VariantUtils.ConvertTo<CardCostColor>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<Color>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = VariantUtils.CreateFrom<Color>(ref outlineColorInHand);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCard.MethodName.OnInstantiated) || StringName.op_Equality(ref method, NCard.MethodName._Ready) || StringName.op_Equality(ref method, NCard.MethodName._EnterTree) || StringName.op_Equality(ref method, NCard.MethodName._ExitTree) || StringName.op_Equality(ref method, NCard.MethodName.InitPool) || StringName.op_Equality(ref method, NCard.MethodName.GetCurrentSize) || StringName.op_Equality(ref method, NCard.MethodName.UpdateVisuals) || StringName.op_Equality(ref method, NCard.MethodName.ShowUpgradePreview) || StringName.op_Equality(ref method, NCard.MethodName.UpdateEnchantmentVisuals) || StringName.op_Equality(ref method, NCard.MethodName.OnEnchantmentStatusChanged) || StringName.op_Equality(ref method, NCard.MethodName.SetEnchantmentStatus) || StringName.op_Equality(ref method, NCard.MethodName.UpdateEnergyCostVisuals) || StringName.op_Equality(ref method, NCard.MethodName.SetPretendCardCanBePlayed) || StringName.op_Equality(ref method, NCard.MethodName.SetForceUnpoweredPreview) || StringName.op_Equality(ref method, NCard.MethodName.UpdateEnergyCostColor) || StringName.op_Equality(ref method, NCard.MethodName.UpdateStarCostVisuals) || StringName.op_Equality(ref method, NCard.MethodName.UpdateStarCostText) || StringName.op_Equality(ref method, NCard.MethodName.UpdateStarCostColor) || StringName.op_Equality(ref method, NCard.MethodName.GetCostTextColorInHand) || StringName.op_Equality(ref method, NCard.MethodName.GetCostOutlineColorInHand) || StringName.op_Equality(ref method, NCard.MethodName.PlayRandomizeCostAnim) || StringName.op_Equality(ref method, NCard.MethodName.Reload) || StringName.op_Equality(ref method, NCard.MethodName.UpdatePortrait) || StringName.op_Equality(ref method, NCard.MethodName.UpdateTypePlaque) || StringName.op_Equality(ref method, NCard.MethodName.UpdateTypePlaqueSizeAndPosition) || StringName.op_Equality(ref method, NCard.MethodName.UpdateTitleLabel) || StringName.op_Equality(ref method, NCard.MethodName.GetTitleLabelOutlineColor) || StringName.op_Equality(ref method, NCard.MethodName.ReloadOverlay) || StringName.op_Equality(ref method, NCard.MethodName.OnAfflictionChanged) || StringName.op_Equality(ref method, NCard.MethodName.OnEnchantmentChanged) || StringName.op_Equality(ref method, NCard.MethodName.GetTitleText) || StringName.op_Equality(ref method, NCard.MethodName.ActivateRewardScreenGlow) || StringName.op_Equality(ref method, NCard.MethodName.KillRarityGlow) || StringName.op_Equality(ref method, NCard.MethodName.AnimCardToPlayPile) || StringName.op_Equality(ref method, NCard.MethodName.OnReturnedFromPool) || StringName.op_Equality(ref method, NCard.MethodName.OnFreedToPool) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCard.PropertyName.CardHighlight))
    {
      this.CardHighlight = VariantUtils.ConvertTo<NCardHighlight>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName.Body))
    {
      this.Body = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName.Visibility))
    {
      this.Visibility = VariantUtils.ConvertTo<ModelVisibility>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName.PlayPileTween))
    {
      this.PlayPileTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName.RandomizeCostTween))
    {
      this.RandomizeCostTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName.DisplayingPile))
    {
      this.DisplayingPile = VariantUtils.ConvertTo<PileType>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._titleLabel))
    {
      this._titleLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._descriptionLabel))
    {
      this._descriptionLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._ancientPortrait))
    {
      this._ancientPortrait = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._portrait))
    {
      this._portrait = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._frame))
    {
      this._frame = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._ancientBorderGlassOverlay))
    {
      this._ancientBorderGlassOverlay = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._ancientBorder))
    {
      this._ancientBorder = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._ancientBanner))
    {
      this._ancientBanner = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._ancientTextBg))
    {
      this._ancientTextBg = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._portraitBorder))
    {
      this._portraitBorder = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._banner))
    {
      this._banner = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._lock))
    {
      this._lock = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._typePlaque))
    {
      this._typePlaque = VariantUtils.ConvertTo<NinePatchRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._typeLabel))
    {
      this._typeLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._portraitCanvasGroup))
    {
      this._portraitCanvasGroup = VariantUtils.ConvertTo<CanvasGroup>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._rareGlow))
    {
      this._rareGlow = VariantUtils.ConvertTo<NCardRareGlow>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._uncommonGlow))
    {
      this._uncommonGlow = VariantUtils.ConvertTo<NCardUncommonGlow>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._sparkles))
    {
      this._sparkles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._energyIcon))
    {
      this._energyIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._energyLabel))
    {
      this._energyLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._unplayableEnergyIcon))
    {
      this._unplayableEnergyIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._starIcon))
    {
      this._starIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._starLabel))
    {
      this._starLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._unplayableStarIcon))
    {
      this._unplayableStarIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._overlayContainer))
    {
      this._overlayContainer = VariantUtils.ConvertTo<Node>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._cardOverlay))
    {
      this._cardOverlay = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._cardVfxContainer))
    {
      this._cardVfxContainer = VariantUtils.ConvertTo<Node>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._enchantmentTab))
    {
      this._enchantmentTab = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._enchantmentVfxOverride))
    {
      this._enchantmentVfxOverride = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._enchantmentIcon))
    {
      this._enchantmentIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._enchantmentLabel))
    {
      this._enchantmentLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._defaultEnchantmentPosition))
    {
      this._defaultEnchantmentPosition = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._pretendCardCanBePlayed))
    {
      this._pretendCardCanBePlayed = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._forceUnpoweredPreview))
    {
      this._forceUnpoweredPreview = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._portraitBlurMaterial))
    {
      this._portraitBlurMaterial = VariantUtils.ConvertTo<Material>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._canvasGroupMaskBlurMaterial))
    {
      this._canvasGroupMaskBlurMaterial = VariantUtils.ConvertTo<Material>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._canvasGroupBlurMaterial))
    {
      this._canvasGroupBlurMaterial = VariantUtils.ConvertTo<Material>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._canvasGroupMaskMaterial))
    {
      this._canvasGroupMaskMaterial = VariantUtils.ConvertTo<Material>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCard.PropertyName._visibility))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._visibility = VariantUtils.ConvertTo<ModelVisibility>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCard.PropertyName.CardHighlight))
    {
      ref godot_variant local = ref value;
      NCardHighlight cardHighlight = this.CardHighlight;
      godot_variant from = VariantUtils.CreateFrom<NCardHighlight>(ref cardHighlight);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName.Body))
    {
      ref godot_variant local = ref value;
      Control body = this.Body;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref body);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName.Visibility))
    {
      ref godot_variant local = ref value;
      ModelVisibility visibility = this.Visibility;
      godot_variant from = VariantUtils.CreateFrom<ModelVisibility>(ref visibility);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName.PlayPileTween))
    {
      ref godot_variant local = ref value;
      Tween playPileTween = this.PlayPileTween;
      godot_variant from = VariantUtils.CreateFrom<Tween>(ref playPileTween);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName.RandomizeCostTween))
    {
      ref godot_variant local = ref value;
      Tween randomizeCostTween = this.RandomizeCostTween;
      godot_variant from = VariantUtils.CreateFrom<Tween>(ref randomizeCostTween);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName.DisplayingPile))
    {
      ref godot_variant local = ref value;
      PileType displayingPile = this.DisplayingPile;
      godot_variant from = VariantUtils.CreateFrom<PileType>(ref displayingPile);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName.EnchantmentTab))
    {
      ref godot_variant local = ref value;
      Control enchantmentTab = this.EnchantmentTab;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref enchantmentTab);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName.EnchantmentVfxOverride))
    {
      ref godot_variant local = ref value;
      TextureRect enchantmentVfxOverride = this.EnchantmentVfxOverride;
      godot_variant from = VariantUtils.CreateFrom<TextureRect>(ref enchantmentVfxOverride);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName.OverlayContainer))
    {
      ref godot_variant local = ref value;
      Node overlayContainer = this.OverlayContainer;
      godot_variant from = VariantUtils.CreateFrom<Node>(ref overlayContainer);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName.CardVfxContainer))
    {
      ref godot_variant local = ref value;
      Node cardVfxContainer = this.CardVfxContainer;
      godot_variant from = VariantUtils.CreateFrom<Node>(ref cardVfxContainer);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._titleLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._titleLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._descriptionLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._descriptionLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._ancientPortrait))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._ancientPortrait);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._portrait))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._portrait);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._frame))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._frame);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._ancientBorderGlassOverlay))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._ancientBorderGlassOverlay);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._ancientBorder))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._ancientBorder);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._ancientBanner))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._ancientBanner);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._ancientTextBg))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._ancientTextBg);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._portraitBorder))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._portraitBorder);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._banner))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._banner);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._lock))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._lock);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._typePlaque))
    {
      value = VariantUtils.CreateFrom<NinePatchRect>(ref this._typePlaque);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._typeLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._typeLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._portraitCanvasGroup))
    {
      value = VariantUtils.CreateFrom<CanvasGroup>(ref this._portraitCanvasGroup);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._rareGlow))
    {
      value = VariantUtils.CreateFrom<NCardRareGlow>(ref this._rareGlow);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._uncommonGlow))
    {
      value = VariantUtils.CreateFrom<NCardUncommonGlow>(ref this._uncommonGlow);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._sparkles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._sparkles);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._energyIcon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._energyIcon);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._energyLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._energyLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._unplayableEnergyIcon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._unplayableEnergyIcon);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._starIcon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._starIcon);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._starLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._starLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._unplayableStarIcon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._unplayableStarIcon);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._overlayContainer))
    {
      value = VariantUtils.CreateFrom<Node>(ref this._overlayContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._cardOverlay))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._cardOverlay);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._cardVfxContainer))
    {
      value = VariantUtils.CreateFrom<Node>(ref this._cardVfxContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._enchantmentTab))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._enchantmentTab);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._enchantmentVfxOverride))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._enchantmentVfxOverride);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._enchantmentIcon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._enchantmentIcon);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._enchantmentLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._enchantmentLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._defaultEnchantmentPosition))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._defaultEnchantmentPosition);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._pretendCardCanBePlayed))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._pretendCardCanBePlayed);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._forceUnpoweredPreview))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._forceUnpoweredPreview);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._portraitBlurMaterial))
    {
      value = VariantUtils.CreateFrom<Material>(ref this._portraitBlurMaterial);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._canvasGroupMaskBlurMaterial))
    {
      value = VariantUtils.CreateFrom<Material>(ref this._canvasGroupMaskBlurMaterial);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._canvasGroupBlurMaterial))
    {
      value = VariantUtils.CreateFrom<Material>(ref this._canvasGroupBlurMaterial);
      return true;
    }
    if (StringName.op_Equality(ref name, NCard.PropertyName._canvasGroupMaskMaterial))
    {
      value = VariantUtils.CreateFrom<Material>(ref this._canvasGroupMaskMaterial);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCard.PropertyName._visibility))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<ModelVisibility>(ref this._visibility);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCard.PropertyName._titleLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCard.PropertyName._descriptionLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCard.PropertyName._ancientPortrait, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCard.PropertyName._portrait, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCard.PropertyName._frame, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCard.PropertyName._ancientBorderGlassOverlay, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCard.PropertyName._ancientBorder, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCard.PropertyName._ancientBanner, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCard.PropertyName._ancientTextBg, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCard.PropertyName._portraitBorder, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCard.PropertyName._banner, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCard.PropertyName._lock, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCard.PropertyName._typePlaque, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCard.PropertyName._typeLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCard.PropertyName._portraitCanvasGroup, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCard.PropertyName._rareGlow, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCard.PropertyName._uncommonGlow, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCard.PropertyName._sparkles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCard.PropertyName._energyIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCard.PropertyName._energyLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCard.PropertyName._unplayableEnergyIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCard.PropertyName._starIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCard.PropertyName._starLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCard.PropertyName._unplayableStarIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCard.PropertyName._overlayContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCard.PropertyName._cardOverlay, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCard.PropertyName._cardVfxContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCard.PropertyName._enchantmentTab, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCard.PropertyName._enchantmentVfxOverride, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCard.PropertyName._enchantmentIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCard.PropertyName._enchantmentLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NCard.PropertyName._defaultEnchantmentPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCard.PropertyName.CardHighlight, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCard.PropertyName.Body, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCard.PropertyName._pretendCardCanBePlayed, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCard.PropertyName._forceUnpoweredPreview, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCard.PropertyName._portraitBlurMaterial, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCard.PropertyName._canvasGroupMaskBlurMaterial, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCard.PropertyName._canvasGroupBlurMaterial, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCard.PropertyName._canvasGroupMaskMaterial, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NCard.PropertyName._visibility, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NCard.PropertyName.Visibility, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCard.PropertyName.PlayPileTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCard.PropertyName.RandomizeCostTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NCard.PropertyName.DisplayingPile, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCard.PropertyName.EnchantmentTab, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCard.PropertyName.EnchantmentVfxOverride, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCard.PropertyName.OverlayContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCard.PropertyName.CardVfxContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo1 = info;
    StringName cardHighlight1 = NCard.PropertyName.CardHighlight;
    NCardHighlight cardHighlight2 = this.CardHighlight;
    Variant variant1 = Variant.From<NCardHighlight>(ref cardHighlight2);
    serializationInfo1.AddProperty(cardHighlight1, variant1);
    GodotSerializationInfo serializationInfo2 = info;
    StringName body1 = NCard.PropertyName.Body;
    Control body2 = this.Body;
    Variant variant2 = Variant.From<Control>(ref body2);
    serializationInfo2.AddProperty(body1, variant2);
    GodotSerializationInfo serializationInfo3 = info;
    StringName visibility1 = NCard.PropertyName.Visibility;
    ModelVisibility visibility2 = this.Visibility;
    Variant variant3 = Variant.From<ModelVisibility>(ref visibility2);
    serializationInfo3.AddProperty(visibility1, variant3);
    GodotSerializationInfo serializationInfo4 = info;
    StringName playPileTween1 = NCard.PropertyName.PlayPileTween;
    Tween playPileTween2 = this.PlayPileTween;
    Variant variant4 = Variant.From<Tween>(ref playPileTween2);
    serializationInfo4.AddProperty(playPileTween1, variant4);
    GodotSerializationInfo serializationInfo5 = info;
    StringName randomizeCostTween1 = NCard.PropertyName.RandomizeCostTween;
    Tween randomizeCostTween2 = this.RandomizeCostTween;
    Variant variant5 = Variant.From<Tween>(ref randomizeCostTween2);
    serializationInfo5.AddProperty(randomizeCostTween1, variant5);
    GodotSerializationInfo serializationInfo6 = info;
    StringName displayingPile1 = NCard.PropertyName.DisplayingPile;
    PileType displayingPile2 = this.DisplayingPile;
    Variant variant6 = Variant.From<PileType>(ref displayingPile2);
    serializationInfo6.AddProperty(displayingPile1, variant6);
    info.AddProperty(NCard.PropertyName._titleLabel, Variant.From<MegaLabel>(ref this._titleLabel));
    info.AddProperty(NCard.PropertyName._descriptionLabel, Variant.From<MegaRichTextLabel>(ref this._descriptionLabel));
    info.AddProperty(NCard.PropertyName._ancientPortrait, Variant.From<TextureRect>(ref this._ancientPortrait));
    info.AddProperty(NCard.PropertyName._portrait, Variant.From<TextureRect>(ref this._portrait));
    info.AddProperty(NCard.PropertyName._frame, Variant.From<TextureRect>(ref this._frame));
    info.AddProperty(NCard.PropertyName._ancientBorderGlassOverlay, Variant.From<TextureRect>(ref this._ancientBorderGlassOverlay));
    info.AddProperty(NCard.PropertyName._ancientBorder, Variant.From<TextureRect>(ref this._ancientBorder));
    info.AddProperty(NCard.PropertyName._ancientBanner, Variant.From<Control>(ref this._ancientBanner));
    info.AddProperty(NCard.PropertyName._ancientTextBg, Variant.From<TextureRect>(ref this._ancientTextBg));
    info.AddProperty(NCard.PropertyName._portraitBorder, Variant.From<TextureRect>(ref this._portraitBorder));
    info.AddProperty(NCard.PropertyName._banner, Variant.From<TextureRect>(ref this._banner));
    info.AddProperty(NCard.PropertyName._lock, Variant.From<TextureRect>(ref this._lock));
    info.AddProperty(NCard.PropertyName._typePlaque, Variant.From<NinePatchRect>(ref this._typePlaque));
    info.AddProperty(NCard.PropertyName._typeLabel, Variant.From<MegaLabel>(ref this._typeLabel));
    info.AddProperty(NCard.PropertyName._portraitCanvasGroup, Variant.From<CanvasGroup>(ref this._portraitCanvasGroup));
    info.AddProperty(NCard.PropertyName._rareGlow, Variant.From<NCardRareGlow>(ref this._rareGlow));
    info.AddProperty(NCard.PropertyName._uncommonGlow, Variant.From<NCardUncommonGlow>(ref this._uncommonGlow));
    info.AddProperty(NCard.PropertyName._sparkles, Variant.From<GpuParticles2D>(ref this._sparkles));
    info.AddProperty(NCard.PropertyName._energyIcon, Variant.From<TextureRect>(ref this._energyIcon));
    info.AddProperty(NCard.PropertyName._energyLabel, Variant.From<MegaLabel>(ref this._energyLabel));
    info.AddProperty(NCard.PropertyName._unplayableEnergyIcon, Variant.From<TextureRect>(ref this._unplayableEnergyIcon));
    info.AddProperty(NCard.PropertyName._starIcon, Variant.From<TextureRect>(ref this._starIcon));
    info.AddProperty(NCard.PropertyName._starLabel, Variant.From<MegaLabel>(ref this._starLabel));
    info.AddProperty(NCard.PropertyName._unplayableStarIcon, Variant.From<TextureRect>(ref this._unplayableStarIcon));
    info.AddProperty(NCard.PropertyName._overlayContainer, Variant.From<Node>(ref this._overlayContainer));
    info.AddProperty(NCard.PropertyName._cardOverlay, Variant.From<Control>(ref this._cardOverlay));
    info.AddProperty(NCard.PropertyName._cardVfxContainer, Variant.From<Node>(ref this._cardVfxContainer));
    info.AddProperty(NCard.PropertyName._enchantmentTab, Variant.From<Control>(ref this._enchantmentTab));
    info.AddProperty(NCard.PropertyName._enchantmentVfxOverride, Variant.From<TextureRect>(ref this._enchantmentVfxOverride));
    info.AddProperty(NCard.PropertyName._enchantmentIcon, Variant.From<TextureRect>(ref this._enchantmentIcon));
    info.AddProperty(NCard.PropertyName._enchantmentLabel, Variant.From<MegaLabel>(ref this._enchantmentLabel));
    info.AddProperty(NCard.PropertyName._defaultEnchantmentPosition, Variant.From<Vector2>(ref this._defaultEnchantmentPosition));
    info.AddProperty(NCard.PropertyName._pretendCardCanBePlayed, Variant.From<bool>(ref this._pretendCardCanBePlayed));
    info.AddProperty(NCard.PropertyName._forceUnpoweredPreview, Variant.From<bool>(ref this._forceUnpoweredPreview));
    info.AddProperty(NCard.PropertyName._portraitBlurMaterial, Variant.From<Material>(ref this._portraitBlurMaterial));
    info.AddProperty(NCard.PropertyName._canvasGroupMaskBlurMaterial, Variant.From<Material>(ref this._canvasGroupMaskBlurMaterial));
    info.AddProperty(NCard.PropertyName._canvasGroupBlurMaterial, Variant.From<Material>(ref this._canvasGroupBlurMaterial));
    info.AddProperty(NCard.PropertyName._canvasGroupMaskMaterial, Variant.From<Material>(ref this._canvasGroupMaskMaterial));
    info.AddProperty(NCard.PropertyName._visibility, Variant.From<ModelVisibility>(ref this._visibility));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCard.PropertyName.CardHighlight, ref variant1))
      this.CardHighlight = ((Variant) ref variant1).As<NCardHighlight>();
    Variant variant2;
    if (info.TryGetProperty(NCard.PropertyName.Body, ref variant2))
      this.Body = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NCard.PropertyName.Visibility, ref variant3))
      this.Visibility = ((Variant) ref variant3).As<ModelVisibility>();
    Variant variant4;
    if (info.TryGetProperty(NCard.PropertyName.PlayPileTween, ref variant4))
      this.PlayPileTween = ((Variant) ref variant4).As<Tween>();
    Variant variant5;
    if (info.TryGetProperty(NCard.PropertyName.RandomizeCostTween, ref variant5))
      this.RandomizeCostTween = ((Variant) ref variant5).As<Tween>();
    Variant variant6;
    if (info.TryGetProperty(NCard.PropertyName.DisplayingPile, ref variant6))
      this.DisplayingPile = ((Variant) ref variant6).As<PileType>();
    Variant variant7;
    if (info.TryGetProperty(NCard.PropertyName._titleLabel, ref variant7))
      this._titleLabel = ((Variant) ref variant7).As<MegaLabel>();
    Variant variant8;
    if (info.TryGetProperty(NCard.PropertyName._descriptionLabel, ref variant8))
      this._descriptionLabel = ((Variant) ref variant8).As<MegaRichTextLabel>();
    Variant variant9;
    if (info.TryGetProperty(NCard.PropertyName._ancientPortrait, ref variant9))
      this._ancientPortrait = ((Variant) ref variant9).As<TextureRect>();
    Variant variant10;
    if (info.TryGetProperty(NCard.PropertyName._portrait, ref variant10))
      this._portrait = ((Variant) ref variant10).As<TextureRect>();
    Variant variant11;
    if (info.TryGetProperty(NCard.PropertyName._frame, ref variant11))
      this._frame = ((Variant) ref variant11).As<TextureRect>();
    Variant variant12;
    if (info.TryGetProperty(NCard.PropertyName._ancientBorderGlassOverlay, ref variant12))
      this._ancientBorderGlassOverlay = ((Variant) ref variant12).As<TextureRect>();
    Variant variant13;
    if (info.TryGetProperty(NCard.PropertyName._ancientBorder, ref variant13))
      this._ancientBorder = ((Variant) ref variant13).As<TextureRect>();
    Variant variant14;
    if (info.TryGetProperty(NCard.PropertyName._ancientBanner, ref variant14))
      this._ancientBanner = ((Variant) ref variant14).As<Control>();
    Variant variant15;
    if (info.TryGetProperty(NCard.PropertyName._ancientTextBg, ref variant15))
      this._ancientTextBg = ((Variant) ref variant15).As<TextureRect>();
    Variant variant16;
    if (info.TryGetProperty(NCard.PropertyName._portraitBorder, ref variant16))
      this._portraitBorder = ((Variant) ref variant16).As<TextureRect>();
    Variant variant17;
    if (info.TryGetProperty(NCard.PropertyName._banner, ref variant17))
      this._banner = ((Variant) ref variant17).As<TextureRect>();
    Variant variant18;
    if (info.TryGetProperty(NCard.PropertyName._lock, ref variant18))
      this._lock = ((Variant) ref variant18).As<TextureRect>();
    Variant variant19;
    if (info.TryGetProperty(NCard.PropertyName._typePlaque, ref variant19))
      this._typePlaque = ((Variant) ref variant19).As<NinePatchRect>();
    Variant variant20;
    if (info.TryGetProperty(NCard.PropertyName._typeLabel, ref variant20))
      this._typeLabel = ((Variant) ref variant20).As<MegaLabel>();
    Variant variant21;
    if (info.TryGetProperty(NCard.PropertyName._portraitCanvasGroup, ref variant21))
      this._portraitCanvasGroup = ((Variant) ref variant21).As<CanvasGroup>();
    Variant variant22;
    if (info.TryGetProperty(NCard.PropertyName._rareGlow, ref variant22))
      this._rareGlow = ((Variant) ref variant22).As<NCardRareGlow>();
    Variant variant23;
    if (info.TryGetProperty(NCard.PropertyName._uncommonGlow, ref variant23))
      this._uncommonGlow = ((Variant) ref variant23).As<NCardUncommonGlow>();
    Variant variant24;
    if (info.TryGetProperty(NCard.PropertyName._sparkles, ref variant24))
      this._sparkles = ((Variant) ref variant24).As<GpuParticles2D>();
    Variant variant25;
    if (info.TryGetProperty(NCard.PropertyName._energyIcon, ref variant25))
      this._energyIcon = ((Variant) ref variant25).As<TextureRect>();
    Variant variant26;
    if (info.TryGetProperty(NCard.PropertyName._energyLabel, ref variant26))
      this._energyLabel = ((Variant) ref variant26).As<MegaLabel>();
    Variant variant27;
    if (info.TryGetProperty(NCard.PropertyName._unplayableEnergyIcon, ref variant27))
      this._unplayableEnergyIcon = ((Variant) ref variant27).As<TextureRect>();
    Variant variant28;
    if (info.TryGetProperty(NCard.PropertyName._starIcon, ref variant28))
      this._starIcon = ((Variant) ref variant28).As<TextureRect>();
    Variant variant29;
    if (info.TryGetProperty(NCard.PropertyName._starLabel, ref variant29))
      this._starLabel = ((Variant) ref variant29).As<MegaLabel>();
    Variant variant30;
    if (info.TryGetProperty(NCard.PropertyName._unplayableStarIcon, ref variant30))
      this._unplayableStarIcon = ((Variant) ref variant30).As<TextureRect>();
    Variant variant31;
    if (info.TryGetProperty(NCard.PropertyName._overlayContainer, ref variant31))
      this._overlayContainer = ((Variant) ref variant31).As<Node>();
    Variant variant32;
    if (info.TryGetProperty(NCard.PropertyName._cardOverlay, ref variant32))
      this._cardOverlay = ((Variant) ref variant32).As<Control>();
    Variant variant33;
    if (info.TryGetProperty(NCard.PropertyName._cardVfxContainer, ref variant33))
      this._cardVfxContainer = ((Variant) ref variant33).As<Node>();
    Variant variant34;
    if (info.TryGetProperty(NCard.PropertyName._enchantmentTab, ref variant34))
      this._enchantmentTab = ((Variant) ref variant34).As<Control>();
    Variant variant35;
    if (info.TryGetProperty(NCard.PropertyName._enchantmentVfxOverride, ref variant35))
      this._enchantmentVfxOverride = ((Variant) ref variant35).As<TextureRect>();
    Variant variant36;
    if (info.TryGetProperty(NCard.PropertyName._enchantmentIcon, ref variant36))
      this._enchantmentIcon = ((Variant) ref variant36).As<TextureRect>();
    Variant variant37;
    if (info.TryGetProperty(NCard.PropertyName._enchantmentLabel, ref variant37))
      this._enchantmentLabel = ((Variant) ref variant37).As<MegaLabel>();
    Variant variant38;
    if (info.TryGetProperty(NCard.PropertyName._defaultEnchantmentPosition, ref variant38))
      this._defaultEnchantmentPosition = ((Variant) ref variant38).As<Vector2>();
    Variant variant39;
    if (info.TryGetProperty(NCard.PropertyName._pretendCardCanBePlayed, ref variant39))
      this._pretendCardCanBePlayed = ((Variant) ref variant39).As<bool>();
    Variant variant40;
    if (info.TryGetProperty(NCard.PropertyName._forceUnpoweredPreview, ref variant40))
      this._forceUnpoweredPreview = ((Variant) ref variant40).As<bool>();
    Variant variant41;
    if (info.TryGetProperty(NCard.PropertyName._portraitBlurMaterial, ref variant41))
      this._portraitBlurMaterial = ((Variant) ref variant41).As<Material>();
    Variant variant42;
    if (info.TryGetProperty(NCard.PropertyName._canvasGroupMaskBlurMaterial, ref variant42))
      this._canvasGroupMaskBlurMaterial = ((Variant) ref variant42).As<Material>();
    Variant variant43;
    if (info.TryGetProperty(NCard.PropertyName._canvasGroupBlurMaterial, ref variant43))
      this._canvasGroupBlurMaterial = ((Variant) ref variant43).As<Material>();
    Variant variant44;
    if (info.TryGetProperty(NCard.PropertyName._canvasGroupMaskMaterial, ref variant44))
      this._canvasGroupMaskMaterial = ((Variant) ref variant44).As<Material>();
    Variant variant45;
    if (!info.TryGetProperty(NCard.PropertyName._visibility, ref variant45))
      return;
    this._visibility = ((Variant) ref variant45).As<ModelVisibility>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName OnInstantiated = StringName.op_Implicit(nameof (OnInstantiated));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName InitPool = StringName.op_Implicit(nameof (InitPool));
    public static readonly StringName GetCurrentSize = StringName.op_Implicit(nameof (GetCurrentSize));
    public static readonly StringName UpdateVisuals = StringName.op_Implicit(nameof (UpdateVisuals));
    public static readonly StringName ShowUpgradePreview = StringName.op_Implicit(nameof (ShowUpgradePreview));
    public static readonly StringName UpdateEnchantmentVisuals = StringName.op_Implicit(nameof (UpdateEnchantmentVisuals));
    public static readonly StringName OnEnchantmentStatusChanged = StringName.op_Implicit(nameof (OnEnchantmentStatusChanged));
    public static readonly StringName SetEnchantmentStatus = StringName.op_Implicit(nameof (SetEnchantmentStatus));
    public static readonly StringName UpdateEnergyCostVisuals = StringName.op_Implicit(nameof (UpdateEnergyCostVisuals));
    public static readonly StringName SetPretendCardCanBePlayed = StringName.op_Implicit(nameof (SetPretendCardCanBePlayed));
    public static readonly StringName SetForceUnpoweredPreview = StringName.op_Implicit(nameof (SetForceUnpoweredPreview));
    public static readonly StringName UpdateEnergyCostColor = StringName.op_Implicit(nameof (UpdateEnergyCostColor));
    public static readonly StringName UpdateStarCostVisuals = StringName.op_Implicit(nameof (UpdateStarCostVisuals));
    public static readonly StringName UpdateStarCostText = StringName.op_Implicit(nameof (UpdateStarCostText));
    public static readonly StringName UpdateStarCostColor = StringName.op_Implicit(nameof (UpdateStarCostColor));
    public static readonly StringName GetCostTextColorInHand = StringName.op_Implicit(nameof (GetCostTextColorInHand));
    public static readonly StringName GetCostOutlineColorInHand = StringName.op_Implicit(nameof (GetCostOutlineColorInHand));
    public static readonly StringName PlayRandomizeCostAnim = StringName.op_Implicit(nameof (PlayRandomizeCostAnim));
    public static readonly StringName Reload = StringName.op_Implicit(nameof (Reload));
    public static readonly StringName UpdatePortrait = StringName.op_Implicit(nameof (UpdatePortrait));
    public static readonly StringName UpdateTypePlaque = StringName.op_Implicit(nameof (UpdateTypePlaque));
    public static readonly StringName UpdateTypePlaqueSizeAndPosition = StringName.op_Implicit(nameof (UpdateTypePlaqueSizeAndPosition));
    public static readonly StringName UpdateTitleLabel = StringName.op_Implicit(nameof (UpdateTitleLabel));
    public static readonly StringName GetTitleLabelOutlineColor = StringName.op_Implicit(nameof (GetTitleLabelOutlineColor));
    public static readonly StringName ReloadOverlay = StringName.op_Implicit(nameof (ReloadOverlay));
    public static readonly StringName OnAfflictionChanged = StringName.op_Implicit(nameof (OnAfflictionChanged));
    public static readonly StringName OnEnchantmentChanged = StringName.op_Implicit(nameof (OnEnchantmentChanged));
    public static readonly StringName GetTitleText = StringName.op_Implicit(nameof (GetTitleText));
    public static readonly StringName ActivateRewardScreenGlow = StringName.op_Implicit(nameof (ActivateRewardScreenGlow));
    public static readonly StringName KillRarityGlow = StringName.op_Implicit(nameof (KillRarityGlow));
    public static readonly StringName AnimCardToPlayPile = StringName.op_Implicit(nameof (AnimCardToPlayPile));
    public static readonly StringName OnReturnedFromPool = StringName.op_Implicit(nameof (OnReturnedFromPool));
    public static readonly StringName OnFreedToPool = StringName.op_Implicit(nameof (OnFreedToPool));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName CardHighlight = StringName.op_Implicit(nameof (CardHighlight));
    public static readonly StringName Body = StringName.op_Implicit(nameof (Body));
    public static readonly StringName Visibility = StringName.op_Implicit(nameof (Visibility));
    public static readonly StringName PlayPileTween = StringName.op_Implicit(nameof (PlayPileTween));
    public static readonly StringName RandomizeCostTween = StringName.op_Implicit(nameof (RandomizeCostTween));
    public static readonly StringName DisplayingPile = StringName.op_Implicit(nameof (DisplayingPile));
    public static readonly StringName EnchantmentTab = StringName.op_Implicit(nameof (EnchantmentTab));
    public static readonly StringName EnchantmentVfxOverride = StringName.op_Implicit(nameof (EnchantmentVfxOverride));
    public static readonly StringName OverlayContainer = StringName.op_Implicit(nameof (OverlayContainer));
    public static readonly StringName CardVfxContainer = StringName.op_Implicit(nameof (CardVfxContainer));
    public static readonly StringName _titleLabel = StringName.op_Implicit(nameof (_titleLabel));
    public static readonly StringName _descriptionLabel = StringName.op_Implicit(nameof (_descriptionLabel));
    public static readonly StringName _ancientPortrait = StringName.op_Implicit(nameof (_ancientPortrait));
    public static readonly StringName _portrait = StringName.op_Implicit(nameof (_portrait));
    public static readonly StringName _frame = StringName.op_Implicit(nameof (_frame));
    public static readonly StringName _ancientBorderGlassOverlay = StringName.op_Implicit(nameof (_ancientBorderGlassOverlay));
    public static readonly StringName _ancientBorder = StringName.op_Implicit(nameof (_ancientBorder));
    public static readonly StringName _ancientBanner = StringName.op_Implicit(nameof (_ancientBanner));
    public static readonly StringName _ancientTextBg = StringName.op_Implicit(nameof (_ancientTextBg));
    public static readonly StringName _portraitBorder = StringName.op_Implicit(nameof (_portraitBorder));
    public static readonly StringName _banner = StringName.op_Implicit(nameof (_banner));
    public static readonly StringName _lock = StringName.op_Implicit(nameof (_lock));
    public static readonly StringName _typePlaque = StringName.op_Implicit(nameof (_typePlaque));
    public static readonly StringName _typeLabel = StringName.op_Implicit(nameof (_typeLabel));
    public static readonly StringName _portraitCanvasGroup = StringName.op_Implicit(nameof (_portraitCanvasGroup));
    public static readonly StringName _rareGlow = StringName.op_Implicit(nameof (_rareGlow));
    public static readonly StringName _uncommonGlow = StringName.op_Implicit(nameof (_uncommonGlow));
    public static readonly StringName _sparkles = StringName.op_Implicit(nameof (_sparkles));
    public static readonly StringName _energyIcon = StringName.op_Implicit(nameof (_energyIcon));
    public static readonly StringName _energyLabel = StringName.op_Implicit(nameof (_energyLabel));
    public static readonly StringName _unplayableEnergyIcon = StringName.op_Implicit(nameof (_unplayableEnergyIcon));
    public static readonly StringName _starIcon = StringName.op_Implicit(nameof (_starIcon));
    public static readonly StringName _starLabel = StringName.op_Implicit(nameof (_starLabel));
    public static readonly StringName _unplayableStarIcon = StringName.op_Implicit(nameof (_unplayableStarIcon));
    public static readonly StringName _overlayContainer = StringName.op_Implicit(nameof (_overlayContainer));
    public static readonly StringName _cardOverlay = StringName.op_Implicit(nameof (_cardOverlay));
    public static readonly StringName _cardVfxContainer = StringName.op_Implicit(nameof (_cardVfxContainer));
    public static readonly StringName _enchantmentTab = StringName.op_Implicit(nameof (_enchantmentTab));
    public static readonly StringName _enchantmentVfxOverride = StringName.op_Implicit(nameof (_enchantmentVfxOverride));
    public static readonly StringName _enchantmentIcon = StringName.op_Implicit(nameof (_enchantmentIcon));
    public static readonly StringName _enchantmentLabel = StringName.op_Implicit(nameof (_enchantmentLabel));
    public static readonly StringName _defaultEnchantmentPosition = StringName.op_Implicit(nameof (_defaultEnchantmentPosition));
    public static readonly StringName _pretendCardCanBePlayed = StringName.op_Implicit(nameof (_pretendCardCanBePlayed));
    public static readonly StringName _forceUnpoweredPreview = StringName.op_Implicit(nameof (_forceUnpoweredPreview));
    public static readonly StringName _portraitBlurMaterial = StringName.op_Implicit(nameof (_portraitBlurMaterial));
    public static readonly StringName _canvasGroupMaskBlurMaterial = StringName.op_Implicit(nameof (_canvasGroupMaskBlurMaterial));
    public static readonly StringName _canvasGroupBlurMaterial = StringName.op_Implicit(nameof (_canvasGroupBlurMaterial));
    public static readonly StringName _canvasGroupMaskMaterial = StringName.op_Implicit(nameof (_canvasGroupMaskMaterial));
    public static readonly StringName _visibility = StringName.op_Implicit(nameof (_visibility));
  }

  public class SignalName : Control.SignalName
  {
  }
}
