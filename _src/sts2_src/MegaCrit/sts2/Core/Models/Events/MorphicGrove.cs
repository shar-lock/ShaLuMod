// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.MorphicGrove
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class MorphicGrove : EventModel
{
  private const int _transformCount = 2;

  public override bool IsShared => true;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new MaxHpVar(5M));
    }
  }

  public override bool IsAllowed(IRunState runState)
  {
    return runState.Players.All<Player>((Func<Player, bool>) (p => p.Gold >= 100 && p.Deck.Cards.Count<CardModel>((Func<CardModel, bool>) (c => c.IsTransformable)) >= 2));
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      new EventOption((EventModel) this, new Func<Task>(this.Group), "MORPHIC_GROVE.pages.INITIAL.options.GROUP", new IHoverTip[1]
      {
        HoverTipFactory.Static(StaticHoverTip.Transform)
      }),
      new EventOption((EventModel) this, new Func<Task>(this.Loner), "MORPHIC_GROVE.pages.INITIAL.options.LONER", Array.Empty<IHoverTip>())
    });
  }

  private async Task Loner()
  {
    await CreatureCmd.GainMaxHp(this.Owner.Creature, this.DynamicVars.MaxHp.BaseValue);
    this.SetEventFinished(this.L10NLookup("MORPHIC_GROVE.pages.LONER.description"));
  }

  private async Task Group()
  {
    await PlayerCmd.LoseGold((Decimal) this.Owner.Gold, this.Owner, GoldLossType.Stolen);
    foreach (CardModel original in (await CardSelectCmd.FromDeckForTransformation(this.Owner, new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, 2))).ToList<CardModel>())
    {
      CardPileAddResult random = await CardCmd.TransformToRandom(original, this.Rng, CardPreviewStyle.EventLayout);
    }
    this.SetEventFinished(this.L10NLookup("MORPHIC_GROVE.pages.GROUP.description"));
  }
}
