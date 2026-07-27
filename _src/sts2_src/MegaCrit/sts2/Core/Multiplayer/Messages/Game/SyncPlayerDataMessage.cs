// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Messages.Game.SyncPlayerDataMessage
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Saves.Runs;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Game;

public struct SyncPlayerDataMessage : INetMessage, IPacketSerializable
{
  public SerializablePlayer player;

  public bool ShouldBroadcast => true;

  public NetTransferMode Mode => NetTransferMode.Reliable;

  public LogLevel LogLevel => LogLevel.VeryDebug;

  public bool ShouldBuffer => true;

  public void Serialize(PacketWriter writer) => writer.Write<SerializablePlayer>(this.player);

  public void Deserialize(PacketReader reader) => this.player = reader.Read<SerializablePlayer>();
}
