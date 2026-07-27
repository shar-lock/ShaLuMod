// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Spite
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
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

public sealed class Spite : CardModel
{
  public Spite()
    : base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
  {
  }

  protected override bool ShouldGlowGoldInternal => Spite.LostHpThisTurn(this.Owner.Creature);

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new DamageVar(5M, ValueProp.Move),
        (DynamicVar) new RepeatVar(2)
      });
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    int hitCount = Spite.LostHpThisTurn(this.Owner.Creature) ? this.DynamicVars.Repeat.IntValue : 1;
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).WithHitCount(hitCount).FromCard((CardModel) this, cardPlay).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
  }

  protected override void OnUpgrade() => this.DynamicVars.Repeat.UpgradeValueBy(1M);

  private static bool LostHpThisTurn(Creature creature)
  {
    return CombatManager.Instance.History.Entries.OfType<DamageReceivedEntry>().Any<DamageReceivedEntry>((Func<DamageReceivedEntry, bool>) (e => e.HappenedThisTurn(creature.CombatState) && e.Receiver == creature && e.Result.UnblockedDamage > 0));
  }
}
