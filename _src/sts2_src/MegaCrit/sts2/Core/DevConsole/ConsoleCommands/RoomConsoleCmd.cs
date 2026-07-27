// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.ConsoleCommands.RoomConsoleCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;

public class RoomConsoleCmd : AbstractConsoleCmd
{
  public override string CmdName => "room";

  public override string Args => "<id:string>";

  public override string Description => "Jumps a player to a specific room.";

  public override bool IsNetworked => true;

  public override CmdResult Process(Player? issuingPlayer, string[] args)
  {
    if (args.Length == 0)
      return new CmdResult(false, "No room name specified.");
    if (!RunManager.Instance.IsInProgress)
      return new CmdResult(false, "A run is currently not in progress!");
    string upperInvariant = args[0].ToUpperInvariant();
    RoomType result;
    if (!AbstractConsoleCmd.TryParseEnum<RoomType>(upperInvariant, out result))
      return new CmdResult(false, $"Room '{upperInvariant}' not found");
    return new CmdResult((Task) RunManager.Instance.EnterRoomDebug(result), true, $"Jumped to room: '{result}'");
  }

  public override CompletionResult GetArgumentCompletions(Player? player, string[] args)
  {
    if (args.Length <= 1)
      return this.CompleteArgument((IEnumerable<string>) ((IEnumerable<string>) Enum.GetNames(typeof (RoomType))).Where<string>((Func<string, bool>) (n => !n.Equals("Unassigned"))).ToList<string>(), Array.Empty<string>(), ((IEnumerable<string>) args).FirstOrDefault<string>() ?? "");
    return new CompletionResult()
    {
      Type = CompletionType.Argument,
      ArgumentContext = this.CmdName
    };
  }
}
