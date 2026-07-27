// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Potions.EntropicBrew
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Potions;

public sealed class EntropicBrew : PotionModel
{
  public override PotionRarity Rarity => PotionRarity.Rare;

  public override PotionUsage Usage => PotionUsage.AnyTime;

  public override TargetType TargetType => TargetType.AnyPlayer;

  protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
  {
    PotionModel.AssertValidForTargetedPotion(target);
    Player targetPlayer = target.Player;
    while (targetPlayer.HasOpenPotionSlots)
    {
      if (!(await PotionCmd.TryToProcure(PotionFactory.CreateRandomPotionOutOfCombat(targetPlayer, targetPlayer.RunState.Rng.CombatPotionGeneration).ToMutable(), targetPlayer)).success)
      {
        targetPlayer = (Player) null;
        return;
      }
    }
    targetPlayer = (Player) null;
  }
}
