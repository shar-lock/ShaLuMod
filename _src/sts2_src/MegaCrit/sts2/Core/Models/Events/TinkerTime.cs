// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.TinkerTime
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class TinkerTime : EventModel
{
  private CardType _chosenCardType;

  private CardType ChosenCardType
  {
    get => this._chosenCardType;
    set
    {
      this.AssertMutable();
      this._chosenCardType = value;
    }
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlySingleElementList<EventOption>(new EventOption((EventModel) this, new Func<Task>(this.ChooseCardType), "TINKER_TIME.pages.INITIAL.options.CHOOSE_CARD_TYPE", Array.Empty<IHoverTip>()));
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[12]
      {
        (DynamicVar) new DamageVar(12M, ValueProp.Move),
        (DynamicVar) new BlockVar(8M, ValueProp.Move),
        (DynamicVar) new PowerVar<WeakPower>("SappingWeak", 2M),
        (DynamicVar) new PowerVar<VulnerablePower>("SappingVulnerable", 2M),
        new DynamicVar("ViolenceHits", 3M),
        (DynamicVar) new PowerVar<StranglePower>("ChokingDamage", 6M),
        (DynamicVar) new EnergyVar("EnergizedEnergy", 2),
        (DynamicVar) new CardsVar("WisdomCards", 3),
        (DynamicVar) new PowerVar<StrengthPower>("ExpertiseStrength", 2M),
        (DynamicVar) new PowerVar<DexterityPower>("ExpertiseDexterity", 2M),
        new DynamicVar("CuriousReduction", 1M),
        (DynamicVar) new EnergyVar("energyPrefix", 1)
      });
    }
  }

  private Task ChooseCardType()
  {
    // ISSUE: object of a compiler-generated type is created
    IEnumerable<EventOption> collection = (IEnumerable<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[3]
    {
      new EventOption((EventModel) this, new Func<Task>(this.Attack), "TINKER_TIME.pages.CHOOSE_CARD_TYPE.options.ATTACK", new IHoverTip[1]
      {
        (IHoverTip) this.GetCardTypeHoverTip(CardType.Attack)
      }),
      new EventOption((EventModel) this, new Func<Task>(this.Skill), "TINKER_TIME.pages.CHOOSE_CARD_TYPE.options.SKILL", new IHoverTip[1]
      {
        (IHoverTip) this.GetCardTypeHoverTip(CardType.Skill)
      }),
      new EventOption((EventModel) this, new Func<Task>(this.Power), "TINKER_TIME.pages.CHOOSE_CARD_TYPE.options.POWER", new IHoverTip[1]
      {
        (IHoverTip) this.GetCardTypeHoverTip(CardType.Power)
      })
    });
    this.SetEventState(this.L10NLookup("TINKER_TIME.pages.CHOOSE_CARD_TYPE.description"), collection.TakeRandom<EventOption>(2, this.Rng));
    return Task.CompletedTask;
  }

  private CardHoverTip GetCardTypeHoverTip(CardType cardType)
  {
    MadScience card = this.Owner.RunState.CreateCard<MadScience>(this.Owner);
    card.TinkerTimeType = cardType;
    card.TinkerTimeRider = TinkerTime.RiderEffect.None;
    return new CardHoverTip((CardModel) card);
  }

  private Task Attack()
  {
    this.ChosenCardType = CardType.Attack;
    return this.ChooseRiderEffect();
  }

  private Task Skill()
  {
    this.ChosenCardType = CardType.Skill;
    return this.ChooseRiderEffect();
  }

  private Task Power()
  {
    this.ChosenCardType = CardType.Power;
    return this.ChooseRiderEffect();
  }

  private Task ChooseRiderEffect()
  {
    IEnumerable<TinkerTime.RiderEffect> collection;
    switch (this.ChosenCardType)
    {
      case CardType.Attack:
        // ISSUE: object of a compiler-generated type is created
        collection = (IEnumerable<TinkerTime.RiderEffect>) new \u003C\u003Ez__ReadOnlyArray<TinkerTime.RiderEffect>(new TinkerTime.RiderEffect[3]
        {
          TinkerTime.RiderEffect.Sapping,
          TinkerTime.RiderEffect.Violence,
          TinkerTime.RiderEffect.Choking
        });
        break;
      case CardType.Skill:
        // ISSUE: object of a compiler-generated type is created
        collection = (IEnumerable<TinkerTime.RiderEffect>) new \u003C\u003Ez__ReadOnlyArray<TinkerTime.RiderEffect>(new TinkerTime.RiderEffect[3]
        {
          TinkerTime.RiderEffect.Energized,
          TinkerTime.RiderEffect.Wisdom,
          TinkerTime.RiderEffect.Chaos
        });
        break;
      case CardType.Power:
        // ISSUE: object of a compiler-generated type is created
        collection = (IEnumerable<TinkerTime.RiderEffect>) new \u003C\u003Ez__ReadOnlyArray<TinkerTime.RiderEffect>(new TinkerTime.RiderEffect[3]
        {
          TinkerTime.RiderEffect.Expertise,
          TinkerTime.RiderEffect.Curious,
          TinkerTime.RiderEffect.Improvement
        });
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
    List<TinkerTime.RiderEffect> riders = collection.TakeRandom<TinkerTime.RiderEffect>(2, this.Rng).ToList<TinkerTime.RiderEffect>();
    // ISSUE: object of a compiler-generated type is created
    this.SetEventState(this.L10NLookup("TINKER_TIME.pages.CHOOSE_RIDER.description"), (IEnumerable<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      new EventOption((EventModel) this, (Func<Task>) (() => this.RiderChosen(riders[0])), TinkerTime.GetRiderLocKey(riders[0]), new IHoverTip[1]
      {
        (IHoverTip) this.GetRiderHoverTip(riders[0])
      }),
      new EventOption((EventModel) this, (Func<Task>) (() => this.RiderChosen(riders[1])), TinkerTime.GetRiderLocKey(riders[1]), new IHoverTip[1]
      {
        (IHoverTip) this.GetRiderHoverTip(riders[1])
      })
    }));
    return Task.CompletedTask;
  }

  private CardHoverTip GetRiderHoverTip(TinkerTime.RiderEffect rider)
  {
    MadScience card = this.Owner.RunState.CreateCard<MadScience>(this.Owner);
    card.TinkerTimeType = this.ChosenCardType;
    card.TinkerTimeRider = rider;
    return new CardHoverTip((CardModel) card);
  }

  private async Task RiderChosen(TinkerTime.RiderEffect rider)
  {
    MadScience card = this.Owner.RunState.CreateCard<MadScience>(this.Owner);
    card.TinkerTimeType = this.ChosenCardType;
    card.TinkerTimeRider = rider;
    CardCmd.PreviewCardPileAdd(await CardPileCmd.Add((CardModel) card, PileType.Deck), 3f);
    this.SetEventFinished(this.L10NLookup("TINKER_TIME.pages.DONE.description"));
  }

  private static string GetRiderLocKey(TinkerTime.RiderEffect rider)
  {
    switch (rider)
    {
      case TinkerTime.RiderEffect.None:
        throw new ArgumentOutOfRangeException(nameof (rider), (object) rider, "None is not a valid rider");
      case TinkerTime.RiderEffect.Sapping:
        return "TINKER_TIME.pages.CHOOSE_RIDER.options.SAPPING";
      case TinkerTime.RiderEffect.Violence:
        return "TINKER_TIME.pages.CHOOSE_RIDER.options.VIOLENCE";
      case TinkerTime.RiderEffect.Choking:
        return "TINKER_TIME.pages.CHOOSE_RIDER.options.CHOKING";
      case TinkerTime.RiderEffect.Energized:
        return "TINKER_TIME.pages.CHOOSE_RIDER.options.ENERGIZED";
      case TinkerTime.RiderEffect.Wisdom:
        return "TINKER_TIME.pages.CHOOSE_RIDER.options.WISDOM";
      case TinkerTime.RiderEffect.Chaos:
        return "TINKER_TIME.pages.CHOOSE_RIDER.options.CHAOS";
      case TinkerTime.RiderEffect.Expertise:
        return "TINKER_TIME.pages.CHOOSE_RIDER.options.EXPERTISE";
      case TinkerTime.RiderEffect.Curious:
        return "TINKER_TIME.pages.CHOOSE_RIDER.options.CURIOUS";
      case TinkerTime.RiderEffect.Improvement:
        return "TINKER_TIME.pages.CHOOSE_RIDER.options.IMPROVEMENT";
      default:
        throw new ArgumentOutOfRangeException(nameof (rider), (object) rider, (string) null);
    }
  }

  public static IHoverTip[] GetRiderHoverTips(TinkerTime.RiderEffect rider)
  {
    switch (rider)
    {
      case TinkerTime.RiderEffect.None:
        return Array.Empty<IHoverTip>();
      case TinkerTime.RiderEffect.Sapping:
        return new IHoverTip[2]
        {
          HoverTipFactory.FromPower<WeakPower>(),
          HoverTipFactory.FromPower<VulnerablePower>()
        };
      case TinkerTime.RiderEffect.Violence:
        return Array.Empty<IHoverTip>();
      case TinkerTime.RiderEffect.Choking:
        return new IHoverTip[1]
        {
          HoverTipFactory.FromPower<StranglePower>()
        };
      case TinkerTime.RiderEffect.Energized:
        return Array.Empty<IHoverTip>();
      case TinkerTime.RiderEffect.Wisdom:
        return Array.Empty<IHoverTip>();
      case TinkerTime.RiderEffect.Chaos:
        return Array.Empty<IHoverTip>();
      case TinkerTime.RiderEffect.Expertise:
        return new IHoverTip[2]
        {
          HoverTipFactory.FromPower<StrengthPower>(),
          HoverTipFactory.FromPower<DexterityPower>()
        };
      case TinkerTime.RiderEffect.Curious:
        return Array.Empty<IHoverTip>();
      case TinkerTime.RiderEffect.Improvement:
        return Array.Empty<IHoverTip>();
      default:
        throw new ArgumentOutOfRangeException(nameof (rider), (object) rider, (string) null);
    }
  }

  public enum RiderEffect
  {
    None,
    Sapping,
    Violence,
    Choking,
    Energized,
    Wisdom,
    Chaos,
    Expertise,
    Curious,
    Improvement,
  }
}
