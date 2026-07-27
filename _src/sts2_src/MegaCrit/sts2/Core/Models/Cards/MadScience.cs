// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.MadScience
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class MadScience : CardModel
{
  public const int attackDamage = 12;
  public const int skillBlock = 8;
  public const string sappingWeakKey = "SappingWeak";
  public const int sappingWeakValue = 2;
  public const string sappingVulnerableKey = "SappingVulnerable";
  public const int sappingVulnerableValue = 2;
  public const string violenceHitsKey = "ViolenceHits";
  public const int violenceHitsValue = 3;
  public const string chokingDamageKey = "ChokingDamage";
  public const int chokingDamageValue = 6;
  public const string energizedEnergyKey = "EnergizedEnergy";
  public const int energizedEnergyValue = 2;
  public const string wisdomCardsKey = "WisdomCards";
  public const int wisdomCardsValue = 3;
  public const string expertiseStrengthKey = "ExpertiseStrength";
  public const int expertiseStrengthValue = 2;
  public const string expertiseDexterityKey = "ExpertiseDexterity";
  public const int expertiseDexterityValue = 2;
  public const string curiousReductionKey = "CuriousReduction";
  public const int curiousReductionValue = 1;
  private CardType _tinkerTimeType;
  private TinkerTime.RiderEffect _tinkerTimeRider;
  private CardModel? _mockedChaosCard;

  public MadScience()
    : base(1, CardType.Attack, CardRarity.Event, TargetType.AnyEnemy, false)
  {
  }

  public override string PortraitPath => this.GetPortraitPath(this.TinkerTimeType);

  public override string BetaPortraitPath => CardModel.MissingPortraitPath;

  string[] CardModel.AllPortraitPaths
  {
    [PreserveBaseOverrides] get
    {
      return new string[3]
      {
        this.GetPortraitPath(CardType.Attack),
        this.GetPortraitPath(CardType.Skill),
        this.GetPortraitPath(CardType.Power)
      };
    }
  }

  private string GetPortraitPath(CardType cardType)
  {
    return ImageHelper.GetImagePath($"atlases/card_atlas.sprites/event/{this.GetPortraitFilename(cardType)}.tres");
  }

  private string GetPortraitFilename(CardType cardType)
  {
    switch (cardType)
    {
      case CardType.None:
        return "mad_science_attack";
      case CardType.Attack:
        return "mad_science_attack";
      case CardType.Skill:
        return "mad_science_skill";
      case CardType.Power:
        return "mad_science_power";
      default:
        throw new InvalidOperationException($"Mad Science is invalid type {this.TinkerTimeType}.");
    }
  }

  public override CardType Type => this.TinkerTimeType;

  public override TargetType TargetType
  {
    get => this.TinkerTimeType != CardType.Attack ? TargetType.Self : TargetType.AnyEnemy;
  }

  public override bool GainsBlock => this.TinkerTimeType == CardType.Skill;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[11]
      {
        (DynamicVar) new DamageVar(12M, ValueProp.Move),
        (DynamicVar) new BlockVar(8M, ValueProp.Move),
        (DynamicVar) new PowerVar<WeakPower>("SappingWeak", 2M),
        (DynamicVar) new PowerVar<VulnerablePower>("SappingVulnerable", 2M),
        new DynamicVar("ViolenceHits", 3M),
        (DynamicVar) new PowerVar<StranglePower>("ChokingDamage", 6M),
        (DynamicVar) new EnergyVar("EnergizedEnergy", 2),
        (DynamicVar) new CardsVar("WisdomCards", 3),
        (DynamicVar) new PowerVar<StrengthPower>("ExpertiseStrength", 2M),
        (DynamicVar) new PowerVar<DexterityPower>("ExpertiseDexterity", 2M),
        new DynamicVar("CuriousReduction", 1M)
      });
    }
  }

  [SavedProperty(SerializationCondition.AlwaysSave, -1)]
  public CardType TinkerTimeType
  {
    get => this._tinkerTimeType;
    set
    {
      this.AssertMutable();
      this._tinkerTimeType = value;
    }
  }

  [SavedProperty(SerializationCondition.SaveIfNotTypeDefault)]
  public TinkerTime.RiderEffect TinkerTimeRider
  {
    get => this._tinkerTimeRider;
    set
    {
      this.AssertMutable();
      this._tinkerTimeRider = value;
    }
  }

  private CardModel? MockedChaosCard
  {
    get => this._mockedChaosCard;
    set
    {
      this.AssertMutable();
      this._mockedChaosCard = value;
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return this.TinkerTimeRider != TinkerTime.RiderEffect.None ? (IEnumerable<IHoverTip>) TinkerTime.GetRiderHoverTips(this.TinkerTimeRider) : (IEnumerable<IHoverTip>) Array.Empty<IHoverTip>();
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (this.TargetType == TargetType.AnyEnemy && cardPlay.Target == null)
      ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    switch (this.TinkerTimeType)
    {
      case CardType.Attack:
        await this.ExecuteAttack(choiceContext, cardPlay.Target, cardPlay);
        break;
      case CardType.Skill:
        await this.ExecuteSkill(cardPlay);
        break;
      case CardType.Power:
        await this.ExecutePower(choiceContext);
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
    bool flag;
    switch (this.TinkerTimeRider)
    {
      case TinkerTime.RiderEffect.Sapping:
      case TinkerTime.RiderEffect.Choking:
      case TinkerTime.RiderEffect.Energized:
      case TinkerTime.RiderEffect.Wisdom:
      case TinkerTime.RiderEffect.Chaos:
        flag = true;
        break;
      default:
        flag = false;
        break;
    }
    if (!flag)
      return;
    await this.ExecuteRider(this.TinkerTimeRider, cardPlay.Target, choiceContext);
  }

  private async Task ExecuteAttack(
    PlayerChoiceContext choiceContext,
    Creature target,
    CardPlay cardPlay)
  {
    int hitCount = this.TinkerTimeRider == TinkerTime.RiderEffect.Violence ? this.DynamicVars["ViolenceHits"].IntValue : 1;
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).WithHitCount(hitCount).FromCard((CardModel) this, cardPlay).Targeting(target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
  }

  private async Task ExecuteSkill(CardPlay cardPlay)
  {
    Decimal num = await CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars.Block, cardPlay);
  }

  private async Task ExecutePower(PlayerChoiceContext choiceContext)
  {
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
    switch (this.TinkerTimeRider)
    {
      case TinkerTime.RiderEffect.Expertise:
        StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>(choiceContext, this.Owner.Creature, this.DynamicVars["ExpertiseStrength"].BaseValue, this.Owner.Creature, (CardModel) this);
        DexterityPower dexterityPower = await PowerCmd.Apply<DexterityPower>(choiceContext, this.Owner.Creature, this.DynamicVars["ExpertiseDexterity"].BaseValue, this.Owner.Creature, (CardModel) this);
        break;
      case TinkerTime.RiderEffect.Curious:
        CuriousPower curiousPower = await PowerCmd.Apply<CuriousPower>(choiceContext, this.Owner.Creature, this.DynamicVars["CuriousReduction"].BaseValue, this.Owner.Creature, (CardModel) this);
        break;
      case TinkerTime.RiderEffect.Improvement:
        ImprovementPower improvementPower = await PowerCmd.Apply<ImprovementPower>(choiceContext, this.Owner.Creature, 1M, this.Owner.Creature, (CardModel) this);
        break;
    }
  }

  protected override void OnUpgrade() => this.AddKeyword(CardKeyword.Innate);

  protected override void AddExtraArgsToDescription(LocString description)
  {
    description.Add("CardType", this.TinkerTimeType.ToString());
    description.Add("HasRider", this.TinkerTimeRider != 0);
    foreach (TinkerTime.RiderEffect riderEffect in Enum.GetValues<TinkerTime.RiderEffect>())
      description.Add(riderEffect.ToString(), this.TinkerTimeRider == riderEffect);
  }

  private async Task ExecuteRider(
    TinkerTime.RiderEffect rider,
    Creature? target,
    PlayerChoiceContext choiceContext)
  {
    switch (rider)
    {
      case TinkerTime.RiderEffect.Sapping:
        WeakPower weakPower = await PowerCmd.Apply<WeakPower>(choiceContext, target, this.DynamicVars["SappingWeak"].BaseValue, this.Owner.Creature, (CardModel) this);
        VulnerablePower vulnerablePower = await PowerCmd.Apply<VulnerablePower>(choiceContext, target, this.DynamicVars["SappingVulnerable"].BaseValue, this.Owner.Creature, (CardModel) this);
        break;
      case TinkerTime.RiderEffect.Choking:
        StranglePower stranglePower = await PowerCmd.Apply<StranglePower>(choiceContext, target, this.DynamicVars["ChokingDamage"].BaseValue, this.Owner.Creature, (CardModel) this);
        break;
      case TinkerTime.RiderEffect.Energized:
        await PlayerCmd.GainEnergy((Decimal) this.DynamicVars["EnergizedEnergy"].IntValue, this.Owner);
        break;
      case TinkerTime.RiderEffect.Wisdom:
        IEnumerable<CardModel> cardModels = await CardPileCmd.Draw(choiceContext, (Decimal) this.DynamicVars["WisdomCards"].IntValue, this.Owner);
        break;
      case TinkerTime.RiderEffect.Chaos:
        CardModel card = this.MockedChaosCard == null ? CardFactory.GetDistinctForCombat(this.Owner, this.Owner.Character.CardPool.GetUnlockedCards(this.Owner.UnlockState, this.Owner.RunState.CardMultiplayerConstraint), 1, this.Owner.RunState.Rng.CombatCardGeneration).First<CardModel>() : this.MockedChaosCard;
        card.SetToFreeThisTurn();
        CardPileAddResult combat = await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, this.Owner);
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof (rider), (object) rider, (string) null);
    }
  }

  public void MockChaosCard(CardModel card)
  {
    this.AssertMutable();
    card.AssertMutable();
    this.MockedChaosCard = card;
  }
}
