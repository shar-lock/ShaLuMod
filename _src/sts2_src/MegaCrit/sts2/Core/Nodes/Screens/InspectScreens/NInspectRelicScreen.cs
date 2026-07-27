// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.InspectScreens.NInspectRelicScreen
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Debug;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.Unlocks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.InspectScreens;

[ScriptPath("res://src/Core/Nodes/Screens/InspectScreens/NInspectRelicScreen.cs")]
public class NInspectRelicScreen : Control, IScreenContext
{
  private static readonly StringName _v = new StringName("v");
  private static readonly StringName _s = new StringName("s");
  private static readonly StringName _h = new StringName("h");
  private static readonly string _scenePath = SceneHelper.GetScenePath("screens/inspect_relic_screen/inspect_relic_screen");
  private Control _popup;
  private Control _backstop;
  private MegaLabel _nameLabel;
  private MegaLabel _rarityLabel;
  private MegaRichTextLabel _description;
  private MegaRichTextLabel _flavor;
  private TextureRect _relicImage;
  private ShaderMaterial _frameHsv;
  private NGoldArrowButton _leftButton;
  private NGoldArrowButton _rightButton;
  private Control _hoverTipRect;
  private Tween? _screenTween;
  private Tween? _popupTween;
  private Vector2 _popupPosition;
  private float _leftButtonX;
  private float _rightButtonX;
  private const double _arrowButtonDelay = 0.1;
  private IReadOnlyList<RelicModel> _relics;
  private int _index;
  private HashSet<RelicModel> _allUnlockedRelics = new HashSet<RelicModel>();

  public static string[] AssetPaths
  {
    get => new string[1]{ NInspectRelicScreen._scenePath };
  }

  public static NInspectRelicScreen? Create()
  {
    return TestMode.IsOn ? (NInspectRelicScreen) null : PreloadManager.Cache.GetScene(NInspectRelicScreen._scenePath).Instantiate<NInspectRelicScreen>((PackedScene.GenEditState) 0L);
  }

