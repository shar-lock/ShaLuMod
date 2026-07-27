// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Achievements.SkillNecrobinder2Achievement
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Platform;
using System;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Achievements;

public class SkillNecrobinder2Achievement : AchievementModel
{
  private const int _strengthThreshold = 50;

  public override Task AfterPowerAmountChanged(
    PlayerChoiceContext choiceContext,
    PowerModel power,
    Decimal amount,
    Creature? applier,
    CardModel? cardSource)
  {
    if (!LocalContext.IsMe(applier) || !(power is StrengthPower) || !(power.Owner.Monster is Osty) || power.Amount < 50)
      return Task.CompletedTask;
    AchievementsUtil.Unlock(Achievement.CharacterSkillNecrobinder2, applier.Player);
    return Task.CompletedTask;
  }
}
