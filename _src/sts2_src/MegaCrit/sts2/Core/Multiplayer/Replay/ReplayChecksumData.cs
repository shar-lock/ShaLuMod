// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Replay.ReplayChecksumData
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Replay;

public struct ReplayChecksumData : IPacketSerializable
{
  public NetChecksumData checksumData;
  public string context;
  public NetFullCombatState fullState;

  public void Serialize(PacketWriter writer)
  {
    writer.Write<NetChecksumData>(this.checksumData);
    writer.WriteString(this.context);
    writer.Write<NetFullCombatState>(this.fullState);
  }

  public void Deserialize(PacketReader reader)
  {
    this.checksumData = reader.Read<NetChecksumData>();
    this.context = reader.ReadString();
    this.fullState = reader.Read<NetFullCombatState>();
  }

  public ReplayChecksumData Anonymized()
  {
    return this with
    {
      fullState = this.fullState.Anonymized()
    };
  }
}
