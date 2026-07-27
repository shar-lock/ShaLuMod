// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Combat.History.Entries.CardPlayFinishedEntry
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using System.Collections.Generic;
using System.Text;

#nullable enable
namespace MegaCrit.Sts2.Core.Combat.History.Entries;

public class CardPlayFinishedEntry : CombatHistoryEntry
{
  public CardPlay CardPlay { get; }

  public bool WasEthereal { get; }

  public override string Description
  {
    get
    {
      StringBuilder stringBuilder1 = new StringBuilder($"{this.Actor.Player.Character.Id.Entry} played {this.CardPlay.Card.Id.Entry}");
      if (this.CardPlay.Target != null)
      {
        StringBuilder stringBuilder2 = stringBuilder1;
        StringBuilder stringBuilder3 = stringBuilder2;
        StringBuilder.AppendInterpolatedStringHandler interpolatedStringHandler;
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(11, 1, stringBuilder2);
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(" targeting ");
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(this.CardPlay.Target.Monster.Id.Entry);
        ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
        stringBuilder3.Append(ref local);
      }
      return stringBuilder1.ToString();
    }
  }

  public CardPlayFinishedEntry(
    CardPlay cardPlay,
    int roundNumber,
    CombatSide currentSide,
    CombatHistory history,
    IEnumerable<Player> players)
    : base(cardPlay.Card.Owner.Creature, roundNumber, currentSide, history, players)
  {
    this.CardPlay = cardPlay;
    this.WasEthereal = cardPlay.Card.Keywords.Contains(CardKeyword.Ethereal);
  }
}
