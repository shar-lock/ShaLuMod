// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.UnceasingTop
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class UnceasingTop : RelicModel
{
  public override string FlashSfx => "event:/sfx/ui/relic_activate_draw";

  public override RelicRarity Rarity => RelicRarity.Rare;

  public override async Task AfterHandEmptied(PlayerChoiceContext choiceContext, Player player)
  {
    if (player != this.Owner || !UnceasingTop.IsValidPhase(player.PlayerCombatState.Phase))
      return;
    this.Flash();
    CardModel cardModel = await CardPileCmd.Draw(choiceContext, player);
  }

  private static bool IsValidPhase(PlayerTurnPhase phase)
  {
    bool flag;
    switch (phase)
    {
      case PlayerTurnPhase.AutoPrePlay:
      case PlayerTurnPhase.Play:
      case PlayerTurnPhase.AutoPostPlay:
        flag = true;
        break;
      default:
        flag = false;
        break;
    }
    return flag;
  }
}
