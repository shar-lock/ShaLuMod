// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.NostalgiaPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using System;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class NostalgiaPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override CardLocation ModifyCardPlayResultLocation(
    CardModel card,
    bool isAutoPlay,
    ResourceInfo resources,
    CardLocation location)
  {
    if (card.Owner.Creature != this.Owner)
      return location;
    bool flag1;
    switch (card.Type)
    {
      case CardType.Attack:
      case CardType.Skill:
        flag1 = true;
        break;
      default:
        flag1 = false;
        break;
    }
    if (!flag1 || location.pileType != PileType.Discard || CombatManager.Instance.History.CardPlaysStarted.Count<CardPlayStartedEntry>((Func<CardPlayStartedEntry, bool>) (e =>
    {
      bool flag2 = e.HappenedThisTurn(this.CombatState);
      if (flag2)
      {
        bool flag3;
        switch (e.CardPlay.Card.Type)
        {
          case CardType.Attack:
          case CardType.Skill:
            flag3 = true;
            break;
          default:
            flag3 = false;
            break;
        }
        flag2 = flag3;
      }
      return flag2 && e.CardPlay.Player == this.Owner.Player;
    })) >= this.Amount)
      return location;
    location.pileType = PileType.Draw;
    location.position = CardPilePosition.Top;
    return location;
  }

  public override Task AfterModifyingCardPlayResultLocation(CardModel card, CardLocation location)
  {
    if (card.Owner.Creature != this.Owner)
      return Task.CompletedTask;
    this.Flash();
    return Task.CompletedTask;
  }
}
