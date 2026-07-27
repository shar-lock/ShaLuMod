// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Timeline.StoryModel
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.SourceGeneration;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Timeline;

[GenerateSubtypes]
public abstract class StoryModel
{
  private static readonly Dictionary<string, Type> _storyTypeDictionary = new Dictionary<string, Type>();

  protected abstract string Id { get; }

  public abstract EpochModel[] Epochs { get; }

  static StoryModel()
  {
    for (int i = 0; i < StoryModelSubtypes.Count; ++i)
    {
      Type type = StoryModelSubtypes.Get(i);
      StoryModel instance = (StoryModel) Activator.CreateInstance(type);
      StoryModel._storyTypeDictionary[instance.Id] = type;
    }
  }

  public static EpochModel? PrevChapter(EpochModel model)
  {
    if (model.StoryId == null)
      return (EpochModel) null;
    StoryModel storyModel = StoryModel.Get(StringHelper.Slugify(model.StoryId));
    for (int index1 = 0; index1 < storyModel.Epochs.Length; ++index1)
    {
      if (storyModel.Epochs[index1].Id == model.Id)
      {
        for (int index2 = index1 - 1; index2 >= 0; --index2)
        {
          EpochModel epoch = storyModel.Epochs[index2];
          if (SaveManager.Instance.IsEpochRevealed(epoch.Id))
            return epoch;
        }
        return (EpochModel) null;
      }
    }
    Log.Error($"Epoch: {model.Id} was not found in {storyModel.Id}");
    return (EpochModel) null;
  }

  public static EpochModel? NextChapter(EpochModel model)
  {
    if (model.StoryId == null)
      return (EpochModel) null;
    StoryModel storyModel = StoryModel.Get(StringHelper.Slugify(model.StoryId));
    for (int index1 = 0; index1 < storyModel.Epochs.Length; ++index1)
    {
      if (storyModel.Epochs[index1].Id == model.Id)
      {
        for (int index2 = index1 + 1; index2 < storyModel.Epochs.Length; ++index2)
        {
          EpochModel epoch = storyModel.Epochs[index2];
          if (SaveManager.Instance.IsEpochRevealed(epoch.Id))
            return epoch;
        }
        return (EpochModel) null;
      }
    }
    Log.Error($"Epoch: {model.Id} was not found in {storyModel.Id}");
    return (EpochModel) null;
  }

  public static StoryModel Get(string id)
  {
    Type type;
    if (StoryModel._storyTypeDictionary.TryGetValue(id, out type))
      return (StoryModel) Activator.CreateInstance(type);
    throw new ArgumentException($"Story with id '{id}' does not exist.");
  }
}
