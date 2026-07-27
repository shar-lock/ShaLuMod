// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.ConsoleCommands.UnlockConsoleCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Timeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;

public class UnlockConsoleCmd : AbstractConsoleCmd
{
  private static readonly Dictionary<string, Action<UnlockConsoleCmd, List<string>?>> _unlockActions = new Dictionary<string, Action<UnlockConsoleCmd, List<string>>>()
  {
    ["cards"] = (Action<UnlockConsoleCmd, List<string>>) ((c, s) => c.UnlockCards(s)),
    ["potions"] = (Action<UnlockConsoleCmd, List<string>>) ((c, s) => c.UnlockPotions(s)),
    ["relics"] = (Action<UnlockConsoleCmd, List<string>>) ((c, s) => c.UnlockRelics(s)),
    ["monsters"] = (Action<UnlockConsoleCmd, List<string>>) ((c, s) => c.UnlockMonsters(s)),
    ["events"] = (Action<UnlockConsoleCmd, List<string>>) ((c, s) => c.UnlockEvents(s)),
    ["acts"] = (Action<UnlockConsoleCmd, List<string>>) ((c, s) => c.UnlockActs()),
    ["epochs"] = (Action<UnlockConsoleCmd, List<string>>) ((c, s) => c.UnlockEpochs(s)),
    ["ascensions"] = (Action<UnlockConsoleCmd, List<string>>) ((c, s) => c.UnlockAscensions(s)),
    ["all"] = (Action<UnlockConsoleCmd, List<string>>) ((c, s) =>
    {
      c.UnlockCards((List<string>) null);
      c.UnlockPotions((List<string>) null);
      c.UnlockRelics((List<string>) null);
      c.UnlockMonsters((List<string>) null);
      c.UnlockEvents((List<string>) null);
      c.UnlockActs();
      c.UnlockEpochs((List<string>) null);
      c.UnlockAscensions((List<string>) null);
    })
  };
  private static readonly List<string> _validDiscoveryTypes = UnlockConsoleCmd._unlockActions.Keys.ToList<string>();
  private static readonly Dictionary<string, Func<IEnumerable<string>>> _validIdSources = new Dictionary<string, Func<IEnumerable<string>>>()
  {
    ["cards"] = (Func<IEnumerable<string>>) (() => ModelDb.AllCards.Select<CardModel, string>((Func<CardModel, string>) (c => c.Id.Entry))),
    ["potions"] = (Func<IEnumerable<string>>) (() => ModelDb.AllPotions.Select<PotionModel, string>((Func<PotionModel, string>) (c => c.Id.Entry))),
    ["relics"] = (Func<IEnumerable<string>>) (() => ModelDb.AllRelics.Select<RelicModel, string>((Func<RelicModel, string>) (c => c.Id.Entry))),
    ["monsters"] = (Func<IEnumerable<string>>) (() => ModelDb.Monsters.Select<MonsterModel, string>((Func<MonsterModel, string>) (c => c.Id.Entry))),
    ["events"] = (Func<IEnumerable<string>>) (() => ModelDb.AllEvents.Select<EventModel, string>((Func<EventModel, string>) (c => c.Id.Entry))),
    ["epochs"] = (Func<IEnumerable<string>>) (() => (IEnumerable<string>) EpochModel.AllEpochIds)
  };

  public override string CmdName => "unlock";

  public override string Args => "<type:string>";

  public override string Description
  {
    get
    {
      return "Marks all cards/potions/relics/monsters/events/acts/epochs/ascensions as discovered, or 'all' to unlock everything.";
    }
  }

  public override bool IsNetworked => false;

  public override CmdResult Process(Player? issuingPlayer, string[] args)
  {
    if (args.Length < 1)
      return new CmdResult(false, "No argument specified.\n" + this.Args);
    List<string> list = args.Length > 1 ? ((IEnumerable<string>) RuntimeHelpers.GetSubArray<string>(args, Range.StartAt(Index.op_Implicit(1)))).ToList<string>() : (List<string>) null;
    return this.ProcessUnlock(args[0], list);
  }

  private CmdResult ProcessUnlock(string discoveryType, List<string>? args)
  {
    Action<UnlockConsoleCmd, List<string>> action;
    if (!UnlockConsoleCmd._unlockActions.TryGetValue(discoveryType, out action))
      return new CmdResult(false, $"Argument {discoveryType} not recognized as a discovery type.\n{this.Args}");
    if (args != null)
    {
      for (int index = 0; index < args.Count; ++index)
        args[index] = args[index].ToUpperInvariant();
      Func<IEnumerable<string>> func;
      if (UnlockConsoleCmd._validIdSources.TryGetValue(discoveryType, out func))
      {
        HashSet<string> validIds = func().ToHashSet<string>();
        List<string> list = args.Where<string>((Func<string, bool>) (id => !validIds.Contains(id))).ToList<string>();
        if (list.Count > 0)
          return new CmdResult(false, $"Unknown {discoveryType}: {string.Join(", ", (IEnumerable<string>) list)}");
      }
    }
    action(this, args);
    SaveManager.Instance.SaveProgressFile();
    return new CmdResult(true, $"Unlocked {discoveryType} {(args != null ? string.Join(", ", (IEnumerable<string>) args) : "")}");
  }

