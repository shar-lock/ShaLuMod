// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Potions.LiquidMemories
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Potions;

public sealed class LiquidMemories : PotionModel
{
  public override PotionRarity Rarity => PotionRarity.Rare;

  public override PotionUsage Usage => PotionUsage.CombatOnly;

  public override TargetType TargetType => TargetType.AnyPlayer;

  protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
  {
    PotionModel.AssertValidForTargetedPotion(target);
    Player player = target.Player;
    NCombatRoom.Instance?.PlaySplashVfx(target, new Color(Colors.Blue, 1f));
    CardModel card = (await CardSelectCmd.FromCombatPile(choiceContext, PileType.Discard.GetPile(player), player, new CardSelectorPrefs(this.SelectionScreenPrompt, 1))).FirstOrDefault<CardModel>();
    if (card == null)
      return;
    card.SetToFreeThisTurn();
    CardPileAddResult cardPileAddResult = await CardPileCmd.Add(card, PileType.Hand);
  }
}
