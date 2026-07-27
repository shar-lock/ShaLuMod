// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Achievements.SkillIronclad2Achievement
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.ValueProps;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Achievements;

public class SkillIronclad2Achievement : AchievementModel
{
  private const int _damageRequirement = 999;

  public override Task AfterDamageGiven(
    PlayerChoiceContext choiceContext,
    Creature? dealer,
    DamageResult result,
    ValueProp props,
    Creature target,
    CardModel? cardSource)
  {
    if (!LocalContext.IsMe(dealer) || result.UnblockedDamage < 999)
      return Task.CompletedTask;
    AchievementsUtil.Unlock(Achievement.CharacterSkillIronclad2, dealer.Player);
    return Task.CompletedTask;
  }
}
