// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.BrightestFlame
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class BrightestFlame : CardModel
{
  public BrightestFlame()
    : base(0, CardType.Skill, CardRarity.Ancient, TargetType.Self)
  {
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[3]
      {
        (DynamicVar) new MaxHpVar(1M),
        (DynamicVar) new EnergyVar(2),
        (DynamicVar) new CardsVar(2)
      });
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(this.EnergyHoverTip);
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    await PlayerCmd.GainEnergy((Decimal) this.DynamicVars.Energy.IntValue, this.Owner);
    IEnumerable<CardModel> cardModels = await CardPileCmd.Draw(choiceContext, this.DynamicVars.Cards.BaseValue, this.Owner);
    await CreatureCmd.LoseMaxHp(choiceContext, this.Owner.Creature, this.DynamicVars.MaxHp.BaseValue, true);
  }

  protected override void OnUpgrade()
  {
    this.DynamicVars.Energy.UpgradeValueBy(1M);
    this.DynamicVars.Cards.UpgradeValueBy(1M);
  }
}
