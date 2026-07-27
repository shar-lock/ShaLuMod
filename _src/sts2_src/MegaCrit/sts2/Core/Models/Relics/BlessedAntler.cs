// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.BlessedAntler
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class BlessedAntler : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Ancient;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new EnergyVar(1),
        (DynamicVar) new CardsVar(3)
      });
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      List<IHoverTip> items = new List<IHoverTip>();
      items.Add(HoverTipFactory.ForEnergy((RelicModel) this));
      items.AddRange(HoverTipFactory.FromCardWithCardHoverTips<Dazed>());
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlyList<IHoverTip>(items);
    }
  }

  public override Decimal ModifyMaxEnergy(Player player, Decimal amount)
  {
    return player != this.Owner ? amount : amount + (Decimal) this.DynamicVars.Energy.IntValue;
  }

  public override async Task BeforeHandDraw(
    Player player,
    PlayerChoiceContext choiceContext,
    ICombatState combatState)
  {
    if (player != this.Owner || this.Owner.PlayerCombatState.TurnNumber != 1)
      return;
    this.Flash();
    List<CardModel> cards = new List<CardModel>();
    for (int index = 0; index < this.DynamicVars.Cards.IntValue; ++index)
      cards.Add((CardModel) combatState.CreateCard<Dazed>(this.Owner));
    CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardsToCombat((IEnumerable<CardModel>) cards, PileType.Draw, this.Owner, CardPilePosition.Random));
    await Cmd.Wait(3f);
  }
}
