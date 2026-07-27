// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Badges.DebufferModel
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Badges;

public class DebufferModel : BadgeModel
{
  public override Task AfterPowerAmountChanged(
    PlayerChoiceContext choiceContext,
    PowerModel power,
    Decimal amount,
    Creature? applier,
    CardModel? cardSource)
  {
    if (power.GetTypeForAmount(amount) == PowerType.Debuff && !(power is ITemporaryPower) && applier != null && applier.Player != null && applier != power.Owner)
      ++applier.Player.ExtraFields.DebuffsApplied;
    return Task.CompletedTask;
  }
}
