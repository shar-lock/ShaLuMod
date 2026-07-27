// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Dualcast
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Dualcast : CardModel
{
  public Dualcast()
    : base(1, CardType.Skill, CardRarity.Basic, TargetType.Self)
  {
  }

  public override OrbEvokeType OrbEvokeType => OrbEvokeType.Front;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Evoke));
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (this.Owner.PlayerCombatState.OrbQueue.Orbs.Count <= 0)
      return;
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
    await OrbCmd.EvokeNext(choiceContext, this.Owner, false);
    await Cmd.CustomScaledWait(0.1f, 0.25f);
    await OrbCmd.EvokeNext(choiceContext, this.Owner);
  }

  protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
}
