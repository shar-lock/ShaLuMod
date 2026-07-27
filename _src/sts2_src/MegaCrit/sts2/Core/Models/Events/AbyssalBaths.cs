// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.AbyssalBaths
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class AbyssalBaths : EventModel
{
  private const int _baseDamage = 3;
  private const int _damageScaling = 1;
  private int _lingerCount;

  private int LingerCount
  {
    get => this._lingerCount;
    set
    {
      this.AssertMutable();
      this._lingerCount = value;
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[3]
      {
        (DynamicVar) new MaxHpVar(2M),
        (DynamicVar) new DamageVar(3M, ValueProp.Unblockable | ValueProp.Unpowered),
        (DynamicVar) new HealVar(10M)
      });
    }
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      new EventOption((EventModel) this, new Func<Task>(this.Immerse), "ABYSSAL_BATHS.pages.INITIAL.options.IMMERSE", Array.Empty<IHoverTip>()).ThatDoesDamage((Decimal) this.DynamicVars.Damage.IntValue - this.DynamicVars.MaxHp.BaseValue),
      new EventOption((EventModel) this, new Func<Task>(this.Abstain), "ABYSSAL_BATHS.pages.INITIAL.options.ABSTAIN", Array.Empty<IHoverTip>())
    });
  }

  private async Task Immerse()
  {
    await this.OnImmerse();
    // ISSUE: object of a compiler-generated type is created
    this.SetEventState(this.L10NLookup("ABYSSAL_BATHS.pages.IMMERSE.description"), (IEnumerable<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      new EventOption((EventModel) this, new Func<Task>(this.Linger), "ABYSSAL_BATHS.pages.ALL.options.LINGER", Array.Empty<IHoverTip>()).ThatDoesDamage((Decimal) this.DynamicVars.Damage.IntValue - this.DynamicVars.MaxHp.BaseValue),
      new EventOption((EventModel) this, new Func<Task>(this.ExitBaths), "ABYSSAL_BATHS.pages.ALL.options.EXIT_BATHS", Array.Empty<IHoverTip>())
    }));
  }

  private async Task Abstain()
  {
    await CreatureCmd.Heal(this.Owner.Creature, (Decimal) this.DynamicVars.Heal.IntValue);
    this.SetEventFinished(this.L10NLookup("ABYSSAL_BATHS.pages.ABSTAIN.description"));
  }

  private async Task Linger()
  {
    this.LingerCount++;
    if (this.LingerCount > 9)
      this.LingerCount = 9;
    await this.OnImmerse();
    Decimal damage = (Decimal) this.DynamicVars.Damage.IntValue - this.DynamicVars.MaxHp.BaseValue;
    if (this.WillKillPlayer(damage))
    {
      // ISSUE: object of a compiler-generated type is created
      this.SetEventState(this.L10NLookup("ABYSSAL_BATHS.pages.DEATH_WARNING.description"), (IEnumerable<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
      {
        new EventOption((EventModel) this, new Func<Task>(this.Linger), "ABYSSAL_BATHS.pages.ALL.options.LINGER", Array.Empty<IHoverTip>()).ThatDoesDamage(damage),
        new EventOption((EventModel) this, new Func<Task>(this.ExitBaths), "ABYSSAL_BATHS.pages.ALL.options.EXIT_BATHS", Array.Empty<IHoverTip>())
      }));
    }
    else
    {
      // ISSUE: object of a compiler-generated type is created
      this.SetEventState(this.L10NLookup($"ABYSSAL_BATHS.pages.LINGER{this.LingerCount}.description"), (IEnumerable<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
      {
        new EventOption((EventModel) this, new Func<Task>(this.Linger), "ABYSSAL_BATHS.pages.ALL.options.LINGER", Array.Empty<IHoverTip>()).ThatDoesDamage(damage),
        new EventOption((EventModel) this, new Func<Task>(this.ExitBaths), "ABYSSAL_BATHS.pages.ALL.options.EXIT_BATHS", Array.Empty<IHoverTip>())
      }));
    }
  }

  private bool WillKillPlayer(Decimal damage) => (Decimal) this.Owner.Creature.CurrentHp <= damage;

  private Task ExitBaths()
  {
    this.SetEventFinished(this.L10NLookup("ABYSSAL_BATHS.pages.EXIT_BATHS.description"));
    return Task.CompletedTask;
  }

  private async Task OnImmerse()
  {
    await CreatureCmd.GainMaxHp(this.Owner.Creature, this.DynamicVars.MaxHp.BaseValue);
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Owner.Creature, this.DynamicVars.Damage, (Creature) null, (CardModel) null, (CardPlay) null);
    DamageVar damage = this.DynamicVars.Damage;
    damage.BaseValue = damage.BaseValue + 1M;
  }
}
