// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.TuningFork
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class TuningFork : RelicModel
{
  private bool _isActivating;
  private int _skillsPlayed;

  public override RelicRarity Rarity => RelicRarity.Uncommon;

  public override bool ShowCounter => true;

  public override int DisplayAmount
  {
    get => !this.IsActivating ? this.SkillsPlayed : this.DynamicVars.Cards.IntValue;
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block));
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new CardsVar(10),
        (DynamicVar) new BlockVar(7M, ValueProp.Unpowered)
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

  [SavedProperty]
  public int SkillsPlayed
  {
    get => this._skillsPlayed;
    private set
    {
      this.AssertMutable();
      if (this._skillsPlayed == value)
        return;
      this._skillsPlayed = value;
      this.UpdateDisplay();
    }
  }

  private int SkillsThreshold => this.DynamicVars.Cards.IntValue;

  private void UpdateDisplay()
  {
    if (this.IsActivating)
      this.Status = RelicStatus.Normal;
    else
      this.Status = this.SkillsPlayed == this.SkillsThreshold - 1 ? RelicStatus.Active : RelicStatus.Normal;
    this.InvokeDisplayAmountChanged();
  }

  public void NotifySkillPlayed() => ++this.SkillsPlayed;

  public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (cardPlay.Card.Owner != this.Owner || cardPlay.Card.Type != CardType.Skill)
      return;
    this.SkillsPlayed++;
    if (this.SkillsPlayed < this.SkillsThreshold)
      return;
    TaskHelper.RunSafely(this.DoActivateVisuals());
    Decimal num = await CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars.Block, (CardPlay) null);
    this.SkillsPlayed -= this.SkillsThreshold;
  }

  private async Task DoActivateVisuals()
  {
    this.IsActivating = true;
    this.Flash();
    await Cmd.Wait(1f);
    this.IsActivating = false;
  }
}
