// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.RazorTooth
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class RazorTooth : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Rare;

  public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (cardPlay.Card.Owner != this.Owner)
      return Task.CompletedTask;
    bool flag;
    switch (cardPlay.Card.Type)
    {
      case CardType.Attack:
      case CardType.Skill:
        flag = true;
        break;
      default:
        flag = false;
        break;
    }
    if (!flag || !cardPlay.Card.IsUpgradable)
      return Task.CompletedTask;
    CardCmd.Upgrade(cardPlay.Card);
    return Task.CompletedTask;
  }
}
