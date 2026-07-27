// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.ConsoleCommands.CardConsoleCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;

public class CardConsoleCmd : AbstractConsoleCmd
{
  public override string CmdName => "card";

  public override string Args => "<card-id:string> [pileName:string]";

  public override string Description
  {
    get
    {
      return "Spawns a card into a pile (hand by default). Screaming snake case ('BODY_SLAM', not 'Body Slam').";
    }
  }

  public override bool IsNetworked => true;

  public override CmdResult Process(Player? issuingPlayer, string[] args)
  {
    if (args.Length == 0)
      return new CmdResult(false, "No card name specified.");
    if (!RunManager.Instance.IsInProgress)
      return new CmdResult(false, "A run is currently not in progress!");
    PileType result = PileType.Hand;
    if (args.Length >= 2 && !AbstractConsoleCmd.TryParseEnum<PileType>(args[1], out result))
      return new CmdResult(false, $"Unknown pile '{args[1]}'. Valid piles: {string.Join(", ", Enum.GetNames<PileType>())}");
    if (result == PileType.Hand)
    {
      int count = PileType.Hand.GetPile(issuingPlayer).Cards.Count;
      if (count >= CardPile.MaxCardsInHand)
        return new CmdResult(false, $"The hand is full ({count}/{CardPile.MaxCardsInHand}).");
    }
    string cardName = args[0].ToUpperInvariant();
    CardModel canonicalCard = ModelDb.AllCards.FirstOrDefault<CardModel>((Func<CardModel, bool>) (c => c.Id.Entry == cardName));
    if (canonicalCard == null)
      return new CmdResult(false, $"Card '{cardName}' not found");
    return new CmdResult((Task) CardPileCmd.Add((result.IsCombatPile() ? (ICardScope) CombatManager.Instance.DebugOnlyGetState() : (ICardScope) RunManager.Instance.DebugOnlyGetState()).CreateCard(canonicalCard, issuingPlayer), result), true, $"Added card '{canonicalCard.Id.Entry}' to '{result}'");
  }

  public override CompletionResult GetArgumentCompletions(Player? player, string[] args)
  {
    if (args.Length <= 1)
      return this.CompleteArgument((IEnumerable<string>) ModelDb.AllCards.Select<CardModel, string>((Func<CardModel, string>) (card => card.Id.Entry)).ToList<string>(), Array.Empty<string>(), ((IEnumerable<string>) args).FirstOrDefault<string>() ?? "");
    if (args.Length == 2)
      return this.CompleteArgument((IEnumerable<string>) ((IEnumerable<string>) Enum.GetNames<PileType>()).ToList<string>(), new string[1]
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
