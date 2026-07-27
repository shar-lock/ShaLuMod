// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Modifiers.Murderous
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Modifiers;

public class Murderous : ModifierModel
{
  private const int _strengthAmount = 3;

  public override async Task AfterRoomEntered(AbstractRoom room)
  {
    if (!(room is CombatRoom combatRoom))
      return;
    IReadOnlyList<StrengthPower> strengthPowerList = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) combatRoom.CombatState.Creatures, 3M, (Creature) null, (CardModel) null);
  }

  public override Task AfterCreatureAddedToCombat(Creature creature)
  {
    return creature.Side == CombatSide.Player ? Task.CompletedTask : (Task) PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), creature, 3M, (Creature) null, (CardModel) null);
  }
}
