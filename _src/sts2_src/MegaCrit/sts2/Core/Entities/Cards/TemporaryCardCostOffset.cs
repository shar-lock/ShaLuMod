// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Cards.TemporaryCardCostOffset
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Cards;

public class TemporaryCardCostOffset
{
  public int Offset { get; private set; }

  public bool ClearsWhenTurnEnds { get; private set; }

  public bool ClearsWhenCardIsPlayed { get; private set; }

  public static TemporaryCardCostOffset UntilPlayed(int offset)
  {
    return new TemporaryCardCostOffset()
    {
      Offset = offset,
      ClearsWhenTurnEnds = false,
      ClearsWhenCardIsPlayed = true
    };
  }

  public static TemporaryCardCostOffset ThisTurn(int offset)
  {
    return new TemporaryCardCostOffset()
    {
      Offset = offset,
      ClearsWhenTurnEnds = true,
      ClearsWhenCardIsPlayed = true
    };
  }

  public static TemporaryCardCostOffset ThisCombat(int offset)
  {
    return new TemporaryCardCostOffset()
    {
      Offset = offset,
      ClearsWhenTurnEnds = false,
      ClearsWhenCardIsPlayed = false
    };
  }
}
