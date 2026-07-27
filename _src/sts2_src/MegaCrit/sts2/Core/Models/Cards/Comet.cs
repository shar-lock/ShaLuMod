// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Comet
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Comet : CardModel
{
  public Comet()
    : base(0, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
  {
  }

  public override int CanonicalStarCost => 5;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[3]
      {
        (DynamicVar) new DamageVar(33M, ValueProp.Move),
        (DynamicVar) new PowerVar<VulnerablePower>(3M),
        (DynamicVar) new PowerVar<WeakPower>(3M)
      });
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlyArray<IHoverTip>(new IHoverTip[2]
      {
        HoverTipFactory.FromPower<WeakPower>(),
        HoverTipFactory.FromPower<VulnerablePower>()
      });
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
    NCreature creatureNode = NCombatRoom.Instance?.GetCreatureNode(cardPlay.Target);
    if (creatureNode != null)
    {
      NSmallMagicMissileVfx child = NSmallMagicMissileVfx.Create(creatureNode.GetBottomOfHitbox(), new Color("50b598"));
      ((Node) NCombatRoom.Instance.CombatVfxContainer).AddChildSafely((Node) child);
      await Cmd.Wait(child.WaitTime);
    }
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this, cardPlay).Targeting(cardPlay.Target).WithNoAttackerAnim().Execute(choiceContext);
    WeakPower weakPower = await PowerCmd.Apply<WeakPower>(choiceContext, cardPlay.Target, this.DynamicVars.Weak.BaseValue, this.Owner.Creature, (CardModel) this);
    VulnerablePower vulnerablePower = await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target, this.DynamicVars.Vulnerable.BaseValue, this.Owner.Creature, (CardModel) this);
  }

  protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(11M);
}
