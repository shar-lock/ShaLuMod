// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Debt
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Debt : CardModel
{
  public Debt()
    : base(-1, CardType.Curse, CardRarity.Curse, TargetType.None)
  {
  }

  public override int MaxUpgradeLevel => 0;

  public override IEnumerable<CardKeyword> CanonicalKeywords
  {
    get
    {
      return (IEnumerable<CardKeyword>) new \u003C\u003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Unplayable);
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new GoldVar(10));
    }
  }

  public override bool HasTurnEndInHandEffect => true;

  protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
  {
    await PlayerCmd.LoseGold((Decimal) Mathf.Min(this.DynamicVars.Gold.IntValue, this.Owner.Gold), this.Owner);
  }
}
