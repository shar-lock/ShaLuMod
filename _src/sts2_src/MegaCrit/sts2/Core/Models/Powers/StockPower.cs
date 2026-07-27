// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.StockPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Monsters;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class StockPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override async Task AfterDeath(
    PlayerChoiceContext choiceContext,
    Creature target,
    bool wasRemovalPrevented,
    float deathAnimLength)
  {
    if (wasRemovalPrevented || target != this.Owner || this.Amount <= 0)
      return;
    Axebot mutable = (Axebot) ModelDb.Monster<Axebot>().ToMutable();
    mutable.ShouldPlaySpawnAnimation = true;
    mutable.StockAmount = this.Amount - 1;
    Creature creature = await CreatureCmd.Add((MonsterModel) mutable, this.CombatState, this.Owner.Side, this.Owner.SlotName);
    creature.SetNodeVisible(false);
    TaskHelper.RunSafely(StockPower.RevealReplacementAfterDeathAnim(creature, deathAnimLength));
  }

  private static async Task RevealReplacementAfterDeathAnim(
    Creature creature,
    float deathAnimLength)
  {
    await Cmd.CustomScaledWait(deathAnimLength, deathAnimLength);
    creature.SetNodeVisible(true);
    await CreatureCmd.TriggerAnim(creature, "respawn", 0.0f);
  }

  public override bool ShouldStopCombatFromEnding() => true;
}
