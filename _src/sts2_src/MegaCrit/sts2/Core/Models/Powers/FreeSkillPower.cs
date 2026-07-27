// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.FreeSkillPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using System;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class FreeSkillPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override bool TryModifyEnergyCostInCombatLate(
    CardModel card,
    Decimal originalCost,
    out Decimal modifiedCost)
  {
    modifiedCost = originalCost;
    if (card.Owner.Creature != this.Owner || card.Type != CardType.Skill)
      return false;
    PileType? type = card.Pile?.Type;
    bool flag;
    if (type.HasValue)
    {
      switch (type.GetValueOrDefault())
      {
        case PileType.Hand:
        case PileType.Play:
          flag = true;
          goto label_6;
      }
    }
    flag = false;
label_6:
    if (!flag)
      return false;
    modifiedCost = 0M;
    return true;
  }

  public override async Task BeforeCardPlayed(CardPlay cardPlay)
  {
    if (cardPlay.Card.Owner.Creature != this.Owner || cardPlay.Card.Type != CardType.Skill)
      return;
    PileType? type = cardPlay.Card.Pile?.Type;
    bool flag;
    if (type.HasValue)
    {
      switch (type.GetValueOrDefault())
      {
        case PileType.Hand:
        case PileType.Play:
          flag = true;
          goto label_6;
      }
    }
    flag = false;
label_6:
    if (!flag)
      return;
    await PowerCmd.Decrement((PowerModel) this);
  }
}
