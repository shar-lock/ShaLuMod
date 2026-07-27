// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.ConsoleCommands.DieConsoleCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Runs;

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;

public class DieConsoleCmd : AbstractConsoleCmd
{
  public override string CmdName => "die";

  public override string Args => "";

  public override string Description => "You die";

  public override bool IsNetworked => true;

  public override CmdResult Process(Player? issuingPlayer, string[] args)
  {
    return issuingPlayer == null || !RunManager.Instance.IsInProgress ? new CmdResult(false, "A run does not appear to be in progress") : new CmdResult(CreatureCmd.Kill(issuingPlayer.Creature), true, "You died.");
  }
}
