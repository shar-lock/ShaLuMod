// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.GalvanicPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Afflictions;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class GalvanicPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new StringVar("AfflictionTitle", ModelDb.Affliction<Galvanized>().Title.GetFormattedText()));
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get => HoverTipFactory.FromAffliction<Galvanized>(this.Amount);
  }

  public override async Task BeforeCombatStart()
  {
    foreach (Creature creature in this.Owner.CombatState.Allies.ToList<Creature>())
    {
      if (creature.IsPlayer)
      {
        foreach (CardModel card in creature.Player.PlayerCombatState.AllCards.Where<CardModel>((Func<CardModel, bool>) (c => c.Type == CardType.Power)))
        {
          Galvanized galvanized = await CardCmd.Afflict<Galvanized>(card, (Decimal) this.Amount);
        }
      }
    }
  }

  public override async Task AfterCardEnteredCombat(CardModel card)
  {
    if (card.Affliction != null || card.Type != CardType.Power)
      return;
    Galvanized galvanized = await CardCmd.Afflict<Galvanized>(card, (Decimal) this.Amount);
  }

  public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (!(cardPlay.Card.Affliction is Galvanized))
      return;
    VfxCmd.PlayOnCreature(cardPlay.Card.Owner.Creature, "vfx/vfx_attack_lightning");
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, cardPlay.Card.Owner.Creature, (Decimal) this.Amount, ValueProp.Unpowered | ValueProp.Move, (CardModel) null, (CardPlay) null);
  }
}
