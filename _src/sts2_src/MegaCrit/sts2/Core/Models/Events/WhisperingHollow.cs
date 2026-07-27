// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.WhisperingHollow
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class WhisperingHollow : EventModel
{
  private const int _baseGold = 35;
  private const int _goldVariance = 9;

  public override bool IsAllowed(IRunState runState)
  {
    return runState.Players.All<Player>((Func<Player, bool>) (p => p.Gold >= 44));
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new GoldVar(35),
        (DynamicVar) new HpLossVar(9M)
      });
    }
  }

  public override void CalculateVars()
  {
    GoldVar gold = this.DynamicVars.Gold;
    gold.BaseValue = gold.BaseValue + (Decimal) this.Rng.NextInt(-9, 10);
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      new EventOption((EventModel) this, new Func<Task>(this.Gold), "WHISPERING_HOLLOW.pages.INITIAL.options.GOLD", Array.Empty<IHoverTip>()),
      new EventOption((EventModel) this, new Func<Task>(this.Hug), "WHISPERING_HOLLOW.pages.INITIAL.options.HUG", new IHoverTip[1]
      {
        HoverTipFactory.Static(StaticHoverTip.Transform)
      }).ThatDoesDamage((Decimal) this.DynamicVars.HpLoss.IntValue)
    });
  }

  private async Task Gold()
  {
    await PlayerCmd.LoseGold((Decimal) this.DynamicVars.Gold.IntValue, this.Owner, GoldLossType.Spent);
    await RewardsCmd.OfferCustom(this.Owner, new List<Reward>(2)
    {
      (Reward) new PotionReward(this.Owner),
      (Reward) new PotionReward(this.Owner)
    });
    this.SetEventFinished(this.L10NLookup("WHISPERING_HOLLOW.pages.GOLD.description"));
  }

  private async Task Hug()
  {
    foreach (CardModel original in (await CardSelectCmd.FromDeckForTransformation(this.Owner, new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, 1))).ToList<CardModel>())
    {
      CardPileAddResult random = await CardCmd.TransformToRandom(original, this.Rng, CardPreviewStyle.EventLayout);
    }
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Owner.Creature, this.DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered, (Creature) null, (CardModel) null, (CardPlay) null);
    this.SetEventFinished(this.L10NLookup("WHISPERING_HOLLOW.pages.HUG.description"));
  }
}
