// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Misery
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Misery : CardModel
{
  public Misery()
    : base(0, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
  {
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new DamageVar(7M, ValueProp.Move));
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    Dictionary<PowerModel, int> debuffAmounts = Enumerable.ToDictionary<PowerModel, int>(cardPlay.Target.Powers.Where<PowerModel>((Func<PowerModel, bool>) (p => p.TypeForCurrentAmount == PowerType.Debuff)).Select<PowerModel, (PowerModel, int)>((Func<PowerModel, (PowerModel, int)>) (p => ((PowerModel) p.ClonePreservingMutability(), p.Amount))));
    foreach (KeyValuePair<PowerModel, int> keyValuePair1 in debuffAmounts)
    {
      ITemporaryPower temporaryPower = keyValuePair1.Key as ITemporaryPower;
      if (temporaryPower != null)
      {
        KeyValuePair<PowerModel, int> keyValuePair2 = debuffAmounts.FirstOrDefault<KeyValuePair<PowerModel, int>>((Func<KeyValuePair<PowerModel, int>, bool>) (p => p.Key.Id == temporaryPower.InternallyAppliedPower.Id));
        if (keyValuePair2.Key != null)
          debuffAmounts[keyValuePair2.Key] += keyValuePair1.Value;
      }
    }
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this, cardPlay).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
    foreach (Creature hittableEnemy in (IEnumerable<Creature>) this.CombatState.HittableEnemies)
    {
      Creature enemy = hittableEnemy;
      if (enemy != cardPlay.Target)
      {
        foreach (KeyValuePair<PowerModel, int> keyValuePair in debuffAmounts)
        {
          if (keyValuePair.Value != 0)
          {
            PowerModel instanceForStacking = PowerCmd.FindExistingInstanceForStacking(keyValuePair.Key, enemy, keyValuePair.Key.Applier);
            if (instanceForStacking != null)
            {
              int num = await PowerCmd.ModifyAmount(choiceContext, instanceForStacking, (Decimal) keyValuePair.Value, keyValuePair.Key.Applier, (CardModel) this);
            }
            else
              await PowerCmd.Apply(choiceContext, (PowerModel) keyValuePair.Key.ClonePreservingMutability(), enemy, (Decimal) keyValuePair.Value, keyValuePair.Key.Applier, (CardModel) this);
          }
        }
        enemy = (Creature) null;
      }
    }
    debuffAmounts = (Dictionary<PowerModel, int>) null;
  }

  protected override void OnUpgrade()
  {
    this.DynamicVars.Damage.UpgradeValueBy(2M);
    this.AddKeyword(CardKeyword.Retain);
  }
}
