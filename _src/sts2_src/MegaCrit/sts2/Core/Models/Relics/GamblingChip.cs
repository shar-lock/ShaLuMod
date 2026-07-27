// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.GamblingChip
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class GamblingChip : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Rare;

  public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
  {
    if (player != this.Owner || this.Owner.PlayerCombatState.TurnNumber > 1)
      return;
    List<CardModel> list = (await CardSelectCmd.FromHandForDiscard(choiceContext, this.Owner, new CardSelectorPrefs(this.SelectionScreenPrompt, 0, 999999999), (Func<CardModel, bool>) null, (AbstractModel) this)).ToList<CardModel>();
    if (list.Count == 0)
      return;
    await CardCmd.DiscardAndDraw(choiceContext, (IEnumerable<CardModel>) list, list.Count);
  }
}
