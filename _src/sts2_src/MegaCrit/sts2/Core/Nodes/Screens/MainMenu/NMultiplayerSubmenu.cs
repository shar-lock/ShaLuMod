// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NMultiplayerSubmenu
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Daily;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using MegaCrit.Sts2.Core.Nodes.Screens.CustomRun;
using MegaCrit.Sts2.Core.Nodes.Screens.DailyRun;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Platform.Steam;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;

[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NMultiplayerSubmenu.cs")]
public class NMultiplayerSubmenu : NSubmenu
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("screens/multiplayer_submenu");
  private NSubmenuButton _hostButton;
  private NSubmenuButton _loadButton;
  private NSubmenuButton _abandonButton;
  private NSubmenuButton _joinButton;
  private const string _keyHost = "HOST";
  private const string _keyLoad = "MP_LOAD";
  private const string _keyJoin = "JOIN";
  private const string _keyAbandon = "MP_ABANDON";
  private Control _loadingOverlay;

  public static NMultiplayerSubmenu? Create()
  {
    return TestMode.IsOn ? (NMultiplayerSubmenu) null : PreloadManager.Cache.GetScene(NMultiplayerSubmenu._scenePath).Instantiate<NMultiplayerSubmenu>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._loadingOverlay = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%LoadingOverlay"));
    this._hostButton = ((Node) this).GetNode<NSubmenuButton>(NodePath.op_Implicit("ButtonContainer/HostButton"));
    ((GodotObject) this._hostButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnHostPressed)), 0U);
    this._hostButton.SetIconAndLocalization("HOST");
    this._loadButton = ((Node) this).GetNode<NSubmenuButton>(NodePath.op_Implicit("ButtonContainer/LoadButton"));
    ((GodotObject) this._loadButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.StartLoad)), 0U);
    this._loadButton.SetIconAndLocalization("MP_LOAD");
    this._joinButton = ((Node) this).GetNode<NSubmenuButton>(NodePath.op_Implicit("ButtonContainer/JoinButton"));
    ((GodotObject) this._joinButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OpenJoinFriendsScreen)), 0U);
    this._joinButton.SetIconAndLocalization("JOIN");
    this._abandonButton = ((Node) this).GetNode<NSubmenuButton>(NodePath.op_Implicit("ButtonContainer/AbandonButton"));
    ((GodotObject) this._abandonButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.AbandonRun)), 0U);
    this._abandonButton.SetIconAndLocalization("MP_ABANDON");
    this.UpdateButtons();
  }

  private void UpdateButtons()
  {
    ((CanvasItem) this._hostButton).Visible = !SaveManager.Instance.HasMultiplayerRunSave;
    ((CanvasItem) this._loadButton).Visible = SaveManager.Instance.HasMultiplayerRunSave;
    ((CanvasItem) this._abandonButton).Visible = SaveManager.Instance.HasMultiplayerRunSave;
  }

  protected override Control InitialFocusedControl
  {
    get
    {
      return SaveManager.Instance.HasMultiplayerRunSave ? (Control) this._loadButton : (Control) this._hostButton;
    }
  }

  private void AbandonRun(NButton _) => TaskHelper.RunSafely(this.TryAbandonMultiplayerRun());

  private async Task TryAbandonMultiplayerRun()
  {
    LocString header = new LocString("main_menu_ui", "ABANDON_RUN_CONFIRMATION.header");
    LocString body = new LocString("main_menu_ui", "ABANDON_RUN_CONFIRMATION.body");
    LocString yesButton = new LocString("main_menu_ui", "GENERIC_POPUP.confirm");
    LocString noButton = new LocString("main_menu_ui", "GENERIC_POPUP.cancel");
    NGenericPopup modalToCreate = NGenericPopup.Create();
    NModalContainer.Instance.Add((Node) modalToCreate);
    if (!await modalToCreate.WaitForConfirmation(body, header, noButton, yesButton))
      return;
    ulong localPlayerId = PlatformUtil.GetLocalPlayerId(PlatformUtil.PrimaryPlatform);
    ReadSaveResult<SerializableRun> readSaveResult = SaveManager.Instance.LoadAndCanonicalizeMultiplayerRunSave(localPlayerId);
    if (readSaveResult.Success)
    {
      if (readSaveResult.SaveData != null)
      {
        try
        {
          SerializableRun saveData = readSaveResult.SaveData;
          SaveManager.Instance.UpdateProgressWithRunData(saveData, false);
          RunHistoryUtilities.CreateRunHistoryEntry(saveData, false, true, saveData.PlatformType);
          DateTimeOffset? dailyTime = saveData.DailyTime;
          if (dailyTime.HasValue)
          {
            int dailyScore = ScoreUtility.CalculateDailyScore(saveData, localPlayerId, false);
            dailyTime = saveData.DailyTime;
            TaskHelper.RunSafely(DailyRunUtility.UploadScore(dailyTime.Value, dailyScore, saveData.Players));
            goto label_8;
          }
          goto label_8;
        }
        catch (Exception ex)
        {
          Log.Error($"ERROR: Failed to upload run history/metrics: {ex}");
          goto label_8;
        }
      }
    }
    Log.Error($"ERROR: Failed to load multiplayer run save: status={readSaveResult.Status}. Deleting current run...");
label_8:
    SaveManager.Instance.DeleteCurrentMultiplayerRun();
    this.UpdateButtons();
  }

  private void StartLoad(NButton _)
  {
    ReadSaveResult<SerializableRun> readSaveResult = SaveManager.Instance.LoadAndCanonicalizeMultiplayerRunSave(PlatformUtil.GetLocalPlayerId(!SteamInitializer.Initialized || CommandLineHelper.HasArg("fastmp") ? PlatformType.None : PlatformType.Steam));
    if (!readSaveResult.Success || readSaveResult.SaveData == null)
    {
      Log.Warn("Broken multiplayer run save detected, disabling button");
      this._loadButton.Disable();
      NModalContainer.Instance.Add((Node) NErrorPopup.Create(new LocString("main_menu_ui", "INVALID_SAVE_POPUP.title"), new LocString("main_menu_ui", "INVALID_SAVE_POPUP.description_run"), new LocString("main_menu_ui", "INVALID_SAVE_POPUP.dismiss"), true));
      NModalContainer.Instance.ShowBackstop();
    }
    else
      this.StartHost(readSaveResult.SaveData);
  }

  private void OnHostPressed(NButton _)
  {
    if (SaveManager.Instance.Progress.NumberOfRuns > 0)
      this._stack.PushSubmenuType<NMultiplayerHostSubmenu>();
    else
      TaskHelper.RunSafely(NMultiplayerHostSubmenu.StartHostAsync(GameMode.Standard, this._loadingOverlay, this._stack));
  }

  public void FastHost(GameMode gameMode)
  {
    this._stack.PushSubmenuType<NMultiplayerHostSubmenu>().StartHost(gameMode);
  }

  public void StartHost(SerializableRun run) => TaskHelper.RunSafely(this.StartHostAsync(run));

  private async Task StartHostAsync(SerializableRun run)
  {
    PlatformType platformType = !SteamInitializer.Initialized || CommandLineHelper.HasArg("fastmp") ? PlatformType.None : PlatformType.Steam;
    ((CanvasItem) this._loadingOverlay).Visible = true;
    try
    {
      NetHostGameService netService = new NetHostGameService();
      NetErrorInfo? nullable = new NetErrorInfo?();
      if (platformType == PlatformType.Steam)
        nullable = await netService.StartSteamHost(4);
      else
        netService.StartENetHost((ushort) 33771, 4);
      if (!nullable.HasValue)
      {
        switch (run.GameMode)
        {
          case GameMode.Daily:
            NDailyRunLoadScreen submenuType1 = this._stack.GetSubmenuType<NDailyRunLoadScreen>();
            submenuType1.InitializeAsHost((INetGameService) netService, run);
            this._stack.Push((NSubmenu) submenuType1);
            break;
          case GameMode.Custom:
            NCustomRunLoadScreen submenuType2 = this._stack.GetSubmenuType<NCustomRunLoadScreen>();
            submenuType2.InitializeAsHost((INetGameService) netService, run);
            this._stack.Push((NSubmenu) submenuType2);
            break;
          default:
            NMultiplayerLoadGameScreen submenuType3 = this._stack.GetSubmenuType<NMultiplayerLoadGameScreen>();
            submenuType3.InitializeAsHost((INetGameService) netService, run);
            this._stack.Push((NSubmenu) submenuType3);
            break;
        }
      }
      else
      {
        NErrorPopup modalToCreate = NErrorPopup.Create(nullable.Value);
        if (modalToCreate != null)
          NModalContainer.Instance.Add((Node) modalToCreate);
      }
      netService = (NetHostGameService) null;
    }
    finally
    {
      ((CanvasItem) this._loadingOverlay).Visible = false;
    }
  }

  private void OpenJoinFriendsScreen(NButton _) => this.OnJoinFriendsPressed();

  public NJoinFriendScreen OnJoinFriendsPressed()
  {
    return this._stack.PushSubmenuType<NJoinFriendScreen>();
  }

  protected override void OnSubmenuShown()
  {
    base.OnSubmenuShown();
    if (SaveManager.Instance.SeenFtue("multiplayer_warning") || SaveManager.Instance.Progress.NumberOfRuns != 0 || CommandLineHelper.HasArg("fastmp"))
      return;
    NModalContainer.Instance.Add((Node) NMultiplayerWarningPopup.Create());
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(10)
    {
      new MethodInfo(NMultiplayerSubmenu.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerSubmenu.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerSubmenu.MethodName.UpdateButtons, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerSubmenu.MethodName.AbandonRun, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMultiplayerSubmenu.MethodName.StartLoad, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMultiplayerSubmenu.MethodName.OnHostPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMultiplayerSubmenu.MethodName.FastHost, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("gameMode"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMultiplayerSubmenu.MethodName.OpenJoinFriendsScreen, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMultiplayerSubmenu.MethodName.OnJoinFriendsPressed, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerSubmenu.MethodName.OnSubmenuShown, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMultiplayerSubmenu.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NMultiplayerSubmenu nmultiplayerSubmenu = NMultiplayerSubmenu.Create();
      ret = VariantUtils.CreateFrom<NMultiplayerSubmenu>(ref nmultiplayerSubmenu);
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerSubmenu.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerSubmenu.MethodName.UpdateButtons) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateButtons();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerSubmenu.MethodName.AbandonRun) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.AbandonRun(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerSubmenu.MethodName.StartLoad) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.StartLoad(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerSubmenu.MethodName.OnHostPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnHostPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerSubmenu.MethodName.FastHost) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.FastHost(VariantUtils.ConvertTo<GameMode>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerSubmenu.MethodName.OpenJoinFriendsScreen) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OpenJoinFriendsScreen(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerSubmenu.MethodName.OnJoinFriendsPressed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NJoinFriendScreen njoinFriendScreen = this.OnJoinFriendsPressed();
      ret = VariantUtils.CreateFrom<NJoinFriendScreen>(ref njoinFriendScreen);
      return true;
    }
    if (!StringName.op_Equality(ref method, NMultiplayerSubmenu.MethodName.OnSubmenuShown) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnSubmenuShown();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMultiplayerSubmenu.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NMultiplayerSubmenu nmultiplayerSubmenu = NMultiplayerSubmenu.Create();
      ret = VariantUtils.CreateFrom<NMultiplayerSubmenu>(ref nmultiplayerSubmenu);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMultiplayerSubmenu.MethodName.Create) || StringName.op_Equality(ref method, NMultiplayerSubmenu.MethodName._Ready) || StringName.op_Equality(ref method, NMultiplayerSubmenu.MethodName.UpdateButtons) || StringName.op_Equality(ref method, NMultiplayerSubmenu.MethodName.AbandonRun) || StringName.op_Equality(ref method, NMultiplayerSubmenu.MethodName.StartLoad) || StringName.op_Equality(ref method, NMultiplayerSubmenu.MethodName.OnHostPressed) || StringName.op_Equality(ref method, NMultiplayerSubmenu.MethodName.FastHost) || StringName.op_Equality(ref method, NMultiplayerSubmenu.MethodName.OpenJoinFriendsScreen) || StringName.op_Equality(ref method, NMultiplayerSubmenu.MethodName.OnJoinFriendsPressed) || StringName.op_Equality(ref method, NMultiplayerSubmenu.MethodName.OnSubmenuShown) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMultiplayerSubmenu.PropertyName._hostButton))
    {
      this._hostButton = VariantUtils.ConvertTo<NSubmenuButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerSubmenu.PropertyName._loadButton))
    {
      this._loadButton = VariantUtils.ConvertTo<NSubmenuButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerSubmenu.PropertyName._abandonButton))
    {
      this._abandonButton = VariantUtils.ConvertTo<NSubmenuButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerSubmenu.PropertyName._joinButton))
    {
      this._joinButton = VariantUtils.ConvertTo<NSubmenuButton>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMultiplayerSubmenu.PropertyName._loadingOverlay))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._loadingOverlay = VariantUtils.ConvertTo<Control>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMultiplayerSubmenu.PropertyName.InitialFocusedControl))
    {
      ref godot_variant local = ref value;
      Control initialFocusedControl = this.InitialFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref initialFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerSubmenu.PropertyName._hostButton))
    {
      value = VariantUtils.CreateFrom<NSubmenuButton>(ref this._hostButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerSubmenu.PropertyName._loadButton))
    {
      value = VariantUtils.CreateFrom<NSubmenuButton>(ref this._loadButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerSubmenu.PropertyName._abandonButton))
    {
      value = VariantUtils.CreateFrom<NSubmenuButton>(ref this._abandonButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerSubmenu.PropertyName._joinButton))
    {
      value = VariantUtils.CreateFrom<NSubmenuButton>(ref this._joinButton);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMultiplayerSubmenu.PropertyName._loadingOverlay))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Control>(ref this._loadingOverlay);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NMultiplayerSubmenu.PropertyName._hostButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerSubmenu.PropertyName._loadButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerSubmenu.PropertyName._abandonButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerSubmenu.PropertyName._joinButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerSubmenu.PropertyName._loadingOverlay, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerSubmenu.PropertyName.InitialFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NMultiplayerSubmenu.PropertyName._hostButton, Variant.From<NSubmenuButton>(ref this._hostButton));
    info.AddProperty(NMultiplayerSubmenu.PropertyName._loadButton, Variant.From<NSubmenuButton>(ref this._loadButton));
    info.AddProperty(NMultiplayerSubmenu.PropertyName._abandonButton, Variant.From<NSubmenuButton>(ref this._abandonButton));
    info.AddProperty(NMultiplayerSubmenu.PropertyName._joinButton, Variant.From<NSubmenuButton>(ref this._joinButton));
    info.AddProperty(NMultiplayerSubmenu.PropertyName._loadingOverlay, Variant.From<Control>(ref this._loadingOverlay));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMultiplayerSubmenu.PropertyName._hostButton, ref variant1))
      this._hostButton = ((Variant) ref variant1).As<NSubmenuButton>();
    Variant variant2;
    if (info.TryGetProperty(NMultiplayerSubmenu.PropertyName._loadButton, ref variant2))
      this._loadButton = ((Variant) ref variant2).As<NSubmenuButton>();
    Variant variant3;
    if (info.TryGetProperty(NMultiplayerSubmenu.PropertyName._abandonButton, ref variant3))
      this._abandonButton = ((Variant) ref variant3).As<NSubmenuButton>();
    Variant variant4;
    if (info.TryGetProperty(NMultiplayerSubmenu.PropertyName._joinButton, ref variant4))
      this._joinButton = ((Variant) ref variant4).As<NSubmenuButton>();
    Variant variant5;
    if (!info.TryGetProperty(NMultiplayerSubmenu.PropertyName._loadingOverlay, ref variant5))
      return;
    this._loadingOverlay = ((Variant) ref variant5).As<Control>();
  }

  public new class MethodName : NSubmenu.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName UpdateButtons = StringName.op_Implicit(nameof (UpdateButtons));
    public static readonly StringName AbandonRun = StringName.op_Implicit(nameof (AbandonRun));
    public static readonly StringName StartLoad = StringName.op_Implicit(nameof (StartLoad));
    public static readonly StringName OnHostPressed = StringName.op_Implicit(nameof (OnHostPressed));
    public static readonly StringName FastHost = StringName.op_Implicit(nameof (FastHost));
    public static readonly StringName OpenJoinFriendsScreen = StringName.op_Implicit(nameof (OpenJoinFriendsScreen));
    public static readonly StringName OnJoinFriendsPressed = StringName.op_Implicit(nameof (OnJoinFriendsPressed));
    public new static readonly StringName OnSubmenuShown = StringName.op_Implicit(nameof (OnSubmenuShown));
  }

  public new class PropertyName : NSubmenu.PropertyName
  {
    public new static readonly StringName InitialFocusedControl = StringName.op_Implicit(nameof (InitialFocusedControl));
    public static readonly StringName _hostButton = StringName.op_Implicit(nameof (_hostButton));
    public static readonly StringName _loadButton = StringName.op_Implicit(nameof (_loadButton));
    public static readonly StringName _abandonButton = StringName.op_Implicit(nameof (_abandonButton));
    public static readonly StringName _joinButton = StringName.op_Implicit(nameof (_joinButton));
    public static readonly StringName _loadingOverlay = StringName.op_Implicit(nameof (_loadingOverlay));
  }

  public new class SignalName : NSubmenu.SignalName
  {
  }
}
