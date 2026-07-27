// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Cards.CardPlay
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Cards;

public class CardPlay
{
  public required CardModel Card { get; init; }

  public required Player Player { get; init; }

  public required Creature? Target { get; init; }

  public required PileType ResultPile { get; init; }

  public required ResourceInfo Resources { get; init; }

  public required bool IsAutoPlay { get; init; }

  public required int PlayIndex { get; init; }

  public required int PlayCount { get; init; }

  public bool IsFirstInSeries => this.PlayIndex == 0;

  public bool IsLastInSeries => this.PlayIndex == this.PlayCount - 1;
}
