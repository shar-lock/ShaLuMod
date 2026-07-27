// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.ToolsOfTheTradePower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class ToolsOfTheTradePower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override Decimal ModifyHandDraw(Player player, Decimal count)
  {
    return player != this.Owner.Player ? count : count + (Decimal) this.Amount;
  }

  public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
  {
    if (player != this.Owner.Player)
      return;
    List<CardModel> list = (await CardSelectCmd.FromHandForDiscard(choiceContext, player, new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt, this.Amount), (Func<CardModel, bool>) null, (AbstractModel) this)).ToList<CardModel>();
    if (list.Count == 0)
      return;
    await CardCmd.Discard(choiceContext, (IEnumerable<CardModel>) list);
  }
}
