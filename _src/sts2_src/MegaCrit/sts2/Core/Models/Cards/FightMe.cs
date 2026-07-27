// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.FightMe
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class FightMe : CardModel
{
  private const string _enemyStrengthKey = "EnemyStrength";

  public FightMe()
    : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
  {
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>());
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[4]
      {
        (DynamicVar) new DamageVar(5M, ValueProp.Move),
        (DynamicVar) new RepeatVar(2),
        (DynamicVar) new PowerVar<StrengthPower>(3M),
        new DynamicVar("EnemyStrength", 1M)
      });
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).WithHitCount(this.DynamicVars.Repeat.IntValue).FromCard((CardModel) this, cardPlay).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "heavy_attack.mp3").Execute(choiceContext);
    StrengthPower strengthPower1 = await PowerCmd.Apply<StrengthPower>(choiceContext, this.Owner.Creature, this.DynamicVars["StrengthPower"].BaseValue, this.Owner.Creature, (CardModel) this);
    StrengthPower strengthPower2 = await PowerCmd.Apply<StrengthPower>(choiceContext, cardPlay.Target, this.DynamicVars["EnemyStrength"].BaseValue, this.Owner.Creature, (CardModel) this);
  }

  protected override void OnUpgrade()
  {
    this.DynamicVars.Damage.UpgradeValueBy(1M);
    this.DynamicVars.Strength.UpgradeValueBy(1M);
  }
}
