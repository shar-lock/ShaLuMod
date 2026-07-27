// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.CreativeAiPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class CreativeAiPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override async Task BeforeHandDraw(
    Player player,
    PlayerChoiceContext choiceContext,
    ICombatState combatState)
  {
    if (player != this.Owner.Player)
      return;
    for (int i = 0; i < this.Amount; ++i)
    {
      CardModel card = CardFactory.GetDistinctForCombat(player, player.Character.CardPool.GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint).Where<CardModel>((Func<CardModel, bool>) (c => c.Type == CardType.Power)), 1, player.RunState.Rng.CombatCardGeneration).FirstOrDefault<CardModel>();
      if (card != null)
      {
        CardPileAddResult combat = await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, this.Owner.Player);
      }
    }
  }
}
