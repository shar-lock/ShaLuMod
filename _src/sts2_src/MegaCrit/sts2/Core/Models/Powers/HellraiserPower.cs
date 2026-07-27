// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.HellraiserPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class HellraiserPower : PowerModel
{
  private const int _infiniteAutoPlayCap = 9;

  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Single;

  protected override object InitInternalData() => (object) new HellraiserPower.Data();

  public override async Task AfterCardDrawnEarly(
    PlayerChoiceContext choiceContext,
    CardModel card,
    bool fromHandDraw)
  {
    HellraiserPower.Data data;
    if (card.Owner.Creature != this.Owner)
      data = (HellraiserPower.Data) null;
    else if (!card.Tags.Contains<CardTag>(CardTag.Strike))
    {
      data = (HellraiserPower.Data) null;
    }
    else
    {
      data = this.GetInternalData<HellraiserPower.Data>();
      bool flag = true;
      if (this.Owner.CombatState.HittableEnemies.All<Creature>((Func<Creature, bool>) (c => c.HpDisplay.IsInfinite())))
      {
        if (data.infiniteAutoPlaysThisTurn >= 9)
        {
          flag = false;
          if (!data.showedCapReachedMessage)
          {
            ThinkCmd.Play(new LocString("powers", "HELLRAISER_POWER.infiniteAutoPlayCapReached"), this.Owner);
            data.showedCapReachedMessage = true;
          }
        }
        ++data.infiniteAutoPlaysThisTurn;
      }
      else
        this.ResetInfiniteAutoPlayData();
      if (!flag)
      {
        data = (HellraiserPower.Data) null;
      }
      else
      {
        data.autoPlayingCards.Add(card);
        await CardCmd.AutoPlay(choiceContext, card, (Creature) null);
        data.autoPlayingCards.Remove(card);
        data = (HellraiserPower.Data) null;
      }
    }
  }

  public override Task AfterSideTurnEnd(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    if (!participants.Contains<Creature>(this.Owner))
      return Task.CompletedTask;
    this.ResetInfiniteAutoPlayData();
    return Task.CompletedTask;
  }

  public override Task BeforeAttack(AttackCommand command)
  {
    if (!((IEnumerable<AbstractModel>) this.GetInternalData<HellraiserPower.Data>().autoPlayingCards).Contains<AbstractModel>(command.ModelSource))
      return Task.CompletedTask;
    command.WithHitFx("vfx/hellraiser_attack_vfx", command.HitSfx, command.TmpHitSfx).WithAttackerAnim("Cast", command.Attacker.Player.Character.CastAnimDelay).SpawningHitVfxOnEachCreature().WithHitVfxSpawnedAtBase();
    return Task.CompletedTask;
  }

  private void ResetInfiniteAutoPlayData()
  {
    HellraiserPower.Data internalData = this.GetInternalData<HellraiserPower.Data>();
    internalData.infiniteAutoPlaysThisTurn = 0;
    internalData.showedCapReachedMessage = false;
  }

  private class Data
  {
    public readonly HashSet<CardModel> autoPlayingCards = new HashSet<CardModel>();
    public int infiniteAutoPlaysThisTurn;
    public bool showedCapReachedMessage;
  }
}
