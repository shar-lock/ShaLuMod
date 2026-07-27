// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Transport.ENet.ENetPacket
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Multiplayer;
using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Transport.ENet;

public class ENetPacket
{
  private readonly byte[] _packetBytes;

  public ENetPacketType PacketType => (ENetPacketType) this._packetBytes[0];

  public byte[] AllBytes => this._packetBytes;

  public ENetPacket(byte[] bytes) => this._packetBytes = bytes;

  public static ENetPacket FromHandshakeRequest(ENetHandshakeRequest request)
  {
    byte[] bytes = new byte[9];
    bytes[0] = (byte) 0;
    Span<byte> span = MemoryExtensions.AsSpan<byte>(bytes);
    ref Span<byte> local = ref span;
    BinaryPrimitives.WriteUInt64BigEndian(local.Slice(1, local.Length - 1), request.netId);
    return new ENetPacket(bytes);
  }

  public ENetHandshakeRequest AsHandshakeRequest()
  {
    if (this.PacketType != ENetPacketType.HandshakeRequest)
      throw new InvalidOperationException($"Attempted to interpret ENet packet of type {this.PacketType} as handshake request");
    ulong num = BinaryPrimitives.ReadUInt64BigEndian(ReadOnlySpan<byte>.op_Implicit(RuntimeHelpers.GetSubArray<byte>(this._packetBytes, Range.StartAt(Index.op_Implicit(1)))));
    return new ENetHandshakeRequest() { netId = num };
  }

  public static ENetPacket FromHandshakeResponse(ENetHandshakeResponse response)
  {
    byte[] bytes = new byte[10];
    bytes[0] = (byte) 1;
    bytes[1] = (byte) response.status;
    Span<byte> span = MemoryExtensions.AsSpan<byte>(bytes);
    ref Span<byte> local = ref span;
    BinaryPrimitives.WriteUInt64BigEndian(local.Slice(2, local.Length - 2), response.netId);
    return new ENetPacket(bytes);
  }

  public ENetHandshakeResponse AsHandshakeResponse()
  {
    if (this.PacketType != ENetPacketType.HandshakeResponse)
      throw new InvalidOperationException($"Attempted to interpret ENet packet of type {this.PacketType} as handshake response");
    ENetHandshakeStatus packetByte = (ENetHandshakeStatus) this._packetBytes[1];
    ulong num = BinaryPrimitives.ReadUInt64BigEndian(ReadOnlySpan<byte>.op_Implicit(RuntimeHelpers.GetSubArray<byte>(this._packetBytes, Range.StartAt(Index.op_Implicit(2)))));
    return new ENetHandshakeResponse()
    {
      netId = num,
      status = packetByte
    };
  }

  public static ENetPacket FromDisconnection(ENetDisconnection disconnection)
  {
    return new ENetPacket(new byte[2]
    {
      (byte) 2,
      (byte) disconnection.reason
    });
  }

  public ENetDisconnection AsDisconnection()
  {
    if (this.PacketType != ENetPacketType.Disconnection)
      throw new InvalidOperationException($"Attempted to interpret ENet packet of type {this.PacketType} as disconnection");
    NetError packetByte = (NetError) this._packetBytes[1];
    return new ENetDisconnection() { reason = packetByte };
  }

  public static ENetPacket FromAppMessage(byte[] messageBytes, int count)
  {
    byte[] numArray = new byte[count + 1];
    numArray[0] = (byte) 3;
    Array.Copy((Array) messageBytes, 0, (Array) numArray, 1, count);
    return new ENetPacket(numArray);
  }

  public byte[] AsAppMessage()
  {
    if (this.PacketType != ENetPacketType.ApplicationMessage)
      throw new InvalidOperationException($"Attempted to interpret ENet packet of type {this.PacketType} as disconnection");
    return RuntimeHelpers.GetSubArray<byte>(this._packetBytes, Range.StartAt(Index.op_Implicit(1)));
  }
}
