// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Platform.Steam.SteamJoinCallbackHandler
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Connection;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Debug.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using MegaCrit.Sts2.Core.Runs;
using Steamworks;
using System;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Platform.Steam;

public class SteamJoinCallbackHandler : IDisposable
{
  private readonly Callback<GameLobbyJoinRequested_t> _steamJoinCallback;

  public SteamJoinCallbackHandler()
  {
    // ISSUE: method pointer
    this._steamJoinCallback = new Callback<GameLobbyJoinRequested_t>(new Callback<GameLobbyJoinRequested_t>.DispatchDelegate((object) this, __methodptr(OnSteamLobbyJoinRequested)), false);
  }

  public void CheckForCommandLineJoin()
  {
    string s;
    if (!CommandLineHelper.TryGetValue("+connect_lobby", out s))
      return;
    ulong lobbyId = ulong.Parse(s);
    Log.Info($"Joining to host via Steam invite that caused the game to launch. Lobby id: {lobbyId}");
    TaskHelper.RunSafely(SteamJoinCallbackHandler.JoinToHost(lobbyId, new ulong?()));
  }

  public void Dispose() => this._steamJoinCallback.Dispose();

  private void OnSteamLobbyJoinRequested(GameLobbyJoinRequested_t lobbyJoinRequest)
  {
    Log.Info($"Joining to host via Steam invite. Lobby: {lobbyJoinRequest.m_steamIDLobby.m_SteamID} player: {lobbyJoinRequest.m_steamIDFriend.m_SteamID}");
    TaskHelper.RunSafely(SteamJoinCallbackHandler.JoinToHost(lobbyJoinRequest.m_steamIDLobby.m_SteamID, new ulong?(lobbyJoinRequest.m_steamIDFriend.m_SteamID)));
  }

  private static async Task JoinToHost(
    ulong lobbyId,
    ulong? playerId,
    IClientConnectionInitializer? connInitializer = null)
  {
    if (NGame.Instance.RootSceneContainer.CurrentScene is NMultiplayerTest currentScene)
    {
      if (connInitializer == null)
        connInitializer = (IClientConnectionInitializer) SteamClientConnectionInitializer.FromLobby(lobbyId);
      await currentScene.JoinToHost(connInitializer);
    }
    else
    {
      if (RunManager.Instance.IsInProgress)
      {
        LocString body = new LocString("gameplay_ui", "QUIT_AND_JOIN_CONFIRMATION.body");
        playerId.GetValueOrDefault();
        if (!playerId.HasValue)
          playerId = new ulong?(SteamMatchmaking.GetLobbyOwner(new CSteamID(lobbyId)).m_SteamID);
        body.Add("host", PlatformUtil.GetPlayerName(PlatformType.Steam, playerId.Value));
        NGenericPopup modalToCreate = NGenericPopup.Create();
        NModalContainer.Instance.Add((Node) modalToCreate);
        if (!await modalToCreate.WaitForConfirmation(body, new LocString("gameplay_ui", "QUIT_AND_JOIN_CONFIRMATION.header"), new LocString("gameplay_ui", "QUIT_AND_JOIN_CONFIRMATION.cancel"), new LocString("gameplay_ui", "QUIT_AND_JOIN_CONFIRMATION.confirm")))
          return;
      }
      if (NGame.Instance.MainMenu == null)
        await NGame.Instance.ReturnToMainMenu();
      while (NGame.Instance.MainMenu?.SubmenuStack.Peek() != null)
        NGame.Instance.MainMenu?.SubmenuStack.Pop();
      if (connInitializer == null)
        connInitializer = (IClientConnectionInitializer) SteamClientConnectionInitializer.FromLobby(lobbyId);
      await NGame.Instance.MainMenu.JoinGame(connInitializer);
    }
  }
}
