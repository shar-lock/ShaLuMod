// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Achievements.SkillRegent2Achievement
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Platform;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Achievements;

public class SkillRegent2Achievement : AchievementModel
{
  private const int _starThreshold = 20;

  public override Task AfterStarsGained(int amount, Player gainer)
  {
    if (!LocalContext.IsMe(gainer))
      return Task.CompletedTask;
    PlayerCombatState playerCombatState = gainer.PlayerCombatState;
    if ((playerCombatState != null ? (playerCombatState.Stars < 20 ? 1 : 0) : 0) != 0)
      return Task.CompletedTask;
    AchievementsUtil.Unlock(Achievement.CharacterSkillRegent2, gainer);
    return Task.CompletedTask;
  }
}
