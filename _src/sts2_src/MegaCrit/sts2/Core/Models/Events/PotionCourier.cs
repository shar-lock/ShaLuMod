// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.PotionCourier
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.PotionPools;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class PotionCourier : EventModel
{
  private const string _foulPotionsKey = "FoulPotions";

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      new EventOption((EventModel) this, new Func<Task>(this.GrabPotions), "POTION_COURIER.pages.INITIAL.options.GRAB_POTIONS", new IHoverTip[1]
      {
        HoverTipFactory.FromPotion<FoulPotion>()
      }),
      new EventOption((EventModel) this, new Func<Task>(this.Ransack), "POTION_COURIER.pages.INITIAL.options.RANSACK", Array.Empty<IHoverTip>())
    });
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("FoulPotions", 3M));
    }
  }

  public override bool IsAllowed(IRunState runState) => runState.CurrentActIndex > 0;

  private async Task GrabPotions()
  {
    List<Reward> rewards = new List<Reward>();
    for (int index = 0; index < this.DynamicVars["FoulPotions"].IntValue; ++index)
      rewards.Add((Reward) new PotionReward(ModelDb.Potion<FoulPotion>().ToMutable(), this.Owner));
    await RewardsCmd.OfferCustom(this.Owner, rewards);
    this.SetEventFinished(this.L10NLookup("POTION_COURIER.pages.GRAB_POTIONS.description"));
  }

  private async Task Ransack()
  {
    PotionModel potionModel = this.Owner.PlayerRng.Rewards.NextItem<PotionModel>(this.Owner.Character.PotionPool.GetUnlockedPotions(this.Owner.UnlockState).Concat<PotionModel>(ModelDb.PotionPool<SharedPotionPool>().GetUnlockedPotions(this.Owner.UnlockState)).Where<PotionModel>((Func<PotionModel, bool>) (p => p.Rarity == PotionRarity.Uncommon)));
    if (potionModel != null)
      await RewardsCmd.OfferCustom(this.Owner, new List<Reward>(1)
      {
        (Reward) new PotionReward(potionModel.ToMutable(), this.Owner)
      });
    this.SetEventFinished(this.L10NLookup("POTION_COURIER.pages.RANSACK.description"));
  }
}
