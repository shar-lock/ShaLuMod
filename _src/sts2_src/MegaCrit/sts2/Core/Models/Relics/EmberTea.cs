// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.EmberTea
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class EmberTea : RelicModel
{
  private const string _combatsKey = "Combats";
  private int _combatsLeft = 5;

  public override RelicRarity Rarity => RelicRarity.Event;

  public override bool IsUsedUp => this.CombatsLeft <= 0;

  public override bool ShowCounter => true;

  public override int DisplayAmount => Math.Max(0, this.CombatsLeft);

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        new DynamicVar("Combats", (Decimal) this.CombatsLeft),
        (DynamicVar) new PowerVar<StrengthPower>(2M)
      });
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>());
    }
  }

  [SavedProperty]
  public int CombatsLeft
  {
    get => this._combatsLeft;
    set
    {
      this.AssertMutable();
      this._combatsLeft = value;
      this.DynamicVars["Combats"].BaseValue = (Decimal) this._combatsLeft;
      this.InvokeDisplayAmountChanged();
      if (!this.IsUsedUp)
        return;
      this.Status = RelicStatus.Disabled;
    }
  }

  public override async Task AfterRoomEntered(AbstractRoom room)
  {
    if (this.IsUsedUp || !(room is CombatRoom))
      return;
    this.Flash();
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Owner.Creature, this.DynamicVars.Strength.BaseValue, (Creature) null, (CardModel) null);
    this.CombatsLeft--;
  }
}
