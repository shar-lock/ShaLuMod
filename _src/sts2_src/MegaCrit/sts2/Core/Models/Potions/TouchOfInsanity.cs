// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Potions.TouchOfInsanity
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Potions;

public sealed class TouchOfInsanity : PotionModel
{
  public override PotionRarity Rarity => PotionRarity.Uncommon;

  public override PotionUsage Usage => PotionUsage.CombatOnly;

  public override TargetType TargetType => TargetType.AnyPlayer;

  protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
  {
    PotionModel.AssertValidForTargetedPotion(target);
    (await CardSelectCmd.FromHand(choiceContext, target.Player, new CardSelectorPrefs(this.SelectionScreenPrompt, 1), (Func<CardModel, bool>) (c => c.CostsEnergyOrStars(false) || c.CostsEnergyOrStars(true)), (AbstractModel) this)).FirstOrDefault<CardModel>()?.SetToFreeThisCombat();
  }
}
