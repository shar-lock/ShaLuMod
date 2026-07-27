// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.NCardPileScreen
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens;

[ScriptPath("res://src/Core/Nodes/Screens/NCardPileScreen.cs")]
public class NCardPileScreen : Control, ICapstoneScreen, IScreenContext
{
  private ColorRect _background;
  private NCardGrid _grid;
  private NButton _backButton;
  private MegaRichTextLabel _bottomLabel;
  private Tween? _currentTween;
  private string[] _closeHotkeys = Array.Empty<string>();

  private static string ScenePath => SceneHelper.GetScenePath("/screens/card_pile_screen");

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NCardPileScreen.ScenePath);
    }
  }

  public NetScreenType ScreenType => NetScreenType.CardPile;

  public CardPile Pile { get; private set; }

  public override void _Ready()
  {
    this._bottomLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%BottomLabel"));
    switch (this.Pile.Type)
    {
      case PileType.Draw:
        this._bottomLabel.Text = "[center]" + new LocString("gameplay_ui", "DRAW_PILE_INFO").GetFormattedText();
        break;
      case PileType.Discard:
        this._bottomLabel.Text = "[center]" + new LocString("gameplay_ui", "DISCARD_PILE_INFO").GetFormattedText();
        break;
      case PileType.Exhaust:
        this._bottomLabel.Text = "[center]" + new LocString("gameplay_ui", "EXHAUST_PILE_INFO").GetFormattedText();
        break;
      default:
        ((CanvasItem) this._bottomLabel).Visible = false;
        Log.Info("CardPileScreen has no info text.");
        break;
    }
    this._backButton = ((Node) this).GetNode<NButton>(NodePath.op_Implicit("BackButton"));
    ((GodotObject) this._backButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnReturnButtonPressed)), 0U);
    this._backButton.Enable();
    this._grid = ((Node) this).GetNode<NCardGrid>(NodePath.op_Implicit("CardGrid"));
    this.OnPileContentsChanged();
    this._grid.InsetForTopBar();
    this._background = ((Node) this).GetNode<ColorRect>(NodePath.op_Implicit("Background"));
    ((CanvasItem) this._background).Modulate = StsColors.transparentBlack;
    this._currentTween = ((Node) this).CreateTween();
    this._currentTween.TweenProperty((GodotObject) this._background, NodePath.op_Implicit("modulate"), Variant.op_Implicit(StsColors.screenBackdrop), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    ((Node) this).ProcessMode = ((CanvasItem) this).Visible ? (Node.ProcessModeEnum) 0L : (Node.ProcessModeEnum) 4L;
  }

  public override void _EnterTree()
  {
    ((Node) this)._EnterTree();
    this.Pile.ContentsChanged += new Action(this.OnPileContentsChanged);
    foreach (string closeHotkey in this._closeHotkeys)
      NHotkeyManager.Instance.PushHotkeyReleasedBinding(closeHotkey, new Action(NCapstoneContainer.Instance.Close));
  }

  public override void _ExitTree()
  {
    ((Node) this)._ExitTree();
    this.Pile.ContentsChanged -= new Action(this.OnPileContentsChanged);
    foreach (string closeHotkey in this._closeHotkeys)
      NHotkeyManager.Instance.RemoveHotkeyReleasedBinding(closeHotkey, new Action(NCapstoneContainer.Instance.Close));
  }

  public static NCardPileScreen ShowScreen(CardPile pile, string[] closeHotkeys)
  {
    NDebugAudioManager.Instance?.Play("map_open.mp3");
    NCardPileScreen screen = PreloadManager.Cache.GetScene(NCardPileScreen.ScenePath).Instantiate<NCardPileScreen>((PackedScene.GenEditState) 0L);
    ((Node) screen).Name = StringName.op_Implicit($"{nameof (NCardPileScreen)}-{pile.Type}");
    screen.Pile = pile;
    screen._closeHotkeys = closeHotkeys;
    NCapstoneContainer.Instance.Open((ICapstoneScreen) screen);
    return screen;
  }

  private void OnPileContentsChanged()
  {
    List<CardModel> list = this.Pile.Cards.ToList<CardModel>();
    if (this.Pile.Type == PileType.Draw)
      list.Sort((Comparison<CardModel>) ((c1, c2) => c1.Rarity != c2.Rarity ? c1.Rarity.CompareTo((object) c2.Rarity) : string.Compare(c1.Id.Entry, c2.Id.Entry, StringComparison.Ordinal)));
    NCardGrid grid = this._grid;
    List<CardModel> cardsToDisplay = list;
    int type = (int) this.Pile.Type;
    int capacity = 1;
    List<SortingOrders> sortingPriority = new List<SortingOrders>(capacity);
    CollectionsMarshal.SetCount<SortingOrders>(sortingPriority, capacity);
    CollectionsMarshal.AsSpan<SortingOrders>(sortingPriority)[0] = SortingOrders.Ascending;
    grid.SetCards((IReadOnlyList<CardModel>) cardsToDisplay, (PileType) type, sortingPriority);
  }

  private void OnReturnButtonPressed(NButton _) => NCapstoneContainer.Instance.Close();

  public void AfterCapstoneOpened() => ((CanvasItem) this).Visible = true;

  public void AfterCapstoneClosed()
  {
    ((CanvasItem) this).Visible = false;
    ((Node) this).QueueFreeSafely();
  }

  public bool UseSharedBackstop => true;

  public Control? DefaultFocusedControl => this._grid.DefaultFocusedControl;

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(7)
    {
      new MethodInfo(NCardPileScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardPileScreen.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardPileScreen.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardPileScreen.MethodName.OnPileContentsChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardPileScreen.MethodName.OnReturnButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCardPileScreen.MethodName.AfterCapstoneOpened, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardPileScreen.MethodName.AfterCapstoneClosed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCardPileScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardPileScreen.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardPileScreen.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardPileScreen.MethodName.OnPileContentsChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPileContentsChanged();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardPileScreen.MethodName.OnReturnButtonPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnReturnButtonPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardPileScreen.MethodName.AfterCapstoneOpened) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AfterCapstoneOpened();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCardPileScreen.MethodName.AfterCapstoneClosed) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.AfterCapstoneClosed();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCardPileScreen.MethodName._Ready) || StringName.op_Equality(ref method, NCardPileScreen.MethodName._EnterTree) || StringName.op_Equality(ref method, NCardPileScreen.MethodName._ExitTree) || StringName.op_Equality(ref method, NCardPileScreen.MethodName.OnPileContentsChanged) || StringName.op_Equality(ref method, NCardPileScreen.MethodName.OnReturnButtonPressed) || StringName.op_Equality(ref method, NCardPileScreen.MethodName.AfterCapstoneOpened) || StringName.op_Equality(ref method, NCardPileScreen.MethodName.AfterCapstoneClosed) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardPileScreen.PropertyName._background))
    {
      this._background = VariantUtils.ConvertTo<ColorRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardPileScreen.PropertyName._grid))
    {
      this._grid = VariantUtils.ConvertTo<NCardGrid>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardPileScreen.PropertyName._backButton))
    {
      this._backButton = VariantUtils.ConvertTo<NButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardPileScreen.PropertyName._bottomLabel))
    {
      this._bottomLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardPileScreen.PropertyName._currentTween))
    {
      this._currentTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardPileScreen.PropertyName._closeHotkeys))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._closeHotkeys = VariantUtils.ConvertTo<string[]>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardPileScreen.PropertyName.ScreenType))
    {
      ref godot_variant local = ref value;
      NetScreenType screenType = this.ScreenType;
      godot_variant from = VariantUtils.CreateFrom<NetScreenType>(ref screenType);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardPileScreen.PropertyName.UseSharedBackstop))
    {
      ref godot_variant local = ref value;
      bool useSharedBackstop = this.UseSharedBackstop;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref useSharedBackstop);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardPileScreen.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardPileScreen.PropertyName._background))
    {
      value = VariantUtils.CreateFrom<ColorRect>(ref this._background);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardPileScreen.PropertyName._grid))
    {
      value = VariantUtils.CreateFrom<NCardGrid>(ref this._grid);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardPileScreen.PropertyName._backButton))
    {
      value = VariantUtils.CreateFrom<NButton>(ref this._backButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardPileScreen.PropertyName._bottomLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._bottomLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardPileScreen.PropertyName._currentTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._currentTween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardPileScreen.PropertyName._closeHotkeys))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<string[]>(ref this._closeHotkeys);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCardPileScreen.PropertyName._background, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardPileScreen.PropertyName._grid, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardPileScreen.PropertyName._backButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardPileScreen.PropertyName._bottomLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardPileScreen.PropertyName._currentTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 34L, NCardPileScreen.PropertyName._closeHotkeys, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NCardPileScreen.PropertyName.ScreenType, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCardPileScreen.PropertyName.UseSharedBackstop, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardPileScreen.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NCardPileScreen.PropertyName._background, Variant.From<ColorRect>(ref this._background));
    info.AddProperty(NCardPileScreen.PropertyName._grid, Variant.From<NCardGrid>(ref this._grid));
    info.AddProperty(NCardPileScreen.PropertyName._backButton, Variant.From<NButton>(ref this._backButton));
    info.AddProperty(NCardPileScreen.PropertyName._bottomLabel, Variant.From<MegaRichTextLabel>(ref this._bottomLabel));
    info.AddProperty(NCardPileScreen.PropertyName._currentTween, Variant.From<Tween>(ref this._currentTween));
    info.AddProperty(NCardPileScreen.PropertyName._closeHotkeys, Variant.From<string[]>(ref this._closeHotkeys));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCardPileScreen.PropertyName._background, ref variant1))
      this._background = ((Variant) ref variant1).As<ColorRect>();
    Variant variant2;
    if (info.TryGetProperty(NCardPileScreen.PropertyName._grid, ref variant2))
      this._grid = ((Variant) ref variant2).As<NCardGrid>();
    Variant variant3;
    if (info.TryGetProperty(NCardPileScreen.PropertyName._backButton, ref variant3))
      this._backButton = ((Variant) ref variant3).As<NButton>();
    Variant variant4;
    if (info.TryGetProperty(NCardPileScreen.PropertyName._bottomLabel, ref variant4))
      this._bottomLabel = ((Variant) ref variant4).As<MegaRichTextLabel>();
    Variant variant5;
    if (info.TryGetProperty(NCardPileScreen.PropertyName._currentTween, ref variant5))
      this._currentTween = ((Variant) ref variant5).As<Tween>();
    Variant variant6;
    if (!info.TryGetProperty(NCardPileScreen.PropertyName._closeHotkeys, ref variant6))
      return;
    this._closeHotkeys = ((Variant) ref variant6).As<string[]>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName OnPileContentsChanged = StringName.op_Implicit(nameof (OnPileContentsChanged));
    public static readonly StringName OnReturnButtonPressed = StringName.op_Implicit(nameof (OnReturnButtonPressed));
    public static readonly StringName AfterCapstoneOpened = StringName.op_Implicit(nameof (AfterCapstoneOpened));
    public static readonly StringName AfterCapstoneClosed = StringName.op_Implicit(nameof (AfterCapstoneClosed));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName ScreenType = StringName.op_Implicit(nameof (ScreenType));
    public static readonly StringName UseSharedBackstop = StringName.op_Implicit(nameof (UseSharedBackstop));
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _background = StringName.op_Implicit(nameof (_background));
    public static readonly StringName _grid = StringName.op_Implicit(nameof (_grid));
    public static readonly StringName _backButton = StringName.op_Implicit(nameof (_backButton));
    public static readonly StringName _bottomLabel = StringName.op_Implicit(nameof (_bottomLabel));
    public static readonly StringName _currentTween = StringName.op_Implicit(nameof (_currentTween));
    public static readonly StringName _closeHotkeys = StringName.op_Implicit(nameof (_closeHotkeys));
  }

  public class SignalName : Control.SignalName
  {
  }
}
