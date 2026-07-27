// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Singleton.MultiplayerScalingModel
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Singleton;

public class MultiplayerScalingModel : SingletonModel
{
  private RunState _runState;
  private CombatState? _combatState;

  public override bool ShouldReceiveCombatHooks => true;

  public void Initialize(RunState state)
  {
    this._runState = this._runState == null ? state : throw new InvalidOperationException("Already initialized");
  }

  public void OnCombatEntered(CombatState combatState) => this._combatState = combatState;

  public void OnCombatFinished() => this._combatState = (CombatState) null;

  public override Decimal ModifyBlockMultiplicative(
    Creature target,
    Decimal block,
    ValueProp props,
    CardModel? cardSource,
    CardPlay? cardPlay)
  {
    if (target != null && !target.IsPrimaryEnemy && !target.IsSecondaryEnemy || !props.IsPoweredCardOrMonsterMoveBlock())
      return 1M;
    int count = this._runState.Players.Count;
    return count <= 2 ? (Decimal) count : (Decimal) count * MultiplayerScalingModel.GetMultiplayerScaling(this._combatState.Encounter, this._runState.CurrentActIndex);
  }

  public static Decimal GetMultiplayerScaling(EncounterModel? encounter, int actIndex)
  {
    switch (actIndex)
    {
      case 0:
        return 1.1M;
      case 1:
        return 1.2M;
      case 2:
        return encounter != null && encounter.RoomType == RoomType.Boss ? 1.3M : 1.2M;
      default:
        throw new ArgumentOutOfRangeException(nameof (actIndex), (object) actIndex, "Invalid act index for HP scaling");
    }
  }
}
