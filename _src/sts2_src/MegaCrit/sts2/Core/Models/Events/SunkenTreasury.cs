// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.SunkenTreasury
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class SunkenTreasury : EventModel
{
  private const string _smallChestGoldKey = "SmallChestGold";
  private const string _largeChestGoldKey = "LargeChestGold";

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      new EventOption((EventModel) this, new Func<Task>(this.FirstChest), "SUNKEN_TREASURY.pages.INITIAL.options.FIRST_CHEST", Array.Empty<IHoverTip>()),
      new EventOption((EventModel) this, new Func<Task>(this.SecondChest), "SUNKEN_TREASURY.pages.INITIAL.options.SECOND_CHEST", HoverTipFactory.FromCardWithCardHoverTips<Greed>())
    });
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        new DynamicVar("SmallChestGold", 60M),
        new DynamicVar("LargeChestGold", 333M)
      });
    }
  }

  public override void CalculateVars()
  {
    this.DynamicVars["SmallChestGold"].BaseValue += (Decimal) (this.Rng.NextInt(16 /*0x10*/) - 8);
    this.DynamicVars["LargeChestGold"].BaseValue += (Decimal) (this.Rng.NextInt(61) - 30);
  }

  private async Task FirstChest()
  {
    await PlayerCmd.GainGold(this.DynamicVars["SmallChestGold"].BaseValue, this.Owner);
    this.SetEventFinished(this.L10NLookup("SUNKEN_TREASURY.pages.FIRST_CHEST.description"));
  }

  private async Task SecondChest()
  {
    await PlayerCmd.GainGold(this.DynamicVars["LargeChestGold"].BaseValue, this.Owner);
    CardModel deck = await CardPileCmd.AddCurseToDeck<Greed>(this.Owner);
    LocString description = this.L10NLookup("SUNKEN_TREASURY.pages.SECOND_CHEST.description");
    description.Add("Monologue", new LocString("characters", this.Owner.Character.Id.Entry + ".goldMonologue"));
    this.SetEventFinished(description);
  }
}
