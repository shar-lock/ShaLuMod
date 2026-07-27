// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NJoinFriendScreen
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Connection;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using MegaCrit.Sts2.Core.Nodes.Screens.CustomRun;
using MegaCrit.Sts2.Core.Nodes.Screens.DailyRun;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Platform.Steam;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;

[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NJoinFriendScreen.cs")]
public class NJoinFriendScreen : NSubmenu
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("screens/join_friend_submenu");
  private Control _buttonContainer;
  private Control _loadingOverlay;
  private Control _loadingFriendsIndicator;
  private MegaLabel _noFriendsLabel;
  private NJoinFriendRefreshButton _refreshButton;
  private Task? _refreshTask;
  private JoinFlow? _currentJoinFlow;

  protected override Control? InitialFocusedControl
  {
    get
    {
      return ((Node) this._buttonContainer).GetChildCount(false) <= 0 ? (Control) this._refreshButton : ((Node) this._buttonContainer).GetChild<Control>(0, false);
    }
  }

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[2]
      {
        NJoinFriendScreen._scenePath,
        NJoinFriendButton.scenePath
      });
    }
  }

  public bool DebugFriendsButtons => false;

  public static NJoinFriendScreen? Create()
  {
    return TestMode.IsOn ? (NJoinFriendScreen) null : PreloadManager.Cache.GetScene(NJoinFriendScreen._scenePath).Instantiate<NJoinFriendScreen>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._buttonContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%ButtonContainer"));
    this._loadingOverlay = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%LoadingOverlay"));
    this._loadingFriendsIndicator = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%LoadingIndicator"));
    this._noFriendsLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%NoFriendsText"));
    this._refreshButton = ((Node) this).GetNode<NJoinFriendRefreshButton>(NodePath.op_Implicit("%RefreshButton"));
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("TitleLabel")).SetTextAutoSize(new LocString("main_menu_ui", "JOIN_FRIENDS_MENU.title").GetFormattedText());
    this._noFriendsLabel.SetTextAutoSize(new LocString("main_menu_ui", "JOIN_FRIENDS_MENU.noFriends").GetFormattedText());
    ((GodotObject) this._refreshButton).Connect(NClickableControl.SignalName.Released, Callable.From<NClickableControl>((Action<NClickableControl>) (_ => this.RefreshButtonClicked())), 0U);
    ((CanvasItem) this._loadingFriendsIndicator).Visible = false;
    ((CanvasItem) this._noFriendsLabel).Visible = false;
  }

  public override void OnSubmenuOpened()
  {
    base.OnSubmenuOpened();
    ((CanvasItem) this._loadingOverlay).Visible = false;
    if ((!SteamInitializer.Initialized || CommandLineHelper.HasArg("fastmp")) && !this.DebugFriendsButtons)
      TaskHelper.RunSafely(this.FastMpJoin());
    else
      this._refreshTask = TaskHelper.RunSafely(this.ShowFriends());
  }

  public override void OnSubmenuClosed() => this._currentJoinFlow?.CancelToken.Cancel();

  private async Task FastMpJoin()
  {
    ulong netId = 1000;
    string s;
    if (CommandLineHelper.TryGetValue("clientId", out s))
      netId = ulong.Parse(s);
    DisplayServer.WindowSetTitle("Slay The Spire 2 (Client)", 0);
    await this.JoinGameAsync((IClientConnectionInitializer) new ENetClientConnectionInitializer(netId, "127.0.0.1", (ushort) 33771));
  }

  private void RefreshButtonClicked()
  {
    Task refreshTask = this._refreshTask;
    if (refreshTask != null && !refreshTask.IsCompleted)
      return;
    this._refreshTask = TaskHelper.RunSafely(this.RefreshButtonClickedAsync());
  }

  private async Task RefreshButtonClickedAsync()
  {
    ((CanvasItem) this._noFriendsLabel).Visible = false;
    if (SteamInitializer.Initialized)
    {
      ((CanvasItem) this._loadingFriendsIndicator).Visible = true;
      await Cmd.Wait(0.5f);
      ((CanvasItem) this._loadingFriendsIndicator).Visible = false;
    }
    await this.ShowFriends();
    Control initialFocusedControl = this.InitialFocusedControl;
    if (initialFocusedControl == null)
      return;
    initialFocusedControl.TryGrabFocus();
  }

  private async Task ShowFriends()
  {
    ((CanvasItem) this._loadingFriendsIndicator).Visible = true;
    foreach (Node child in ((Node) this._buttonContainer).GetChildren(false))
      child.QueueFreeSafely();
    if (SteamInitializer.Initialized)
    {
      foreach (ulong friendsWithOpenLobby in await PlatformUtil.GetFriendsWithOpenLobbies(PlatformType.Steam))
      {
        NJoinFriendButton child = NJoinFriendButton.Create(friendsWithOpenLobby);
        ((Node) this._buttonContainer).AddChildSafely((Node) child);
        SteamClientConnectionInitializer connInitializer = SteamClientConnectionInitializer.FromPlayer(friendsWithOpenLobby);
        ((GodotObject) child).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>((Action<NButton>) (_ => this.JoinGame((IClientConnectionInitializer) connInitializer))), 0U);
        NButton nbutton;
        if (nbutton == null)
          nbutton = (NButton) child;
      }
    }
    if (this.DebugFriendsButtons)
    {
      for (int index = 0; index < Rng.Chaotic.NextInt(5, 20); ++index)
        ((Node) this._buttonContainer).AddChildSafely((Node) NJoinFriendButton.Create(index == 0 ? 1UL : (ulong) (index * 1000)));
    }
    ActiveScreenContext.Instance.Update();
    ((CanvasItem) this._loadingFriendsIndicator).Visible = false;
    ((CanvasItem) this._noFriendsLabel).Visible = ((Node) this._buttonContainer).GetChildCount(false) == 0;
  }

  private void JoinGame(IClientConnectionInitializer connInitializer)
  {
    TaskHelper.RunSafely(this.JoinGameAsync(connInitializer));
  }

  public async Task JoinGameAsync(IClientConnectionInitializer connInitializer)
  {
    JoinFlow currentJoinFlow = this._currentJoinFlow;
    if ((currentJoinFlow != null ? (currentJoinFlow.NetService.IsConnected ? 1 : 0) : 0) != 0)
    {
      Log.Warn($"Tried to join game with connection {connInitializer} while we were already joining a game! Ignoring this attempt");
    }
    else
    {
      ((CanvasItem) this._loadingOverlay).Visible = true;
      this._currentJoinFlow = new JoinFlow((INetClientGameService) new NetClientGameService());
      try
      {
        Log.Info($"Attempting to join game with connection initializer {connInitializer}");
        JoinResult joinResult = await this._currentJoinFlow.Begin(connInitializer, ((Node) this).GetTree());
        if (joinResult.sessionState.GetValueOrDefault() == RunSessionState.InLobby)
        {
          if (joinResult.gameMode == GameMode.Standard)
          {
            NCharacterSelectScreen submenuType = this._stack.GetSubmenuType<NCharacterSelectScreen>();
            submenuType.InitializeMultiplayerAsClient((INetGameService) this._currentJoinFlow.NetService, joinResult.joinResponse.Value);
            this._stack.Push((NSubmenu) submenuType);
          }
          else if (joinResult.gameMode == GameMode.Daily)
          {
            NDailyRunScreen submenuType = this._stack.GetSubmenuType<NDailyRunScreen>();
            submenuType.InitializeMultiplayerAsClient((INetGameService) this._currentJoinFlow.NetService, joinResult.joinResponse.Value);
            this._stack.Push((NSubmenu) submenuType);
          }
          else
          {
            if (joinResult.gameMode != GameMode.Custom)
              throw new ArgumentOutOfRangeException("gameMode", (object) joinResult.gameMode, "Invalid game mode!");
            NCustomRunScreen submenuType = this._stack.GetSubmenuType<NCustomRunScreen>();
            submenuType.InitializeMultiplayerAsClient((INetGameService) this._currentJoinFlow.NetService, joinResult.joinResponse.Value);
            this._stack.Push((NSubmenu) submenuType);
          }
        }
        else if (joinResult.sessionState.GetValueOrDefault() == RunSessionState.InLoadedLobby)
        {
          if (joinResult.gameMode == GameMode.Standard)
          {
            NMultiplayerLoadGameScreen submenuType = this._stack.GetSubmenuType<NMultiplayerLoadGameScreen>();
            submenuType.InitializeAsClient((INetGameService) this._currentJoinFlow.NetService, joinResult.loadJoinResponse.Value);
            this._stack.Push((NSubmenu) submenuType);
          }
          else if (joinResult.gameMode == GameMode.Daily)
          {
            NDailyRunLoadScreen submenuType = this._stack.GetSubmenuType<NDailyRunLoadScreen>();
            submenuType.InitializeAsClient((INetGameService) this._currentJoinFlow.NetService, joinResult.loadJoinResponse.Value);
            this._stack.Push((NSubmenu) submenuType);
          }
          else
          {
            if (joinResult.gameMode != GameMode.Custom)
              return;
            NCustomRunLoadScreen submenuType = this._stack.GetSubmenuType<NCustomRunLoadScreen>();
            submenuType.InitializeAsClient((INetGameService) this._currentJoinFlow.NetService, joinResult.loadJoinResponse.Value);
            this._stack.Push((NSubmenu) submenuType);
          }
        }
        else
        {
          if (joinResult.sessionState.GetValueOrDefault() != RunSessionState.Running)
            return;
          NErrorPopup modalToCreate = NErrorPopup.Create(new NetErrorInfo(NetError.RunInProgress, false));
          if (modalToCreate != null)
            NModalContainer.Instance.Add((Node) modalToCreate);
          this._currentJoinFlow.NetService.Disconnect(NetError.RunInProgress);
        }
      }
      catch (ClientConnectionFailedException ex)
      {
        Log.Error($"Received connection failed exception while joining game: {ex}");
        NErrorPopup modalToCreate = NErrorPopup.Create(ex.info);
        if (modalToCreate != null)
          NModalContainer.Instance.Add((Node) modalToCreate);
        this._currentJoinFlow.NetService.Disconnect(ex.info.GetReason());
      }
      catch (OperationCanceledException ex)
      {
        Log.Warn("Joining was canceled by user");
      }
      catch (Exception ex)
      {
        Log.Error($"Received unexpected exception {ex.GetType()} while joining game! Disconnecting with InternalError");
        NErrorPopup modalToCreate = NErrorPopup.Create(new NetErrorInfo(NetError.InternalError, false));
        if (modalToCreate != null)
          NModalContainer.Instance.Add((Node) modalToCreate);
        this._currentJoinFlow.NetService.Disconnect(NetError.InternalError);
        throw;
      }
      finally
      {
        if (GodotObject.IsInstanceValid((GodotObject) this))
          ((CanvasItem) this._loadingOverlay).Visible = false;
      }
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NJoinFriendScreen.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NJoinFriendScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NJoinFriendScreen.MethodName.OnSubmenuOpened, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NJoinFriendScreen.MethodName.OnSubmenuClosed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NJoinFriendScreen.MethodName.RefreshButtonClicked, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NJoinFriendScreen.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NJoinFriendScreen njoinFriendScreen = NJoinFriendScreen.Create();
      ret = VariantUtils.CreateFrom<NJoinFriendScreen>(ref njoinFriendScreen);
      return true;
    }
    if (StringName.op_Equality(ref method, NJoinFriendScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NJoinFriendScreen.MethodName.OnSubmenuOpened) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuOpened();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NJoinFriendScreen.MethodName.OnSubmenuClosed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuClosed();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NJoinFriendScreen.MethodName.RefreshButtonClicked) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.RefreshButtonClicked();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NJoinFriendScreen.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NJoinFriendScreen njoinFriendScreen = NJoinFriendScreen.Create();
      ret = VariantUtils.CreateFrom<NJoinFriendScreen>(ref njoinFriendScreen);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NJoinFriendScreen.MethodName.Create) || StringName.op_Equality(ref method, NJoinFriendScreen.MethodName._Ready) || StringName.op_Equality(ref method, NJoinFriendScreen.MethodName.OnSubmenuOpened) || StringName.op_Equality(ref method, NJoinFriendScreen.MethodName.OnSubmenuClosed) || StringName.op_Equality(ref method, NJoinFriendScreen.MethodName.RefreshButtonClicked) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NJoinFriendScreen.PropertyName._buttonContainer))
    {
      this._buttonContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NJoinFriendScreen.PropertyName._loadingOverlay))
    {
      this._loadingOverlay = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NJoinFriendScreen.PropertyName._loadingFriendsIndicator))
    {
      this._loadingFriendsIndicator = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NJoinFriendScreen.PropertyName._noFriendsLabel))
    {
      this._noFriendsLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NJoinFriendScreen.PropertyName._refreshButton))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._refreshButton = VariantUtils.ConvertTo<NJoinFriendRefreshButton>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NJoinFriendScreen.PropertyName.InitialFocusedControl))
    {
      ref godot_variant local = ref value;
      Control initialFocusedControl = this.InitialFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref initialFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NJoinFriendScreen.PropertyName.DebugFriendsButtons))
    {
      ref godot_variant local = ref value;
      bool debugFriendsButtons = this.DebugFriendsButtons;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref debugFriendsButtons);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NJoinFriendScreen.PropertyName._buttonContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._buttonContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NJoinFriendScreen.PropertyName._loadingOverlay))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._loadingOverlay);
      return true;
    }
    if (StringName.op_Equality(ref name, NJoinFriendScreen.PropertyName._loadingFriendsIndicator))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._loadingFriendsIndicator);
      return true;
    }
    if (StringName.op_Equality(ref name, NJoinFriendScreen.PropertyName._noFriendsLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._noFriendsLabel);
      return true;
    }
    if (!StringName.op_Equality(ref name, NJoinFriendScreen.PropertyName._refreshButton))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<NJoinFriendRefreshButton>(ref this._refreshButton);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NJoinFriendScreen.PropertyName.InitialFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NJoinFriendScreen.PropertyName._buttonContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NJoinFriendScreen.PropertyName._loadingOverlay, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NJoinFriendScreen.PropertyName._loadingFriendsIndicator, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NJoinFriendScreen.PropertyName._noFriendsLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NJoinFriendScreen.PropertyName._refreshButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NJoinFriendScreen.PropertyName.DebugFriendsButtons, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NJoinFriendScreen.PropertyName._buttonContainer, Variant.From<Control>(ref this._buttonContainer));
    info.AddProperty(NJoinFriendScreen.PropertyName._loadingOverlay, Variant.From<Control>(ref this._loadingOverlay));
    info.AddProperty(NJoinFriendScreen.PropertyName._loadingFriendsIndicator, Variant.From<Control>(ref this._loadingFriendsIndicator));
    info.AddProperty(NJoinFriendScreen.PropertyName._noFriendsLabel, Variant.From<MegaLabel>(ref this._noFriendsLabel));
    info.AddProperty(NJoinFriendScreen.PropertyName._refreshButton, Variant.From<NJoinFriendRefreshButton>(ref this._refreshButton));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NJoinFriendScreen.PropertyName._buttonContainer, ref variant1))
      this._buttonContainer = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NJoinFriendScreen.PropertyName._loadingOverlay, ref variant2))
      this._loadingOverlay = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NJoinFriendScreen.PropertyName._loadingFriendsIndicator, ref variant3))
      this._loadingFriendsIndicator = ((Variant) ref variant3).As<Control>();
    Variant variant4;
    if (info.TryGetProperty(NJoinFriendScreen.PropertyName._noFriendsLabel, ref variant4))
      this._noFriendsLabel = ((Variant) ref variant4).As<MegaLabel>();
    Variant variant5;
    if (!info.TryGetProperty(NJoinFriendScreen.PropertyName._refreshButton, ref variant5))
      return;
    this._refreshButton = ((Variant) ref variant5).As<NJoinFriendRefreshButton>();
  }

  public new class MethodName : NSubmenu.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName OnSubmenuOpened = StringName.op_Implicit(nameof (OnSubmenuOpened));
    public new static readonly StringName OnSubmenuClosed = StringName.op_Implicit(nameof (OnSubmenuClosed));
    public static readonly StringName RefreshButtonClicked = StringName.op_Implicit(nameof (RefreshButtonClicked));
  }

  public new class PropertyName : NSubmenu.PropertyName
  {
    public new static readonly StringName InitialFocusedControl = StringName.op_Implicit(nameof (InitialFocusedControl));
    public static readonly StringName DebugFriendsButtons = StringName.op_Implicit(nameof (DebugFriendsButtons));
    public static readonly StringName _buttonContainer = StringName.op_Implicit(nameof (_buttonContainer));
    public static readonly StringName _loadingOverlay = StringName.op_Implicit(nameof (_loadingOverlay));
    public static readonly StringName _loadingFriendsIndicator = StringName.op_Implicit(nameof (_loadingFriendsIndicator));
    public static readonly StringName _noFriendsLabel = StringName.op_Implicit(nameof (_noFriendsLabel));
    public static readonly StringName _refreshButton = StringName.op_Implicit(nameof (_refreshButton));
  }

  public new class SignalName : NSubmenu.SignalName
  {
  }
}
