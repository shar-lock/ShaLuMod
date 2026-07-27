// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Omnislice
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

public sealed class Omnislice : CardModel
{
  public Omnislice()
    : base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
  {
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new DamageVar(8M, ValueProp.Move));
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    AttackContext context = await AttackCommand.CreateContextAsync(this.CombatState, choiceContext, cardPlay);
    object obj = (object) null;
    int num = 0;
    try
    {
      List<DamageResult> list1 = (await CreatureCmd.Damage(choiceContext, cardPlay.Target, this.DynamicVars.Damage.BaseValue, ValueProp.Move, (CardModel) this, cardPlay)).ToList<DamageResult>();
      context.AddHit((IEnumerable<DamageResult>) list1);
      DamageResult damageResult = list1.FirstOrDefault<DamageResult>();
      if (damageResult != null)
      {
        // ISSUE: object of a compiler-generated type is created
        List<Creature> list2 = this.CombatState.GetTeammatesOf(damageResult.Receiver).Except<Creature>((IEnumerable<Creature>) new \u003C\u003Ez__ReadOnlySingleElementList<Creature>(cardPlay.Target)).Where<Creature>((Func<Creature, bool>) (e => e.IsHittable)).ToList<Creature>();
        if (list2.Count != 0)
        {
          AttackContext attackContext = context;
          attackContext.AddHit(await CreatureCmd.Damage(choiceContext, (IEnumerable<Creature>) list2, (Decimal) (damageResult.TotalDamage + damageResult.OverkillDamage), ValueProp.Unpowered | ValueProp.Move, this.Owner.Creature, (CardModel) this, cardPlay));
          attackContext = (AttackContext) null;
        }
      }
      num = 1;
    }
    catch (object ex)
    {
      obj = ex;
    }
    if (context != null)
      await context.DisposeAsync();
    object obj1 = obj;
    if (obj1 != null)
    {
      if (!(obj1 is Exception source))
        throw obj1;
      ExceptionDispatchInfo.Capture(source).Throw();
    }
    if (num == 1)
    {
      context = (AttackContext) null;
    }
    else
    {
      obj = (object) null;
      context = (AttackContext) null;
      context = (AttackContext) null;
    }
  }

  protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(3M);
}
