// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Replay.CombatReplayEvent
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Replay;

public struct CombatReplayEvent : IPacketSerializable
{
  public CombatReplayEventType eventType;
  public ulong? playerId;
  public INetAction? action;
  public uint? hookId;
  public uint? actionId;
  public GameActionType? gameActionType;
  public uint? choiceId;
  public NetPlayerChoiceResult? playerChoiceResult;

  public void Serialize(PacketWriter writer)
  {
    writer.WriteInt((int) this.eventType, 3);
    switch (this.eventType)
    {
      case CombatReplayEventType.GameAction:
        writer.WriteULong(this.playerId.Value);
        writer.WriteByte((byte) this.action.ToId());
        writer.Write<INetAction>(this.action);
        break;
      case CombatReplayEventType.HookAction:
        writer.WriteULong(this.playerId.Value);
        writer.WriteUInt(this.hookId.Value);
        writer.WriteEnum<GameActionType>(this.gameActionType.Value);
        break;
      case CombatReplayEventType.ResumeAction:
        writer.WriteUInt(this.actionId.Value);
        break;
      case CombatReplayEventType.PlayerChoice:
        writer.WriteULong(this.playerId.Value);
        writer.WriteUInt(this.choiceId.Value);
        writer.Write<NetPlayerChoiceResult>(this.playerChoiceResult.Value);
        break;
      default:
        throw new ArgumentOutOfRangeException("eventType");
    }
  }

  public void Deserialize(PacketReader reader)
  {
    this.eventType = (CombatReplayEventType) reader.ReadInt(3);
    switch (this.eventType)
    {
      case CombatReplayEventType.GameAction:
        this.playerId = new ulong?(reader.ReadULong());
        int id = (int) reader.ReadByte();
        Type type;
        if (!ActionTypes.TryGetActionType(id, out type))
          throw new InvalidOperationException($"Received net action of type {id} that does not map to any type!");
        this.action = (INetAction) Activator.CreateInstance(type);
        this.action.Deserialize(reader);
        break;
      case CombatReplayEventType.HookAction:
        this.playerId = new ulong?(reader.ReadULong());
        this.hookId = new uint?(reader.ReadUInt());
        this.gameActionType = new GameActionType?(reader.ReadEnum<GameActionType>());
        break;
      case CombatReplayEventType.ResumeAction:
        this.actionId = new uint?(reader.ReadUInt());
        break;
      case CombatReplayEventType.PlayerChoice:
        this.playerId = new ulong?(reader.ReadULong());
        this.choiceId = new uint?(reader.ReadUInt());
        this.playerChoiceResult = new NetPlayerChoiceResult?(reader.Read<NetPlayerChoiceResult>());
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  public CombatReplayEvent Anonymized()
  {
    return this with
    {
      playerId = this.playerId.HasValue ? new ulong?(IdAnonymizer.Anonymize(this.playerId.Value)) : new ulong?()
    };
  }
}
