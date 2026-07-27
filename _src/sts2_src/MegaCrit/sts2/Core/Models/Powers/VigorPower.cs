// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.VigorPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class VigorPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  protected override object InitInternalData() => (object) new VigorPower.Data();

  public override Task BeforeAttack(AttackCommand command)
  {
    if (command.Attacker != this.Owner || !command.DamageProps.IsPoweredAttack())
      return Task.CompletedTask;
    VigorPower.Data internalData = this.GetInternalData<VigorPower.Data>();
    if (internalData.commandToModify != null || command.ModelSource != null && !(command.ModelSource is CardModel) || !command.DamageProps.IsPoweredAttack())
      return Task.CompletedTask;
    internalData.commandToModify = command;
    internalData.amountWhenAttackStarted = this.Amount;
    return Task.CompletedTask;
  }

  public override Decimal ModifyDamageAdditive(
    Creature? target,
    Decimal amount,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource,
    CardPlay? cardPlay)
  {
    if (this.Owner != dealer || !props.IsPoweredAttack())
      return 0M;
    VigorPower.Data internalData = this.GetInternalData<VigorPower.Data>();
    return internalData.commandToModify != null && cardSource != null && cardSource != internalData.commandToModify.ModelSource || internalData.commandToModify != null && internalData.commandToModify.Attacker != dealer ? 0M : (Decimal) this.Amount;
  }

  public override async Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
  {
    VigorPower.Data internalData = this.GetInternalData<VigorPower.Data>();
    if (command != internalData.commandToModify)
      return;
    int num = await PowerCmd.ModifyAmount(choiceContext, (PowerModel) this, (Decimal) -internalData.amountWhenAttackStarted, (Creature) null, (CardModel) null);
  }

  private class Data
  {
    public AttackCommand? commandToModify;
    public int amountWhenAttackStarted;
  }
}
