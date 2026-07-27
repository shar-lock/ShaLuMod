// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.AutoSlay.Helpers.AutoSlayCardSelector
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.CardRewardAlternatives;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.AutoSlay.Helpers;

public class AutoSlayCardSelector : ICardSelector
{
  private readonly Rng _random;

  public AutoSlayCardSelector(Rng random) => this._random = random;

  public Task<IEnumerable<CardModel>> GetSelectedCards(
    IEnumerable<CardModel> options,
    int minSelect,
    int maxSelect)
  {
    List<CardModel> list = options.ToList<CardModel>();
    if (list.Count == 0)
      return Task.FromResult<IEnumerable<CardModel>>((IEnumerable<CardModel>) Array.Empty<CardModel>());
    int count = Math.Min(maxSelect, list.Count);
    if (count < minSelect)
      count = Math.Min(minSelect, list.Count);
    this._random.Shuffle<CardModel>((IList<CardModel>) list);
    IEnumerable<CardModel> cardModels = list.Take<CardModel>(count);
    AutoSlayLog.Info($"Auto-selected {count} card(s) for selection prompt");
    return Task.FromResult<IEnumerable<CardModel>>(cardModels);
  }

  public CardRewardSelection GetSelectedCardReward(
    IReadOnlyList<CardCreationResult> options,
    IReadOnlyList<CardRewardAlternative> alternatives)
  {
    if (options.Count == 0)
      return new CardRewardSelection();
    int index = this._random.NextInt(options.Count);
    return new CardRewardSelection()
    {
      card = options[index].Card,
      alternative = (CardRewardAlternative) null
    };
  }
}
