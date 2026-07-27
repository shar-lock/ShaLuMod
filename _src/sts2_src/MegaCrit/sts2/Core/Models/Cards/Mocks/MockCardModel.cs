// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Mocks.MockCardModel
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models.CardPools;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards.Mocks;

public abstract class MockCardModel : CardModel
{
  protected int _mockEnergyCost = 1;
  protected CardMultiplayerConstraint _mockMultiplayerConstraint;
  protected bool _mockEnergyCostX;
  protected int _mockStarCost;
  protected bool _mockStarCostX;
  protected Func<CardModel, Task>? _mockExtraLogic;
  protected int _mockMaxUpgradeLevel = 1;
  protected CardRarity _mockRarity = CardRarity.Common;
  protected int _mockSelfHpLoss;
  protected HashSet<CardTag>? _mockTags;
  protected CardPoolModel? _mockPool;
  protected Action<CardModel>? _mockUpgradeLogic;

  protected MockCardModel()
    : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
  {
  }

  public override bool IsMock => true;

  protected override int CanonicalEnergyCost => this._mockEnergyCost;

  protected override bool HasEnergyCostX => this._mockEnergyCostX;

  public override int CanonicalStarCost => this._mockStarCost;

  public override bool HasStarCostX => this._mockStarCostX;

  public override CardMultiplayerConstraint MultiplayerConstraint
  {
    get => this._mockMultiplayerConstraint;
  }

  public override bool GainsBlock => this.GetBaseBlock() > 0;

  public override int MaxUpgradeLevel => this._mockMaxUpgradeLevel;

  public override CardRarity Rarity => this._mockRarity;

  public override CardPoolModel Pool
  {
    get => this._mockPool ?? (CardPoolModel) ModelDb.CardPool<DeprivedCardPool>();
  }

  public override IEnumerable<CardTag> Tags
  {
    get => (IEnumerable<CardTag>) this._mockTags ?? (IEnumerable<CardTag>) new HashSet<CardTag>();
  }

  public abstract MockCardModel MockBlock(int block);

  public MockCardModel MockCanonical()
  {
    this.AssertMutable();
    this.CombatState?.RemoveCard((CardModel) this);
    this.NeverEverCallThisOutsideOfTests_ClearOwner();
    this.NeverEverCallThisOutsideOfTests_SetIsMutable(false);
    return this;
  }

  public MockCardModel MockEnergyCost(int cost)
  {
    this.AssertMutable();
    this._mockEnergyCost = cost;
    this._mockEnergyCostX = false;
    this.MockSetEnergyCost(new CardEnergyCost((CardModel) this, cost, false));
    return this;
  }

  public MockCardModel MockMultiplayerType(CardMultiplayerConstraint constraint)
  {
    this.AssertMutable();
    this._mockMultiplayerConstraint = constraint;
    return this;
  }

  public MockCardModel MockEnergyCostX()
  {
    this.AssertMutable();
    this._mockEnergyCostX = true;
    this._mockEnergyCost = 0;
    this.MockSetEnergyCost(new CardEnergyCost((CardModel) this, 0, true));
    return this;
  }

  public MockCardModel MockStarCost(int cost)
  {
    this.AssertMutable();
    this._mockStarCost = cost;
    this._mockStarCostX = false;
    return this;
  }

  public MockCardModel MockStarCostX()
  {
    this.AssertMutable();
    this._mockStarCostX = true;
    this._mockStarCost = 0;
    return this;
  }

  public MockCardModel MockExtraLogic(Func<CardModel, Task> extraLogic)
  {
    this.AssertMutable();
    this._mockExtraLogic = extraLogic;
    return this;
  }

  public MockCardModel MockKeyword(CardKeyword keyword)
  {
    this.AssertMutable();
    this.AddKeyword(keyword);
    return this;
  }

  public MockCardModel MockReplay(int count)
  {
    this.AssertMutable();
    this.BaseReplayCount = count;
    return this;
  }

  public MockCardModel MockRarity(CardRarity rarity)
  {
    this.AssertMutable();
    this._mockRarity = rarity;
    return this;
  }

  public MockCardModel MockPool<T>() where T : CardPoolModel
  {
    this.AssertMutable();
    this._mockPool = (CardPoolModel) ModelDb.CardPool<T>();
    return this;
  }

  public MockCardModel MockTag(CardTag tag)
  {
    this.AssertMutable();
    if (this._mockTags == null)
      this._mockTags = new HashSet<CardTag>();
    this._mockTags.Add(tag);
    return this;
  }

  public MockCardModel MockSelfHpLoss(int hpLoss)
  {
    this.AssertMutable();
    this._mockSelfHpLoss = hpLoss;
    return this;
  }

  public MockCardModel MockUnUpgradable()
  {
    this.AssertMutable();
    this._mockMaxUpgradeLevel = 0;
    return this;
  }

  public MockCardModel MockUpgradeLogic(Action<CardModel> upgradeLogic)
  {
    this.AssertMutable();
    this._mockUpgradeLogic = upgradeLogic;
    return this;
  }

  protected abstract int GetBaseBlock();
}
