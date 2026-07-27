// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Ftue.NCombatRulesFtue
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Debug;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Ftue;

[ScriptPath("res://src/Core/Nodes/Ftue/NCombatRulesFtue.cs")]
public class NCombatRulesFtue : NFtue
{
  [Export]
  private Texture2D _image1;
  [Export]
  private Texture2D _image2;
  [Export]
  private Texture2D _image3;
  public const string id = "combat_rules_ftue";
  private static readonly string _scenePath = SceneHelper.GetScenePath("ftue/combat_rules_ftue");
  private NButton _prevButton;
  private NButton _nextButton;
  private MegaLabel _pageCount;
  private TextureRect _image;
  private MegaRichTextLabel _bodyText;
  private MegaLabel _header;
  private int _currentPage = 1;
  private const int _totalPages = 3;
  private Vector2 _imagePosition;
  private Vector2 _textPosition;
  private Tween? _pageTurnTween;
  private const double _textTweenSpeed = 0.6;
  private static readonly Vector2 _imageAnimOffset = new Vector2(200f, 0.0f);

  public override void _Ready()
  {
    this._image = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("Image"));
    this._bodyText = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%Description"));
    this._pageCount = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("PageCount"));
    this._header = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("Header"));
    this._prevButton = ((Node) this).GetNode<NButton>(NodePath.op_Implicit("LeftArrow"));
    this._nextButton = ((Node) this).GetNode<NButton>(NodePath.op_Implicit("RightArrow"));
    ((CanvasItem) this._image).Modulate = Colors.Transparent;
    ((CanvasItem) this._bodyText).Modulate = Colors.Transparent;
    ((GodotObject) this._prevButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.ToggleLeft)), 0U);
    ((GodotObject) this._nextButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.ToggleRight)), 0U);
    ((CanvasItem) this._prevButton).Visible = false;
    this._prevButton.Disable();
    ((CanvasItem) this._nextButton).Visible = false;
    this._nextButton.Disable();
    ((CanvasItem) this._pageCount).Visible = false;
    ((CanvasItem) this._header).Visible = false;
  }

  public void Start()
  {
    NModalContainer.Instance?.ShowBackstop();
    this._currentPage = 1;
    this._imagePosition = ((Control) this._image).Position;
    this._bodyText.Text = new LocString("ftues", "TUTORIAL_FTUE_BODY_1").GetFormattedText();
    ((CanvasItem) this._bodyText).Modulate = StsColors.transparentWhite;
    this._textPosition = ((Control) this._bodyText).Position;
    LocString locString = new LocString("ftues", "COMBAT_BASICS_FTUE_PAGE_COUNT");
    locString.Add("totalPages", 3M);
    locString.Add("currentPage", (Decimal) this._currentPage);
    this._pageCount.SetTextAutoSize(locString.GetFormattedText());
    this._header.SetTextAutoSize(new LocString("ftues", "COMBAT_BASICS_FTUE_HEADER").GetFormattedText());
    ((CanvasItem) this._nextButton).Visible = true;
    this._nextButton.Enable();
    ((CanvasItem) this._pageCount).Visible = true;
    ((CanvasItem) this._header).Visible = true;
    this._pageTurnTween = ((Node) this).CreateTween().SetParallel(true);
    this._pageTurnTween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L).From(Variant.op_Implicit(0.0f));
    this._pageTurnTween.TweenProperty((GodotObject) this._bodyText, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 0L);
    this._pageTurnTween.TweenProperty((GodotObject) this._bodyText, NodePath.op_Implicit("visible_ratio"), Variant.op_Implicit(1f), 0.6).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 1L).From(Variant.op_Implicit(0.0f));
  }

  public static NCombatRulesFtue? Create()
  {
    return TestMode.IsOn ? (NCombatRulesFtue) null : PreloadManager.Cache.GetScene(NCombatRulesFtue._scenePath).Instantiate<NCombatRulesFtue>((PackedScene.GenEditState) 0L);
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
    if (inputEvent.IsActionPressed(MegaInput.left, false, false) && this._prevButton.IsEnabled)
      this.ToggleLeft(this._prevButton);
    if (!inputEvent.IsActionPressed(MegaInput.right, false, false) || !this._nextButton.IsEnabled)
      return;
    this.ToggleRight(this._nextButton);
  }

  private void ToggleLeft(NButton _)
  {
    --this._currentPage;
    switch (this._currentPage)
    {
      case 1:
        ((CanvasItem) this._prevButton).Visible = false;
        this._prevButton.Disable();
        this._bodyText.SetTextAutoSize(new LocString("ftues", "TUTORIAL_FTUE_BODY_1").GetFormattedText());
        this._image.Texture = this._image1;
        break;
      case 2:
        this._bodyText.SetTextAutoSize(new LocString("ftues", "TUTORIAL_FTUE_BODY_2").GetFormattedText());
        this._image.Texture = this._image2;
        break;
    }
    LocString locString = new LocString("ftues", "COMBAT_BASICS_FTUE_PAGE_COUNT");
    locString.Add("totalPages", 3M);
    locString.Add("currentPage", (Decimal) this._currentPage);
    this._pageCount.SetTextAutoSize(locString.GetFormattedText());
    this._pageTurnTween?.Kill();
    this._pageTurnTween = ((Node) this).CreateTween().SetParallel(true);
    this._pageTurnTween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L).From(Variant.op_Implicit(0.5f));
    this._pageTurnTween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("position"), Variant.op_Implicit(this._imagePosition), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L).From(Variant.op_Implicit(Vector2.op_Subtraction(this._imagePosition, NCombatRulesFtue._imageAnimOffset)));
    this._pageTurnTween.TweenProperty((GodotObject) this._bodyText, NodePath.op_Implicit("position"), Variant.op_Implicit(this._textPosition), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L).From(Variant.op_Implicit(Vector2.op_Subtraction(this._textPosition, NCombatRulesFtue._imageAnimOffset)));
    this._pageTurnTween.TweenProperty((GodotObject) this._bodyText, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.6).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 0L).From(Variant.op_Implicit(0.0f));
    this._pageTurnTween.TweenProperty((GodotObject) this._bodyText, NodePath.op_Implicit("visible_ratio"), Variant.op_Implicit(1f), 0.6).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 1L).From(Variant.op_Implicit(0.0f));
  }

  private void ToggleRight(NButton _)
  {
    if (this._currentPage == 3)
    {
      this._pageTurnTween?.Kill();
      SaveManager.Instance.MarkFtueAsComplete("combat_rules_ftue");
      ((Node) NCombatRoom.Instance).AddChildSafely((Node) NCombatStartBanner.Create());
      this.CloseFtue();
    }
    else
    {
      ++this._currentPage;
      switch (this._currentPage)
      {
        case 2:
          ((CanvasItem) this._prevButton).Visible = true;
          this._prevButton.Enable();
          this._bodyText.SetTextAutoSize(new LocString("ftues", "TUTORIAL_FTUE_BODY_2").GetFormattedText());
          this._image.Texture = this._image2;
          break;
        case 3:
          this._bodyText.SetTextAutoSize(new LocString("ftues", "TUTORIAL_FTUE_BODY_3").GetFormattedText());
          this._image.Texture = this._image3;
          break;
      }
      LocString locString = new LocString("ftues", "COMBAT_BASICS_FTUE_PAGE_COUNT");
      locString.Add("totalPages", 3M);
      locString.Add("currentPage", (Decimal) this._currentPage);
      this._pageCount.SetTextAutoSize(locString.GetFormattedText());
      this._pageTurnTween?.Kill();
      this._pageTurnTween = ((Node) this).CreateTween().SetParallel(true);
      this._pageTurnTween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L).From(Variant.op_Implicit(0.5f));
      this._pageTurnTween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("position"), Variant.op_Implicit(this._imagePosition), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L).From(Variant.op_Implicit(Vector2.op_Addition(this._imagePosition, NCombatRulesFtue._imageAnimOffset)));
      this._pageTurnTween.TweenProperty((GodotObject) this._bodyText, NodePath.op_Implicit("position"), Variant.op_Implicit(this._textPosition), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L).From(Variant.op_Implicit(Vector2.op_Addition(this._textPosition, NCombatRulesFtue._imageAnimOffset)));
      this._pageTurnTween.TweenProperty((GodotObject) this._bodyText, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.6).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 0L).From(Variant.op_Implicit(0.0f));
      this._pageTurnTween.TweenProperty((GodotObject) this._bodyText, NodePath.op_Implicit("visible_ratio"), Variant.op_Implicit(1f), 0.6).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 1L).From(Variant.op_Implicit(0.0f));
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NCombatRulesFtue.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatRulesFtue.MethodName.Start, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatRulesFtue.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatRulesFtue.MethodName._Input, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCombatRulesFtue.MethodName.ToggleLeft, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCombatRulesFtue.MethodName.ToggleRight, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NCombatRulesFtue.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatRulesFtue.MethodName.Start) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Start();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatRulesFtue.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NCombatRulesFtue ncombatRulesFtue = NCombatRulesFtue.Create();
      ret = VariantUtils.CreateFrom<NCombatRulesFtue>(ref ncombatRulesFtue);
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatRulesFtue.MethodName._Input) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Input(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatRulesFtue.MethodName.ToggleLeft) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ToggleLeft(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCombatRulesFtue.MethodName.ToggleRight) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.ToggleRight(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCombatRulesFtue.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NCombatRulesFtue ncombatRulesFtue = NCombatRulesFtue.Create();
      ret = VariantUtils.CreateFrom<NCombatRulesFtue>(ref ncombatRulesFtue);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCombatRulesFtue.MethodName._Ready) || StringName.op_Equality(ref method, NCombatRulesFtue.MethodName.Start) || StringName.op_Equality(ref method, NCombatRulesFtue.MethodName.Create) || StringName.op_Equality(ref method, NCombatRulesFtue.MethodName._Input) || StringName.op_Equality(ref method, NCombatRulesFtue.MethodName.ToggleLeft) || StringName.op_Equality(ref method, NCombatRulesFtue.MethodName.ToggleRight) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCombatRulesFtue.PropertyName._image1))
    {
      this._image1 = VariantUtils.ConvertTo<Texture2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRulesFtue.PropertyName._image2))
    {
      this._image2 = VariantUtils.ConvertTo<Texture2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRulesFtue.PropertyName._image3))
    {
      this._image3 = VariantUtils.ConvertTo<Texture2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRulesFtue.PropertyName._prevButton))
    {
      this._prevButton = VariantUtils.ConvertTo<NButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRulesFtue.PropertyName._nextButton))
    {
      this._nextButton = VariantUtils.ConvertTo<NButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRulesFtue.PropertyName._pageCount))
    {
      this._pageCount = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRulesFtue.PropertyName._image))
    {
      this._image = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRulesFtue.PropertyName._bodyText))
    {
      this._bodyText = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRulesFtue.PropertyName._header))
    {
      this._header = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRulesFtue.PropertyName._currentPage))
    {
      this._currentPage = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRulesFtue.PropertyName._imagePosition))
    {
      this._imagePosition = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRulesFtue.PropertyName._textPosition))
    {
      this._textPosition = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCombatRulesFtue.PropertyName._pageTurnTween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._pageTurnTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCombatRulesFtue.PropertyName._image1))
    {
      value = VariantUtils.CreateFrom<Texture2D>(ref this._image1);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRulesFtue.PropertyName._image2))
    {
      value = VariantUtils.CreateFrom<Texture2D>(ref this._image2);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRulesFtue.PropertyName._image3))
    {
      value = VariantUtils.CreateFrom<Texture2D>(ref this._image3);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRulesFtue.PropertyName._prevButton))
    {
      value = VariantUtils.CreateFrom<NButton>(ref this._prevButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRulesFtue.PropertyName._nextButton))
    {
      value = VariantUtils.CreateFrom<NButton>(ref this._nextButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRulesFtue.PropertyName._pageCount))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._pageCount);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRulesFtue.PropertyName._image))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._image);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRulesFtue.PropertyName._bodyText))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._bodyText);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRulesFtue.PropertyName._header))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._header);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRulesFtue.PropertyName._currentPage))
    {
      value = VariantUtils.CreateFrom<int>(ref this._currentPage);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRulesFtue.PropertyName._imagePosition))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._imagePosition);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRulesFtue.PropertyName._textPosition))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._textPosition);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCombatRulesFtue.PropertyName._pageTurnTween))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Tween>(ref this._pageTurnTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCombatRulesFtue.PropertyName._image1, (PropertyHint) 17L, "Texture2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NCombatRulesFtue.PropertyName._image2, (PropertyHint) 17L, "Texture2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NCombatRulesFtue.PropertyName._image3, (PropertyHint) 17L, "Texture2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NCombatRulesFtue.PropertyName._prevButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatRulesFtue.PropertyName._nextButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatRulesFtue.PropertyName._pageCount, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatRulesFtue.PropertyName._image, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatRulesFtue.PropertyName._bodyText, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatRulesFtue.PropertyName._header, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NCombatRulesFtue.PropertyName._currentPage, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NCombatRulesFtue.PropertyName._imagePosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NCombatRulesFtue.PropertyName._textPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatRulesFtue.PropertyName._pageTurnTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NCombatRulesFtue.PropertyName._image1, Variant.From<Texture2D>(ref this._image1));
    info.AddProperty(NCombatRulesFtue.PropertyName._image2, Variant.From<Texture2D>(ref this._image2));
    info.AddProperty(NCombatRulesFtue.PropertyName._image3, Variant.From<Texture2D>(ref this._image3));
    info.AddProperty(NCombatRulesFtue.PropertyName._prevButton, Variant.From<NButton>(ref this._prevButton));
    info.AddProperty(NCombatRulesFtue.PropertyName._nextButton, Variant.From<NButton>(ref this._nextButton));
    info.AddProperty(NCombatRulesFtue.PropertyName._pageCount, Variant.From<MegaLabel>(ref this._pageCount));
    info.AddProperty(NCombatRulesFtue.PropertyName._image, Variant.From<TextureRect>(ref this._image));
    info.AddProperty(NCombatRulesFtue.PropertyName._bodyText, Variant.From<MegaRichTextLabel>(ref this._bodyText));
    info.AddProperty(NCombatRulesFtue.PropertyName._header, Variant.From<MegaLabel>(ref this._header));
    info.AddProperty(NCombatRulesFtue.PropertyName._currentPage, Variant.From<int>(ref this._currentPage));
    info.AddProperty(NCombatRulesFtue.PropertyName._imagePosition, Variant.From<Vector2>(ref this._imagePosition));
    info.AddProperty(NCombatRulesFtue.PropertyName._textPosition, Variant.From<Vector2>(ref this._textPosition));
    info.AddProperty(NCombatRulesFtue.PropertyName._pageTurnTween, Variant.From<Tween>(ref this._pageTurnTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCombatRulesFtue.PropertyName._image1, ref variant1))
      this._image1 = ((Variant) ref variant1).As<Texture2D>();
    Variant variant2;
    if (info.TryGetProperty(NCombatRulesFtue.PropertyName._image2, ref variant2))
      this._image2 = ((Variant) ref variant2).As<Texture2D>();
    Variant variant3;
    if (info.TryGetProperty(NCombatRulesFtue.PropertyName._image3, ref variant3))
      this._image3 = ((Variant) ref variant3).As<Texture2D>();
    Variant variant4;
    if (info.TryGetProperty(NCombatRulesFtue.PropertyName._prevButton, ref variant4))
      this._prevButton = ((Variant) ref variant4).As<NButton>();
    Variant variant5;
    if (info.TryGetProperty(NCombatRulesFtue.PropertyName._nextButton, ref variant5))
      this._nextButton = ((Variant) ref variant5).As<NButton>();
    Variant variant6;
    if (info.TryGetProperty(NCombatRulesFtue.PropertyName._pageCount, ref variant6))
      this._pageCount = ((Variant) ref variant6).As<MegaLabel>();
    Variant variant7;
    if (info.TryGetProperty(NCombatRulesFtue.PropertyName._image, ref variant7))
      this._image = ((Variant) ref variant7).As<TextureRect>();
    Variant variant8;
    if (info.TryGetProperty(NCombatRulesFtue.PropertyName._bodyText, ref variant8))
      this._bodyText = ((Variant) ref variant8).As<MegaRichTextLabel>();
    Variant variant9;
    if (info.TryGetProperty(NCombatRulesFtue.PropertyName._header, ref variant9))
      this._header = ((Variant) ref variant9).As<MegaLabel>();
    Variant variant10;
    if (info.TryGetProperty(NCombatRulesFtue.PropertyName._currentPage, ref variant10))
      this._currentPage = ((Variant) ref variant10).As<int>();
    Variant variant11;
    if (info.TryGetProperty(NCombatRulesFtue.PropertyName._imagePosition, ref variant11))
      this._imagePosition = ((Variant) ref variant11).As<Vector2>();
    Variant variant12;
    if (info.TryGetProperty(NCombatRulesFtue.PropertyName._textPosition, ref variant12))
      this._textPosition = ((Variant) ref variant12).As<Vector2>();
    Variant variant13;
    if (!info.TryGetProperty(NCombatRulesFtue.PropertyName._pageTurnTween, ref variant13))
      return;
    this._pageTurnTween = ((Variant) ref variant13).As<Tween>();
  }

  public new class MethodName : NFtue.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName Start = StringName.op_Implicit(nameof (Start));
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Input = StringName.op_Implicit(nameof (_Input));
    public static readonly StringName ToggleLeft = StringName.op_Implicit(nameof (ToggleLeft));
    public static readonly StringName ToggleRight = StringName.op_Implicit(nameof (ToggleRight));
  }

  public new class PropertyName : NFtue.PropertyName
  {
    public static readonly StringName _image1 = StringName.op_Implicit(nameof (_image1));
    public static readonly StringName _image2 = StringName.op_Implicit(nameof (_image2));
    public static readonly StringName _image3 = StringName.op_Implicit(nameof (_image3));
    public static readonly StringName _prevButton = StringName.op_Implicit(nameof (_prevButton));
    public static readonly StringName _nextButton = StringName.op_Implicit(nameof (_nextButton));
    public static readonly StringName _pageCount = StringName.op_Implicit(nameof (_pageCount));
    public static readonly StringName _image = StringName.op_Implicit(nameof (_image));
    public static readonly StringName _bodyText = StringName.op_Implicit(nameof (_bodyText));
    public static readonly StringName _header = StringName.op_Implicit(nameof (_header));
    public static readonly StringName _currentPage = StringName.op_Implicit(nameof (_currentPage));
    public static readonly StringName _imagePosition = StringName.op_Implicit(nameof (_imagePosition));
    public static readonly StringName _textPosition = StringName.op_Implicit(nameof (_textPosition));
    public static readonly StringName _pageTurnTween = StringName.op_Implicit(nameof (_pageTurnTween));
  }

  public new class SignalName : NFtue.SignalName
  {
  }
}
