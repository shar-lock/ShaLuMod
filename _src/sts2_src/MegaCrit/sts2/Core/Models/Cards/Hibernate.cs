// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Hibernate
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Powers;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Hibernate : CardModel
{
  public Hibernate()
    : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
  {
  }

  public override CardMultiplayerConstraint MultiplayerConstraint
  {
    get => CardMultiplayerConstraint.MultiplayerOnly;
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlyArray<IHoverTip>(new IHoverTip[3]
      {
        HoverTipFactory.Static(StaticHoverTip.Channeling),
        HoverTipFactory.FromOrb<FrostOrb>(),
        HoverTipFactory.Static(StaticHoverTip.Block)
      });
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new RepeatVar(2));
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    HibernatePower hibernatePower = await PowerCmd.Apply<HibernatePower>(choiceContext, this.Owner.Creature, 1M, this.Owner.Creature, cardPlay.Card);
    for (int i = 0; i < this.DynamicVars.Repeat.IntValue; ++i)
      await OrbCmd.Channel<FrostOrb>(choiceContext, this.Owner);
  }

  protected override void OnUpgrade() => this.DynamicVars.Repeat.UpgradeValueBy(1M);
}
