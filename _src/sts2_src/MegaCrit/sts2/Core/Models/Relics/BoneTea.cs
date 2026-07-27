// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.BoneTea
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class BoneTea : RelicModel
{
  private const string _combatsKey = "Combats";
  private int _combatsLeft = 1;

  public override RelicRarity Rarity => RelicRarity.Event;

  public override bool IsUsedUp => this.CombatsLeft <= 0;

  public override bool ShowCounter => false;

  public override int DisplayAmount => Math.Max(0, this.CombatsLeft);

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Combats", (Decimal) this.CombatsLeft));
    }
  }

  [SavedProperty]
  public int CombatsLeft
  {
    get => this._combatsLeft;
    set
    {
      this.AssertMutable();
      this._combatsLeft = value;
      this.DynamicVars["Combats"].BaseValue = (Decimal) this._combatsLeft;
      this.InvokeDisplayAmountChanged();
      if (!this.IsUsedUp)
        return;
      this.Status = RelicStatus.Disabled;
    }
  }

  public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
  {
    if (this.IsUsedUp || player != this.Owner || this.Owner.PlayerCombatState.TurnNumber > 1)
      return Task.CompletedTask;
    foreach (CardModel card in (IEnumerable<CardModel>) PileType.Hand.GetPile(this.Owner).Cards)
      CardCmd.Upgrade(card);
    --this.CombatsLeft;
    this.Flash();
    return Task.CompletedTask;
  }
}
