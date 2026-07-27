// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.DustyTome
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class DustyTome : RelicModel
{
  private const string _ancientCardKey = "AncientCard";
  private ModelId? _ancientCard;
  private IEnumerable<IHoverTip> _extraHoverTips = (IEnumerable<IHoverTip>) Array.Empty<IHoverTip>();

  public override RelicRarity Rarity => RelicRarity.Ancient;

  [SavedProperty]
  public ModelId? AncientCard
  {
    get => this._ancientCard;
    set
    {
      this.AssertMutable();
      this._ancientCard = value;
      if (!(this._ancientCard != (ModelId) null))
        return;
      CardModel card = SaveUtil.CardOrDeprecated(this._ancientCard);
      this._extraHoverTips = card.HoverTips.Concat<IHoverTip>((IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard(card, true)));
      ((StringVar) this.DynamicVars[nameof (AncientCard)]).StringValue = card.Title;
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new StringVar("AncientCard"));
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips => this._extraHoverTips;

  public void SetupForPlayer(Player player)
  {
    IEnumerable<CardModel> items = player.Character.CardPool.GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint).Where<CardModel>((Func<CardModel, bool>) (c => c.Rarity == CardRarity.Ancient && !ArchaicTooth.TranscendenceCards.Contains(c)));
    this.AncientCard = player.PlayerRng.Rewards.NextItem<CardModel>(items).Id;
  }

  public override async Task AfterObtained()
  {
    CardModel card = this.Owner.RunState.CreateCard(ModelDb.GetById<CardModel>(this.AncientCard), this.Owner);
    CardCmd.Upgrade(card);
    CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(card, PileType.Deck), 2f);
  }
}
