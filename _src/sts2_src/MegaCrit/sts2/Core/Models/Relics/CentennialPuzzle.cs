// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.CentennialPuzzle
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class CentennialPuzzle : RelicModel
{
  private bool _usedThisCombat;

  public override RelicRarity Rarity => RelicRarity.Common;

  public override string FlashSfx => "event:/sfx/ui/relic_activate_draw";

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new CardsVar(3));
    }
  }

  public bool UsedThisCombat
  {
    get => this._usedThisCombat;
    private set
    {
      if (this._usedThisCombat == value)
        return;
      this.AssertMutable();
      this._usedThisCombat = value;
    }
  }

  public override async Task AfterDamageReceived(
    PlayerChoiceContext choiceContext,
    Creature target,
    DamageResult result,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource)
  {
    if (!CombatManager.Instance.IsInProgress || target != this.Owner.Creature || result.UnblockedDamage <= 0 || this.UsedThisCombat)
      return;
    this.Flash();
    this.UsedThisCombat = true;
    for (int i = 0; (Decimal) i < this.DynamicVars.Cards.BaseValue; ++i)
    {
      CardModel cardModel = await CardPileCmd.Draw(choiceContext, this.Owner);
    }
  }

  public override Task AfterCombatEnd(CombatRoom _)
  {
    this.UsedThisCombat = false;
    return Task.CompletedTask;
  }
}
