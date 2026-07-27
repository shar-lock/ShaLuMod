// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.HeirloomHammer
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
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class HeirloomHammer : CardModel
{
  public HeirloomHammer()
    : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
  {
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new DamageVar(20M, ValueProp.Move),
        (DynamicVar) new RepeatVar(1)
      });
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this, cardPlay).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3").Execute(choiceContext);
    CardSelectorPrefs prefs = new CardSelectorPrefs(this.SelectionScreenPrompt, 1);
    CardModel selection = (await CardSelectCmd.FromHand(choiceContext, this.Owner, prefs, (Func<CardModel, bool>) (c => c.VisualCardPool.IsColorless), (AbstractModel) this)).FirstOrDefault<CardModel>();
    if (selection == null)
    {
      selection = (CardModel) null;
    }
    else
    {
      for (int i = 0; i < this.DynamicVars.Repeat.IntValue; ++i)
      {
        CardPileAddResult combat = await CardPileCmd.AddGeneratedCardToCombat(selection.CreateClone(), PileType.Hand, this.Owner);
      }
      selection = (CardModel) null;
    }
  }

  protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(5M);
}
