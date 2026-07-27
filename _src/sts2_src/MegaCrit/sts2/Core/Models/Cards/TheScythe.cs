// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.TheScythe
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
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

public sealed class TheScythe : CardModel
{
  private const string _increaseKey = "Increase";
  private const int _baseDamage = 13;
  private int _currentDamage = 13;
  private int _increasedDamage;

  public TheScythe()
    : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
  {
  }

  [SavedProperty]
  public int CurrentDamage
  {
    get => this._currentDamage;
    set
    {
      this.AssertMutable();
      this._currentDamage = value;
      this.DynamicVars.Damage.BaseValue = (Decimal) this._currentDamage;
    }
  }

  [SavedProperty]
  public int IncreasedDamage
  {
    get => this._increasedDamage;
    set
    {
      this.AssertMutable();
      this._increasedDamage = value;
    }
  }

  public override IEnumerable<CardKeyword> CanonicalKeywords
  {
    get
    {
      return (IEnumerable<CardKeyword>) new \u003C\u003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new DamageVar((Decimal) this.CurrentDamage, ValueProp.Move),
        (DynamicVar) new IntVar("Increase", 4M)
      });
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this, cardPlay).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
    int intValue = this.DynamicVars["Increase"].IntValue;
    this.BuffFromPlay(intValue);
    if (!(this.DeckVersion is TheScythe deckVersion))
      return;
    deckVersion.BuffFromPlay(intValue);
  }

  protected override void OnUpgrade() => this.DynamicVars["Increase"].UpgradeValueBy(1M);

  protected override void AfterDowngraded() => this.UpdateDamage();

  private void BuffFromPlay(int extraDamage)
  {
    this.IncreasedDamage += extraDamage;
    this.UpdateDamage();
  }

  private void UpdateDamage() => this.CurrentDamage = 13 + this.IncreasedDamage;
}
