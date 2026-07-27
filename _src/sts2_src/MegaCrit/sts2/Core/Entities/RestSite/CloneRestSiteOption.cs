// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.RestSite.CloneRestSiteOption
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.RestSite;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.RestSite;

public class CloneRestSiteOption(Player owner) : RestSiteOption(owner)
{
  public override string OptionId => "CLONE";

  public override LocString Description
  {
    get
    {
      LocString description = base.Description;
      description.Add("EnchantmentName", ModelDb.Enchantment<Clone>().Title.GetFormattedText());
      return description;
    }
  }

  public override async Task<bool> OnSelect()
  {
    IEnumerable<CardModel> list = (IEnumerable<CardModel>) this.Owner.Deck.Cards.Where<CardModel>((Func<CardModel, bool>) (c => c.Enchantment is Clone)).ToList<CardModel>();
    List<CardPileAddResult> results = new List<CardPileAddResult>();
    foreach (CardModel mutableCard in list)
    {
      CardModel card = this.Owner.RunState.CloneCard(mutableCard);
      List<CardPileAddResult> cardPileAddResultList = results;
      cardPileAddResultList.Add(await CardPileCmd.Add(card, PileType.Deck));
      cardPileAddResultList = (List<CardPileAddResult>) null;
    }
    CardCmd.PreviewCardPileAdd((IReadOnlyList<CardPileAddResult>) results, style: CardPreviewStyle.MessyLayout);
    bool flag = true;
    results = (List<CardPileAddResult>) null;
    return flag;
  }

  public override Task DoLocalPostSelectVfx(CancellationToken ct = default (CancellationToken))
  {
    NGame.Instance?.ScreenShake(ShakeStrength.Strong, ShakeDuration.Short);
    return Task.CompletedTask;
  }

  public override Task DoRemotePostSelectVfx()
  {
    NRestSiteRoom instance = NRestSiteRoom.Instance;
    NRestSiteCharacter parent = instance != null ? instance.Characters.First<NRestSiteCharacter>((Func<NRestSiteCharacter, bool>) (c => c.Player == this.Owner)) : (NRestSiteCharacter) null;
    parent?.Shake();
    NRelicFlashVfx child = NRelicFlashVfx.Create((RelicModel) ModelDb.Relic<PaelsGrowth>());
    if (child == null)
      return Task.CompletedTask;
    if (parent != null)
      ((Node) parent).AddChildSafely((Node) child);
    child.Position = Vector2.Zero;
    return Task.CompletedTask;
  }
}
