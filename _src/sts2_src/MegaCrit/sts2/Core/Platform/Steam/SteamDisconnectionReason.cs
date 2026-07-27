// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Platform.Steam.SteamDisconnectionReason
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

#nullable disable
namespace MegaCrit.Sts2.Core.Platform.Steam;

public enum SteamDisconnectionReason
{
  None = 0,
  AppGeneric = 1000, // 0x000003E8
  AppInternalError = 1017, // 0x000003F9
  AppException = 2000, // 0x000007D0
  LocalMin = 3000, // 0x00000BB8
  RunningInOfflineMode = 3001, // 0x00000BB9
  ManyRelayConnectivity = 3002, // 0x00000BBA
  HostedServerPrimaryRelay = 3003, // 0x00000BBB
  NetworkConfig = 3004, // 0x00000BBC
  LocalRights = 3005, // 0x00000BBD
  LocalMax = 3999, // 0x00000F9F
  RemoteTimeout = 4001, // 0x00000FA1
  BadCrypt = 4002, // 0x00000FA2
  BadCert = 4003, // 0x00000FA3
  NotLoggedIn = 4004, // 0x00000FA4
  NotRunningApp = 4005, // 0x00000FA5
  BadProtocolVersion = 4006, // 0x00000FA6
  MiscGeneric = 5001, // 0x00001389
  InternalError = 5002, // 0x0000138A
  MiscTimeout = 5003, // 0x0000138B
  RelayConnectivity = 5004, // 0x0000138C
  SteamConnectivity = 5005, // 0x0000138D
  NoRelaySessions = 5006, // 0x0000138E
  RendezvousTimeout = 5008, // 0x00001390
  RelayReceivedUnexpectedNoConnectionPacket = 5010, // 0x00001392
}
