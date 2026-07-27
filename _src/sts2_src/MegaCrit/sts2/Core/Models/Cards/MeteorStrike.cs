// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.MeteorStrike
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
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class MeteorStrike : CardModel
{
  public MeteorStrike()
    : base(5, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
  {
  }

  protected override HashSet<CardTag> CanonicalTags
  {
    get => new HashSet<CardTag>() { CardTag.Strike };
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new DamageVar(24M, ValueProp.Move));
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlyArray<IHoverTip>(new IHoverTip[2]
      {
        HoverTipFactory.Static(StaticHoverTip.Channeling),
        HoverTipFactory.FromOrb<PlasmaOrb>()
      });
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    NCreature creatureNode = NCombatRoom.Instance?.GetCreatureNode(cardPlay.Target);
    if (creatureNode != null)
    {
      NLargeMagicMissileVfx child = NLargeMagicMissileVfx.Create(creatureNode.GetBottomOfHitbox(), new Color("50b598"));
      ((Node) NCombatRoom.Instance.CombatVfxContainer).AddChildSafely((Node) child);
      await Cmd.Wait(child.WaitTime);
    }
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this, cardPlay).Targeting(cardPlay.Target).WithHitFx(tmpSfx: "blunt_attack.mp3").Execute(choiceContext);
    for (int i = 0; i < 3; ++i)
      await OrbCmd.Channel<PlasmaOrb>(choiceContext, this.Owner);
  }

  protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(6M);
}
