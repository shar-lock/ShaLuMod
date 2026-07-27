// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.GameActions.PlayCardAction
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.GameActions;

public sealed class PlayCardAction : GameAction
{
  private CardModel? _card;

  public override ulong OwnerId => this.Player.NetId;

  public override GameActionType ActionType => GameActionType.CombatPlayPhaseOnly;

  public Player Player { get; }

  public NetCombatCard NetCombatCard { get; }

  public ModelId CardModelId { get; }

  public uint? TargetId { get; }

  public PlayerChoiceContext? PlayerChoiceContext { get; private set; }

  public PlayCardAction(CardModel cardModel, Creature? target)
  {
    uint? nullable1;
    if (target != null)
    {
      nullable1 = target.CombatId;
      if (!nullable1.HasValue)
        throw new InvalidOperationException($"Cannot target card against target {target} with no combat ID!");
    }
    this.Player = cardModel.Owner;
    this.NetCombatCard = NetCombatCard.FromModel(cardModel);
    this.CardModelId = cardModel.Id;
    uint? nullable2;
    if (target == null)
    {
      nullable1 = new uint?();
      nullable2 = nullable1;
    }
    else
      nullable2 = target.CombatId;
    this.TargetId = nullable2;
  }

  public PlayCardAction(
    Player player,
    NetCombatCard netCombatCard,
    ModelId cardModelId,
    uint? targetId)
  {
    this.Player = player;
    this.NetCombatCard = netCombatCard;
    this.CardModelId = cardModelId;
    this.TargetId = targetId;
  }

  public Creature? Target => this.Player.Creature.CombatState?.GetCreature(this.TargetId);

  protected override async Task ExecuteAction()
  {
    this._card = this.NetCombatCard.ToCardModel();
    NCardPlayQueue.Instance?.UpdateCardBeforeExecution(this);
    Creature target = await this.Player.Creature.CombatState.GetCreatureAsync(this.TargetId, 10.0);
    CardPile pile = this._card.Pile;
    if ((pile != null ? (pile.Type != PileType.Hand ? 1 : 0) : 1) != 0)
    {
      NCardPlayQueue instance = NCardPlayQueue.Instance;
      if (instance == null)
      {
        target = (Creature) null;
      }
      else
      {
        instance.RemoveCardFromQueueForCancellation(this);
        target = (Creature) null;
      }
    }
    else
    {
      bool flag1 = target == null;
      if (flag1)
      {
        bool flag2;
        switch (this._card.TargetType)
        {
          case TargetType.AnyEnemy:
          case TargetType.AnyAlly:
            flag2 = true;
            break;
          default:
            flag2 = false;
            break;
        }
        flag1 = flag2;
      }
      if (flag1)
        Log.Warn($"Attempted to play card {this._card} with TargetType of type 'Any', but no target was passed to the play card action!");
      if (!this._card.CanPlay(out UnplayableReason _, out AbstractModel _) || !this._card.IsValidTarget(target))
      {
        this.Cancel();
        target = (Creature) null;
      }
      else
      {
        string str;
        if (target == null)
        {
          str = "no target";
        }
        else
        {
          DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 2);
          interpolatedStringHandler.AppendLiteral("targeting ");
          interpolatedStringHandler.AppendFormatted(target.LogName);
          interpolatedStringHandler.AppendLiteral(" (index ");
          ref DefaultInterpolatedStringHandler local = ref interpolatedStringHandler;
          ICombatState combatState = this.Player.Creature.CombatState;
          int? nullable = combatState != null ? new int?(combatState.Creatures.IndexOf<Creature>(target)) : new int?();
          local.AppendFormatted<int?>(nullable);
          interpolatedStringHandler.AppendLiteral(")");
          str = interpolatedStringHandler.ToStringAndClear();
        }
        Log.Info($"Player {this._card.Owner.NetId} playing card {this._card.Id.Entry} ({str})");
        (int num1, int num2) = await this._card.SpendResources();
        ResourceInfo resources = new ResourceInfo()
        {
          EnergySpent = num1,
          EnergyValue = num1,
          StarsSpent = num2,
          StarValue = num2
        };
        this.PlayerChoiceContext = (PlayerChoiceContext) new GameActionPlayerChoiceContext((GameAction) this);
        await this._card.OnPlayWrapper(this.PlayerChoiceContext, target, false, resources);
        target = (Creature) null;
      }
    }
  }

  protected override void CancelAction()
  {
    if (TestMode.IsOn && !RunManager.Instance.IsInProgress)
      return;
    if (this._card == null)
      this._card = this.NetCombatCard.ToCardModelOrNull();
    if (this._card == null)
      return;
    NCardPlayQueue.Instance?.RemoveCardFromQueueForCancellation(this);
    if (!LocalContext.IsMe(this.Player))
      return;
    NPlayerHand.Instance?.TryCancelCardPlay(this._card);
  }

  public override INetAction ToNetAction()
  {
    return (INetAction) new NetPlayCardAction()
    {
      card = this.NetCombatCard,
      modelId = this.CardModelId,
      targetId = this.TargetId
    };
  }

  public override string ToString()
  {
    return $"{nameof (PlayCardAction)} card: {this.NetCombatCard.ToCardModelOrNull()} index: {this.NetCombatCard.CombatCardIndex} targetid: {this.TargetId}";
  }
}
