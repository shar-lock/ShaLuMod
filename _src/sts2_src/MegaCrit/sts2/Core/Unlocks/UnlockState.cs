// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Unlocks.UnlockState
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Timeline;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Unlocks;

public class UnlockState
{
  public static readonly UnlockState none = new UnlockState((IEnumerable<string>) Array.Empty<string>(), (IEnumerable<ModelId>) Array.Empty<ModelId>(), 0);
  public static readonly UnlockState all = new UnlockState((IEnumerable<string>) EpochModel.AllEpochIds, ModelDb.AllEncounters.Select<EncounterModel, ModelId>((Func<EncounterModel, ModelId>) (e => e.Id)), 999999999);
  private readonly HashSet<string> _unlockedEpochIds;
  private readonly HashSet<ModelId> _encountersSeen;

  public int NumberOfRuns { get; }

  public IEnumerable<CharacterModel> Characters
  {
    get
    {
      List<CharacterModel> list = ModelDb.AllCharacters.ToList<CharacterModel>();
      if (!this.IsEpochRevealed<Silent1Epoch>())
        list.Remove((CharacterModel) ModelDb.Character<Silent>());
      if (!this.IsEpochRevealed<Regent1Epoch>())
        list.Remove((CharacterModel) ModelDb.Character<Regent>());
      if (!this.IsEpochRevealed<Necrobinder1Epoch>())
        list.Remove((CharacterModel) ModelDb.Character<Necrobinder>());
      if (!this.IsEpochRevealed<Defect1Epoch>())
        list.Remove((CharacterModel) ModelDb.Character<Defect>());
      return (IEnumerable<CharacterModel>) list;
    }
  }

  public IEnumerable<AncientEventModel> SharedAncients
  {
    get
    {
      List<AncientEventModel> list = ModelDb.AllSharedAncients.ToList<AncientEventModel>();
      if (!this.IsEpochRevealed<DarvEpoch>())
        list.Remove((AncientEventModel) ModelDb.AncientEvent<Darv>());
      return (IEnumerable<AncientEventModel>) list;
    }
  }

  public IEnumerable<RelicModel> Relics
  {
    get
    {
      return ModelDb.AllRelicPools.Select<RelicPoolModel, IEnumerable<RelicModel>>((Func<RelicPoolModel, IEnumerable<RelicModel>>) (p => p.GetUnlockedRelics(this))).SelectMany<IEnumerable<RelicModel>, RelicModel>((Func<IEnumerable<RelicModel>, IEnumerable<RelicModel>>) (r => r));
    }
  }

  public IEnumerable<PotionModel> Potions
  {
    get
    {
      return ModelDb.AllPotionPools.Select<PotionPoolModel, IEnumerable<PotionModel>>((Func<PotionPoolModel, IEnumerable<PotionModel>>) (p => p.GetUnlockedPotions(this))).SelectMany<IEnumerable<PotionModel>, PotionModel>((Func<IEnumerable<PotionModel>, IEnumerable<PotionModel>>) (r => r));
    }
  }

  public bool HasSeenEncounter(EncounterModel encounter)
  {
    return this._encountersSeen.Contains(encounter.Id);
  }

  public IEnumerable<CardPoolModel> CharacterCardPools
  {
    get
    {
      return this.Characters.Select<CharacterModel, CardPoolModel>((Func<CharacterModel, CardPoolModel>) (c => c.CardPool));
    }
  }

  public IEnumerable<CardModel> Cards
  {
    get
    {
      return this.CardPools.SelectMany<CardPoolModel, CardModel>((Func<CardPoolModel, IEnumerable<CardModel>>) (p => p.AllCards)).Concat<CardModel>(this.Characters.SelectMany<CharacterModel, CardModel>((Func<CharacterModel, IEnumerable<CardModel>>) (c => c.StartingDeck)).Distinct<CardModel>()).Distinct<CardModel>();
    }
  }

  public IEnumerable<CardPoolModel> CardPools
  {
    get
    {
      return this.CharacterCardPools.Concat<CardPoolModel>(ModelDb.AllSharedCardPools).Distinct<CardPoolModel>();
    }
  }

