// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Cards.CardLocation
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Cards;

public record struct CardLocation(Player player, PileType pileType, CardPilePosition position)
{
  public Player player = player;
  public PileType pileType = pileType;
  public CardPilePosition position = position;

  [CompilerGenerated]
  public override readonly int GetHashCode()
  {
    return (EqualityComparer<Player>.Default.GetHashCode(this.player) * -1521134295 + EqualityComparer<PileType>.Default.GetHashCode(this.pileType)) * -1521134295 + EqualityComparer<CardPilePosition>.Default.GetHashCode(this.position);
  }

  [CompilerGenerated]
  public readonly bool Equals(CardLocation other)
  {
    return EqualityComparer<Player>.Default.Equals(this.player, other.player) && EqualityComparer<PileType>.Default.Equals(this.pileType, other.pileType) && EqualityComparer<CardPilePosition>.Default.Equals(this.position, other.position);
  }
}
