// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.ReboundPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class ReboundPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override CardLocation ModifyCardPlayResultLocation(
    CardModel card,
    bool isAutoPlay,
    ResourceInfo resources,
    CardLocation location)
  {
    if (card.Owner.Creature != this.Owner || location.pileType != PileType.Discard)
      return location;
    location.pileType = PileType.Draw;
    location.position = CardPilePosition.Top;
    return location;
  }

  public override async Task AfterModifyingCardPlayResultLocation(
    CardModel card,
    CardLocation location)
  {
    if (card.Owner.Creature != this.Owner)
      return;
    this.Flash();
    await PowerCmd.Decrement((PowerModel) this);
  }

  public override async Task AfterSideTurnEnd(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    if (!participants.Contains<Creature>(this.Owner))
      return;
    await PowerCmd.Remove((PowerModel) this);
  }
}
