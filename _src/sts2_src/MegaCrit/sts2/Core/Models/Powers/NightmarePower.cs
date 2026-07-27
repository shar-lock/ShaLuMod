// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.NightmarePower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class NightmarePower : PowerModel
{
  private const string _cardKey = "Card";

  public override PowerType Type => PowerType.Buff;

  public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

  public override PowerStackType StackType => PowerStackType.Counter;

  protected override object InitInternalData() => (object) new NightmarePower.Data();

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new StringVar("Card"));
    }
  }

  public override async Task BeforeHandDraw(
    Player player,
    PlayerChoiceContext choiceContext,
    ICombatState combatState)
  {
    CardModel card;
    if (player != this.Owner.Player)
    {
      card = (CardModel) null;
    }
    else
    {
      card = this.GetInternalData<NightmarePower.Data>().selectedCard;
      for (int i = 0; i < this.Amount; ++i)
      {
        CardPileAddResult combat = await CardPileCmd.AddGeneratedCardToCombat(card.CreateClone(), PileType.Hand, this.Owner.Player);
      }
      await PowerCmd.Remove((PowerModel) this);
      card = (CardModel) null;
    }
  }

  public void SetSelectedCard(CardModel card)
  {
    CardModel clone = card.CreateClone();
    CardCmd.ClearAffliction(clone);
    this.GetInternalData<NightmarePower.Data>().selectedCard = clone;
    ((StringVar) this.DynamicVars["Card"]).StringValue = clone.Title;
  }

  private class Data
  {
    public CardModel? selectedCard;
  }
}
