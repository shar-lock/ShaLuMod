// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.InterceptPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class InterceptPower : PowerModel
{
  private const string _coveringKey = "Covering";

  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Single;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new StringVar("Covering"));
    }
  }

  protected override object InitInternalData() => (object) new InterceptPower.Data();

  public void AddCoveredCreature(Creature c)
  {
    List<Creature> coveredCreatures = this.GetInternalData<InterceptPower.Data>().coveredCreatures;
    if (!this.GetInternalData<InterceptPower.Data>().coveredCreatures.Contains(c))
      coveredCreatures.Add(c);
    StringVar dynamicVar = (StringVar) this.DynamicVars["Covering"];
    dynamicVar.StringValue = "";
    for (int index = 0; index < coveredCreatures.Count; ++index)
    {
      dynamicVar.StringValue += PlatformUtil.GetPlayerName(RunManager.Instance.NetService.Platform, coveredCreatures[index].Player.NetId);
      if (index == coveredCreatures.Count - 2)
        dynamicVar.StringValue += ", and ";
      else if (index < coveredCreatures.Count - 2)
        dynamicVar.StringValue += ", ";
    }
  }

  public override Decimal ModifyDamageMultiplicative(
    Creature? target,
    Decimal amount,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource,
    CardPlay? cardPlay)
  {
    return target != this.Owner || !props.IsPoweredAttack() ? 1M : (Decimal) (this.GetInternalData<InterceptPower.Data>().coveredCreatures.Count + 1);
  }

  public override async Task AfterSideTurnEnd(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    if (side != CombatSide.Enemy)
      return;
    await PowerCmd.Remove((PowerModel) this);
  }

  private class Data
  {
    public readonly List<Creature> coveredCreatures = new List<Creature>();
  }
}
