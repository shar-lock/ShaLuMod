// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.Guardbot
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class Guardbot : MonsterModel
{
  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 17, 16 /*0x10*/);
  }

  public override int MaxInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 21, 20);
  }

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Armor;

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    if (!TestMode.IsOff)
      return;
    FabricatorNormal.SetBotFallPosition(NCombatRoom.Instance.GetCreatureNode(this.Creature));
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("GUARD_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.GuardMove), new AbstractIntent[1]
    {
      (AbstractIntent) new DefendIntent()
    });
    initialState.FollowUpState = (MonsterState) initialState;
    states.Add((MonsterState) initialState);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task GuardMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play(this.CastSfx);
    await CreatureCmd.TriggerAnim(this.Creature, "Cast", 0.6f);
    foreach (Creature creature in this.CombatState.Enemies.Where<Creature>((Func<Creature, bool>) (c => c.Monster is Fabricator)).ToList<Creature>())
    {
      Decimal num = await CreatureCmd.GainBlock(creature, 15M, ValueProp.Unpowered, (CardPlay) null);
    }
  }
}
