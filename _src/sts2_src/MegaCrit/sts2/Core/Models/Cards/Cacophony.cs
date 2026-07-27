// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Cacophony
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Cacophony : CardModel
{
  public const int cardAmount = 33;

  public Cacophony()
    : base(2, CardType.Power, CardRarity.Rare, TargetType.Self)
  {
  }

  public override CardMultiplayerConstraint MultiplayerConstraint
  {
    get => CardMultiplayerConstraint.MultiplayerOnly;
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new CardsVar(33),
        (DynamicVar) new DamageVar(66M, ValueProp.Unpowered)
      });
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    CacophonyPower cacophonyPower = await PowerCmd.Apply<CacophonyPower>(choiceContext, this.Owner.Creature, (Decimal) this.DynamicVars.Damage.IntValue, this.Owner.Creature, (CardModel) this);
  }

  protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(33M);
}
