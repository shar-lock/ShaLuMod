// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Runs.RelicGrabBag
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Runs;

public class RelicGrabBag
{
  private static readonly HashSet<RelicRarity> _rarities = new HashSet<RelicRarity>()
  {
    RelicRarity.Common,
    RelicRarity.Uncommon,
    RelicRarity.Rare,
    RelicRarity.Shop
  };
  private readonly Dictionary<RelicRarity, List<RelicModel>> _deques = new Dictionary<RelicRarity, List<RelicModel>>();
  private readonly List<RelicModel> _mpFallbackDequeue = new List<RelicModel>();
  private readonly bool _refreshAllowed;
  private List<RelicModel>? _originalRelics;

  public bool IsPopulated => this._deques.Count > 0;

  public bool HasAvailableRelics(IRunState runState)
  {
    foreach (RelicRarity rarity in RelicGrabBag._rarities)
    {
      if (this.GetAvailableDeque(rarity, runState, (Func<RelicModel, bool>) (_ => true)) != null)
        return true;
    }
    return this._mpFallbackDequeue.Any<RelicModel>((Func<RelicModel, bool>) (r => r.IsAllowed(runState)));
  }

  public RelicGrabBag()
  {
  }

  public RelicGrabBag(bool refreshAllowed) => this._refreshAllowed = refreshAllowed;

  public void Populate(Player player, Rng rng)
  {
    if (this.IsPopulated)
      throw new InvalidOperationException("Grab bag was already populated.");
    List<RelicModel> list1 = ModelDb.RelicPool<SharedRelicPool>().GetUnlockedRelics(player.UnlockState).ToList<RelicModel>();
    list1.AddRange(player.Character.RelicPool.GetUnlockedRelics(player.UnlockState));
    list1.RemoveAll((Predicate<RelicModel>) (r => !RelicGrabBag._rarities.Contains(r.Rarity)));
    this._originalRelics = list1;
    foreach (RelicModel relicModel in list1)
    {
      List<RelicModel> relicModelList;
      if (!this._deques.TryGetValue(relicModel.Rarity, out relicModelList))
      {
        relicModelList = new List<RelicModel>();
        this._deques[relicModel.Rarity] = relicModelList;
      }
      relicModelList.Add(relicModel);
    }
    foreach (List<RelicModel> list2 in this._deques.Values)
      list2.UnstableShuffle<RelicModel>(rng);
  }

  public void Populate(IEnumerable<RelicModel> relics, Rng rng)
  {
    if (this.IsPopulated)
      throw new InvalidOperationException("Grab bag was already populated.");
    this._originalRelics = relics.ToList<RelicModel>();
    foreach (RelicModel originalRelic in this._originalRelics)
    {
      List<RelicModel> relicModelList;
      if (!this._deques.TryGetValue(originalRelic.Rarity, out relicModelList))
      {
        relicModelList = new List<RelicModel>();
        this._deques[originalRelic.Rarity] = relicModelList;
      }
      relicModelList.Add(originalRelic);
    }
    foreach (List<RelicModel> list in this._deques.Values)
      list.UnstableShuffle<RelicModel>(rng);
  }

  public RelicModel? PullFromFront(RelicRarity rarity, IRunState runState)
  {
    return this.PullFromFront(rarity, (Func<RelicModel, bool>) (_ => true), runState);
  }

  public RelicModel? PullFromFront(
    RelicRarity rarity,
    Func<RelicModel, bool> filter,
    IRunState runState)
  {
    List<RelicModel> availableDeque = this.GetAvailableDeque(rarity, runState, filter);
    if (availableDeque == null || availableDeque.Count == 0)
      return (RelicModel) null;
    for (int index = 0; index < availableDeque.Count; ++index)
    {
      RelicModel relicModel = availableDeque[index];
      if (filter(relicModel))
      {
        availableDeque.RemoveAt(index);
        return relicModel;
      }
    }
    return (RelicModel) null;
  }

  public RelicModel? PullFromBack(
    RelicRarity rarity,
    Func<RelicModel, bool> filter,
    IRunState runState)
  {
    List<RelicModel> availableDeque = this.GetAvailableDeque(rarity, runState, filter);
    if (availableDeque == null || availableDeque.Count == 0)
      return (RelicModel) null;
    for (int index = availableDeque.Count - 1; index >= 0; --index)
    {
      RelicModel relicModel = availableDeque[index];
      if (filter(relicModel))
      {
        availableDeque.RemoveAt(index);
        return relicModel;
      }
    }
    return (RelicModel) null;
  }

  public void Remove<T>() where T : RelicModel => this.Remove((RelicModel) ModelDb.Relic<T>());

  public void Remove(RelicModel relic)
  {
    foreach (KeyValuePair<RelicRarity, List<RelicModel>> deque in this._deques)
      deque.Value.RemoveAll((Predicate<RelicModel>) (r => r.Id == relic.Id));
  }

  public void MoveToFallback(RelicModel toRemove)
  {
    RelicModel relicModel = (RelicModel) null;
    foreach (KeyValuePair<RelicRarity, List<RelicModel>> deque in this._deques)
    {
      List<RelicModel> relicModelList = deque.Value;
      for (int index = 0; index < relicModelList.Count; ++index)
      {
        if (relicModelList[index].Id == toRemove.Id)
        {
          if (relicModel == null)
            relicModel = relicModelList[index];
          relicModelList.RemoveAt(index);
          --index;
        }
      }
    }
    if (relicModel == null)
      return;
    this._mpFallbackDequeue.Add(relicModel);
  }

