// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Quadcast
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Quadcast : CardModel
{
  public Quadcast()
    : base(1, CardType.Skill, CardRarity.Ancient, TargetType.Self)
  {
  }

  public override OrbEvokeType OrbEvokeType => OrbEvokeType.Front;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new RepeatVar(4));
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (this.Owner.PlayerCombatState.OrbQueue.Orbs.Count <= 0)
      return;
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
    for (int i = 0; i < this.DynamicVars.Repeat.IntValue; ++i)
    {
      await OrbCmd.EvokeNext(choiceContext, this.Owner, i == this.DynamicVars.Repeat.IntValue - 1);
      if (i != this.DynamicVars.Repeat.IntValue - 1)
        await Cmd.CustomScaledWait(0.15f, 0.25f);
    }
  }

  protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
}
