// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.NeowsTorment
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Cards;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class NeowsTorment : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Ancient;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get => HoverTipFactory.FromCardWithCardHoverTips<NeowsFury>();
  }

  public override bool HasUponPickupEffect => true;

  public override async Task AfterObtained()
  {
    // ISSUE: object of a compiler-generated type is created
    CardCmd.PreviewCardPileAdd((IReadOnlyList<CardPileAddResult>) new \u003C\u003Ez__ReadOnlySingleElementList<CardPileAddResult>(await CardPileCmd.Add((CardModel) this.Owner.RunState.CreateCard<NeowsFury>(this.Owner), PileType.Deck)), 2f);
  }
}
