// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.WasteAway
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Models.Powers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class WasteAway : CardModel, KnowledgeDemon.IChoosable
{
  public WasteAway()
    : base(-1, CardType.Status, CardRarity.Status, TargetType.None)
  {
  }

  public override bool CanBeGeneratedInCombat => false;

  public override int MaxUpgradeLevel => 0;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(this.EnergyHoverTip);
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new PowerVar<WasteAwayPower>(1M));
    }
  }

  public async Task OnChosen()
  {
    WasteAwayPower wasteAwayPower = await PowerCmd.Apply<WasteAwayPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Owner.Creature, (Decimal) this.DynamicVars["WasteAwayPower"].IntValue, this.Owner.Creature, (CardModel) this);
  }
}
