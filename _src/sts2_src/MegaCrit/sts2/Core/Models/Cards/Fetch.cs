// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Fetch
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Fetch : CardModel
{
  public Fetch()
    : base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
  {
  }

  protected override HashSet<CardTag> CanonicalTags
  {
    get => new HashSet<CardTag>() { CardTag.OstyAttack };
  }

  protected override bool ShouldGlowRedInternal => this.Owner.IsOstyMissing;

  protected override bool ShouldGlowGoldInternal => !this.HasBeenPlayedThisTurn;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new OstyDamageVar(3M, ValueProp.Move),
        (DynamicVar) new CardsVar(1)
      });
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    if (Osty.CheckMissingWithAnim(this.Owner))
      return;
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.OstyDamage.BaseValue).FromOsty(this.Owner.Osty, (CardModel) this, cardPlay).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3").Execute(choiceContext);
    if (this.HasBeenPlayedThisTurn)
      return;
    IEnumerable<CardModel> cardModels = await CardPileCmd.Draw(choiceContext, this.DynamicVars.Cards.BaseValue, this.Owner);
  }

  protected override void OnUpgrade() => this.DynamicVars.OstyDamage.UpgradeValueBy(3M);

  private bool HasBeenPlayedThisTurn
  {
    get
    {
      return CombatManager.Instance.History.CardPlaysFinished.Any<CardPlayFinishedEntry>((Func<CardPlayFinishedEntry, bool>) (e => e.CardPlay.Card == this && e.HappenedThisTurn(this.CombatState)));
    }
  }
}
