// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.ArchaicTooth
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class ArchaicTooth : RelicModel
{
  private const string _starterCardKey = "StarterCard";
  private const string _ancientCardKey = "AncientCard";
  private SerializableCard? _serializableStarterCard;
  private SerializableCard? _serializableAncientCard;
  private List<IHoverTip> _extraHoverTips = new List<IHoverTip>();

  private static Dictionary<ModelId, CardModel> TranscendenceUpgrades
  {
    get
    {
      return new Dictionary<ModelId, CardModel>()
      {
        {
          ModelDb.Card<Bash>().Id,
          (CardModel) ModelDb.Card<Break>()
        },
        {
          ModelDb.Card<Neutralize>().Id,
          (CardModel) ModelDb.Card<Suppress>()
        },
        {
          ModelDb.Card<Unleash>().Id,
          (CardModel) ModelDb.Card<Protector>()
        },
        {
          ModelDb.Card<FallingStar>().Id,
          (CardModel) ModelDb.Card<MeteorShower>()
        },
        {
          ModelDb.Card<Dualcast>().Id,
          (CardModel) ModelDb.Card<Quadcast>()
        }
      };
    }
  }

  public static List<CardModel> TranscendenceCards
  {
    get => ArchaicTooth.TranscendenceUpgrades.Values.ToList<CardModel>();
  }

  public override RelicRarity Rarity => RelicRarity.Ancient;

  [SavedProperty]
  public SerializableCard? StarterCard
  {
    get => this._serializableStarterCard;
    private set
    {
      this.AssertMutable();
      this._serializableStarterCard = value;
      this.UpdateHoverTips();
    }
  }

  [SavedProperty]
  public SerializableCard? AncientCard
  {
    get => this._serializableAncientCard;
    private set
    {
      this.AssertMutable();
      this._serializableAncientCard = value;
      this.UpdateHoverTips();
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new StringVar("StarterCard"),
        (DynamicVar) new StringVar("AncientCard")
      });
    }
  }

  protected override void AfterCloned()
  {
    base.AfterCloned();
    this._extraHoverTips = new List<IHoverTip>();
  }

  public bool SetupForPlayer(Player player)
  {
    this.AssertMutable();
    CardModel transcendenceStarterCard = this.GetTranscendenceStarterCard(player);
    if (transcendenceStarterCard == null)
      return false;
    this.StarterCard = transcendenceStarterCard.ToSerializable();
    this.AncientCard = this.GetTranscendenceTransformedCard(transcendenceStarterCard).ToSerializable();
    this.UpdateHoverTips();
    return true;
  }

  public void SetupForTests(SerializableCard starterCard, SerializableCard ancientCard)
  {
    this.AssertMutable();
    this.StarterCard = starterCard;
    this.AncientCard = ancientCard;
    this.UpdateHoverTips();
  }

  private void UpdateHoverTips()
  {
    this._extraHoverTips.Clear();
    if (this.StarterCard != null)
    {
      CardModel card = CardModel.FromSerializable(this.StarterCard);
      this._extraHoverTips.AddRange(card.HoverTips);
      this._extraHoverTips.Add(HoverTipFactory.FromCard(card));
      ((StringVar) this.DynamicVars["StarterCard"]).StringValue = card.Title;
    }
    if (this.AncientCard == null)
      return;
    CardModel card1 = CardModel.FromSerializable(this.AncientCard);
    this._extraHoverTips.AddRange(card1.HoverTips);
    this._extraHoverTips.Add(HoverTipFactory.FromCard(card1));
    ((StringVar) this.DynamicVars["AncientCard"]).StringValue = card1.Title;
  }

  private CardModel? GetTranscendenceStarterCard(Player player)
  {
    return player.Deck.Cards.FirstOrDefault<CardModel>((Func<CardModel, bool>) (c => ArchaicTooth.TranscendenceUpgrades.ContainsKey(c.Id)));
  }

  private CardModel GetTranscendenceTransformedCard(CardModel starterCard)
  {
    CardModel canonicalCard;
    if (!ArchaicTooth.TranscendenceUpgrades.TryGetValue(starterCard.Id, out canonicalCard))
      return (CardModel) this.Owner.RunState.CreateCard<Doubt>(starterCard.Owner);
    CardModel card = starterCard.Owner.RunState.CreateCard(canonicalCard, starterCard.Owner);
    if (starterCard.IsUpgraded)
      CardCmd.Upgrade(card);
    if (starterCard.Enchantment != null)
    {
      EnchantmentModel enchantment = (EnchantmentModel) starterCard.Enchantment.MutableClone();
      CardCmd.Enchant(enchantment, card, (Decimal) enchantment.Amount);
    }
    return card;
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get => (IEnumerable<IHoverTip>) this._extraHoverTips;
  }

  public override async Task AfterObtained()
  {
    CardModel transcendenceStarterCard = this.GetTranscendenceStarterCard(this.Owner);
    CardPileAddResult? nullable = await CardCmd.Transform(transcendenceStarterCard, this.GetTranscendenceTransformedCard(transcendenceStarterCard));
  }
}
