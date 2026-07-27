// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.ConsoleCommands.WinConsoleCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;

public class WinConsoleCmd : AbstractConsoleCmd
{
  public override string CmdName => "win";

  public override string Args => "";

  public override string Description => "You win the combat";

  public override bool IsNetworked => true;

  public override CmdResult Process(Player? issuingPlayer, string[] args)
  {
    if (!CombatManager.Instance.IsInProgress)
      return new CmdResult(false, "This doesn't appear to be a combat!");
    List<Creature> list = CombatManager.Instance.DebugOnlyGetState().Enemies.ToList<Creature>();
    return new CmdResult(this.KillEnemies(list), true, $"Killed: [{string.Join(",", list.Select<Creature, MonsterModel>((Func<Creature, MonsterModel>) (c => c.Monster)).Where<MonsterModel>((Func<MonsterModel, bool>) (m => m != null)).Select<MonsterModel, string>((Func<MonsterModel, string>) (m => m.Id.Entry.ToString())))}]");
  }

  private async Task KillEnemies(List<Creature> creatures)
  {
    foreach (Creature creature in creatures)
    {
      creature.RemoveAllPowersInternalExcept();
      await CreatureCmd.Kill(creature);
    }
    int num = await CombatManager.Instance.CheckWinCondition() ? 1 : 0;
  }
}
