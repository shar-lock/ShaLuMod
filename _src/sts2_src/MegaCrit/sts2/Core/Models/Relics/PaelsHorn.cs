// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.PaelsHorn
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

public sealed class PaelsHorn : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Ancient;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get => HoverTipFactory.FromCardWithCardHoverTips<Relax>();
  }

  public override async Task AfterObtained()
  {
    List<CardPileAddResult> results = new List<CardPileAddResult>();
    for (int i = 0; i < 2; ++i)
      results.Add(await CardPileCmd.Add((CardModel) this.Owner.RunState.CreateCard<Relax>(this.Owner), PileType.Deck));
    CardCmd.PreviewCardPileAdd((IReadOnlyList<CardPileAddResult>) results, 2f);
    results = (List<CardPileAddResult>) null;
  }
}
