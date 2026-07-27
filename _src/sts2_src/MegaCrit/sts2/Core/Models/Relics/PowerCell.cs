// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.PowerCell
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class PowerCell : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Rare;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new CardsVar(2));
    }
  }

  public override async Task BeforeSideTurnStart(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IReadOnlyList<Creature> participants,
    ICombatState combatState)
  {
    if (!participants.Contains<Creature>(this.Owner.Creature) || this.Owner.PlayerCombatState.TurnNumber > 1)
      return;
    this.Flash();
    IReadOnlyList<CardPileAddResult> cardPileAddResultList = await CardPileCmd.Add(PileType.Draw.GetPile(this.Owner).Cards.Where<CardModel>((Func<CardModel, bool>) (c => !c.EnergyCost.CostsX && c.EnergyCost.GetWithModifiers(CostModifiers.Local) == 0)).ToList<CardModel>().StableShuffle<CardModel>(this.Owner.RunState.Rng.CombatCardSelection).Take<CardModel>(this.DynamicVars.Cards.IntValue), PileType.Hand);
  }
}
