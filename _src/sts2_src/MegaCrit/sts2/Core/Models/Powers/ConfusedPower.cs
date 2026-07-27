// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.ConfusedPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.TestSupport;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class ConfusedPower : PowerModel
{
  private int _testEnergyCostOverride = -1;

  public override PowerType Type => PowerType.Debuff;

  public override PowerStackType StackType => PowerStackType.Single;

  public int TestEnergyCostOverride
  {
    private get => this._testEnergyCostOverride;
    set
    {
      TestMode.AssertOn();
      this.AssertMutable();
      this._testEnergyCostOverride = value;
    }
  }

  public override Task AfterCardDrawn(
    PlayerChoiceContext choiceContext,
    CardModel card,
    bool fromHandDraw)
  {
    if (card.Owner != this.Owner.Player || card.EnergyCost.Canonical < 0)
      return Task.CompletedTask;
    int cost = this.NextEnergyCost();
    card.EnergyCost.SetThisCombat(cost);
    NCard.FindOnTable(card)?.PlayRandomizeCostAnim();
    return Task.CompletedTask;
  }

  private int NextEnergyCost()
  {
    return this.TestEnergyCostOverride >= 0 ? this.TestEnergyCostOverride : this.Owner.Player.RunState.Rng.CombatEnergyCosts.NextInt(4);
  }
}
