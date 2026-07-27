// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.ConsoleCommands.EnchantConsoleCmd
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

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;

public class EnchantConsoleCmd : AbstractConsoleCmd
{
  public override string CmdName => "enchant";

  public override string Args => "<id:string> [amount:int] [hand-index:int]";

  public override string Description
  {
    get => "Enchants a card in the player's hand with the specified enchantment.";
  }

  public override bool IsNetworked => true;

  public override CmdResult Process(Player? issuingPlayer, string[] args)
  {
    if (args.Length == 0)
      return new CmdResult(false, "Must specify an enchantment ID!");
    if (!CombatManager.Instance.IsInProgress)
      return new CmdResult(false, "Combat is not currently in progress!");
    ModelId id = new ModelId(ModelId.SlugifyCategory<EnchantmentModel>(), args[0].ToUpperInvariant());
    EnchantmentModel mutable;
    try
    {
      mutable = ModelDb.GetById<EnchantmentModel>(id).ToMutable();
    }
    catch (ModelNotFoundException ex)
    {
      return new CmdResult(false, $"Enchantment '{id.Entry}' not found");
    }
    int result1 = 1;
    int result2 = 0;
    if (args.Length > 1 && !int.TryParse(args[1], out result1))
      return new CmdResult(false, $"Arg 2 must be the enchantment amount (int), got '{args[1]}'.");
    if (args.Length > 2 && !int.TryParse(args[2], out result2))
      return new CmdResult(false, $"Arg 3 must be the hand index (int), got '{args[2]}'.");
    CardPile pile = PileType.Hand.GetPile(issuingPlayer);
    int count = pile.Cards.Count;
    if (result2 < 0 || result2 >= count)
      return new CmdResult(false, $"Invalid hand index {result2}. Valid range: 0-{count - 1}.");
    CardModel card = pile.Cards[result2];
    CardCmd.Enchant(mutable, card, (Decimal) result1);
    return new CmdResult(true, $"Enchanted card {card.Title} with {result1} {mutable.Title.GetFormattedText()}");
  }

  public override CompletionResult GetArgumentCompletions(Player? player, string[] args)
  {
    if (args.Length <= 1)
      return this.CompleteArgument((IEnumerable<string>) ModelDb.DebugEnchantments.Select<EnchantmentModel, string>((Func<EnchantmentModel, string>) (e => e.Id.Entry)).ToList<string>(), Array.Empty<string>(), ((IEnumerable<string>) args).FirstOrDefault<string>() ?? "");
    return new CompletionResult()
    {
      Type = CompletionType.Argument,
      ArgumentContext = this.CmdName
    };
  }
}
