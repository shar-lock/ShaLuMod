// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Potions.Ashwater
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Potions;

public sealed class Ashwater : PotionModel
{
  private readonly Color _tint = new Color("83ebdf");

  public override PotionRarity Rarity => PotionRarity.Uncommon;

  public override PotionUsage Usage => PotionUsage.CombatOnly;

  public override TargetType TargetType => TargetType.AnyPlayer;

  public override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));
    }
  }

  protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
  {
    PotionModel.AssertValidForTargetedPotion(target);
    NCombatRoom.Instance?.PlaySplashVfx(target, this._tint);
    foreach (CardModel card in await CardSelectCmd.FromHand(choiceContext, target.Player, new CardSelectorPrefs(this.SelectionScreenPrompt, 0, 999999999), (Func<CardModel, bool>) null, (AbstractModel) this))
      await CardCmd.Exhaust(choiceContext, card);
  }
}
