// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Largesse
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.CardPools;
using System;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Largesse : CardModel
{
  public Largesse()
    : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.AnyAlly)
  {
  }

  public override CardMultiplayerConstraint MultiplayerConstraint
  {
    get => CardMultiplayerConstraint.MultiplayerOnly;
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
    CardModel card = CardFactory.GetDistinctForCombat(cardPlay.Target.Player, ModelDb.CardPool<ColorlessCardPool>().GetUnlockedCards(cardPlay.Target.Player.UnlockState, cardPlay.Target.Player.RunState.CardMultiplayerConstraint), 1, this.Owner.RunState.Rng.CombatCardGeneration).FirstOrDefault<CardModel>();
    if (card == null)
      return;
    if (this.IsUpgraded)
      CardCmd.Upgrade(card);
    CardPileAddResult combat = await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, this.Owner);
  }
}
