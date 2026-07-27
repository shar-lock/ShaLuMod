// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.DaggerThrow
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class DaggerThrow : CardModel
{
  public DaggerThrow()
    : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
  {
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new DamageVar(9M, ValueProp.Move));
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this, cardPlay).Targeting(cardPlay.Target).WithAttackerFx((Func<Node2D>) (() => (Node2D) NDaggerSprayFlurryVfx.Create(this.Owner.Creature, new Color("#b1ccca"), true))).WithHitVfxNode((Func<Creature, Node2D>) (t => (Node2D) NDaggerSprayImpactVfx.Create(t, new Color("#b1ccca"), true))).Execute(choiceContext);
    IEnumerable<CardModel> cardModels = await CardPileCmd.Draw(choiceContext, 1M, this.Owner);
    CardModel card = (await CardSelectCmd.FromHandForDiscard(choiceContext, this.Owner, new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt, 1), (Func<CardModel, bool>) null, (AbstractModel) this)).FirstOrDefault<CardModel>();
    if (card == null)
      return;
    await CardCmd.Discard(choiceContext, card);
  }

  protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(3M);
}
