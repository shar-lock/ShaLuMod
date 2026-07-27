// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Game.NetLoadingHandle
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Game;

public class NetLoadingHandle : IDisposable
{
  private static readonly Dictionary<INetGameService, int> _loadCounts = new Dictionary<INetGameService, int>();
  private readonly INetGameService _netService;

  public NetLoadingHandle(INetGameService netService)
  {
    this._netService = netService;
    int num;
    if (!NetLoadingHandle._loadCounts.TryGetValue(this._netService, out num) || num == 0)
      this._netService.SetGameLoading(true);
    NetLoadingHandle._loadCounts[this._netService] = num + 1;
  }

  public void Dispose()
  {
    if (NetLoadingHandle._loadCounts[this._netService] == 1)
      this._netService.SetGameLoading(false);
    NetLoadingHandle._loadCounts[this._netService]--;
  }

  public static void Release(INetGameService netService)
  {
    NetLoadingHandle._loadCounts.Remove(netService);
  }
}
