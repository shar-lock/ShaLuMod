// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.Amalgamator
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class Amalgamator : EventModel
{
  public override bool IsAllowed(IRunState runState)
  {
    return runState.Players.All<Player>((Func<Player, bool>) (p => p.Deck.Cards.Count<CardModel>((Func<CardModel, bool>) (c => Amalgamator.IsValid(CardTag.Strike, c))) >= 2 && p.Deck.Cards.Count<CardModel>((Func<CardModel, bool>) (c => Amalgamator.IsValid(CardTag.Defend, c))) >= 2));
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new StringVar("Card1", ModelDb.Card<UltimateStrike>().Title),
        (DynamicVar) new StringVar("Card2", ModelDb.Card<UltimateDefend>().Title)
      });
    }
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      new EventOption((EventModel) this, new Func<Task>(this.CombineStrikes), this.InitialOptionKey("COMBINE_STRIKES"), HoverTipFactory.FromCardWithCardHoverTips<UltimateStrike>()),
      new EventOption((EventModel) this, new Func<Task>(this.CombineDefends), this.InitialOptionKey("COMBINE_DEFENDS"), HoverTipFactory.FromCardWithCardHoverTips<UltimateDefend>())
    });
  }

  private async Task CombineStrikes()
  {
    await CardPileCmd.RemoveFromDeck((IReadOnlyList<CardModel>) (await CardSelectCmd.FromDeckForRemoval(this.Owner, new CardSelectorPrefs(CardSelectorPrefs.RemoveSelectionPrompt, 2), (Func<CardModel, bool>) (c => Amalgamator.IsValid(CardTag.Strike, c)))).ToList<CardModel>());
    NDebugAudioManager.Instance?.Play("card_smith.mp3", variance: PitchVariance.Small);
    NGame.Instance.ScreenShakeTrauma(ShakeStrength.Strong);
    await Task.Delay(300);
    NDebugAudioManager.Instance?.Play("card_smith.mp3", variance: PitchVariance.Small);
    NGame.Instance.ScreenShakeTrauma(ShakeStrength.Strong);
    CardCmd.PreviewCardPileAdd(await CardPileCmd.Add((CardModel) this.Owner.RunState.CreateCard<UltimateStrike>(this.Owner), PileType.Deck), 2f);
    this.SetEventFinished(this.L10NLookup("AMALGAMATOR.pages.COMBINE_STRIKES.description"));
  }

  private async Task CombineDefends()
  {
    await CardPileCmd.RemoveFromDeck((IReadOnlyList<CardModel>) (await CardSelectCmd.FromDeckForRemoval(this.Owner, new CardSelectorPrefs(CardSelectorPrefs.RemoveSelectionPrompt, 2), (Func<CardModel, bool>) (c => Amalgamator.IsValid(CardTag.Defend, c)))).ToList<CardModel>());
    NDebugAudioManager.Instance?.Play("card_smith.mp3", variance: PitchVariance.Small);
    NGame.Instance.ScreenShakeTrauma(ShakeStrength.Strong);
    await Task.Delay(300);
    NDebugAudioManager.Instance?.Play("card_smith.mp3", variance: PitchVariance.Small);
    NGame.Instance.ScreenShakeTrauma(ShakeStrength.Strong);
    CardCmd.PreviewCardPileAdd(await CardPileCmd.Add((CardModel) this.Owner.RunState.CreateCard<UltimateDefend>(this.Owner), PileType.Deck), 2f);
    this.SetEventFinished(this.L10NLookup("AMALGAMATOR.pages.COMBINE_DEFENDS.description"));
  }

  private static bool IsValid(CardTag tag, CardModel card)
  {
    return card.Tags.Contains<CardTag>(tag) && card != null && card.Rarity == CardRarity.Basic && card.IsRemovable;
  }
}
