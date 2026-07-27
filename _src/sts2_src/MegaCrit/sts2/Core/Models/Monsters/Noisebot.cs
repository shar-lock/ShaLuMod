// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.Noisebot
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class Noisebot : MonsterModel
{
  private const int _noiseStatusCount = 2;

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 19, 18);
  }

  public override int MaxInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 24, 23);
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
    MoveState initialState = new MoveState("NOISE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.NoiseMove), new AbstractIntent[1]
    {
      (AbstractIntent) new StatusIntent(2)
    });
    initialState.FollowUpState = (MonsterState) initialState;
    states.Add((MonsterState) initialState);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task NoiseMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play(this.CastSfx);
    await CreatureCmd.TriggerAnim(this.Creature, "Cast", 0.6f);
    foreach (Creature target in (IEnumerable<Creature>) targets)
    {
      Player player = target.Player ?? target.PetOwner;
      CardPileAddResult[] statusCards = new CardPileAddResult[2];
      CardModel card1 = (CardModel) this.CombatState.CreateCard<Dazed>(player);
      CardPileAddResult[] cardPileAddResultArray = statusCards;
      cardPileAddResultArray[0] = await CardPileCmd.AddGeneratedCardToCombat(card1, PileType.Discard, (Player) null);
      cardPileAddResultArray = (CardPileAddResult[]) null;
      CardModel card2 = (CardModel) this.CombatState.CreateCard<Dazed>(player);
      cardPileAddResultArray = statusCards;
      cardPileAddResultArray[1] = await CardPileCmd.AddGeneratedCardToCombat(card2, PileType.Draw, (Player) null, CardPilePosition.Random);
      cardPileAddResultArray = (CardPileAddResult[]) null;
      if (LocalContext.IsMe(player))
      {
        CardCmd.PreviewCardPileAdd((IReadOnlyList<CardPileAddResult>) statusCards);
        await Cmd.Wait(1f);
      }
      player = (Player) null;
      statusCards = (CardPileAddResult[]) null;
    }
  }
}
