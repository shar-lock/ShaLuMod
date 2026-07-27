// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.TheBomb
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

public sealed class TheBomb : CardModel
{
  private const string _turnsKey = "Turns";
  private const string _bombDamageKey = "BombDamage";

  public TheBomb()
    : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
  {
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        new DynamicVar("Turns", 3M),
        new DynamicVar("BombDamage", 40M)
      });
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    (await PowerCmd.Apply<TheBombPower>(choiceContext, this.Owner.Creature, this.DynamicVars["Turns"].BaseValue, this.Owner.Creature, (CardModel) this)).SetDamage(this.DynamicVars["BombDamage"].BaseValue);
  }

  protected override void OnUpgrade() => this.DynamicVars["BombDamage"].UpgradeValueBy(10M);
}
