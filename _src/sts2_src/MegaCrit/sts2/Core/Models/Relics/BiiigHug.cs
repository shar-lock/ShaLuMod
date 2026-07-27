// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.BiiigHug
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class BiiigHug : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Ancient;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get => HoverTipFactory.FromCardWithCardHoverTips<Soot>();
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new CardsVar(4));
    }
  }

  public override async Task AfterObtained()
  {
    await CardPileCmd.RemoveFromDeck((IReadOnlyList<CardModel>) (await CardSelectCmd.FromDeckForRemoval(this.Owner, new CardSelectorPrefs(CardSelectorPrefs.RemoveSelectionPrompt, this.DynamicVars.Cards.IntValue))).ToList<CardModel>());
  }

  public override async Task AfterShuffle(PlayerChoiceContext choiceContext, Player shuffler)
  {
    CardModel soot;
    if (shuffler != this.Owner)
    {
      soot = (CardModel) null;
    }
    else
    {
      soot = (CardModel) shuffler.Creature.CombatState.CreateCard<Soot>(this.Owner);
      CardPileAddResult combat = await CardPileCmd.AddGeneratedCardToCombat(soot, PileType.Draw, this.Owner, CardPilePosition.Random);
      this.Flash();
      CardCmd.Preview(soot, 0.75f);
      await Cmd.Wait(1f);
      soot = (CardModel) null;
    }
  }
}
