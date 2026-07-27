// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.ForegoneConclusionPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class ForegoneConclusionPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override async Task BeforeHandDraw(
    Player player,
    PlayerChoiceContext choiceContext,
    ICombatState combatState)
  {
    if (player != this.Owner.Player)
      return;
    await CardPileCmd.ShuffleIfNecessary(choiceContext, this.Owner.Player);
    IReadOnlyList<CardPileAddResult> cardPileAddResultList = await CardPileCmd.Add(await CardSelectCmd.FromCombatPile(choiceContext, PileType.Draw.GetPile(this.Owner.Player), this.Owner.Player, new CardSelectorPrefs(this.SelectionScreenPrompt, this.Amount)), PileType.Hand);
    await PowerCmd.Remove((PowerModel) this);
  }
}
