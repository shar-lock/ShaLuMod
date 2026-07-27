// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.PauseMenu.NPauseMenu
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Nodes.Screens.Settings;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.PauseMenu;

[ScriptPath("res://src/Core/Nodes/Screens/PauseMenu/NPauseMenu.cs")]
public class NPauseMenu : NSubmenu
{
  private static readonly LocString _pausedLoc = new LocString("gameplay_ui", "PAUSE_MENU.PAUSED");
  private static readonly LocString _resumeLoc = new LocString("gameplay_ui", "PAUSE_MENU.RESUME");
  private static readonly LocString _settingsLoc = new LocString("gameplay_ui", "PAUSE_MENU.SETTINGS");
  private static readonly LocString _compendiumLoc = new LocString("gameplay_ui", "PAUSE_MENU.COMPENDIUM");
  private static readonly LocString _giveUpLoc = new LocString("gameplay_ui", "PAUSE_MENU.GIVE_UP");
  private static readonly LocString _disconnectLoc = new LocString("gameplay_ui", "PAUSE_MENU.DISCONNECT");
  private static readonly LocString _saveAndQuitLoc = new LocString("gameplay_ui", "PAUSE_MENU.SAVE_AND_QUIT");
  private NBackButton _backButton;
  private Control _buttonContainer;
  private NPauseMenuButton _resumeButton;
  private NPauseMenuButton _settingsButton;
  private NPauseMenuButton _compendiumButton;
  private NPauseMenuButton _giveUpButton;
  private NPauseMenuButton _disconnectButton;
  private NPauseMenuButton _saveAndQuitButton;
  private MegaLabel _pausedLabel;
  private IRunState _runState;

  protected override Control InitialFocusedControl => (Control) this._resumeButton;

  private NPauseMenuButton[] Buttons
  {
    get
    {
      return new NPauseMenuButton[6]
      {
        this._resumeButton,
        this._settingsButton,
        this._compendiumButton,
        this._giveUpButton,
        this._disconnectButton,
        this._saveAndQuitButton
      };
    }
  }

  public bool UseSharedBackstop => true;

  public NetScreenType ScreenType => NetScreenType.PauseMenu;

