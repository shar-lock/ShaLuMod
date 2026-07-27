// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.ConsoleCommands.BlockConsoleCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;

public class BlockConsoleCmd : AbstractConsoleCmd
{
  public override string CmdName => "block";

  public override string Args => "<int:amount> <target-index:int>";

  public override string Description
  {
    get => "Gives block to player, or to target creature if index is given (0 is player).";
  }

  public override bool IsNetworked => true;

  public override CmdResult Process(Player? issuingPlayer, string[] args)
  {
    if (issuingPlayer == null)
      return new CmdResult(false, "This command only works during a run.");
    if (!CombatManager.Instance.IsInProgress)
      return new CmdResult(false, "This doesn't appear to be a combat!");
    int length = args.Length;
    if (length < 1 || length > 2)
      return new CmdResult(false, "There must be 1 or 2 args.");
    int result1;
    if (!int.TryParse(args[0], out result1))
      return new CmdResult(false, "Arg 1 must be the amount of block.");
    if (result1 < 0)
      return new CmdResult(false, "Removing block is not supported.");
    Creature creature = issuingPlayer.Creature;
    Creature target;
    if (args.Length == 1)
    {
      target = creature;
    }
    else
    {
      int result2;
      if (!int.TryParse(args[1], out result2))
        return new CmdResult(false, "Arg 2 must be the target index if specified.");
      IReadOnlyList<Creature> creatures = creature.CombatState.Creatures;
      if (result2 < 0 || result2 >= creatures.Count)
        return new CmdResult(false, $"Invalid target index {result2}. Valid range: 0-{creatures.Count - 1}");
      target = creatures[result2];
    }
    return new CmdResult(BlockConsoleCmd.GainBlock(target, result1), true, $"Added '{result1}' block to {target}.");
  }

  private static async Task GainBlock(Creature target, int amount)
  {
    Decimal num = await CreatureCmd.GainBlock(target, new BlockVar((Decimal) amount, ValueProp.Unpowered), (CardPlay) null);
  }
}
