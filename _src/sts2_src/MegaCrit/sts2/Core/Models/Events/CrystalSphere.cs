// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.CrystalSphere
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class CrystalSphere : EventModel
{
  private const string _uncoverFutureCostKey = "UncoverFutureCost";
  private const string _uncoverFutureProphesizeKey = "UncoverFutureProphesizeCount";
  private const string _paymentPlanKey = "PaymentPlanCount";
  private const int _uncoverFutureCost = 50;
  private const int _uncoverFutureRandomMin = 1;
  private const int _uncoverFutureRandomMax = 50;
  private const int _uncoverFutureProphesizeCount = 3;
  private const int _paymentPlanCount = 6;

  public override bool IsDeterministic => false;

  public override void CalculateVars()
  {
    this.DynamicVars["UncoverFutureCost"].BaseValue += (Decimal) this.Rng.NextInt(1, 50);
  }

  public override bool IsAllowed(IRunState runState)
  {
    return runState.Players.All<Player>((Func<Player, bool>) (p => p.Gold >= 100)) && runState.CurrentActIndex > 0;
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[4]
      {
        new DynamicVar("UncoverFutureCost", 50M),
        new DynamicVar("UncoverFutureProphesizeCount", 3M),
        new DynamicVar("PaymentPlanCount", 6M),
        (DynamicVar) new StringVar("CurseTitle", ModelDb.Card<Debt>().Title)
      });
    }
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      new EventOption((EventModel) this, new Func<Task>(this.UncoverFuture), "CRYSTAL_SPHERE.pages.INITIAL.options.UNCOVER_FUTURE", Array.Empty<IHoverTip>()),
      new EventOption((EventModel) this, new Func<Task>(this.PaymentPlan), "CRYSTAL_SPHERE.pages.INITIAL.options.PAYMENT_PLAN", HoverTipFactory.FromCardWithCardHoverTips<Debt>())
    });
  }

  private async Task UncoverFuture()
  {
    await PlayerCmd.LoseGold(this.DynamicVars["UncoverFutureCost"].BaseValue, this.Owner, GoldLossType.Spent);
    await new CrystalSphereMinigame(this.Owner, this.Rng, 3).PlayMinigame();
    this.SetEventFinished(this.L10NLookup("CRYSTAL_SPHERE.pages.FINISH.description"));
  }

  private async Task PaymentPlan()
  {
    CardModel deck = await CardPileCmd.AddCurseToDeck<Debt>(this.Owner);
    await new CrystalSphereMinigame(this.Owner, this.Rng, 6).PlayMinigame();
    this.SetEventFinished(this.L10NLookup("CRYSTAL_SPHERE.pages.FINISH.description"));
  }
}
