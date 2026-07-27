// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.BurningSticks
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Rooms;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class BurningSticks : RelicModel
{
  private bool _wasUsedThisCombat;

  public override RelicRarity Rarity => RelicRarity.Shop;

  private bool WasUsedThisCombat
  {
    get => this._wasUsedThisCombat;
    set
    {
      this.AssertMutable();
      this._wasUsedThisCombat = value;
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));
    }
  }

  public override Task AfterRoomEntered(AbstractRoom room)
  {
    if (!(room is CombatRoom))
      return Task.CompletedTask;
    this.WasUsedThisCombat = false;
    this.Status = RelicStatus.Active;
    return Task.CompletedTask;
  }

  public override async Task AfterCardExhausted(
    PlayerChoiceContext choiceContext,
    CardModel card,
    bool causedByEthereal)
  {
    if (card.Owner != this.Owner || this.WasUsedThisCombat || card.Type != CardType.Skill)
      return;
    this.Flash();
    CardPileAddResult combat = await CardPileCmd.AddGeneratedCardToCombat(card.CreateClone(), PileType.Hand, this.Owner);
    this.Status = RelicStatus.Normal;
    this.WasUsedThisCombat = true;
  }

  public override Task AfterCombatEnd(CombatRoom _)
  {
    this.WasUsedThisCombat = false;
    this.Status = RelicStatus.Normal;
    return Task.CompletedTask;
  }
}
