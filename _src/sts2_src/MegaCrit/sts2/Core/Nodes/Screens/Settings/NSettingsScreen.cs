// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Settings.NSettingsScreen
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Nodes.Screens.ModdingScreen;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings;

[ScriptPath("res://src/Core/Nodes/Screens/Settings/NSettingsScreen.cs")]
public class NSettingsScreen : NSubmenu
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("screens/settings_screen");
  private NSettingsTabManager _settingsTabManager;
  private NOpenFeedbackScreenButton _feedbackScreenButton;
  private NOpenModdingScreenButton _moddingScreenButton;
  private NSettingsToast _toast;
  private bool _isInRun;
  public static readonly Vector2 settingTipsOffset = new Vector2(1012f, -60f);
  private 
  #nullable disable
  NSettingsScreen.SettingsClosedEventHandler backing_SettingsClosed;
  private NSettingsScreen.SettingsOpenedEventHandler backing_SettingsOpened;

  public static 
  #nullable enable
  string[] AssetPaths
  {
    get
    {
      return new string[2]
      {
        NSettingsScreen._scenePath,
        "res://images/ui/language_warning.png"
      };
    }
  }

  protected override Control? InitialFocusedControl
  {
    get => this._settingsTabManager.DefaultFocusedControl;
  }

  public void SetIsInRun(bool isInRun) => this._isInRun = isInRun;

  public override void _Ready()
  {
    this.ConnectSignals();
    this._settingsTabManager = ((Node) this).GetNode<NSettingsTabManager>(NodePath.op_Implicit("%SettingsTabManager"));
    this._feedbackScreenButton = ((Node) this).GetNode<NOpenFeedbackScreenButton>(NodePath.op_Implicit("%FeedbackButton"));
    this._moddingScreenButton = ((Node) this).GetNode<NOpenModdingScreenButton>(NodePath.op_Implicit("%ModdingButton"));
    this._toast = ((Node) this).GetNode<NSettingsToast>(NodePath.op_Implicit("%Toast"));
    this.LocalizeLabels();
    ((Node) this).ProcessMode = ((CanvasItem) this).Visible ? (Node.ProcessModeEnum) 0L : (Node.ProcessModeEnum) 4L;
    ((GodotObject) this._settingsTabManager).Connect(NSettingsTabManager.SignalName.TabChanged, Callable.From(new Action(this.OnSettingsTabChanged)), 0U);
    ((GodotObject) this._moddingScreenButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OpenModdingScreen)), 0U);
    ((GodotObject) this._feedbackScreenButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OpenFeedbackScreen)), 0U);
    if (SaveManager.Instance.SettingsSave.ModSettings != null && ModManager.Mods.Count > 0)
    {
      ((CanvasItem) ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Modding"))).Visible = true;
      ((CanvasItem) ((Node) this).GetNode<Control>(NodePath.op_Implicit("%ModdingDivider"))).Visible = true;
    }
    if (PlatformUtil.GetSupportedWindowMode() == SupportedWindowMode.FullscreenOnly)
    {
      ((CanvasItem) ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Fullscreen"))).Visible = false;
      ((CanvasItem) ((Node) this).GetNode<Control>(NodePath.op_Implicit("%FullscreenDivider"))).Visible = false;
    }
    if (!RunManager.Instance.IsInProgress)
      return;
    ((CanvasItem) ((Node) this).GetNode<Node>(NodePath.op_Implicit("%LanguageLine")).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("Label"))).Modulate = StsColors.gray;
    NLanguageDropdown node = ((Node) this).GetNode<NLanguageDropdown>(NodePath.op_Implicit("%LanguageDropdown"));
    ((CanvasItem) node).Modulate = StsColors.gray;
    node.Disable();
    this._moddingScreenButton.Disable();
  }

  public void ShowToast(LocString locString) => this._toast.Show(locString);

  private void OnSettingsTabChanged()
  {
  }

  private void LocalizeLabels()
  {
    Node content1 = (Node) ((Node) this).GetNode<NSettingsPanel>(NodePath.op_Implicit("%GeneralSettings")).Content;
    NSettingsScreen.LocHelper(content1.GetNode<Node>(NodePath.op_Implicit("LanguageLine")), new LocString("settings_ui", this._isInRun ? "LANGUAGE_IN_RUN" : "LANGUAGE"));
    NSettingsScreen.LocHelper(content1.GetNode<Node>(NodePath.op_Implicit("FastMode")), new LocString("settings_ui", "FASTMODE"));
    NSettingsScreen.LocHelper(content1.GetNode<Node>(NodePath.op_Implicit("PhobiaMode")), new LocString("settings_ui", "PHOBIA_MODE"));
    NSettingsScreen.LocHelper(content1.GetNode<Node>(NodePath.op_Implicit("Screenshake")), new LocString("settings_ui", "SCREENSHAKE"));
    NSettingsScreen.LocHelper(content1.GetNode<Node>(NodePath.op_Implicit("CommonTooltips")), new LocString("settings_ui", "COMMON_TOOLTIPS"));
    NSettingsScreen.LocHelper(content1.GetNode<Node>(NodePath.op_Implicit("ShowRunTimer")), new LocString("settings_ui", "SHOW_RUN_TIMER_HEADER"));
    NSettingsScreen.LocHelper(content1.GetNode<Node>(NodePath.op_Implicit("ShowHandCardCount")), new LocString("settings_ui", "SHOW_HAND_CARD_COUNT_HEADER"));
    NSettingsScreen.LocHelper(content1.GetNode<Node>(NodePath.op_Implicit("ShowMpDrawings")), new LocString("settings_ui", "SHOW_MP_DRAWINGS_HEADER"));
    NSettingsScreen.LocHelper(content1.GetNode<Node>(NodePath.op_Implicit("LongPressConfirmations")), new LocString("settings_ui", "LONG_PRESS_CONFIRMATION_HEADER"));
    NSettingsScreen.LocHelper(content1.GetNode<Node>(NodePath.op_Implicit("SkipIntroLogo")), new LocString("settings_ui", "SKIP_INTRO_LOGO_HEADER"));
    NSettingsScreen.LocHelper(content1.GetNode<Node>(NodePath.op_Implicit("LimitFpsInBackground")), new LocString("settings_ui", "LIMIT_FPS_IN_BACKGROUND_HEADER"));
    NSettingsScreen.LocHelper(content1.GetNode<Node>(NodePath.op_Implicit("UploadGameplayData")), new LocString("settings_ui", "UPLOAD_GAMEPLAY_DATA"));
    NSettingsScreen.LocHelper(content1.GetNode<Node>(NodePath.op_Implicit("TextEffects")), new LocString("settings_ui", "TEXT_EFFECTS"));
    NSettingsScreen.LocHelper(content1.GetNode<Node>(NodePath.op_Implicit("SendFeedback")), new LocString("settings_ui", "SEND_FEEDBACK"));
    NSettingsScreen.LocHelper(content1.GetNode<Node>(NodePath.op_Implicit("ResetTutorials")), new LocString("settings_ui", "TUTORIAL_RESET"));
    NSettingsScreen.LocHelper(content1.GetNode<Node>(NodePath.op_Implicit("Credits")), new LocString("settings_ui", "CREDITS"));
    NSettingsScreen.LocHelper(content1.GetNode<Node>(NodePath.op_Implicit("ResetGameplay")), new LocString("settings_ui", "RESET_DEFAULT"));
    Node content2 = (Node) ((Node) this).GetNode<NSettingsPanel>(NodePath.op_Implicit("%GraphicsSettings")).Content;
    NSettingsScreen.LocHelper(content2.GetNode<Node>(NodePath.op_Implicit("Fullscreen")), new LocString("settings_ui", "FULLSCREEN"));
    NSettingsScreen.LocHelper(content2.GetNode<Node>(NodePath.op_Implicit("DisplaySelection")), new LocString("settings_ui", "DISPLAY_SELECTION"));
    NSettingsScreen.LocHelper(content2.GetNode<Node>(NodePath.op_Implicit("WindowedResolution")), new LocString("settings_ui", "RESOLUTION"));
    NSettingsScreen.LocHelper(content2.GetNode<Node>(NodePath.op_Implicit("AspectRatio")), new LocString("settings_ui", "ASPECT_RATIO"));
    NSettingsScreen.LocHelper(content2.GetNode<Node>(NodePath.op_Implicit("WindowResizing")), new LocString("settings_ui", "WINDOW_RESIZING"));
    NSettingsScreen.LocHelper(content2.GetNode<Node>(NodePath.op_Implicit("VSync")), new LocString("settings_ui", "VSYNC"));
    NSettingsScreen.LocHelper(content2.GetNode<Node>(NodePath.op_Implicit("MaxFps")), new LocString("settings_ui", "FPS_CAP"));
    NSettingsScreen.LocHelper(content2.GetNode<Node>(NodePath.op_Implicit("Msaa")), new LocString("settings_ui", "MSAA"));
    NSettingsScreen.LocHelper(content2.GetNode<Node>(NodePath.op_Implicit("ResetGraphics")), new LocString("settings_ui", "RESET_DEFAULT"));
    Node content3 = (Node) ((Node) this).GetNode<NSettingsPanel>(NodePath.op_Implicit("%SoundSettings")).Content;
    NSettingsScreen.LocHelper(content3.GetNode<Node>(NodePath.op_Implicit("MasterVolume")), new LocString("settings_ui", "MASTER_VOLUME"));
    NSettingsScreen.LocHelper(content3.GetNode<Node>(NodePath.op_Implicit("BgmVolume")), new LocString("settings_ui", "MUSIC_VOLUME"));
    NSettingsScreen.LocHelper(content3.GetNode<Node>(NodePath.op_Implicit("SfxVolume")), new LocString("settings_ui", "SFX_VOLUME"));
    NSettingsScreen.LocHelper(content3.GetNode<Node>(NodePath.op_Implicit("AmbienceVolume")), new LocString("settings_ui", "AMBIENCE_VOLUME"));
    NSettingsScreen.LocHelper(content3.GetNode<Node>(NodePath.op_Implicit("MuteIfBackground")), new LocString("settings_ui", "BACKGROUND_MUTE"));
  }

  private static void LocHelper(Node settingsLineNode, LocString locString)
  {
    settingsLineNode.GetNode<MegaRichTextLabel>(NodePath.op_Implicit("Label")).Text = locString.GetFormattedText();
  }

  private void OpenModdingScreen(NButton _) => this._stack.PushSubmenuType<NModdingScreen>();

  private void OpenFeedbackScreen(NButton _)
  {
    this._lastFocusedControl = (Control) this._feedbackScreenButton;
    TaskHelper.RunSafely(this.OpenFeedbackScreen());
  }

  public async Task OpenFeedbackScreen()
  {
    Log.Info("Opening feedback screen");
    ((CanvasItem) this).Visible = false;
    NGame.Instance.MainMenu?.DisableBackstopInstantly();
    NCapstoneContainer.Instance?.DisableBackstopInstantly();
    NRun.Instance?.GlobalUi.RelicInventory.ShowImmediately();
    NRun.Instance?.GlobalUi.MultiplayerPlayerContainer.ShowImmediately();
    double num1 = (double) await ((Node) this).AwaitProcessFrame();
    double num2 = (double) await ((Node) this).AwaitProcessFrame();
    double num3 = (double) await ((Node) this).AwaitProcessFrame();
    Image image = ((Texture2D) ((Node) this).GetViewport().GetTexture()).GetImage();
    ((CanvasItem) this).Visible = true;
    NRun.Instance?.GlobalUi.RelicInventory.HideImmediately();
    NRun.Instance?.GlobalUi.MultiplayerPlayerContainer.HideImmediately();
    NGame.Instance.MainMenu?.EnableBackstopInstantly();
    NCapstoneContainer.Instance?.EnableBackstopInstantly();
    NSendFeedbackScreen feedbackScreen = NGame.Instance.GetOrCreateFeedbackScreen();
    feedbackScreen.SetScreenshot(image);
    feedbackScreen.Open();
  }

  public override void OnSubmenuOpened()
  {
    base.OnSubmenuOpened();
    ((Node) this).ProcessMode = (Node.ProcessModeEnum) 0L;
    ((GodotObject) this).EmitSignal(NSettingsScreen.SignalName.SettingsOpened, Array.Empty<Variant>());
    this._settingsTabManager.ResetTabs();
    this._settingsTabManager.Enable();
  }

  public override void OnSubmenuClosed()
  {
    base.OnSubmenuClosed();
    ((Node) this).ProcessMode = (Node.ProcessModeEnum) 4L;
    SaveManager.Instance.SaveSettings();
    SaveManager.Instance.SavePrefsFile();
    ((GodotObject) this).EmitSignal(NSettingsScreen.SignalName.SettingsClosed, Array.Empty<Variant>());
    this._settingsTabManager.Disable();
  }

  protected override void OnSubmenuHidden()
  {
    base.OnSubmenuClosed();
    ((Node) this).ProcessMode = (Node.ProcessModeEnum) 4L;
    ((GodotObject) this).EmitSignal(NSettingsScreen.SignalName.SettingsClosed, Array.Empty<Variant>());
  }

  protected override void OnSubmenuShown()
  {
    base.OnSubmenuShown();
    ((Node) this).ProcessMode = (Node.ProcessModeEnum) 0L;
    ((GodotObject) this).EmitSignal(NSettingsScreen.SignalName.SettingsOpened, Array.Empty<Variant>());
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(10)
    {
      new MethodInfo(NSettingsScreen.MethodName.SetIsInRun, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("isInRun"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NSettingsScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSettingsScreen.MethodName.OnSettingsTabChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSettingsScreen.MethodName.LocalizeLabels, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSettingsScreen.MethodName.OpenModdingScreen, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NSettingsScreen.MethodName.OpenFeedbackScreen, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NSettingsScreen.MethodName.OnSubmenuOpened, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSettingsScreen.MethodName.OnSubmenuClosed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSettingsScreen.MethodName.OnSubmenuHidden, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSettingsScreen.MethodName.OnSubmenuShown, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSettingsScreen.MethodName.SetIsInRun) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetIsInRun(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSettingsScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSettingsScreen.MethodName.OnSettingsTabChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSettingsTabChanged();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSettingsScreen.MethodName.LocalizeLabels) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.LocalizeLabels();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSettingsScreen.MethodName.OpenModdingScreen) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OpenModdingScreen(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSettingsScreen.MethodName.OpenFeedbackScreen) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OpenFeedbackScreen(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSettingsScreen.MethodName.OnSubmenuOpened) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuOpened();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSettingsScreen.MethodName.OnSubmenuClosed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuClosed();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSettingsScreen.MethodName.OnSubmenuHidden) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuHidden();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NSettingsScreen.MethodName.OnSubmenuShown) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnSubmenuShown();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSettingsScreen.MethodName.SetIsInRun) || StringName.op_Equality(ref method, NSettingsScreen.MethodName._Ready) || StringName.op_Equality(ref method, NSettingsScreen.MethodName.OnSettingsTabChanged) || StringName.op_Equality(ref method, NSettingsScreen.MethodName.LocalizeLabels) || StringName.op_Equality(ref method, NSettingsScreen.MethodName.OpenModdingScreen) || StringName.op_Equality(ref method, NSettingsScreen.MethodName.OpenFeedbackScreen) || StringName.op_Equality(ref method, NSettingsScreen.MethodName.OnSubmenuOpened) || StringName.op_Equality(ref method, NSettingsScreen.MethodName.OnSubmenuClosed) || StringName.op_Equality(ref method, NSettingsScreen.MethodName.OnSubmenuHidden) || StringName.op_Equality(ref method, NSettingsScreen.MethodName.OnSubmenuShown) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSettingsScreen.PropertyName._settingsTabManager))
    {
      this._settingsTabManager = VariantUtils.ConvertTo<NSettingsTabManager>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSettingsScreen.PropertyName._feedbackScreenButton))
    {
      this._feedbackScreenButton = VariantUtils.ConvertTo<NOpenFeedbackScreenButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSettingsScreen.PropertyName._moddingScreenButton))
    {
      this._moddingScreenButton = VariantUtils.ConvertTo<NOpenModdingScreenButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSettingsScreen.PropertyName._toast))
    {
      this._toast = VariantUtils.ConvertTo<NSettingsToast>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSettingsScreen.PropertyName._isInRun))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._isInRun = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSettingsScreen.PropertyName.InitialFocusedControl))
    {
      ref godot_variant local = ref value;
      Control initialFocusedControl = this.InitialFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref initialFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NSettingsScreen.PropertyName._settingsTabManager))
    {
      value = VariantUtils.CreateFrom<NSettingsTabManager>(ref this._settingsTabManager);
      return true;
    }
    if (StringName.op_Equality(ref name, NSettingsScreen.PropertyName._feedbackScreenButton))
    {
      value = VariantUtils.CreateFrom<NOpenFeedbackScreenButton>(ref this._feedbackScreenButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NSettingsScreen.PropertyName._moddingScreenButton))
    {
      value = VariantUtils.CreateFrom<NOpenModdingScreenButton>(ref this._moddingScreenButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NSettingsScreen.PropertyName._toast))
    {
      value = VariantUtils.CreateFrom<NSettingsToast>(ref this._toast);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSettingsScreen.PropertyName._isInRun))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<bool>(ref this._isInRun);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NSettingsScreen.PropertyName.InitialFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSettingsScreen.PropertyName._settingsTabManager, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSettingsScreen.PropertyName._feedbackScreenButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSettingsScreen.PropertyName._moddingScreenButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSettingsScreen.PropertyName._toast, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NSettingsScreen.PropertyName._isInRun, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NSettingsScreen.PropertyName._settingsTabManager, Variant.From<NSettingsTabManager>(ref this._settingsTabManager));
    info.AddProperty(NSettingsScreen.PropertyName._feedbackScreenButton, Variant.From<NOpenFeedbackScreenButton>(ref this._feedbackScreenButton));
    info.AddProperty(NSettingsScreen.PropertyName._moddingScreenButton, Variant.From<NOpenModdingScreenButton>(ref this._moddingScreenButton));
    info.AddProperty(NSettingsScreen.PropertyName._toast, Variant.From<NSettingsToast>(ref this._toast));
    info.AddProperty(NSettingsScreen.PropertyName._isInRun, Variant.From<bool>(ref this._isInRun));
    info.AddSignalEventDelegate(NSettingsScreen.SignalName.SettingsClosed, (Delegate) this.backing_SettingsClosed);
    info.AddSignalEventDelegate(NSettingsScreen.SignalName.SettingsOpened, (Delegate) this.backing_SettingsOpened);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NSettingsScreen.PropertyName._settingsTabManager, ref variant1))
      this._settingsTabManager = ((Variant) ref variant1).As<NSettingsTabManager>();
    Variant variant2;
    if (info.TryGetProperty(NSettingsScreen.PropertyName._feedbackScreenButton, ref variant2))
      this._feedbackScreenButton = ((Variant) ref variant2).As<NOpenFeedbackScreenButton>();
    Variant variant3;
    if (info.TryGetProperty(NSettingsScreen.PropertyName._moddingScreenButton, ref variant3))
      this._moddingScreenButton = ((Variant) ref variant3).As<NOpenModdingScreenButton>();
    Variant variant4;
    if (info.TryGetProperty(NSettingsScreen.PropertyName._toast, ref variant4))
      this._toast = ((Variant) ref variant4).As<NSettingsToast>();
    Variant variant5;
    if (info.TryGetProperty(NSettingsScreen.PropertyName._isInRun, ref variant5))
      this._isInRun = ((Variant) ref variant5).As<bool>();
    NSettingsScreen.SettingsClosedEventHandler closedEventHandler;
    if (info.TryGetSignalEventDelegate<NSettingsScreen.SettingsClosedEventHandler>(NSettingsScreen.SignalName.SettingsClosed, ref closedEventHandler))
      this.backing_SettingsClosed = closedEventHandler;
    NSettingsScreen.SettingsOpenedEventHandler openedEventHandler;
    if (!info.TryGetSignalEventDelegate<NSettingsScreen.SettingsOpenedEventHandler>(NSettingsScreen.SignalName.SettingsOpened, ref openedEventHandler))
      return;
    this.backing_SettingsOpened = openedEventHandler;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<MethodInfo> GetGodotSignalList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NSettingsScreen.SignalName.SettingsClosed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSettingsScreen.SignalName.SettingsOpened, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  public event NSettingsScreen.SettingsClosedEventHandler SettingsClosed
  {
    add => this.backing_SettingsClosed += value;
    remove => this.backing_SettingsClosed -= value;
  }

  protected void EmitSignalSettingsClosed()
  {
    ((GodotObject) this).EmitSignal(NSettingsScreen.SignalName.SettingsClosed, Array.Empty<Variant>());
  }

  public event NSettingsScreen.SettingsOpenedEventHandler SettingsOpened
  {
    add => this.backing_SettingsOpened += value;
    remove => this.backing_SettingsOpened -= value;
  }

  protected void EmitSignalSettingsOpened()
  {
    ((GodotObject) this).EmitSignal(NSettingsScreen.SignalName.SettingsOpened, Array.Empty<Variant>());
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RaiseGodotClassSignalCallbacks(
    in godot_string_name signal,
    NativeVariantPtrArgs args)
  {
    if (StringName.op_Equality(ref signal, NSettingsScreen.SignalName.SettingsClosed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NSettingsScreen.SettingsClosedEventHandler backingSettingsClosed = this.backing_SettingsClosed;
      if (backingSettingsClosed == null)
        return;
      backingSettingsClosed();
    }
    else if (StringName.op_Equality(ref signal, NSettingsScreen.SignalName.SettingsOpened) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NSettingsScreen.SettingsOpenedEventHandler backingSettingsOpened = this.backing_SettingsOpened;
      if (backingSettingsOpened == null)
        return;
      backingSettingsOpened();
    }
    else
      ((GodotObject) this).RaiseGodotClassSignalCallbacks(ref signal, args);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassSignal(in godot_string_name signal)
  {
    return StringName.op_Equality(ref signal, NSettingsScreen.SignalName.SettingsClosed) || StringName.op_Equality(ref signal, NSettingsScreen.SignalName.SettingsOpened) || base.HasGodotClassSignal(ref signal);
  }

  [Signal]
  public delegate void SettingsClosedEventHandler();

  [Signal]
  public delegate void SettingsOpenedEventHandler();

  public new class MethodName : NSubmenu.MethodName
  {
    public static readonly StringName SetIsInRun = StringName.op_Implicit(nameof (SetIsInRun));
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnSettingsTabChanged = StringName.op_Implicit(nameof (OnSettingsTabChanged));
    public static readonly StringName LocalizeLabels = StringName.op_Implicit(nameof (LocalizeLabels));
    public static readonly StringName OpenModdingScreen = StringName.op_Implicit(nameof (OpenModdingScreen));
    public static readonly StringName OpenFeedbackScreen = StringName.op_Implicit(nameof (OpenFeedbackScreen));
    public new static readonly StringName OnSubmenuOpened = StringName.op_Implicit(nameof (OnSubmenuOpened));
    public new static readonly StringName OnSubmenuClosed = StringName.op_Implicit(nameof (OnSubmenuClosed));
    public new static readonly StringName OnSubmenuHidden = StringName.op_Implicit(nameof (OnSubmenuHidden));
    public new static readonly StringName OnSubmenuShown = StringName.op_Implicit(nameof (OnSubmenuShown));
  }

  public new class PropertyName : NSubmenu.PropertyName
  {
    public new static readonly StringName InitialFocusedControl = StringName.op_Implicit(nameof (InitialFocusedControl));
    public static readonly StringName _settingsTabManager = StringName.op_Implicit(nameof (_settingsTabManager));
    public static readonly StringName _feedbackScreenButton = StringName.op_Implicit(nameof (_feedbackScreenButton));
    public static readonly StringName _moddingScreenButton = StringName.op_Implicit(nameof (_moddingScreenButton));
    public static readonly StringName _toast = StringName.op_Implicit(nameof (_toast));
    public static readonly StringName _isInRun = StringName.op_Implicit(nameof (_isInRun));
  }

  public new class SignalName : NSubmenu.SignalName
  {
    public static readonly StringName SettingsClosed = StringName.op_Implicit(nameof (SettingsClosed));
    public static readonly StringName SettingsOpened = StringName.op_Implicit(nameof (SettingsOpened));
  }
}
