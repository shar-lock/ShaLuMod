// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.GigantificationPower
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

public sealed class GigantificationPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  protected override object InitInternalData() => (object) new GigantificationPower.Data();

  public override Task BeforeAttack(AttackCommand command)
  {
    if (!(command.ModelSource is CardModel modelSource) || modelSource.Owner.Creature != this.Owner || modelSource.Type != CardType.Attack || !command.DamageProps.IsPoweredAttack())
      return Task.CompletedTask;
    GigantificationPower.Data internalData = this.GetInternalData<GigantificationPower.Data>();
    if (internalData.commandToModify != null)
      return Task.CompletedTask;
    internalData.commandToModify = command;
    return Task.CompletedTask;
  }

  public override Decimal ModifyDamageMultiplicative(
    Creature? target,
    Decimal amount,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource,
    CardPlay? cardPlay)
  {
    if (cardSource == null || cardSource.Owner.Creature != this.Owner || !props.IsPoweredAttack())
      return 1M;
    GigantificationPower.Data internalData = this.GetInternalData<GigantificationPower.Data>();
    return internalData.commandToModify != null && cardSource != internalData.commandToModify.ModelSource ? 1M : 3M;
  }

  public override async Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
  {
    GigantificationPower.Data internalData = this.GetInternalData<GigantificationPower.Data>();
    if (command != internalData.commandToModify)
      return;
    internalData.commandToModify = (AttackCommand) null;
    await PowerCmd.Decrement((PowerModel) this);
  }

  private class Data
  {
    public AttackCommand? commandToModify;
  }
}
