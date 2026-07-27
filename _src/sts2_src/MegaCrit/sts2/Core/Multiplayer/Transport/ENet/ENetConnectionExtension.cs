// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Transport.ENet.ENetConnectionExtension
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Collections;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Transport.ENet;

public static class ENetConnectionExtension
{
  public static bool TryService(this ENetConnection connection, out ENetServiceData? output)
  {
    Array array = connection.Service(0);
    output = new ENetServiceData?();
    if (array == null)
      return false;
    Variant variant1 = array[0];
    ENetConnection.EventType eventType = ((Variant) ref variant1).As<ENetConnection.EventType>();
    if (eventType == null)
      return false;
    ENetServiceData enetServiceData1 = new ENetServiceData();
    enetServiceData1.type = eventType;
    ref ENetServiceData local1 = ref enetServiceData1;
    Variant variant2 = array[1];
    ENetPacketPeer enetPacketPeer = ((Variant) ref variant2).As<ENetPacketPeer>();
    local1.peer = enetPacketPeer;
    enetServiceData1.originalData = array;
    ENetServiceData enetServiceData2 = enetServiceData1;
    if (eventType == 3L)
    {
      ref ENetServiceData local2 = ref enetServiceData2;
      Variant variant3 = array[3];
      int num = ((Variant) ref variant3).As<int>();
      local2.channel = num;
      enetServiceData2.packetData = ((PacketPeer) enetServiceData2.peer).GetPacket();
      enetServiceData2.error = ((PacketPeer) enetServiceData2.peer).GetPacketError();
      enetServiceData2.mode = NetTransferMode.None;
    }
    output = new ENetServiceData?(enetServiceData2);
    return true;
  }
}
