// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Transport.Steam.SteamUtil
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Logging;
using Steamworks;
using System;
using System.Runtime.InteropServices;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Transport.Steam;

public static class SteamUtil
{
  public const uint handshakeMagicBytes = 2204332656;
  private static readonly IntPtr[] _messageBuffer = new IntPtr[64 /*0x40*/];

  public static NetTransferMode ModeFromFlags(int flags)
  {
    if ((flags & 8) > 0)
      return NetTransferMode.Reliable;
    if (flags == 0)
      return NetTransferMode.Unreliable;
    throw new ArgumentOutOfRangeException();
  }

  public static int FlagsFromMode(NetTransferMode mode)
  {
    if (mode == NetTransferMode.Unreliable)
      return 0;
    if (mode == NetTransferMode.Reliable)
      return 8;
    throw new ArgumentOutOfRangeException(nameof (mode), (object) mode, (string) null);
  }

  public static SteamNetworkingIdentity ToNetId(this CSteamID id)
  {
    SteamNetworkingIdentity netId = new SteamNetworkingIdentity();
    ((SteamNetworkingIdentity) ref netId).SetSteamID(id);
    return netId;
  }

  public static SteamNetworkingIdentity ToNetId64(this ulong id)
  {
    SteamNetworkingIdentity netId64 = new SteamNetworkingIdentity();
    ((SteamNetworkingIdentity) ref netId64).SetSteamID64(id);
    return netId64;
  }

  public static void ProcessMessages(HSteamNetConnection conn, INetHandler handler, Logger logger)
  {
    int messagesOnConnection;
    do
    {
      messagesOnConnection = SteamNetworkingSockets.ReceiveMessagesOnConnection(conn, SteamUtil._messageBuffer, SteamUtil._messageBuffer.Length);
      if (messagesOnConnection > 0)
        logger.VeryDebug($"Processing {messagesOnConnection} packets");
      for (int index = 0; index < messagesOnConnection; ++index)
      {
        IntPtr ptr = SteamUtil._messageBuffer[index];
        SteamNetworkingMessage_t structure = Marshal.PtrToStructure<SteamNetworkingMessage_t>(ptr);
        byte[] numArray = new byte[structure.m_cbSize];
        Marshal.Copy(structure.m_pData, numArray, 0, structure.m_cbSize);
        NetTransferMode mode = SteamUtil.ModeFromFlags(structure.m_nFlags);
        logger.VeryDebug($"Received packet of size {numArray.Length} from sender {((SteamNetworkingIdentity) ref structure.m_identityPeer).GetSteamID64()} ({mode}, {structure.m_nChannel})");
        handler.OnPacketReceived(((SteamNetworkingIdentity) ref structure.m_identityPeer).GetSteamID().m_SteamID, numArray, mode, structure.m_nChannel);
        SteamNetworkingMessage_t.Release(ptr);
      }
    }
    while (messagesOnConnection == SteamUtil._messageBuffer.Length);
  }
}
