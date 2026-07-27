// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Synchronize
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Synchronize : CardModel
{
  private const string _calculatedFocusKey = "CalculatedFocus";

  public Synchronize()
    : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
  {
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<FocusPower>());
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[3]
      {
        (DynamicVar) new CalculationBaseVar(0M),
        (DynamicVar) new CalculationExtraVar(2M),
        (DynamicVar) new CalculatedVar("CalculatedFocus").WithMultiplier((Func<CardModel, Creature, Decimal>) ((card, _) => (Decimal) card.Owner.PlayerCombatState.OrbQueue.Orbs.GroupBy<OrbModel, ModelId>((Func<OrbModel, ModelId>) (orb => orb.Id)).Count<IGrouping<ModelId, OrbModel>>()))
      });
    }
  }

  public override IEnumerable<CardKeyword> CanonicalKeywords
  {
    get
    {
      return (IEnumerable<CardKeyword>) new \u003C\u003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
    SynchronizePower synchronizePower = await PowerCmd.Apply<SynchronizePower>(choiceContext, this.Owner.Creature, ((CalculatedVar) this.DynamicVars["CalculatedFocus"]).Calculate(cardPlay.Target), this.Owner.Creature, (CardModel) this);
  }

  protected override void OnUpgrade() => this.RemoveKeyword(CardKeyword.Exhaust);
}
