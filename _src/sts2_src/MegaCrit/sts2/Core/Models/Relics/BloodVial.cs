// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.BloodVial
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class BloodVial : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Common;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new HealVar(2M));
    }
  }

  public override async Task AfterPlayerTurnStartLate(
    PlayerChoiceContext choiceContext,
    Player player)
  {
    if (player != this.Owner || this.Owner.PlayerCombatState.TurnNumber > 1)
      return;
    await CreatureCmd.Heal(this.Owner.Creature, (Decimal) this.DynamicVars.Heal.IntValue);
  }
}
