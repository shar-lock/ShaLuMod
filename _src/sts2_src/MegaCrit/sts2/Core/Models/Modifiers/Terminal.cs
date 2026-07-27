// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Modifiers.Terminal
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Modifiers;

public class Terminal : ModifierModel
{
  public override async Task AfterRoomEntered(AbstractRoom room)
  {
    if (this.RunState.BaseRoom == room)
    {
      foreach (Player player in (IEnumerable<Player>) this.RunState.Players)
      {
        Creature creature = player.Creature;
        if (creature == null || creature.CurrentHp > 1 || creature.MaxHp > 1)
          await CreatureCmd.LoseMaxHp((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), player.Creature, 1M, false);
      }
    }
    if (!(room is CombatRoom combatRoom))
      return;
    foreach (Creature playerCreature in (IEnumerable<Creature>) combatRoom.CombatState.PlayerCreatures)
    {
      PlatingPower platingPower = await PowerCmd.Apply<PlatingPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), playerCreature, 5M, (Creature) null, (CardModel) null);
    }
  }
}
