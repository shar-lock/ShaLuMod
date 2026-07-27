// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.NutritiousSoup
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class NutritiousSoup : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Ancient;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get => HoverTipFactory.FromEnchantment<TezcatarasEmber>();
  }

  public override Task AfterObtained()
  {
    foreach (CardModel card in (IEnumerable<CardModel>) PileType.Deck.GetPile(this.Owner).Cards.ToList<CardModel>())
    {
      if (card.Rarity == CardRarity.Basic && card.Tags.Contains<CardTag>(CardTag.Strike) && ModelDb.Enchantment<TezcatarasEmber>().CanEnchant(card))
      {
        CardCmd.Enchant<TezcatarasEmber>(card, 1M);
        NCardEnchantVfx child = NCardEnchantVfx.Create(card);
        if (child != null)
        {
          NRun instance = NRun.Instance;
          if (instance != null)
            ((Node) instance.GlobalUi.CardPreviewContainer).AddChildSafely((Node) child);
        }
      }
    }
    return Task.CompletedTask;
  }
}
