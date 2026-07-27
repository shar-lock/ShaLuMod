// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.RoundTeaParty
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class RoundTeaParty : EventModel
{
  public override bool IsAllowed(IRunState runState)
  {
    return runState.Players.All<Player>((Func<Player, bool>) (p => p.Creature.CurrentHp >= 12));
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      new EventOption((EventModel) this, new Func<Task>(this.EnjoyTea), "ROUND_TEA_PARTY.pages.INITIAL.options.ENJOY_TEA", HoverTipFactory.FromRelic<RoyalPoison>()),
      new EventOption((EventModel) this, new Func<Task>(this.PickFight), "ROUND_TEA_PARTY.pages.INITIAL.options.PICK_FIGHT", Array.Empty<IHoverTip>()).ThatDoesDamage(this.DynamicVars.Damage.BaseValue)
    });
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new DamageVar(11M, ValueProp.Unblockable | ValueProp.Unpowered),
        (DynamicVar) new StringVar("Relic", ModelDb.Relic<RoyalPoison>().Title.GetFormattedText())
      });
    }
  }

  private async Task EnjoyTea()
  {
    Creature targetCreature = this.Owner.Creature;
    RoyalPoison royalPoison = await RelicCmd.Obtain<RoyalPoison>(this.Owner);
    await CreatureCmd.Heal(targetCreature, (Decimal) (targetCreature.MaxHp - targetCreature.CurrentHp));
    this.SetEventFinished(this.L10NLookup("ROUND_TEA_PARTY.pages.ENJOY_TEA.description"));
    targetCreature = (Creature) null;
  }

  private Task PickFight()
  {
    // ISSUE: object of a compiler-generated type is created
    this.SetEventState(this.L10NLookup("ROUND_TEA_PARTY.pages.PICK_FIGHT.description"), (IEnumerable<EventOption>) new \u003C\u003Ez__ReadOnlySingleElementList<EventOption>(new EventOption((EventModel) this, new Func<Task>(this.ContinueFight), "ROUND_TEA_PARTY.pages.PICK_FIGHT.options.CONTINUE_FIGHT", Array.Empty<IHoverTip>()).ThatWontSaveToChoiceHistory()));
    return Task.CompletedTask;
  }

  private async Task ContinueFight()
  {
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Owner.Creature, this.DynamicVars.Damage, (Creature) null, (CardModel) null, (CardPlay) null);
    RelicModel relicModel = await RelicCmd.Obtain(RelicFactory.PullNextRelicFromFront(this.Owner).ToMutable(), this.Owner);
    this.SetEventFinished(this.L10NLookup("ROUND_TEA_PARTY.pages.CONTINUE_FIGHT.description"));
  }
}
