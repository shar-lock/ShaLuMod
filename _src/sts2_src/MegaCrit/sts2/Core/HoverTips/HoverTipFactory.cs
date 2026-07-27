// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.HoverTips.HoverTipFactory
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Runs;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.HoverTips;

public static class HoverTipFactory
{
  private static readonly Dictionary<CardKeyword, HoverTip> _keywordHoverTips = new Dictionary<CardKeyword, HoverTip>();
  private static readonly Dictionary<ModelId, HoverTip> _potionHoverTips = new Dictionary<ModelId, HoverTip>();

  public static IEnumerable<IHoverTip> FromEnchantment<T>(int amount = 1) where T : EnchantmentModel
  {
    EnchantmentModel mutable = ModelDb.Enchantment<T>().ToMutable();
    mutable.Amount = amount;
    mutable.RecalculateValues();
    return mutable.HoverTips;
  }

  public static IEnumerable<IHoverTip> FromAffliction<T>(int amount = 1) where T : AfflictionModel
  {
    AfflictionModel mutable = ModelDb.Affliction<T>().ToMutable();
    mutable.Amount = amount;
    return mutable.HoverTips;
  }

  public static IHoverTip FromKeyword(CardKeyword keyword)
  {
    if (!HoverTipFactory._keywordHoverTips.ContainsKey(keyword))
      HoverTipFactory._keywordHoverTips[keyword] = new HoverTip(keyword.GetTitle(), keyword.GetDescription());
    return (IHoverTip) HoverTipFactory._keywordHoverTips[keyword];
  }

  public static IHoverTip FromPower<T>(int? amount = null) where T : PowerModel
  {
    return HoverTipFactory.FromPower((PowerModel) ModelDb.Power<T>(), amount);
  }

  public static IHoverTip FromPower(PowerModel model, int? amount = null)
  {
    return (IHoverTip) model.GetDumbHoverTip(amount);
  }

  public static IEnumerable<IHoverTip> FromPowerWithPowerHoverTips<T>(int? amount = null) where T : PowerModel
  {
    return ((IEnumerable<IHoverTip>) new IHoverTip[1]
    {
      HoverTipFactory.FromPower<T>(amount)
    }).Concat<IHoverTip>(ModelDb.Power<T>().HoverTips);
  }

  public static IHoverTip FromPotion<T>() where T : PotionModel
  {
    return HoverTipFactory.FromPotion((PotionModel) ModelDb.Potion<T>());
  }

  public static IHoverTip FromPotion(PotionModel model)
  {
    HoverTipFactory._potionHoverTips.TryAdd(model.Id, model.HoverTip);
    return (IHoverTip) HoverTipFactory._potionHoverTips[model.Id];
  }

  public static IHoverTip FromOrb<T>() where T : OrbModel
  {
    return (IHoverTip) ModelDb.Orb<T>().DumbHoverTip;
  }

  public static IEnumerable<IHoverTip> FromRelic<T>() where T : RelicModel
  {
    return HoverTipFactory.FromRelic((RelicModel) ModelDb.Relic<T>());
  }

  public static IEnumerable<IHoverTip> FromRelicExcludingItself<T>() where T : RelicModel
  {
    return HoverTipFactory.FromRelicExcludingItself((RelicModel) ModelDb.Relic<T>());
  }

  public static IEnumerable<IHoverTip> FromRelic(RelicModel relic) => relic.HoverTips;

  public static IEnumerable<IHoverTip> FromRelicExcludingItself(RelicModel relic)
  {
    return relic.HoverTipsExcludingRelic;
  }

  public static IEnumerable<IHoverTip> FromCardWithCardHoverTips<T>(bool upgrade = false) where T : CardModel
  {
    return ((IEnumerable<IHoverTip>) new IHoverTip[1]
    {
      HoverTipFactory.FromCard<T>(upgrade)
    }).Concat<IHoverTip>(ModelDb.Card<T>().HoverTips);
  }

  public static IHoverTip FromCard<T>(bool upgrade = false) where T : CardModel
  {
    return HoverTipFactory.FromCard((CardModel) ModelDb.Card<T>(), upgrade);
  }

  public static IHoverTip FromCard(CardModel card, bool upgrade = false)
  {
    if (upgrade)
    {
      card = (CardModel) card.MutableClone();
      card.UpgradeInternal();
      card.FinalizeUpgradeInternal();
    }
    return (IHoverTip) new CardHoverTip(card);
  }

  public static IHoverTip Static(StaticHoverTip tip, params DynamicVar[] vars)
  {
    string str = StringHelper.Slugify(tip.ToString());
    LocString title = HoverTipFactory.L10NStatic(str + ".title");
    LocString description = HoverTipFactory.L10NStatic(str + ".description");
    foreach (DynamicVar var in vars)
    {
      title.Add(var);
      description.Add(var);
    }
    return (IHoverTip) new HoverTip(title, description);
  }

  public static IHoverTip ForEnergy(CardModel card)
  {
    return HoverTipFactory.ForEnergyWithIconPath(EnergyIconHelper.GetPath((AbstractModel) card));
  }

  public static IHoverTip ForEnergy(PotionModel potion)
  {
    return HoverTipFactory.ForEnergyWithIconPath(EnergyIconHelper.GetPath((AbstractModel) potion));
  }

  public static IHoverTip ForEnergy(PowerModel power)
  {
    return HoverTipFactory.ForEnergyWithIconPath(EnergyIconHelper.GetPath((AbstractModel) power));
  }

  public static IHoverTip ForEnergy(Player player)
  {
    return HoverTipFactory.ForEnergyWithIconPath(EnergyIconHelper.GetPath((AbstractModel) player.Character.CardPool));
  }

  public static IHoverTip ForEnergy(RelicModel relic)
  {
    return HoverTipFactory.ForEnergyWithIconPath(EnergyIconHelper.GetPath((AbstractModel) relic));
  }

  private static IHoverTip ForEnergyWithIconPath(string path)
  {
    string energyIconPrefix = RunManager.Instance.GetLocalCharacterEnergyIconPrefix();
    return (IHoverTip) new HoverTip(HoverTipFactory.L10NStatic("ENERGY.title"), HoverTipFactory.L10NStatic("ENERGY.description"), PreloadManager.Cache.GetTexture2D(energyIconPrefix != null ? EnergyIconHelper.GetPath(energyIconPrefix) : path));
  }

  public static IEnumerable<IHoverTip> FromForge()
  {
    List<IHoverTip> items = new List<IHoverTip>();
    items.Add(HoverTipFactory.Static(StaticHoverTip.Forge));
    items.AddRange(HoverTipFactory.FromCardWithCardHoverTips<SovereignBlade>());
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlyList<IHoverTip>(items);
  }

  private static LocString L10NStatic(string entry) => new LocString("static_hover_tips", entry);
}
