// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.DualWield
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class DualWield : CardModel
{
  public DualWield()
    : base(1, CardType.Skill, CardRarity.Event, TargetType.Self)
  {
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new CardsVar(1));
    }
  }

  public override CardPoolModel VisualCardPool
  {
    get => (CardPoolModel) ModelDb.CardPool<IroncladCardPool>();
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    CardSelectorPrefs prefs = new CardSelectorPrefs(this.SelectionScreenPrompt, 1);
    CardModel selection = (await CardSelectCmd.FromHand(choiceContext, this.Owner, prefs, (Func<CardModel, bool>) (c =>
    {
      bool flag;
      switch (c.Type)
      {
        case CardType.Attack:
        case CardType.Power:
          flag = true;
          break;
        default:
          flag = false;
          break;
      }
      return flag;
    }), (AbstractModel) this)).FirstOrDefault<CardModel>();
    if (selection == null)
    {
      selection = (CardModel) null;
    }
    else
    {
      for (int i = 0; i < this.DynamicVars.Cards.IntValue; ++i)
      {
        CardPileAddResult combat = await CardPileCmd.AddGeneratedCardToCombat(selection.CreateClone(), PileType.Hand, this.Owner);
      }
      selection = (CardModel) null;
    }
  }

  protected override void OnUpgrade() => this.DynamicVars.Cards.UpgradeValueBy(1M);
}
