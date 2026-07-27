// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Game.Lobby.ILoadRunLobbyListener
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Multiplayer;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;

public interface ILoadRunLobbyListener
{
  void PlayerConnected(ulong playerId);

  void RemotePlayerDisconnected(ulong playerId);

  Task<bool> ShouldAllowRunToBegin();

  void BeginRun();

  void PlayerReadyChanged(ulong playerId);

  void LocalPlayerDisconnected(NetErrorInfo info);
}
