// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect.NAscensionPanel
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;

[ScriptPath("res://src/Core/Nodes/Screens/CharacterSelect/NAscensionPanel.cs")]
public class NAscensionPanel : Control
{
  private static readonly StringName _tabLeftHotkey = MegaInput.viewDeckAndTabLeft;
  private static readonly StringName _tabRightHotkey = MegaInput.viewExhaustPileAndTabRight;
  private static readonly StringName _fontOutlineTheme = StringName.op_Implicit("font_outline_color");
  private static readonly StringName _h = new StringName("h");
  private static readonly StringName _v = new StringName("v");
  private static readonly Color _redLabelOutline = new Color("593400");
  private static readonly Color _blueLabelOutline = new Color("004759");
  private int _maxAscension;
  private NButton _leftArrow;
  private NButton _rightArrow;
  private MegaLabel _ascensionLevel;
  private MegaRichTextLabel _info;
  private TextureRect _leftTriggerIcon;
  private TextureRect _rightTriggerIcon;
  private ShaderMaterial _iconHsv;
  private bool _arrowsVisible = true;
  private MultiplayerUiMode _mode = MultiplayerUiMode.Singleplayer;
  private Tween? _tween;
  private 
  #nullable disable
  NAscensionPanel.AscensionLevelChangedEventHandler backing_AscensionLevelChanged;

  public int Ascension { get; private set; }

