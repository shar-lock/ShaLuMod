// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Stampede
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Stampede : CardModel
{
  private const string _powerKey = "Power";

  public Stampede()
    : base(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
  {
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Power", 1M));
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    StampedePower stampedePower = await PowerCmd.Apply<StampedePower>(choiceContext, this.Owner.Creature, this.DynamicVars["Power"].BaseValue, this.Owner.Creature, (CardModel) this);
  }

  protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
}
