// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.ConsoleCommands.TrailerConsoleCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Nodes;

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;

public class TrailerConsoleCmd : AbstractConsoleCmd
{
  public override string CmdName => "trailer";

  public override string Args => "";

  public override string Description
  {
    get => "Toggles the ability to show and hide UI elements via 0 - 9 and +- keys.";
  }

  public override bool IsNetworked => false;

  public override CmdResult Process(Player? issuingPlayer, string[] args)
  {
    NGame.ToggleTrailerMode();
    return new CmdResult(true, "Trailer mode " + (NGame.IsTrailerMode ? "enabled" : "disabled"));
  }
}
