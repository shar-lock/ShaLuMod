// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.GunkUp
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class GunkUp : CardModel
{
  public GunkUp()
    : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
  {
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Slimed>());
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new DamageVar(4M, ValueProp.Move),
        (DynamicVar) new RepeatVar(3)
      });
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).WithHitCount(this.DynamicVars.Repeat.IntValue).FromCard((CardModel) this, cardPlay).Targeting(cardPlay.Target).WithHitFx(tmpSfx: "blunt_attack.mp3").WithHitVfxNode(GunkUp.\u003C\u003EO.\u003C0\u003E__Create ?? (GunkUp.\u003C\u003EO.\u003C0\u003E__Create = new Func<Creature, Node2D>(NGoopyImpactVfx.Create))).Execute(choiceContext);
    CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat((CardModel) this.CombatState.CreateCard<Slimed>(this.Owner), PileType.Discard, this.Owner));
    await Cmd.Wait(0.5f);
  }

  protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(1M);
}
