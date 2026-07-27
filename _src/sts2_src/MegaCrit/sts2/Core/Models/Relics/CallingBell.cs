// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.CallingBell
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class CallingBell : RelicModel
{
  private const string _relicsKey = "Relics";

  public override RelicRarity Rarity => RelicRarity.Ancient;

  public override bool HasUponPickupEffect => true;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Relics", 3M));
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get => HoverTipFactory.FromCardWithCardHoverTips<CurseOfTheBell>();
  }

  public override async Task AfterObtained()
  {
    CardModel deck = await CardPileCmd.AddCurseToDeck<CurseOfTheBell>(this.Owner);
    await Cmd.Wait(0.75f);
    await RewardsCmd.OfferCustom(this.Owner, this.GenerateRewards());
  }

  private List<Reward> GenerateRewards()
  {
    if (TestMode.IsOn)
    {
      int capacity = 3;
      List<Reward> rewards = new List<Reward>(capacity);
      CollectionsMarshal.SetCount<Reward>(rewards, capacity);
      Span<Reward> span = CollectionsMarshal.AsSpan<Reward>(rewards);
      int num1 = 0;
      span[num1] = (Reward) new RelicReward(ModelDb.Relic<Anchor>().ToMutable(), this.Owner);
      int num2 = num1 + 1;
      span[num2] = (Reward) new RelicReward(ModelDb.Relic<GremlinHorn>().ToMutable(), this.Owner);
      int num3 = num2 + 1;
      span[num3] = (Reward) new RelicReward(ModelDb.Relic<MummifiedHand>().ToMutable(), this.Owner);
      return rewards;
    }
    int capacity1 = 3;
    List<Reward> rewards1 = new List<Reward>(capacity1);
    CollectionsMarshal.SetCount<Reward>(rewards1, capacity1);
    Span<Reward> span1 = CollectionsMarshal.AsSpan<Reward>(rewards1);
    int num4 = 0;
    span1[num4] = (Reward) new RelicReward(RelicRarity.Common, this.Owner);
    int num5 = num4 + 1;
    span1[num5] = (Reward) new RelicReward(RelicRarity.Uncommon, this.Owner);
    int num6 = num5 + 1;
    span1[num6] = (Reward) new RelicReward(RelicRarity.Rare, this.Owner);
    return rewards1;
  }
}
