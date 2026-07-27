// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Game.NetGameTypeExtensions
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

#nullable disable
namespace MegaCrit.Sts2.Core.Multiplayer.Game;

public static class NetGameTypeExtensions
{
  public static bool IsMultiplayer(this NetGameType type)
  {
    bool flag;
    switch (type)
    {
      case NetGameType.Host:
      case NetGameType.Client:
        flag = true;
        break;
      default:
        flag = false;
        break;
    }
    return flag;
  }
}