  private List<RelicModel>? GetAvailableDeque(
    RelicRarity rarity,
    IRunState runState,
    Func<RelicModel, bool> filter)
  {
    this.RemoveDisallowedRelicsFromDeques(runState);
    List<RelicModel> deque = this.GetDeque(rarity);
    if (deque.Count == 0 && this._refreshAllowed)
    {
      this.RefreshRarity(rarity);
      this.RemoveDisallowedRelicsFromDeques(runState);
    }
    for (; deque != null && !this.DequeHasAnyRelics(deque, filter); deque = rarity == RelicRarity.None ? (List<RelicModel>) null : this.GetDeque(rarity))
    {
      RelicRarity relicRarity;
      switch (rarity)
      {
        case RelicRarity.Common:
          relicRarity = RelicRarity.Uncommon;
          break;
        case RelicRarity.Uncommon:
          relicRarity = RelicRarity.Rare;
          break;
        case RelicRarity.Shop:
          relicRarity = RelicRarity.Common;
          break;
        default:
          relicRarity = RelicRarity.None;
          break;
      }
      rarity = relicRarity;
    }
    if (deque == null && this.DequeHasAnyRelics(this._mpFallbackDequeue, filter))
      deque = this._mpFallbackDequeue;
    return deque;
  }

  private bool DequeHasAnyRelics(List<RelicModel> deque, Func<RelicModel, bool> filter)
  {
    return deque.Any<RelicModel>(new Func<RelicModel, bool>(filter.Invoke));
  }

  private void RemoveDisallowedRelicsFromDeques(IRunState runState)
  {
    foreach (KeyValuePair<RelicRarity, List<RelicModel>> deque in this._deques)
    {
      List<RelicModel> relicModelList = deque.Value;
      for (int index = 0; index < relicModelList.Count; ++index)
      {
        if (!relicModelList[index].IsAllowed(runState))
        {
          relicModelList.RemoveAt(index);
          --index;
        }
      }
    }
    for (int index = 0; index < this._mpFallbackDequeue.Count; ++index)
    {
      if (!this._mpFallbackDequeue[index].IsAllowed(runState))
      {
        this._mpFallbackDequeue.RemoveAt(index);
        --index;
      }
    }
  }

  public SerializableRelicGrabBag ToSerializable()
  {
    SerializableRelicGrabBag serializable = new SerializableRelicGrabBag();
    foreach (KeyValuePair<RelicRarity, List<RelicModel>> deque in this._deques)
    {
      RelicRarity relicRarity;
      List<RelicModel> source;
      deque.Deconstruct(ref relicRarity, ref source);
      RelicRarity key = relicRarity;
      List<ModelId> list = source.Select<RelicModel, ModelId>((Func<RelicModel, ModelId>) (r => r.Id)).ToList<ModelId>();
      serializable.RelicIdLists[key] = list;
    }
    return serializable;
  }

  public static RelicGrabBag FromSerializable(SerializableRelicGrabBag save)
  {
    RelicGrabBag relicGrabBag = new RelicGrabBag();
    relicGrabBag.LoadFromSerializable(save);
    return relicGrabBag;
  }

  public void LoadFromSerializable(SerializableRelicGrabBag save)
  {
    foreach (KeyValuePair<RelicRarity, List<ModelId>> relicIdList in save.RelicIdLists)
    {
      RelicRarity relicRarity;
      List<ModelId> modelIdList1;
      relicIdList.Deconstruct(ref relicRarity, ref modelIdList1);
      RelicRarity key = relicRarity;
      List<ModelId> modelIdList2 = modelIdList1;
      List<RelicModel> relicModelList;
      if (!this._deques.TryGetValue(key, out relicModelList))
      {
        relicModelList = new List<RelicModel>();
        this._deques[key] = relicModelList;
      }
      relicModelList.Clear();
      foreach (ModelId id in modelIdList2)
      {
        RelicModel byIdOrNull = ModelDb.GetByIdOrNull<RelicModel>(id);
        if (byIdOrNull != null)
          relicModelList.Add(byIdOrNull);
      }
    }
  }

  private List<RelicModel> GetDeque(RelicRarity rarity)
  {
    List<RelicModel> relicModelList;
    return !this._deques.TryGetValue(rarity, out relicModelList) ? new List<RelicModel>() : relicModelList;
  }

  private void RefreshRarity(RelicRarity rarity)
  {
    if (this._originalRelics == null)
      throw new InvalidOperationException("Tried to refresh relics but original list is null");
    foreach (RelicModel originalRelic in this._originalRelics)
    {
      if (originalRelic.Rarity == rarity)
      {
        List<RelicModel> relicModelList;
        if (!this._deques.TryGetValue(originalRelic.Rarity, out relicModelList))
        {
          relicModelList = new List<RelicModel>();
          this._deques[originalRelic.Rarity] = relicModelList;
        }
        relicModelList.Add(originalRelic);
      }
    }
  }

  public bool Contains(RelicModel relic)
  {
    relic.AssertCanonical();
    foreach (KeyValuePair<RelicRarity, List<RelicModel>> deque in this._deques)
    {
      if (deque.Value.Contains(relic))
        return true;
    }
    return false;
  }
}
