// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Combat.CombatStateTracker
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Combat;

public class CombatStateTracker
{
  private readonly CombatManager _combatManager;
  private Task? _combatStateChangedDeferredTask;
  private CombatState? _state;

  public event Action<CombatState>? CombatStateChanged;

  public CombatStateTracker(CombatManager combatManager)
  {
    this._combatManager = combatManager;
    this._combatManager.History.Changed += new Action(this.OnCombatHistoryChanged);
    this._combatManager.CombatBegan += new Action<CombatState>(this.OnCombatBegan);
    this._combatManager.CreaturesChanged += new Action<CombatState>(this.OnCreaturesChanged);
    this._combatManager.TurnStarted += new Action<CombatState>(this.OnTurnStarted);
    this._combatManager.TurnEnded += new Action<CombatState>(this.OnTurnEnded);
  }

  ~CombatStateTracker()
  {
    this._combatManager.History.Changed -= new Action(this.OnCombatHistoryChanged);
    this._combatManager.CombatBegan -= new Action<CombatState>(this.OnCombatBegan);
    this._combatManager.CreaturesChanged -= new Action<CombatState>(this.OnCreaturesChanged);
    this._combatManager.TurnStarted -= new Action<CombatState>(this.OnTurnStarted);
    this._combatManager.TurnEnded -= new Action<CombatState>(this.OnTurnEnded);
  }

  public void SetState(CombatState state) => this._state = state;

  public void Subscribe(CardModel card)
  {
    card.AfflictionChanged += new Action(this.OnCardValueChanged);
    card.EnchantmentChanged += new Action(this.OnCardValueChanged);
    card.EnergyCostChanged += new Action(this.OnCardValueChanged);
    card.ReplayCountChanged += new Action(this.OnCardValueChanged);
    card.Played += new Action(this.OnCardValueChanged);
    card.Drawn += new Action(this.OnCardValueChanged);
    card.StarCostChanged += new Action(this.OnCardValueChanged);
    card.Upgraded += new Action(this.OnCardValueChanged);
    card.Forged += new Action(this.OnCardValueChanged);
  }

  public void Unsubscribe(CardModel card)
  {
    card.AfflictionChanged -= new Action(this.OnCardValueChanged);
    card.EnchantmentChanged -= new Action(this.OnCardValueChanged);
    card.EnergyCostChanged -= new Action(this.OnCardValueChanged);
    card.ReplayCountChanged -= new Action(this.OnCardValueChanged);
    card.Played -= new Action(this.OnCardValueChanged);
    card.Drawn -= new Action(this.OnCardValueChanged);
    card.StarCostChanged -= new Action(this.OnCardValueChanged);
    card.Upgraded -= new Action(this.OnCardValueChanged);
    card.Forged -= new Action(this.OnCardValueChanged);
  }

  public void Subscribe(CardPile pile)
  {
    pile.ContentsChanged += new Action(this.OnCardPileContentsChanged);
  }

  public void Unsubscribe(CardPile pile)
  {
    pile.ContentsChanged -= new Action(this.OnCardPileContentsChanged);
  }

  public void Subscribe(Creature creature)
  {
    creature.BlockChanged += new Action<int, int>(this.OnCreatureValueChanged);
    creature.CurrentHpChanged += new Action<int, int>(this.OnCreatureValueChanged);
    creature.MaxHpChanged += new Action<int, int>(this.OnCreatureValueChanged);
    creature.PowerApplied += new Action<PowerModel>(this.OnPowerAppliedOrRemoved);
    creature.PowerIncreased += new Action<PowerModel, int, bool>(this.OnPowerIncreased);
    creature.PowerDecreased += new Action<PowerModel, bool>(this.OnPowerDecreased);
    creature.PowerRemoved += new Action<PowerModel>(this.OnPowerAppliedOrRemoved);
    creature.Died += new Action<Creature>(this.OnCreatureChanged);
  }

  public void Unsubscribe(Creature creature)
  {
    creature.BlockChanged -= new Action<int, int>(this.OnCreatureValueChanged);
    creature.CurrentHpChanged -= new Action<int, int>(this.OnCreatureValueChanged);
    creature.MaxHpChanged -= new Action<int, int>(this.OnCreatureValueChanged);
    creature.PowerApplied -= new Action<PowerModel>(this.OnPowerAppliedOrRemoved);
    creature.PowerIncreased -= new Action<PowerModel, int, bool>(this.OnPowerIncreased);
    creature.PowerDecreased -= new Action<PowerModel, bool>(this.OnPowerDecreased);
    creature.PowerRemoved -= new Action<PowerModel>(this.OnPowerAppliedOrRemoved);
    creature.Died -= new Action<Creature>(this.OnCreatureChanged);
  }

