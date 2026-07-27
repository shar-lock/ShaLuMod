// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.EscapePlan
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class EscapePlan : CardModel
{
  public EscapePlan()
    : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
  {
  }

  public override bool GainsBlock => true;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new BlockVar(3M, ValueProp.Move));
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    CardModel cardModel = (await CardPileCmd.Draw(choiceContext, 1M, this.Owner)).FirstOrDefault<CardModel>();
    if (cardModel == null || cardModel.Type != CardType.Skill)
      return;
    Decimal num = await CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars.Block, cardPlay);
  }

  protected override void OnUpgrade() => this.DynamicVars.Block.UpgradeValueBy(2M);
}
