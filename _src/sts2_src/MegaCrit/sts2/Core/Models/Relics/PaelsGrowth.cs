// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.PaelsGrowth
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Enchantments;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class PaelsGrowth : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Ancient;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new StringVar("EnchantmentName", ModelDb.Enchantment<Clone>().Title.GetFormattedText()));
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get => HoverTipFactory.FromEnchantment<Clone>();
  }

  public override async Task AfterObtained()
  {
    CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1);
    foreach (CardModel card in await CardSelectCmd.FromDeckForEnchantment(this.Owner, (EnchantmentModel) ModelDb.Enchantment<Clone>(), 1, prefs))
    {
      CardCmd.Enchant<Clone>(card, 4M);
      CardCmd.Preview(card);
    }
  }

  public override bool TryModifyRestSiteOptions(Player player, ICollection<RestSiteOption> options)
  {
    if (player != this.Owner)
      return false;
    options.Add((RestSiteOption) new CloneRestSiteOption(player));
    return true;
  }
}
