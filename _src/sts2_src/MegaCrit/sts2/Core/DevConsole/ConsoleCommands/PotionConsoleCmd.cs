// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.ConsoleCommands.PotionConsoleCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;

public class PotionConsoleCmd : AbstractConsoleCmd
{
  public override string CmdName => "potion";

  public override string Args => "<id:string>";

  public override string Description
  {
    get => "Adds potion to belt. Screaming snake case ('ENTROPIC_BREW', not 'Entropic Brew').";
  }

  public override bool IsNetworked => true;

  public override CmdResult Process(Player? issuingPlayer, string[] args)
  {
    if (args.Length < 1)
      return new CmdResult(false, this.CmdName + " requires a potion name");
    if (!RunManager.Instance.IsInProgress)
      return new CmdResult(false, "A run is not in progress.");
    string potionId = args[0].ToUpperInvariant();
    PotionModel potionModel = ModelDb.AllPotions.FirstOrDefault<PotionModel>((Func<PotionModel, bool>) (p => p.Id.Entry == potionId));
    return potionModel == null ? new CmdResult(false, $"Potion '{potionId}' not found") : new CmdResult((Task) PotionCmd.TryToProcure(potionModel.ToMutable(), issuingPlayer), true, "Added potion " + potionModel.Id.Entry);
  }

  public override CompletionResult GetArgumentCompletions(Player? player, string[] args)
  {
    if (args.Length <= 1)
      return this.CompleteArgument((IEnumerable<string>) ModelDb.AllPotions.Select<PotionModel, string>((Func<PotionModel, string>) (p => p.Id.Entry)).ToList<string>(), Array.Empty<string>(), ((IEnumerable<string>) args).FirstOrDefault<string>() ?? "");
    return new CompletionResult()
    {
      Type = CompletionType.Argument,
      ArgumentContext = this.CmdName
    };
  }
}
