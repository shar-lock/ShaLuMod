// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.DrowningBeacon
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rewards;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class DrowningBeacon : EventModel
{
  private const string _potionKey = "Potion";
  private const string _relicKey = "Relic";

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[3]
      {
        (DynamicVar) new HpLossVar(13M),
        (DynamicVar) new StringVar("Potion", ModelDb.Potion<GlowwaterPotion>().Title.GetFormattedText()),
        (DynamicVar) new StringVar("Relic", ModelDb.Relic<FresnelLens>().Title.GetFormattedText())
      });
    }
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      new EventOption((EventModel) this, new Func<Task>(this.BottleOption), "DROWNING_BEACON.pages.INITIAL.options.BOTTLE", new IHoverTip[1]
      {
        HoverTipFactory.FromPotion((PotionModel) ModelDb.Potion<GlowwaterPotion>())
      }),
      new EventOption((EventModel) this, new Func<Task>(this.ClimbOption), "DROWNING_BEACON.pages.INITIAL.options.CLIMB", HoverTipFactory.FromRelic<FresnelLens>()).ThatDecreasesMaxHp(this.DynamicVars.HpLoss.BaseValue)
    });
  }

  private async Task BottleOption()
  {
    await RewardsCmd.OfferCustom(this.Owner, new List<Reward>(1)
    {
      (Reward) new PotionReward(ModelDb.Potion<GlowwaterPotion>().ToMutable(), this.Owner)
    });
    this.SetEventFinished(this.L10NLookup("DROWNING_BEACON.pages.BOTTLE.description"));
  }

  private async Task ClimbOption()
  {
    await CreatureCmd.LoseMaxHp((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Owner.Creature, this.DynamicVars.HpLoss.BaseValue, false);
    RelicModel relicModel = await RelicCmd.Obtain(ModelDb.Relic<FresnelLens>().ToMutable(), this.Owner);
    this.SetEventFinished(this.L10NLookup("DROWNING_BEACON.pages.CLIMB.description"));
  }
}
