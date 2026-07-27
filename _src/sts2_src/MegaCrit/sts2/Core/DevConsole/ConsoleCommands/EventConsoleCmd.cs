// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.ConsoleCommands.EventConsoleCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;

public class EventConsoleCmd : AbstractConsoleCmd
{
  public override string CmdName => "event";

  public override string Args => "<id:string>";

  public override string Description => "Jumps a player to a specific event.";

  public override bool IsNetworked => true;

  public override CmdResult Process(Player? issuingPlayer, string[] args)
  {
    if (args.Length == 0)
      return new CmdResult(false, "No event name specified.");
    if (!RunManager.Instance.IsInProgress)
      return new CmdResult(false, "A run is currently not in progress!");
    string eventName = args[0].ToUpperInvariant();
    EventModel eventModel = EventConsoleCmd.Events.FirstOrDefault<EventModel>((Func<EventModel, bool>) (c => c.Id.Entry == eventName));
    if (eventModel == null)
      return new CmdResult(false, $"Event '{eventName}' not found");
    MapPointType mapPointType = eventModel is AncientEventModel ? MapPointType.Ancient : MapPointType.Unknown;
    issuingPlayer.RunState.AppendToMapPointHistory(mapPointType, RoomType.Event, eventModel.Id);
    return new CmdResult(RunManager.Instance.EnterRoom((AbstractRoom) new EventRoom(eventModel)), true, $"Jumped to event: '{eventModel.Id.Entry}'");
  }

  public override CompletionResult GetArgumentCompletions(Player? player, string[] args)
  {
    if (args.Length <= 1)
      return this.CompleteArgument((IEnumerable<string>) EventConsoleCmd.Events.Select<EventModel, string>((Func<EventModel, string>) (e => e.Id.Entry)).ToList<string>(), Array.Empty<string>(), ((IEnumerable<string>) args).FirstOrDefault<string>() ?? "");
    return new CompletionResult()
    {
      Type = CompletionType.Argument,
      ArgumentContext = this.CmdName
    };
  }

  private static IEnumerable<EventModel> Events
  {
    get => ModelDb.AllEvents.Concat<EventModel>((IEnumerable<EventModel>) ModelDb.AllAncients);
  }
}
