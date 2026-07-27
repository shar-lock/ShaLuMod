// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Enchantments.Imbued
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Enchantments;

public sealed class Imbued : EnchantmentModel
{
  public override bool CanEnchantCardType(CardType cardType) => cardType == CardType.Skill;

  public override bool ShouldStartAtBottomOfDrawPile => true;

  public override bool ShowAmount => false;

  public override async Task AfterAutoPrePlayPhaseEntered(
    PlayerChoiceContext choiceContext,
    Player player)
  {
    if (player != this.Card.Owner || player.PlayerCombatState.TurnNumber > 1)
      return;
    await CardCmd.AutoPlay(choiceContext, this.Card, (Creature) null);
  }
}
