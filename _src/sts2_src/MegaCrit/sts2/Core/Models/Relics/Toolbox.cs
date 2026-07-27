// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.Toolbox
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class Toolbox : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Shop;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new CardsVar(3));
    }
  }

  public override async Task BeforeHandDraw(
    Player player,
    PlayerChoiceContext choiceContext,
    ICombatState combatState)
  {
    if (player != this.Owner || this.Owner.PlayerCombatState.TurnNumber != 1)
      return;
    this.Flash();
    CardModel card = await CardSelectCmd.FromChooseACardScreen(choiceContext, (IReadOnlyList<CardModel>) CardFactory.GetDistinctForCombat(this.Owner, ModelDb.CardPool<ColorlessCardPool>().GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint), this.DynamicVars.Cards.IntValue, this.Owner.RunState.Rng.CombatCardGeneration).ToList<CardModel>(), this.Owner);
    if (card == null)
      return;
    CardPileAddResult combat = await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, player);
  }
}
