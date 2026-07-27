// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.PaelsClaw
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class PaelsClaw : RelicModel
{
  public const int cardsCount = 3;
  private const int _enchantmentAmount = 2;

  public override RelicRarity Rarity => RelicRarity.Ancient;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new CardsVar(3),
        (DynamicVar) new StringVar("EnchantmentName", ModelDb.Enchantment<Goopy>().Title.GetFormattedText())
      });
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get => HoverTipFactory.FromEnchantment<Goopy>(2);
  }

  public override Task AfterObtained()
  {
    foreach (CardModel card in (IEnumerable<CardModel>) PileType.Deck.GetPile(this.Owner).Cards.ToList<CardModel>())
    {
      if (ModelDb.Enchantment<Goopy>().CanEnchant(card))
      {
        CardCmd.Enchant<Goopy>(card, 1M);
        NRun instance = NRun.Instance;
        if (instance != null)
          ((Node) instance.GlobalUi.CardPreviewContainer).AddChildSafely((Node) NCardEnchantVfx.Create(card));
      }
    }
    return Task.CompletedTask;
  }
}
