// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.GoldPlatedCables
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Relics;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class GoldPlatedCables : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Uncommon;

  public override int ModifyOrbPassiveTriggerCounts(OrbModel orb, int triggerCount)
  {
    return orb.Owner != this.Owner || orb != this.Owner.PlayerCombatState.OrbQueue.Orbs[0] ? triggerCount : triggerCount + 1;
  }

  public override Task AfterModifyingOrbPassiveTriggerCount(OrbModel orb)
  {
    this.Flash();
    return Task.CompletedTask;
  }
}
