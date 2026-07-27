// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.TestSupport.TestCardSelector
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.CardRewardAlternatives;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.TestSupport;

public class TestCardSelector : ICardSelector
{
  private readonly Queue<TaskCompletionSource<IEnumerable<CardModel>>> _cardsToSelectTaskQueue = new Queue<TaskCompletionSource<IEnumerable<CardModel>>>();
  private readonly Queue<TaskCompletionSource<IEnumerable<int>>> _indicesToSelectTaskQueue = new Queue<TaskCompletionSource<IEnumerable<int>>>();
  private TestCardSelector.CardRewardSelectionDelegate? _cardRewardSelectionDelegate;
  private bool _shouldBlock;
  private CancellationTokenSource? _blockCancelSource;

  public TaskCompletionSource? BlockedTcs { get; private set; }

  public void Cleanup()
  {
    this._cardsToSelectTaskQueue.Clear();
    this._indicesToSelectTaskQueue.Clear();
    this._shouldBlock = false;
    this.BlockedTcs = (TaskCompletionSource) null;
    this._blockCancelSource?.Cancel();
    this._blockCancelSource = (CancellationTokenSource) null;
    this._cardRewardSelectionDelegate = (TestCardSelector.CardRewardSelectionDelegate) null;
  }

  public TaskCompletionSource<IEnumerable<CardModel>> SetupForAsyncCardSelection()
  {
    TaskCompletionSource<IEnumerable<CardModel>> completionSource = new TaskCompletionSource<IEnumerable<CardModel>>();
    this._cardsToSelectTaskQueue.Enqueue(completionSource);
    return completionSource;
  }

  public TaskCompletionSource<IEnumerable<int>> SetupForAsyncIndexSelection()
  {
    TaskCompletionSource<IEnumerable<int>> completionSource = new TaskCompletionSource<IEnumerable<int>>();
    this._indicesToSelectTaskQueue.Enqueue(completionSource);
    return completionSource;
  }

  public void PrepareToSelect(IEnumerable<CardModel> cards)
  {
    TaskCompletionSource<IEnumerable<CardModel>> completionSource = new TaskCompletionSource<IEnumerable<CardModel>>();
    completionSource.SetResult(cards);
    this._cardsToSelectTaskQueue.Enqueue(completionSource);
  }

  public void PrepareToSelect(IEnumerable<int> indices)
  {
    TaskCompletionSource<IEnumerable<int>> completionSource = new TaskCompletionSource<IEnumerable<int>>();
    completionSource.SetResult(indices);
    this._indicesToSelectTaskQueue.Enqueue(completionSource);
  }

  public void PrepareToSelectCardReward(TestCardSelector.CardRewardSelectionDelegate del)
  {
    this._cardRewardSelectionDelegate = del;
  }

  public void PrepareToSelectCardRewardAtIndex(int index)
  {
    this.PrepareToSelectCardReward((TestCardSelector.CardRewardSelectionDelegate) ((c, _) => new CardRewardSelection()
    {
      card = c[index].Card
    }));
  }

  public void PrepareToSelectCardRewardAlternativeAtIndex(int index)
  {
    this.PrepareToSelectCardReward((TestCardSelector.CardRewardSelectionDelegate) ((_, a) => new CardRewardSelection()
    {
      alternative = a[index]
    }));
  }

  public CardRewardSelection GetSelectedCardReward(
    IReadOnlyList<CardCreationResult> options,
    IReadOnlyList<CardRewardAlternative> alternatives)
  {
    if (this._cardRewardSelectionDelegate != null)
      return this._cardRewardSelectionDelegate(options, alternatives);
    return new CardRewardSelection()
    {
      card = options.FirstOrDefault<CardCreationResult>()?.Card
    };
  }

  public Task PrepareToBlock()
  {
    this._shouldBlock = true;
    this.BlockedTcs = new TaskCompletionSource();
    this._blockCancelSource = new CancellationTokenSource();
    return this.BlockedTcs.Task;
  }

  public async Task<IEnumerable<CardModel>> GetSelectedCards(
    IEnumerable<CardModel> options,
    int minSelect,
    int maxSelect)
  {
    if (this._shouldBlock)
    {
      this.BlockedTcs.SetResult();
      await Task.Delay(5000, this._blockCancelSource.Token);
      throw new InvalidOperationException("Test told us to block, but it did not finish within 5 seconds!");
    }
    if (this._cardsToSelectTaskQueue.Count > 0)
    {
      IEnumerable<CardModel> task = await this._cardsToSelectTaskQueue.Dequeue().Task;
      if (task.Any<CardModel>((Func<CardModel, bool>) (c => !options.Contains<CardModel>(c))))
        throw new InvalidOperationException("Selected card missing from options.");
      return task;
    }
    return this._indicesToSelectTaskQueue.Count > 0 ? (await this._indicesToSelectTaskQueue.Dequeue().Task).Select<int, CardModel>(new Func<int, CardModel>(((Enumerable) options).ElementAt<CardModel>)) : (IEnumerable<CardModel>) Array.Empty<CardModel>();
  }

  public delegate CardRewardSelection CardRewardSelectionDelegate(
    IReadOnlyList<CardCreationResult> options,
    IReadOnlyList<CardRewardAlternative> alternatives);
}
