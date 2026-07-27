// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Cards.TemporaryCardCost
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Cards;

public class TemporaryCardCost
{
  public int Cost { get; private set; }

  public bool ClearsWhenTurnEnds { get; private set; }

  public bool ClearsWhenCardIsPlayed { get; private set; }

  public static TemporaryCardCost UntilPlayed(int cost)
  {
    return new TemporaryCardCost()
    {
      Cost = Math.Max(cost, 0),
      ClearsWhenTurnEnds = false,
      ClearsWhenCardIsPlayed = true
    };
  }

  public static TemporaryCardCost ThisTurn(int cost)
  {
    return new TemporaryCardCost()
    {
      Cost = Math.Max(cost, 0),
      ClearsWhenTurnEnds = true,
      ClearsWhenCardIsPlayed = true
    };
  }

  public static TemporaryCardCost ThisCombat(int cost)
  {
    return new TemporaryCardCost()
    {
      Cost = Math.Max(cost, 0),
      ClearsWhenTurnEnds = false,
      ClearsWhenCardIsPlayed = false
    };
  }
}
