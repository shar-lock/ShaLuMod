// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.AbstractModel
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.CardRewardAlternatives;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models.Exceptions;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.SourceGeneration;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models;

[GenerateSubtypes]
public abstract class AbstractModel : IComparable<AbstractModel>
{
  public event Action<AbstractModel>? ExecutionFinished;

  public ModelId Id { get; }

  public bool IsMutable { get; private set; }

  public bool IsCanonical => !this.IsMutable;

  public int CategorySortingId { get; private set; }

  public int EntrySortingId { get; private set; }

  protected AbstractModel()
  {
    Type type = this.GetType();
    ModelId id = ModelDb.GetId(type);
    AbstractModel byIdOrNull = ModelDb.GetByIdOrNull<AbstractModel>(id);
    if (byIdOrNull != null)
      throw new DuplicateModelException($"ModelDb already contains ID {id} mapped to type {byIdOrNull.GetType()}, but you are trying to map it to type {type}. Possible causes:\n - You have called a constructor on an AbstractModel. Use ModelDb instead.\n - There is a conflict in mod content names.");
    this.Id = id;
  }

  public void InitId(ModelId id)
  {
    this.AssertCanonical();
    this.CategorySortingId = ModelIdSerializationCache.GetNetIdForCategory(this.Id.Category);
    this.EntrySortingId = ModelIdSerializationCache.GetNetIdForEntry(this.Id.Entry);
  }

  public virtual int CompareTo(AbstractModel? other)
  {
    if (this == other)
      return 0;
    return other == null ? 1 : this.Id.CompareTo(other.Id);
  }

  public virtual bool IsMock => false;

  public void AssertMutable()
  {
    if (!this.IsMutable)
      throw new CanonicalModelException(this.GetType());
  }

  public void AssertCanonical()
  {
    if (this.IsMutable)
      throw new MutableModelException(this.GetType());
  }

  public AbstractModel ClonePreservingMutability() => !this.IsMutable ? this : this.MutableClone();

  public AbstractModel MutableClone()
  {
    AbstractModel abstractModel = (AbstractModel) this.MemberwiseClone();
    abstractModel.IsMutable = true;
    abstractModel.DeepCloneFields();
    abstractModel.AfterCloned();
    return abstractModel;
  }

  protected virtual void DeepCloneFields() => this.AssertMutable();

  protected virtual void AfterCloned() => this.ExecutionFinished = (Action<AbstractModel>) null;

  public void InvokeExecutionFinished()
  {
    Action<AbstractModel> executionFinished = this.ExecutionFinished;
    if (executionFinished == null)
      return;
    executionFinished(this);
  }

  public virtual bool PreviewOutsideOfCombat => false;

  public abstract bool ShouldReceiveCombatHooks { get; }

  public virtual Task AfterActEntered() => Task.CompletedTask;

  public virtual Task AfterAddToDeckPrevented(CardModel card) => Task.CompletedTask;

  public virtual Task BeforeAttack(AttackCommand command) => Task.CompletedTask;

  public virtual Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterAutoPostPlayPhaseEntered(
    PlayerChoiceContext choiceContext,
    Player player)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterAutoPrePlayPhaseEnteredEarly(
    PlayerChoiceContext choiceContext,
    Player player)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterAutoPrePlayPhaseEntered(PlayerChoiceContext choiceContext, Player player)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterAutoPrePlayPhaseEnteredLate(
    PlayerChoiceContext choiceContext,
    Player player)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterBlockCleared(Creature creature) => Task.CompletedTask;

