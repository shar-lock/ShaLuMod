// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Uproar
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
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

public sealed class Uproar : CardModel
{
  public Uproar()
    : base(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
  {
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new DamageVar(6M, ValueProp.Move));
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this, cardPlay).WithHitCount(2).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
    CardModel card = PileType.Draw.GetPile(this.Owner).Cards.Where<CardModel>((Func<CardModel, bool>) (c => c.Type == CardType.Attack && !c.Keywords.Contains(CardKeyword.Unplayable))).ToList<CardModel>().StableShuffle<CardModel>(this.Owner.RunState.Rng.Shuffle).FirstOrDefault<CardModel>() ?? PileType.Draw.GetPile(this.Owner).Cards.Where<CardModel>((Func<CardModel, bool>) (c => c.Type == CardType.Attack)).ToList<CardModel>().StableShuffle<CardModel>(this.Owner.RunState.Rng.Shuffle).FirstOrDefault<CardModel>();
    if (card == null)
      return;
    await CardCmd.AutoPlay(choiceContext, card, (Creature) null);
  }

  protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(2M);
}
