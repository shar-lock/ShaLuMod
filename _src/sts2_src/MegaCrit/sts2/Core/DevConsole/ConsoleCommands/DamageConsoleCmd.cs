// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.ConsoleCommands.DamageConsoleCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;

public class DamageConsoleCmd : AbstractConsoleCmd
{
  public override string CmdName => "damage";

  public override string Args => "<amount:int> <target-index:int>";

  public override string Description
  {
    get => "Damage all enemies, or target creature if index is given (0 is player).";
  }

  public override bool IsNetworked => true;

  public override CmdResult Process(Player? issuingPlayer, string[] args)
  {
    if (!CombatManager.Instance.IsInProgress)
      return new CmdResult(false, "This doesn't appear to be a combat!");
    int length = args.Length;
    if (length < 1 || length > 2)
      return new CmdResult(false, "There must be 1 or 2 args.");
    int result1;
    if (!int.TryParse(args[0], out result1))
      return new CmdResult(false, "Arg 1 must be the amount of damage.");
    if (result1 < 0)
      return new CmdResult(false, "The damage amount cannot be negative.");
    CombatState state = CombatManager.Instance.DebugOnlyGetState();
    IEnumerable<Creature> creatures;
    if (args.Length < 2)
    {
      creatures = (IEnumerable<Creature>) state.Enemies;
    }
    else
    {
      int result2;
      if (!int.TryParse(args[1], out result2))
        return new CmdResult(false, "Arg 2 must be the target index if specified.");
      if (result2 < 0 || result2 >= state.Creatures.Count)
        return new CmdResult(false, $"Invalid target index {result2}. Valid range: 0-{state.Creatures.Count - 1}");
      // ISSUE: object of a compiler-generated type is created
      creatures = (IEnumerable<Creature>) new \u003C\u003Ez__ReadOnlySingleElementList<Creature>(state.Creatures[result2]);
    }
    IEnumerable<string> values = creatures.Select<Creature, string>((Func<Creature, string>) (c => !c.IsPlayer ? c.Monster.Id.Entry : "PLAYER"));
    return new CmdResult(this.DamageAndCheckWinCondition(creatures, (Decimal) result1), true, $"Damaged: [{string.Join(",", values)}]");
  }

  private async Task DamageAndCheckWinCondition(IEnumerable<Creature> creatures, Decimal amount)
  {
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage((PlayerChoiceContext) new BlockingPlayerChoiceContext(), (IEnumerable<Creature>) creatures.ToList<Creature>(), amount, ValueProp.Unpowered, (Creature) null, (CardModel) null, (CardPlay) null);
    int num = await CombatManager.Instance.CheckWinCondition() ? 1 : 0;
  }
}
