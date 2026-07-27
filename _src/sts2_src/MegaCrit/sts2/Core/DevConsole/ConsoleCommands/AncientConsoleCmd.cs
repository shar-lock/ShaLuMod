// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.ConsoleCommands.AncientConsoleCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;

public class AncientConsoleCmd : AbstractConsoleCmd
{
  public override string CmdName => "ancient";

  public override string Args => "<id:string> <choice:string>";

  public override string Description => "Opens an ancient event with the selected choice";

  public override bool IsNetworked => true;

  public override CmdResult Process(Player? issuingPlayer, string[] args)
  {
    if (args.Length == 0)
      return new CmdResult(false, "No ancient ID specified.");
    EventModel byIdOrNull = ModelDb.GetByIdOrNull<EventModel>(new ModelId(ModelDb.GetCategory(typeof (EventModel)), args[0].ToUpperInvariant()));
    if (!(byIdOrNull is AncientEventModel ancientEventModel))
      return new CmdResult(false, "Invalid ancient ID.");
    string choice = (string) null;
    if (args.Length > 1)
      choice = args[1].ToUpperInvariant();
    if (choice != null && !ancientEventModel.AllPossibleOptions.Any<EventOption>((Func<EventOption, bool>) (option => option.TextKey.Contains(choice))))
      return new CmdResult(false, "invalid ancient choice.");
    EventRoom room = new EventRoom(byIdOrNull)
    {
      OnStart = new Action<EventModel>(SetDebugOption)
    };
    issuingPlayer.RunState.AppendToMapPointHistory(MapPointType.Ancient, RoomType.Event, byIdOrNull.Id);
    return new CmdResult(RunManager.Instance.EnterRoom((AbstractRoom) room), true, $"Opened Ancient Event. Forced {choice ?? "no"} option");

    void SetDebugOption(EventModel e) => ((AncientEventModel) e).DebugOption = choice;
  }

  public override CompletionResult GetArgumentCompletions(Player? player, string[] args)
  {
    if (args.Length <= 1)
      return this.CompleteArgument((IEnumerable<string>) ModelDb.AllAncients.Select<AncientEventModel, string>((Func<AncientEventModel, string>) (ancient => ancient.Id.Entry)).ToList<string>(), Array.Empty<string>(), ((IEnumerable<string>) args).FirstOrDefault<string>() ?? "");
    if (args.Length == 2 && ModelDb.GetByIdOrNull<EventModel>(new ModelId(ModelDb.GetCategory(typeof (EventModel)), args[0].ToUpperInvariant())) is AncientEventModel byIdOrNull)
      return this.CompleteArgument((IEnumerable<string>) byIdOrNull.AllPossibleOptions.Select<EventOption, string>((Func<EventOption, string>) (option => ((IEnumerable<string>) option.TextKey.Split('.', StringSplitOptions.None)).Last<string>())).ToList<string>(), new string[1]
      {
        args[0]
      }, args[1]);
    return new CompletionResult()
    {
      Type = CompletionType.Argument,
      ArgumentContext = this.CmdName
    };
  }
}
