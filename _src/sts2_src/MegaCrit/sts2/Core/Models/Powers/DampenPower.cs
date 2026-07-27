// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.DampenPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class DampenPower : PowerModel
{
  public override PowerType Type => PowerType.Debuff;

  public override PowerStackType StackType => PowerStackType.None;

  protected override object InitInternalData() => (object) new DampenPower.Data();

  public override Task AfterApplied(Creature? applier, CardModel? cardSource)
  {
    foreach (CardModel cardModel in this.Owner.Player.PlayerCombatState.AllCards.Where<CardModel>((Func<CardModel, bool>) (c => c.IsUpgraded)))
    {
      this.GetInternalData<DampenPower.Data>().downgradedCardsToOldUpgradeLevels.Add(cardModel, cardModel.CurrentUpgradeLevel);
      CardCmd.Downgrade(cardModel);
    }
    this.Flash();
    return Task.CompletedTask;
  }

  public override async Task AfterDeath(
    PlayerChoiceContext choiceContext,
    Creature creature,
    bool wasRemovalPrevented,
    float deathAnimLength)
  {
    if (wasRemovalPrevented)
      return;
    DampenPower.Data internalData = this.GetInternalData<DampenPower.Data>();
    if (!internalData.casters.Contains(creature))
      return;
    internalData.casters.Remove(creature);
    if (internalData.casters.Count != 0)
      return;
    await PowerCmd.Remove((PowerModel) this);
  }

  public override Task AfterRemoved(Creature oldOwner)
  {
    foreach (KeyValuePair<CardModel, int> toOldUpgradeLevel in this.GetInternalData<DampenPower.Data>().downgradedCardsToOldUpgradeLevels)
    {
      CardModel cardModel;
      int num1;
      toOldUpgradeLevel.Deconstruct(ref cardModel, ref num1);
      CardModel card = cardModel;
      int num2 = num1;
      for (int index = 0; index < num2; ++index)
        CardCmd.Upgrade(card);
    }
    return Task.CompletedTask;
  }

  public void AddCaster(Creature creature)
  {
    this.GetInternalData<DampenPower.Data>().casters.Add(creature);
  }

  private class Data
  {
    public readonly HashSet<Creature> casters = new HashSet<Creature>();
    public readonly Dictionary<CardModel, int> downgradedCardsToOldUpgradeLevels = new Dictionary<CardModel, int>();
  }
}
