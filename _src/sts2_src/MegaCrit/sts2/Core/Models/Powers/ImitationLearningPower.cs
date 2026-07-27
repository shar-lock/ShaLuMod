// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.ImitationLearningPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class ImitationLearningPower : PowerModel
{
  private const string _targetPlayerKey = "TargetPlayer";
  private Player? _playerTarget;

  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

  public Player PlayerTarget
  {
    get => this._playerTarget ?? throw new InvalidOperationException();
    set
    {
      this.AssertMutable();
      this._playerTarget = value;
      ((StringVar) this.DynamicVars["TargetPlayer"]).StringValue = PlatformUtil.GetPlayerName(RunManager.Instance.NetService.Platform, this._playerTarget.NetId);
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new StringVar("TargetPlayer"));
    }
  }

  protected override object InitInternalData() => (object) new ImitationLearningPower.Data();

  public override Task BeforeCardPlayed(CardPlay cardPlay)
  {
    if (this._playerTarget == null)
      throw new InvalidOperationException("ImitationLearningPower applied without a player target!");
    if (cardPlay.Card.Owner != this._playerTarget || cardPlay.Card.Type != CardType.Power)
      return Task.CompletedTask;
    CardModel cloneForPlayer = cardPlay.Card.CreateCloneForPlayer(this.Owner.Player);
    this.GetInternalData<ImitationLearningPower.Data>().cardsAndClones.Add(cardPlay.Card, cloneForPlayer);
    return Task.CompletedTask;
  }

  public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    CardModel clone;
    if (!this.GetInternalData<ImitationLearningPower.Data>().cardsAndClones.TryGetValue(cardPlay.Card, out clone))
    {
      clone = (CardModel) null;
    }
    else
    {
      this.Flash();
      await PowerCmd.Decrement((PowerModel) this);
      await CardCmd.AutoPlay(choiceContext, clone, (Creature) null);
      clone = (CardModel) null;
    }
  }

  private class Data
  {
    public readonly Dictionary<CardModel, CardModel> cardsAndClones = new Dictionary<CardModel, CardModel>();
  }
}
