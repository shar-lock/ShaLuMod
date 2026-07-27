// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.TabletOfTruth
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class TabletOfTruth : EventModel
{
  private const string _smashHpGainKey = "SmashHPGain";
  private const string _decipherHpLossKey = "DecipherMaxHpLoss";
  private int _decipherCount;

  private int DecipherCount
  {
    get => this._decipherCount;
    set
    {
      this.AssertMutable();
      this._decipherCount = value;
    }
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      new EventOption((EventModel) this, new Func<Task>(this.Decipher), "TABLET_OF_TRUTH.pages.INITIAL.options.DECIPHER_1", Array.Empty<IHoverTip>()).ThatWillKillPlayerIf((Func<Player, bool>) (p => (Decimal) p.Creature.MaxHp <= this.DynamicVars["DecipherMaxHpLoss"].BaseValue)),
      new EventOption((EventModel) this, new Func<Task>(this.Smash), "TABLET_OF_TRUTH.pages.INITIAL.options.SMASH", Array.Empty<IHoverTip>())
    });
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        new DynamicVar("SmashHPGain", 20M),
        new DynamicVar("DecipherMaxHpLoss", 3M)
      });
    }
  }

  private async Task Smash()
  {
    await CreatureCmd.Heal(this.Owner.Creature, this.DynamicVars["SmashHPGain"].BaseValue);
    this.SetEventFinished(this.L10NLookup("TABLET_OF_TRUTH.pages.SMASH.description"));
  }

  private async Task Decipher()
  {
    await this.LoseMaxHpAndUpgrade(this.DynamicVars["DecipherMaxHpLoss"].BaseValue);
    this.DecipherCount++;
    if (this.DecipherCount == 5)
    {
      this.SetEventFinished(this.L10NLookup($"TABLET_OF_TRUTH.pages.DECIPHER_{this.DecipherCount}.description"));
    }
    else
    {
      this.DynamicVars["DecipherMaxHpLoss"].BaseValue = (Decimal) this.GetDecipherCost();
      // ISSUE: object of a compiler-generated type is created
      this.SetEventState(this.L10NLookup($"TABLET_OF_TRUTH.pages.DECIPHER_{this.DecipherCount}.description"), (IEnumerable<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
      {
        new EventOption((EventModel) this, new Func<Task>(this.Decipher), $"TABLET_OF_TRUTH.pages.DECIPHER_{this.DecipherCount}.options.DECIPHER", Array.Empty<IHoverTip>()).ThatWillKillPlayerIf((Func<Player, bool>) (p => (Decimal) p.Creature.MaxHp <= this.DynamicVars["DecipherMaxHpLoss"].BaseValue)),
        new EventOption((EventModel) this, new Func<Task>(this.GiveUp), "TABLET_OF_TRUTH.pages.DECIPHER.options.GIVE_UP", Array.Empty<IHoverTip>())
      }));
    }
  }

  public int GetDecipherCost()
  {
    Player owner = this.Owner;
    switch (this.DecipherCount)
    {
      case 1:
        return 6;
      case 2:
        return 12;
      case 3:
        return 24;
      case 4:
        return owner.Creature.MaxHp - 1;
      default:
        Log.Error($"DecipherCount: {this.DecipherCount} should not be called.");
        return 999999999;
    }
  }

  private Task GiveUp()
  {
    this.SetEventFinished(this.L10NLookup("TABLET_OF_TRUTH.pages.GIVE_UP.description"));
    return Task.CompletedTask;
  }

  private async Task LoseMaxHpAndUpgrade(Decimal hp)
  {
    if (hp < (Decimal) this.Owner.Creature.MaxHp)
    {
      await CreatureCmd.LoseMaxHp((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Owner.Creature, hp, false);
      List<CardModel> list = PileType.Deck.GetPile(this.Owner).Cards.Where<CardModel>((Func<CardModel, bool>) (c => c.IsUpgradable)).ToList<CardModel>();
      if (this._decipherCount == 4)
      {
        foreach (CardModel card in list)
        {
          CardCmd.Upgrade(card, CardPreviewStyle.MessyLayout);
          await Cmd.CustomScaledWait(0.1f, 0.2f);
        }
        await Cmd.CustomScaledWait(0.6f, 1.2f);
      }
      else
      {
        if (list.Count == 0)
          return;
        CardCmd.Upgrade(this.Rng.NextItem<CardModel>((IEnumerable<CardModel>) list), CardPreviewStyle.EventLayout);
      }
    }
    else
    {
      await CreatureCmd.LoseMaxHp((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Owner.Creature, (Decimal) (this.Owner.Creature.MaxHp - 1), false);
      await CreatureCmd.Kill(this.Owner.Creature);
    }
  }
}
