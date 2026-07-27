// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.Mocks.MockCloneCardsOnPlayPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers.Mocks;

public sealed class MockCloneCardsOnPlayPower : PowerModel
{
  public override bool IsMock => true;

  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Single;

  public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (cardPlay.Card.Owner != this.Owner.Player)
      return;
    CardPileAddResult combat = await CardPileCmd.AddGeneratedCardToCombat(cardPlay.Card.CreateClone(), PileType.Hand, this.Owner.Player);
  }
}