  public void Subscribe(PlayerCombatState combatState)
  {
    combatState.EnergyChanged += new Action<int, int>(this.OnPlayerCombatStateValueChanged);
    combatState.PlayerTurnPhaseChanged += new Action(this.OnPlayerStateChanged);
    combatState.StarsChanged += new Action<int, int>(this.OnPlayerCombatStateValueChanged);
  }

  public void Unsubscribe(PlayerCombatState combatState)
  {
    combatState.EnergyChanged -= new Action<int, int>(this.OnPlayerCombatStateValueChanged);
    combatState.PlayerTurnPhaseChanged -= new Action(this.OnPlayerStateChanged);
    combatState.StarsChanged -= new Action<int, int>(this.OnPlayerCombatStateValueChanged);
  }

  private void OnCombatBegan(CombatState _)
  {
    this.NotifyCombatStateChanged(nameof (OnCombatBegan));
  }

  private void OnCardPileContentsChanged()
  {
    this.NotifyCombatStateChanged(nameof (OnCardPileContentsChanged));
  }

  private void OnCardValueChanged() => this.NotifyCombatStateChanged(nameof (OnCardValueChanged));

  private void OnCombatHistoryChanged()
  {
    this.NotifyCombatStateChanged(nameof (OnCombatHistoryChanged));
  }

  private void OnCreatureValueChanged(int _, int __)
  {
    this.NotifyCombatStateChanged(nameof (OnCreatureValueChanged));
  }

  private void OnCreaturesChanged(CombatState _)
  {
    this.NotifyCombatStateChanged("OnCreatureChanged");
  }

  private void OnCreatureChanged(Creature _) => this.NotifyCombatStateChanged("OnCreaturesChanged");

  private void OnPlayerStateChanged()
  {
    this.NotifyCombatStateChanged(nameof (OnPlayerStateChanged));
  }

  private void OnPlayerCombatStateValueChanged(int _, int __)
  {
    this.NotifyCombatStateChanged(nameof (OnPlayerCombatStateValueChanged));
  }

  private void OnPowerAppliedOrRemoved(PowerModel _)
  {
    this.NotifyCombatStateChanged(nameof (OnPowerAppliedOrRemoved));
  }

  private void OnPowerDecreased(PowerModel _, bool __)
  {
    this.NotifyCombatStateChanged(nameof (OnPowerDecreased));
  }

  private void OnPowerIncreased(PowerModel _, int __, bool ___)
  {
    this.NotifyCombatStateChanged(nameof (OnPowerIncreased));
  }

  private void OnTurnStarted(CombatState _)
  {
    this.NotifyCombatStateChanged(nameof (OnTurnStarted));
  }

  private void OnTurnEnded(CombatState _) => this.NotifyCombatStateChanged(nameof (OnTurnEnded));

  private void NotifyCombatStateChanged(string caller)
  {
    if (TestMode.IsOn)
    {
      if (this.CombatStateChanged != null)
        throw new InvalidOperationException("Backend should not be subscribing to CombatStateChanged!");
    }
    else
    {
      if (this._combatStateChangedDeferredTask != null && !this._combatStateChangedDeferredTask.IsCompleted)
        return;
      this._combatStateChangedDeferredTask = TaskHelper.RunSafely(this.CallCombatStateChangedDeferred());
    }
  }

  private async Task CallCombatStateChangedDeferred()
  {
    Node instance = (Node) NRun.Instance;
    if ((instance != null ? instance.GetTreeOrNull() : (SceneTree) null) != null)
    {
      double num = (double) await instance.AwaitProcessFrame();
    }
    CombatState state = this._state;
    if (state == null)
      return;
    IReadOnlyList<Creature> creatures = state.Creatures;
    if (creatures == null || creatures.Count <= 0)
      return;
    LocalContext.GetMe((ICombatState) this._state)?.PlayerCombatState?.RecalculateCardValues();
    Action<CombatState> combatStateChanged = this.CombatStateChanged;
    if (combatStateChanged == null)
      return;
    combatStateChanged(this._state);
  }
}
