// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.MonsterMoves.Intents.SingleAttackIntent
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.MonsterMoves.Intents;

public class SingleAttackIntent : AttackIntent
{
  public override int Repeats => 1;

  protected override LocString IntentLabelFormat
  {
    get => new LocString("intents", "FORMAT_DAMAGE_SINGLE");
  }

  public SingleAttackIntent(int damage)
  {
    this.DamageCalc = (Func<Decimal>) (() => (Decimal) damage);
  }

  public SingleAttackIntent(Func<Decimal> damageCalc) => this.DamageCalc = damageCalc;

  public override int GetTotalDamage(IEnumerable<Creature> targets, Creature owner)
  {
    return this.GetSingleDamage(targets, owner);
  }

  public override LocString GetIntentLabel(IEnumerable<Creature> targets, Creature owner)
  {
    LocString intentLabelFormat = this.IntentLabelFormat;
    float totalDamage = (float) this.GetTotalDamage(targets, owner);
    intentLabelFormat.Add("Damage", (Decimal) (int) totalDamage);
    return intentLabelFormat;
  }
}
