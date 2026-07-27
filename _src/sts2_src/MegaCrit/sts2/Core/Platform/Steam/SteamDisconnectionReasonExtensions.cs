// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Platform.Steam.SteamDisconnectionReasonExtensions
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using System;

#nullable disable
namespace MegaCrit.Sts2.Core.Platform.Steam;

public static class SteamDisconnectionReasonExtensions
{
  public static SteamDisconnectionReason ToSteam(this NetError reason)
  {
    return (SteamDisconnectionReason) (1000 + reason);
  }

  public static NetError ToApp(this SteamDisconnectionReason steamReason)
  {
    if (steamReason >= SteamDisconnectionReason.AppGeneric && steamReason <= (SteamDisconnectionReason) 1999)
    {
      NetError app = (NetError) (steamReason - 1000);
      if (Enum.IsDefined<NetError>(app))
        return app;
      Log.Error($"Received unknown application error from Steam: {steamReason}");
      return NetError.UnknownNetworkError;
    }
    if (steamReason >= SteamDisconnectionReason.LocalMin)
    {
      if (steamReason > SteamDisconnectionReason.LocalMax)
      {
        if (steamReason != SteamDisconnectionReason.RemoteTimeout)
        {
          if ((uint) (steamReason - 4002) <= 1U)
            return NetError.SecureConnectionFailed;
          switch (steamReason - 5003)
          {
            case SteamDisconnectionReason.None:
              break;
            case (SteamDisconnectionReason) 1:
            case (SteamDisconnectionReason) 2:
              goto label_11;
            case (SteamDisconnectionReason) 5:
            case (SteamDisconnectionReason) 7:
              return NetError.TryAgainLater;
            default:
              goto label_15;
          }
        }
        return NetError.Timeout;
      }
label_11:
      return NetError.NoInternet;
    }
    if (steamReason == SteamDisconnectionReason.None)
      return NetError.None;
label_15:
    return NetError.UnknownNetworkError;
  }
}
