// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.SkittishPower
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
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class SkittishPower : PowerModel
{
  private const string _extendSfx = "event:/sfx/enemy/enemy_attacks/phantasmal_gardeners/phantasmal_gardeners_extend";
  private const string _retractSfx = "event:/sfx/enemy/enemy_attacks/phantasmal_gardeners/phantasmal_gardeners_retract";

  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override bool ShouldScaleInMultiplayer => true;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block));
    }
  }

  public bool HasGainedBlockThisTurn
  {
    get => this.GetInternalData<SkittishPower.Data>().hasGainedBlockThisTurn;
    private set
    {
      this.AssertMutable();
      this.GetInternalData<SkittishPower.Data>().hasGainedBlockThisTurn = value;
    }
  }

  protected override object InitInternalData() => (object) new SkittishPower.Data();

  public override async Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
  {
    if (this.HasGainedBlockThisTurn || !command.DamageProps.HasFlag((Enum) ValueProp.Move) || !(command.ModelSource is CardModel))
      return;
    DamageResult damageResult = command.Results.SelectMany<List<DamageResult>, DamageResult>((Func<List<DamageResult>, IEnumerable<DamageResult>>) (r => (IEnumerable<DamageResult>) r)).FirstOrDefault<DamageResult>((Func<DamageResult, bool>) (r => r.Receiver == this.Owner));
    if (damageResult == null || damageResult.UnblockedDamage == 0)
      return;
    this.HasGainedBlockThisTurn = true;
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/phantasmal_gardeners/phantasmal_gardeners_retract");
    await CreatureCmd.TriggerAnim(this.Owner, "BlockStart", 0.3f);
    Decimal num = await CreatureCmd.GainBlock(this.Owner, (Decimal) this.Amount, ValueProp.Unpowered, (CardPlay) null);
  }

  public override async Task AfterSideTurnEnd(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    if (side == this.Owner.Side)
      return;
    if (this.HasGainedBlockThisTurn)
    {
      SfxCmd.Play("event:/sfx/enemy/enemy_attacks/phantasmal_gardeners/phantasmal_gardeners_extend");
      await CreatureCmd.TriggerAnim(this.Owner, "BlockEnd", 0.15f);
    }
    this.HasGainedBlockThisTurn = false;
  }

  public override Decimal GetScaledAmountForMultiplayer(
    ICombatState combatState,
    Creature? applier,
    Decimal amount,
    Creature target,
    CardModel? cardSource)
  {
    return amount * (1M + (Decimal) (combatState.Players.Count - 1) * 0.5M);
  }

  private class Data
  {
    public bool hasGainedBlockThisTurn;
  }
}