  public virtual Task BeforeBlockGained(
    Creature creature,
    Decimal amount,
    ValueProp props,
    CardModel? cardSource)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterBlockGained(
    Creature creature,
    Decimal amount,
    ValueProp props,
    CardModel? cardSource)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterBlockBroken(
    PlayerChoiceContext choiceContext,
    Creature target,
    Creature? breaker)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterCardChangedPiles(
    CardModel card,
    PileType oldPileType,
    AbstractModel? clonedBy)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterCardChangedPilesLate(
    CardModel card,
    PileType oldPileType,
    AbstractModel? clonedBy)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterCardDiscarded(PlayerChoiceContext choiceContext, CardModel card)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterCardDrawnEarly(
    PlayerChoiceContext choiceContext,
    CardModel card,
    bool fromHandDraw)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterCardDrawn(
    PlayerChoiceContext choiceContext,
    CardModel card,
    bool fromHandDraw)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterCardEnteredCombat(CardModel card) => Task.CompletedTask;

  public virtual Task AfterCardGeneratedForCombat(CardModel card, Player? creator)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterCardExhausted(
    PlayerChoiceContext choiceContext,
    CardModel card,
    bool causedByEthereal)
  {
    return Task.CompletedTask;
  }

  public virtual Task BeforeCardAutoPlayed(CardModel card, Creature? target, AutoPlayType type)
  {
    return Task.CompletedTask;
  }

  public virtual Task BeforeCardPlayed(CardPlay cardPlay) => Task.CompletedTask;

  public virtual Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterCardPlayedLate(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    return Task.CompletedTask;
  }

  public virtual Task BeforeCombatStart() => Task.CompletedTask;

  public virtual Task BeforeCombatStartLate() => Task.CompletedTask;

  public virtual Task AfterCombatEnd(CombatRoom room) => Task.CompletedTask;

  public virtual Task BeforeCombatRewardOffered(RewardsSet rewards, CombatRoom room)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterCombatVictoryEarly(CombatRoom room) => Task.CompletedTask;

  public virtual Task AfterCombatVictory(CombatRoom room) => Task.CompletedTask;

  public virtual Task AfterCreatureAddedToCombat(Creature creature) => Task.CompletedTask;

  public virtual Task AfterCurrentHpChanged(Creature creature, Decimal delta) => Task.CompletedTask;

  public virtual Task AfterDamageGiven(
    PlayerChoiceContext choiceContext,
    Creature? dealer,
    DamageResult result,
    ValueProp props,
    Creature target,
    CardModel? cardSource)
  {
    return Task.CompletedTask;
  }

