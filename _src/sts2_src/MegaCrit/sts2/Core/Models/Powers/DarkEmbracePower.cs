// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.DarkEmbracePower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class DarkEmbracePower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  protected override object InitInternalData() => (object) new DarkEmbracePower.Data();

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));
    }
  }

  public override async Task AfterCardExhausted(
    PlayerChoiceContext choiceContext,
    CardModel card,
    bool causedByEthereal)
  {
    if (card.Owner.Creature != this.Owner)
      return;
    if (causedByEthereal)
    {
      ++this.GetInternalData<DarkEmbracePower.Data>().etherealCount;
    }
    else
    {
      IEnumerable<CardModel> cardModels = await CardPileCmd.Draw(choiceContext, (Decimal) this.Amount, this.Owner.Player);
    }
  }

  public override async Task AfterSideTurnEnd(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    DarkEmbracePower.Data data;
    if (!participants.Contains<Creature>(this.Owner))
    {
      data = (DarkEmbracePower.Data) null;
    }
    else
    {
      data = this.GetInternalData<DarkEmbracePower.Data>();
      IEnumerable<CardModel> cardModels = await CardPileCmd.Draw(choiceContext, (Decimal) (this.Amount * data.etherealCount), this.Owner.Player);
      data.etherealCount = 0;
      data = (DarkEmbracePower.Data) null;
    }
  }

  private class Data
  {
    public int etherealCount;
  }
}