  public void Open(IReadOnlyList<RelicModel> relics, RelicModel relic)
  {
    Log.Info($"Inspecting Relic: {relic.Title}");
    UnlockState stateFromProgress = SaveManager.Instance.GenerateUnlockStateFromProgress();
    this._allUnlockedRelics.Clear();
    this._allUnlockedRelics.UnionWith(stateFromProgress.Relics);
    this._relics = (IReadOnlyList<RelicModel>) relics.ToList<RelicModel>();
    this._index = relics.IndexOf<RelicModel>(relic);
    this.SetRelic(this._index);
    ((CanvasItem) this).Visible = true;
    ((CanvasItem) this._popup).Modulate = StsColors.transparentBlack;
    ((CanvasItem) this._leftButton).Modulate = StsColors.transparentBlack;
    ((CanvasItem) this._rightButton).Modulate = StsColors.transparentBlack;
    this._leftButton.Enable();
    this._rightButton.Enable();
    ((CanvasItem) this._backstop).Visible = true;
    this._backstop.MouseFilter = (Control.MouseFilterEnum) 0L;
    this._leftButton.MouseFilter = (Control.MouseFilterEnum) 0L;
    this._rightButton.MouseFilter = (Control.MouseFilterEnum) 0L;
    this._screenTween?.Kill();
    this._screenTween = ((Node) this).CreateTween().SetParallel(true);
    this._screenTween.TweenProperty((GodotObject) this._backstop, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.9f), 0.25);
    this._screenTween.TweenProperty((GodotObject) this._leftButton, NodePath.op_Implicit("position:x"), Variant.op_Implicit(this._leftButtonX), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L).From(Variant.op_Implicit(this._leftButtonX + 100f)).SetDelay(0.1);
    this._screenTween.TweenProperty((GodotObject) this._leftButton, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.25).SetDelay(0.1);
    this._screenTween.TweenProperty((GodotObject) this._rightButton, NodePath.op_Implicit("position:x"), Variant.op_Implicit(this._rightButtonX), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L).From(Variant.op_Implicit(this._rightButtonX - 100f)).SetDelay(0.1);
    this._screenTween.TweenProperty((GodotObject) this._rightButton, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.25).SetDelay(0.1);
    this._popupTween?.Kill();
    this._popupTween = ((Node) this).CreateTween().SetParallel(true);
    this._popupTween.TweenProperty((GodotObject) this._popup, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.25);
    this._popupTween.TweenProperty((GodotObject) this._popup, NodePath.op_Implicit("position"), Variant.op_Implicit(this._popupPosition), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L).From(Variant.op_Implicit(Vector2.op_Addition(this._popupPosition, new Vector2(0.0f, 200f))));
    ActiveScreenContext.Instance.Update();
    NHotkeyManager.Instance.AddBlockingScreen((Node) this);
    NHotkeyManager.Instance.PushHotkeyPressedBinding(StringName.op_Implicit(MegaInput.cancel), new Action(this.Close));
    NHotkeyManager.Instance.PushHotkeyPressedBinding(StringName.op_Implicit(MegaInput.pauseAndBack), new Action(this.Close));
  }

  public override void _Ready()
  {
    this._popup = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Popup"));
    this._backstop = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Backstop"));
    this._nameLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%RelicName"));
    this._rarityLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%Rarity"));
    this._description = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%RelicDescription"));
    this._flavor = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%FlavorText"));
    this._relicImage = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%RelicImage"));
    this._frameHsv = (ShaderMaterial) ((CanvasItem) ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Frame"))).Material;
    this._leftButton = ((Node) this).GetNode<NGoldArrowButton>(NodePath.op_Implicit("%LeftArrow"));
    this._rightButton = ((Node) this).GetNode<NGoldArrowButton>(NodePath.op_Implicit("%RightArrow"));
    this._popupPosition = this._popup.Position;
    this._hoverTipRect = ((Node) this).GetNode<Control>(NodePath.op_Implicit("HoverTipRect"));
    this._backstop = (Control) ((Node) this).GetNode<NButton>(NodePath.op_Implicit("Backstop"));
    ((GodotObject) this._backstop).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnBackstopPressed)), 0U);
    this._leftButton = ((Node) this).GetNode<NGoldArrowButton>(NodePath.op_Implicit("LeftArrow"));
    ((GodotObject) this._leftButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnLeftButtonPressed)), 0U);
    this._rightButton = ((Node) this).GetNode<NGoldArrowButton>(NodePath.op_Implicit("RightArrow"));
    ((GodotObject) this._rightButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnRightButtonPressed)), 0U);
    this._leftButtonX = this._leftButton.Position.X;
    this._rightButtonX = this._rightButton.Position.X;
  }

  public override void _Input(InputEvent inputEvent)
  {
    if (!((CanvasItem) this).IsVisibleInTree() || NDevConsole.IsConsoleVisible)
      return;
    bool flag;
    switch (((Node) this).GetViewport().GuiGetFocusOwner())
    {
      case TextEdit _:
      case LineEdit _:
        flag = true;
        break;
      default:
        flag = false;
        break;
    }
    if (flag)
      return;
    if (inputEvent.IsActionPressed(MegaInput.left, false, false))
      this.OnLeftButtonPressed((NButton) this._leftButton);
    if (!inputEvent.IsActionPressed(MegaInput.right, false, false))
      return;
    this.OnRightButtonPressed((NButton) this._rightButton);
  }

  private void OnRightButtonPressed(NButton button)
  {
    this.SetRelic(this._index + 1);
    ((CanvasItem) this._popup).Modulate = Colors.White;
    this._popupTween?.Kill();
    this._popupTween = ((Node) this).CreateTween().SetParallel(true);
    this._popupTween.TweenProperty((GodotObject) this._popup, NodePath.op_Implicit("position"), Variant.op_Implicit(this._popupPosition), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L).From(Variant.op_Implicit(Vector2.op_Addition(this._popupPosition, new Vector2(100f, 0.0f))));
  }

  private void OnLeftButtonPressed(NButton button)
  {
    this.SetRelic(this._index - 1);
    ((CanvasItem) this._popup).Modulate = Colors.White;
    this._popupTween?.Kill();
    this._popupTween = ((Node) this).CreateTween().SetParallel(true);
    this._popupTween.TweenProperty((GodotObject) this._popup, NodePath.op_Implicit("position"), Variant.op_Implicit(this._popupPosition), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L).From(Variant.op_Implicit(Vector2.op_Addition(this._popupPosition, new Vector2(-100f, 0.0f))));
  }

  private void SetRelic(int index)
  {
    this._index = Math.Clamp(index, 0, this._relics.Count - 1);
    ((CanvasItem) this._leftButton).Visible = this._index > 0;
    this._leftButton.MouseFilter = this._index > 0 ? (Control.MouseFilterEnum) 0L : (Control.MouseFilterEnum) 2L;
    ((CanvasItem) this._rightButton).Visible = this._index < this._relics.Count - 1;
    this._rightButton.MouseFilter = this._index < this._relics.Count - 1 ? (Control.MouseFilterEnum) 0L : (Control.MouseFilterEnum) 2L;
    this.UpdateRelicDisplay();
  }

  private void UpdateRelicDisplay()
  {
    RelicModel relic = this._relics[this._index];
    if (!this._allUnlockedRelics.Contains(relic.CanonicalInstance))
    {
      this._nameLabel.SetTextAutoSize(new LocString("inspect_relic_screen", "LOCKED_TITLE").GetFormattedText());
      this._rarityLabel.SetTextAutoSize(string.Empty);
      ((CanvasItem) this._relicImage).SelfModulate = Colors.White;
      this._description.SetTextAutoSize(new LocString("inspect_relic_screen", "LOCKED_DESCRIPTION").GetFormattedText());
      this._flavor.Text = string.Empty;
      this.SetRarityVisuals(RelicRarity.Common);
      this._relicImage.Texture = PreloadManager.Cache.GetTexture2D(ImageHelper.GetImagePath("packed/common_ui/locked_model.png"));
    }
    else if (!SaveManager.Instance.IsRelicSeen(relic))
    {
      this._nameLabel.SetTextAutoSize(new LocString("inspect_relic_screen", "UNDISCOVERED_TITLE").GetFormattedText());
      this._rarityLabel.SetTextAutoSize(string.Empty);
      ((CanvasItem) this._relicImage).SelfModulate = StsColors.ninetyPercentBlack;
      this._description.SetTextAutoSize(new LocString("inspect_relic_screen", "UNDISCOVERED_DESCRIPTION").GetFormattedText());
      this._flavor.Text = string.Empty;
      this.SetRarityVisuals(relic.Rarity);
      this._relicImage.Texture = relic.BigIcon;
    }
    else
    {
      this._nameLabel.SetTextAutoSize(relic.Title.GetFormattedText());
      this._rarityLabel.SetTextAutoSize(new LocString("gameplay_ui", "RELIC_RARITY." + relic.Rarity.ToString().ToUpperInvariant()).GetFormattedText());
      ((CanvasItem) this._relicImage).SelfModulate = Colors.White;
      this._description.SetTextAutoSize(relic.DynamicDescription.GetFormattedText());
      this._flavor.SetTextAutoSize(relic.Flavor.GetFormattedText());
      this.SetRarityVisuals(relic.Rarity);
      this._relicImage.Texture = relic.BigIcon;
    }
    NHoverTipSet.Clear();
    if (!SaveManager.Instance.IsRelicSeen(relic))
      return;
    NHoverTipSet.CreateAndShow((Control) this, relic.HoverTipsExcludingRelic)?.SetAlignment(this._hoverTipRect, HoverTip.GetHoverTipAlignment((Control) this));
  }

  private void SetRarityVisuals(RelicRarity rarity)
  {
    Vector3 vector3;
    switch (rarity)
    {
      case RelicRarity.None:
      case RelicRarity.Starter:
      case RelicRarity.Common:
        ((CanvasItem) this._rarityLabel).Modulate = StsColors.cream;
        // ISSUE: explicit constructor call
        ((Vector3) ref vector3).\u002Ector(0.95f, 0.25f, 0.9f);
        break;
      case RelicRarity.Uncommon:
        ((CanvasItem) this._rarityLabel).Modulate = StsColors.blue;
        // ISSUE: explicit constructor call
        ((Vector3) ref vector3).\u002Ector(0.426f, 0.8f, 1.1f);
        break;
      case RelicRarity.Rare:
        ((CanvasItem) this._rarityLabel).Modulate = StsColors.gold;
        // ISSUE: explicit constructor call
        ((Vector3) ref vector3).\u002Ector(1f, 0.8f, 1.15f);
        break;
      case RelicRarity.Shop:
        ((CanvasItem) this._rarityLabel).Modulate = StsColors.blue;
        // ISSUE: explicit constructor call
        ((Vector3) ref vector3).\u002Ector(0.525f, 2.5f, 0.85f);
        break;
      case RelicRarity.Event:
        ((CanvasItem) this._rarityLabel).Modulate = StsColors.green;
        // ISSUE: explicit constructor call
        ((Vector3) ref vector3).\u002Ector(0.23f, 0.75f, 0.9f);
        break;
      case RelicRarity.Ancient:
        ((CanvasItem) this._rarityLabel).Modulate = StsColors.red;
        // ISSUE: explicit constructor call
        ((Vector3) ref vector3).\u002Ector(0.875f, 3f, 0.9f);
        break;
      default:
        Log.Error("Unspecified relic rarity: " + rarity.ToString());
        throw new ArgumentOutOfRangeException();
    }
    this._frameHsv.SetShaderParameter(NInspectRelicScreen._h, Variant.op_Implicit(vector3.X));
    this._frameHsv.SetShaderParameter(NInspectRelicScreen._s, Variant.op_Implicit(vector3.Y));
    this._frameHsv.SetShaderParameter(NInspectRelicScreen._v, Variant.op_Implicit(vector3.Z));
  }

  public void Close()
  {
    if (!((CanvasItem) this).Visible)
      return;
    NHotkeyManager.Instance.RemoveBlockingScreen((Node) this);
    NHotkeyManager.Instance.RemoveHotkeyPressedBinding(StringName.op_Implicit(MegaInput.cancel), new Action(this.Close));
    NHotkeyManager.Instance.RemoveHotkeyPressedBinding(StringName.op_Implicit(MegaInput.pauseAndBack), new Action(this.Close));
    this._backstop.MouseFilter = (Control.MouseFilterEnum) 2L;
    this._leftButton.MouseFilter = (Control.MouseFilterEnum) 2L;
    this._rightButton.MouseFilter = (Control.MouseFilterEnum) 2L;
    this._leftButton.Disable();
    this._rightButton.Disable();
    this._screenTween?.Kill();
    this._screenTween = ((Node) this).CreateTween().SetParallel(true);
    this._screenTween.TweenProperty((GodotObject) this._backstop, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.25);
    this._screenTween.TweenProperty((GodotObject) this._leftButton, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.1);
    this._screenTween.TweenProperty((GodotObject) this._rightButton, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.1);
    this._screenTween.TweenProperty((GodotObject) this._popup, NodePath.op_Implicit("modulate"), Variant.op_Implicit(StsColors.transparentWhite), 0.1);
    this._screenTween.Chain().TweenCallback(Callable.From((Action) (() =>
    {
      ((CanvasItem) this).Visible = false;
      ActiveScreenContext.Instance.Update();
    })));
    NHoverTipSet.Clear();
  }

  private void OnBackstopPressed(NButton _) => this.Close();

  public Control? DefaultFocusedControl => (Control) null;

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(10)
    {
      new MethodInfo(NInspectRelicScreen.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NInspectRelicScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NInspectRelicScreen.MethodName._Input, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NInspectRelicScreen.MethodName.OnRightButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("button"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NInspectRelicScreen.MethodName.OnLeftButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("button"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NInspectRelicScreen.MethodName.SetRelic, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("index"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NInspectRelicScreen.MethodName.UpdateRelicDisplay, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NInspectRelicScreen.MethodName.SetRarityVisuals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("rarity"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NInspectRelicScreen.MethodName.Close, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NInspectRelicScreen.MethodName.OnBackstopPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NInspectRelicScreen.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NInspectRelicScreen ninspectRelicScreen = NInspectRelicScreen.Create();
      ret = VariantUtils.CreateFrom<NInspectRelicScreen>(ref ninspectRelicScreen);
      return true;
    }
    if (StringName.op_Equality(ref method, NInspectRelicScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NInspectRelicScreen.MethodName._Input) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Input(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NInspectRelicScreen.MethodName.OnRightButtonPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnRightButtonPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NInspectRelicScreen.MethodName.OnLeftButtonPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnLeftButtonPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NInspectRelicScreen.MethodName.SetRelic) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetRelic(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NInspectRelicScreen.MethodName.UpdateRelicDisplay) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateRelicDisplay();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NInspectRelicScreen.MethodName.SetRarityVisuals) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetRarityVisuals(VariantUtils.ConvertTo<RelicRarity>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NInspectRelicScreen.MethodName.Close) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Close();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NInspectRelicScreen.MethodName.OnBackstopPressed) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnBackstopPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NInspectRelicScreen.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NInspectRelicScreen ninspectRelicScreen = NInspectRelicScreen.Create();
      ret = VariantUtils.CreateFrom<NInspectRelicScreen>(ref ninspectRelicScreen);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NInspectRelicScreen.MethodName.Create) || StringName.op_Equality(ref method, NInspectRelicScreen.MethodName._Ready) || StringName.op_Equality(ref method, NInspectRelicScreen.MethodName._Input) || StringName.op_Equality(ref method, NInspectRelicScreen.MethodName.OnRightButtonPressed) || StringName.op_Equality(ref method, NInspectRelicScreen.MethodName.OnLeftButtonPressed) || StringName.op_Equality(ref method, NInspectRelicScreen.MethodName.SetRelic) || StringName.op_Equality(ref method, NInspectRelicScreen.MethodName.UpdateRelicDisplay) || StringName.op_Equality(ref method, NInspectRelicScreen.MethodName.SetRarityVisuals) || StringName.op_Equality(ref method, NInspectRelicScreen.MethodName.Close) || StringName.op_Equality(ref method, NInspectRelicScreen.MethodName.OnBackstopPressed) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NInspectRelicScreen.PropertyName._popup))
    {
      this._popup = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectRelicScreen.PropertyName._backstop))
    {
      this._backstop = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectRelicScreen.PropertyName._nameLabel))
    {
      this._nameLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectRelicScreen.PropertyName._rarityLabel))
    {
      this._rarityLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectRelicScreen.PropertyName._description))
    {
      this._description = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectRelicScreen.PropertyName._flavor))
    {
      this._flavor = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectRelicScreen.PropertyName._relicImage))
    {
      this._relicImage = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectRelicScreen.PropertyName._frameHsv))
    {
      this._frameHsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectRelicScreen.PropertyName._leftButton))
    {
      this._leftButton = VariantUtils.ConvertTo<NGoldArrowButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectRelicScreen.PropertyName._rightButton))
    {
      this._rightButton = VariantUtils.ConvertTo<NGoldArrowButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectRelicScreen.PropertyName._hoverTipRect))
    {
      this._hoverTipRect = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectRelicScreen.PropertyName._screenTween))
    {
      this._screenTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectRelicScreen.PropertyName._popupTween))
    {
      this._popupTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectRelicScreen.PropertyName._popupPosition))
    {
      this._popupPosition = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectRelicScreen.PropertyName._leftButtonX))
    {
      this._leftButtonX = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectRelicScreen.PropertyName._rightButtonX))
    {
      this._rightButtonX = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NInspectRelicScreen.PropertyName._index))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._index = VariantUtils.ConvertTo<int>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NInspectRelicScreen.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectRelicScreen.PropertyName._popup))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._popup);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectRelicScreen.PropertyName._backstop))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._backstop);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectRelicScreen.PropertyName._nameLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._nameLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectRelicScreen.PropertyName._rarityLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._rarityLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectRelicScreen.PropertyName._description))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._description);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectRelicScreen.PropertyName._flavor))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._flavor);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectRelicScreen.PropertyName._relicImage))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._relicImage);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectRelicScreen.PropertyName._frameHsv))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._frameHsv);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectRelicScreen.PropertyName._leftButton))
    {
      value = VariantUtils.CreateFrom<NGoldArrowButton>(ref this._leftButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectRelicScreen.PropertyName._rightButton))
    {
      value = VariantUtils.CreateFrom<NGoldArrowButton>(ref this._rightButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectRelicScreen.PropertyName._hoverTipRect))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._hoverTipRect);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectRelicScreen.PropertyName._screenTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._screenTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectRelicScreen.PropertyName._popupTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._popupTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectRelicScreen.PropertyName._popupPosition))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._popupPosition);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectRelicScreen.PropertyName._leftButtonX))
    {
      value = VariantUtils.CreateFrom<float>(ref this._leftButtonX);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectRelicScreen.PropertyName._rightButtonX))
    {
      value = VariantUtils.CreateFrom<float>(ref this._rightButtonX);
      return true;
    }
    if (!StringName.op_Equality(ref name, NInspectRelicScreen.PropertyName._index))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<int>(ref this._index);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NInspectRelicScreen.PropertyName._popup, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NInspectRelicScreen.PropertyName._backstop, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NInspectRelicScreen.PropertyName._nameLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NInspectRelicScreen.PropertyName._rarityLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NInspectRelicScreen.PropertyName._description, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NInspectRelicScreen.PropertyName._flavor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NInspectRelicScreen.PropertyName._relicImage, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NInspectRelicScreen.PropertyName._frameHsv, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NInspectRelicScreen.PropertyName._leftButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NInspectRelicScreen.PropertyName._rightButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NInspectRelicScreen.PropertyName._hoverTipRect, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NInspectRelicScreen.PropertyName._screenTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NInspectRelicScreen.PropertyName._popupTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NInspectRelicScreen.PropertyName._popupPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NInspectRelicScreen.PropertyName._leftButtonX, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NInspectRelicScreen.PropertyName._rightButtonX, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NInspectRelicScreen.PropertyName._index, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NInspectRelicScreen.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NInspectRelicScreen.PropertyName._popup, Variant.From<Control>(ref this._popup));
    info.AddProperty(NInspectRelicScreen.PropertyName._backstop, Variant.From<Control>(ref this._backstop));
    info.AddProperty(NInspectRelicScreen.PropertyName._nameLabel, Variant.From<MegaLabel>(ref this._nameLabel));
    info.AddProperty(NInspectRelicScreen.PropertyName._rarityLabel, Variant.From<MegaLabel>(ref this._rarityLabel));
    info.AddProperty(NInspectRelicScreen.PropertyName._description, Variant.From<MegaRichTextLabel>(ref this._description));
    info.AddProperty(NInspectRelicScreen.PropertyName._flavor, Variant.From<MegaRichTextLabel>(ref this._flavor));
    info.AddProperty(NInspectRelicScreen.PropertyName._relicImage, Variant.From<TextureRect>(ref this._relicImage));
    info.AddProperty(NInspectRelicScreen.PropertyName._frameHsv, Variant.From<ShaderMaterial>(ref this._frameHsv));
    info.AddProperty(NInspectRelicScreen.PropertyName._leftButton, Variant.From<NGoldArrowButton>(ref this._leftButton));
    info.AddProperty(NInspectRelicScreen.PropertyName._rightButton, Variant.From<NGoldArrowButton>(ref this._rightButton));
    info.AddProperty(NInspectRelicScreen.PropertyName._hoverTipRect, Variant.From<Control>(ref this._hoverTipRect));
    info.AddProperty(NInspectRelicScreen.PropertyName._screenTween, Variant.From<Tween>(ref this._screenTween));
    info.AddProperty(NInspectRelicScreen.PropertyName._popupTween, Variant.From<Tween>(ref this._popupTween));
    info.AddProperty(NInspectRelicScreen.PropertyName._popupPosition, Variant.From<Vector2>(ref this._popupPosition));
    info.AddProperty(NInspectRelicScreen.PropertyName._leftButtonX, Variant.From<float>(ref this._leftButtonX));
    info.AddProperty(NInspectRelicScreen.PropertyName._rightButtonX, Variant.From<float>(ref this._rightButtonX));
    info.AddProperty(NInspectRelicScreen.PropertyName._index, Variant.From<int>(ref this._index));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NInspectRelicScreen.PropertyName._popup, ref variant1))
      this._popup = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NInspectRelicScreen.PropertyName._backstop, ref variant2))
      this._backstop = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NInspectRelicScreen.PropertyName._nameLabel, ref variant3))
      this._nameLabel = ((Variant) ref variant3).As<MegaLabel>();
    Variant variant4;
    if (info.TryGetProperty(NInspectRelicScreen.PropertyName._rarityLabel, ref variant4))
      this._rarityLabel = ((Variant) ref variant4).As<MegaLabel>();
    Variant variant5;
    if (info.TryGetProperty(NInspectRelicScreen.PropertyName._description, ref variant5))
      this._description = ((Variant) ref variant5).As<MegaRichTextLabel>();
    Variant variant6;
    if (info.TryGetProperty(NInspectRelicScreen.PropertyName._flavor, ref variant6))
      this._flavor = ((Variant) ref variant6).As<MegaRichTextLabel>();
    Variant variant7;
    if (info.TryGetProperty(NInspectRelicScreen.PropertyName._relicImage, ref variant7))
      this._relicImage = ((Variant) ref variant7).As<TextureRect>();
    Variant variant8;
    if (info.TryGetProperty(NInspectRelicScreen.PropertyName._frameHsv, ref variant8))
      this._frameHsv = ((Variant) ref variant8).As<ShaderMaterial>();
    Variant variant9;
    if (info.TryGetProperty(NInspectRelicScreen.PropertyName._leftButton, ref variant9))
      this._leftButton = ((Variant) ref variant9).As<NGoldArrowButton>();
    Variant variant10;
    if (info.TryGetProperty(NInspectRelicScreen.PropertyName._rightButton, ref variant10))
      this._rightButton = ((Variant) ref variant10).As<NGoldArrowButton>();
    Variant variant11;
    if (info.TryGetProperty(NInspectRelicScreen.PropertyName._hoverTipRect, ref variant11))
      this._hoverTipRect = ((Variant) ref variant11).As<Control>();
    Variant variant12;
    if (info.TryGetProperty(NInspectRelicScreen.PropertyName._screenTween, ref variant12))
      this._screenTween = ((Variant) ref variant12).As<Tween>();
    Variant variant13;
    if (info.TryGetProperty(NInspectRelicScreen.PropertyName._popupTween, ref variant13))
      this._popupTween = ((Variant) ref variant13).As<Tween>();
    Variant variant14;
    if (info.TryGetProperty(NInspectRelicScreen.PropertyName._popupPosition, ref variant14))
      this._popupPosition = ((Variant) ref variant14).As<Vector2>();
    Variant variant15;
    if (info.TryGetProperty(NInspectRelicScreen.PropertyName._leftButtonX, ref variant15))
      this._leftButtonX = ((Variant) ref variant15).As<float>();
    Variant variant16;
    if (info.TryGetProperty(NInspectRelicScreen.PropertyName._rightButtonX, ref variant16))
      this._rightButtonX = ((Variant) ref variant16).As<float>();
    Variant variant17;
    if (!info.TryGetProperty(NInspectRelicScreen.PropertyName._index, ref variant17))
      return;
    this._index = ((Variant) ref variant17).As<int>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _Input = StringName.op_Implicit(nameof (_Input));
    public static readonly StringName OnRightButtonPressed = StringName.op_Implicit(nameof (OnRightButtonPressed));
    public static readonly StringName OnLeftButtonPressed = StringName.op_Implicit(nameof (OnLeftButtonPressed));
    public static readonly StringName SetRelic = StringName.op_Implicit(nameof (SetRelic));
    public static readonly StringName UpdateRelicDisplay = StringName.op_Implicit(nameof (UpdateRelicDisplay));
    public static readonly StringName SetRarityVisuals = StringName.op_Implicit(nameof (SetRarityVisuals));
    public static readonly StringName Close = StringName.op_Implicit(nameof (Close));
    public static readonly StringName OnBackstopPressed = StringName.op_Implicit(nameof (OnBackstopPressed));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _popup = StringName.op_Implicit(nameof (_popup));
    public static readonly StringName _backstop = StringName.op_Implicit(nameof (_backstop));
    public static readonly StringName _nameLabel = StringName.op_Implicit(nameof (_nameLabel));
    public static readonly StringName _rarityLabel = StringName.op_Implicit(nameof (_rarityLabel));
    public static readonly StringName _description = StringName.op_Implicit(nameof (_description));
    public static readonly StringName _flavor = StringName.op_Implicit(nameof (_flavor));
    public static readonly StringName _relicImage = StringName.op_Implicit(nameof (_relicImage));
    public static readonly StringName _frameHsv = StringName.op_Implicit(nameof (_frameHsv));
    public static readonly StringName _leftButton = StringName.op_Implicit(nameof (_leftButton));
    public static readonly StringName _rightButton = StringName.op_Implicit(nameof (_rightButton));
    public static readonly StringName _hoverTipRect = StringName.op_Implicit(nameof (_hoverTipRect));
    public static readonly StringName _screenTween = StringName.op_Implicit(nameof (_screenTween));
    public static readonly StringName _popupTween = StringName.op_Implicit(nameof (_popupTween));
    public static readonly StringName _popupPosition = StringName.op_Implicit(nameof (_popupPosition));
    public static readonly StringName _leftButtonX = StringName.op_Implicit(nameof (_leftButtonX));
    public static readonly StringName _rightButtonX = StringName.op_Implicit(nameof (_rightButtonX));
    public static readonly StringName _index = StringName.op_Implicit(nameof (_index));
  }

  public class SignalName : Control.SignalName
  {
  }
}
