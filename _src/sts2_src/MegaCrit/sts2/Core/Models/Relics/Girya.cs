// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.Girya
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class Girya : RelicModel
{
  private int _timesLifted;
  public const int maxLifts = 3;

  public override RelicRarity Rarity => RelicRarity.Rare;

  public override bool IsAllowed(IRunState runState)
  {
    return RelicModel.IsBeforeAct3TreasureChest(runState);
  }

  public override bool ShowCounter => true;

  public override int DisplayAmount => this.TimesLifted;

  [SavedProperty]
  public int TimesLifted
  {
    get => this._timesLifted;
    set
    {
      this.AssertMutable();
      this._timesLifted = value;
      this.InvokeDisplayAmountChanged();
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>());
    }
  }

  public override async Task AfterRoomEntered(AbstractRoom room)
  {
    if (this.TimesLifted <= 0 || !(room is CombatRoom))
      return;
    this.Flash();
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Owner.Creature, (Decimal) this.TimesLifted, this.Owner.Creature, (CardModel) null);
  }

  public override bool TryModifyRestSiteOptions(Player player, ICollection<RestSiteOption> options)
  {
    if (player != this.Owner || this.TimesLifted >= 3)
      return false;
    options.Add((RestSiteOption) new LiftRestSiteOption(player));
    return true;
  }
}
