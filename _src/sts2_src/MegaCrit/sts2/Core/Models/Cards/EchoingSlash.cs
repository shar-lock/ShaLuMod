// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.EchoingSlash
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class EchoingSlash : CardModel
{
  public EchoingSlash()
    : base(1, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
  {
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new DamageVar(10M, ValueProp.Move));
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    AttackContext attackContext = await AttackCommand.CreateContextAsync(this.CombatState, choiceContext, cardPlay);
    object obj = (object) null;
    int num = 0;
    try
    {
      IEnumerable<DamageResult> damageResults;
      for (int attackCount = 1; attackCount > 0; attackCount += damageResults.Count<DamageResult>((Func<DamageResult, bool>) (r => r.WasTargetKilled)))
      {
        --attackCount;
        damageResults = await CreatureCmd.Damage(choiceContext, (IEnumerable<Creature>) this.CombatState?.HittableEnemies, this.DynamicVars.Damage, this.Owner.Creature, (CardModel) this, cardPlay);
        attackContext.AddHit(damageResults);
      }
      num = 1;
    }
    catch (object ex)
    {
      obj = ex;
    }
    if (attackContext != null)
      await attackContext.DisposeAsync();
    object obj1 = obj;
    if (obj1 != null)
    {
      if (!(obj1 is Exception source))
        throw obj1;
      ExceptionDispatchInfo.Capture(source).Throw();
    }
    if (num == 1)
    {
      attackContext = (AttackContext) null;
    }
    else
    {
      obj = (object) null;
      attackContext = (AttackContext) null;
      attackContext = (AttackContext) null;
    }
  }

  protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(3M);
}
