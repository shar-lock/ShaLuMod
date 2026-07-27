// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.EntropyPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class EntropyPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Transform));
    }
  }

  public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
  {
    if (player != this.Owner.Player)
      return;
    foreach (CardModel original in (await CardSelectCmd.FromHand(choiceContext, player, new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, this.Amount), (Func<CardModel, bool>) null, (AbstractModel) this)).ToList<CardModel>())
    {
      CardPileAddResult random = await CardCmd.TransformToRandom(original, player.RunState.Rng.CombatCardSelection);
    }
  }
}
