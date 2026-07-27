// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.PossessSpeedPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class PossessSpeedPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Single;

  protected override object InitInternalData() => (object) new PossessSpeedPower.Data();

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DexterityPower>());
    }
  }

  private Dictionary<Creature, Decimal> StolenDexterity
  {
    get => this.GetInternalData<PossessSpeedPower.Data>().stolenDexterity;
  }

  public override Task AfterPowerAmountChanged(
    PlayerChoiceContext choiceContext,
    PowerModel power,
    Decimal amount,
    Creature? applier,
    CardModel? cardSource)
  {
    if (applier != this.Owner || !power.Owner.IsPlayer || !(power is DexterityPower) || amount >= 0M)
      return Task.CompletedTask;
    if (!this.StolenDexterity.ContainsKey(power.Owner))
      this.StolenDexterity.Add(power.Owner, 0M);
    this.StolenDexterity[power.Owner] += amount;
    return Task.CompletedTask;
  }

  public override async Task AfterDeath(
    PlayerChoiceContext choiceContext,
    Creature creature,
    bool wasRemovalPrevented,
    float deathAnimLength)
  {
    if (wasRemovalPrevented || creature != this.Owner)
      return;
    foreach (KeyValuePair<Creature, Decimal> keyValuePair in this.StolenDexterity)
    {
      DexterityPower dexterityPower = await PowerCmd.Apply<DexterityPower>(choiceContext, keyValuePair.Key, -keyValuePair.Value, (Creature) null, (CardModel) null);
    }
  }

  private class Data
  {
    public Dictionary<Creature, Decimal> stolenDexterity = new Dictionary<Creature, Decimal>();
  }
}
