// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.GraveWarden
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class GraveWarden : CardModel
{
  public GraveWarden()
    : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
  {
  }

  public override bool GainsBlock => true;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new BlockVar(8M, ValueProp.Move),
        (DynamicVar) new CardsVar(1)
      });
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Soul>());
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
    Decimal num = await CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars.Block, cardPlay);
    CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardsToCombat((IEnumerable<CardModel>) Soul.Create(this.Owner, this.DynamicVars.Cards.IntValue, this.CombatState), PileType.Draw, this.Owner, CardPilePosition.Random));
  }

  protected override void OnUpgrade() => this.DynamicVars.Block.UpgradeValueBy(3M);
}
