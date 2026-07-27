// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.CosmicIndifference
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class CosmicIndifference : CardModel
{
  public CosmicIndifference()
    : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
  {
  }

  public override bool GainsBlock => true;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new BlockVar(6M, ValueProp.Move));
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    Decimal num = await CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars.Block, cardPlay);
    CardSelectorPrefs prefs = new CardSelectorPrefs(this.SelectionScreenPrompt, 1);
    PileType.Discard.GetPile(this.Owner);
    CardModel card = (await CardSelectCmd.FromCombatPile(choiceContext, PileType.Discard.GetPile(this.Owner), this.Owner, prefs)).FirstOrDefault<CardModel>();
    bool flag1 = card != null;
    if (flag1)
    {
      PileType? type = card.Pile?.Type;
      bool flag2;
      if (type.HasValue)
      {
        switch (type.GetValueOrDefault())
        {
          case PileType.Draw:
          case PileType.Discard:
            flag2 = true;
            goto label_7;
        }
      }
      flag2 = false;
label_7:
      flag1 = flag2;
    }
    if (!flag1)
      return;
    CardPileAddResult cardPileAddResult = await CardPileCmd.Add(card, PileType.Draw, CardPilePosition.Top);
  }

  protected override void OnUpgrade() => this.DynamicVars.Block.UpgradeValueBy(3M);
}
