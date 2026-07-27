// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.TheLegendsWereTrue
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.PotionPools;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class TheLegendsWereTrue : EventModel
{
  public override bool IsAllowed(IRunState runState)
  {
    return runState.CurrentActIndex == 0 && runState.Players.All<Player>((Func<Player, bool>) (p => p.Deck.Cards.Count > 0)) && runState.Players.All<Player>((Func<Player, bool>) (p => p.Creature.CurrentHp >= 10));
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new DamageVar(8M, ValueProp.Unblockable | ValueProp.Unpowered));
    }
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      new EventOption((EventModel) this, new Func<Task>(this.NabTheMap), "THE_LEGENDS_WERE_TRUE.pages.INITIAL.options.NAB_THE_MAP", HoverTipFactory.FromCardWithCardHoverTips<SpoilsMap>()),
      new EventOption((EventModel) this, new Func<Task>(this.SlowlyFindAnExit), "THE_LEGENDS_WERE_TRUE.pages.INITIAL.options.SLOWLY_FIND_AN_EXIT", Array.Empty<IHoverTip>()).ThatDoesDamage(this.DynamicVars.Damage.BaseValue)
    });
  }

  private async Task NabTheMap()
  {
    CardCmd.PreviewCardPileAdd(await CardPileCmd.Add((CardModel) this.Owner.RunState.CreateCard<SpoilsMap>(this.Owner), PileType.Deck));
    await Cmd.CustomScaledWait(0.5f, 1.2f);
    this.SetEventFinished(this.L10NLookup("THE_LEGENDS_WERE_TRUE.pages.NAB_THE_MAP.description"));
  }

  private async Task SlowlyFindAnExit()
  {
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Owner.Creature, this.DynamicVars.Damage, (Creature) null, (CardModel) null, (CardPlay) null);
    PotionModel potionModel = this.Owner.PlayerRng.Rewards.NextItem<PotionModel>(this.Owner.Character.PotionPool.GetUnlockedPotions(this.Owner.UnlockState).Concat<PotionModel>(ModelDb.PotionPool<SharedPotionPool>().GetUnlockedPotions(this.Owner.UnlockState)));
    if (potionModel != null)
      await RewardsCmd.OfferCustom(this.Owner, new List<Reward>(1)
      {
        (Reward) new PotionReward(potionModel.ToMutable(), this.Owner)
      });
    this.SetEventFinished(this.L10NLookup("THE_LEGENDS_WERE_TRUE.pages.SLOWLY_FIND_AN_EXIT.description"));
  }
}
