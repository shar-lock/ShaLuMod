// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.Mocks.MockScaleInMultiplayerPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers.Mocks;

public class MockScaleInMultiplayerPower : PowerModel
{
  public bool shouldScaleInMultiplayer = true;
  public static MockScaleInMultiplayerPower.GetScaledAmountForMultiplayerDelegate? getScaledAmountForMultiplayer;

  public override bool IsMock => true;

  public override PowerType Type => PowerType.Debuff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override bool AllowNegative => true;

  public override bool ShouldScaleInMultiplayer => this.shouldScaleInMultiplayer;

  public override Decimal GetScaledAmountForMultiplayer(
    ICombatState combatState,
    Creature? applier,
    Decimal amount,
    Creature target,
    CardModel? cardSource)
  {
    return MockScaleInMultiplayerPower.getScaledAmountForMultiplayer != null ? MockScaleInMultiplayerPower.getScaledAmountForMultiplayer(combatState, applier, amount, target, cardSource) : base.GetScaledAmountForMultiplayer(combatState, applier, amount, target, cardSource);
  }

  public delegate Decimal GetScaledAmountForMultiplayerDelegate(
    ICombatState combatState,
    Creature? applier,
    Decimal amount,
    Creature target,
    CardModel? cardSource);
}
