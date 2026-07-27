// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.ConsoleCommands.StarsConsoleCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;

public class StarsConsoleCmd : AbstractConsoleCmd
{
  public override string CmdName => "stars";

  public override string Args => "<amount:int>";

  public override string Description => "Adds stars to player";

  public override bool IsNetworked => true;

  public override CmdResult Process(Player? issuingPlayer, string[] args)
  {
    if (args.Length == 0)
      return new CmdResult(false, "The first argument must be an int.");
    int result;
    if (!int.TryParse(args[0], out result))
      return new CmdResult(false, "The first argument must be an int.");
    if (issuingPlayer == null)
      return new CmdResult(false, "This command only works during a run.");
    if (issuingPlayer.PlayerCombatState == null)
      return new CmdResult(false, "This command only works in combat.");
    if (result < 0)
      return new CmdResult(false, "The stars amount cannot be negative.");
    return new CmdResult(PlayerCmd.GainStars((Decimal) result, issuingPlayer), true, $"Added '{result}' stars.");
  }
}
