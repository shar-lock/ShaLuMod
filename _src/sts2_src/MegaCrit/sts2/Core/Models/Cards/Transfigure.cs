// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Transfigure
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
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

public sealed class Transfigure : CardModel
{
  public Transfigure()
    : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
  {
  }

  public override bool CanBeGeneratedInCombat => false;

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
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new EnergyVar(1));
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlyArray<IHoverTip>(new IHoverTip[2]
      {
        this.EnergyHoverTip,
        HoverTipFactory.Static(StaticHoverTip.ReplayStatic)
      });
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    IEnumerable<CardModel> cardToTransfigure = await CardSelectCmd.FromHand(choiceContext, this.Owner, new CardSelectorPrefs(this.SelectionScreenPrompt, 1), (Func<CardModel, bool>) null, (AbstractModel) this);
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
    foreach (CardModel cardModel in cardToTransfigure)
    {
      if (!cardModel.EnergyCost.CostsX && cardModel.EnergyCost.GetWithModifiers(CostModifiers.None) >= 0)
        cardModel.EnergyCost.AddThisCombat(1);
      ++cardModel.BaseReplayCount;
    }
    cardToTransfigure = (IEnumerable<CardModel>) null;
  }

  protected override void OnUpgrade() => this.RemoveKeyword(CardKeyword.Exhaust);
}
