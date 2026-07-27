// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Commands.Builders.AttackContext
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Commands.Builders;

public sealed class AttackContext : IAsyncDisposable
{
  private readonly ICombatState _combatState;
  private readonly PlayerChoiceContext _choiceContext;
  private readonly AttackCommand _attackCommand;
  private bool _disposed;

  private AttackContext(
    ICombatState combatState,
    PlayerChoiceContext choiceContext,
    CardPlay cardPlay)
  {
    this._combatState = combatState;
    this._choiceContext = choiceContext;
    this._attackCommand = new AttackCommand(0M).FromCard(cardPlay.Card, cardPlay).TargetingAllOpponents(combatState);
  }

  public static async Task<AttackContext> CreateAsync(
    ICombatState combatState,
    PlayerChoiceContext choiceContext,
    CardPlay cardPlay)
  {
    AttackContext context = new AttackContext(combatState, choiceContext, cardPlay);
    await Hook.BeforeAttack(combatState, context._attackCommand);
    AttackContext async = context;
    context = (AttackContext) null;
    return async;
  }

  public void AddHit(IEnumerable<DamageResult> results)
  {
    this._attackCommand.IncrementHitsInternal();
    this._attackCommand.AddResultsInternal(results);
  }

  public async ValueTask DisposeAsync()
  {
    if (this._disposed)
      return;
    this._disposed = true;
    try
    {
      await Hook.AfterAttack(this._combatState, this._choiceContext, this._attackCommand);
    }
    catch (Exception ex)
    {
      Log.Error(ex.ToString());
    }
  }
}
