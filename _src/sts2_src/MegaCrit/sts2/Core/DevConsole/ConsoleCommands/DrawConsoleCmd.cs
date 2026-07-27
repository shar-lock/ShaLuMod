// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.ConsoleCommands.DrawConsoleCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;

public class DrawConsoleCmd : AbstractConsoleCmd
{
  public override string CmdName => "draw";

  public override string Args => "<count:int>";

  public override string Description => "Draw X many cards.";

  public override bool IsNetworked => true;

  public override CmdResult Process(Player? issuingPlayer, string[] args)
  {
    int count;
    if (args.Length == 0)
    {
      count = 1;
    }
    else
    {
      int result;
      if (!int.TryParse(args[0], out result))
        return new CmdResult(false, "First argument is not an int");
      count = result;
    }
    if (count <= 0)
      return new CmdResult(false, "Draw nothing?");
    if (!RunManager.Instance.IsInProgress || issuingPlayer == null)
      return new CmdResult(false, "A run hasn't started");
    return new CmdResult(this.DrawTask(issuingPlayer, count), true, $"Drawn '{count}' cards.");
  }

  private async Task DrawTask(Player player, int count)
  {
    HookPlayerChoiceContext choiceContext = new HookPlayerChoiceContext(player, LocalContext.NetId.Value, GameActionType.Combat);
    Task task = (Task) CardPileCmd.Draw((PlayerChoiceContext) choiceContext, (Decimal) count, player);
    int num = await choiceContext.AssignTaskAndWaitForPauseOrCompletion(task) ? 1 : 0;
  }
}
