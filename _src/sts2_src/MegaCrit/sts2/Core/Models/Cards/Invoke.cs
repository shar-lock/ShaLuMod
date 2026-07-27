// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Invoke
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Powers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Invoke : CardModel
{
  public Invoke()
    : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
  {
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlyArray<IHoverTip>(new IHoverTip[2]
      {
        HoverTipFactory.Static(StaticHoverTip.SummonDynamic, (DynamicVar) this.DynamicVars.Summon),
        this.EnergyHoverTip
      });
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new SummonVar(2M),
        (DynamicVar) new EnergyVar(2)
      });
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    await CreatureCmd.TriggerAnim(this.Owner.Creature, Necrobinder.GetSummonAnimIfApplicable(this.Owner.Character), Necrobinder.GetSummonDelayIfApplicable(this.Owner.Character));
    SummonNextTurnPower summonNextTurnPower = await PowerCmd.Apply<SummonNextTurnPower>(choiceContext, this.Owner.Creature, (Decimal) this.DynamicVars.Summon.IntValue, this.Owner.Creature, (CardModel) this);
    EnergyNextTurnPower energyNextTurnPower = await PowerCmd.Apply<EnergyNextTurnPower>(choiceContext, this.Owner.Creature, (Decimal) this.DynamicVars.Energy.IntValue, this.Owner.Creature, (CardModel) this);
  }

  protected override void OnUpgrade()
  {
    this.DynamicVars.Summon.UpgradeValueBy(1M);
    this.DynamicVars.Energy.UpgradeValueBy(1M);
  }
}
