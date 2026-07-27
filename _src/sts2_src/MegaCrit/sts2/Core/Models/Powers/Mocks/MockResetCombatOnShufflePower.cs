// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.Mocks.MockResetCombatOnShufflePower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers.Mocks;

public sealed class MockResetCombatOnShufflePower : PowerModel
{
  private bool _hasReset;

  public override bool IsMock => true;

  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Single;

  private bool HasReset
  {
    get => this._hasReset;
    set
    {
      this.AssertMutable();
      this._hasReset = value;
    }
  }

  public override Task AfterCardChangedPiles(
    CardModel card,
    PileType oldPileType,
    AbstractModel? clonedBy)
  {
    if (!this.HasReset && oldPileType == PileType.Discard)
    {
      CardPile pile = card.Pile;
      if ((pile != null ? (pile.Type != PileType.Draw ? 1 : 0) : 1) == 0)
      {
        this.HasReset = true;
        CombatManager.Instance.Reset(true);
        return Task.CompletedTask;
      }
    }
    return Task.CompletedTask;
  }
}
