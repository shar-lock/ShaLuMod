// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby.InitialGameInfoMessage
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Runs;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby;

public struct InitialGameInfoMessage : INetMessage, IPacketSerializable
{
  public string version;
  public PlatformBranch branch;
  public uint idDatabaseHash;
  public List<string>? gameplayAffectingMods;
  public List<string>? otherMods;
  public GameMode gameMode;
  public RunSessionState sessionState;
  public ConnectionFailureReason? connectionFailureReason;

  public bool ShouldBroadcast => false;

  public NetTransferMode Mode => NetTransferMode.Reliable;

  public LogLevel LogLevel => LogLevel.Info;

  public bool ShouldBuffer => true;

  public static InitialGameInfoMessage Basic()
  {
    return new InitialGameInfoMessage()
    {
      version = NGame.GetGameVersion(),
      branch = PlatformUtil.GetPlatformBranch(),
      idDatabaseHash = ModelIdSerializationCache.Hash,
      gameplayAffectingMods = ModManager.GetGameplayRelevantModNameList(),
      otherMods = ModManager.GetNonGameplayRelevantModNameList()
    };
  }

  public void Serialize(PacketWriter writer)
  {
    writer.WriteString(this.version);
    writer.WriteEnum<PlatformBranch>(this.branch);
    writer.WriteUInt(this.idDatabaseHash);
    writer.WriteEnum<GameMode>(this.gameMode);
    writer.WriteEnum<RunSessionState>(this.sessionState);
    writer.WriteBool(this.connectionFailureReason.HasValue);
    if (this.connectionFailureReason.HasValue)
      writer.WriteEnum<ConnectionFailureReason>(this.connectionFailureReason.Value);
    writer.WriteBool(this.gameplayAffectingMods != null);
    if (this.gameplayAffectingMods != null)
    {
      writer.WriteInt(this.gameplayAffectingMods.Count);
      foreach (string gameplayAffectingMod in this.gameplayAffectingMods)
        writer.WriteString(gameplayAffectingMod);
    }
    writer.WriteBool(this.otherMods != null);
    if (this.otherMods == null)
      return;
    writer.WriteInt(this.otherMods.Count);
    foreach (string otherMod in this.otherMods)
      writer.WriteString(otherMod);
  }

  public void Deserialize(PacketReader reader)
  {
    this.version = reader.ReadString();
    this.branch = reader.ReadEnum<PlatformBranch>();
    this.idDatabaseHash = reader.ReadUInt();
    this.gameMode = reader.ReadEnum<GameMode>();
    this.sessionState = reader.ReadEnum<RunSessionState>();
    if (reader.ReadBool())
      this.connectionFailureReason = new ConnectionFailureReason?(reader.ReadEnum<ConnectionFailureReason>());
    if (reader.ReadBool())
    {
      int num = reader.ReadInt();
      this.gameplayAffectingMods = new List<string>();
      for (int index = 0; index < num; ++index)
        this.gameplayAffectingMods.Add(reader.ReadString());
    }
    if (!reader.ReadBool())
      return;
    int num1 = reader.ReadInt();
    this.otherMods = new List<string>();
    for (int index = 0; index < num1; ++index)
      this.otherMods.Add(reader.ReadString());
  }
}
