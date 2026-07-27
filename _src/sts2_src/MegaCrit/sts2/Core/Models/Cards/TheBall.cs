// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.TheBall
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class TheBall : CardModel
{
  private const string _increaseKey = "Increase";
  private Decimal _extraDamageFromPlays;

  public TheBall()
    : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
  {
  }

  public override CardMultiplayerConstraint MultiplayerConstraint
  {
    get => CardMultiplayerConstraint.MultiplayerOnly;
  }

  private Decimal ExtraDamageFromPlays
  {
    get => this._extraDamageFromPlays;
    set
    {
      this.AssertMutable();
      this._extraDamageFromPlays = value;
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new DamageVar(10M, ValueProp.Move),
        new DynamicVar("Increase", 10M)
      });
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this, cardPlay).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
    DamageVar damage = this.DynamicVars.Damage;
    damage.BaseValue = damage.BaseValue + this.DynamicVars["Increase"].BaseValue;
    this.ExtraDamageFromPlays += this.DynamicVars["Increase"].BaseValue;
  }

  protected override CardLocation GetResultLocationForCardPlay()
  {
    CardLocation locationForCardPlay = base.GetResultLocationForCardPlay();
    if (this.CombatState == null)
      return locationForCardPlay;
    List<Creature> list = this.CombatState.GetTeammatesOf(this.Owner.Creature).Where<Creature>((Func<Creature, bool>) (c => c != null && c.IsAlive && c.IsPlayer && c.Player != this.Owner)).ToList<Creature>();
    if (list.Count == 0)
      return locationForCardPlay;
    locationForCardPlay.player = this.Owner.RunState.Rng.CombatTargets.NextItem<Creature>((IEnumerable<Creature>) list).Player;
    if (locationForCardPlay.pileType == PileType.Discard)
    {
      locationForCardPlay.pileType = PileType.Draw;
      locationForCardPlay.position = CardPilePosition.Random;
    }
    return locationForCardPlay;
  }

  protected override void AfterDowngraded()
  {
    base.AfterDowngraded();
    DamageVar damage = this.DynamicVars.Damage;
    damage.BaseValue = damage.BaseValue + this.ExtraDamageFromPlays;
  }

  protected override void OnUpgrade() => this.DynamicVars["Increase"].UpgradeValueBy(5M);
}
