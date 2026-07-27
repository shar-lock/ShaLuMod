// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.PersonalHivePower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class PersonalHivePower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Dazed>());
    }
  }

  public override async Task AfterDamageReceived(
    PlayerChoiceContext choiceContext,
    Creature target,
    DamageResult _,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource)
  {
    CardPileAddResult[] statusCards;
    if (target != this.Owner)
      statusCards = (CardPileAddResult[]) null;
    else if (dealer == null)
      statusCards = (CardPileAddResult[]) null;
    else if (!props.IsPoweredAttack())
    {
      statusCards = (CardPileAddResult[]) null;
    }
    else
    {
      if (dealer.Monster is Osty)
        dealer = dealer.PetOwner.Creature;
      statusCards = new CardPileAddResult[this.Amount];
      for (int i = 0; i < this.Amount; ++i)
      {
        CardModel card = (CardModel) this.CombatState.CreateCard<Dazed>(dealer.Player);
        CardPileAddResult[] cardPileAddResultArray = statusCards;
        int index = i;
        cardPileAddResultArray[index] = await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Draw, (Player) null, CardPilePosition.Random);
        cardPileAddResultArray = (CardPileAddResult[]) null;
      }
      CardCmd.PreviewCardPileAdd((IReadOnlyList<CardPileAddResult>) statusCards);
      await Cmd.Wait(0.5f);
      statusCards = (CardPileAddResult[]) null;
    }
  }
}