  private void UnlockCards(List<string>? cards)
  {
    foreach (ModelId cardId in (cards != null ? cards.Select<string, ModelId>((Func<string, ModelId>) (c => new ModelId("CARD", c))) : (IEnumerable<ModelId>) null) ?? ModelDb.AllCards.Select<CardModel, ModelId>((Func<CardModel, ModelId>) (c => c.Id)))
      SaveManager.Instance.Progress.MarkCardAsSeen(cardId);
  }

  private void UnlockRelics(List<string>? relics)
  {
    foreach (ModelId relicId in (relics != null ? relics.Select<string, ModelId>((Func<string, ModelId>) (c => new ModelId("RELIC", c))) : (IEnumerable<ModelId>) null) ?? ModelDb.AllRelics.Select<RelicModel, ModelId>((Func<RelicModel, ModelId>) (c => c.Id)))
      SaveManager.Instance.Progress.MarkRelicAsSeen(relicId);
  }

  private void UnlockPotions(List<string>? potions)
  {
    foreach (ModelId potionId in (potions != null ? potions.Select<string, ModelId>((Func<string, ModelId>) (c => new ModelId("POTION", c))) : (IEnumerable<ModelId>) null) ?? ModelDb.AllPotions.Select<PotionModel, ModelId>((Func<PotionModel, ModelId>) (c => c.Id)))
      SaveManager.Instance.Progress.MarkPotionAsSeen(potionId);
  }

  private void UnlockMonsters(List<string>? monsters)
  {
    foreach (ModelId enemyId in (monsters != null ? monsters.Select<string, ModelId>((Func<string, ModelId>) (c => new ModelId("MONSTER", c))) : (IEnumerable<ModelId>) null) ?? ModelDb.Monsters.Select<MonsterModel, ModelId>((Func<MonsterModel, ModelId>) (c => c.Id)))
    {
      EnemyStats enemyStats = SaveManager.Instance.Progress.GetOrCreateEnemyStats(enemyId);
      if (enemyStats.FightStats.Count == 0)
        enemyStats.FightStats.Add(new FightStats()
        {
          Character = ModelDb.GetId<Ironclad>(),
          Wins = 1
        });
    }
  }

  private void UnlockActs()
  {
    foreach (AbstractModel act in ModelDb.Acts)
      SaveManager.Instance.Progress.MarkActAsSeen(act.Id);
  }

  private void UnlockEvents(List<string>? events)
  {
    foreach (ModelId eventId in (events != null ? events.Select<string, ModelId>((Func<string, ModelId>) (c => new ModelId("EVENT", c))) : (IEnumerable<ModelId>) null) ?? ModelDb.AllEvents.Select<EventModel, ModelId>((Func<EventModel, ModelId>) (c => c.Id)))
      SaveManager.Instance.Progress.MarkEventAsSeen(eventId);
  }

  private void UnlockEpochs(List<string>? epochs)
  {
    foreach (string epochId in ((IReadOnlyList<string>) epochs ?? EpochModel.AllEpochIds).Except<string>(SaveManager.Instance.Progress.Epochs.Where<SerializableEpoch>((Func<SerializableEpoch, bool>) (e => e.State == EpochState.Revealed)).Select<SerializableEpoch, string>((Func<SerializableEpoch, string>) (e => e.Id))))
      SaveManager.Instance.ObtainEpochOverride(epochId, EpochState.Revealed);
  }

  private void UnlockAscensions(List<string>? ascensions)
  {
    if (ascensions != null)
      throw new NotImplementedException();
    SaveManager.Instance.Progress.MaxMultiplayerAscension = 10;
    foreach (AbstractModel allCharacter in ModelDb.AllCharacters)
      SaveManager.Instance.Progress.GetOrCreateCharacterStats(allCharacter.Id).MaxAscension = 10;
  }

  public override CompletionResult GetArgumentCompletions(Player? player, string[] args)
  {
    if (args.Length <= 1)
    {
      string partial = args.Length != 0 ? args[0] : "";
      List<string> stringList = !string.IsNullOrWhiteSpace(partial) ? UnlockConsoleCmd._validDiscoveryTypes.Where<string>((Func<string, bool>) (type => type.Contains(partial, StringComparison.OrdinalIgnoreCase))).ToList<string>() : UnlockConsoleCmd._validDiscoveryTypes;
      return new CompletionResult()
      {
        Candidates = stringList,
        Type = CompletionType.Argument,
        ArgumentContext = this.CmdName
      };
    }
    return new CompletionResult()
    {
      Type = CompletionType.Argument,
      ArgumentContext = this.CmdName
    };
  }
}
