// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.WhisperingEarring
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class WhisperingEarring : RelicModel
{
  public const int maxCardsToPlay = 13;

  public override RelicRarity Rarity => RelicRarity.Ancient;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy((RelicModel) this));
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new EnergyVar(1));
    }
  }

  public override Decimal ModifyMaxEnergy(Player player, Decimal amount)
  {
    return player != this.Owner ? amount : amount + this.DynamicVars.Energy.BaseValue;
  }

  public override async Task AfterAutoPrePlayPhaseEnteredLate(
    PlayerChoiceContext choiceContext,
    Player player)
  {
    ICombatState combatState;
    if (player != this.Owner)
    {
      combatState = (ICombatState) null;
    }
    else
    {
      combatState = player.Creature.CombatState;
      if (this.Owner.PlayerCombatState.TurnNumber > 1)
      {
        combatState = (ICombatState) null;
      }
      else
      {
        this.Flash();
        bool flag;
        using (CardSelectCmd.PushSelector((ICardSelector) new VakuuCardSelector()))
        {
          int cardsPlayed = 0;
          int startTurn = this.Owner.PlayerCombatState.TurnNumber;
          while (cardsPlayed < 13 && !CombatManager.Instance.IsOverOrEnding && !CombatManager.Instance.IsPlayerReadyToEndTurn(player) && this.Owner.PlayerCombatState.TurnNumber == startTurn)
          {
            CardModel card = PileType.Hand.GetPile(this.Owner).Cards.FirstOrDefault<CardModel>((Func<CardModel, bool>) (c => c.CanPlay()));
            if (card != null)
            {
              Creature target = this.GetTarget(card, combatState);
              (int, int) valueTuple = await card.SpendResources();
              await CardCmd.AutoPlay(choiceContext, card, target, skipXCapture: true);
              ++cardsPlayed;
              card = (CardModel) null;
              target = (Creature) null;
            }
            else
              break;
          }
          flag = cardsPlayed >= 13;
          if (cardsPlayed == 0)
          {
            combatState = (ICombatState) null;
            return;
          }
        }
        TalkCmd.Play(flag ? new LocString("relics", "WHISPERING_EARRING.warning") : new LocString("relics", "WHISPERING_EARRING.approval"), this.Owner.Creature, VfxColor.Purple);
        combatState = (ICombatState) null;
      }
    }
  }

  private Creature? GetTarget(CardModel card, ICombatState combatState)
  {
    Rng combatTargets = this.Owner.RunState.Rng.CombatTargets;
    Creature target;
    switch (card.TargetType)
    {
      case TargetType.AnyEnemy:
        target = combatState.HittableEnemies.FirstOrDefault<Creature>();
        break;
      case TargetType.AnyPlayer:
        target = this.Owner.Creature;
        break;
      case TargetType.AnyAlly:
        target = combatTargets.NextItem<Creature>(combatState.Allies.Where<Creature>((Func<Creature, bool>) (c => c != null && c.IsAlive && c.IsPlayer && c != this.Owner.Creature)));
        break;
      default:
        target = (Creature) null;
        break;
    }
    return target;
  }
}
