// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Runs.IPlayerCollection
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Runs;

public interface IPlayerCollection
{
  IReadOnlyList<Player> Players { get; }

  int GetPlayerSlotIndex(Player player);

  Player? GetPlayer(ulong netId);
}
