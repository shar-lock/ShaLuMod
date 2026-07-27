// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.ConsoleCommands.InstantConsoleCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;

public class InstantConsoleCmd : AbstractConsoleCmd
{
  public override string CmdName => "instant";

  public override string Args => "";

  public override string Description => "Turns instant mode on.";

  public override bool IsNetworked => false;

  public override CmdResult Process(Player? issuingPlayer, string[] args)
  {
    if (SaveManager.Instance.PrefsSave.FastMode == FastModeType.Instant)
    {
      SaveManager.Instance.PrefsSave.FastMode = FastModeType.Fast;
      return new CmdResult(true, "Instant mode off");
    }
    SaveManager.Instance.PrefsSave.FastMode = FastModeType.Instant;
    return new CmdResult(true, "INSTANT MODE ACTIVE");
  }
}
