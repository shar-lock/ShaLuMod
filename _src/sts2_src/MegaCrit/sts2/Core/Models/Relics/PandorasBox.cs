// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.PandorasBox
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class PandorasBox : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Ancient;

  public override bool HasUponPickupEffect => true;

  public override async Task AfterObtained()
  {
    List<CardPileAddResult> list = (await CardCmd.Transform(PileType.Deck.GetPile(this.Owner).Cards.Where<CardModel>((Func<CardModel, bool>) (c => c != null && c.IsBasicStrikeOrDefend && c.IsRemovable)).ToList<CardModel>().Select<CardModel, CardTransformation>((Func<CardModel, CardTransformation>) (c => new CardTransformation(c, CardFactory.CreateRandomCardForTransform(c, false, this.Owner.RunState.Rng.Niche)))), (Rng) null, CardPreviewStyle.None)).ToList<CardPileAddResult>();
    if (list.Count <= 0 || !LocalContext.IsMe(this.Owner))
      return;
    NSimpleCardsViewScreen.ShowScreen(list, new LocString("relics", "PANDORAS_BOX.infoText"));
  }
}
