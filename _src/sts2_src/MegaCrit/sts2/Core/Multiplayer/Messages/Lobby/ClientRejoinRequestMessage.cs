// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby.ClientRejoinRequestMessage
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using System.Runtime.InteropServices;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct ClientRejoinRequestMessage : INetMessage, IPacketSerializable
{
  public bool ShouldBroadcast => false;

  public NetTransferMode Mode => NetTransferMode.Reliable;

  public LogLevel LogLevel => LogLevel.Info;

  public bool ShouldBuffer => true;

  public void Serialize(PacketWriter writer)
  {
  }

  public void Deserialize(PacketReader reader)
  {
  }
}
