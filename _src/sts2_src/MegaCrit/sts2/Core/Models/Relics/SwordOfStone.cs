// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.SwordOfStone
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class SwordOfStone : RelicModel
{
  private const string _elitesKey = "Elites";
  private int _elitesDefeated;

  public override RelicRarity Rarity => RelicRarity.Event;

  public override bool ShowCounter => true;

  public override int DisplayAmount => this.ElitesDefeated;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Elites", 5M));
    }
  }

  [SavedProperty]
  public int ElitesDefeated
  {
    get => this._elitesDefeated;
    set
    {
      this.AssertMutable();
      this._elitesDefeated = value;
      this.InvokeDisplayAmountChanged();
    }
  }

  public override async Task AfterCombatVictory(CombatRoom room)
  {
    if (room.RoomType != RoomType.Elite)
      return;
    this.ElitesDefeated++;
    this.Flash();
    if (!((Decimal) this.ElitesDefeated >= this.DynamicVars["Elites"].BaseValue))
      return;
    RelicModel relicModel = await RelicCmd.Replace((RelicModel) this, ModelDb.Relic<SwordOfJade>().ToMutable());
  }
}
