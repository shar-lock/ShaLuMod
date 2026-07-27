// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Multiplayer.NetError
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

#nullable disable
namespace MegaCrit.Sts2.Core.Entities.Multiplayer;

public enum NetError
{
  None,
  Quit,
  QuitGameOver,
  HostAbandoned,
  Kicked,
  InvalidJoin,
  CancelledJoin,
  LobbyFull,
  RunInProgress,
  NotInSaveGame,
  VersionMismatch,
  JoinBlockedByUser,
  StateDivergence,
  HandshakeTimeout,
  ModMismatch,
  NoInternet,
  Timeout,
  InternalError,
  UnknownNetworkError,
  RateLimited,
  TryAgainLater,
  FailedToHost,
  SecureConnectionFailed,
}
