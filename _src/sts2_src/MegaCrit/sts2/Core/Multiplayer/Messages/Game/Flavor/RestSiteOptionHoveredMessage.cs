// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Flavor.RestSiteOptionHoveredMessage
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Runs;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Flavor;

public struct RestSiteOptionHoveredMessage : 
  INetMessage,
  IPacketSerializable,
  IRunLocationTargetedMessage
{
  public required uint? optionIndex;

  public bool ShouldBroadcast => true;

  public NetTransferMode Mode => NetTransferMode.Unreliable;

  public LogLevel LogLevel => LogLevel.VeryDebug;

  public bool ShouldBuffer => true;

  public RunLocation Location { get; set; }

  public void Serialize(PacketWriter writer)
  {
    writer.WriteBool(this.optionIndex.HasValue);
    if (this.optionIndex.HasValue)
      writer.WriteUInt(this.optionIndex.Value, 5);
    writer.Write<RunLocation>(this.Location);
  }

  public void Deserialize(PacketReader reader)
  {
    if (reader.ReadBool())
      this.optionIndex = new uint?(reader.ReadUInt(5));
    this.Location = reader.Read<RunLocation>();
  }
}
