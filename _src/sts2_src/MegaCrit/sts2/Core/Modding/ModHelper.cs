// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Modding.ModHelper
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Modding;

public static class ModHelper
{
  private static readonly Dictionary<Type, ModHelper.ModPoolContent> _moddedContentForPools = new Dictionary<Type, ModHelper.ModPoolContent>();
  private static readonly List<ModHelper.ModRunHookSubscriber> _runHookSubscribers = new List<ModHelper.ModRunHookSubscriber>();
  private static readonly List<ModHelper.ModCombatHookSubscriber> _combatHookSubscribers = new List<ModHelper.ModCombatHookSubscriber>();

  public static void AddModelToPool<TPoolType, TModelType>()
    where TPoolType : AbstractModel, IPoolModel
    where TModelType : AbstractModel
  {
    ModHelper.AddModelToPool(typeof (TPoolType), typeof (TModelType));
  }

  public static void AddModelToPool(Type poolType, Type modelType)
  {
    ModHelper.ModPoolContent modPoolContent;
    if (!ModHelper._moddedContentForPools.TryGetValue(poolType, out modPoolContent))
    {
      modPoolContent = new ModHelper.ModPoolContent()
      {
        modelsToAdd = new List<Type>()
      };
      ModHelper._moddedContentForPools.Add(poolType, modPoolContent);
    }
    if (modPoolContent.isFrozen)
      throw new InvalidOperationException($"Tried to add model {modelType} to pool {poolType}, but it's too late! You must add content before the game is initialized.");
    modPoolContent.modelsToAdd.Add(modelType);
  }

  public static IEnumerable<TModelType> ConcatModelsFromMods<TModelType>(
    IPoolModel poolModel,
    IEnumerable<TModelType> pool)
    where TModelType : AbstractModel
  {
    Type type = poolModel.GetType();
    ModHelper.ModPoolContent modPoolContent;
    if (!ModHelper._moddedContentForPools.TryGetValue(type, out modPoolContent))
    {
      modPoolContent = new ModHelper.ModPoolContent();
      ModHelper._moddedContentForPools.Add(type, modPoolContent);
    }
    modPoolContent.isFrozen = true;
    if (modPoolContent.modelsToAdd == null)
      return pool;
    IEnumerable<TModelType> second = modPoolContent.modelsToAdd.Select<Type, TModelType>((Func<Type, TModelType>) (t => ModelDb.GetById<TModelType>(ModelDb.GetId(t))));
    return pool.Concat<TModelType>(second);
  }

  public static void SubscribeForRunStateHooks(string id, RunHookSubscriptionDelegate del)
  {
    if (ModHelper._runHookSubscribers.Any<ModHelper.ModRunHookSubscriber>((Func<ModHelper.ModRunHookSubscriber, bool>) (s => s.id == id)))
    {
      Log.Error($"Tried to subscribe for RunState hooks with id {id}, but it's already been used! Ignoring subscription");
    }
    else
    {
      ModHelper._runHookSubscribers.Add(new ModHelper.ModRunHookSubscriber()
      {
        id = id,
        del = del
      });
      ModHelper._runHookSubscribers.Sort((Comparison<ModHelper.ModRunHookSubscriber>) ((x, y) => string.CompareOrdinal(x.id, y.id)));
    }
  }

  public static void SubscribeForCombatStateHooks(string id, CombatHookSubscriptionDelegate del)
  {
    if (ModHelper._combatHookSubscribers.Any<ModHelper.ModCombatHookSubscriber>((Func<ModHelper.ModCombatHookSubscriber, bool>) (s => s.id == id)))
    {
      Log.Error($"Tried to subscribe for CombatState hooks with id {id}, but it's already been used! Ignoring subscription");
    }
    else
    {
      ModHelper._combatHookSubscribers.Add(new ModHelper.ModCombatHookSubscriber()
      {
        id = id,
        del = del
      });
      ModHelper._combatHookSubscribers.Sort((Comparison<ModHelper.ModCombatHookSubscriber>) ((x, y) => string.CompareOrdinal(x.id, y.id)));
    }
  }

  public static IEnumerable<AbstractModel> IterateAllRunStateSubscribers(RunState runState)
  {
    foreach (ModHelper.ModRunHookSubscriber runHookSubscriber in ModHelper._runHookSubscribers)
    {
      IEnumerable<AbstractModel> abstractModels = runHookSubscriber.del(runState);
      if (abstractModels != null)
      {
        IEnumerator<AbstractModel> enumerator = abstractModels.GetEnumerator();
        while (enumerator.MoveNext())
        {
          AbstractModel current = enumerator.Current;
          if (current != null)
            yield return current;
        }
        enumerator = (IEnumerator<AbstractModel>) null;
      }
    }
  }

  public static IEnumerable<AbstractModel> IterateAllCombatStateSubscribers(CombatState combatState)
  {
    foreach (ModHelper.ModCombatHookSubscriber combatHookSubscriber in ModHelper._combatHookSubscribers)
    {
      IEnumerable<AbstractModel> abstractModels = combatHookSubscriber.del(combatState);
      if (abstractModels != null)
      {
        IEnumerator<AbstractModel> enumerator = abstractModels.GetEnumerator();
        while (enumerator.MoveNext())
        {
          AbstractModel current = enumerator.Current;
          if (current != null)
            yield return current;
        }
        enumerator = (IEnumerator<AbstractModel>) null;
      }
    }
  }

  private class ModPoolContent
  {
    public bool isFrozen;
    public List<Type>? modelsToAdd;
  }

  private class ModRunHookSubscriber
  {
    public required string id;
    public required RunHookSubscriptionDelegate del;
  }

  private class ModCombatHookSubscriber
  {
    public required string id;
    public required CombatHookSubscriptionDelegate del;
  }
}
