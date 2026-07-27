// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.GeneticAlgorithm
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class GeneticAlgorithm : CardModel
{
  private const string _increaseKey = "Increase";
  private int _currentBlock = 1;
  private int _increasedBlock;

  public GeneticAlgorithm()
    : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
  {
  }

  public override bool GainsBlock => true;

  [SavedProperty]
  public int CurrentBlock
  {
    get => this._currentBlock;
    set
    {
      this.AssertMutable();
      this._currentBlock = value;
      this.DynamicVars.Block.BaseValue = (Decimal) this._currentBlock;
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new BlockVar((Decimal) this.CurrentBlock, ValueProp.Move),
        (DynamicVar) new IntVar("Increase", 3M)
      });
    }
  }

  public override IEnumerable<CardKeyword> CanonicalKeywords
  {
    get
    {
      return (IEnumerable<CardKeyword>) new \u003C\u003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
    }
  }

  [SavedProperty]
  public int IncreasedBlock
  {
    get => this._increasedBlock;
    set
    {
      this.AssertMutable();
      this._increasedBlock = value;
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    Decimal num = await CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars.Block, cardPlay);
    int intValue = this.DynamicVars["Increase"].IntValue;
    this.BuffFromPlay(intValue);
    if (!(this.DeckVersion is GeneticAlgorithm deckVersion))
      return;
    deckVersion.BuffFromPlay(intValue);
  }

  protected override void OnUpgrade() => this.DynamicVars["Increase"].UpgradeValueBy(1M);

  protected override void AfterDowngraded() => this.UpdateBlock();

  private void BuffFromPlay(int extraBlock)
  {
    this.IncreasedBlock += extraBlock;
    this.UpdateBlock();
  }

  private void UpdateBlock() => this.CurrentBlock = 1 + this.IncreasedBlock;
}
