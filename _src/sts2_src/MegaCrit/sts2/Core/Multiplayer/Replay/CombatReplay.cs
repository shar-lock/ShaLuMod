// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Replay.CombatReplay
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Replay;

public class CombatReplay : IPacketSerializable
{
  public string version;
  public string gitCommit;
  public uint modelIdHash;
  public List<uint> choiceIds = new List<uint>();
  public List<int> rewardIds = new List<int>();
  public uint nextActionId;
  public uint nextChecksumId;
  public uint nextHookId;
  public SerializableRun serializableRun;
  public List<CombatReplayEvent> events = new List<CombatReplayEvent>();
  public List<ReplayChecksumData> checksumData = new List<ReplayChecksumData>();

  public void Serialize(PacketWriter writer)
  {
    writer.WriteString(this.version);
    writer.WriteString(this.gitCommit);
    writer.WriteUInt(this.modelIdHash);
    writer.WriteInt(this.choiceIds.Count);
    foreach (uint choiceId in this.choiceIds)
      writer.WriteUInt(choiceId);
    writer.WriteInt(this.rewardIds.Count);
    foreach (int rewardId in this.rewardIds)
      writer.WriteInt(rewardId);
    writer.WriteUInt(this.nextActionId);
    writer.WriteUInt(this.nextChecksumId);
    writer.WriteUInt(this.nextHookId);
    writer.Write<SerializableRun>(this.serializableRun);
    writer.WriteList<CombatReplayEvent>((IReadOnlyList<CombatReplayEvent>) this.events);
    writer.WriteList<ReplayChecksumData>((IReadOnlyList<ReplayChecksumData>) this.checksumData);
  }

  public void Deserialize(PacketReader reader)
  {
    this.version = reader.ReadString();
    this.gitCommit = reader.ReadString();
    this.modelIdHash = reader.ReadUInt();
    int num1 = reader.ReadInt();
    for (int index = 0; index < num1; ++index)
      this.choiceIds.Add(reader.ReadUInt());
    int num2 = reader.ReadInt();
    for (int index = 0; index < num2; ++index)
      this.rewardIds.Add(reader.ReadInt());
    this.nextActionId = reader.ReadUInt();
    this.nextChecksumId = reader.ReadUInt();
    this.nextHookId = reader.ReadUInt();
    this.serializableRun = reader.Read<SerializableRun>();
    this.events = reader.ReadList<CombatReplayEvent>();
    this.checksumData = reader.ReadList<ReplayChecksumData>();
  }

  public CombatReplay Anonymized()
  {
    return new CombatReplay()
    {
      version = this.version,
      gitCommit = this.gitCommit,
      modelIdHash = this.modelIdHash,
      choiceIds = this.choiceIds,
      rewardIds = this.rewardIds,
      nextActionId = this.nextActionId,
      nextChecksumId = this.nextChecksumId,
      nextHookId = this.nextHookId,
      serializableRun = this.serializableRun.Anonymized(),
      events = this.events.Select<CombatReplayEvent, CombatReplayEvent>((Func<CombatReplayEvent, CombatReplayEvent>) (e => e.Anonymized())).ToList<CombatReplayEvent>(),
      checksumData = this.checksumData.Select<ReplayChecksumData, ReplayChecksumData>((Func<ReplayChecksumData, ReplayChecksumData>) (c => c.Anonymized())).ToList<ReplayChecksumData>()
    };
  }
}