  public override void _Ready()
  {
    this.ConnectSignals();
    this._buttonContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%ButtonContainer"));
    this._resumeButton = ((Node) this._buttonContainer).GetNode<NPauseMenuButton>(NodePath.op_Implicit("Resume"));
    this._settingsButton = ((Node) this._buttonContainer).GetNode<NPauseMenuButton>(NodePath.op_Implicit("Settings"));
    this._compendiumButton = ((Node) this._buttonContainer).GetNode<NPauseMenuButton>(NodePath.op_Implicit("Compendium"));
    this._giveUpButton = ((Node) this._buttonContainer).GetNode<NPauseMenuButton>(NodePath.op_Implicit("GiveUp"));
    this._disconnectButton = ((Node) this._buttonContainer).GetNode<NPauseMenuButton>(NodePath.op_Implicit("Disconnect"));
    this._saveAndQuitButton = ((Node) this._buttonContainer).GetNode<NPauseMenuButton>(NodePath.op_Implicit("SaveAndQuit"));
    this._backButton = ((Node) this).GetNode<NBackButton>(NodePath.op_Implicit("%BackButton"));
    this._pausedLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%PausedText/Label"));
    this.RefreshLabels();
    ((GodotObject) this._resumeButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnBackOrResumeButtonPressed)), 0U);
    ((GodotObject) this._settingsButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnSettingsButtonPressed)), 0U);
    ((GodotObject) this._compendiumButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnCompendiumButtonPressed)), 0U);
    ((GodotObject) this._giveUpButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnGiveUpButtonPressed)), 0U);
    ((GodotObject) this._disconnectButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnDisconnectButtonPressed)), 0U);
    ((GodotObject) this._saveAndQuitButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnSaveAndQuitButtonPressed)), 0U);
    ((GodotObject) this._backButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnBackOrResumeButtonPressed)), 0U);
    this._backButton.Disable();
    ((CanvasItem) this._giveUpButton).Visible = RunManager.Instance.NetService.Type != NetGameType.Client;
    ((CanvasItem) this._saveAndQuitButton).Visible = RunManager.Instance.NetService.Type != NetGameType.Client;
    ((CanvasItem) this._disconnectButton).Visible = RunManager.Instance.NetService.Type == NetGameType.Client;
    for (int index = 0; index < this.Buttons.Length; ++index)
    {
      this.Buttons[index].FocusNeighborLeft = ((Node) this.Buttons[index]).GetPath();
      this.Buttons[index].FocusNeighborRight = ((Node) this.Buttons[index]).GetPath();
      NPauseMenuButton button = this.Buttons[index];
      NodePath path;
      if (index <= 0)
      {
        NPauseMenuButton[] buttons = this.Buttons;
        path = ((Node) buttons[buttons.Length - 1]).GetPath();
      }
      else
        path = ((Node) this.Buttons[index - 1]).GetPath();
      button.FocusNeighborTop = path;
      this.Buttons[index].FocusNeighborBottom = index < this.Buttons.Length - 1 ? ((Node) this.Buttons[index + 1]).GetPath() : ((Node) this.Buttons[0]).GetPath();
    }
  }

  public override void _ExitTree() => RunManager.Instance.IsPaused = false;

  private void RefreshLabels()
  {
    this._pausedLabel.SetTextAutoSize(NPauseMenu._pausedLoc.GetFormattedText());
    ((Node) this._resumeButton).GetNode<MegaLabel>(NodePath.op_Implicit("Label")).SetTextAutoSize(NPauseMenu._resumeLoc.GetFormattedText());
    ((Node) this._settingsButton).GetNode<MegaLabel>(NodePath.op_Implicit("Label")).SetTextAutoSize(NPauseMenu._settingsLoc.GetFormattedText());
    ((Node) this._compendiumButton).GetNode<MegaLabel>(NodePath.op_Implicit("Label")).SetTextAutoSize(NPauseMenu._compendiumLoc.GetFormattedText());
    ((Node) this._giveUpButton).GetNode<MegaLabel>(NodePath.op_Implicit("Label")).SetTextAutoSize(NPauseMenu._giveUpLoc.GetFormattedText());
    ((Node) this._disconnectButton).GetNode<MegaLabel>(NodePath.op_Implicit("Label")).SetTextAutoSize(NPauseMenu._disconnectLoc.GetFormattedText());
    ((Node) this._saveAndQuitButton).GetNode<MegaLabel>(NodePath.op_Implicit("Label")).SetTextAutoSize(NPauseMenu._saveAndQuitLoc.GetFormattedText());
  }

  public void Initialize(IRunState runState)
  {
    this._runState = runState;
    if (!RunManager.Instance.IsInProgress || this._runState.IsGameOver)
      this._giveUpButton.Disable();
    else
      this._giveUpButton.Enable();
    ((CanvasItem) this._compendiumButton).Visible = !NGame.IsReleaseGame() || SaveManager.Instance.IsCompendiumAvailable();
  }

  private void OnBackOrResumeButtonPressed(NButton _)
  {
    SfxCmd.Play("event:/sfx/ui/map/map_close");
    NCapstoneContainer.Instance.Close();
    NRun.Instance.GlobalUi.TopBar.Pause.ToggleAnimState();
  }

  private void OnSettingsButtonPressed(NButton _) => this._stack.PushSubmenuType<NSettingsScreen>();

  private void OnCompendiumButtonPressed(NButton _)
  {
    NCompendiumSubmenu submenuType = this._stack.GetSubmenuType<NCompendiumSubmenu>();
    submenuType.Initialize(this._runState);
    this._stack.Push((NSubmenu) submenuType);
  }

  private void OnGiveUpButtonPressed(NButton _)
  {
    NModalContainer.Instance.Add((Node) NAbandonRunConfirmPopup.Create((NMainMenu) null));
  }

  private void OnDisconnectButtonPressed(NButton _)
  {
    if (RunManager.Instance.NetService.IsConnected)
      NModalContainer.Instance.Add((Node) NDisconnectConfirmPopup.Create());
    else
      TaskHelper.RunSafely(NGame.Instance.ReturnToMainMenuAfterRun());
  }

  private void OnSaveAndQuitButtonPressed(NButton _) => TaskHelper.RunSafely(this.CloseToMenu());

  private async Task CloseToMenu()
  {
    this._resumeButton.Disable();
    this._settingsButton.Disable();
    this._compendiumButton.Disable();
    this._giveUpButton.Disable();
    this._disconnectButton.Disable();
    this._saveAndQuitButton.Disable();
    this._backButton.Disable();
    NRunMusicController.Instance.StopMusic();
    NDebugAudioManager.Instance?.StopAll();
    await NGame.Instance.ReturnToMainMenu();
  }

  public override void OnSubmenuOpened()
  {
    base.OnSubmenuOpened();
    RunManager.Instance.IsPaused = true;
    NHotkeyManager.Instance.AddBlockingScreen((Node) this);
  }

  public override void OnSubmenuClosed()
  {
    this._backButton.Disable();
    ((CanvasItem) this).Visible = false;
    RunManager.Instance.IsPaused = false;
    NHotkeyManager.Instance.RemoveBlockingScreen((Node) this);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(11)
    {
      new MethodInfo(NPauseMenu.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPauseMenu.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPauseMenu.MethodName.RefreshLabels, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPauseMenu.MethodName.OnBackOrResumeButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NPauseMenu.MethodName.OnSettingsButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NPauseMenu.MethodName.OnCompendiumButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NPauseMenu.MethodName.OnGiveUpButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NPauseMenu.MethodName.OnDisconnectButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NPauseMenu.MethodName.OnSaveAndQuitButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NPauseMenu.MethodName.OnSubmenuOpened, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPauseMenu.MethodName.OnSubmenuClosed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NPauseMenu.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPauseMenu.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPauseMenu.MethodName.RefreshLabels) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshLabels();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPauseMenu.MethodName.OnBackOrResumeButtonPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnBackOrResumeButtonPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPauseMenu.MethodName.OnSettingsButtonPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnSettingsButtonPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPauseMenu.MethodName.OnCompendiumButtonPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnCompendiumButtonPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPauseMenu.MethodName.OnGiveUpButtonPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnGiveUpButtonPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPauseMenu.MethodName.OnDisconnectButtonPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnDisconnectButtonPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPauseMenu.MethodName.OnSaveAndQuitButtonPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnSaveAndQuitButtonPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPauseMenu.MethodName.OnSubmenuOpened) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuOpened();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NPauseMenu.MethodName.OnSubmenuClosed) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnSubmenuClosed();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NPauseMenu.MethodName._Ready) || StringName.op_Equality(ref method, NPauseMenu.MethodName._ExitTree) || StringName.op_Equality(ref method, NPauseMenu.MethodName.RefreshLabels) || StringName.op_Equality(ref method, NPauseMenu.MethodName.OnBackOrResumeButtonPressed) || StringName.op_Equality(ref method, NPauseMenu.MethodName.OnSettingsButtonPressed) || StringName.op_Equality(ref method, NPauseMenu.MethodName.OnCompendiumButtonPressed) || StringName.op_Equality(ref method, NPauseMenu.MethodName.OnGiveUpButtonPressed) || StringName.op_Equality(ref method, NPauseMenu.MethodName.OnDisconnectButtonPressed) || StringName.op_Equality(ref method, NPauseMenu.MethodName.OnSaveAndQuitButtonPressed) || StringName.op_Equality(ref method, NPauseMenu.MethodName.OnSubmenuOpened) || StringName.op_Equality(ref method, NPauseMenu.MethodName.OnSubmenuClosed) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPauseMenu.PropertyName._backButton))
    {
      this._backButton = VariantUtils.ConvertTo<NBackButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPauseMenu.PropertyName._buttonContainer))
    {
      this._buttonContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPauseMenu.PropertyName._resumeButton))
    {
      this._resumeButton = VariantUtils.ConvertTo<NPauseMenuButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPauseMenu.PropertyName._settingsButton))
    {
      this._settingsButton = VariantUtils.ConvertTo<NPauseMenuButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPauseMenu.PropertyName._compendiumButton))
    {
      this._compendiumButton = VariantUtils.ConvertTo<NPauseMenuButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPauseMenu.PropertyName._giveUpButton))
    {
      this._giveUpButton = VariantUtils.ConvertTo<NPauseMenuButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPauseMenu.PropertyName._disconnectButton))
    {
      this._disconnectButton = VariantUtils.ConvertTo<NPauseMenuButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPauseMenu.PropertyName._saveAndQuitButton))
    {
      this._saveAndQuitButton = VariantUtils.ConvertTo<NPauseMenuButton>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPauseMenu.PropertyName._pausedLabel))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._pausedLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPauseMenu.PropertyName.InitialFocusedControl))
    {
      ref godot_variant local = ref value;
      Control initialFocusedControl = this.InitialFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref initialFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NPauseMenu.PropertyName.Buttons))
    {
      value = VariantUtils.CreateFromSystemArrayOfGodotObject((GodotObject[]) this.Buttons);
      return true;
    }
    if (StringName.op_Equality(ref name, NPauseMenu.PropertyName.UseSharedBackstop))
    {
      ref godot_variant local = ref value;
      bool useSharedBackstop = this.UseSharedBackstop;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref useSharedBackstop);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NPauseMenu.PropertyName.ScreenType))
    {
      ref godot_variant local = ref value;
      NetScreenType screenType = this.ScreenType;
      godot_variant from = VariantUtils.CreateFrom<NetScreenType>(ref screenType);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NPauseMenu.PropertyName._backButton))
    {
      value = VariantUtils.CreateFrom<NBackButton>(ref this._backButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NPauseMenu.PropertyName._buttonContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._buttonContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NPauseMenu.PropertyName._resumeButton))
    {
      value = VariantUtils.CreateFrom<NPauseMenuButton>(ref this._resumeButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NPauseMenu.PropertyName._settingsButton))
    {
      value = VariantUtils.CreateFrom<NPauseMenuButton>(ref this._settingsButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NPauseMenu.PropertyName._compendiumButton))
    {
      value = VariantUtils.CreateFrom<NPauseMenuButton>(ref this._compendiumButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NPauseMenu.PropertyName._giveUpButton))
    {
      value = VariantUtils.CreateFrom<NPauseMenuButton>(ref this._giveUpButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NPauseMenu.PropertyName._disconnectButton))
    {
      value = VariantUtils.CreateFrom<NPauseMenuButton>(ref this._disconnectButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NPauseMenu.PropertyName._saveAndQuitButton))
    {
      value = VariantUtils.CreateFrom<NPauseMenuButton>(ref this._saveAndQuitButton);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPauseMenu.PropertyName._pausedLabel))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<MegaLabel>(ref this._pausedLabel);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NPauseMenu.PropertyName._backButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPauseMenu.PropertyName._buttonContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPauseMenu.PropertyName._resumeButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPauseMenu.PropertyName._settingsButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPauseMenu.PropertyName._compendiumButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPauseMenu.PropertyName._giveUpButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPauseMenu.PropertyName._disconnectButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPauseMenu.PropertyName._saveAndQuitButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPauseMenu.PropertyName._pausedLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPauseMenu.PropertyName.InitialFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 28L, NPauseMenu.PropertyName.Buttons, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NPauseMenu.PropertyName.UseSharedBackstop, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NPauseMenu.PropertyName.ScreenType, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NPauseMenu.PropertyName._backButton, Variant.From<NBackButton>(ref this._backButton));
    info.AddProperty(NPauseMenu.PropertyName._buttonContainer, Variant.From<Control>(ref this._buttonContainer));
    info.AddProperty(NPauseMenu.PropertyName._resumeButton, Variant.From<NPauseMenuButton>(ref this._resumeButton));
    info.AddProperty(NPauseMenu.PropertyName._settingsButton, Variant.From<NPauseMenuButton>(ref this._settingsButton));
    info.AddProperty(NPauseMenu.PropertyName._compendiumButton, Variant.From<NPauseMenuButton>(ref this._compendiumButton));
    info.AddProperty(NPauseMenu.PropertyName._giveUpButton, Variant.From<NPauseMenuButton>(ref this._giveUpButton));
    info.AddProperty(NPauseMenu.PropertyName._disconnectButton, Variant.From<NPauseMenuButton>(ref this._disconnectButton));
    info.AddProperty(NPauseMenu.PropertyName._saveAndQuitButton, Variant.From<NPauseMenuButton>(ref this._saveAndQuitButton));
    info.AddProperty(NPauseMenu.PropertyName._pausedLabel, Variant.From<MegaLabel>(ref this._pausedLabel));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NPauseMenu.PropertyName._backButton, ref variant1))
      this._backButton = ((Variant) ref variant1).As<NBackButton>();
    Variant variant2;
    if (info.TryGetProperty(NPauseMenu.PropertyName._buttonContainer, ref variant2))
      this._buttonContainer = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NPauseMenu.PropertyName._resumeButton, ref variant3))
      this._resumeButton = ((Variant) ref variant3).As<NPauseMenuButton>();
    Variant variant4;
    if (info.TryGetProperty(NPauseMenu.PropertyName._settingsButton, ref variant4))
      this._settingsButton = ((Variant) ref variant4).As<NPauseMenuButton>();
    Variant variant5;
    if (info.TryGetProperty(NPauseMenu.PropertyName._compendiumButton, ref variant5))
      this._compendiumButton = ((Variant) ref variant5).As<NPauseMenuButton>();
    Variant variant6;
    if (info.TryGetProperty(NPauseMenu.PropertyName._giveUpButton, ref variant6))
      this._giveUpButton = ((Variant) ref variant6).As<NPauseMenuButton>();
    Variant variant7;
    if (info.TryGetProperty(NPauseMenu.PropertyName._disconnectButton, ref variant7))
      this._disconnectButton = ((Variant) ref variant7).As<NPauseMenuButton>();
    Variant variant8;
    if (info.TryGetProperty(NPauseMenu.PropertyName._saveAndQuitButton, ref variant8))
      this._saveAndQuitButton = ((Variant) ref variant8).As<NPauseMenuButton>();
    Variant variant9;
    if (!info.TryGetProperty(NPauseMenu.PropertyName._pausedLabel, ref variant9))
      return;
    this._pausedLabel = ((Variant) ref variant9).As<MegaLabel>();
  }

  public new class MethodName : NSubmenu.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName RefreshLabels = StringName.op_Implicit(nameof (RefreshLabels));
    public static readonly StringName OnBackOrResumeButtonPressed = StringName.op_Implicit(nameof (OnBackOrResumeButtonPressed));
    public static readonly StringName OnSettingsButtonPressed = StringName.op_Implicit(nameof (OnSettingsButtonPressed));
    public static readonly StringName OnCompendiumButtonPressed = StringName.op_Implicit(nameof (OnCompendiumButtonPressed));
    public static readonly StringName OnGiveUpButtonPressed = StringName.op_Implicit(nameof (OnGiveUpButtonPressed));
    public static readonly StringName OnDisconnectButtonPressed = StringName.op_Implicit(nameof (OnDisconnectButtonPressed));
    public static readonly StringName OnSaveAndQuitButtonPressed = StringName.op_Implicit(nameof (OnSaveAndQuitButtonPressed));
    public new static readonly StringName OnSubmenuOpened = StringName.op_Implicit(nameof (OnSubmenuOpened));
    public new static readonly StringName OnSubmenuClosed = StringName.op_Implicit(nameof (OnSubmenuClosed));
  }

  public new class PropertyName : NSubmenu.PropertyName
  {
    public new static readonly StringName InitialFocusedControl = StringName.op_Implicit(nameof (InitialFocusedControl));
    public static readonly StringName Buttons = StringName.op_Implicit(nameof (Buttons));
    public static readonly StringName UseSharedBackstop = StringName.op_Implicit(nameof (UseSharedBackstop));
    public static readonly StringName ScreenType = StringName.op_Implicit(nameof (ScreenType));
    public new static readonly StringName _backButton = StringName.op_Implicit(nameof (_backButton));
    public static readonly StringName _buttonContainer = StringName.op_Implicit(nameof (_buttonContainer));
    public static readonly StringName _resumeButton = StringName.op_Implicit(nameof (_resumeButton));
    public static readonly StringName _settingsButton = StringName.op_Implicit(nameof (_settingsButton));
    public static readonly StringName _compendiumButton = StringName.op_Implicit(nameof (_compendiumButton));
    public static readonly StringName _giveUpButton = StringName.op_Implicit(nameof (_giveUpButton));
    public static readonly StringName _disconnectButton = StringName.op_Implicit(nameof (_disconnectButton));
    public static readonly StringName _saveAndQuitButton = StringName.op_Implicit(nameof (_saveAndQuitButton));
    public static readonly StringName _pausedLabel = StringName.op_Implicit(nameof (_pausedLabel));
  }

  public new class SignalName : NSubmenu.SignalName
  {
  }
}
