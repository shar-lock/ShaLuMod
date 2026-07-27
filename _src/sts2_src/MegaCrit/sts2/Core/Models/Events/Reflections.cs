// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.Reflections
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class Reflections : EventModel
{
  public override void OnRoomEnter()
  {
    NEventRoom instance = NEventRoom.Instance;
    if (instance == null)
      return;
    Control vfxContainer = instance.VfxContainer;
    if (vfxContainer == null)
      return;
    ((Godot.Node) vfxContainer).AddChildSafely((Godot.Node) NMirrorVfx.Create());
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      new EventOption((EventModel) this, new Func<Task>(this.TouchAMirror), "REFLECTIONS.pages.INITIAL.options.TOUCH_A_MIRROR", Array.Empty<IHoverTip>()),
      new EventOption((EventModel) this, new Func<Task>(this.Shatter), "REFLECTIONS.pages.INITIAL.options.SHATTER", HoverTipFactory.FromCardWithCardHoverTips<BadLuck>())
    });
  }

  private async Task TouchAMirror()
  {
    List<CardModel> upgradedCards = this.Owner.Deck.Cards.Where<CardModel>((Func<CardModel, bool>) (c => c.IsUpgraded)).ToList<CardModel>();
    int i;
    for (i = 0; i < 2 && upgradedCards.Count > 0; ++i)
    {
      CardModel card = this.Rng.NextItem<CardModel>((IEnumerable<CardModel>) upgradedCards);
      upgradedCards.Remove(card);
      CardCmd.Downgrade(card);
      CardCmd.Preview(card, style: CardPreviewStyle.MessyLayout);
      await Cmd.CustomScaledWait(0.3f, 0.5f);
    }
    List<CardModel> upgradableCards = this.Owner.Deck.Cards.Where<CardModel>((Func<CardModel, bool>) (c => c.IsUpgradable)).ToList<CardModel>();
    for (i = 0; i < 4 && upgradableCards.Count > 0; ++i)
    {
      CardModel card = this.Rng.NextItem<CardModel>((IEnumerable<CardModel>) upgradableCards);
      upgradableCards.Remove(card);
      CardCmd.Upgrade(card, CardPreviewStyle.MessyLayout);
      await Cmd.CustomScaledWait(0.3f, 0.5f);
    }
    await Cmd.CustomScaledWait(0.6f, 1.2f);
    this.SetEventFinished(this.L10NLookup("REFLECTIONS.pages.TOUCH_A_MIRROR.description"));
    upgradedCards = (List<CardModel>) null;
    upgradableCards = (List<CardModel>) null;
  }

  private async Task Shatter()
  {
    int originalDeckSize = this.Owner.Deck.Cards.Count;
    for (int i = 0; i < originalDeckSize; ++i)
    {
      CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(this.Owner.RunState.CloneCard(this.Owner.Deck.Cards[i]), PileType.Deck), style: CardPreviewStyle.MessyLayout);
      await Cmd.CustomScaledWait(0.1f, 0.2f);
    }
    await Cmd.CustomScaledWait(0.6f, 1.2f);
    CardModel deck = await CardPileCmd.AddCurseToDeck<BadLuck>(this.Owner);
    this.SetEventFinished(this.L10NLookup("REFLECTIONS.pages.SHATTER.description"));
  }
}
