// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Achievements.SkillRegent1Achievement
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Platform;
using System;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Achievements;

public class SkillRegent1Achievement : AchievementModel
{
  private const int _damageThreshold = 999;

  public override Task AfterForge(Decimal amount, Player forger, AbstractModel? source)
  {
    if (!LocalContext.IsMe(forger) || AchievementsUtil.IsUnlocked(Achievement.CharacterSkillRegent1))
      return Task.CompletedTask;
    foreach (CardModel cardModel in forger.PlayerCombatState.AllCards.OfType<SovereignBlade>())
    {
      if (cardModel.DynamicVars.Damage.BaseValue >= 999M)
        AchievementsUtil.Unlock(Achievement.CharacterSkillRegent1, forger);
    }
    return Task.CompletedTask;
  }
}
