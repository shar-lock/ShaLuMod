// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Achievements.SkillIronclad1Achievement
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Rooms;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Achievements;

public class SkillIronclad1Achievement : AchievementModel
{
  private const int _exhaustRequirement = 20;
  private int _cardsExhaustedThisCombat;

  public override Task AfterCardExhausted(
    PlayerChoiceContext choiceContext,
    CardModel card,
    bool causedByEthereal)
  {
    if (!LocalContext.IsMine(card))
      return Task.CompletedTask;
    ++this._cardsExhaustedThisCombat;
    if (this._cardsExhaustedThisCombat >= 20)
      AchievementsUtil.Unlock(Achievement.CharacterSkillIronclad1, card.Owner);
    return Task.CompletedTask;
  }

  public override Task AfterRoomEntered(AbstractRoom room)
  {
    this._cardsExhaustedThisCombat = 0;
    return Task.CompletedTask;
  }
}
