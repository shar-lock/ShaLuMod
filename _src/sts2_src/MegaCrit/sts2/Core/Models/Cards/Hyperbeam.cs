// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Hyperbeam
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
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Hyperbeam : CardModel
{
  public Hyperbeam()
    : base(2, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
  {
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new DamageVar(30M, ValueProp.Move),
        (DynamicVar) new PowerVar<FocusPower>(3M)
      });
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<FocusPower>());
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this, cardPlay).TargetingAllOpponents(this.CombatState).WithAttackerAnim("Cast", 0.5f).BeforeDamage((Func<Task>) (async () =>
    {
      List<Creature> enemies = this.CombatState.Enemies.Where<Creature>((Func<Creature, bool>) (e => e.IsAlive)).ToList<Creature>();
      NHyperbeamVfx child1 = NHyperbeamVfx.Create(this.Owner.Creature, enemies.Last<Creature>());
      if (child1 != null)
      {
        NCombatRoom instance = NCombatRoom.Instance;
        if (instance != null)
          ((Node) instance.CombatVfxContainer).AddChildSafely((Node) child1);
        await Cmd.Wait(0.5f);
      }
      foreach (Creature target in enemies)
      {
        NHyperbeamImpactVfx child2 = NHyperbeamImpactVfx.Create(this.Owner.Creature, target);
        if (child2 != null)
        {
          NCombatRoom instance = NCombatRoom.Instance;
          if (instance != null)
            ((Node) instance.CombatVfxContainer).AddChildSafely((Node) child2);
        }
      }
      enemies = (List<Creature>) null;
    })).Execute(choiceContext);
    FocusPower focusPower = await PowerCmd.Apply<FocusPower>(choiceContext, this.Owner.Creature, -this.DynamicVars["FocusPower"].BaseValue, this.Owner.Creature, (CardModel) this);
  }

  protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(8M);
}
