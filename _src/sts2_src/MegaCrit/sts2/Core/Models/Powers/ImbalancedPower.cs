// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.ImbalancedPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class ImbalancedPower : PowerModel
{
  public override PowerType Type => PowerType.Debuff;

  public override PowerStackType StackType => PowerStackType.Single;

  public override async Task AfterDamageGiven(
    PlayerChoiceContext choiceContext,
    Creature? dealer,
    DamageResult result,
    ValueProp props,
    Creature target,
    CardModel? cardSource)
  {
    if (dealer != this.Owner || !result.WasFullyBlocked)
      return;
    this.Flash();
    if (this.Owner.Monster is BowlbugRock monster)
      monster.IsOffBalance = true;
    else
      await CreatureCmd.Stun(this.Owner);
  }
}
