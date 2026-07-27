// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.FranticEscape
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class FranticEscape : CardModel
{
  public FranticEscape()
    : base(1, CardType.Status, CardRarity.Status, TargetType.Self)
  {
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      SandpitPower power = this.GetSandpitEnemy()?.GetPower<SandpitPower>();
      return power == null ? (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<SandpitPower>()) : power.HoverTips;
    }
  }

  public override int MaxUpgradeLevel => 0;

  public override bool CanBeGeneratedInCombat => false;

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    Creature sandpitEnemy = this.GetSandpitEnemy();
    SandpitPower power = sandpitEnemy != null ? sandpitEnemy.Powers.OfType<SandpitPower>().FirstOrDefault<SandpitPower>((Func<SandpitPower, bool>) (s => s.Target == this.Owner.Creature)) : (SandpitPower) null;
    if (power != null)
    {
      int num = await PowerCmd.ModifyAmount(choiceContext, (PowerModel) power, 1M, sandpitEnemy, (CardModel) this);
    }
    this.EnergyCost.AddThisCombat(1);
  }

  private Creature? GetSandpitEnemy()
  {
    ICombatState combatState = this.CombatState;
    return combatState == null ? (Creature) null : combatState.Enemies.FirstOrDefault<Creature>((Func<Creature, bool>) (c => c.HasPower<SandpitPower>()));
  }
}
