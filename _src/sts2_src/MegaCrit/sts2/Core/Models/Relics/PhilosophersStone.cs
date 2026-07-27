// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.PhilosophersStone
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class PhilosophersStone : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Ancient;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new PowerVar<StrengthPower>(1M),
        (DynamicVar) new EnergyVar(1)
      });
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlyArray<IHoverTip>(new IHoverTip[2]
      {
        HoverTipFactory.FromPower<StrengthPower>(),
        HoverTipFactory.ForEnergy((RelicModel) this)
      });
    }
  }

  public override Decimal ModifyMaxEnergy(Player player, Decimal amount)
  {
    return player != this.Owner ? amount : amount + (Decimal) this.DynamicVars.Energy.IntValue;
  }

  public override Task AfterCreatureAddedToCombat(Creature creature)
  {
    if (creature.Side == this.Owner.Creature.Side)
      return Task.CompletedTask;
    this.Flash();
    return (Task) PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), creature, this.DynamicVars["StrengthPower"].BaseValue, (Creature) null, (CardModel) null);
  }

  public override async Task AfterRoomEntered(AbstractRoom room)
  {
    if (!(room is CombatRoom))
      return;
    IEnumerable<Creature> targets = this.Owner.Creature.CombatState.GetOpponentsOf(this.Owner.Creature).Where<Creature>((Func<Creature, bool>) (c => c.IsAlive));
    this.Flash();
    IReadOnlyList<StrengthPower> strengthPowerList = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), targets, this.DynamicVars["StrengthPower"].BaseValue, (Creature) null, (CardModel) null);
  }
}
