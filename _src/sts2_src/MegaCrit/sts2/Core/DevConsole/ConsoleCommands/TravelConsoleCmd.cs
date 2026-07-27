// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.ConsoleCommands.TravelConsoleCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Runs;

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;

public class TravelConsoleCmd : AbstractConsoleCmd
{
  public override string CmdName => "travel";

  public override string Args => "";

  public override string Description => "Enables you to jump to any room on the map.";

  public override bool IsNetworked => true;

  public override CmdResult Process(Player? issuingPlayer, string[] args)
  {
    if (!RunManager.Instance.IsInProgress)
      return new CmdResult(false, "A run is currently not in progress...");
    NMapScreen.Instance.SetDebugTravelEnabled(!NMapScreen.Instance.IsDebugTravelEnabled);
    return new CmdResult(true, "Travel mode " + (NMapScreen.Instance.IsDebugTravelEnabled ? "enabled" : "disabled"));
  }
}
