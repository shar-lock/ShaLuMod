// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.BubbleBubble
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class BubbleBubble : CardModel
{
  public BubbleBubble()
    : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
  {
  }

  protected override bool ShouldGlowGoldInternal
  {
    get
    {
      ICombatState combatState = this.CombatState;
      return combatState != null && combatState.HittableEnemies.Any<Creature>((Func<Creature, bool>) (e => e.HasPower<PoisonPower>()));
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<PoisonPower>());
    }
  }

  protected override IEnumerable<string> ExtraRunAssetPaths => NSmokePuffVfx.AssetPaths;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new PowerVar<PoisonPower>(9M));
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    NCreature creatureNode = NCombatRoom.Instance?.GetCreatureNode(cardPlay.Target);
    if (creatureNode != null)
      ((Node) NCombatRoom.Instance.CombatVfxContainer).AddChildSafely((Node) NGaseousImpactVfx.Create(creatureNode.VfxSpawnPosition, new Color("83eb85")));
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
    if (!cardPlay.Target.HasPower<PoisonPower>())
      return;
    PoisonPower poisonPower = await PowerCmd.Apply<PoisonPower>(choiceContext, cardPlay.Target, this.DynamicVars.Poison.BaseValue, this.Owner.Creature, (CardModel) this);
  }

  protected override void OnUpgrade() => this.DynamicVars.Poison.UpgradeValueBy(3M);
}
