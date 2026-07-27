// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Pyre
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Pyre : CardModel
{
  public Pyre()
    : base(2, CardType.Power, CardRarity.Rare, TargetType.Self)
  {
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new EnergyVar(1));
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(this.EnergyHoverTip);
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    PyrePower pyrePower = await PowerCmd.Apply<PyrePower>(choiceContext, this.Owner.Creature, this.DynamicVars.Energy.BaseValue, this.Owner.Creature, (CardModel) this);
  }

  public override async Task OnEnqueuePlayVfx(Creature? target)
  {
    NFireBurningVfx child = NFireBurningVfx.Create(this.Owner.Creature, 1f, false);
    NCombatRoom instance = NCombatRoom.Instance;
    if (instance != null)
      ((Node) instance.CombatVfxContainer).AddChildSafely((Node) child);
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "PowerUp", this.Owner.Character.PowerUpAnimDelay);
  }

  protected override void OnUpgrade() => this.DynamicVars.Energy.UpgradeValueBy(1M);
}
