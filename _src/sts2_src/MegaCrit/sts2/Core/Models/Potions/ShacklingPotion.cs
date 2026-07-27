// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Potions.ShacklingPotion
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Potions;

public sealed class ShacklingPotion : PotionModel
{
  public override PotionRarity Rarity => PotionRarity.Rare;

  public override PotionUsage Usage => PotionUsage.CombatOnly;

  public override TargetType TargetType => TargetType.AllEnemies;

  public override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>());
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new PowerVar<StrengthPower>(7M));
    }
  }

  protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
  {
    Creature creature = this.Owner.Creature;
    foreach (Creature hittableEnemy in (IEnumerable<Creature>) creature.CombatState.HittableEnemies)
      NCombatRoom.Instance?.PlaySplashVfx(hittableEnemy, new Color("91a19f"));
    IReadOnlyList<ShacklingPotionPower> shacklingPotionPowerList = await PowerCmd.Apply<ShacklingPotionPower>(choiceContext, (IEnumerable<Creature>) creature.CombatState.HittableEnemies, (Decimal) this.DynamicVars.Strength.IntValue, this.Owner.Creature, (CardModel) null);
  }
}
