// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.PainfulStabsPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class PainfulStabsPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get => HoverTipFactory.FromCardWithCardHoverTips<Wound>();
  }

  public override bool ShouldPowerBeRemovedAfterOwnerDeath() => false;

  public override bool ShouldCreatureBeRemovedFromCombatAfterDeath(Creature creature)
  {
    return creature != this.Owner;
  }

  public override async Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
  {
    Dictionary<Creature, List<DamageResult>> damageResultsByCreature;
    if (command.Attacker != this.Owner)
      damageResultsByCreature = (Dictionary<Creature, List<DamageResult>>) null;
    else if (command.TargetSide == this.Owner.Side)
      damageResultsByCreature = (Dictionary<Creature, List<DamageResult>>) null;
    else if (!command.DamageProps.IsPoweredAttack())
    {
      damageResultsByCreature = (Dictionary<Creature, List<DamageResult>>) null;
    }
    else
    {
      List<DamageResult> list = command.Results.SelectMany<List<DamageResult>, DamageResult>((Func<List<DamageResult>, IEnumerable<DamageResult>>) (r => (IEnumerable<DamageResult>) r)).ToList<DamageResult>();
      if (!list.Any<DamageResult>((Func<DamageResult, bool>) (r => r.UnblockedDamage > 0)))
      {
        damageResultsByCreature = (Dictionary<Creature, List<DamageResult>>) null;
      }
      else
      {
        damageResultsByCreature = new Dictionary<Creature, List<DamageResult>>();
        foreach (DamageResult damageResult in list)
        {
          if (damageResult.Receiver.IsPlayer)
          {
            if (!damageResultsByCreature.ContainsKey(damageResult.Receiver))
              damageResultsByCreature.Add(damageResult.Receiver, new List<DamageResult>());
            damageResultsByCreature[damageResult.Receiver].Add(damageResult);
          }
        }
        bool anyWoundApplied = false;
        foreach (Creature key in damageResultsByCreature.Keys)
        {
          int num = damageResultsByCreature[key].Count<DamageResult>((Func<DamageResult, bool>) (r => r.UnblockedDamage > 0));
          anyWoundApplied |= num > 0;
          await CardPileCmd.AddToCombatAndPreview<Wound>(key, PileType.Discard, this.Amount * num, (Player) null);
        }
        if (!anyWoundApplied)
        {
          damageResultsByCreature = (Dictionary<Creature, List<DamageResult>>) null;
        }
        else
        {
          this.Flash();
          damageResultsByCreature = (Dictionary<Creature, List<DamageResult>>) null;
        }
      }
    }
  }
}
