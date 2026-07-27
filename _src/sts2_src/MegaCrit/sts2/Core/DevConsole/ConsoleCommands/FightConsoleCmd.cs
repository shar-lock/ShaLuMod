// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.ConsoleCommands.FightConsoleCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Exceptions;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;

public class FightConsoleCmd : AbstractConsoleCmd
{
  public override string CmdName => "fight";

  public override string Args => "<id:string>";

  public override string Description => "Jumps a player to a specific encounter.";

  public override bool IsNetworked => true;

  public override CmdResult Process(Player? issuingPlayer, string[] args)
  {
    if (args.Length == 0)
      return new CmdResult(false, "No encounter name specified.");
    if (!RunManager.Instance.IsInProgress)
      return new CmdResult(false, "A run is currently not in progress!");
    ModelId id = new ModelId(ModelId.SlugifyCategory<EncounterModel>(), args[0].ToUpperInvariant());
    EncounterModel mutable;
    try
    {
      mutable = ModelDb.GetById<EncounterModel>(id).ToMutable();
    }
    catch (ModelNotFoundException ex)
    {
      return new CmdResult(false, $"Encounter '{id.Entry}' not found");
    }
    mutable.DebugRandomizeRng();
    return new CmdResult((Task) RunManager.Instance.EnterRoomDebug(RoomType.Monster, model: (AbstractModel) mutable), true, $"Jumped to encounter: '{mutable.Id.Entry}'");
  }

  public override CompletionResult GetArgumentCompletions(Player? player, string[] args)
  {
    if (args.Length <= 1)
      return this.CompleteArgument((IEnumerable<string>) ModelDb.AllEncounters.Select<EncounterModel, string>((Func<EncounterModel, string>) (e => e.Id.Entry)).ToList<string>(), Array.Empty<string>(), ((IEnumerable<string>) args).FirstOrDefault<string>() ?? "");
    return new CompletionResult()
    {
      Type = CompletionType.Argument,
      ArgumentContext = this.CmdName
    };
  }
}
