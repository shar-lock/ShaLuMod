// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Sync.OptionIndexChosenMessage
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Runs;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Sync;

public struct OptionIndexChosenMessage : 
  INetMessage,
  IPacketSerializable,
  IRunLocationTargetedMessage
{
  public OptionIndexType type;
  public uint optionIndex;
  public RunLocation location;

  public bool ShouldBroadcast => true;

  public NetTransferMode Mode => NetTransferMode.Reliable;

  public LogLevel LogLevel => LogLevel.VeryDebug;

  public bool ShouldBuffer => true;

  public RunLocation Location => this.location;

  public void Serialize(PacketWriter writer)
  {
    writer.WriteEnum<OptionIndexType>(this.type);
    writer.WriteUInt(this.optionIndex, 4);
    writer.Write<RunLocation>(this.location);
  }

  public void Deserialize(PacketReader reader)
  {
    this.type = reader.ReadEnum<OptionIndexType>();
    this.optionIndex = reader.ReadUInt(4);
    this.location = reader.Read<RunLocation>();
  }

  public override string ToString()
  {
    return $"{nameof (OptionIndexChosenMessage)} type {this.type} index {this.optionIndex}";
  }
}
