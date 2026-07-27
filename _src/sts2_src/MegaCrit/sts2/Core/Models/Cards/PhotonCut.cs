// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.PhotonCut
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class PhotonCut : CardModel
{
  private const string _putBackKey = "PutBack";

  public PhotonCut()
    : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
  {
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[3]
      {
        (DynamicVar) new DamageVar(10M, ValueProp.Move),
        (DynamicVar) new CardsVar(1),
        new DynamicVar("PutBack", 1M)
      });
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this, cardPlay).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash", tmpSfx: "dagger_throw.mp3").Execute(choiceContext);
    IEnumerable<CardModel> cardModels = await CardPileCmd.Draw(choiceContext, this.DynamicVars.Cards.BaseValue, this.Owner);
    CardSelectorPrefs prefs = new CardSelectorPrefs(this.SelectionScreenPrompt, this.DynamicVars["PutBack"].IntValue);
    IReadOnlyList<CardPileAddResult> cardPileAddResultList = await CardPileCmd.Add(await CardSelectCmd.FromHand(choiceContext, this.Owner, prefs, (Func<CardModel, bool>) null, (AbstractModel) this), PileType.Draw, CardPilePosition.Top);
  }

  protected override void OnUpgrade()
  {
    this.DynamicVars.Damage.UpgradeValueBy(3M);
    this.DynamicVars.Cards.UpgradeValueBy(1M);
  }
}
