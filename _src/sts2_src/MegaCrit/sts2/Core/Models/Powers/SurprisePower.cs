// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.SurprisePower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Models.Monsters;
using System;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class SurprisePower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Single;

  public override async Task AfterDeath(
    PlayerChoiceContext choiceContext,
    Creature target,
    bool wasRemovalPrevented,
    float deathAnimLength)
  {
    Creature fatGremlin;
    if (wasRemovalPrevented)
      fatGremlin = (Creature) null;
    else if (this.Owner != target)
    {
      fatGremlin = (Creature) null;
    }
    else
    {
      fatGremlin = this.CombatState.CreateCreature(ModelDb.Monster<FatGremlin>().ToMutable(), CombatSide.Enemy, "fat");
      int totalStolen = 0;
      foreach (ThieveryPower powerInstance in this.Owner.GetPowerInstances<ThieveryPower>())
      {
        int intValue = powerInstance.DynamicVars.Gold.IntValue;
        totalStolen += intValue;
        HeistPower mutable = (HeistPower) ModelDb.Power<HeistPower>().ToMutable();
        mutable.Target = powerInstance.Target;
        await PowerCmd.Apply(choiceContext, (PowerModel) mutable, fatGremlin, (Decimal) intValue, this.Owner, (CardModel) null);
      }
      Creature creature = await CreatureCmd.Add<SneakyGremlin>(this.CombatState, "sneaky");
      await CreatureCmd.Add(fatGremlin);
      if (totalStolen <= 0)
        fatGremlin = (Creature) null;
      else if (!(this.CombatState.Encounter is GremlinMercNormal encounter))
      {
        fatGremlin = (Creature) null;
      }
      else
      {
        encounter.MarkGoldStolen();
        fatGremlin = (Creature) null;
      }
    }
  }

  public override bool ShouldStopCombatFromEnding() => true;
}
