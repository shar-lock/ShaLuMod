// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.DeadlyPoison
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class DeadlyPoison : CardModel
{
  public DeadlyPoison()
    : base(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
  {
  }

  protected override IEnumerable<string> ExtraRunAssetPaths => NSmokePuffVfx.AssetPaths;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new PowerVar<PoisonPower>(5M));
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<PoisonPower>());
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    NPoisonImpactVfx child = NPoisonImpactVfx.Create(cardPlay.Target);
    NCombatRoom instance = NCombatRoom.Instance;
    if (instance != null)
      ((Node) instance.CombatVfxContainer).AddChildSafely((Node) child);
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
    PoisonPower poisonPower = await PowerCmd.Apply<PoisonPower>(choiceContext, cardPlay.Target, this.DynamicVars.Poison.BaseValue, this.Owner.Creature, (CardModel) this);
  }

  protected override void OnUpgrade() => this.DynamicVars.Poison.UpgradeValueBy(2M);
}
