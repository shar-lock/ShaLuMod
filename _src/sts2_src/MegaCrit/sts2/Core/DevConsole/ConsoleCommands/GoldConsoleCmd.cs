// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.ConsoleCommands.GoldConsoleCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Runs;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;

public class GoldConsoleCmd : AbstractConsoleCmd
{
  public override string CmdName => "gold";

  public override string Args => "<amount:int>";

  public override string Description => "Manipulate player gold. Cha-ching!";

  public override bool IsNetworked => true;

  public override CmdResult Process(Player? issuingPlayer, string[] args)
  {
    if (args.Length < 1)
      return new CmdResult(false, "An amount is required");
    int result;
    if (!int.TryParse(args[0], out result))
      return new CmdResult(false, "First argument (the gold amount) must be an int.");
    if (issuingPlayer == null || !RunManager.Instance.IsInProgress)
      return new CmdResult(false, "A run does not appear to be in progress");
    return new CmdResult(PlayerCmd.GainGold((Decimal) result, issuingPlayer), true, $"'{result}' gold added.");
  }
}
