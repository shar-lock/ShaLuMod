// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.ConsoleCommands.RemoveCardConsoleCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

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

public class RemoveCardConsoleCmd : AbstractConsoleCmd
{
  public override string CmdName => "remove_card";

  public override string Args => "<id:string> [pileName:string]";

  public override string Description
  {
    get
    {
      return "Removes a card from your Hand or Deck. Screaming snake case ('BODY_SLAM', not 'Body Slam').";
    }
  }

  public override bool IsNetworked => true;

  public override CmdResult Process(Player? issuingPlayer, string[] args)
  {
    if (args.Length == 0)
      return new CmdResult(false, "No card name specified.");
    if (!RunManager.Instance.IsInProgress)
      return new CmdResult(false, "A run is currently not in progress!");
    string cardName = args[0].ToUpperInvariant();
    CardModel cardModel = ModelDb.AllCards.FirstOrDefault<CardModel>((Func<CardModel, bool>) (c => c.Id.Entry == cardName));
    PileType result = PileType.Hand;
    if (args.Length >= 2 && !AbstractConsoleCmd.TryParseEnum<PileType>(args[1], out result))
      return new CmdResult(false, $"Unknown pile '{args[1]}'. Valid piles: Hand, Deck");
    if (cardModel == null)
      return new CmdResult(false, $"Card '{cardName}' not found");
    ModelId id = cardModel.Id;
    Task task = Task.CompletedTask;
    switch (result)
    {
      case PileType.Hand:
        using (IEnumerator<CardModel> enumerator = PileType.Hand.GetPile(issuingPlayer).Cards.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            CardModel current = enumerator.Current;
            if (current.Id == id)
            {
              task = CardPileCmd.RemoveFromCombat(current);
              break;
            }
          }
          break;
        }
      case PileType.Deck:
        using (IEnumerator<CardModel> enumerator = PileType.Deck.GetPile(issuingPlayer).Cards.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            CardModel current = enumerator.Current;
            if (current.Id == id)
            {
              task = CardPileCmd.RemoveFromDeck(current);
              break;
            }
          }
          break;
        }
      default:
        return new CmdResult(false, "Unsupported pile.");
    }
    return new CmdResult(task, true, $"Removed card '{id.Entry}'");
  }

  public override CompletionResult GetArgumentCompletions(Player? player, string[] args)
  {
    if (args.Length <= 1)
    {
      List<string> stringList = new List<string>();
      if (player != null && RunManager.Instance.IsInProgress)
      {
        IEnumerable<string> collection1 = PileType.Hand.GetPile(player).Cards.Select<CardModel, string>((Func<CardModel, string>) (c => c.Id.Entry)).Distinct<string>();
        IEnumerable<string> collection2 = PileType.Deck.GetPile(player).Cards.Select<CardModel, string>((Func<CardModel, string>) (c => c.Id.Entry)).Distinct<string>();
        stringList.AddRange(collection1);
        stringList.AddRange(collection2);
        stringList = stringList.Distinct<string>().ToList<string>();
      }
      if (stringList.Count == 0)
        stringList = ModelDb.AllCards.Select<CardModel, string>((Func<CardModel, string>) (card => card.Id.Entry)).ToList<string>();
      return this.CompleteArgument((IEnumerable<string>) stringList, Array.Empty<string>(), ((IEnumerable<string>) args).FirstOrDefault<string>() ?? "");
    }
    if (args.Length == 2)
      return this.CompleteArgument((IEnumerable<string>) new List<string>()
      {
        "Hand",
        "Deck"
      }, new string[1]{ args[0] }, args[1]);
    return new CompletionResult()
    {
      Type = CompletionType.Argument,
      ArgumentContext = this.CmdName
    };
  }
}