  public UnlockState(
    IEnumerable<string> unlockedEpochIds,
    IEnumerable<ModelId> encountersSeen,
    int numberOfRuns)
  {
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    this._unlockedEpochIds = unlockedEpochIds.Where<string>(UnlockState.\u003C\u003EO.\u003C0\u003E__IsValidEpoch ?? (UnlockState.\u003C\u003EO.\u003C0\u003E__IsValidEpoch = new Func<string, bool>(UnlockState.IsValidEpoch))).ToHashSet<string>();
    this._encountersSeen = encountersSeen.ToHashSet<ModelId>();
    this.NumberOfRuns = numberOfRuns;
  }

  public UnlockState(ProgressState progress)
  {
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    this._unlockedEpochIds = progress.Epochs.Where<SerializableEpoch>((Func<SerializableEpoch, bool>) (e => e.State == EpochState.Revealed)).Select<SerializableEpoch, string>((Func<SerializableEpoch, string>) (e => e.Id)).Where<string>(UnlockState.\u003C\u003EO.\u003C0\u003E__IsValidEpoch ?? (UnlockState.\u003C\u003EO.\u003C0\u003E__IsValidEpoch = new Func<string, bool>(UnlockState.IsValidEpoch))).ToHashSet<string>();
    this._encountersSeen = progress.EncounterStats.Keys.Where<ModelId>((Func<ModelId, bool>) (id => ModelDb.GetByIdOrNull<AbstractModel>(id) is EncounterModel)).ToHashSet<ModelId>();
    this.NumberOfRuns = progress.NumberOfRuns;
  }

  public UnlockState(IEnumerable<UnlockState> unlockStatesEnumerable)
  {
    UnlockState[] array = unlockStatesEnumerable.ToArray<UnlockState>();
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    this._unlockedEpochIds = ((IEnumerable<UnlockState>) array).Select<UnlockState, HashSet<string>>((Func<UnlockState, HashSet<string>>) (s => s._unlockedEpochIds)).SelectMany<HashSet<string>, string>((Func<HashSet<string>, IEnumerable<string>>) (m => (IEnumerable<string>) m)).Distinct<string>().Where<string>(UnlockState.\u003C\u003EO.\u003C0\u003E__IsValidEpoch ?? (UnlockState.\u003C\u003EO.\u003C0\u003E__IsValidEpoch = new Func<string, bool>(UnlockState.IsValidEpoch))).ToHashSet<string>();
    this._encountersSeen = ((IEnumerable<UnlockState>) array).Select<UnlockState, HashSet<ModelId>>((Func<UnlockState, HashSet<ModelId>>) (s => s._encountersSeen)).SelectMany<HashSet<ModelId>, ModelId>((Func<HashSet<ModelId>, IEnumerable<ModelId>>) (b => (IEnumerable<ModelId>) b)).Distinct<ModelId>().ToHashSet<ModelId>();
    this.NumberOfRuns = ((IEnumerable<UnlockState>) array).Max<UnlockState>((Func<UnlockState, int>) (s => s.NumberOfRuns));
  }

  public static bool IsValidEpoch(string epochId) => EpochModel.IsValid(epochId);

  public bool IsEpochRevealed<T>() where T : EpochModel
  {
    return this._unlockedEpochIds.Contains(EpochModel.GetId<T>());
  }

  public int EpochUnlockCount() => this._unlockedEpochIds.Count;

  public SerializableUnlockState ToSerializable()
  {
    return new SerializableUnlockState()
    {
      UnlockedEpochs = this._unlockedEpochIds.ToList<string>(),
      EncountersSeen = this._encountersSeen.ToList<ModelId>(),
      NumberOfRuns = this.NumberOfRuns
    };
  }

  public static UnlockState FromSerializable(SerializableUnlockState unlockState)
  {
    if (unlockState == null)
      return UnlockState.all;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    List<ModelId> list = unlockState.EncountersSeen.Select<ModelId, EncounterModel>(UnlockState.\u003C\u003EO.\u003C1\u003E__EncounterOrDeprecated ?? (UnlockState.\u003C\u003EO.\u003C1\u003E__EncounterOrDeprecated = new Func<ModelId, EncounterModel>(SaveUtil.EncounterOrDeprecated))).Select<EncounterModel, ModelId>((Func<EncounterModel, ModelId>) (e => e.Id)).ToHashSet<ModelId>().ToList<ModelId>();
    return new UnlockState((IEnumerable<string>) unlockState.UnlockedEpochs, (IEnumerable<ModelId>) list, unlockState.NumberOfRuns);
  }
}
