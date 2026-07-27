// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.NChooseARelicSelection
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Relics;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens;

[ScriptPath("res://src/Core/Nodes/Screens/NChooseARelicSelection.cs")]
public class NChooseARelicSelection : Control, IOverlayScreen, IScreenContext
{
  private const float _relicXSpacing = 200f;
  private NCommonBanner _banner;
  private Control _relicRow;
  private NChoiceSelectionSkipButton _skipButton;
  private readonly TaskCompletionSource<IEnumerable<RelicModel>> _completionSource = new TaskCompletionSource<IEnumerable<RelicModel>>();
  private bool _screenComplete;
  private bool _relicSelected;
  private Tween? _cardTween;
  private Tween? _fadeTween;
  private IReadOnlyList<RelicModel> _relics;

  public NetScreenType ScreenType => NetScreenType.Rewards;

  private static string ScenePath
  {
    get => SceneHelper.GetScenePath("screens/choose_a_relic_selection_screen");
  }

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NChooseARelicSelection.ScenePath);
    }
  }

  public static NChooseARelicSelection? ShowScreen(IReadOnlyList<RelicModel> relics)
  {
    if (TestMode.IsOn)
      return (NChooseARelicSelection) null;
    NChooseARelicSelection screen = PreloadManager.Cache.GetScene(NChooseARelicSelection.ScenePath).Instantiate<NChooseARelicSelection>((PackedScene.GenEditState) 0L);
    ((Node) screen).Name = StringName.op_Implicit("NChooseACardSelectionScreen");
    screen._relics = relics;
    NOverlayStack.Instance.Push((IOverlayScreen) screen);
    return screen;
  }

  public override void _Ready()
  {
    this._banner = ((Node) this).GetNode<NCommonBanner>(NodePath.op_Implicit("Banner"));
    this._banner.label.SetTextAutoSize(new LocString("gameplay_ui", "CHOOSE_RELIC_HEADER").GetRawText());
    this._banner.AnimateIn();
    this._relicRow = ((Node) this).GetNode<Control>(NodePath.op_Implicit("RelicRow"));
    Vector2 vector2 = Vector2.op_Multiply(Vector2.op_Multiply(Vector2.op_Multiply(Vector2.Left, (float) (this._relics.Count - 1)), 200f), 0.5f);
    for (int index = 0; index < this._relics.Count; ++index)
    {
      NRelicBasicHolder holder = NRelicBasicHolder.Create(this._relics[index]);
      holder.Scale = Vector2.op_Multiply(Vector2.One, 2f);
      ((Node) this._relicRow).AddChildSafely((Node) holder);
      ((GodotObject) holder).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>((Action<NButton>) (_ => this.SelectHolder(holder))), 0U);
      this._cardTween = ((Node) this).CreateTween().SetParallel(true);
      this._cardTween.TweenProperty((GodotObject) holder, NodePath.op_Implicit("position"), Variant.op_Implicit(Vector2.op_Addition(Vector2.op_Addition(holder.Position, vector2), Vector2.op_Multiply(Vector2.op_Multiply(Vector2.Right, 200f), (float) index))), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
      this._cardTween.TweenProperty((GodotObject) holder, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L).From(Variant.op_Implicit(Colors.Black));
    }
    this._skipButton = ((Node) this).GetNode<NChoiceSelectionSkipButton>(NodePath.op_Implicit("SkipButton"));
    ((GodotObject) this._skipButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnSkipButtonReleased)), 0U);
    this._skipButton.AnimateIn();
    List<NRelicBasicHolder> list = ((IEnumerable) ((Node) this._relicRow).GetChildren(false)).OfType<NRelicBasicHolder>().ToList<NRelicBasicHolder>();
    this._skipButton.FocusNeighborTop = ((Node) ((IEnumerable) ((Node) this._relicRow).GetChildren(false)).OfType<NRelicBasicHolder>().ToList<NRelicBasicHolder>()[list.Count / 2]).GetPath();
    this._skipButton.FocusNeighborBottom = ((Node) this._skipButton).GetPath();
    this._skipButton.FocusNeighborLeft = ((Node) this._skipButton).GetPath();
    this._skipButton.FocusNeighborRight = ((Node) this._skipButton).GetPath();
    for (int index = 0; index < ((Node) this._relicRow).GetChildCount(false); ++index)
    {
      Control child = ((Node) this._relicRow).GetChild<Control>(index, false);
      child.FocusNeighborBottom = ((Node) child).GetPath();
      child.FocusNeighborTop = ((Node) child).GetPath();
      child.FocusNeighborLeft = index > 0 ? ((Node) this._relicRow).GetChild(index - 1, false).GetPath() : ((Node) this._relicRow).GetChild(((Node) this._relicRow).GetChildCount(false) - 1, false).GetPath();
      child.FocusNeighborRight = index < ((Node) this._relicRow).GetChildCount(false) - 1 ? ((Node) this._relicRow).GetChild(index + 1, false).GetPath() : ((Node) this._relicRow).GetChild(0, false).GetPath();
    }
  }

  public override void _ExitTree()
  {
    if (((Task) this._completionSource.Task).IsCompleted)
      return;
    this._completionSource.SetCanceled();
  }

  private void SelectHolder(NRelicBasicHolder relicHolder)
  {
    RelicModel model = relicHolder.Relic.Model;
    this._screenComplete = true;
    this._relicSelected = true;
    this._completionSource.SetResult((IEnumerable<RelicModel>) new RelicModel[1]
    {
      model
    });
  }

  public async Task<IEnumerable<RelicModel>> RelicsSelected()
  {
    IEnumerable<RelicModel> task = await this._completionSource.Task;
    NOverlayStack.Instance.Remove((IOverlayScreen) this);
    return task;
  }

  private void OnSkipButtonReleased(NButton _)
  {
    this._screenComplete = true;
    this._completionSource.SetResult((IEnumerable<RelicModel>) Array.Empty<RelicModel>());
  }

  public void AfterOverlayOpened()
  {
    ((CanvasItem) this).Modulate = Colors.Transparent;
    this._fadeTween?.Kill();
    this._fadeTween = ((Node) this).CreateTween();
    this._fadeTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.2);
  }

  public void AfterOverlayClosed()
  {
    this._fadeTween?.Kill();
    ((Node) this).QueueFreeSafely();
  }

  public void AfterOverlayShown() => ((CanvasItem) this).Visible = true;

  public void AfterOverlayHidden() => ((CanvasItem) this).Visible = false;

  public bool UseSharedBackstop => true;

  public Control DefaultFocusedControl
  {
    get
    {
      List<NRelicBasicHolder> list = ((IEnumerable) ((Node) this._relicRow).GetChildren(false)).OfType<NRelicBasicHolder>().ToList<NRelicBasicHolder>();
      return (Control) list[list.Count / 2];
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(8)
    {
      new MethodInfo(NChooseARelicSelection.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NChooseARelicSelection.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NChooseARelicSelection.MethodName.SelectHolder, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("relicHolder"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NChooseARelicSelection.MethodName.OnSkipButtonReleased, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NChooseARelicSelection.MethodName.AfterOverlayOpened, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NChooseARelicSelection.MethodName.AfterOverlayClosed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NChooseARelicSelection.MethodName.AfterOverlayShown, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NChooseARelicSelection.MethodName.AfterOverlayHidden, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NChooseARelicSelection.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NChooseARelicSelection.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NChooseARelicSelection.MethodName.SelectHolder) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SelectHolder(VariantUtils.ConvertTo<NRelicBasicHolder>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NChooseARelicSelection.MethodName.OnSkipButtonReleased) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnSkipButtonReleased(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NChooseARelicSelection.MethodName.AfterOverlayOpened) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AfterOverlayOpened();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NChooseARelicSelection.MethodName.AfterOverlayClosed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AfterOverlayClosed();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NChooseARelicSelection.MethodName.AfterOverlayShown) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AfterOverlayShown();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NChooseARelicSelection.MethodName.AfterOverlayHidden) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.AfterOverlayHidden();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NChooseARelicSelection.MethodName._Ready) || StringName.op_Equality(ref method, NChooseARelicSelection.MethodName._ExitTree) || StringName.op_Equality(ref method, NChooseARelicSelection.MethodName.SelectHolder) || StringName.op_Equality(ref method, NChooseARelicSelection.MethodName.OnSkipButtonReleased) || StringName.op_Equality(ref method, NChooseARelicSelection.MethodName.AfterOverlayOpened) || StringName.op_Equality(ref method, NChooseARelicSelection.MethodName.AfterOverlayClosed) || StringName.op_Equality(ref method, NChooseARelicSelection.MethodName.AfterOverlayShown) || StringName.op_Equality(ref method, NChooseARelicSelection.MethodName.AfterOverlayHidden) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NChooseARelicSelection.PropertyName._banner))
    {
      this._banner = VariantUtils.ConvertTo<NCommonBanner>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseARelicSelection.PropertyName._relicRow))
    {
      this._relicRow = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseARelicSelection.PropertyName._skipButton))
    {
      this._skipButton = VariantUtils.ConvertTo<NChoiceSelectionSkipButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseARelicSelection.PropertyName._screenComplete))
    {
      this._screenComplete = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseARelicSelection.PropertyName._relicSelected))
    {
      this._relicSelected = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseARelicSelection.PropertyName._cardTween))
    {
      this._cardTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NChooseARelicSelection.PropertyName._fadeTween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._fadeTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NChooseARelicSelection.PropertyName.ScreenType))
    {
      ref godot_variant local = ref value;
      NetScreenType screenType = this.ScreenType;
      godot_variant from = VariantUtils.CreateFrom<NetScreenType>(ref screenType);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseARelicSelection.PropertyName.UseSharedBackstop))
    {
      ref godot_variant local = ref value;
      bool useSharedBackstop = this.UseSharedBackstop;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref useSharedBackstop);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseARelicSelection.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseARelicSelection.PropertyName._banner))
    {
      value = VariantUtils.CreateFrom<NCommonBanner>(ref this._banner);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseARelicSelection.PropertyName._relicRow))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._relicRow);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseARelicSelection.PropertyName._skipButton))
    {
      value = VariantUtils.CreateFrom<NChoiceSelectionSkipButton>(ref this._skipButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseARelicSelection.PropertyName._screenComplete))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._screenComplete);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseARelicSelection.PropertyName._relicSelected))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._relicSelected);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseARelicSelection.PropertyName._cardTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._cardTween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NChooseARelicSelection.PropertyName._fadeTween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._fadeTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NChooseARelicSelection.PropertyName._banner, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NChooseARelicSelection.PropertyName._relicRow, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NChooseARelicSelection.PropertyName._skipButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NChooseARelicSelection.PropertyName._screenComplete, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NChooseARelicSelection.PropertyName._relicSelected, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NChooseARelicSelection.PropertyName._cardTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NChooseARelicSelection.PropertyName._fadeTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NChooseARelicSelection.PropertyName.ScreenType, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NChooseARelicSelection.PropertyName.UseSharedBackstop, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NChooseARelicSelection.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NChooseARelicSelection.PropertyName._banner, Variant.From<NCommonBanner>(ref this._banner));
    info.AddProperty(NChooseARelicSelection.PropertyName._relicRow, Variant.From<Control>(ref this._relicRow));
    info.AddProperty(NChooseARelicSelection.PropertyName._skipButton, Variant.From<NChoiceSelectionSkipButton>(ref this._skipButton));
    info.AddProperty(NChooseARelicSelection.PropertyName._screenComplete, Variant.From<bool>(ref this._screenComplete));
    info.AddProperty(NChooseARelicSelection.PropertyName._relicSelected, Variant.From<bool>(ref this._relicSelected));
    info.AddProperty(NChooseARelicSelection.PropertyName._cardTween, Variant.From<Tween>(ref this._cardTween));
    info.AddProperty(NChooseARelicSelection.PropertyName._fadeTween, Variant.From<Tween>(ref this._fadeTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NChooseARelicSelection.PropertyName._banner, ref variant1))
      this._banner = ((Variant) ref variant1).As<NCommonBanner>();
    Variant variant2;
    if (info.TryGetProperty(NChooseARelicSelection.PropertyName._relicRow, ref variant2))
      this._relicRow = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NChooseARelicSelection.PropertyName._skipButton, ref variant3))
      this._skipButton = ((Variant) ref variant3).As<NChoiceSelectionSkipButton>();
    Variant variant4;
    if (info.TryGetProperty(NChooseARelicSelection.PropertyName._screenComplete, ref variant4))
      this._screenComplete = ((Variant) ref variant4).As<bool>();
    Variant variant5;
    if (info.TryGetProperty(NChooseARelicSelection.PropertyName._relicSelected, ref variant5))
      this._relicSelected = ((Variant) ref variant5).As<bool>();
    Variant variant6;
    if (info.TryGetProperty(NChooseARelicSelection.PropertyName._cardTween, ref variant6))
      this._cardTween = ((Variant) ref variant6).As<Tween>();
    Variant variant7;
    if (!info.TryGetProperty(NChooseARelicSelection.PropertyName._fadeTween, ref variant7))
      return;
    this._fadeTween = ((Variant) ref variant7).As<Tween>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName SelectHolder = StringName.op_Implicit(nameof (SelectHolder));
    public static readonly StringName OnSkipButtonReleased = StringName.op_Implicit(nameof (OnSkipButtonReleased));
    public static readonly StringName AfterOverlayOpened = StringName.op_Implicit(nameof (AfterOverlayOpened));
    public static readonly StringName AfterOverlayClosed = StringName.op_Implicit(nameof (AfterOverlayClosed));
    public static readonly StringName AfterOverlayShown = StringName.op_Implicit(nameof (AfterOverlayShown));
    public static readonly StringName AfterOverlayHidden = StringName.op_Implicit(nameof (AfterOverlayHidden));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName ScreenType = StringName.op_Implicit(nameof (ScreenType));
    public static readonly StringName UseSharedBackstop = StringName.op_Implicit(nameof (UseSharedBackstop));
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _banner = StringName.op_Implicit(nameof (_banner));
    public static readonly StringName _relicRow = StringName.op_Implicit(nameof (_relicRow));
    public static readonly StringName _skipButton = StringName.op_Implicit(nameof (_skipButton));
    public static readonly StringName _screenComplete = StringName.op_Implicit(nameof (_screenComplete));
    public static readonly StringName _relicSelected = StringName.op_Implicit(nameof (_relicSelected));
    public static readonly StringName _cardTween = StringName.op_Implicit(nameof (_cardTween));
    public static readonly StringName _fadeTween = StringName.op_Implicit(nameof (_fadeTween));
  }

  public class SignalName : Control.SignalName
  {
  }
}
