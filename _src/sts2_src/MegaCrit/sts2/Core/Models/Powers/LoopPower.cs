// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.LoopPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class LoopPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
  {
    if (player != this.Owner.Player || player.PlayerCombatState.OrbQueue.Orbs.Count == 0)
      return;
    for (int i = 0; i < this.Amount; ++i)
    {
      await OrbCmd.Passive(choiceContext, player.PlayerCombatState.OrbQueue.Orbs[0], (Creature) null);
      await Cmd.Wait(0.25f);
    }
  }
}
