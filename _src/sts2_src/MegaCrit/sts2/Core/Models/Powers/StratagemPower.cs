// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.StratagemPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class StratagemPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override async Task AfterShuffle(PlayerChoiceContext choiceContext, Player player)
  {
    if (player != this.Owner.Player)
      return;
    this.Flash();
    foreach (CardModel card in await CardSelectCmd.FromCombatPile(choiceContext, PileType.Draw.GetPile(this.Owner.Player), this.Owner.Player, new CardSelectorPrefs(this.SelectionScreenPrompt, this.Amount)))
    {
      CardPileAddResult cardPileAddResult = await CardPileCmd.Add(card, PileType.Hand);
    }
  }
}
