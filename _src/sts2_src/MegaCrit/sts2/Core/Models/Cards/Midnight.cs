// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Midnight
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
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public class Midnight : CardModel
{
  public Midnight()
    : base(12, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
  {
  }

  public override CardMultiplayerConstraint MultiplayerConstraint
  {
    get => CardMultiplayerConstraint.MultiplayerOnly;
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new DamageVar(60M, ValueProp.Move));
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this, cardPlay).Targeting(cardPlay.Target).WithAttackerAnim(Ironclad.GetHeavyAnimIfApplicable(this.Owner.Character), Ironclad.GetHeavyAttackDelayIfApplicable(this.Owner.Character)).WithHitFx("vfx/vfx_heavy_blunt", tmpSfx: "heavy_attack.mp3").WithHitVfxSpawnedAtBase().Execute(choiceContext);
  }

  public override Task AfterCardEnteredCombat(CardModel card)
  {
    if (card != this || this.IsClone)
      return Task.CompletedTask;
    this.ReduceCostBy(CombatManager.Instance.History.Entries.OfType<CardExhaustedEntry>().Count<CardExhaustedEntry>());
    return Task.CompletedTask;
  }

  public override Task AfterCardExhausted(
    PlayerChoiceContext choiceContext,
    CardModel card,
    bool causedByEthereal)
  {
    this.ReduceCostBy(1);
    return Task.CompletedTask;
  }

  private void ReduceCostBy(int amount) => this.EnergyCost.AddThisCombat(-amount);

  protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(12M);
}
