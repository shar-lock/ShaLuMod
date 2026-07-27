// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.ConsoleCommands.ApplyPowerConsoleCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;

public class ApplyPowerConsoleCmd : AbstractConsoleCmd
{
  private static List<PowerModel>? _allPowers;

  public override string CmdName => "power";

  public override string Args => "<id:string> <amount:int> <target-index:int>";

  public override string Description => "Grant power to given target at index.";

  public override bool IsNetworked => true;

  private static IEnumerable<PowerModel> AllPowers
  {
    get
    {
      if (ApplyPowerConsoleCmd._allPowers == null)
        ApplyPowerConsoleCmd._allPowers = ((IEnumerable<Type>) ModelDb.AllAbstractModelSubtypes).Where<Type>((Func<Type, bool>) (t => t.IsSubclassOf(typeof (PowerModel)))).Select<Type, PowerModel>(ApplyPowerConsoleCmd.\u003C\u003EO.\u003C0\u003E__DebugPower ?? (ApplyPowerConsoleCmd.\u003C\u003EO.\u003C0\u003E__DebugPower = new Func<Type, PowerModel>(ModelDb.DebugPower))).ToList<PowerModel>();
      return (IEnumerable<PowerModel>) ApplyPowerConsoleCmd._allPowers;
    }
  }

  public override CmdResult Process(Player? issuingPlayer, string[] args)
  {
    if (!CombatManager.Instance.IsInProgress)
      return new CmdResult(false, "This doesn't appear to be a combat!");
    if (args.Length < 3)
      return new CmdResult(false, "There must be 3 args.");
    int result1;
    if (!int.TryParse(args[1], out result1))
      return new CmdResult(false, "Arg 1 must be the amount of power to be applied.");
    string powerId = args[0].ToUpperInvariant();
    PowerModel power = ApplyPowerConsoleCmd.AllPowers.FirstOrDefault<PowerModel>((Func<PowerModel, bool>) (c => c.Id.Entry == powerId));
    if (power == null)
      return new CmdResult(false, $"The power id {powerId} does not exist.");
    int result2;
    if (!int.TryParse(args[2], out result2))
      return new CmdResult(false, "Arg 2 must be the target index if specified.");
    IReadOnlyList<Creature> creatures = CombatManager.Instance.DebugOnlyGetState().Creatures;
    if (result2 < 0 || result2 >= creatures.Count)
      return new CmdResult(false, $"Invalid target index {result2}. Valid range: 0-{creatures.Count - 1}");
    Creature target = creatures[result2];
    PowerModel power1 = target.Powers.FirstOrDefault<PowerModel>((Func<PowerModel, bool>) (p => p.GetType() == power.GetType()));
    PlayerChoiceContext choiceContext = (PlayerChoiceContext) new BlockingPlayerChoiceContext();
    Task task = power.InstanceType != PowerInstanceType.None || power1 == null ? PowerCmd.Apply(choiceContext, power.ToMutable(), target, (Decimal) result1, (Creature) null, (CardModel) null) : (Task) PowerCmd.ModifyAmount(choiceContext, power1, (Decimal) result1, (Creature) null, (CardModel) null);
    string str = target.IsPlayer ? "PLAYER" : target.Monster.Id.Entry;
    string msg = $"AppliedPower: [{string.Join(",", new ReadOnlySpan<string>(ref str))}]";
    return new CmdResult(task, true, msg);
  }

  public override CompletionResult GetArgumentCompletions(Player? player, string[] args)
  {
    if (args.Length <= 1)
      return this.CompleteArgument((IEnumerable<string>) ApplyPowerConsoleCmd.AllPowers.Select<PowerModel, string>((Func<PowerModel, string>) (p => p.Id.Entry)).ToList<string>(), Array.Empty<string>(), ((IEnumerable<string>) args).FirstOrDefault<string>() ?? "");
    return new CompletionResult()
    {
      Type = CompletionType.Argument,
      ArgumentContext = this.CmdName
    };
  }
}
