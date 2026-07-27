// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Merchant.MerchantInventory
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Merchant;

public class MerchantInventory
{
  private static readonly CardType[] _coloredCardTypes = new CardType[5]
  {
    CardType.Attack,
    CardType.Attack,
    CardType.Skill,
    CardType.Skill,
    CardType.Power
  };
  private static readonly CardRarity[] _colorlessCardRarities = new CardRarity[2]
  {
    CardRarity.Uncommon,
    CardRarity.Rare
  };
  private readonly List<MerchantCardEntry> _characterCardEntries = new List<MerchantCardEntry>();
  private readonly List<MerchantCardEntry> _colorlessCardEntries = new List<MerchantCardEntry>();
  private readonly List<MerchantRelicEntry> _relicEntries = new List<MerchantRelicEntry>();
  private readonly List<MerchantPotionEntry> _potionEntries = new List<MerchantPotionEntry>();

  public IReadOnlyList<MerchantCardEntry> CharacterCardEntries
  {
    get => (IReadOnlyList<MerchantCardEntry>) this._characterCardEntries;
  }

  public IReadOnlyList<MerchantCardEntry> ColorlessCardEntries
  {
    get => (IReadOnlyList<MerchantCardEntry>) this._colorlessCardEntries;
  }

  public IReadOnlyList<MerchantRelicEntry> RelicEntries
  {
    get => (IReadOnlyList<MerchantRelicEntry>) this._relicEntries;
  }

  public IReadOnlyList<MerchantPotionEntry> PotionEntries
  {
    get => (IReadOnlyList<MerchantPotionEntry>) this._potionEntries;
  }

  public MerchantCardRemovalEntry? CardRemovalEntry { get; private set; }

  public Player Player { get; }

  public IEnumerable<MerchantEntry> AllEntries
  {
    get
    {
      return ((IEnumerable<IEnumerable<MerchantEntry>>) new IEnumerable<MerchantEntry>[4]
      {
        (IEnumerable<MerchantEntry>) this.CardEntries,
        (IEnumerable<MerchantEntry>) this.RelicEntries,
        (IEnumerable<MerchantEntry>) this.PotionEntries,
        this.CardRemovalEntry != null ? (IEnumerable<MerchantEntry>) new \u003C\u003Ez__ReadOnlySingleElementList<MerchantEntry>((MerchantEntry) this.CardRemovalEntry) : (IEnumerable<MerchantEntry>) Array.Empty<MerchantEntry>()
      }).SelectMany<IEnumerable<MerchantEntry>, MerchantEntry>((Func<IEnumerable<MerchantEntry>, IEnumerable<MerchantEntry>>) (e => e));
    }
  }

  public IEnumerable<MerchantCardEntry> CardEntries
  {
    get
    {
      return this.CharacterCardEntries.Concat<MerchantCardEntry>((IEnumerable<MerchantCardEntry>) this.ColorlessCardEntries);
    }
  }

  public MerchantInventory(Player player) => this.Player = player;

  public static MerchantInventory CreateForNormalMerchant(Player player)
  {
    MerchantInventory forNormalMerchant = new MerchantInventory(player);
    forNormalMerchant.PopulateCharacterCardEntries();
    forNormalMerchant.PopulateColorlessCardEntries();
    forNormalMerchant.PopulateRelicEntries();
    forNormalMerchant.PopulatePotionEntries();
    forNormalMerchant.CardRemovalEntry = new MerchantCardRemovalEntry(player);
    foreach (MerchantEntry allEntry in forNormalMerchant.AllEntries)
      allEntry.PurchaseCompleted += new Action<PurchaseStatus, MerchantEntry>(forNormalMerchant.UpdateEntries);
    return forNormalMerchant;
  }

  public void AddRelicEntry(MerchantRelicEntry entry) => this._relicEntries.Add(entry);

  private void PopulateCharacterCardEntries()
  {
    int num = this.Player.PlayerRng.Shops.NextInt(MerchantInventory._coloredCardTypes.Length);
    List<CardModel> list = this.Player.Character.CardPool.GetUnlockedCards(this.Player.UnlockState, this.Player.RunState.CardMultiplayerConstraint).ToList<CardModel>();
    for (int index = 0; index < MerchantInventory._coloredCardTypes.Length; ++index)
    {
      MerchantCardEntry merchantCardEntry = new MerchantCardEntry(this.Player, this, (IEnumerable<CardModel>) list, MerchantInventory._coloredCardTypes[index]);
      merchantCardEntry.Populate();
      this._characterCardEntries.Add(merchantCardEntry);
      if (num == index)
        merchantCardEntry.SetOnSale();
    }
  }

  private void PopulateColorlessCardEntries()
  {
    List<CardModel> list = ModelDb.CardPool<ColorlessCardPool>().GetUnlockedCards(this.Player.UnlockState, this.Player.RunState.CardMultiplayerConstraint).ToList<CardModel>();
    foreach (CardRarity colorlessCardRarity in MerchantInventory._colorlessCardRarities)
    {
      MerchantCardEntry merchantCardEntry = new MerchantCardEntry(this.Player, this, (IEnumerable<CardModel>) list, colorlessCardRarity);
      merchantCardEntry.Populate();
      this._colorlessCardEntries.Add(merchantCardEntry);
    }
  }

  private void PopulateRelicEntries()
  {
    RelicRarity[] relicRarityArray = new RelicRarity[3]
    {
      RelicFactory.RollRarity(this.Player),
      RelicFactory.RollRarity(this.Player),
      RelicRarity.Shop
    };
    foreach (RelicRarity rarity in relicRarityArray)
      this.AddRelicEntry(new MerchantRelicEntry(rarity, this.Player));
  }

  private void PopulatePotionEntries()
  {
    foreach (PotionModel potionModel in PotionFactory.CreateRandomPotionsOutOfCombat(this.Player, 3, this.Player.PlayerRng.Shops).ToList<PotionModel>())
      this._potionEntries.Add(new MerchantPotionEntry(potionModel.ToMutable(), this.Player));
  }

  private void UpdateEntries(PurchaseStatus _, MerchantEntry __)
  {
    foreach (MerchantEntry allEntry in this.AllEntries)
      allEntry.OnMerchantInventoryUpdated();
  }
}
