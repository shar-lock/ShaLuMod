// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.ConsoleCommands.HealConsoleCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;

public class HealConsoleCmd : AbstractConsoleCmd
{
  public override string CmdName => "heal";

  public override string Args => "<amount:int> [index:int]";

  public override string Description => "Heal the player some amount of HP.";

  public override bool IsNetworked => true;

  public override CmdResult Process(Player? issuingPlayer, string[] args)
  {
    if (args.Length < 1)
      return new CmdResult(false, "An amount is required");
    int result1;
    if (!int.TryParse(args[0], out result1))
      return new CmdResult(false, "First argument (the heal amount) must be an int.");
    if (!RunManager.Instance.IsInProgress)
      return new CmdResult(false, "A run does not appear to be in progress");
    if (result1 < 0)
      return new CmdResult(false, "The heal amount cannot be negative.");
    Creature creature;
    if (args.Length > 1)
    {
      int result2;
      if (!int.TryParse(args[1], out result2))
        return new CmdResult(false, $"Arg 2 must be the target index (int), got '{args[1]}'.");
      IReadOnlyList<Creature> allies = CombatManager.Instance.DebugOnlyGetState().Allies;
      if (result2 < 0 || result2 >= allies.Count)
        return new CmdResult(false, $"Invalid target index {result2}. Valid range: 0-{allies.Count - 1}");
      creature = allies[result2];
    }
    else
      creature = issuingPlayer.Creature;
    return new CmdResult(CreatureCmd.Heal(creature, (Decimal) result1), true, $"Healed '{result1}' HP to {creature}.");
  }
}
