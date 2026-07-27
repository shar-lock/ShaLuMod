// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Game.JoinResult
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby;
using MegaCrit.Sts2.Core.Runs;

#nullable disable
namespace MegaCrit.Sts2.Core.Multiplayer.Game;

public struct JoinResult
{
  public GameMode gameMode;
  public RunSessionState? sessionState;
  public ClientRejoinResponseMessage? rejoinResponse;
  public ClientLoadJoinResponseMessage? loadJoinResponse;
  public ClientLobbyJoinResponseMessage? joinResponse;
}
