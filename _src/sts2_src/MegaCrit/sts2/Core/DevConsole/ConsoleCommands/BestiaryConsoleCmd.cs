// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.ConsoleCommands.BestiaryConsoleCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Nodes;

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;

public class BestiaryConsoleCmd : AbstractConsoleCmd
{
  public override string CmdName => "bestiary";

  public override string Args => "";

  public override string Description => "Opens the bestiary (WIP)";

  public override bool IsNetworked => false;

  public override CmdResult Process(Player? issuingPlayer, string[] args)
  {
    NGame.Instance.MainMenu.OpenCompendiumSubmenu().OpenBestiary();
    return new CmdResult(true, "Opened bestiary submenu");
  }
}
