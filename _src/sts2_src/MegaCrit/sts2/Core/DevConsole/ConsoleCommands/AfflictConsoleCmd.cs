// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.ConsoleCommands.AfflictConsoleCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;

public class AfflictConsoleCmd : AbstractConsoleCmd
{
  public override string CmdName => "afflict";

  public override string Args => "<id:string> [amount:int] [hand-index:int]";

  public override string Description
  {
    get => "Apply the specified affliction to a card in the player's hand";
  }

  public override bool IsNetworked => true;

  public override CmdResult Process(Player? issuingPlayer, string[] args)
  {
    if (args.Length == 0)
      return new CmdResult(false, "Must specify an affliction ID!");
    if (!CombatManager.Instance.IsInProgress)
      return new CmdResult(false, "Combat is not currently in progress!");
    ModelId id = new ModelId(ModelId.SlugifyCategory<AfflictionModel>(), args[0].ToUpperInvariant());
    AfflictionModel mutable;
    try
    {
      mutable = ModelDb.GetById<AfflictionModel>(id).ToMutable();
    }
    catch (ModelNotFoundException ex)
    {
      return new CmdResult(false, $"Affliction '{id.Entry}' not found");
    }
    int result1 = 0;
    int result2 = 0;
    if (args.Length > 1 && !int.TryParse(args[1], out result1))
      return new CmdResult(false, $"Arg 2 must be the affliction count (int), got '{args[1]}'.");
    if (args.Length > 2 && !int.TryParse(args[2], out result2))
      return new CmdResult(false, $"Arg 3 must be the hand index (int), got '{args[2]}'.");
    CardPile pile = PileType.Hand.GetPile(issuingPlayer);
    int count = pile.Cards.Count;
    if (result2 < 0 || result2 >= count)
      return new CmdResult(false, $"Invalid hand index {result2}. Valid range: 0-{count - 1}.");
    CardModel card = pile.Cards[result2];
    return new CmdResult((Task) CardCmd.Afflict(mutable, card, (Decimal) result1), true, $"Afflicted card {card.Title} with {result1} {mutable.Title.GetFormattedText()}");
  }

  public override CompletionResult GetArgumentCompletions(Player? player, string[] args)
  {
    if (args.Length <= 1)
      return this.CompleteArgument((IEnumerable<string>) ModelDb.DebugAfflictions.Select<AfflictionModel, string>((Func<AfflictionModel, string>) (affliction => affliction.Id.Entry)).ToList<string>(), Array.Empty<string>(), ((IEnumerable<string>) args).FirstOrDefault<string>() ?? "");
    return new CompletionResult()
    {
      Type = CompletionType.Argument,
      ArgumentContext = this.CmdName
    };
  }
}
