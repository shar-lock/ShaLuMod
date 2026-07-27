// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.ModdingScreen.NModMenuRow
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
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.ModdingScreen;

[ScriptPath("res://src/Core/Nodes/Screens/ModdingScreen/NModMenuRow.cs")]
public class NModMenuRow : NClickableControl
{
  private TextureRect _controllerIcon;
  private static readonly string _scenePath = SceneHelper.GetScenePath("screens/modding/modding_screen_row");
  private const float _selectedAlpha = 0.25f;
  private Panel _selectionHighlight;
  private NTickbox _tickbox;
  private NModdingScreen _screen;
  private bool _isSelected;

  private string Hotkey => StringName.op_Implicit(MegaInput.accept);

  public Mod? Mod { get; private set; }

  public static NModMenuRow? Create(NModdingScreen screen, Mod mod)
  {
    if (TestMode.IsOn)
      return (NModMenuRow) null;
    NModMenuRow nmodMenuRow = PreloadManager.Cache.GetScene(NModMenuRow._scenePath).Instantiate<NModMenuRow>((PackedScene.GenEditState) 0L);
    nmodMenuRow.Mod = mod;
    nmodMenuRow._screen = screen;
    return nmodMenuRow;
  }

  public override void _Ready()
  {
    if (this.Mod == null)
      return;
    this._selectionHighlight = ((Node) this).GetNode<Panel>(NodePath.op_Implicit("SelectionHighlight"));
    this._tickbox = ((Node) this).GetNode<NTickbox>(NodePath.op_Implicit("Tickbox"));
    MegaRichTextLabel node1 = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("Title"));
    TextureRect node2 = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("PlatformIcon"));
    this._controllerIcon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("ControllerIcon"));
    this._tickbox = ((Node) this).GetNode<NTickbox>(NodePath.op_Implicit("Tickbox"));
    Panel selectionHighlight = this._selectionHighlight;
    Color color1 = ((CanvasItem) this._selectionHighlight).Modulate;
    color1.A = 0.0f;
    Color color2 = color1;
    ((CanvasItem) selectionHighlight).Modulate = color2;
    NTickbox tickbox = this._tickbox;
    ModSettings modSettings = SaveManager.Instance.SettingsSave.ModSettings;
    int num = (modSettings != null ? (modSettings.IsModDisabled(this.Mod) ? 1 : 0) : 0) == 0 ? 1 : 0;
    tickbox.IsTicked = num != 0;
    ((GodotObject) this._tickbox).Connect(NTickbox.SignalName.Toggled, Callable.From<NTickbox>(new Action<NTickbox>(this.OnTickboxToggled)), 0U);
    node1.Text = this.Mod.manifest?.name ?? this.Mod.manifest?.id ?? "<null>";
    node2.Texture = NModMenuRow.GetPlatformIcon(this.Mod.modSource);
    ModLoadState state = this.Mod.state;
    switch (state)
    {
      case ModLoadState.None:
        color1 = StsColors.purple;
        break;
      case ModLoadState.Loaded:
        color1 = Colors.White;
        break;
      case ModLoadState.Failed:
        color1 = StsColors.red;
        break;
      case ModLoadState.Disabled:
        color1 = StsColors.gray;
        break;
      case ModLoadState.DisabledDuplicate:
        color1 = StsColors.gray;
        break;
      case ModLoadState.AddedAtRuntime:
        color1 = StsColors.gray;
        break;
      default:
        // ISSUE: reference to a compiler-generated method
        \u003CPrivateImplementationDetails\u003E.ThrowSwitchExpressionException((object) state);
        break;
    }
    Color color3 = color1;
    ((CanvasItem) node1).Modulate = color3;
    ((CanvasItem) node2).Modulate = color3;
    this.ConnectSignals();
    ((GodotObject) NControllerManager.Instance).Connect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.UpdateControllerButton)), 0U);
    ((GodotObject) NControllerManager.Instance).Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.UpdateControllerButton)), 0U);
    ((GodotObject) NInputManager.Instance).Connect(NInputManager.SignalName.InputRebound, Callable.From(new Action(this.UpdateControllerButton)), 0U);
    this.UpdateControllerButton();
  }

  public override void _GuiInput(InputEvent inputEvent)
  {
    base._GuiInput(inputEvent);
    if (!inputEvent.IsActionPressed(StringName.op_Implicit(this.Hotkey), false, false) || !this._isSelected)
      return;
    this._tickbox.ForceToggleTick();
  }

  protected override void OnFocus()
  {
    if (this._isSelected)
      return;
    Panel selectionHighlight = this._selectionHighlight;
    Color darkBlue = StsColors.darkBlue;
    darkBlue.A = 0.25f;
    Color color = darkBlue;
    ((CanvasItem) selectionHighlight).Modulate = color;
    this.UpdateControllerButton();
  }

  private void UpdateControllerButton()
  {
    ((CanvasItem) this._controllerIcon).SetVisible(this._isSelected && NControllerManager.Instance.IsUsingController);
    this._controllerIcon.Texture = NInputManager.Instance.GetHotkeyIcon(this.Hotkey);
  }

  protected override void OnUnfocus()
  {
    if (this._isSelected)
      return;
    ((CanvasItem) this._selectionHighlight).Modulate = Colors.Transparent;
  }

  protected override void OnRelease() => this._screen.OnRowSelected(this);

  public void SetSelected(bool isSelected)
  {
    if (this._isSelected != isSelected)
    {
      this._isSelected = isSelected;
      if (this._isSelected)
      {
        Panel selectionHighlight = this._selectionHighlight;
        Color blue = StsColors.blue;
        blue.A = 0.25f;
        Color color = blue;
        ((CanvasItem) selectionHighlight).Modulate = color;
      }
      else if (this.IsFocused)
      {
        Panel selectionHighlight = this._selectionHighlight;
        Color darkBlue = StsColors.darkBlue;
        darkBlue.A = 0.25f;
        Color color = darkBlue;
        ((CanvasItem) selectionHighlight).Modulate = color;
      }
      else
        ((CanvasItem) this._selectionHighlight).Modulate = Colors.Transparent;
    }
    this.UpdateControllerButton();
  }

  private void OnTickboxToggled(NTickbox tickbox)
  {
    SettingsSave settingsSave = SaveManager.Instance.SettingsSave;
    if (settingsSave.ModSettings == null)
    {
      ModSettings modSettings;
      settingsSave.ModSettings = modSettings = new ModSettings();
    }
    foreach (SettingsSaveMod mod in SaveManager.Instance.SettingsSave.ModSettings.ModList)
    {
      if (mod.Id == this.Mod?.manifest?.id && mod.Source == this.Mod.modSource)
        mod.IsEnabled = tickbox.IsTicked;
    }
    this._screen.OnModEnabledOrDisabled();
  }

  public static Texture2D GetPlatformIcon(ModSource modSource)
  {
    AssetCache cache = PreloadManager.Cache;
    string imagePath;
    if (modSource != ModSource.ModsDirectory)
    {
      if (modSource != ModSource.SteamWorkshop)
        throw new ArgumentOutOfRangeException(nameof (modSource), (object) modSource, (string) null);
      imagePath = ImageHelper.GetImagePath("ui/mods/steam_logo.png");
    }
    else
      imagePath = ImageHelper.GetImagePath("ui/mods/folder.png");
    return cache.GetTexture2D(imagePath);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(9)
    {
      new MethodInfo(NModMenuRow.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NModMenuRow.MethodName._GuiInput, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NModMenuRow.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NModMenuRow.MethodName.UpdateControllerButton, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NModMenuRow.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NModMenuRow.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NModMenuRow.MethodName.SetSelected, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("isSelected"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NModMenuRow.MethodName.OnTickboxToggled, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("tickbox"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NModMenuRow.MethodName.GetPlatformIcon, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Texture2D"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("modSource"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NModMenuRow.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NModMenuRow.MethodName._GuiInput) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      base._GuiInput(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NModMenuRow.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NModMenuRow.MethodName.UpdateControllerButton) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateControllerButton();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NModMenuRow.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NModMenuRow.MethodName.OnRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRelease();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NModMenuRow.MethodName.SetSelected) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetSelected(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NModMenuRow.MethodName.OnTickboxToggled) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnTickboxToggled(VariantUtils.ConvertTo<NTickbox>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NModMenuRow.MethodName.GetPlatformIcon) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    Texture2D platformIcon = NModMenuRow.GetPlatformIcon(VariantUtils.ConvertTo<ModSource>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = VariantUtils.CreateFrom<Texture2D>(ref platformIcon);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NModMenuRow.MethodName.GetPlatformIcon) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      Texture2D platformIcon = NModMenuRow.GetPlatformIcon(VariantUtils.ConvertTo<ModSource>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<Texture2D>(ref platformIcon);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NModMenuRow.MethodName._Ready) || StringName.op_Equality(ref method, NModMenuRow.MethodName._GuiInput) || StringName.op_Equality(ref method, NModMenuRow.MethodName.OnFocus) || StringName.op_Equality(ref method, NModMenuRow.MethodName.UpdateControllerButton) || StringName.op_Equality(ref method, NModMenuRow.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NModMenuRow.MethodName.OnRelease) || StringName.op_Equality(ref method, NModMenuRow.MethodName.SetSelected) || StringName.op_Equality(ref method, NModMenuRow.MethodName.OnTickboxToggled) || StringName.op_Equality(ref method, NModMenuRow.MethodName.GetPlatformIcon) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NModMenuRow.PropertyName._controllerIcon))
    {
      this._controllerIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NModMenuRow.PropertyName._selectionHighlight))
    {
      this._selectionHighlight = VariantUtils.ConvertTo<Panel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NModMenuRow.PropertyName._tickbox))
    {
      this._tickbox = VariantUtils.ConvertTo<NTickbox>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NModMenuRow.PropertyName._screen))
    {
      this._screen = VariantUtils.ConvertTo<NModdingScreen>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NModMenuRow.PropertyName._isSelected))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._isSelected = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NModMenuRow.PropertyName.Hotkey))
    {
      ref godot_variant local = ref value;
      string hotkey = this.Hotkey;
      godot_variant from = VariantUtils.CreateFrom<string>(ref hotkey);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NModMenuRow.PropertyName._controllerIcon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._controllerIcon);
      return true;
    }
    if (StringName.op_Equality(ref name, NModMenuRow.PropertyName._selectionHighlight))
    {
      value = VariantUtils.CreateFrom<Panel>(ref this._selectionHighlight);
      return true;
    }
    if (StringName.op_Equality(ref name, NModMenuRow.PropertyName._tickbox))
    {
      value = VariantUtils.CreateFrom<NTickbox>(ref this._tickbox);
      return true;
    }
    if (StringName.op_Equality(ref name, NModMenuRow.PropertyName._screen))
    {
      value = VariantUtils.CreateFrom<NModdingScreen>(ref this._screen);
      return true;
    }
    if (!StringName.op_Equality(ref name, NModMenuRow.PropertyName._isSelected))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<bool>(ref this._isSelected);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 4L, NModMenuRow.PropertyName.Hotkey, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NModMenuRow.PropertyName._controllerIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NModMenuRow.PropertyName._selectionHighlight, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NModMenuRow.PropertyName._tickbox, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NModMenuRow.PropertyName._screen, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NModMenuRow.PropertyName._isSelected, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NModMenuRow.PropertyName._controllerIcon, Variant.From<TextureRect>(ref this._controllerIcon));
    info.AddProperty(NModMenuRow.PropertyName._selectionHighlight, Variant.From<Panel>(ref this._selectionHighlight));
    info.AddProperty(NModMenuRow.PropertyName._tickbox, Variant.From<NTickbox>(ref this._tickbox));
    info.AddProperty(NModMenuRow.PropertyName._screen, Variant.From<NModdingScreen>(ref this._screen));
    info.AddProperty(NModMenuRow.PropertyName._isSelected, Variant.From<bool>(ref this._isSelected));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NModMenuRow.PropertyName._controllerIcon, ref variant1))
      this._controllerIcon = ((Variant) ref variant1).As<TextureRect>();
    Variant variant2;
    if (info.TryGetProperty(NModMenuRow.PropertyName._selectionHighlight, ref variant2))
      this._selectionHighlight = ((Variant) ref variant2).As<Panel>();
    Variant variant3;
    if (info.TryGetProperty(NModMenuRow.PropertyName._tickbox, ref variant3))
      this._tickbox = ((Variant) ref variant3).As<NTickbox>();
    Variant variant4;
    if (info.TryGetProperty(NModMenuRow.PropertyName._screen, ref variant4))
      this._screen = ((Variant) ref variant4).As<NModdingScreen>();
    Variant variant5;
    if (!info.TryGetProperty(NModMenuRow.PropertyName._isSelected, ref variant5))
      return;
    this._isSelected = ((Variant) ref variant5).As<bool>();
  }

  public new class MethodName : NClickableControl.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName _GuiInput = StringName.op_Implicit(nameof (_GuiInput));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public static readonly StringName UpdateControllerButton = StringName.op_Implicit(nameof (UpdateControllerButton));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
    public static readonly StringName SetSelected = StringName.op_Implicit(nameof (SetSelected));
    public static readonly StringName OnTickboxToggled = StringName.op_Implicit(nameof (OnTickboxToggled));
    public static readonly StringName GetPlatformIcon = StringName.op_Implicit(nameof (GetPlatformIcon));
  }

  public new class PropertyName : NClickableControl.PropertyName
  {
    public static readonly StringName Hotkey = StringName.op_Implicit(nameof (Hotkey));
    public static readonly StringName _controllerIcon = StringName.op_Implicit(nameof (_controllerIcon));
    public static readonly StringName _selectionHighlight = StringName.op_Implicit(nameof (_selectionHighlight));
    public static readonly StringName _tickbox = StringName.op_Implicit(nameof (_tickbox));
    public static readonly StringName _screen = StringName.op_Implicit(nameof (_screen));
    public static readonly StringName _isSelected = StringName.op_Implicit(nameof (_isSelected));
  }

  public new class SignalName : NClickableControl.SignalName
  {
  }
}
