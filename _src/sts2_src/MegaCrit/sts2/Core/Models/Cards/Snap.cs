// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Snap
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Snap : CardModel
{
  public Snap()
    : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
  {
  }

  protected override bool ShouldGlowRedInternal => this.Owner.IsOstyMissing;

  protected override HashSet<CardTag> CanonicalTags
  {
    get => new HashSet<CardTag>() { CardTag.OstyAttack };
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Retain));
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new OstyDamageVar(7M, ValueProp.Move));
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    if (!Osty.CheckMissingWithAnim(this.Owner))
    {
      AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.OstyDamage.BaseValue).FromOsty(this.Owner.Osty, (CardModel) this, cardPlay).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3").Execute(choiceContext);
    }
    CardSelectorPrefs prefs = new CardSelectorPrefs(this.SelectionScreenPrompt, 1);
    CardModel card = (await CardSelectCmd.FromHand(choiceContext, this.Owner, prefs, (Func<CardModel, bool>) (c => !c.Keywords.Contains(CardKeyword.Retain)), (AbstractModel) this)).FirstOrDefault<CardModel>();
    if (card == null)
      return;
    CardCmd.ApplyKeyword(card, CardKeyword.Retain);
  }

  protected override void OnUpgrade() => this.DynamicVars.OstyDamage.UpgradeValueBy(3M);
}
