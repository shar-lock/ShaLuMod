// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Achievements.SkillSilent1Achievement
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Rooms;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Achievements;

public class SkillSilent1Achievement : AchievementModel
{
  private CardModel? _firstCardOnStack;
  private int _slyCardsPlayed;

  public override Task BeforeCardPlayed(CardPlay cardPlay)
  {
    if (!LocalContext.IsMine(cardPlay.Card) || this._firstCardOnStack != null)
      return Task.CompletedTask;
    this._firstCardOnStack = cardPlay.Card;
    return Task.CompletedTask;
  }

  public override Task BeforeCardAutoPlayed(CardModel card, Creature? target, AutoPlayType type)
  {
    if (!LocalContext.IsMine(card) || type != AutoPlayType.SlyDiscard || this._firstCardOnStack == null)
      return Task.CompletedTask;
    ++this._slyCardsPlayed;
    if (this._slyCardsPlayed >= 5)
      AchievementsUtil.Unlock(Achievement.CharacterSkillSilent1, card.Owner);
    return Task.CompletedTask;
  }

  public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (!LocalContext.IsMine(cardPlay.Card) || cardPlay.Card != this._firstCardOnStack)
      return Task.CompletedTask;
    this._firstCardOnStack = (CardModel) null;
    this._slyCardsPlayed = 0;
    return Task.CompletedTask;
  }

  public override Task AfterRoomEntered(AbstractRoom room)
  {
    this._firstCardOnStack = (CardModel) null;
    this._slyCardsPlayed = 0;
    return Task.CompletedTask;
  }
}
