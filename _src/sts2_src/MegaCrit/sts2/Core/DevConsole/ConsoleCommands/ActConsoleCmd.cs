// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.ConsoleCommands.ActConsoleCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;

public class ActConsoleCmd : AbstractConsoleCmd
{
  public override string CmdName => "act";

  public override string Args => "<int|string: act>";

  public override string Description
  {
    get
    {
      return "Jumps to an act. If integer, will jump to that act. Otherwise, replaces the current act with the act passed.";
    }
  }

  public override bool IsNetworked => true;

  public override CmdResult Process(Player? issuingPlayer, string[] args)
  {
    if (args.Length != 1)
      return new CmdResult(false, "There must be one argument.");
    if (issuingPlayer?.RunState == null)
      return new CmdResult(false, "This command only works during a run.");
    int result;
    if (int.TryParse(args[0], out result))
    {
      int count = issuingPlayer.RunState.Acts.Count;
      if (result > count || result < 1)
        return new CmdResult(false, $"The act you are trying to navigate to does not exist. Select act indexes between: 1-{count}");
      return new CmdResult(ActConsoleCmd.NextAct(result - 1), true, $"Navigated to act '{result}'.");
    }
    string actName = args[0].ToUpperInvariant();
    ActModel actModel = ModelDb.Acts.FirstOrDefault<ActModel>((Func<ActModel, bool>) (c => c.Id.Entry == actName));
    if (actModel == null)
      return new CmdResult(false, $"Act named {actName} not found.");
    RunState runState = (RunState) issuingPlayer.RunState;
    ActModel mutable = actModel.ToMutable();
    runState.SetActDebug(mutable);
    mutable.GenerateRooms(runState.Rng.UpFront, runState.UnlockState, runState.Players.Count > 1);
    return new CmdResult(RunManager.Instance.EnterAct(runState.CurrentActIndex), true, $"Replaced current act with act {actName}.");
  }

  private static async Task NextAct(int actIndex)
  {
    NMapScreen.Instance.SetTravelEnabled(true);
    await RunManager.Instance.EnterAct(actIndex);
  }
}
