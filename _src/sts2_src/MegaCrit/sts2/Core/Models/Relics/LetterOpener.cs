// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.LetterOpener
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class LetterOpener : RelicModel
{
  private bool _isActivating;
  private int _skillsPlayedThisTurn;

  public override RelicRarity Rarity => RelicRarity.Uncommon;

  public override bool ShowCounter => CombatManager.Instance.IsInProgress;

  public override int DisplayAmount
  {
    get
    {
      return !this.IsActivating ? this.SkillsPlayedThisTurn % this.DynamicVars.Cards.IntValue : this.DynamicVars.Cards.IntValue;
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new CardsVar(3),
        (DynamicVar) new DamageVar(5M, ValueProp.Unpowered)
      });
    }
  }

  private bool IsActivating
  {
    get => this._isActivating;
    set
    {
      this.AssertMutable();
      this._isActivating = value;
      this.UpdateDisplay();
    }
  }

  private int SkillsPlayedThisTurn
  {
    get => this._skillsPlayedThisTurn;
    set
    {
      this.AssertMutable();
      this._skillsPlayedThisTurn = value;
      this.UpdateDisplay();
    }
  }

  private void UpdateDisplay()
  {
    if (this.IsActivating)
    {
      this.Status = RelicStatus.Normal;
    }
    else
    {
      int intValue = this.DynamicVars.Cards.IntValue;
      this.Status = this.SkillsPlayedThisTurn % intValue == intValue - 1 ? RelicStatus.Active : RelicStatus.Normal;
    }
    this.InvokeDisplayAmountChanged();
  }

  public override Task BeforeCombatStart()
  {
    this.SkillsPlayedThisTurn = 0;
    this.Status = RelicStatus.Normal;
    return Task.CompletedTask;
  }

  public override Task AfterSideTurnStart(
    CombatSide side,
    IReadOnlyList<Creature> participants,
    ICombatState combatState)
  {
    if (!participants.Contains<Creature>(this.Owner.Creature) || this.Owner.PlayerCombatState.TurnNumber == 1)
      return Task.CompletedTask;
    this.SkillsPlayedThisTurn = 0;
    this.Status = RelicStatus.Normal;
    return Task.CompletedTask;
  }

  public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (cardPlay.Card.Owner != this.Owner || !CombatManager.Instance.IsInProgress || cardPlay.Card.Type != CardType.Skill)
      return;
    this.SkillsPlayedThisTurn++;
    if (this.SkillsPlayedThisTurn % this.DynamicVars.Cards.IntValue != 0)
      return;
    TaskHelper.RunSafely(this.DoActivateVisuals());
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, (IEnumerable<Creature>) this.Owner.Creature.CombatState.HittableEnemies, this.DynamicVars.Damage, this.Owner.Creature);
  }

  private async Task DoActivateVisuals()
  {
    this.IsActivating = true;
    this.Flash();
    await Cmd.Wait(1f);
    this.IsActivating = false;
  }

  public override Task AfterCombatEnd(CombatRoom _)
  {
    this.Status = RelicStatus.Normal;
    this.IsActivating = false;
    return Task.CompletedTask;
  }

  public int GetSkillsPlayedForTest() => this.SkillsPlayedThisTurn;
}
