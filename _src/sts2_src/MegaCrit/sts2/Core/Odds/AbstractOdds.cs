// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Odds.AbstractOdds
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Random;

#nullable enable
namespace MegaCrit.Sts2.Core.Odds;

public abstract class AbstractOdds(float initialValue, Rng rng)
{
  protected readonly Rng _rng = rng;

  public float CurrentValue { get; protected set; } = initialValue;

  public void OverrideCurrentValue(float newValue) => this.CurrentValue = newValue;
}
