// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.TemporaryStrengthPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public abstract class TemporaryStrengthPower : PowerModel, ITemporaryPower
{
  public override PowerType Type => !this.IsPositive ? PowerType.Debuff : PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public abstract AbstractModel OriginModel { get; }

  public PowerModel InternallyAppliedPower => (PowerModel) ModelDb.Power<StrengthPower>();

  protected virtual bool IsPositive => true;

  private int Sign => !this.IsPositive ? -1 : 1;

  public override LocString Title
  {
    get
    {
      switch (this.OriginModel)
      {
        case CardModel cardModel:
          return cardModel.TitleLocString;
        case PotionModel potionModel:
          return potionModel.Title;
        case RelicModel relicModel:
          return relicModel.Title;
        default:
          throw new InvalidOperationException();
      }
    }
  }

  public override LocString Description
  {
    get
    {
      return new LocString("powers", this.IsPositive ? "TEMPORARY_STRENGTH_POWER.description" : "TEMPORARY_STRENGTH_DOWN.description");
    }
  }

  protected override string SmartDescriptionLocKey
  {
    get
    {
      return !this.IsPositive ? "TEMPORARY_STRENGTH_DOWN.smartDescription" : "TEMPORARY_STRENGTH_POWER.smartDescription";
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      List<IHoverTip> items = new List<IHoverTip>();
      List<IHoverTip> hoverTipList = items;
      IEnumerable<IHoverTip> collection;
      switch (this.OriginModel)
      {
        case CardModel card:
          collection = (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard(card));
          break;
        case PotionModel _:
          collection = (IEnumerable<IHoverTip>) Array.Empty<IHoverTip>();
          break;
        case RelicModel relic:
          collection = HoverTipFactory.FromRelic(relic);
          break;
        default:
          throw new InvalidOperationException();
      }
      hoverTipList.AddRange(collection);
      items.Add(HoverTipFactory.FromPower<StrengthPower>());
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlyList<IHoverTip>(items);
    }
  }

  public override async Task BeforeApplied(
    Creature target,
    Decimal amount,
    Creature? applier,
    CardModel? cardSource)
  {
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), target, (Decimal) this.Sign * amount, applier, cardSource, true);
  }

  public override async Task AfterPowerAmountChanged(
    PlayerChoiceContext choiceContext,
    PowerModel power,
    Decimal amount,
    Creature? applier,
    CardModel? cardSource)
  {
    if (amount == (Decimal) this.Amount || power != this)
      return;
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>(choiceContext, this.Owner, (Decimal) this.Sign * amount, applier, cardSource, true);
  }

  public override async Task AfterSideTurnEnd(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    if (!participants.Contains<Creature>(this.Owner))
      return;
    this.Flash();
    await PowerCmd.Remove((PowerModel) this);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>(choiceContext, this.Owner, (Decimal) (-this.Sign * this.Amount), this.Owner, (CardModel) null);
  }
}
