// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.FishingRod
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class FishingRod : RelicModel
{
  private const string _combatsKey = "Combats";
  private int _combatsSeen;

  public override RelicRarity Rarity => RelicRarity.Ancient;

  public override int DisplayAmount => this.CombatsSeen % this.DynamicVars["Combats"].IntValue;

  public override bool ShowCounter => true;

  [SavedProperty]
  public int CombatsSeen
  {
    get => this._combatsSeen;
    set
    {
      this.AssertMutable();
      this._combatsSeen = value;
      this.InvokeDisplayAmountChanged();
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Combats", 3M));
    }
  }

  public override Task AfterCombatEnd(CombatRoom room)
  {
    if (room.Encounter.RoomType != RoomType.Monster)
      return Task.CompletedTask;
    ++this.CombatsSeen;
    if (this.CombatsSeen % this.DynamicVars["Combats"].IntValue == 0)
    {
      this.Flash();
      CardModel card = this.Owner.RunState.Rng.Niche.NextItem<CardModel>(PileType.Deck.GetPile(this.Owner).Cards.Where<CardModel>((Func<CardModel, bool>) (c => c.IsUpgradable)));
      if (card != null)
        CardCmd.Upgrade(card);
    }
    return Task.CompletedTask;
  }
}
