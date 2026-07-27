// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Game.Lobby.IStartRunLobbyListener
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;

public interface IStartRunLobbyListener
{
  void PlayerConnected(LobbyPlayer player);

  void PlayerChanged(LobbyPlayer player, bool isRandomCharacterResolution);

  void AscensionChanged();

  void SeedChanged();

  void ModifiersChanged();

  void MaxAscensionChanged();

  void RemotePlayerDisconnected(LobbyPlayer player);

  void BeginRun(string seed, List<ActModel> acts, IReadOnlyList<ModifierModel> modifiers);

  void LocalPlayerDisconnected(NetErrorInfo info);
}
