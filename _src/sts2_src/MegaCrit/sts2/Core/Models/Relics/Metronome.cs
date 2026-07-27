// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.Metronome
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public class Metronome : RelicModel
{
  private const string _orbCountKey = "OrbCount";
  private bool _isActivating;
  private int _orbsChanneled;

  public override RelicRarity Rarity => RelicRarity.Rare;

  public override bool ShowCounter
  {
    get
    {
      if (!CombatManager.Instance.IsInProgress)
        return false;
      return this.OrbsChanneled < this.DynamicVars["OrbCount"].IntValue || this.IsActivating;
    }
  }

  public override int DisplayAmount
  {
    get => Math.Min(this.OrbsChanneled, this.DynamicVars["OrbCount"].IntValue);
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new DamageVar(30M, ValueProp.Unpowered),
        new DynamicVar("OrbCount", 7M)
      });
    }
  }

  private bool IsActivating
  {
    get => this._isActivating;
    set
    {
      this.AssertMutable();
      this._isActivating = value;
      this.UpdateDisplay();
    }
  }

  private int OrbsChanneled
  {
    get => this._orbsChanneled;
    set
    {
      this.AssertMutable();
      this._orbsChanneled = value;
      this.UpdateDisplay();
    }
  }

  public override Task AfterRoomEntered(AbstractRoom room)
  {
    if (!(room is CombatRoom))
      return Task.CompletedTask;
    this.OrbsChanneled = 0;
    this.UpdateDisplay();
    return Task.CompletedTask;
  }

  public override async Task AfterOrbChanneled(
    PlayerChoiceContext choiceContext,
    Player player,
    OrbModel orb)
  {
    if (player != this.Owner)
      return;
    this.OrbsChanneled++;
    if (this.OrbsChanneled != this.DynamicVars["OrbCount"].IntValue)
      return;
    TaskHelper.RunSafely(this.DoActivateVisuals());
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, (IEnumerable<Creature>) this.Owner.Creature.CombatState.HittableEnemies, this.DynamicVars.Damage, this.Owner.Creature);
  }

  public override Task AfterCombatEnd(CombatRoom _)
  {
    this.Status = RelicStatus.Normal;
    this.OrbsChanneled = 0;
    this.UpdateDisplay();
    return Task.CompletedTask;
  }

  private void UpdateDisplay()
  {
    if (this.OrbsChanneled == this.DynamicVars["OrbCount"].IntValue - 1 && !this.IsActivating)
      this.Status = RelicStatus.Active;
    else
      this.Status = RelicStatus.Normal;
    this.InvokeDisplayAmountChanged();
  }

  private async Task DoActivateVisuals()
  {
    this.IsActivating = true;
    this.Flash();
    await Cmd.Wait(1f);
    this.IsActivating = false;
  }
}
