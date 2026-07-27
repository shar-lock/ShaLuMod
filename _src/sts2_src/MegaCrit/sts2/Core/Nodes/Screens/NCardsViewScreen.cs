// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.NCardsViewScreen
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens;

[ScriptPath("res://src/Core/Nodes/Screens/NCardsViewScreen.cs")]
public abstract class NCardsViewScreen : Control, ICapstoneScreen, IScreenContext
{
  private ColorRect _background;
  protected NCardGrid _grid;
  protected NButton _backButton;
  private NTickbox _showUpgrades;
  private RichTextLabel _bottomLabel;
  protected List<CardModel> _cards;
  protected LocString _infoText;

  public abstract NetScreenType ScreenType { get; }

  public override void _Ready()
  {
    if (((object) this).GetType() != typeof (NCardsViewScreen))
    {
      Log.Error($"{((object) this).GetType()}");
      throw new InvalidOperationException("Don't call base._Ready()! Call ConnectSignals() instead.");
    }
    this.ConnectSignals();
  }

  protected virtual void ConnectSignals()
  {
    this._bottomLabel = ((Node) this).GetNode<RichTextLabel>(NodePath.op_Implicit("%BottomLabel"));
    this._backButton = ((Node) this).GetNode<NButton>(NodePath.op_Implicit("BackButton"));
    ((GodotObject) this._backButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnReturnButtonPressed)), 0U);
    this._backButton.Enable();
    this._grid = ((Node) this).GetNode<NCardGrid>(NodePath.op_Implicit("CardGrid"));
    this._grid.InsetForTopBar();
    this._bottomLabel.Text = this._infoText.GetFormattedText();
    this._showUpgrades = ((Node) this).GetNode<NTickbox>(NodePath.op_Implicit("%Upgrades"));
    ((GodotObject) this._showUpgrades).Connect(NTickbox.SignalName.Toggled, Callable.From<NTickbox>(new Action<NTickbox>(this.ToggleShowUpgrades)), 0U);
    this.OnControllerStateUpdated();
    ((GodotObject) NControllerManager.Instance).Connect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.OnControllerStateUpdated)), 0U);
    ((GodotObject) NControllerManager.Instance).Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.OnControllerStateUpdated)), 0U);
    ((Node) this).ProcessMode = ((CanvasItem) this).Visible ? (Node.ProcessModeEnum) 0L : (Node.ProcessModeEnum) 4L;
  }

  protected void ShowCardDetail(CardModel cardModel)
  {
    this._backButton.Disable();
    List<CardModel> list = this._grid.CurrentlyDisplayedCards.ToList<CardModel>();
    NInspectCardScreen inspectCardScreen = NGame.Instance.GetInspectCardScreen();
    inspectCardScreen.Open(list, list.IndexOf(cardModel), this._grid.IsShowingUpgrades);
    ((GodotObject) inspectCardScreen).Connect(CanvasItem.SignalName.VisibilityChanged, Callable.From((Action) (() =>
    {
      if (((CanvasItem) inspectCardScreen).Visible)
        return;
      this.OnInspectCardHidden();
    })), 4U);
  }

  protected virtual void OnInspectCardHidden() => this._backButton.Enable();

  private void ToggleShowUpgrades(NTickbox tickbox)
  {
    this._grid.IsShowingUpgrades = tickbox.IsTicked;
  }

  protected void OnReturnButtonPressed(NButton _) => NCapstoneContainer.Instance.Close();

  public virtual void AfterCapstoneOpened() => this._showUpgrades.IsTicked = false;

  public virtual void AfterCapstoneClosed()
  {
    ((CanvasItem) this).Visible = false;
    ((Node) this).QueueFreeSafely();
  }

  public Control? DefaultFocusedControl => this._grid.DefaultFocusedControl;

  public Control? FocusedControlFromTopBar => this._grid.FocusedControlFromTopBar;

  public bool UseSharedBackstop => true;

  private void OnControllerStateUpdated()
  {
    ((CanvasItem) this._showUpgrades).Visible = !NControllerManager.Instance.IsUsingController;
    if (!NControllerManager.Instance.IsUsingController)
      return;
    this._showUpgrades.IsTicked = false;
    this.ToggleShowUpgrades(this._showUpgrades);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(8)
    {
      new MethodInfo(NCardsViewScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardsViewScreen.MethodName.ConnectSignals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardsViewScreen.MethodName.OnInspectCardHidden, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardsViewScreen.MethodName.ToggleShowUpgrades, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("tickbox"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCardsViewScreen.MethodName.OnReturnButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCardsViewScreen.MethodName.AfterCapstoneOpened, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardsViewScreen.MethodName.AfterCapstoneClosed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardsViewScreen.MethodName.OnControllerStateUpdated, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCardsViewScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardsViewScreen.MethodName.ConnectSignals) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ConnectSignals();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardsViewScreen.MethodName.OnInspectCardHidden) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnInspectCardHidden();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardsViewScreen.MethodName.ToggleShowUpgrades) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ToggleShowUpgrades(VariantUtils.ConvertTo<NTickbox>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardsViewScreen.MethodName.OnReturnButtonPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnReturnButtonPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardsViewScreen.MethodName.AfterCapstoneOpened) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AfterCapstoneOpened();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardsViewScreen.MethodName.AfterCapstoneClosed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AfterCapstoneClosed();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCardsViewScreen.MethodName.OnControllerStateUpdated) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnControllerStateUpdated();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCardsViewScreen.MethodName._Ready) || StringName.op_Equality(ref method, NCardsViewScreen.MethodName.ConnectSignals) || StringName.op_Equality(ref method, NCardsViewScreen.MethodName.OnInspectCardHidden) || StringName.op_Equality(ref method, NCardsViewScreen.MethodName.ToggleShowUpgrades) || StringName.op_Equality(ref method, NCardsViewScreen.MethodName.OnReturnButtonPressed) || StringName.op_Equality(ref method, NCardsViewScreen.MethodName.AfterCapstoneOpened) || StringName.op_Equality(ref method, NCardsViewScreen.MethodName.AfterCapstoneClosed) || StringName.op_Equality(ref method, NCardsViewScreen.MethodName.OnControllerStateUpdated) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardsViewScreen.PropertyName._background))
    {
      this._background = VariantUtils.ConvertTo<ColorRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardsViewScreen.PropertyName._grid))
    {
      this._grid = VariantUtils.ConvertTo<NCardGrid>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardsViewScreen.PropertyName._backButton))
    {
      this._backButton = VariantUtils.ConvertTo<NButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardsViewScreen.PropertyName._showUpgrades))
    {
      this._showUpgrades = VariantUtils.ConvertTo<NTickbox>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardsViewScreen.PropertyName._bottomLabel))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._bottomLabel = VariantUtils.ConvertTo<RichTextLabel>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardsViewScreen.PropertyName.ScreenType))
    {
      ref godot_variant local = ref value;
      NetScreenType screenType = this.ScreenType;
      godot_variant from = VariantUtils.CreateFrom<NetScreenType>(ref screenType);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardsViewScreen.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardsViewScreen.PropertyName.FocusedControlFromTopBar))
    {
      ref godot_variant local = ref value;
      Control controlFromTopBar = this.FocusedControlFromTopBar;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref controlFromTopBar);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardsViewScreen.PropertyName.UseSharedBackstop))
    {
      ref godot_variant local = ref value;
      bool useSharedBackstop = this.UseSharedBackstop;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref useSharedBackstop);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardsViewScreen.PropertyName._background))
    {
      value = VariantUtils.CreateFrom<ColorRect>(ref this._background);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardsViewScreen.PropertyName._grid))
    {
      value = VariantUtils.CreateFrom<NCardGrid>(ref this._grid);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardsViewScreen.PropertyName._backButton))
    {
      value = VariantUtils.CreateFrom<NButton>(ref this._backButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardsViewScreen.PropertyName._showUpgrades))
    {
      value = VariantUtils.CreateFrom<NTickbox>(ref this._showUpgrades);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardsViewScreen.PropertyName._bottomLabel))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<RichTextLabel>(ref this._bottomLabel);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCardsViewScreen.PropertyName._background, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardsViewScreen.PropertyName._grid, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardsViewScreen.PropertyName._backButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardsViewScreen.PropertyName._showUpgrades, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardsViewScreen.PropertyName._bottomLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NCardsViewScreen.PropertyName.ScreenType, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardsViewScreen.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardsViewScreen.PropertyName.FocusedControlFromTopBar, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCardsViewScreen.PropertyName.UseSharedBackstop, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NCardsViewScreen.PropertyName._background, Variant.From<ColorRect>(ref this._background));
    info.AddProperty(NCardsViewScreen.PropertyName._grid, Variant.From<NCardGrid>(ref this._grid));
    info.AddProperty(NCardsViewScreen.PropertyName._backButton, Variant.From<NButton>(ref this._backButton));
    info.AddProperty(NCardsViewScreen.PropertyName._showUpgrades, Variant.From<NTickbox>(ref this._showUpgrades));
    info.AddProperty(NCardsViewScreen.PropertyName._bottomLabel, Variant.From<RichTextLabel>(ref this._bottomLabel));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCardsViewScreen.PropertyName._background, ref variant1))
      this._background = ((Variant) ref variant1).As<ColorRect>();
    Variant variant2;
    if (info.TryGetProperty(NCardsViewScreen.PropertyName._grid, ref variant2))
      this._grid = ((Variant) ref variant2).As<NCardGrid>();
    Variant variant3;
    if (info.TryGetProperty(NCardsViewScreen.PropertyName._backButton, ref variant3))
      this._backButton = ((Variant) ref variant3).As<NButton>();
    Variant variant4;
    if (info.TryGetProperty(NCardsViewScreen.PropertyName._showUpgrades, ref variant4))
      this._showUpgrades = ((Variant) ref variant4).As<NTickbox>();
    Variant variant5;
    if (!info.TryGetProperty(NCardsViewScreen.PropertyName._bottomLabel, ref variant5))
      return;
    this._bottomLabel = ((Variant) ref variant5).As<RichTextLabel>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName ConnectSignals = StringName.op_Implicit(nameof (ConnectSignals));
    public static readonly StringName OnInspectCardHidden = StringName.op_Implicit(nameof (OnInspectCardHidden));
    public static readonly StringName ToggleShowUpgrades = StringName.op_Implicit(nameof (ToggleShowUpgrades));
    public static readonly StringName OnReturnButtonPressed = StringName.op_Implicit(nameof (OnReturnButtonPressed));
    public static readonly StringName AfterCapstoneOpened = StringName.op_Implicit(nameof (AfterCapstoneOpened));
    public static readonly StringName AfterCapstoneClosed = StringName.op_Implicit(nameof (AfterCapstoneClosed));
    public static readonly StringName OnControllerStateUpdated = StringName.op_Implicit(nameof (OnControllerStateUpdated));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName ScreenType = StringName.op_Implicit(nameof (ScreenType));
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName FocusedControlFromTopBar = StringName.op_Implicit(nameof (FocusedControlFromTopBar));
    public static readonly StringName UseSharedBackstop = StringName.op_Implicit(nameof (UseSharedBackstop));
    public static readonly StringName _background = StringName.op_Implicit(nameof (_background));
    public static readonly StringName _grid = StringName.op_Implicit(nameof (_grid));
    public static readonly StringName _backButton = StringName.op_Implicit(nameof (_backButton));
    public static readonly StringName _showUpgrades = StringName.op_Implicit(nameof (_showUpgrades));
    public static readonly StringName _bottomLabel = StringName.op_Implicit(nameof (_bottomLabel));
  }

  public class SignalName : Control.SignalName
  {
  }
}