  public override void _Ready()
  {
    this._leftTriggerIcon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%LeftTriggerIcon"));
    this._rightTriggerIcon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%RightTriggerIcon"));
    this._leftArrow = ((Node) this).GetNode<NButton>(NodePath.op_Implicit("HBoxContainer/LeftArrowContainer/LeftArrow"));
    this._rightArrow = ((Node) this).GetNode<NButton>(NodePath.op_Implicit("HBoxContainer/RightArrowContainer/RightArrow"));
    this._ascensionLevel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("HBoxContainer/AscensionIconContainer/AscensionIcon/AscensionLevel"));
    this._info = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("HBoxContainer/AscensionDescription/Description"));
    this._iconHsv = (ShaderMaterial) ((CanvasItem) ((Node) this).GetNode<Control>(NodePath.op_Implicit("%AscensionIcon"))).Material;
    ((GodotObject) this._leftArrow).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>((Action<NButton>) (_ => this.DecrementAscension())), 0U);
    ((GodotObject) this._rightArrow).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>((Action<NButton>) (_ => this.IncrementAscension())), 0U);
    ((GodotObject) NControllerManager.Instance).Connect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.UpdateControllerButton)), 0U);
    ((GodotObject) NControllerManager.Instance).Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.UpdateControllerButton)), 0U);
    ((GodotObject) NInputManager.Instance).Connect(NInputManager.SignalName.InputRebound, Callable.From(new Action(this.UpdateControllerButton)), 0U);
    this.UpdateControllerButton();
  }

  public void Initialize(MultiplayerUiMode mode)
  {
    this._mode = mode;
    if (this._mode == MultiplayerUiMode.Host)
    {
      this.SetFireBlue();
      this._arrowsVisible = true;
      this.SetMaxAscension(SaveManager.Instance.Progress.MaxMultiplayerAscension);
      this.SetAscensionLevel(Math.Min(this._maxAscension, SaveManager.Instance.Progress.PreferredMultiplayerAscension));
      NHotkeyManager.Instance.PushHotkeyPressedBinding(StringName.op_Implicit(NAscensionPanel._tabLeftHotkey), new Action(this.DecrementAscension));
      NHotkeyManager.Instance.PushHotkeyPressedBinding(StringName.op_Implicit(NAscensionPanel._tabRightHotkey), new Action(this.IncrementAscension));
    }
    else if (this._mode == MultiplayerUiMode.Singleplayer)
    {
      this.SetFireRed();
      this._arrowsVisible = true;
      this.SetMaxAscension(0);
      this.SetAscensionLevel(0);
      NHotkeyManager.Instance.PushHotkeyPressedBinding(StringName.op_Implicit(NAscensionPanel._tabLeftHotkey), new Action(this.DecrementAscension));
      NHotkeyManager.Instance.PushHotkeyPressedBinding(StringName.op_Implicit(NAscensionPanel._tabRightHotkey), new Action(this.IncrementAscension));
    }
    else
    {
      bool flag;
      switch (this._mode)
      {
        case MultiplayerUiMode.Client:
        case MultiplayerUiMode.Load:
          flag = true;
          break;
        default:
          flag = false;
          break;
      }
      if (!flag)
        return;
      this.SetFireBlue();
      this._arrowsVisible = false;
      this.SetMaxAscension(0);
    }
  }

  private void SetFireBlue()
  {
    this._iconHsv.SetShaderParameter(NAscensionPanel._h, Variant.op_Implicit(0.52f));
    this._iconHsv.SetShaderParameter(NAscensionPanel._v, Variant.op_Implicit(1.2f));
    ((Control) this._ascensionLevel).AddThemeColorOverride(NAscensionPanel._fontOutlineTheme, NAscensionPanel._blueLabelOutline);
  }

  private void SetFireRed()
  {
    this._iconHsv.SetShaderParameter(NAscensionPanel._h, Variant.op_Implicit(1f));
    this._iconHsv.SetShaderParameter(NAscensionPanel._v, Variant.op_Implicit(1f));
    ((Control) this._ascensionLevel).AddThemeColorOverride(NAscensionPanel._fontOutlineTheme, NAscensionPanel._redLabelOutline);
  }

  public void Cleanup()
  {
    bool flag;
    switch (this._mode)
    {
      case MultiplayerUiMode.Singleplayer:
      case MultiplayerUiMode.Host:
        flag = true;
        break;
      default:
        flag = false;
        break;
    }
    if (!flag)
      return;
    NHotkeyManager.Instance.RemoveHotkeyPressedBinding(StringName.op_Implicit(NAscensionPanel._tabLeftHotkey), new Action(this.DecrementAscension));
    NHotkeyManager.Instance.RemoveHotkeyPressedBinding(StringName.op_Implicit(NAscensionPanel._tabRightHotkey), new Action(this.IncrementAscension));
  }

  public void SetAscensionLevel(int ascension)
  {
    if (this.Ascension != ascension)
    {
      this.Ascension = ascension;
      ((GodotObject) this).EmitSignal(NAscensionPanel.SignalName.AscensionLevelChanged, Array.Empty<Variant>());
    }
    this.RefreshAscensionText();
    this.RefreshArrowVisibility();
  }

  private void IncrementAscension()
  {
    if (this.Ascension >= this._maxAscension)
      return;
    this.SetAscensionLevel(this.Ascension + 1);
  }

  private void DecrementAscension()
  {
    if (this.Ascension <= 0)
      return;
    this.SetAscensionLevel(this.Ascension - 1);
  }

  private void RefreshArrowVisibility()
  {
    ((CanvasItem) this._leftArrow).Visible = this._arrowsVisible && this.Ascension != 0;
    ((CanvasItem) this._rightArrow).Visible = this._arrowsVisible && this.Ascension != this._maxAscension;
  }

  public void SetMaxAscension(int maxAscension)
  {
    Log.Info($"Max ascension changed to {maxAscension}");
    this._maxAscension = maxAscension;
    if (this.Ascension >= this._maxAscension)
      this.SetAscensionLevel(this._maxAscension);
    ((CanvasItem) this).Visible = this._maxAscension > 0;
    this.RefreshArrowVisibility();
  }

  private void RefreshAscensionText()
  {
    this._ascensionLevel.SetTextAutoSize(this.Ascension.ToString());
    this._info.Text = $"[b][gold]{AscensionHelper.GetTitle(this.Ascension).GetFormattedText()}[/gold][/b]\n{AscensionHelper.GetDescription(this.Ascension).GetFormattedText()}";
  }

  public void AnimIn()
  {
    if (!((CanvasItem) this).Visible)
      return;
    Color modulate = ((CanvasItem) this).Modulate;
    modulate.A = 0.0f;
    ((CanvasItem) this).Modulate = modulate;
    Tween tween = this._tween;
    if (tween != null)
      tween.FastForwardToCompletion();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.2);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position:y"), Variant.op_Implicit(this.Position.Y), 0.3).From(Variant.op_Implicit(this.Position.Y + 30f)).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L);
  }

  private void UpdateControllerButton()
  {
    bool flag;
    switch (this._mode)
    {
      case MultiplayerUiMode.Singleplayer:
      case MultiplayerUiMode.Host:
        flag = true;
        break;
      default:
        flag = false;
        break;
    }
    if (flag)
    {
      ((CanvasItem) this._leftTriggerIcon).Visible = NControllerManager.Instance.IsUsingController;
      ((CanvasItem) this._rightTriggerIcon).Visible = NControllerManager.Instance.IsUsingController;
      this._leftTriggerIcon.Texture = NInputManager.Instance.GetHotkeyIcon(StringName.op_Implicit(MegaInput.viewDeckAndTabLeft));
      this._rightTriggerIcon.Texture = NInputManager.Instance.GetHotkeyIcon(StringName.op_Implicit(MegaInput.viewExhaustPileAndTabRight));
    }
    else
    {
      ((CanvasItem) this._leftTriggerIcon).Visible = false;
      ((CanvasItem) this._rightTriggerIcon).Visible = false;
    }
  }

  public void Disable()
  {
    this._leftArrow.Disable();
    this._rightArrow.Disable();
    ((CanvasItem) this._leftTriggerIcon).Visible = false;
    ((CanvasItem) this._rightTriggerIcon).Visible = false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(14)
    {
      new MethodInfo(NAscensionPanel.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAscensionPanel.MethodName.Initialize, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("mode"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NAscensionPanel.MethodName.SetFireBlue, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAscensionPanel.MethodName.SetFireRed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAscensionPanel.MethodName.Cleanup, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAscensionPanel.MethodName.SetAscensionLevel, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("ascension"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NAscensionPanel.MethodName.IncrementAscension, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAscensionPanel.MethodName.DecrementAscension, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAscensionPanel.MethodName.RefreshArrowVisibility, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAscensionPanel.MethodName.SetMaxAscension, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("maxAscension"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NAscensionPanel.MethodName.RefreshAscensionText, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAscensionPanel.MethodName.AnimIn, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAscensionPanel.MethodName.UpdateControllerButton, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAscensionPanel.MethodName.Disable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NAscensionPanel.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAscensionPanel.MethodName.Initialize) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.Initialize(VariantUtils.ConvertTo<MultiplayerUiMode>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAscensionPanel.MethodName.SetFireBlue) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SetFireBlue();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAscensionPanel.MethodName.SetFireRed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SetFireRed();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAscensionPanel.MethodName.Cleanup) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Cleanup();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAscensionPanel.MethodName.SetAscensionLevel) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetAscensionLevel(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAscensionPanel.MethodName.IncrementAscension) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.IncrementAscension();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAscensionPanel.MethodName.DecrementAscension) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DecrementAscension();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAscensionPanel.MethodName.RefreshArrowVisibility) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshArrowVisibility();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAscensionPanel.MethodName.SetMaxAscension) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetMaxAscension(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAscensionPanel.MethodName.RefreshAscensionText) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshAscensionText();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAscensionPanel.MethodName.AnimIn) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimIn();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAscensionPanel.MethodName.UpdateControllerButton) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateControllerButton();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NAscensionPanel.MethodName.Disable) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.Disable();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NAscensionPanel.MethodName._Ready) || StringName.op_Equality(ref method, NAscensionPanel.MethodName.Initialize) || StringName.op_Equality(ref method, NAscensionPanel.MethodName.SetFireBlue) || StringName.op_Equality(ref method, NAscensionPanel.MethodName.SetFireRed) || StringName.op_Equality(ref method, NAscensionPanel.MethodName.Cleanup) || StringName.op_Equality(ref method, NAscensionPanel.MethodName.SetAscensionLevel) || StringName.op_Equality(ref method, NAscensionPanel.MethodName.IncrementAscension) || StringName.op_Equality(ref method, NAscensionPanel.MethodName.DecrementAscension) || StringName.op_Equality(ref method, NAscensionPanel.MethodName.RefreshArrowVisibility) || StringName.op_Equality(ref method, NAscensionPanel.MethodName.SetMaxAscension) || StringName.op_Equality(ref method, NAscensionPanel.MethodName.RefreshAscensionText) || StringName.op_Equality(ref method, NAscensionPanel.MethodName.AnimIn) || StringName.op_Equality(ref method, NAscensionPanel.MethodName.UpdateControllerButton) || StringName.op_Equality(ref method, NAscensionPanel.MethodName.Disable) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NAscensionPanel.PropertyName.Ascension))
    {
      this.Ascension = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAscensionPanel.PropertyName._maxAscension))
    {
      this._maxAscension = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAscensionPanel.PropertyName._leftArrow))
    {
      this._leftArrow = VariantUtils.ConvertTo<NButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAscensionPanel.PropertyName._rightArrow))
    {
      this._rightArrow = VariantUtils.ConvertTo<NButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAscensionPanel.PropertyName._ascensionLevel))
    {
      this._ascensionLevel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAscensionPanel.PropertyName._info))
    {
      this._info = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAscensionPanel.PropertyName._leftTriggerIcon))
    {
      this._leftTriggerIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAscensionPanel.PropertyName._rightTriggerIcon))
    {
      this._rightTriggerIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAscensionPanel.PropertyName._iconHsv))
    {
      this._iconHsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAscensionPanel.PropertyName._arrowsVisible))
    {
      this._arrowsVisible = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAscensionPanel.PropertyName._mode))
    {
      this._mode = VariantUtils.ConvertTo<MultiplayerUiMode>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NAscensionPanel.PropertyName._tween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._tween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NAscensionPanel.PropertyName.Ascension))
    {
      ref godot_variant local = ref value;
      int ascension = this.Ascension;
      godot_variant from = VariantUtils.CreateFrom<int>(ref ascension);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NAscensionPanel.PropertyName._maxAscension))
    {
      value = VariantUtils.CreateFrom<int>(ref this._maxAscension);
      return true;
    }
    if (StringName.op_Equality(ref name, NAscensionPanel.PropertyName._leftArrow))
    {
      value = VariantUtils.CreateFrom<NButton>(ref this._leftArrow);
      return true;
    }
    if (StringName.op_Equality(ref name, NAscensionPanel.PropertyName._rightArrow))
    {
      value = VariantUtils.CreateFrom<NButton>(ref this._rightArrow);
      return true;
    }
    if (StringName.op_Equality(ref name, NAscensionPanel.PropertyName._ascensionLevel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._ascensionLevel);
      return true;
    }
    if (StringName.op_Equality(ref name, NAscensionPanel.PropertyName._info))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._info);
      return true;
    }
    if (StringName.op_Equality(ref name, NAscensionPanel.PropertyName._leftTriggerIcon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._leftTriggerIcon);
      return true;
    }
    if (StringName.op_Equality(ref name, NAscensionPanel.PropertyName._rightTriggerIcon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._rightTriggerIcon);
      return true;
    }
    if (StringName.op_Equality(ref name, NAscensionPanel.PropertyName._iconHsv))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._iconHsv);
      return true;
    }
    if (StringName.op_Equality(ref name, NAscensionPanel.PropertyName._arrowsVisible))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._arrowsVisible);
      return true;
    }
    if (StringName.op_Equality(ref name, NAscensionPanel.PropertyName._mode))
    {
      value = VariantUtils.CreateFrom<MultiplayerUiMode>(ref this._mode);
      return true;
    }
    if (!StringName.op_Equality(ref name, NAscensionPanel.PropertyName._tween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._tween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 2L, NAscensionPanel.PropertyName.Ascension, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NAscensionPanel.PropertyName._maxAscension, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAscensionPanel.PropertyName._leftArrow, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAscensionPanel.PropertyName._rightArrow, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAscensionPanel.PropertyName._ascensionLevel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAscensionPanel.PropertyName._info, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAscensionPanel.PropertyName._leftTriggerIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAscensionPanel.PropertyName._rightTriggerIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAscensionPanel.PropertyName._iconHsv, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NAscensionPanel.PropertyName._arrowsVisible, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NAscensionPanel.PropertyName._mode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAscensionPanel.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName ascension1 = NAscensionPanel.PropertyName.Ascension;
    int ascension2 = this.Ascension;
    Variant variant = Variant.From<int>(ref ascension2);
    serializationInfo.AddProperty(ascension1, variant);
    info.AddProperty(NAscensionPanel.PropertyName._maxAscension, Variant.From<int>(ref this._maxAscension));
    info.AddProperty(NAscensionPanel.PropertyName._leftArrow, Variant.From<NButton>(ref this._leftArrow));
    info.AddProperty(NAscensionPanel.PropertyName._rightArrow, Variant.From<NButton>(ref this._rightArrow));
    info.AddProperty(NAscensionPanel.PropertyName._ascensionLevel, Variant.From<MegaLabel>(ref this._ascensionLevel));
    info.AddProperty(NAscensionPanel.PropertyName._info, Variant.From<MegaRichTextLabel>(ref this._info));
    info.AddProperty(NAscensionPanel.PropertyName._leftTriggerIcon, Variant.From<TextureRect>(ref this._leftTriggerIcon));
    info.AddProperty(NAscensionPanel.PropertyName._rightTriggerIcon, Variant.From<TextureRect>(ref this._rightTriggerIcon));
    info.AddProperty(NAscensionPanel.PropertyName._iconHsv, Variant.From<ShaderMaterial>(ref this._iconHsv));
    info.AddProperty(NAscensionPanel.PropertyName._arrowsVisible, Variant.From<bool>(ref this._arrowsVisible));
    info.AddProperty(NAscensionPanel.PropertyName._mode, Variant.From<MultiplayerUiMode>(ref this._mode));
    info.AddProperty(NAscensionPanel.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddSignalEventDelegate(NAscensionPanel.SignalName.AscensionLevelChanged, (Delegate) this.backing_AscensionLevelChanged);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NAscensionPanel.PropertyName.Ascension, ref variant1))
      this.Ascension = ((Variant) ref variant1).As<int>();
    Variant variant2;
    if (info.TryGetProperty(NAscensionPanel.PropertyName._maxAscension, ref variant2))
      this._maxAscension = ((Variant) ref variant2).As<int>();
    Variant variant3;
    if (info.TryGetProperty(NAscensionPanel.PropertyName._leftArrow, ref variant3))
      this._leftArrow = ((Variant) ref variant3).As<NButton>();
    Variant variant4;
    if (info.TryGetProperty(NAscensionPanel.PropertyName._rightArrow, ref variant4))
      this._rightArrow = ((Variant) ref variant4).As<NButton>();
    Variant variant5;
    if (info.TryGetProperty(NAscensionPanel.PropertyName._ascensionLevel, ref variant5))
      this._ascensionLevel = ((Variant) ref variant5).As<MegaLabel>();
    Variant variant6;
    if (info.TryGetProperty(NAscensionPanel.PropertyName._info, ref variant6))
      this._info = ((Variant) ref variant6).As<MegaRichTextLabel>();
    Variant variant7;
    if (info.TryGetProperty(NAscensionPanel.PropertyName._leftTriggerIcon, ref variant7))
      this._leftTriggerIcon = ((Variant) ref variant7).As<TextureRect>();
    Variant variant8;
    if (info.TryGetProperty(NAscensionPanel.PropertyName._rightTriggerIcon, ref variant8))
      this._rightTriggerIcon = ((Variant) ref variant8).As<TextureRect>();
    Variant variant9;
    if (info.TryGetProperty(NAscensionPanel.PropertyName._iconHsv, ref variant9))
      this._iconHsv = ((Variant) ref variant9).As<ShaderMaterial>();
    Variant variant10;
    if (info.TryGetProperty(NAscensionPanel.PropertyName._arrowsVisible, ref variant10))
      this._arrowsVisible = ((Variant) ref variant10).As<bool>();
    Variant variant11;
    if (info.TryGetProperty(NAscensionPanel.PropertyName._mode, ref variant11))
      this._mode = ((Variant) ref variant11).As<MultiplayerUiMode>();
    Variant variant12;
    if (info.TryGetProperty(NAscensionPanel.PropertyName._tween, ref variant12))
      this._tween = ((Variant) ref variant12).As<Tween>();
    NAscensionPanel.AscensionLevelChangedEventHandler changedEventHandler;
    if (!info.TryGetSignalEventDelegate<NAscensionPanel.AscensionLevelChangedEventHandler>(NAscensionPanel.SignalName.AscensionLevelChanged, ref changedEventHandler))
      return;
    this.backing_AscensionLevelChanged = changedEventHandler;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<MethodInfo> GetGodotSignalList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NAscensionPanel.SignalName.AscensionLevelChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  public event NAscensionPanel.AscensionLevelChangedEventHandler AscensionLevelChanged
  {
    add => this.backing_AscensionLevelChanged += value;
    remove => this.backing_AscensionLevelChanged -= value;
  }

  protected void EmitSignalAscensionLevelChanged()
  {
    ((GodotObject) this).EmitSignal(NAscensionPanel.SignalName.AscensionLevelChanged, Array.Empty<Variant>());
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RaiseGodotClassSignalCallbacks(
    in godot_string_name signal,
    NativeVariantPtrArgs args)
  {
    if (StringName.op_Equality(ref signal, NAscensionPanel.SignalName.AscensionLevelChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NAscensionPanel.AscensionLevelChangedEventHandler ascensionLevelChanged = this.backing_AscensionLevelChanged;
      if (ascensionLevelChanged == null)
        return;
      ascensionLevelChanged();
    }
    else
      ((GodotObject) this).RaiseGodotClassSignalCallbacks(ref signal, args);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassSignal(in godot_string_name signal)
  {
    return StringName.op_Equality(ref signal, NAscensionPanel.SignalName.AscensionLevelChanged) || base.HasGodotClassSignal(ref signal);
  }

  [Signal]
  public delegate void AscensionLevelChangedEventHandler();

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName Initialize = StringName.op_Implicit(nameof (Initialize));
    public static readonly StringName SetFireBlue = StringName.op_Implicit(nameof (SetFireBlue));
    public static readonly StringName SetFireRed = StringName.op_Implicit(nameof (SetFireRed));
    public static readonly StringName Cleanup = StringName.op_Implicit(nameof (Cleanup));
    public static readonly StringName SetAscensionLevel = StringName.op_Implicit(nameof (SetAscensionLevel));
    public static readonly StringName IncrementAscension = StringName.op_Implicit(nameof (IncrementAscension));
    public static readonly StringName DecrementAscension = StringName.op_Implicit(nameof (DecrementAscension));
    public static readonly StringName RefreshArrowVisibility = StringName.op_Implicit(nameof (RefreshArrowVisibility));
    public static readonly StringName SetMaxAscension = StringName.op_Implicit(nameof (SetMaxAscension));
    public static readonly StringName RefreshAscensionText = StringName.op_Implicit(nameof (RefreshAscensionText));
    public static readonly StringName AnimIn = StringName.op_Implicit(nameof (AnimIn));
    public static readonly StringName UpdateControllerButton = StringName.op_Implicit(nameof (UpdateControllerButton));
    public static readonly StringName Disable = StringName.op_Implicit(nameof (Disable));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName Ascension = StringName.op_Implicit(nameof (Ascension));
    public static readonly StringName _maxAscension = StringName.op_Implicit(nameof (_maxAscension));
    public static readonly StringName _leftArrow = StringName.op_Implicit(nameof (_leftArrow));
    public static readonly StringName _rightArrow = StringName.op_Implicit(nameof (_rightArrow));
    public static readonly StringName _ascensionLevel = StringName.op_Implicit(nameof (_ascensionLevel));
    public static readonly StringName _info = StringName.op_Implicit(nameof (_info));
    public static readonly StringName _leftTriggerIcon = StringName.op_Implicit(nameof (_leftTriggerIcon));
    public static readonly StringName _rightTriggerIcon = StringName.op_Implicit(nameof (_rightTriggerIcon));
    public static readonly StringName _iconHsv = StringName.op_Implicit(nameof (_iconHsv));
    public static readonly StringName _arrowsVisible = StringName.op_Implicit(nameof (_arrowsVisible));
    public static readonly StringName _mode = StringName.op_Implicit(nameof (_mode));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
  }

  public class SignalName : Control.SignalName
  {
    public static readonly StringName AscensionLevelChanged = StringName.op_Implicit(nameof (AscensionLevelChanged));
  }
}
