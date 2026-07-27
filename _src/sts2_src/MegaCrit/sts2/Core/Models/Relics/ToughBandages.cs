// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.ToughBandages
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class ToughBandages : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Rare;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new BlockVar(3M, ValueProp.Unpowered));
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block));
    }
  }

  public override async Task AfterCardDiscarded(PlayerChoiceContext choiceContext, CardModel card)
  {
    if (card.Owner != this.Owner || this.Owner.Creature.Side != this.Owner.Creature.CombatState.CurrentSide)
      return;
    this.Flash();
    Decimal num = await CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars.Block, (CardPlay) null);
  }
}
