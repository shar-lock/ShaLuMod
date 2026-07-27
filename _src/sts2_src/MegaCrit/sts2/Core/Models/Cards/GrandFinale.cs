// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.GrandFinale
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

public sealed class GrandFinale : CardModel
{
  public GrandFinale()
    : base(0, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
  {
  }

  protected override bool ShouldGlowGoldInternal => this.IsPlayable;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new DamageVar(60M, ValueProp.Move));
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    NGrandFinaleVfx child = NGrandFinaleVfx.Create(this.Owner.Creature);
    if (child != null)
    {
      NCombatRoom instance = NCombatRoom.Instance;
      if (instance != null)
        ((Node) instance.CombatVfxContainer).AddChildSafely((Node) child);
      await Cmd.Wait(NGrandFinaleVfx.totalAnticipationDuration);
    }
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this, cardPlay).TargetingAllOpponents(this.CombatState).WithHitVfxNode(GrandFinale.\u003C\u003EO.\u003C0\u003E__Create ?? (GrandFinale.\u003C\u003EO.\u003C0\u003E__Create = new Func<Creature, Node2D>(NGrandFinaleImpactVfx.Create))).WithHitFx(tmpSfx: "blunt_attack.mp3").Execute(choiceContext);
  }

  protected override bool IsPlayable => PileType.Draw.GetPile(this.Owner).Cards.Count == 0;

  protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(15M);
}
