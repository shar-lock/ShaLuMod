// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Flatten
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

public sealed class Flatten : CardModel
{
  public Flatten()
    : base(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
  {
  }

  protected override bool ShouldGlowGoldInternal => this.HasOstyAttackedThisTurn;

  protected override bool ShouldGlowRedInternal => this.Owner.IsOstyMissing;

  protected override HashSet<CardTag> CanonicalTags
  {
    get => new HashSet<CardTag>() { CardTag.OstyAttack };
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new OstyDamageVar(12M, ValueProp.Move));
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    if (Osty.CheckMissingWithAnim(this.Owner))
      return;
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.OstyDamage.BaseValue).FromOsty(this.Owner.Osty, (CardModel) this, cardPlay).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3").Execute(choiceContext);
  }

  protected override void OnUpgrade() => this.DynamicVars.OstyDamage.UpgradeValueBy(4M);

  public override Task AfterCardEnteredCombat(CardModel card)
  {
    if (card != this || !this.HasOstyAttackedThisTurn)
      return Task.CompletedTask;
    this.ReduceCost();
    return Task.CompletedTask;
  }

  public override Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
  {
    if (command.Attacker == null || command.Attacker != this.Owner.Osty)
      return Task.CompletedTask;
    this.ReduceCost();
    return Task.CompletedTask;
  }

  private void ReduceCost() => this.EnergyCost.SetThisTurn(0);

  private bool HasOstyAttackedThisTurn
  {
    get
    {
      return CombatManager.Instance.History.Entries.OfType<CreatureAttackedEntry>().Any<CreatureAttackedEntry>((Func<CreatureAttackedEntry, bool>) (e => e.Actor == this.Owner.Osty && e.HappenedThisTurn(this.CombatState)));
    }
  }
}
