// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.SpiritGrafter
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class SpiritGrafter : EventModel
{
  private const string _rejectionHpLossKey = "RejectionHpLoss";
  private const string _letItInHealAmountKey = "LetItInHealAmount";

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      new EventOption((EventModel) this, new Func<Task>(this.LetItIn), "SPIRIT_GRAFTER.pages.INITIAL.options.LET_IT_IN", HoverTipFactory.FromCardWithCardHoverTips<Metamorphosis>()),
      new EventOption((EventModel) this, new Func<Task>(this.Rejection), "SPIRIT_GRAFTER.pages.INITIAL.options.REJECTION", Array.Empty<IHoverTip>()).ThatDoesDamage(this.DynamicVars["RejectionHpLoss"].BaseValue)
    });
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new HpLossVar("RejectionHpLoss", 10M),
        (DynamicVar) new HealVar("LetItInHealAmount", 25M)
      });
    }
  }

  private async Task LetItIn()
  {
    await CreatureCmd.Heal(this.Owner.Creature, this.DynamicVars["LetItInHealAmount"].BaseValue);
    CardCmd.PreviewCardPileAdd(await CardPileCmd.Add((CardModel) this.Owner.RunState.CreateCard<Metamorphosis>(this.Owner), PileType.Deck));
    this.SetEventFinished(this.L10NLookup("SPIRIT_GRAFTER.pages.LET_IT_IN.description"));
  }

  private async Task Rejection()
  {
    CardModel card = (await CardSelectCmd.FromDeckForUpgrade(this.Owner, new CardSelectorPrefs(CardSelectorPrefs.UpgradeSelectionPrompt, 1))).FirstOrDefault<CardModel>();
    if (card != null)
      CardCmd.Upgrade(card);
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Owner.Creature, this.DynamicVars["RejectionHpLoss"].BaseValue, ValueProp.Unblockable | ValueProp.Unpowered, (CardModel) null, (CardPlay) null);
    this.SetEventFinished(this.L10NLookup("SPIRIT_GRAFTER.pages.REJECTION.description"));
  }
}
