// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.AdaptablePower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Monsters;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class AdaptablePower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Single;

  protected override object InitInternalData() => (object) new AdaptablePower.Data();

  private bool IsReviving => this.GetInternalData<AdaptablePower.Data>().isReviving;

  public void DoRevive() => this.GetInternalData<AdaptablePower.Data>().isReviving = false;

  public override async Task AfterDeath(
    PlayerChoiceContext choiceContext,
    Creature creature,
    bool wasRemovalPrevented,
    float deathAnimLength)
  {
    if (wasRemovalPrevented || creature != this.Owner || !(creature.Monster is TestSubject monster))
      return;
    this.GetInternalData<AdaptablePower.Data>().isReviving = true;
    await monster.TriggerDeadState();
  }

  public override bool ShouldAllowHitting(Creature creature)
  {
    return creature != this.Owner || !this.IsReviving;
  }

  public override bool ShouldStopCombatFromEnding() => true;

  public override bool ShouldCreatureBeRemovedFromCombatAfterDeath(Creature creature)
  {
    return creature != this.Owner;
  }

  public override bool ShouldPowerBeRemovedAfterOwnerDeath() => false;

  private class Data
  {
    public bool isReviving;
  }
}
