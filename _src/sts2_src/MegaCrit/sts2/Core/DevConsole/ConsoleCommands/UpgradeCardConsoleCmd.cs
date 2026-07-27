// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.ConsoleCommands.UpgradeCardConsoleCmd
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

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;

public class UpgradeCardConsoleCmd : AbstractConsoleCmd
{
  public override string CmdName => "upgrade";

  public override string Args => "<hand-index:int>";

  public override string Description
  {
    get => "Upgrade the target card based on its hand position (0 is the left most).";
  }

  public override bool IsNetworked => true;

  public override CmdResult Process(Player? issuingPlayer, string[] args)
  {
    if (!RunManager.Instance.IsInProgress)
      return new CmdResult(false, "A run is currently not in progress!");
    int result = 0;
    if (args.Length != 0 && !int.TryParse(args[0], out result))
      return new CmdResult(false, $"Arg 1 must be the hand index (int), got '{args[0]}'.");
    CardPile pile = PileType.Hand.GetPile(issuingPlayer);
    int count = pile.Cards.Count;
    if (result < 0 || result >= count)
      return new CmdResult(false, $"Invalid hand index {result}. Valid range: 0-{count - 1}.");
    CardModel card = pile.Cards[result];
    if (card.CurrentUpgradeLevel == card.MaxUpgradeLevel)
      return new CmdResult(false, $"The card at index={result} is already upgraded to max_level={card.MaxUpgradeLevel}!");
    CardCmd.Upgrade(card);
    return new CmdResult(true, $"Upgraded '{card.Title}' at index '{result}' in hand.");
  }

  public override CompletionResult GetArgumentCompletions(Player? player, string[] args)
  {
    if (args.Length <= 1 && RunManager.Instance.IsInProgress && CombatManager.Instance.IsInProgress && player != null)
    {
      int count = PileType.Hand.GetPile(player).Cards.Count;
      if (count > 0)
        return this.CompleteArgument((IEnumerable<string>) Enumerable.Range(0, count).Select<int, string>((Func<int, string>) (i => i.ToString())).ToList<string>(), Array.Empty<string>(), ((IEnumerable<string>) args).FirstOrDefault<string>() ?? "");
    }
    return new CompletionResult()
    {
      Type = CompletionType.Argument,
      ArgumentContext = this.CmdName
    };
  }
}
