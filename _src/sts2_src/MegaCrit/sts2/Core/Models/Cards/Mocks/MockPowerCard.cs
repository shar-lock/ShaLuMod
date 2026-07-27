// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Mocks.MockPowerCard
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards.Mocks;

public sealed class MockPowerCard : MockCardModel
{
  protected override int CanonicalEnergyCost => 1;

  public override CardType Type => CardType.Power;

  public override TargetType TargetType => TargetType.Self;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new PowerVar<StrengthPower>(2M),
        (DynamicVar) new BlockVar(0M, ValueProp.Move)
      });
    }
  }

  public override MockCardModel MockBlock(int block)
  {
    this.AssertMutable();
    this.DynamicVars.Block.BaseValue = (Decimal) block;
    return (MockCardModel) this;
  }

  protected override int GetBaseBlock() => this.DynamicVars.Block.IntValue;

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (this._mockSelfHpLoss > 0)
    {
      IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, this.Owner.Creature, (Decimal) this._mockSelfHpLoss, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, (CardModel) this, cardPlay);
    }
    if (this.DynamicVars.Block.BaseValue > 0M)
    {
      Decimal num = await CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars.Block, cardPlay);
    }
    Decimal baseValue = this.DynamicVars.Strength.BaseValue;
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>(choiceContext, this.Owner.Creature, baseValue, this.Owner.Creature, (CardModel) this);
    if (this._mockExtraLogic == null)
      return;
    await this._mockExtraLogic((CardModel) this);
  }

  protected override void OnUpgrade()
  {
    if (this._mockUpgradeLogic != null)
      this._mockUpgradeLogic((CardModel) this);
    else
      this.DynamicVars.Strength.UpgradeValueBy(1M);
  }
}
