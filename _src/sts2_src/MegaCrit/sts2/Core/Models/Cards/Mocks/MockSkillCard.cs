// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Mocks.MockSkillCard
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards.Mocks;

public sealed class MockSkillCard : MockCardModel
{
  private const string _drawKey = "Draw";
  private const string _discardKey = "Discard";
  private int _blockCount = 1;
  private TargetType _targetType = TargetType.Self;
  private List<MockSkillCard.PowerApplication> _powerApplications = new List<MockSkillCard.PowerApplication>();
  private List<MockSkillCard.CardCreation> _cardCreations = new List<MockSkillCard.CardCreation>();

  public override CardType Type => CardType.Skill;

  public override TargetType TargetType => this._targetType;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[7]
      {
        (DynamicVar) new BlockVar(5M, ValueProp.Move),
        (DynamicVar) new CardsVar("Draw", 0),
        (DynamicVar) new CardsVar("Discard", 0),
        (DynamicVar) new EnergyVar(0),
        (DynamicVar) new ForgeVar(0),
        (DynamicVar) new StarsVar(0),
        (DynamicVar) new SummonVar(0M)
      });
    }
  }

  protected override void DeepCloneFields()
  {
    base.DeepCloneFields();
    this._powerApplications = this._powerApplications.ToList<MockSkillCard.PowerApplication>();
    this._cardCreations = this._cardCreations.ToList<MockSkillCard.CardCreation>();
  }

  [PreserveBaseOverrides]
  MockSkillCard MockCardModel.MockBlock(int block)
  {
    this.AssertMutable();
    this.DynamicVars.Block.BaseValue = (Decimal) block;
    return this;
  }

  public MockSkillCard MockBlockCount(int blockCount)
  {
    this.AssertMutable();
    this._blockCount = blockCount;
    return this;
  }

  public MockSkillCard MockDraw(int cards)
  {
    this.AssertMutable();
    this.DynamicVars["Draw"].BaseValue = (Decimal) cards;
    return this;
  }

  public MockSkillCard MockSummon(int summons)
  {
    this.AssertMutable();
    this.DynamicVars.Summon.BaseValue = (Decimal) summons;
    return this;
  }

  public MockSkillCard MockDiscard(int cards)
  {
    this.AssertMutable();
    this.DynamicVars["Discard"].BaseValue = (Decimal) cards;
    return this;
  }

  public MockSkillCard MockForge(Decimal forge)
  {
    this.AssertMutable();
    this.DynamicVars.Forge.BaseValue = forge;
    return this;
  }

  public MockSkillCard MockStarGain(Decimal stars)
  {
    this.AssertMutable();
    this.DynamicVars.Stars.BaseValue = stars;
    return this;
  }

  public MockSkillCard MockEnergyGain(Decimal energy)
  {
    this.AssertMutable();
    this.DynamicVars.Energy.BaseValue = energy;
    return this;
  }

  public MockSkillCard MockPower<TPower>(int amount, TargetType targetType) where TPower : PowerModel
  {
    this.AssertMutable();
    if (this._powerApplications.Any<MockSkillCard.PowerApplication>((Func<MockSkillCard.PowerApplication, bool>) (a => a.targetType != targetType)))
      throw new InvalidOperationException("Cannot have multiple power applications with different target types.");
    this._targetType = targetType;
    this._powerApplications.Add(new MockSkillCard.PowerApplication()
    {
      powerType = typeof (TPower),
      amount = amount,
      targetType = targetType
    });
    return this;
  }

  public MockSkillCard MockCreateCards<TCard>(int amount) where TCard : CardModel
  {
    this.AssertMutable();
    this._cardCreations.Add(new MockSkillCard.CardCreation()
    {
      canonicalCard = (CardModel) ModelDb.Card<TCard>(),
      amount = amount
    });
    return this;
  }

  protected override int GetBaseBlock() => this.DynamicVars.Block.IntValue;

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (this._mockSelfHpLoss > 0)
    {
      IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, this.Owner.Creature, (Decimal) this._mockSelfHpLoss, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, (CardModel) this, cardPlay);
    }
    for (int i = 0; i < this._blockCount; ++i)
    {
      Decimal num = await CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars.Block, cardPlay);
    }
    if (this.DynamicVars["Draw"].IntValue > 0)
    {
      IEnumerable<CardModel> cardModels = await CardPileCmd.Draw(choiceContext, (Decimal) this.DynamicVars["Draw"].IntValue, this.Owner);
    }
    if (this.DynamicVars["Discard"].IntValue > 0)
    {
      CardSelectorPrefs prefs = new CardSelectorPrefs(new LocString("cards", "MOCK_SKILL_CARD.discardSelectionPrompt"), this.DynamicVars["Discard"].IntValue);
      await CardCmd.Discard(choiceContext, await CardSelectCmd.FromHandForDiscard(choiceContext, this.Owner, prefs, (Func<CardModel, bool>) null, (AbstractModel) this));
    }
    if (this.DynamicVars.Forge.BaseValue > 0M)
    {
      IEnumerable<SovereignBlade> sovereignBlades = await ForgeCmd.Forge(this.DynamicVars.Forge.BaseValue, this.Owner, (AbstractModel) this);
    }
    if (this.DynamicVars.Stars.BaseValue > 0M)
      await PlayerCmd.GainStars(this.DynamicVars.Stars.BaseValue, this.Owner);
    if (this.DynamicVars.Summon.BaseValue > 0M)
    {
      SummonResult summonResult = await OstyCmd.Summon(choiceContext, this.Owner, this.DynamicVars.Summon.BaseValue, (AbstractModel) this);
    }
    if (this.DynamicVars.Energy.BaseValue > 0M)
      await PlayerCmd.GainEnergy(this.DynamicVars.Energy.BaseValue, this.Owner);
    foreach (MockSkillCard.PowerApplication powerApplication in this._powerApplications)
    {
      MockSkillCard.PowerApplication application = powerApplication;
      IReadOnlyList<Creature> creatureList;
      switch (application.targetType)
      {
        case TargetType.Self:
          // ISSUE: object of a compiler-generated type is created
          creatureList = (IReadOnlyList<Creature>) new \u003C\u003Ez__ReadOnlySingleElementList<Creature>(this.Owner.Creature);
          break;
        case TargetType.AnyEnemy:
          // ISSUE: object of a compiler-generated type is created
          creatureList = (IReadOnlyList<Creature>) new \u003C\u003Ez__ReadOnlySingleElementList<Creature>(cardPlay.Target);
          break;
        case TargetType.AllEnemies:
          creatureList = this.CombatState.Enemies;
          break;
        default:
          throw new ArgumentOutOfRangeException("targetType", (object) application.targetType, (string) null);
      }
      foreach (Creature target in (IEnumerable<Creature>) creatureList)
        await PowerCmd.Apply(choiceContext, ModelDb.GetById<PowerModel>(ModelDb.GetId(application.powerType)).ToMutable(), target, (Decimal) application.amount, this.Owner.Creature, (CardModel) this);
      application = new MockSkillCard.PowerApplication();
    }
    foreach (MockSkillCard.CardCreation cardCreation in this._cardCreations)
    {
      switch (cardCreation.canonicalCard)
      {
        case Shiv _:
          IEnumerable<CardModel> inHand1 = await Shiv.CreateInHand(this.Owner, cardCreation.amount, this.CombatState);
          continue;
        case Soul _:
          IEnumerable<Soul> inHand2 = await Soul.CreateInHand(this.Owner, cardCreation.amount, this.CombatState);
          continue;
        default:
          List<CardModel> cards = new List<CardModel>();
          for (int index = 0; index < cardCreation.amount; ++index)
            cards.Add(this.CombatState.CreateCard(cardCreation.canonicalCard, this.Owner));
          IReadOnlyList<CardPileAddResult> combat = await CardPileCmd.AddGeneratedCardsToCombat((IEnumerable<CardModel>) cards, PileType.Hand, this.Owner);
          continue;
      }
    }
    if (this._mockExtraLogic == null)
      return;
    await this._mockExtraLogic((CardModel) this);
  }

  protected override void OnUpgrade()
  {
    if (this._mockUpgradeLogic != null)
      this._mockUpgradeLogic((CardModel) this);
    else
      this.DynamicVars.Block.UpgradeValueBy(3M);
  }

  private struct PowerApplication
  {
    public System.Type powerType;
    public int amount;
    public TargetType targetType;
  }

  private struct CardCreation
  {
    public CardModel canonicalCard;
    public int amount;
  }
}
