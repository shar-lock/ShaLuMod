// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Enchantments.Adroit
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
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Enchantments;

public sealed class Adroit : EnchantmentModel
{
  public override bool HasExtraCardText => true;

  public override bool ShowAmount => true;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new BlockVar(0M, ValueProp.Move));
    }
  }

  public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay? cardPlay)
  {
    Decimal num = await CreatureCmd.GainBlock(this.Card.Owner.Creature, this.DynamicVars.Block, cardPlay);
  }

  public override void RecalculateValues()
  {
    this.DynamicVars.Block.BaseValue = (Decimal) this.Amount;
  }
}
