// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.PaelsTooth
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class PaelsTooth : RelicModel
{
  public const int cardsCount = 5;
  private const string _cardTitlesKey = "CardTitles";
  private List<SerializableCard> _serializableCards = new List<SerializableCard>();

  public override RelicRarity Rarity => RelicRarity.Ancient;

  public override bool ShowCounter => this.IsMutable && this._serializableCards.Count > 0;

  public override int DisplayAmount => !this.IsMutable ? 0 : this._serializableCards.Count;

  public override bool HasUponPickupEffect => true;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new CardsVar(5),
        (DynamicVar) new StringVar("CardTitles")
      });
    }
  }

  [SavedProperty]
  public List<SerializableCard> SerializableCards
  {
    get => this._serializableCards;
    private set
    {
      this.AssertMutable();
      this._serializableCards.Clear();
      this._serializableCards.AddRange((IEnumerable<SerializableCard>) value);
      this.UpdateCardList();
    }
  }

  protected override void AfterCloned()
  {
    base.AfterCloned();
    this._serializableCards = new List<SerializableCard>();
  }

  public override async Task AfterObtained()
  {
    foreach (CardModel card in (IEnumerable<CardModel>) (await CardSelectCmd.FromDeckForRemoval(this.Owner, new CardSelectorPrefs(CardSelectorPrefs.RemoveSelectionPrompt, this.DynamicVars.Cards.IntValue), (Func<CardModel, bool>) (c => c.IsUpgradable))).OrderBy<CardModel, string>((Func<CardModel, string>) (c => c.Id.Entry), (IComparer<string>) StringComparer.Ordinal))
    {
      this.SerializableCards.Add(((CardModel) card.MutableClone()).ToSerializable());
      await CardPileCmd.RemoveFromDeck(card);
    }
    this.UpdateCardList();
  }

  public override async Task AfterCombatEnd(CombatRoom room)
  {
    SerializableCard serializableCard;
    if (this.Owner.Creature.IsDead)
      serializableCard = (SerializableCard) null;
    else if (this.SerializableCards.Count == 0)
    {
      serializableCard = (SerializableCard) null;
    }
    else
    {
      this.Flash();
      await Cmd.CustomScaledWait(0.1f, 1f);
      serializableCard = this.Owner.PlayerRng.Rewards.NextItem<SerializableCard>((IEnumerable<SerializableCard>) this.SerializableCards);
      CardModel cardModel = CardModel.FromSerializable(serializableCard);
      if (!this.Owner.RunState.ContainsCard(cardModel))
        this.Owner.RunState.AddCard(cardModel, this.Owner);
      if (cardModel.IsUpgradable)
        CardCmd.Upgrade(cardModel, CardPreviewStyle.MessyLayout);
      CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(cardModel, PileType.Deck));
      this.Status = this.SerializableCards.Count > 0 ? RelicStatus.Normal : RelicStatus.Disabled;
      this.SerializableCards.Remove(serializableCard);
      this.UpdateCardList();
      serializableCard = (SerializableCard) null;
    }
  }

  private void UpdateCardList()
  {
    this.Status = this.SerializableCards.Count > 0 ? RelicStatus.Normal : RelicStatus.Disabled;
    ((StringVar) this.DynamicVars["CardTitles"]).StringValue = this.SerializableCards.Count != 0 ? string.Join<string>('\n', this.SerializableCards.Select<SerializableCard, string>((Func<SerializableCard, string>) (c => "- " + SaveUtil.CardOrDeprecated(c.Id).Title))) : string.Empty;
    this.InvokeDisplayAmountChanged();
  }

  public void DebugAddCard(SerializableCard card) => this.SerializableCards.Add(card);
}
