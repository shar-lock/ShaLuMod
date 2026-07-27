// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.ToastyMittens
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
using MegaCrit.Sts2.Core.Models.Powers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class ToastyMittens : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Ancient;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new PowerVar<StrengthPower>(1M));
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlyArray<IHoverTip>(new IHoverTip[2]
      {
        HoverTipFactory.FromKeyword(CardKeyword.Exhaust),
        HoverTipFactory.FromPower<StrengthPower>()
      });
    }
  }

  public override async Task BeforeHandDraw(
    Player player,
    PlayerChoiceContext choiceContext,
    ICombatState combatState)
  {
    if (player != this.Owner.Creature.Player)
      return;
    this.Flash();
    await CardPileCmd.ShuffleIfNecessary(choiceContext, this.Owner);
    IReadOnlyList<CardModel> cards = PileType.Draw.GetPile(player).Cards;
    CardModel card = (CardModel) null;
    if (this.Owner.PlayerCombatState.TurnNumber == 1)
      card = cards.FirstOrDefault<CardModel>((Func<CardModel, bool>) (c => !c.Keywords.Contains(CardKeyword.Innate)));
    if (card == null)
      card = cards.FirstOrDefault<CardModel>();
    if (card != null)
      await CardCmd.Exhaust(choiceContext, card);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>(choiceContext, player.Creature, this.DynamicVars.Strength.BaseValue, player.Creature, (CardModel) null);
  }
}
