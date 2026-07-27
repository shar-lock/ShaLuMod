// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Badges.BadgePool
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Saves;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Badges;

public static class BadgePool
{
  public static IReadOnlyCollection<Badge> CreateAll(SerializableRun run, ulong playerId, bool won)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyCollection<Badge>) new \u003C\u003Ez__ReadOnlyArray<Badge>(new Badge[23]
    {
      (Badge) new CccCombo(run, won, playerId),
      (Badge) new Curses(run, won, playerId),
      (Badge) new DamageLeader(run, won, playerId),
      (Badge) new Debuffer(run, won, playerId),
      (Badge) new DoubleSnecko(run, won, playerId),
      (Badge) new EliteKiller(run, won, playerId),
      (Badge) new Famished(run, won, playerId),
      (Badge) new Glutton(run, won, playerId),
      (Badge) new Healer(run, won, playerId),
      (Badge) new Highlander(run, won, playerId),
      (Badge) new Honed(run, won, playerId),
      (Badge) new BigDeck(run, won, playerId),
      (Badge) new ILikeShiny(run, won, playerId),
      (Badge) new KaChing(run, won, playerId),
      (Badge) new MoneyMoney(run, won, playerId),
      (Badge) new MysteryMachine(run, won, playerId),
      (Badge) new Perfect(run, won, playerId),
      (Badge) new Restful(run, won, playerId),
      (Badge) new Restless(run, won, playerId),
      (Badge) new Speedy(run, won, playerId),
      (Badge) new TabletBadge(run, won, playerId),
      (Badge) new TeamPlayer(run, won, playerId),
      (Badge) new TinyDeck(run, won, playerId)
    });
  }
}
