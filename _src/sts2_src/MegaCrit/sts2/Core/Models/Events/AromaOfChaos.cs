// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.AromaOfChaos
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class AromaOfChaos : EventModel
{
  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      new EventOption((EventModel) this, new Func<Task>(this.LetGo), "AROMA_OF_CHAOS.pages.INITIAL.options.LET_GO", new IHoverTip[1]
      {
        HoverTipFactory.Static(StaticHoverTip.Transform)
      }),
      new EventOption((EventModel) this, new Func<Task>(this.MaintainControl), "AROMA_OF_CHAOS.pages.INITIAL.options.MAINTAIN_CONTROL", Array.Empty<IHoverTip>())
    });
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get => (IEnumerable<DynamicVar>) Array.Empty<DynamicVar>();
  }

  private async Task LetGo()
  {
    CardModel original = (await CardSelectCmd.FromDeckForTransformation(this.Owner, new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, 1))).FirstOrDefault<CardModel>();
    if (original != null)
    {
      CardPileAddResult random = await CardCmd.TransformToRandom(original, this.Rng, CardPreviewStyle.EventLayout);
    }
    this.SetEventFinished(this.L10NLookup("AROMA_OF_CHAOS.pages.LET_GO.description"));
  }

  private async Task MaintainControl()
  {
    CardModel card = (await CardSelectCmd.FromDeckForUpgrade(this.Owner, new CardSelectorPrefs(CardSelectorPrefs.UpgradeSelectionPrompt, 1))).FirstOrDefault<CardModel>();
    if (card != null)
      CardCmd.Upgrade(card);
    LocString description = this.L10NLookup("AROMA_OF_CHAOS.pages.MAINTAIN_CONTROL.description");
    description.Add("AromaPrinciple", new LocString("characters", this.Owner.Character.Id.Entry + ".aromaPrinciple"));
    this.SetEventFinished(description);
  }
}