  public virtual Task BeforeDamageReceived(
    PlayerChoiceContext choiceContext,
    Creature target,
    Decimal amount,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterDamageReceived(
    PlayerChoiceContext choiceContext,
    Creature target,
    DamageResult result,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterDamageReceivedLate(
    PlayerChoiceContext choiceContext,
    Creature target,
    DamageResult result,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource)
  {
    return Task.CompletedTask;
  }

  public virtual Task BeforeDeath(Creature creature) => Task.CompletedTask;

  public virtual Task AfterDeath(
    PlayerChoiceContext choiceContext,
    Creature creature,
    bool wasRemovalPrevented,
    float deathAnimLength)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterDiedToDoom(
    PlayerChoiceContext choiceContext,
    IReadOnlyList<Creature> creatures)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterEnergyReset(Player player) => Task.CompletedTask;

  public virtual Task AfterEnergyResetLate(Player player) => Task.CompletedTask;

  public virtual Task AfterEnergySpent(CardModel card, int amount) => Task.CompletedTask;

  public virtual Task BeforeCardRemoved(CardModel card) => Task.CompletedTask;

  public virtual Task BeforeFlush(PlayerChoiceContext choiceContext, Player player)
  {
    return Task.CompletedTask;
  }

  public virtual Task BeforeFlushLate(PlayerChoiceContext choiceContext, Player player)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterFlush(
    PlayerChoiceContext choiceContext,
    Player player,
    IReadOnlyCollection<CardModel> flushedCards,
    IReadOnlyCollection<CardModel> retainedCards)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterGoldGained(Player player) => Task.CompletedTask;

  public virtual Task BeforeHandDraw(
    Player player,
    PlayerChoiceContext choiceContext,
    ICombatState combatState)
  {
    return Task.CompletedTask;
  }

  public virtual Task BeforeHandDrawLate(
    Player player,
    PlayerChoiceContext choiceContext,
    ICombatState combatState)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterHandEmptied(PlayerChoiceContext choiceContext, Player player)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterItemPurchased(Player player, MerchantEntry itemPurchased, int goldSpent)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterMapGenerated(ActMap map, int actIndex) => Task.CompletedTask;

  public virtual Task AfterModifyingBlockAmount(
    Decimal modifiedAmount,
    CardModel? cardSource,
    CardPlay? cardPlay)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterModifyingCardPlayCount(CardModel card) => Task.CompletedTask;

  public virtual Task AfterModifyingCardPlayResultLocation(
    CardModel card,
    CardLocation cardLocation)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterModifyingOrbPassiveTriggerCount(OrbModel orb) => Task.CompletedTask;

  public virtual Task AfterModifyingCardRewardOptions() => Task.CompletedTask;

  public virtual Task AfterModifyingDamageAmount(CardModel? cardSource) => Task.CompletedTask;

  public virtual Task AfterModifyingEnergyGain() => Task.CompletedTask;

  public virtual Task AfterModifyingGoldGained(Player player, Decimal amount) => Task.CompletedTask;

  public virtual Task AfterModifyingHandDraw() => Task.CompletedTask;

  public virtual Task AfterPreventingDraw() => Task.CompletedTask;

  public virtual Task AfterModifyingHpLostBeforeOsty() => Task.CompletedTask;

  public virtual Task AfterModifyingHpLostAfterOsty() => Task.CompletedTask;

  public virtual Task AfterModifyingPowerAmountReceived(PowerModel power) => Task.CompletedTask;

  public virtual Task AfterModifyingPowerAmountGiven(PowerModel power) => Task.CompletedTask;

  public virtual Task AfterModifyingRewards() => Task.CompletedTask;

  public virtual Task AfterOrbChanneled(
    PlayerChoiceContext choiceContext,
    Player player,
    OrbModel orb)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterOrbEvoked(
    PlayerChoiceContext choiceContext,
    OrbModel orb,
    IEnumerable<Creature> targets)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterOstyRevived(Creature osty) => Task.CompletedTask;

  public virtual Task BeforePotionUsed(PotionModel potion, Creature? target) => Task.CompletedTask;

  public virtual Task AfterPotionUsed(PotionModel potion, Creature? target) => Task.CompletedTask;

  public virtual Task AfterPotionDiscarded(PotionModel potion) => Task.CompletedTask;

  public virtual Task AfterPotionProcured(PotionModel potion) => Task.CompletedTask;

  public virtual Task BeforePowerAmountChanged(
    PowerModel power,
    Decimal amount,
    Creature target,
    Creature? applier,
    CardModel? cardSource)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterPowerAmountChanged(
    PlayerChoiceContext choiceContext,
    PowerModel power,
    Decimal amount,
    Creature? applier,
    CardModel? cardSource)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterPreventingBlockClear(AbstractModel preventer, Creature creature)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterPreventingDeath(Creature creature) => Task.CompletedTask;

  public virtual Task AfterRestSiteHeal(Player player, bool isMimicked) => Task.CompletedTask;

  public virtual Task AfterRestSiteSmith(Player player) => Task.CompletedTask;

  public virtual Task AfterRewardTaken(Player player, Reward reward) => Task.CompletedTask;

  public virtual Task BeforeRoomEntered(AbstractRoom room) => Task.CompletedTask;

  public virtual Task AfterRoomEntered(AbstractRoom room) => Task.CompletedTask;

  public virtual Task AfterShuffle(PlayerChoiceContext choiceContext, Player shuffler)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterStarsSpent(int amount, Player spender) => Task.CompletedTask;

  public virtual Task AfterStarsGained(int amount, Player gainer) => Task.CompletedTask;

  public virtual Task AfterForge(Decimal amount, Player forger, AbstractModel? source)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterSummon(
    PlayerChoiceContext choiceContext,
    Player summoner,
    Decimal amount)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterTakingExtraTurn(Player player) => Task.CompletedTask;

  public virtual Task AfterTargetingBlockedVfx(Creature blocker) => Task.CompletedTask;

  public virtual Task BeforeSideTurnStart(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IReadOnlyList<Creature> participants,
    ICombatState combatState)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterSideTurnStart(
    CombatSide side,
    IReadOnlyList<Creature> participants,
    ICombatState combatState)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterSideTurnStartLate(
    CombatSide side,
    IReadOnlyList<Creature> participants,
    ICombatState combatState)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterPlayerTurnStartEarly(PlayerChoiceContext choiceContext, Player player)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterPlayerTurnStartLate(PlayerChoiceContext choiceContext, Player player)
  {
    return Task.CompletedTask;
  }

  public virtual Task BeforeSideTurnEndVeryEarly(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    return Task.CompletedTask;
  }

  public virtual Task BeforeSideTurnEndEarly(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    return Task.CompletedTask;
  }

  public virtual Task BeforeSideTurnEnd(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterSideTurnEnd(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterSideTurnEndLate(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    return Task.CompletedTask;
  }

  public virtual int ModifyAttackHitCount(AttackCommand attack, int hitCount) => hitCount;

  public virtual Decimal ModifyBlockAdditive(
    Creature target,
    Decimal block,
    ValueProp props,
    CardModel? cardSource,
    CardPlay? cardPlay)
  {
    return 0M;
  }

  public virtual Decimal ModifyBlockMultiplicative(
    Creature target,
    Decimal block,
    ValueProp props,
    CardModel? cardSource,
    CardPlay? cardPlay)
  {
    return 1M;
  }

  public virtual int ModifyCardPlayCount(CardModel card, Creature? target, int playCount)
  {
    return playCount;
  }

  public virtual CardLocation ModifyCardPlayResultLocation(
    CardModel card,
    bool isAutoPlay,
    ResourceInfo resources,
    CardLocation cardLocation)
  {
    return cardLocation;
  }

  public virtual int ModifyOrbPassiveTriggerCounts(OrbModel orb, int triggerCount) => triggerCount;

  public virtual CardCreationOptions ModifyCardRewardCreationOptions(
    Player player,
    CardCreationOptions options)
  {
    return options;
  }

  public virtual CardCreationOptions ModifyCardRewardCreationOptionsLate(
    Player player,
    CardCreationOptions options)
  {
    return options;
  }

  public virtual Decimal ModifyCardRewardUpgradeOdds(Player player, CardModel card, Decimal odds)
  {
    return odds;
  }

  public virtual Decimal ModifyDamageAdditive(
    Creature? target,
    Decimal amount,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource,
    CardPlay? cardPlay)
  {
    return 0M;
  }

  public virtual Decimal ModifyDamageCap(
    Creature? target,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource,
    CardPlay? cardPlay)
  {
    return Decimal.MaxValue;
  }

  public virtual Decimal ModifyDamageMultiplicative(
    Creature? target,
    Decimal amount,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource,
    CardPlay? cardPlay)
  {
    return 1M;
  }

  public virtual Decimal ModifyEnergyGain(Player player, Decimal amount) => amount;

  public virtual Decimal ModifyGoldGained(Player player, Decimal amount) => amount;

  public virtual ActMap ModifyGeneratedMap(IRunState runState, ActMap map, int actIndex) => map;

  public virtual ActMap ModifyGeneratedMapLate(IRunState runState, ActMap map, int actIndex) => map;

  public virtual Decimal ModifyHandDraw(Player player, Decimal count) => count;

  public virtual Decimal ModifyHandDrawLate(Player player, Decimal count) => count;

  public virtual Decimal ModifyHpLostBeforeOsty(
    Creature target,
    Decimal amount,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource)
  {
    return amount;
  }

  public virtual Decimal ModifyHpLostBeforeOstyLate(
    Creature target,
    Decimal amount,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource)
  {
    return amount;
  }

  public virtual Decimal ModifyHpLostAfterOsty(
    Creature target,
    Decimal amount,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource)
  {
    return amount;
  }

  public virtual Decimal ModifyHpLostAfterOstyLate(
    Creature target,
    Decimal amount,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource)
  {
    return amount;
  }

  public virtual Decimal ModifyMaxEnergy(Player player, Decimal amount) => amount;

  public virtual IEnumerable<CardModel> ModifyMerchantCardPool(
    Player player,
    IEnumerable<CardModel> options)
  {
    return options;
  }

  public virtual CardRarity ModifyMerchantCardRarity(Player player, CardRarity rarity) => rarity;

  public virtual void ModifyMerchantCardCreationResults(
    Player player,
    List<CardCreationResult> cards)
  {
  }

  public virtual Decimal ModifyMerchantPrice(Player player, MerchantEntry entry, Decimal cost)
  {
    return cost;
  }

  public virtual Decimal ModifyOrbValue(OrbModel orb, Decimal value) => value;

  public virtual Decimal ModifyPowerAmountGivenAdditive(
    PowerModel power,
    Creature giver,
    Decimal amount,
    Creature? target,
    CardModel? cardSource)
  {
    return 0M;
  }

  public virtual Decimal ModifyPowerAmountGivenMultiplicative(
    PowerModel power,
    Creature giver,
    Decimal amount,
    Creature? target,
    CardModel? cardSource)
  {
    return 1M;
  }

  public virtual Decimal ModifyRestSiteHealAmount(Creature creature, Decimal amount) => amount;

  public virtual void ModifyShuffleOrder(
    Player player,
    List<CardModel> cards,
    bool isInitialShuffle)
  {
  }

  public virtual Decimal ModifySummonAmount(Player summoner, Decimal amount, AbstractModel? source)
  {
    return amount;
  }

  public virtual Creature ModifyUnblockedDamageTarget(
    Creature target,
    Decimal amount,
    ValueProp props,
    Creature? dealer)
  {
    return target;
  }

  public virtual EventModel ModifyNextEvent(EventModel currentEvent) => currentEvent;

  public virtual IReadOnlySet<RoomType> ModifyUnknownMapPointRoomTypes(
    IReadOnlySet<RoomType> roomTypes)
  {
    return roomTypes;
  }

  public virtual float ModifyOddsIncreaseForUnrolledRoomType(RoomType roomType, float oddsIncrease)
  {
    return oddsIncrease;
  }

  public virtual int ModifyXValue(CardModel card, int originalValue) => originalValue;

  public virtual bool TryModifyCardBeingAddedToDeck(CardModel card, out CardModel? newCard)
  {
    newCard = (CardModel) null;
    return false;
  }

  public virtual bool TryModifyCardBeingAddedToDeckLate(CardModel card, out CardModel? newCard)
  {
    newCard = (CardModel) null;
    return false;
  }

  public virtual bool TryModifyCardRewardAlternatives(
    Player player,
    CardReward cardReward,
    List<CardRewardAlternative> alternatives)
  {
    return false;
  }

  public virtual bool TryModifyCardRewardOptions(
    Player player,
    List<CardCreationResult> cardRewardOptions,
    CardCreationOptions creationOptions)
  {
    return false;
  }

  public virtual bool TryModifyCardRewardOptionsLate(
    Player player,
    List<CardCreationResult> cardRewardOptions,
    CardCreationOptions creationOptions)
  {
    return false;
  }

  public virtual bool TryModifyEnergyCostInCombat(
    CardModel card,
    Decimal originalCost,
    out Decimal modifiedCost)
  {
    modifiedCost = originalCost;
    return false;
  }

  public virtual bool TryModifyEnergyCostInCombatLate(
    CardModel card,
    Decimal originalCost,
    out Decimal modifiedCost)
  {
    modifiedCost = originalCost;
    return false;
  }

  public virtual bool TryModifyKeywordsInCombat(CardModel card, ISet<CardKeyword> keywords)
  {
    return false;
  }

  public virtual bool TryModifyStarCost(
    CardModel card,
    Decimal originalCost,
    out Decimal modifiedCost)
  {
    modifiedCost = originalCost;
    return false;
  }

  public virtual bool TryModifyPowerAmountReceived(
    PowerModel canonicalPower,
    Creature target,
    Decimal amount,
    Creature? applier,
    out Decimal modifiedAmount)
  {
    modifiedAmount = amount;
    return false;
  }

  public virtual bool TryModifyRestSiteOptions(Player player, ICollection<RestSiteOption> options)
  {
    return false;
  }

  public virtual bool TryModifyRestSiteHealRewards(
    Player player,
    List<Reward> rewards,
    bool isMimicked)
  {
    return false;
  }

  public virtual bool TryModifyRewards(Player player, List<Reward> rewards, AbstractRoom? room)
  {
    return false;
  }

  public virtual bool TryModifyRewardsLate(Player player, List<Reward> rewards, AbstractRoom? room)
  {
    return false;
  }

  public virtual IReadOnlyList<LocString> ModifyExtraRestSiteHealText(
    Player player,
    IReadOnlyList<LocString> currentExtraText)
  {
    return currentExtraText;
  }

  public virtual bool ShouldAddToDeck(CardModel card) => true;

  public virtual bool ShouldAfflict(CardModel card, AfflictionModel affliction) => true;

  public virtual bool ShouldAllowAncient(Player player, AncientEventModel ancient) => true;

  public virtual bool ShouldAllowHitting(Creature creature) => true;

  public virtual bool ShouldAllowTargeting(Creature target) => true;

  public virtual bool ShouldAllowSelectingMoreCardRewards(Player player, CardReward cardReward)
  {
    return false;
  }

  public virtual bool ShouldClearBlock(Creature creature) => true;

  public virtual bool ShouldDie(Creature creature) => true;

  public virtual bool ShouldDieLate(Creature creature) => true;

  public virtual bool ShouldDisableRemainingRestSiteOptions(Player player) => true;

  public virtual bool ShouldDraw(Player player, bool fromHandDraw) => true;

  public virtual bool ShouldEtherealTrigger(CardModel card) => true;

  public virtual bool ShouldFlush(Player player) => true;

  public virtual bool ShouldGainStars(Decimal amount, Player player) => true;

  public virtual bool ShouldGenerateTreasure(Player player) => true;

  public virtual bool ShouldPayExcessEnergyCostWithStars(Player player) => false;

  public virtual bool ShouldPlay(CardModel card, AutoPlayType autoPlayType) => true;

  public virtual bool ShouldPlayerResetEnergy(Player player) => true;

  public virtual bool ShouldProceedToNextMapPoint() => true;

  public virtual bool ShouldProcurePotion(PotionModel potion, Player player) => true;

  public virtual bool ShouldPowerBeRemovedOnDeath(PowerModel power) => true;

  public virtual bool ShouldRefillMerchantEntry(MerchantEntry entry, Player player) => false;

  public virtual bool ShouldAllowMerchantCardRemoval(Player player) => true;

  public virtual bool ShouldCreatureBeRemovedFromCombatAfterDeath(Creature creature) => true;

  public virtual bool ShouldStopCombatFromEnding() => false;

  public virtual bool ShouldTakeExtraTurn(Player player) => false;

  public virtual bool ShouldForcePotionReward(Player player, RoomType roomType) => false;

  public virtual bool ShouldAllowFreeTravel() => false;

  public override string ToString() => $"{this.Id} ({RuntimeHelpers.GetHashCode((object) this)})";

  protected void NeverEverCallThisOutsideOfTests_SetIsMutable(bool isMutable)
  {
    if (TestMode.IsOff)
      throw new InvalidOperationException("You monster!");
    this.IsMutable = isMutable;
  }
}
