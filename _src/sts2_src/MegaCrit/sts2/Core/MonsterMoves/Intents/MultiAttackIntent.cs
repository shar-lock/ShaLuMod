// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.MonsterMoves.Intents.MultiAttackIntent
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.MonsterMoves.Intents;

public class MultiAttackIntent : AttackIntent
{
  private readonly int _repeat;
  private readonly Func<int>? _repeatCalc;

  protected override LocString IntentLabelFormat => new LocString("intents", "FORMAT_DAMAGE_MULTI");

  public override int Repeats
  {
    get
    {
      Func<int> repeatCalc = this._repeatCalc;
      return repeatCalc == null ? this._repeat : repeatCalc();
    }
  }

  public MultiAttackIntent(int damage, int repeat)
  {
    this.DamageCalc = (Func<Decimal>) (() => (Decimal) damage);
    this._repeat = repeat;
  }

  public MultiAttackIntent(int damage, Func<int> repeatCalc)
  {
    this.DamageCalc = (Func<Decimal>) (() => (Decimal) damage);
    this._repeatCalc = repeatCalc;
  }

  public override int GetTotalDamage(IEnumerable<Creature> targets, Creature owner)
  {
    return this.GetSingleDamage(targets, owner) * this.Repeats;
  }

  public override LocString GetIntentLabel(IEnumerable<Creature> targets, Creature owner)
  {
    LocString intentLabelFormat = this.IntentLabelFormat;
    float singleDamage = (float) this.GetSingleDamage(targets, owner);
    intentLabelFormat.Add("Damage", (Decimal) (int) singleDamage);
    intentLabelFormat.Add("Repeat", (Decimal) this.Repeats);
    return intentLabelFormat;
  }
}
