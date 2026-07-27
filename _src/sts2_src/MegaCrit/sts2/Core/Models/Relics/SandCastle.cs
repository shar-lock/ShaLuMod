// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.SandCastle
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class SandCastle : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Ancient;

  public override bool HasUponPickupEffect => true;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new CardsVar(6));
    }
  }

  public override Task AfterObtained()
  {
    IEnumerable<CardModel> cardModels = PileType.Deck.GetPile(this.Owner).Cards.Where<CardModel>((Func<CardModel, bool>) (c => c != null && c.IsUpgradable)).ToList<CardModel>().StableShuffle<CardModel>(this.Owner.RunState.Rng.Niche).Take<CardModel>(this.DynamicVars.Cards.IntValue);
    NRun.Instance?.GlobalUi.GridCardPreviewContainer.ForceMaxColumnsUntilEmpty(3);
    foreach (CardModel card in cardModels)
      CardCmd.Upgrade(card, CardPreviewStyle.GridLayout);
    return Task.CompletedTask;
  }
}
