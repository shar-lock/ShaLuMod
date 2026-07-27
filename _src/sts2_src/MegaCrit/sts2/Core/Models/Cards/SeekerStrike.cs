// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.SeekerStrike
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class SeekerStrike : CardModel
{
  public SeekerStrike()
    : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
  {
  }

  protected override HashSet<CardTag> CanonicalTags
  {
    get => new HashSet<CardTag>() { CardTag.Strike };
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new DamageVar(9M, ValueProp.Move),
        (DynamicVar) new CardsVar(3)
      });
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this, cardPlay).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
    IEnumerable<CardModel> cardOptions = PileType.Draw.GetPile(this.Owner).Cards.ToList<CardModel>().StableShuffle<CardModel>(this.Owner.RunState.Rng.CombatCardSelection).Take<CardModel>(this.DynamicVars.Cards.IntValue);
    CardModel card = (await CardSelectCmd.FromCombatPile(choiceContext, PileType.Draw.GetPile(this.Owner), this.Owner, new CardSelectorPrefs(this.SelectionScreenPrompt, 1), (Func<CardModel, bool>) (c => cardOptions.Contains<CardModel>(c)))).FirstOrDefault<CardModel>();
    if (card == null)
      ;
    else
    {
      CardPileAddResult cardPileAddResult = await CardPileCmd.Add(card, PileType.Hand);
    }
  }

  protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(3M);
}
