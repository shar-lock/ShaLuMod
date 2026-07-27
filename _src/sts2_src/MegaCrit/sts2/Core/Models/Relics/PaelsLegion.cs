// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.PaelsLegion
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class PaelsLegion : RelicModel
{
  private const string _turnsKey = "Turns";
  private string _skin = PaelsLegion.SkinOptions[0];
  private int _cooldown;
  private bool _triggeredBlockLastTurn;
  private CardPlay? _affectedCardPlay;

  public override bool AddsPet => true;

  public override RelicRarity Rarity => RelicRarity.Ancient;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block));
    }
  }

  public override bool ShowCounter => this.DisplayAmount > 0;

  public static string[] SkinOptions
  {
    get
    {
      return new string[4]
      {
        "eyes",
        "horns",
        "spikes",
        "wings"
      };
    }
  }

  [SavedProperty]
  public string Skin
  {
    get => this._skin;
    set
    {
      this.AssertMutable();
      this._skin = value;
    }
  }

  public override int DisplayAmount
  {
    get
    {
      return !CombatManager.Instance.IsInProgress || this.IsCanonical || this._cooldown <= 0 ? -1 : this._cooldown;
    }
  }

  private int Cooldown
  {
    get => this._cooldown;
    set
    {
      this.AssertMutable();
      this._cooldown = value;
      this.InvokeDisplayAmountChanged();
    }
  }

  private bool TriggeredBlockLastTurn
  {
    get => this._triggeredBlockLastTurn;
    set
    {
      this.AssertMutable();
      this._triggeredBlockLastTurn = value;
      this.InvokeDisplayAmountChanged();
    }
  }

  private CardPlay? AffectedCardPlay
  {
    get => this._affectedCardPlay;
    set
    {
      this.AssertMutable();
      this._affectedCardPlay = value;
      this.InvokeDisplayAmountChanged();
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Turns", 2M));
    }
  }

  public override async Task AfterObtained()
  {
    this.Skin = new Rng(this.Owner, this.Id).NextItem<string>((IEnumerable<string>) PaelsLegion.SkinOptions);
    if (!CombatManager.Instance.IsInProgress)
      return;
    await this.SummonPet();
  }

  public override async Task BeforeCombatStart() => await this.SummonPet();

  public override Decimal ModifyBlockMultiplicative(
    Creature target,
    Decimal block,
    ValueProp props,
    CardModel? cardSource,
    CardPlay? cardPlay)
  {
    return !props.IsCardOrMonsterMove() || cardSource == null || cardSource.Owner != this.Owner || this.Cooldown > 0 ? 1M : 2M;
  }

  public override Task AfterModifyingBlockAmount(
    Decimal modifiedAmount,
    CardModel? cardSource,
    CardPlay? cardPlay)
  {
    if (modifiedAmount <= 0M || cardPlay == null || this.AffectedCardPlay != null && this.AffectedCardPlay != cardPlay)
      return Task.CompletedTask;
    this.AffectedCardPlay = cardPlay;
    return Task.CompletedTask;
  }

  public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (this.AffectedCardPlay == null || this.AffectedCardPlay != cardPlay)
      return;
    this.Flash();
    this.AffectedCardPlay = (CardPlay) null;
    this.Cooldown = this.DynamicVars["Turns"].IntValue;
    this.Status = RelicStatus.Normal;
    await CreatureCmd.TriggerAnim(this.Owner.PlayerCombatState.GetPet<MegaCrit.Sts2.Core.Models.Monsters.PaelsLegion>().Monster.Creature, "BlockTrigger", 0.15f);
    this.TriggeredBlockLastTurn = true;
  }

  public override async Task AfterSideTurnStart(
    CombatSide side,
    IReadOnlyList<Creature> participants,
    ICombatState combatState)
  {
    if (!participants.Contains<Creature>(this.Owner.Creature))
      return;
    bool flag = this.Cooldown > 0;
    this.Cooldown--;
    if (this.Cooldown <= 0)
    {
      this.Status = RelicStatus.Active;
      this.InvokeDisplayAmountChanged();
    }
    MegaCrit.Sts2.Core.Models.Monsters.PaelsLegion monster = (MegaCrit.Sts2.Core.Models.Monsters.PaelsLegion) this.Owner.PlayerCombatState.GetPet<MegaCrit.Sts2.Core.Models.Monsters.PaelsLegion>().Monster;
    if (this.Cooldown > 0 && this.TriggeredBlockLastTurn)
      await CreatureCmd.TriggerAnim(monster.Creature, "SleepTrigger", 0.15f);
    else if (this.Cooldown <= 0 & flag)
      await CreatureCmd.TriggerAnim(monster.Creature, "WakeUpTrigger", 0.15f);
    this.TriggeredBlockLastTurn = false;
  }

  public override Task AfterCombatEnd(CombatRoom _)
  {
    this.Status = RelicStatus.Normal;
    this.Cooldown = 0;
    this.TriggeredBlockLastTurn = false;
    this.AffectedCardPlay = (CardPlay) null;
    this.InvokeDisplayAmountChanged();
    return Task.CompletedTask;
  }

  private async Task SummonPet()
  {
    Creature creature = await PlayerCmd.AddPet<MegaCrit.Sts2.Core.Models.Monsters.PaelsLegion>(this.Owner);
  }
}
