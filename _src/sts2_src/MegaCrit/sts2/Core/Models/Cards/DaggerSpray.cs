// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.DaggerSpray
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class DaggerSpray : CardModel
{
  private const string _daggerSpraySfx = "event:/sfx/characters/silent/silent_dagger_spray";

  public DaggerSpray()
    : base(1, CardType.Attack, CardRarity.Common, TargetType.AllEnemies)
  {
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new DamageVar(4M, ValueProp.Move));
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    SfxCmd.Play("event:/sfx/characters/silent/silent_dagger_spray");
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).WithHitCount(2).FromCard((CardModel) this, cardPlay).TargetingAllOpponents(this.CombatState).WithAttackerFx((Func<Node2D>) (() => (Node2D) NDaggerSprayFlurryVfx.Create(this.Owner.Creature, new Color("#b1ccca"), true))).BeforeDamage((Func<Task>) (() =>
    {
      foreach (Creature hittableEnemy in (IEnumerable<Creature>) this.CombatState.HittableEnemies)
      {
        NDaggerSprayImpactVfx child = NDaggerSprayImpactVfx.Create(hittableEnemy, new Color("#b1ccca"), true);
        NCombatRoom instance = NCombatRoom.Instance;
        if (instance != null)
          ((Node) instance.CombatVfxContainer).AddChildSafely((Node) child);
      }
      return Task.CompletedTask;
    })).Execute(choiceContext);
  }

  protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(2M);
}
