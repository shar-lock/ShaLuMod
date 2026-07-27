// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.EmotionChip
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class EmotionChip : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Rare;

  private bool LostHpInPreviousTurn
  {
    get
    {
      return CombatManager.Instance.History.Entries.OfType<DamageReceivedEntry>().Any<DamageReceivedEntry>((Func<DamageReceivedEntry, bool>) (e => e.Receiver == this.Owner.Creature && !e.Result.WasFullyBlocked && e.HappenedLastPlayerTurn(this.Owner)));
    }
  }

  public override Task AfterDamageReceived(
    PlayerChoiceContext choiceContext,
    Creature target,
    DamageResult result,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource)
  {
    if (!CombatManager.Instance.IsInProgress || target != this.Owner.Creature || result.UnblockedDamage <= 0)
      return Task.CompletedTask;
    this.Status = RelicStatus.Active;
    this.Flash();
    return Task.CompletedTask;
  }

  public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
  {
    if (player != this.Owner)
      return;
    this.Status = RelicStatus.Normal;
    if (!this.LostHpInPreviousTurn)
      return;
    this.Flash();
    foreach (OrbModel orb in (IEnumerable<OrbModel>) this.Owner.PlayerCombatState.OrbQueue.Orbs)
    {
      await OrbCmd.Passive(choiceContext, orb, (Creature) null, true);
      await Cmd.Wait(0.25f);
    }
  }

  public override Task AfterCombatEnd(CombatRoom room)
  {
    this.Status = RelicStatus.Normal;
    return Task.CompletedTask;
  }
}
